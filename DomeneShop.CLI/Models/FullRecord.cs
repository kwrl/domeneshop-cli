using Abstractions.Integrations.Domeneshop;

namespace DomeneShop.CLI.Models;

public record FullRecord(
    int Id,
    string Host,
    string Data,
    DnsRecordType Type,
    string DomainName,
    int DomainId,
    int TimeToLive
);