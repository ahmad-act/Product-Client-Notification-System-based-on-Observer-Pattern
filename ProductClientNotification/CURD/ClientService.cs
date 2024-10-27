using ProductClientNotification.Models;
using ProductClientNotification.Observers;

namespace ProductClientNotification.CURD
{
    public class ClientService
    {
        private readonly AppDbContext _context;
        private readonly ProductNotification _productNotification;

        public ClientService(AppDbContext context, ProductNotification productNotification)
        {
            _context = context;
            _productNotification = productNotification;
        }

        #region CURD Operations

        // Get all clients
        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        // Get a client by ID
        public Client GetClientById(int clientId)
        {
            return _context.Clients.Find(clientId);
        }

        // Add a new client to the database
        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        // Delete a client by ID
        public void DeleteClient(int clientId)
        {
            var client = _context.Clients.Find(clientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
        }

        #endregion

        #region Extra

        // Update a client's subscription preferences
        public void UpdateClientSubscription(int clientId, bool subscribeToPriceChanges, bool subscribeToNewProducts)
        {
            var client = _context.Clients.Find(clientId);
            if (client != null)
            {
                client.NotifyAboutNewProducts = subscribeToNewProducts;
                client.NotifyAboutPriceChanges = subscribeToPriceChanges;

                _context.SaveChanges();
            }
        }

        // Subscribe a client to notifications
        public IDisposable SubscribeClient(Client client)
        {
            var observer = new ClientObserver(client);
            return _productNotification.Subscribe(observer);
        }

        #endregion
    }
}
