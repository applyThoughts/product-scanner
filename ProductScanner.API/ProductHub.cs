using Microsoft.AspNetCore.SignalR;

namespace ProductScanner.API;

public class ProductHub : Hub
{
    public async Task UpdateProductQuantity(string barcode, int newQuantity)
    {
        // Broadcast the updated quantity to all clients
        await Clients.All.SendAsync("ReceiveProductUpdate", barcode, newQuantity);
    }
}