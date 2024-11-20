namespace ProductScanner.API;

public static class ProductRepository
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product { Barcode = "123456", Quantity = 10 },
        new Product { Barcode = "654321", Quantity = 5 },
        new Product { Barcode = "987654", Quantity = 3 },
        new Product { Barcode = "777888", Quantity = 6 },
        new Product { Barcode = "999000", Quantity = 12 },
        new Product { Barcode = "112233", Quantity = 1 },
        new Product { Barcode = "445566", Quantity = 11 },
        new Product { Barcode = "778899", Quantity = 15 },
        new Product { Barcode = "123789", Quantity = 13 },
        new Product { Barcode = "456123", Quantity = 3 },
        new Product { Barcode = "789456", Quantity = 7 }
    };

    public static List<Product> GetAllProducts() => _products;

    public static Product GetProductByBarcode(string barcode)
    {
        return _products.FirstOrDefault(p => p.Barcode == barcode);
    }

    public static void UpdateProduct(Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Barcode == product.Barcode);
        if (existingProduct != null)
        {
            existingProduct.Quantity = product.Quantity;
        }
    }
}