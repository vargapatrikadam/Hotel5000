using Hotel5000_Api_Tests.FunctionalTests.Web.Helpers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.DTOs;
using Xunit;

namespace Hotel5000_Api_Tests.FunctionalTests.Web.Controllers
{
    public class AuthenticationControllerTests : IClassFixture<WebTestFixture>
    {
        private readonly HttpClient client;
        public AuthenticationControllerTests(WebTestFixture factory)
        {
            client = factory.CreateClient();
        }
        [Fact]
        public async Task Login_With_Invalid_Data()
        {
            LoginDto loginData = new LoginDto()
            {
                Identifier = "non existent",
                Password = "non existent"
            };

            var json = JsonConvert.SerializeObject(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", data);
            var code = response.StatusCode;
            Assert.True(code == System.Net.HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task Login_With_Bad_Password()
        {
            LoginDto loginData = new LoginDto()
            {
                Identifier = "preuser1",
                Password = "bad password"
            };

            var json = JsonConvert.SerializeObject(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", data);
            var code = response.StatusCode;
            Assert.True(code == System.Net.HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task Login_With_Valid_Data()
        {
            LoginDto loginData = new LoginDto()
            {
                Identifier = "preuser1",
                Password = "Preuser1password"
            };

            var json = JsonConvert.SerializeObject(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", data);
            var code = response.StatusCode;
            Assert.True(code == System.Net.HttpStatusCode.OK);
        }
    }
}
