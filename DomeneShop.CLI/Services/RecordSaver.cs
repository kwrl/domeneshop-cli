using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Mapping;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public class RecordSaver(IDomeneShopClient client) : IRecordSaver
{
    public async Task SaveAsync(IEnumerable<Record> records)
    {
        foreach (var record in records)
        {
            await client.UpdateDnsRecordAsync(record.DomainId, record.ToDto());
        }
    }
}