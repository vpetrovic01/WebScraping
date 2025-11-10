using WebScraping.Services;

namespace WebScraping.Tests
{
    public class TableParserTests
    {
        private const string ValidTestHtml = @"
        <table>
        <tr><td>Last Update: 2025-11-09</td></tr>
        </table>
        <table>
        <tr><th><b>Gas</b></th></tr>
        <tr><td>Header</td></tr>
        <tr>
            <td>Plant A (1234)</td>
            <td>Other</td>
            <td>450</td>
        </tr>
        <tr>
            <td>Plant B (5678)*</td>
            <td>Other</td>
            <td>999</td>
        </tr>
        </table>";

        private const string NotEnoughRowsHtml = @"
        <table>
        <tr><td>Last Update: 2025-11-09</td></tr>
        </table>
        <table>
        <tr><th><b>Gas</b></th></tr>
        <tr><td>Header</td></tr>
        <tr>
            <td>Plant A (1234)</td>
            <td>Other</td>
        </tr>
        <tr>
            <td>Plant B (5678)*</td>
            <td>Other</td>
            <td>999</td>
        </tr>
        </table>";

        private const string NoHeaderHtml = @"
        <table>
        <tr><td>Last Update: 2025-11-09</td></tr>
        </table>
        <table>
        <tr><th><b>Gas</b></th></tr>
        <tr><td>Header</td></tr>
        <tr>
            <td>Plant A (1234)</td>
            <td>Other</td>
        </tr>
        </table>";

        [Fact]
        public void Parse_GivenValidHtml_ReturnsAssets()
        {
            //Act
            var result = TableParser.Parse(ValidTestHtml, "Gas");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Plant A", result[0].AssetName);
            Assert.Equal("5678", result[1].AssetId);
            Assert.Equal("GAS", result[1].CategoryName);
            Assert.Equal("999", result[1].TotalNetGeneration);
        }

        [Fact]
        public void Parse_GivenNotEnoughRowsHtml_ReturnsEmpty()
        {
            //Act
            var result = TableParser.Parse(NotEnoughRowsHtml, "Gas");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_GivenNoHeaderHtml_ReturnsEmpty()
        {
            //Act
            var result = TableParser.Parse(NoHeaderHtml, "Gas");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_GivenInvalidLastUpdate_ReturnsEmpty()
        {
            //Arrange
            var html = ValidTestHtml.Replace("Last Update", "xyz");

            //Act
            var result = TableParser.Parse(html, "Gas");

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public void Parse_GivenInvalidCategory_ReturnsEmpty()
        {
            //Act
            var result = TableParser.Parse(ValidTestHtml, "Hydro");

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Parse_GivenNullHtml_ReturnsEmpty()
        {
            //Act
            var result = TableParser.Parse(null!, "Hydro");

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
