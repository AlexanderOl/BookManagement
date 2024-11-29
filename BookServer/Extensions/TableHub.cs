using Microsoft.AspNetCore.SignalR;

namespace BookServer.Extensions;

public class TableHub : Hub
{
    public async Task UpdateTable()
    {
        await Clients.All.SendAsync("ReceiveTableUpdate");
    }
}
