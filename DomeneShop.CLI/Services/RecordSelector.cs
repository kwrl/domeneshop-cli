using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public class RecordSelector(IDomeneShopClient client) : IRecordSelector
{
    public async Task<IReadOnlyList<Record>> SelectAsync(RecordSelection recordSelection)
    {
        var domains = await client.GetDomainsAsync();

        var allRecords = new List<Record>();

        foreach (var domain in domains)
        {
            var records= await client.GetDnsRecordsAsync(domain.Id);

            var relevantQueries = records
                .Select(x => new Record(
                    Id: x.Id!.Value,
                    Host: x.Host,
                    Data: x.Data,
                    Type: x.Type,
                    TimeToLive: x.TimeToLive,
                    DomainName: domain.Name,
                    DomainId: domain.Id
                ))
                .Where(recordSelection.Matches);

            allRecords.AddRange(relevantQueries);
        }

        return allRecords;
    }
}