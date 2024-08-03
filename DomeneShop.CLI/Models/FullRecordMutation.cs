using Abstractions.Integrations.Domeneshop;

namespace DomeneShop.CLI.Models;

public record FullRecordMutation(
    string? Host,
    string? Data,
    DnsRecordType? Type,
    int? TimeToLive
)
{
    public FullRecord Update(FullRecord record)
    {
        return record with
        {
            Host = Host ?? record.Host,
            Data = Data ?? record.Data,
            Type = Type ?? record.Type,
            TimeToLive = TimeToLive ?? record.TimeToLive
        };
    }
}