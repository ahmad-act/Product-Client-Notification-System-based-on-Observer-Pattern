using ProductClientNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductClientNotification.Observers
{
    public class AlertMessage<T>
    {
        public AlertType AlertType { get; set; }
        public T Updated { get; set; }
        public T Previous { get; set; }

    }

    public enum AlertType
    {
        Added,              // Notification when a new product is added
        Updated,            // Notification when a product's details are updated
        Removed,            // Notification when a product is removed from the catalog
        PriceUpdated,       // Notification when a product's price changes
    }
}
