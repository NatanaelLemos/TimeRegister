using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegister.Domain.Data;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Domain.AppServices
{
    public class TimeAppService
    {
        private readonly ITimeRepository _repository;

        public TimeAppService(ITimeRepository repository)
        {
            _repository = repository;
        }

        public Task CreateRegister(TimeKeepingBatch time)
        {
            return _repository.CreateRegister(time);
        }

        public async Task<List<OrganizedTime>> Get(DateTime from, DateTime to)
        {
            var times = await _repository.Get(from, to);
            var result = new List<OrganizedTime>();
            OrganizedTime last = null;

            foreach (var time in times)
            {
                if (time.Type == TimeType.Start)
                {
                    last = new OrganizedTime
                    {
                        Start = time.Time
                    };
                    result.Add(last);
                }
                else
                {
                    if (last == null)
                    {
                        continue;
                    }

                    last.End = time.Time;
                }
            }

            return result;
        }

        public async Task<string> GetCsv(DateTime from, DateTime to)
        {
            var times = await _repository.Get(from, to);
            var table = BuildTimesTable(times);
            return ExportTable(table);
        }

        private List<List<string>> BuildTimesTable(List<TimeKeeping> times)
        {
            var table = new List<List<string>>();
            table.Add(new List<string>());
            table[0].Add("Date");

            var line = 0;
            var column = 0;

            foreach (var date in times.GroupBy(t => t.Time.Date))
            {
                table.Add(new List<string>());
                line++;
                column = 0;

                table[line].Add(date.Key.ToString("MM/dd/yyyy"));
                column++;

                foreach (var time in date)
                {
                    AddColumns(table, column);

                    if (time.Type == TimeType.Start && table[0][column] == "Start")
                    {
                        table[line].Add(time.Time.ToString("HH:mm:ss"));
                    }
                    else if (time.Type == TimeType.End && table[0][column] == "End")
                    {
                        table[line].Add(time.Time.ToString("HH:mm:ss"));
                    }
                    else
                    {
                        table[line].Add("");
                        table[line].Add(time.Time.ToString("HH:mm:ss"));

                        column++;
                        AddColumns(table, column);
                    }

                    column++;
                }
            }

            return table;
        }

        private void AddColumns(List<List<string>> table, int column)
        {
            while (table[0].Count <= column)
            {
                if (table[0][table[0].Count - 1] == "Start")
                {
                    table[0].Add("End");
                }
                else
                {
                    table[0].Add("Start");
                }
            }
        }

        private string ExportTable(List<List<string>> table)
        {
            var stringBuilder = new StringBuilder();
            foreach (var line in table)
            {
                foreach (var col in line)
                {
                    stringBuilder.Append(col);
                    stringBuilder.Append(",");
                }
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}
