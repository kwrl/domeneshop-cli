using Abstractions.Integrations.Domeneshop;

namespace DomeneShop.CLI.Models;

public record RecordTransform(
    string? Host,
    string? Data,
    DnsRecordType? Type,
    int? TimeToLive
);