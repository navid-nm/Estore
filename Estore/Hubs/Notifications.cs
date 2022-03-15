using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Estore.Hubs
{
    public class Notifications : Hub
    {
        public async Task NotifyOfMessage(string sender, string recipient)
        {
            await Clients.Others.SendAsync("ReceiveMessageNotification", recipient, sender);
        }

        public async Task NotifyOfSale(string seller, string buyer, string item, string findCode)
        {
            await Clients.Others.SendAsync("ReceiveSaleNotification", seller, buyer, item, findCode);
        }
    }
}
