using Esoteric.Finance.Abstractions.Entities.Dbo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Esoteric.Finance.Data.Tools
{
    public sealed class GeneralLogger : ILogger
    {
        private readonly string _category;
        private readonly Func<LoggerFilterOptions> _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentBag<GeneralLoggerScope> _scopes = new();

        public GeneralLogger(string name, Func<LoggerFilterOptions> settings, IServiceProvider serviceProvider)
        {
            _category = name;
            _settings = settings;
            _serviceProvider = serviceProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            var scope = GeneralLoggerScope.Create(state, this);

            _scopes.Add(scope);

            return scope;
        }

        public bool IsEnabled(LogLevel logLevel) => 
            _settings().MinLevel <= logLevel &&
            _settings().Rules.FirstOrDefault(x => x.ProviderName == GeneralLoggerProvider.Name)?.LogLevel <= logLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) { return; }

            using var scope = _serviceProvider.CreateScope();

            using var context = scope.ServiceProvider.GetRequiredService<EsotericFinanceContext>();

            try
            {
                context.Add(new GeneralLog
                {
                    EventCode = eventId.Id,
                    LevelCode = (int)logLevel,
                    Category = _category,
                    Scope = JsonSerializer.Serialize(GeneralLoggerScope.Combine(_scopes)),
                    Message = formatter(state, exception),
                    Exception = exception?.ToString()
                });

                context.SaveChanges();
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }

    public sealed class GeneralLoggerScope : IDisposable
    {
        private static string UpdateScope(string original, string update) => string.IsNullOrWhiteSpace(update) ? original : update;

        public GeneralLoggerScope(IDictionary<string,string>? scopes)
        {
            foreach (var scope in scopes ?? new Dictionary<string, string>())
            {
                Scopes.AddOrUpdate(scope.Key, scope.Value, UpdateScope);
            }
        }
        public static GeneralLoggerScope Create<TState>(TState state, ILogger logger)
        {
            if (state is IDictionary<string, string> scopes)
            {
                return new(scopes);
            }
            else if (state != null)
            {
                try
                {
                    var scopeAsJson = JsonSerializer.Serialize(state);
                    try
                    {
                        return new(JsonSerializer.Deserialize<IDictionary<string, string>>(scopeAsJson));
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to deserialize {scopeInJson} to IDictionary<string,string>", scopeAsJson);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to serialize {state} to json", state);
                }
            }

            return new(null);
        }
        public static ConcurrentDictionary<string,string> Combine(IEnumerable<GeneralLoggerScope>? scopes)
        {
            var kvps = new ConcurrentDictionary<string, string>();

            foreach (var scope in scopes ?? new List<GeneralLoggerScope>())
            {
                foreach (var kvp in scope.Scopes)
                {
                    kvps.AddOrUpdate(kvp.Key, kvp.Value, UpdateScope);
                }
            }

            return kvps;
        }
        public readonly ConcurrentDictionary<string, string> Scopes = new ConcurrentDictionary<string, string>();
        public void Dispose() => Scopes.Clear();
    }

    [ProviderAlias(GeneralLoggerProvider.Name)]
    public sealed class GeneralLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDisposable _onChangeToken;
        private LoggerFilterOptions _settings;
        private readonly ConcurrentDictionary<string, GeneralLogger> _loggers = new ConcurrentDictionary<string, GeneralLogger>();

        public const string Name = "GeneralLogger";

        public GeneralLoggerProvider(IOptionsMonitor<LoggerFilterOptions> settings, IServiceProvider serviceProvider)
        {
            _settings = settings.CurrentValue;
            _onChangeToken = settings.OnChange(revised => _settings = revised);
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new GeneralLogger(name, () => _settings, _serviceProvider));
        }

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
