using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public interface IDnsRecordService
{
    Task<IReadOnlyList<FullRecord>> SelectAsync(FullRecordQuery recordQuery);
    Task UpdateAsync(
        FullRecordQuery recordQuery,
        FullRecordMutation mutation
    );
}