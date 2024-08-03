
using Abstractions.Integrations.Domeneshop;

namespace DomeneShop.CLI.Models;

public record FullRecordQuery(
    int? Id,
    DnsRecordType? Type,
    string? Host,
    string? Data,
    int? DomainId,
    string? DomainName,
    int? TimeToLive
)
{
    public bool Matches(FullRecord record)
    {
        if (Id.HasValue && record.Id != Id)
        {
            return false;
        }

        if (Type.HasValue && record.Type != Type)
        {
            return false;
        }

        if (Host != null && record.Host.Trim() != Host.Trim())
        {
            return false;
        }
        
        if (Data != null && record.Data.Trim() != Data.Trim())
        {
            return false;
        }
        
        if (DomainId.HasValue && record.DomainId != DomainId)
        {
            return false;
        }
        
        if (DomainName != null && record.DomainName.Trim() != DomainName.Trim())
        {
            return false;
        }
        
        if (TimeToLive.HasValue && record.TimeToLive != TimeToLive)
        {
            return false;
        }

        return true;
    }
}