using System.Net;

namespace DomeneShop.CLI.Services;

public interface IPublicIpV4AddressResolver
{
    public Task<IPAddress> GetAsync();
}