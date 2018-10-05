using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRegister.Domain.Entities
{
    public class TimeKeeping
    {
        public TimeType Type { get; set; }
        public DateTime Time { get; set; }
    }

    public enum TimeType
    {
        Start,
        End
    }
}
