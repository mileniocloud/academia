using Microsoft.AspNet.SignalR;

namespace AcademiaEnVivo.signalr.hubs
{
    public class chatHub : Hub
    {
        public void Send(string name, string message, string room)
        {
            Clients.All.addNewMessageToPage(name, message, room);
        }
    }
}