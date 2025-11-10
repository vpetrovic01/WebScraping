using WebScraping.Models;
using WebScraping.Services;
namespace WebScraping.Tests
{
    public class CsvWriterTests
    {
        [Fact]
        public void Write_GivenValidInput_ThenOutputFile()
        {
            //Arrange 
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.csv");
            List<AssetCategory> assets = new List<AssetCategory>();
            assets.Add(new AssetCategory
            {
                LastUpdate = "2025-11-09",
                AssetName = "Test name",
                AssetId = "#1",
                TotalNetGeneration = "100"
            });
            assets.Add(new AssetCategory
            {
                LastUpdate = "2025-11-09",
                AssetName = "Test name 2",
                AssetId = "#2",
                TotalNetGeneration = "200"
            });
            var expectedContent = "LastUpdate,AssetName,AssetId,TotalNetGeneration\r\n" +
                                  "\"2025-11-09\",\" - Test name\",\"#1\",\"100\"\r\n" +
                                  "\"2025-11-09\",\" - Test name 2\",\"#2\",\"200\"\r\n";

            //Act
            var isSuccess = CsvWriter.TryWrite(filePath, assets);

            //Assert
            Assert.True(isSuccess);
            Assert.True(File.Exists(filePath));

            var actualContent = File.ReadAllText(filePath);
            Assert.Equal(expectedContent, actualContent);
        }

        [Fact]
        public void Write_GivenInvalidFilePath_ThenWriteFailed()
        {
            //Arrange 
            string filePath = "";
            List<AssetCategory> assets = new List<AssetCategory>();
            assets.Add(new AssetCategory
            {
                LastUpdate = "2025-11-09",
                AssetName = "Test name",
                AssetId = "#1",
                TotalNetGeneration = "100"
            });
            assets.Add(new AssetCategory
            {
                LastUpdate = "2025-11-09",
                AssetName = "Test name 2",
                AssetId = "#2",
                TotalNetGeneration = "200"
            });

            //Act
            var isSucces = CsvWriter.TryWrite(filePath, assets);

            //Assert
            Assert.False(isSucces);
        }

        [Fact]
        public void Write_GivenEmptyAssets_ThenWriteFailed()
        {
            //Arrange 
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.csv");
            List<AssetCategory> assets = new List<AssetCategory>();

            //Act
            var isSucces = CsvWriter.TryWrite(filePath, assets);

            //Assert
            Assert.False(isSucces);
        }
    }

}