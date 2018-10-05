using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TimeRegister.Tests
{
    public abstract class TestsBase
    {
        private static TestServer _server;

        public TestsBase()
        {
            var webHost = new WebHostBuilder().UseStartup<TestStartup>();
            _server = new TestServer(webHost);
        }

        public static HttpMessageHandler CreateBackchannelHttpHandler()
        {
            var backHandler = _server.CreateHandler();
            return backHandler;
        }

        protected T GetService<T>()
        {
            return (T)_server.Host.Services.GetService(typeof(T));
        }

        protected HttpClient CreateClient()
        {
            FakeTimeRepository.Clear();
            return _server.CreateClient();
        }

        protected HttpContent CreateJson(object obj)
        {
            var stringPayload = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            return httpContent;
        }

        protected HttpContent CreateFormUrlEncoded(Dictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return content;
        }
    }
}
