using ProductClientNotification.Models;

namespace ProductClientNotification.Observers
{
    public class ProductNotification : IObservable<AlertMessage<Product>>
    {
        private List<IObserver<AlertMessage<Product>>> _alertMessages = new List<IObserver<AlertMessage<Product>>>();

        public IDisposable Subscribe(IObserver<AlertMessage<Product>> _alertMessage)
        {
            if (!this._alertMessages.Contains(_alertMessage))
            {
                this._alertMessages.Add(_alertMessage);
            }

            return new Unsubscriber(_alertMessages, _alertMessage);
        }

        public void NotifyNewProduct(Product product)
        {
            foreach (var alertMessage in _alertMessages)
            {
                AlertMessage<Product> productAlertMessage = new AlertMessage<Product>() { AlertType = AlertType.Added, Updated = product };
                alertMessage.OnNext(productAlertMessage);
            }
        }

        public void NotifyPriceChange(Product updated, Product previous)
        {
            foreach (var clientObserver in _alertMessages)
            {
                AlertMessage<Product> observerType = new AlertMessage<Product>
                {
                    AlertType = AlertType.PriceUpdated,
                    Updated = updated,
                    Previous = previous
                };

                clientObserver.OnNext(observerType);
            }
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<AlertMessage<Product>>> _clientObservers;
            private IObserver<AlertMessage<Product>> _clientObserver;

            public Unsubscriber(List<IObserver<AlertMessage<Product>>> clientObservers, IObserver<AlertMessage<Product>> clientObserver)
            {
                _clientObservers = clientObservers;
                _clientObserver = clientObserver;
            }

            public void Dispose()
            {
                if (_clientObserver != null && _clientObservers.Contains(_clientObserver))
                {
                    _clientObservers.Remove(_clientObserver);
                }
            }
        }
    }
}
