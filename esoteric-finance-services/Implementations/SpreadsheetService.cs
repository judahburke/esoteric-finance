using Esoteric.Finance.Abstractions.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Services.Implementations
{
    internal class SpreadsheetService : ISpreadsheetService
    {
        public Stream GetSpreadsheetFile(IList<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetTransactions(string spreadsheetFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
