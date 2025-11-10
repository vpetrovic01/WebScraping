# WebScraping Project

Simple .NET web scraping application for fetching and parsing data from the AESO market reports page.

## ✅ Features
- Fetch HTML from remote source
- Parse table data
- Extract and format asset generation info
- Covered with unit tests for main services
- After running the program, the generated CSV file is automatically saved in the application's **base directory** with the name format:  
{tableName}\_data\_{timestamp}.csv

*Example:* `Wind_data_20251110_1518.csv`
