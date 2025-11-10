using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Services;

namespace WebScraping.Tests
{
    public class DateTimeParserTests
    {
        [Fact]
        public void Parse_GivenValidDateTime_ReturnsFileDateTime()
        {
            //Arrange
            var test = "Last Update : Nov 10, 2025 13:57";

            //Act
            string result = DateTimeParser.ParseLastUpdateDate(test);

            //Assert
            Assert.Equal("20251110_1357", result);
        }

        [Fact]
        public void Parse_GivenInvalidDateTime_ReturnsCurrentDateTime()
        {
            //Arrange
            var test = "Invalid time";
            string expectedResult = DateTime.Now.ToString("yyyyMMdd_");

            //Act
            string result = DateTimeParser.ParseLastUpdateDate(test);

            //Assert
            Assert.StartsWith(expectedResult, result);
        }

        [Fact]
        public void Parse_GivenNoDate_ThrowsException()
        {
            //Arrange
            var testEmpty = "";
            string? testNull = null;

            Assert.Throws<ArgumentException>(() => DateTimeParser.ParseLastUpdateDate(testEmpty));
            Assert.Throws<ArgumentException>(() => DateTimeParser.ParseLastUpdateDate(testNull!));

        }

    }
}
