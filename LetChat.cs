using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace SignalR_Project
{
    public class LetChat : Hub
    {
        //static HashSet<string> CurrentConnections = new HashSet<string>();
        static HashSet<ChatUser> cusers = new HashSet<ChatUser>();
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            var id = Context.ConnectionId;
            //CurrentConnections.Add(id);
            cusers.Add(new ChatUser { connectionid = id, username = "" });
            return base.OnConnectedAsync();
        }

        public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception exception)
        {
            var connection = cusers.FirstOrDefault(x => x.connectionid == Context.ConnectionId);

            if (connection != null)
            {
                cusers.Remove(connection);
            }

            return base.OnDisconnectedAsync(exception);
        }


        //return list of all active connections
        public List<ChatUser> GetAllActiveConnections()
        {
            return cusers.ToList();
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public void Send(string userId, string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("addMessage", message);
            Clients.Client(userId).SendAsync("addMessage", message);
        }
        public void Send1(string message)
        {
            Clients.All.SendAsync("addMessage", message);
        }
        public void AddGroup(string gname)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, gname);
        }
        public void GetAllGroups(string a)
        {
            Clients.All.SendAsync("allGroups", a);
        }
        public void SendToGroup(string groupName, string message)
        {
            Clients.Group(groupName).SendAsync("addChatMessage", message);
        }
        public void AddUser(string uname)
        {
            var connection = cusers.FirstOrDefault(x => x.connectionid == Context.ConnectionId);
            connection.username = uname;

        }
        public void GetAllUsers(string a)
        {
            Clients.All.SendAsync("allUsers", cusers.ToList());
        }
    }
}
