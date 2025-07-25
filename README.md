# PomeloSoft Stock Management API

This is a simple stock management REST API built with ASP.NET Core. It provides endpoints to manage products and categories.

## Warning

-   Our API service is running on 5008 and the project's connection configurations which are located in app.js, 
are configured according to 5008 port


## Features

-   CRUD operations for Products and Categories.
-   Soft-delete functionality (products and categories can be marked as inactive).
-   In-memory data storage for demonstration purposes.
-   Swagger documentation for easy API exploration.

## Getting Started

### Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

### Running the API

1.  **Clone the repository** (if you haven't already).
2.  **Navigate to the API project directory**:
    ```bash
    cd PomeloSoft.API
    ```
3.  **Run the application**:
    ```bash
    dotnet run
    ```
4.  The API will be available at `https://localhost:7107` and `http://localhost:5008`.
5.  Swagger UI documentation is available at `http://localhost:5008/swagger`.

---

## API Endpoints

The base URL for all endpoints is `http://localhost:5008/api`.

### Categories

| Method | Endpoint                  | Description                                       |
| ------ | ------------------------- | ------------------------------------------------- |
| `GET`  | `/Categories`             | Retrieves a list of all active categories.        |
| `POST` | `/Categories`             | Creates a new category.                           |
| `GET`  | `/Categories/{id}`        | Retrieves a specific category by its ID.          |
| `PUT`  | `/Categories/{id}`        | Updates an existing category's details.           |
| `DELETE`| `/Categories/{id}`        | Deactivates a category (soft delete).             |

### Products

| Method | Endpoint                  | Description                                       |
| ------ | ------------------------- | ------------------------------------------------- |
| `GET`  | `/Products`               | Retrieves a list of all active products.          |
| `GET`  | `/Products/inActive`      | Retrieves a list of all inactive products.        |
| `POST` | `/Products`               | Creates a new product.                            |
| `GET`  | `/Products/{id}`          | Retrieves a specific product by its ID.           |
| `PUT`  | `/Products/{id}`          | Updates an existing product's details.            |
| `DELETE`| `/Products/{id}`          | Deactivates a product (soft delete).              |
| `DELETE`| `/Products/by-name/{name}`| Deactivates a product by its name (soft delete).  |
| `PUT`  | `/Products/{id}/restore`  | Restores an inactive product to be active again.  |

---

## Usage Examples (cURL)

### Create a new Category

```bash
curl -X POST "http://localhost:5008/api/Categories" \
-H "Content-Type: application/json" \
-d '{
  "name": "Keyboards"
}'
```

### Create a new Product

*Note: Make sure the `categoryId` exists.*

```bash
curl -X POST "http://localhost:5008/api/Products" \
-H "Content-Type: application/json" \
-d '{
  "name": "Logitech MX Keys S",
  "brand": "Logitech",
  "price": 129.99,
  "stock": 150,
  "imageThumbnailUrl": "https://example.com/mx-keys.jpg",
  "categoryId": 1
}'
```

### Get All Products

```bash
curl "http://localhost:5008/api/Products"
```

### Update a Product

*This example updates the price and stock for the product with ID `1`. Note that `name` and `brand` cannot be updated.*

```bash
curl -X PUT "http://localhost:5008/api/Products/1" \
-H "Content-Type: application/json" \
-d '{
  "description": "An updated description for the MX Keys S keyboard.",
  "price": 124.99,
  "stock": 120,
  "imageThumbnailUrl": "https://example.com/mx-keys-s.jpg",
  "categoryId": 1
}'
```

### Deactivate a Product (Soft Delete)

*This marks the product with ID `1` as inactive.*

```bash
curl -X DELETE "http://localhost:5008/api/Products/1"
```

### Deactivate a Product by Name (Soft Delete)

*This marks the product named "Logitech MX Keys S" as inactive. Note that the name in the URL must be URL-encoded (e.g., spaces become `%20`).*

```bash
curl -X DELETE "http://localhost:5008/api/Products/by-name/Logitech%20MX%20Keys%20S"
``` 