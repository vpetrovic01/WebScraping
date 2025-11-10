using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Services;

namespace WebScraping.Tests
{
    public class HtmlFetcherTests

    {
        [Fact]
        public async Task Fetch_GivenValidInput_ThenFetched()
        {
            //Arrange
            string url = "http://ets.aeso.ca/ets_web/ip/Market/Reports/CSDReportServlet";
          
            //Act
            var html = await HtmlFetcher.FetchHtml(url);
            
            //Asset
            Assert.False(string.IsNullOrEmpty(html));
        }

        [Fact]
        public async Task Fetch_GivenInvalidInput_ThenError()
        {
            //Arrange
            string url = "***";
            
            //Act
            var html = await HtmlFetcher.FetchHtml(url);
            
            //Asset
            Assert.Equal("", html);
        }

    }
}
