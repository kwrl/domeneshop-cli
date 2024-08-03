using System.CommandLine;
using System.CommandLine.Binding;
using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI;

public class FullRecordQueryBinder(Option<int?> domainIdOption, Option<string?> domainNameOption,
        Option<int?> recordIdOption, Option<DnsRecordType?> recordTypeOption, Option<string?> recordHostOption,
        Option<string?> recordDataOption, Option<int?> timeToLiveOption)
    : BinderBase<FullRecordQuery>
{
    protected override FullRecordQuery GetBoundValue(BindingContext bindingContext)
    {
        var domainId = bindingContext.ParseResult.GetValueForOption(domainIdOption);
        var domainName = bindingContext.ParseResult.GetValueForOption(domainNameOption);
        var recordId = bindingContext.ParseResult.GetValueForOption(recordIdOption);
        var recordType = bindingContext.ParseResult.GetValueForOption(recordTypeOption);
        var recordHost = bindingContext.ParseResult.GetValueForOption(recordHostOption);
        var recordData = bindingContext.ParseResult.GetValueForOption(recordDataOption);
        var timeToLive = bindingContext.ParseResult.GetValueForOption(timeToLiveOption);
        
        return new FullRecordQuery(
            Id: recordId,
            Type: recordType,
            Host: recordHost,
            Data: recordData,
            DomainId: domainId,
            DomainName: domainName,
            TimeToLive: timeToLive
        );
    }
}