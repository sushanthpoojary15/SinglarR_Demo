using Microsoft.AspNetCore.SignalR;
using SignalRWebApi.ChatModels;
using SignalRWebApi.ChatServices;
using System.ComponentModel;

namespace SignalRWebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatServices;

        public ChatHub(ChatService chatServices)
        {
            _chatServices = chatServices; 
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Come2Chat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2Chat");
            var user = _chatServices.GetUserByConnectionId(Context.ConnectionId);
            _chatServices.RemoveUserFromList(user);
            await DisplayOnlineUsers();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReceiveMessage(MessageDTO message)
        {
            await Clients.Group("Come2Chat").SendAsync("NewMessages", message);
        }

        public async Task CreatePrivateChat(MessageDTO message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatServices.GetConnectionIdByUser(message.To);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);
            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);

        }

        public async Task ReceivePrivateMessage(MessageDTO message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Client(privateGroupName).SendAsync("NewPrivateMessage", message);

        }

        public async Task RemovePrivateChat(string from, string to)
        {
            string privateGroupName = GetPrivateGroupName(from, to); 
            await Clients.Client(privateGroupName).SendAsync("ClosePrivateChat");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatServices.GetConnectionIdByUser(to);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatServices.AddUserConnectionId(name, Context.ConnectionId);
            await DisplayOnlineUsers(); 
        }

        public async Task DisplayOnlineUsers()
        {
            var onlineUsers = _chatServices.GetOnlineUsers();
            await Clients.Group("Come2Chat").SendAsync("OnlineUsers", onlineUsers);
        }

        public string GetPrivateGroupName(string from, string to)
        {
                var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }
    }
}
