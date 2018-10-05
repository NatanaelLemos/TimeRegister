using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegister.Domain.Data;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Tests
{
    public class FakeTimeRepository : ITimeRepository
    {
        private static List<TimeKeeping> _db;

        public FakeTimeRepository()
        {
            if (_db == null)
            {
                _db = new List<TimeKeeping>();
            }
        }

        public Task<TimeKeeping> CreateRegister(TimeKeeping time)
        {
            _db.Add(time);
            return Task.Factory.StartNew(() => time);
        }

        public Task CreateRegister(TimeKeepingBatch time)
        {
            foreach (var item in time.Data)
            {
                _db.Add(item);
            }

            return Task.CompletedTask;
        }

        internal static void Clear()
        {
            if(_db != null)
            {
                _db.Clear();
            }
        }

        public Task<List<TimeKeeping>> Get(DateTime from, DateTime to)
        {
            var fromParam = from.Date;
            var toParam = to.Date.AddDays(1);

            return Task.Factory.StartNew(() =>
            {
                return _db.Where(t => t.Time >= fromParam && t.Time < toParam).ToList();
            });
        }
    }
}
