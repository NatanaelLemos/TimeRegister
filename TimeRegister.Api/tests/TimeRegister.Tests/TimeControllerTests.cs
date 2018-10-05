using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;
using Xunit;

namespace TimeRegister.Tests
{
    public class TimeControllerTests : TestsBase
    {
        [Theory]
        [InlineData("/v1/time")]
        public async Task Create_and_Retrieve_Data(string url)
        {
            using (var client = CreateClient())
            using (var postJson = CreateJson(new TimeKeepingBatch
            {
                Data = new List<TimeKeeping>
                {
                    new TimeKeeping { Time = new DateTime(2018,09,30,00,00,00), Type = TimeType.Start },
                    new TimeKeeping { Time = new DateTime(2018,09,30,08,00,00), Type = TimeType.End }
                }
            }))
            {
                var postresult = await client.PostAsync(url, postJson);

                using (var response = await client.GetAsync(url + "/2018-09-30%2000:00:00/2018-09-30%2000:00:00"))
                {
                    var getJson = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<IEnumerable<OrganizedTime>>(getJson);

                    Assert.Equal(200, (int)response.StatusCode);
                    Assert.Single(jsonData);
                    Assert.Equal(
                        "2018-09-30 00:00:00",
                        jsonData.First().Start.ToString("yyyy-MM-dd HH:mm:ss")
                    );
                    Assert.Equal(
                        "2018-09-30 08:00:00",
                        jsonData.First().End.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    );
                }
            }
        }

        [Theory]
        [InlineData("/v1/time")]
        public async Task Create_and_Retrieve_CSV(string url)
        {
            using (var client = CreateClient())
            using (var postJson = CreateJson(new TimeKeepingBatch
            {
                Data = new List<TimeKeeping>
                {
                    new TimeKeeping { Time = new DateTime(2018,09,30,00,00,00), Type = TimeType.Start },
                    new TimeKeeping { Time = new DateTime(2018,09,30,08,00,00), Type = TimeType.End }
                }
            }))
            {
                var postresult = await client.PostAsync(url, postJson);

                using (var response = await client.GetAsync(url + "/2018-09-30%2000:00:00/2018-09-30%2000:00:00/csv"))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Assert.Equal(200, (int)response.StatusCode);
                    Assert.Equal("Date,Start,End,\n09/30/2018,00:00:00,08:00:00,\n", responseString);
                    Assert.Equal("\"Report from 2018-09-30 to 2018-09-30.csv\"", response.Content.Headers.ContentDisposition.FileName);
                }
            }
        }
    }
}
