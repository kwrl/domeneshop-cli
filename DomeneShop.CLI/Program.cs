using System.CommandLine;
using Abstractions.Integrations.Domeneshop;
using DomeneShop.CLI;
using DomeneShop.CLI.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var clientId = Environment.GetEnvironmentVariable("DOMENESHOP_CLIENT_ID") ?? throw new Exception("DOMENESHOP_CLIENT_ID must be set");
var clientSecret = Environment.GetEnvironmentVariable("DOMENESHOP_CLIENT_SECRET") ?? throw new Exception("DOMENESHOP_CLIENT_SECRET must be set");

services
    .AddHttpClient<IPublicIpV4AddressResolver, PublicIpV4AddressResolver>();

services
    .AddSingleton(new DomeneshopOptions
    {
        ClientId = clientId.Trim(),
        ClientSecret = clientSecret.Trim()
    })
    .AddSingleton<IDomeneShopClient, DomeneshopClient>()
    .AddSingleton<IRecordSelector, RecordSelector>()
    .AddSingleton<IRecordTransformer, RecordTransformer>()
    .AddSingleton<IRecordPrinter, CsvRecordPrinter>()
    .AddSingleton<IRecordSaver, RecordSaver>();

var serviceProvider = services.BuildServiceProvider();

var selector = serviceProvider.GetRequiredService<IRecordSelector>();
var transformer = serviceProvider.GetRequiredService<IRecordTransformer>();
var saver = serviceProvider.GetRequiredService<IRecordSaver>();
var printer = serviceProvider.GetRequiredService<IRecordPrinter>();

var domainIdOption = new Option<int?>("--domain-id", "Filter by domain ID");
var domainNameOption = new Option<string?>("--domain-name", "Filter by domain name");
var recordIdOption = new Option<int?>("--id", "Filter by record ID");
var recordTypeOption = new Option<DnsRecordType?>("--type", "Filter by record type");
var recordHostOption = new Option<string?>("--host", "Filter by record host");
var recordDataOption = new Option<string?>("--data", "Filter by record data");
var timeToLiveOption = new Option<int?>("--time-to-live", "Filter by time to live");

var queryBinder = new FullRecordQueryBinder(
    domainIdOption,
    domainNameOption,
    recordIdOption,
    recordTypeOption,
    recordHostOption,
    recordDataOption,
    timeToLiveOption
);

var setTypeOption = new Option<DnsRecordType?>("--set-type", "Set record type");
var setHostOption = new Option<string?>("--set-host", "Set record host");
var setDataOption = new Option<string?>("--set-data", "Set record data");
var setTimeToLiveOption = new Option<int?>("--set-time-to-live", "Set time to live");

var mutationBinder = new FullRecordMutationBinder(
    setHostOption,
    setDataOption,
    setTypeOption,
    setTimeToLiveOption
);

var saveCommand = new Command("save", "Save transformed DNS records")
{
    setTypeOption,
    setHostOption,
    setDataOption,
    setTimeToLiveOption
};

saveCommand.SetHandler(async (query, mutation) =>
    {
        var records = await selector.SelectAsync(query);
        var transformedRecords = await transformer.TransformAsync(records, mutation);
        await saver.SaveAsync(transformedRecords);
    },
    queryBinder,
    mutationBinder
);

var transformCommand = new Command("transform", "Transform selected DNS records")
{
    setTypeOption,
    setHostOption,
    setDataOption,
    setTimeToLiveOption,
    saveCommand
};

transformCommand.SetHandler(async (query, mutation) =>
    {
        var records = await selector.SelectAsync(query);
        var transformedRecords = await transformer.TransformAsync(records, mutation);
        await printer.PrintAsync(transformedRecords);
    },
    queryBinder,
    mutationBinder
);

var selectCommand = new Command("select", "Select DNS records")
{
    domainIdOption,
    domainNameOption,
    recordIdOption,
    recordTypeOption,
    recordHostOption,
    recordDataOption,
    timeToLiveOption,
    transformCommand
};

selectCommand.SetHandler(async (
        recordQuery
    ) =>
    {
        var records = await selector.SelectAsync(recordQuery);
        await printer.PrintAsync(records);
    },
    queryBinder
);


var rootCommand = new RootCommand("CLI for Domeneshop API")
{
    selectCommand
};

await rootCommand.InvokeAsync(args);