using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Mapping;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public class DnsRecordService(IDomeneShopClient client) : IDnsRecordService
{
    private readonly IDomeneShopClient _client = client;

    public async Task<IReadOnlyList<FullRecord>> SelectAsync(FullRecordQuery recordQuery)
    {
        var domains = await _client.GetDomainsAsync();

        var allRecords = new List<FullRecord>();

        foreach (var domain in domains)
        {
            var records= await _client.GetDnsRecordsAsync(domain.Id);

            var relevantQueries = records
                .Select(x => new FullRecord(
                    Id: x.Id!.Value,
                    Host: x.Host,
                    Data: x.Data,
                    Type: x.Type,
                    TimeToLive: x.TimeToLive,
                    DomainName: domain.Name,
                    DomainId: domain.Id
                ))
                .Where(recordQuery.Matches);

            allRecords.AddRange(relevantQueries);
        }

        return allRecords;
    }

    public async Task UpdateAsync(FullRecordQuery recordQuery, FullRecordMutation mutation)
    {
        var records = await SelectAsync(recordQuery);

        foreach (var record in records)
        {
            var updatedRecord = mutation.Update(record);
            await _client.UpdateDnsRecordAsync(record.DomainId, updatedRecord.ToDto());
        }
    }
}