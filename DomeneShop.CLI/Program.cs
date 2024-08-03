using System.CommandLine;
using System.Globalization;
using Abstractions.Integrations.Domeneshop;
using CsvHelper;
using CsvHelper.Configuration;
using DomeneShop.CLI;
using DomeneShop.CLI.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var clientId = Environment.GetEnvironmentVariable("DOMENESHOP_CLIENT_ID") ?? throw new Exception("DOMENESHOP_CLIENT_ID must be set");
var clientSecret = Environment.GetEnvironmentVariable("DOMENESHOP_CLIENT_SECRET") ?? throw new Exception("DOMENESHOP_CLIENT_SECRET must be set");

services
    .AddSingleton(new DomeneshopOptions
    {
        ClientId = clientId.Trim(),
        ClientSecret = clientSecret.Trim()
    })
    .AddSingleton<IDomeneShopClient, DomeneshopClient>()
    .AddSingleton<IDnsRecordService, DnsRecordService>();

var serviceProvider = services.BuildServiceProvider();

var recordService = serviceProvider.GetRequiredService<IDnsRecordService>();

var domainIdOption = new Option<int?>("--domain-id", "Filter by domain ID");
var domainNameOption = new Option<string?>("--domain-name", "Filter by domain name");
var recordIdOption = new Option<int?>("--id", "Filter by record ID");
var recordTypeOption = new Option<DnsRecordType?>("--type", "Filter by record type");
var recordHostOption = new Option<string?>("--host", "Filter by record host");
var recordDataOption = new Option<string?>("--data", "Filter by record data");
var timeToLiveOption = new Option<int?>("--time-to-live", "Filter by time to live");
var includeHeaderOption = new Option<bool>("--include-header", "Include header in output");

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

var mutateCommand = new Command("mutate", "Mutate selected DNS records")
{
    setTypeOption,
    setHostOption,
    setDataOption,
    setTimeToLiveOption
};

mutateCommand.SetHandler(
    (query, mutation) => recordService.UpdateAsync(query, mutation),
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
    includeHeaderOption,
    mutateCommand
};

selectCommand.SetHandler(async (
        recordQuery,
        includeHeader
    ) =>
    {
        var records = await recordService.SelectAsync(recordQuery);

        await using var writer = new StreamWriter(Console.OpenStandardOutput());
        await using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = includeHeader
        });

        await csvWriter.WriteRecordsAsync(records);
    },
    queryBinder,
    includeHeaderOption
);


var rootCommand = new RootCommand("CLI for Domeneshop API")
{
    selectCommand
};

await rootCommand.InvokeAsync(args);