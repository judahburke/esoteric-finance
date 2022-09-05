using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.Settings
{
    public class GeneralLoggerSettings
    {
        public virtual LogLevel LogLevel { get; set; }
    }
}
