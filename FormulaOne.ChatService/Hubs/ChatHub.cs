using FormulaOne.ChatService.DataService;
using FormulaOne.ChatService.Models;
using Microsoft.AspNetCore.SignalR;

namespace FormulaOne.ChatService.Hubs
{
    public class ChatHub : Hub
    {

        private readonly SharedDb _shared;

        public ChatHub(SharedDb shared) => _shared = shared;

        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined");
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            _shared.connections[Context.ConnectionId] = conn;
            await Clients.Group(conn.ChatRoom).SendAsync("JoinSpecificChatRoom", "admin", $"{conn.Username} has joined the {conn.ChatRoom}");

        }

        public async Task SendMessage(string msg)
        {
            if(_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn)) 
            {
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveSpecificMessage", conn.Username, msg);
;            }
        }

        //public async Task LeaveChat(UserConnection conn)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, conn.ChatRoom);
        //    await Clients.Group(conn.ChatRoom).SendAsync("LeaveChatRoom", "admin", $"{conn.Username} has left the room");
        //}
    }
}
