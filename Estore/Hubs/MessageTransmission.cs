using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Estore.Hubs
{
    public class MessageTransmission : Hub
    {
        public async Task NotifyOfMessage(string sender, string recipient)
        {
            await Clients.Others.SendAsync("ReceiveMessageNotification", recipient, sender);
        }
    }
}
