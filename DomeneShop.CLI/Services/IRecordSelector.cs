using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public interface IRecordSelector
{
    Task<IReadOnlyList<Record>> SelectAsync(RecordSelection recordSelection);
}