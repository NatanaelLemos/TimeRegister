using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRegister.Domain.Dtos
{
    public class OrganizedTime
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
