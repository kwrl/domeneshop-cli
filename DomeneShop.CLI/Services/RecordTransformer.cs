using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Services;

public class RecordTransformer(IPublicIpV4AddressResolver ipResolver) : IRecordTransformer
{
    public async Task<IReadOnlyList<Record>> TransformAsync(
        IEnumerable<Record> records, 
        RecordTransform transform
    )
    {
        var processedTransform = await PreProcessTransform(transform);
        
        return records
            .Select(record => Update(record, processedTransform))
            .ToList();
    }

    private const string IpPattern = "{{PUBLIC_IP_V4}}";
    
    private async Task<RecordTransform> PreProcessTransform(RecordTransform transform)
    {
        var host = transform.Host;
        var data = transform.Data;
        var type = transform.Type;
        var timeToLive = transform.TimeToLive;
        
        if (data?.Contains(IpPattern) ?? false)
        {
            var ip = await ipResolver.GetAsync();
            data = data.Replace(IpPattern, ip.ToString());
        }
       
        //TODO: Avoid fetching the IP twice, it's stupid but I'm lazy
        if (host?.Contains(IpPattern) ?? false)
        {
            var ip = await ipResolver.GetAsync();
            host = host.Replace(IpPattern, ip.ToString());
        }

        return new RecordTransform(host, data, type, timeToLive);
    }
    
    public Record Update(Record record, RecordTransform transform)
    {
        return record with
        {
            Host = transform.Host ?? record.Host,
            Data = transform.Data ?? record.Data,
            Type = transform.Type ?? record.Type,
            TimeToLive = transform.TimeToLive ?? record.TimeToLive
        };
    }
}