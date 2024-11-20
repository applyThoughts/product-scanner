using Microsoft.AspNetCore.SignalR;

namespace ProductScanner.API;

public class ProductService
{
    private readonly IHubContext<ProductHub> _hubContext;

    public ProductService(IHubContext<ProductHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task DecrementProductQuantityAsync(string barcode)
    {
        var product = ProductRepository.GetProductByBarcode(barcode);

        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        if (product.Quantity <= 0)
        {
            throw new Exception("Cannot decrement quantity. The product is out of stock.");
        }
        if (product.Quantity-1== 0)
        {
            throw new Exception("Cannot decrement quantity. The product will be out of stock");
        }

        // Decrement the product quantity by 1
        product.Quantity -= 1;
        ProductRepository.UpdateProduct(product);

        // Optionally, use SignalR to notify all clients about the product update
        await NotifyClientsAsync(product.Barcode, product.Quantity);
    }

    private async Task NotifyClientsAsync(string barcode, int newQuantity)
    {
        // Send a SignalR update to all connected clients
        await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", barcode, newQuantity);
    }
}