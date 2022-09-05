using Esoteric.Finance.Abstractions.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Services
{
    public interface ISpreadsheetService
    {
        public IList<Transaction> GetTransactions(string spreadsheetFilePath);
        public Stream GetSpreadsheetFile(IList<Transaction> transactions);
    }
}
