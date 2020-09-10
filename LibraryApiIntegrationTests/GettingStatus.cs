using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{

    public class GettingStatus : IClassFixture<WebTestFixture>
    {
        private HttpClient Client;

        public GettingStatus(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task GetsAnOkStatusCode()
        {
            var response = await Client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SerializeAsJson()
        {
            var response = await Client.GetAsync("/status");
            var content = await response.Content.ReadAsAsync<StatusResponse>();
            Assert.Equal("Looks Good on my end. Party On.", content.message);
            Assert.Equal("Joe Schmidt", content.checkedBy);
            Assert.Equal(new DateTime(1969, 4, 20, 23, 59, 00), content.whenChecked);
        }
        public class StatusResponse
        {
            public string message { get; set; }
            public string checkedBy { get; set; }
            public DateTime whenChecked { get; set; }
        }




    }
}
