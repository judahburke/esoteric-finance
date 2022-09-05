using Esoteric.Finance.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Esoteric.Finance.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddEsotericFinanceServices(this IServiceCollection collection)
        {
            collection.AddTransient<ISpreadsheetService, SpreadsheetService>();

            return collection;
        }
    }
}
