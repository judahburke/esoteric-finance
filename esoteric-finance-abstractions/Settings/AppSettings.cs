using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.Settings
{
    public class AppSettings
    {
        public virtual DataSettings Data { get; set; } = new DataSettings();
    }
}
