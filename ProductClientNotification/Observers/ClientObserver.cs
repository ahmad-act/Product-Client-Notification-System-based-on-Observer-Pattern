using ProductClientNotification.Models;

namespace ProductClientNotification.Observers
{
    public class ClientObserver : IObserver<AlertMessage<Product>>
    {
        private Client _client;

        public ClientObserver(Client client)
        {
            _client = client;
        }

        public void OnCompleted()
        {
            Console.WriteLine($"{_client.Name} has stopped receiving notifications.");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"{_client.Name} encountered an error: {error.Message}");
        }

        public void OnNext(AlertMessage<Product> alertMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (alertMessage.AlertType == AlertType.Added && _client.NotifyAboutNewProducts)
            {
                Console.WriteLine($"[{alertMessage.Updated.Name}] has been created with a price of {alertMessage.Updated.Price:C} : [{_client.Name}] received a notification.");
            }
            else if (alertMessage.AlertType == AlertType.PriceUpdated && _client.NotifyAboutPriceChanges)
            {
                Console.WriteLine($"The price of [{alertMessage.Updated.Name}] has been updated to {alertMessage.Updated.Price:C}. Previous price was {alertMessage.Previous.Price:C} : [{_client.Name}] received a notification.");
            }

            Console.ResetColor();
        }
    }
}
