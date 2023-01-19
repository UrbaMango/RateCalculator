using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TaxUnitTest
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        private readonly HttpClient _client;
        public TestingWebAppFactory(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        }

        [Fact]
        public async Task GetResponse()
        {
            var response = await _client.GetAsync("/Taxes/GetTaxRate?municipality=Copenhagen&taxDate=2024-01-19");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Tax schedule not found for the given Municipality and Tax Date", responseString);

        }
    }
}