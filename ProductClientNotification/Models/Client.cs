using System.ComponentModel.DataAnnotations;

namespace ProductClientNotification.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string Name { get; set; }
        public bool NotifyAboutNewProducts { get; set; }
        public bool NotifyAboutPriceChanges { get; set; }
    }
}
