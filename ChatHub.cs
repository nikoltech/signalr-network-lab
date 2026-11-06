namespace SignalRNetworkLab;

using Microsoft.AspNetCore.SignalR;

public interface IChatClient
{
    Task Receive(string user, string text);
    Task System(string text);
}

public class ChatHub : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.System($"Ваш ConnectionId: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.Group(room).System($"{Context.ConnectionId} увійшов у «{room}»");
    }

    public Task SendToRoom(string room, string user, string text) =>
        Clients.Group(room).Receive(user, text);

    public Task SendToOthers(string user, string text) =>
        Clients.Others.Receive(user, text);
}
