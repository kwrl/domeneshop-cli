using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public class CsvRecordPrinter : IRecordPrinter
{
    public async Task PrintAsync(IEnumerable<Record> records)
    {
        await using var writer = new StreamWriter(Console.OpenStandardOutput());
        await using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        await csvWriter.WriteRecordsAsync(records);
    }
}