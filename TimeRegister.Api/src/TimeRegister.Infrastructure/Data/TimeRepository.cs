using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegister.Domain.Data;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Infrastructure.Data
{
    public class TimeRepository : ITimeRepository
    {
        private readonly MongoDbContext _db;

        public TimeRepository(MongoDbContext db)
        {
            _db = db;
        }

        public async Task CreateRegister(TimeKeepingBatch time)
        {
            await _db.Times.InsertManyAsync(time.Data);
        }

        public async Task<List<TimeKeeping>> Get(DateTime from, DateTime to)
        {
            var result = await _db.Times.Find(Builders<TimeKeeping>.Filter.Empty).ToListAsync();
            return result;
            //var paramFrom = from.Date;
            //var paramTo = to.Date.AddDays(1);

            //var filterFrom = Builders<TimeKeeping>.Filter.Gte(t => t.Time, paramFrom);
            //var filterTo = Builders<TimeKeeping>.Filter.Lt(t => t.Time, paramTo);

            //var result = await _db.Times.Find(filterFrom & filterTo).ToListAsync();
            //return result.OrderBy(r => r.Time).ToList();
        }
    }
}
