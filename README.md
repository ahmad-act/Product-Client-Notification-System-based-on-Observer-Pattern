# Product-Client Notification System based on Observer Pattern

## Overview

This project is a C# application that implements a notification system for clients subscribed to product events such as product creation, deletion, and price updates. It uses the Observer pattern to allow clients to receive notifications based on their subscription preferences.

### Key Features
- **Client Subscription Preferences**: Clients can choose to receive notifications for new products, product updates, removals, and/or price changes.
- **Notification Mechanism**: Clients are notified of events in real-time through the Observer pattern.
- **Database Seeding**: Initial sample data for clients and products is seeded if the database is empty.

## Project Structure

```plaintext
.
├── CURD/
│   ├── ClientService.cs        # CRUD operations for clients and notification subscription management
│   ├── ProductService.cs       # CRUD operations for products and notification triggering
│
├── Models/
│   ├── Client.cs               # Model representing a client with subscription preferences
│   ├── Product.cs              # Model representing a product with price information
│
├── Observers/
│   ├── ProductNotification.cs  # IObservable implementation to manage client notifications
│   ├── ClientObserver.cs       # IObserver implementation representing a client receiving notifications
│   ├── AlertMessage.cs         # Represents notification alerts for product changes,including AlertType enum
│
├── Program.cs                  # Main entry point of the application, manages data seeding, tests, and cleanup
└── README.md                   # Project documentation
```

## Visualization of the relationships

```plaintext
                                  +-----------------------+
                                  |    ProductService     |
                                  |-----------------------|
                                  | + AddProduct()        |
                                  | + UpdateProduct()     |
                                  | + UpdateProductPrice()|
                                  |-----------------------|
                                  |  Calls notifications  |
                                  |  on product changes   |
                                  +-----------+-----------+
                                              |
                                              |
                                              v
                                +---------------------------+
                                |  ProductNotification      |
                                |---------------------------|
                                | - List<IObserver<...>>    |
                                | + Subscribe()             |
                                | + NotifyNewProduct()      |
                                | + NotifyPriceChange()     |
                                |---------------------------|
                                | Manages client observers  |
                                | Sends notifications       |
                                +-------------+-------------+
                                              |
                                              |
                           +------------------+------------------+
                           |                  |                  |
                           v                  |                  v
         +------------------------+           |        +----------------------+
         |      ClientObserver    |           |        |   AlertMessage<T>    |
         |------------------------|           |        |----------------------|
         | - Client Preferences   |           |        | - AlertType enum     |
         | + OnNext()             |           |        | - Updated (Product)  |
         |------------------------|           |        | - Previous (Product) |
         | Receives notifications |           |        |----------------------|
         | Displays messages      |           |        | Holds notification   |
         +------------------------+           |        | data about product   |
                                              |        +----------------------+
                                              |
                                              |
                                              |
                                              v
                                   +-----------------------+
                                   |      ClientService    |
                                   |-----------------------|
                                   | + UpdateClient...()   |
                                   | + SubscribeClient()   |
                                   |-----------------------|
                                   | Manages client        |
                                   | preferences and       |
                                   | subscriptions         |
                                   +-----------------------+
```

## Usage

### Seed the Database

When the application starts, it checks if the database has any clients or products.
If not, it will seed the database with sample clients and products.

### Run Notification Tests

The application simulates product events, such as:
Adding a new product.
Updating the price of an existing product.
Notifications are sent to clients based on their preferences.

### Database Cleanup

After running the notification tests, all clients and products are removed from the database to reset it for future runs.

## Code Explanation

### `Program.cs`

- **Main Method**: Initializes the database, seeding it if necessary, and then runs notification tests to demonstrate client notifications.
- **SeedDatabase**: Seeds initial clients and products if none exist.
- **RunNotificationTests**: Simulates product-related actions to trigger notifications.

### CURD/

- `ClientService.cs`: Manages CRUD operations for clients, updates subscription preferences, and handles client subscriptions to notifications.
- `ProductService.cs`: Manages CRUD operations for products and notifies subscribed clients of new product additions or price changes.

### Models/

- `Client.cs`: Represents clients with their notification preferences.
- `Product.cs`: Represents products with their price details.

### Observers/

- `ProductNotification.cs`: Implements IObservable to manage a list of client observers. Notifies them of product-related events.
- `ClientObserver.cs`: Implements IObserver, allowing each client to receive and display notifications based on their subscription preferences.
- `AlertMessage.cs`: Defines the `AlertMessage<T>` class, which represents a notification alert with details about product changes, as well as an `AlertType` enum indicating the type of alert.

### `AlertMessage.cs` - Notification Alert Details

The `AlertMessage.cs` file plays a central role in structuring notifications for product changes, offering flexibility through generic types to accommodate a variety of updates while maintaining type safety. It consists of the `AlertMessage<T>` class and the `AlertType` enum, outlined as follows:

#### `AlertMessage<T>` Class

The `AlertMessage<T>` class represents a notification message with specific details about a product change. It's defined as a generic class, allowing it to be adapted to different data types (T) while encapsulating the necessary alert information.

#### Properties

- **`AlertType`**: An enum property defining the type of alert (e.g., added, updated, removed).
- **`Updated`**: A property of generic type T that contains the updated product details.
- **`Previous`**: A property of generic type T that holds previous product details, useful in case of updates where comparisons are necessary, such as with price changes.

```csharp
public class AlertMessage<T>
{
    public AlertType AlertType { get; set; }
    public T Updated { get; set; }
    public T Previous { get; set; }
}
```

#### `AlertType` Enum

The `AlertType` enum is used to define the specific event type for a product alert, allowing observers to filter and process notifications based on the type of change. The values include:

- **Added**: Notifies observers when a new product is added to the catalog.
- **Updated**: Used when general product details, other than price, are modified.
- **Removed**: Signals when a product is deleted from the catalog.
- **PriceUpdated**: Indicates a specific notification for price changes, giving the previous and updated values.

```csharp
public enum AlertType
{
    Added,              // Notification when a new product is added
    Updated,            // Notification when a product's details are updated
    Removed,            // Notification when a product is removed from the catalog
    PriceUpdated,       // Notification when a product's price changes
}
```

#### Example Usage of `AlertMessage<T>`

In the ProductNotification class, `AlertMessage<Product>` is created and passed to observers to notify clients about changes:

```csharp
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
```

#### Summary

The `AlertMessage.cs` file, through the `AlertMessage<T>` class and `AlertType` enum, provides a clear, structured approach to notifying clients about changes in products. It enables flexibility and customization of notification messages, ensuring clients receive only the information relevant to their preferences.
