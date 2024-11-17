using Microsoft.AspNetCore.SignalR;

namespace TallerMecanico.Hubs
{
    public class NotificationHub : Hub
    {
        // Método para enviar una notificación a todos los clientes conectados
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
        public async Task NotifyUserAsync(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}