using ProductClientNotification;
using ProductClientNotification.CURD;
using ProductClientNotification.Models;
using ProductClientNotification.Observers;

public class Program
{
    public static void Main()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();

        var notificationService = new ProductNotification();
        var productService = new ProductService(context, notificationService);
        var clientService = new ClientService(context, notificationService);

        // Seed the database with sample clients and products if empty
        SeedDatabase(clientService, productService);

        // Run the notification tests with multiple clients and products
        RunNotificationTests(clientService, productService);

        // Clear database
        context.Clients.RemoveRange(clientService.GetAllClients());
        context.Products.RemoveRange(productService.GetAllProducts());

        // Save changes to commit deletion
        context.SaveChanges();

        Console.WriteLine("All products have been deleted from the database.");
    }

    private static void SeedDatabase(ClientService clientService, ProductService productService)
    {
        // Seed Clients
        var existingClients = clientService.GetAllClients();
        if (existingClients.Count == 0) // Only seed if no clients exist
        {
            var client1 = new Client { Name = "Client1", NotifyAboutNewProducts = true, NotifyAboutPriceChanges = true };
            var client2 = new Client { Name = "Client2", NotifyAboutNewProducts = true, NotifyAboutPriceChanges = false };
            var client3 = new Client { Name = "Client3", NotifyAboutNewProducts = false, NotifyAboutPriceChanges = true };
            var client4 = new Client { Name = "Client4", NotifyAboutNewProducts = false, NotifyAboutPriceChanges = false };

            // Add to database
            clientService.AddClient(client1);
            clientService.AddClient(client2);
            clientService.AddClient(client3);
            clientService.AddClient(client4);

            //Console.WriteLine("Database seeded with initial clients.");
        }

        // Seed Products
        var existingProducts = productService.GetAllProducts();
        if (existingProducts.Count == 0) // Only seed if no products exist
        {
            var product1 = new Product { Name = "Product1", Price = 100m };

            // Add to database
            productService.AddProduct(product1);
        }
    }

    private static void RunNotificationTests(ClientService clientService, ProductService productService)
    {
        var subscriptions = new List<IDisposable>();

        // Retrieve seeded products from the database
        List<Product> products = productService.GetAllProducts();

        // Retrieve clients from the database and subscribe based on preferences
        List<Client> clients = clientService.GetAllClients();

        foreach (var client in clients)
        {
            if (client.NotifyAboutPriceChanges || client.NotifyAboutNewProducts)
            {
                subscriptions.Add(clientService.SubscribeClient(client));
                Console.WriteLine($"{client.Name} subscribed to notifications.");
            }
        }

        // Use case 1: Create new client and subscribe to new products but not products price change
        Product product2 = new Product { Name = "Product2", Price = 200m };
        productService.AddProduct(product2);

        // Use case 1: Update product price, client1 subscribe for both new product and products price change
        Product product1 = products.Where(p => p.Name == "Product1").FirstOrDefault();
        productService.UpdateProductPrice(product1.ProductId, 190m);
    }
}
