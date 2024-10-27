using ProductClientNotification.Models;
using ProductClientNotification.Observers;

namespace ProductClientNotification.CURD
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly ProductNotification _productNotification;

        public ProductService(AppDbContext context, ProductNotification notificationService)
        {
            _context = context;
            _productNotification = notificationService;
        }

        #region CURD Operations

        // Get all products
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        // Get a product by ID
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        // Add a new product and notify subscribed clients
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            _productNotification.NotifyNewProduct(product);
        }

        // Update product details including name and price, and notify if price changes
        public void UpdateProduct(int productId, string newName, decimal newPrice)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                bool priceChanged = product.Price != newPrice;

                var old = new Product() { ProductId = product.ProductId, Name = product.Name, Price = product.Price };

                product.Name = newName;
                product.Price = newPrice;

                _context.SaveChanges();

                // Notify if the price was updated
                if (priceChanged)
                {
                    _productNotification.NotifyPriceChange(product, old);
                }
            }
        }
        // Delete a product by ID
        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        #endregion

        #region Extra

        // Update the price of a product and notify subscribed clients
        public void UpdateProductPrice(int productId, decimal newPrice)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var old = new Product() { ProductId = product.ProductId, Name = product.Name, Price = product.Price };

                product.Price = newPrice;
                _context.SaveChanges();
                _productNotification.NotifyPriceChange(product, old);
            }
        }

        #endregion
    }
}
