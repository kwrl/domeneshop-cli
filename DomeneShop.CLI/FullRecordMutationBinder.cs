using System.CommandLine;
using System.CommandLine.Binding;
using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI.Models;

namespace DomeneShop.CLI;

public class FullRecordMutationBinder : BinderBase<FullRecordMutation>
{
    private readonly Option<string?> _hostOption;
    private readonly Option<string?> _dataOption;
    private readonly Option<DnsRecordType?> _typeOption;
    private readonly Option<int?> _timeToLiveOption;

    public FullRecordMutationBinder(
        Option<string?> hostOption, 
        Option<string?> dataOption, 
        Option<DnsRecordType?> typeOption, 
        Option<int?> timeToLiveOption
    )
    {
        _hostOption = hostOption;
        _dataOption = dataOption;
        _typeOption = typeOption;
        _timeToLiveOption = timeToLiveOption;
    }

    protected override FullRecordMutation GetBoundValue(BindingContext bindingContext)
    {
        var host = bindingContext.ParseResult.GetValueForOption(_hostOption);
        var data = bindingContext.ParseResult.GetValueForOption(_dataOption);
        var type = bindingContext.ParseResult.GetValueForOption(_typeOption);
        var timeToLive = bindingContext.ParseResult.GetValueForOption(_timeToLiveOption);

        return new FullRecordMutation(host, data, type, timeToLive);
    }
}