using System.Net;
using System.Net.Sockets;

namespace DomeneShop.CLI.Services;

public class PublicIpV4AddressResolver(HttpClient httpClient) : IPublicIpV4AddressResolver
{
    public async Task<IPAddress> GetAsync()
    {
        var response = await httpClient.GetAsync("https://api.ipify.org");
        response.EnsureSuccessStatusCode();

        var ipString = await response.Content.ReadAsStringAsync();
        var address = IPAddress.Parse(ipString);

        if (address.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new Exception("Failed to resolve public IPv4 address");
        }

        return address;
    }
}