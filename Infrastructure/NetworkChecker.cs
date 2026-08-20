using System.Net.NetworkInformation;

namespace NineTwoNineTerminal.Infrastructure;

public static class NetworkChecker
{
    public static bool IsConnected(int pingTimeout = 50, int dnsTimeout = 300) =>
        IsConnectedAsync(pingTimeout, dnsTimeout).GetAwaiter().GetResult();

    private static async Task<bool> IsConnectedAsync(int pingTimeout = 50, int dnsTimeout = 300)
    {
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            return false;
        }

        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync("1.1.1.1", pingTimeout);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
        }
        catch
        {
            // Ping can fail if ICMP packets are blocked by firewalls or network policies.
        }

        try
        {
            using var cts = new CancellationTokenSource(dnsTimeout);
            var addresses = await System.Net.Dns.GetHostAddressesAsync("one.one.one.one", cts.Token);
            return addresses.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}
