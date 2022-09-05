using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.Settings
{
    public class DataSettings
    {
        public virtual string? ColumnEncryptionKey { get; set; }
        public virtual string SqlLitePath => System.IO.Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "esoteric-finance.db");
    }
}
