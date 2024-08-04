using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI.Mapping;

public static class Mappings
{
    public static DnsRecord ToDto(this Record record)
    {
        var dto = new DnsRecord(
            type: record.Type,
            host: record.Host,
            data: record.Data
        );

        dto.Id = record.Id;
        dto.TimeToLive = record.TimeToLive;
        
        return dto;
    }
}