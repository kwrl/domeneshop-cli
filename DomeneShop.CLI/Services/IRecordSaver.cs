using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public interface IRecordSaver
{
    Task SaveAsync(IEnumerable<Record> records);
}