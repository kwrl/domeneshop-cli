using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public interface IRecordPrinter
{
    Task PrintAsync(IEnumerable<Record> records);
}