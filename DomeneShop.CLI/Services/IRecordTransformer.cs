using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public interface IRecordTransformer
{
    Task<IReadOnlyList<Record>> TransformAsync(
        IEnumerable<Record> records,
        RecordTransform transform
    );
}