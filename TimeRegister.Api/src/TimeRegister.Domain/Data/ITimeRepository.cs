using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Domain.Data
{
    public interface ITimeRepository
    {
        Task<List<TimeKeeping>> Get(DateTime from, DateTime to);
        Task CreateRegister(TimeKeepingBatch time);
    }
}
