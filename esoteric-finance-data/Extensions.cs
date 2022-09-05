using Esoteric.Finance.Data.Repositories;
using Esoteric.Finance.Data.Repositories.Dbo;
using Esoteric.Finance.Data.Tools;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.Extensions.DependencyInjection;

namespace Esoteric.Finance.Data
{
    public static class Extensions
    {
        public static IServiceCollection AddEsotericFinanceData(this IServiceCollection collection)
        {
            //collection.AddSingleton<ILoggerFactory>(p => new LoggerFactory(new List<ILoggerProvider>
            //{
            //    new GeneralLoggerProvider(p.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>(), p),
            //    new ConsoleLoggerProvider(p.GetRequiredService<IOptionsMonitor<ConsoleLoggerOptions>>())
            //}));
            collection.AddTransient<IDataRepository, PaymentDataRepository>();
            collection.AddTransient<IPaymentDataRepository, PaymentDataRepository>();
            collection.AddSingleton<IEncryptionProvider, EsotericFinanceEncryptionProvider>();
            collection.AddDbContext<EsotericFinanceContext>();

            return collection;
        }
    }
}
