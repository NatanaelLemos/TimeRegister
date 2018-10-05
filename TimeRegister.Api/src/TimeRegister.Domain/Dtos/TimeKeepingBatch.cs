using System;
using System.Collections.Generic;
using System.Text;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Domain.Dtos
{
    public class TimeKeepingBatch
    {
        public List<TimeKeeping> Data { get; set; }
        public TimeKeepingBatch()
        {
            Data = new List<TimeKeeping>();
        }
    }
}
