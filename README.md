
# Product Sorting and Pagination API

This ASP.NET Core Web API project provides functionality to read a raw product file, sort it by different fields (ID, Name, Price), and expose the sorted data through an API that supports pagination and sorting.

##  Project Structure

- **ProductSorterHostedService.cs**  
  Reads and processes `products.txt` into three sorted files:
  - `products_sorted_by_id.txt`
  - `products_sorted_by_name.txt`
  - `products_sorted_by_price.txt`

- **ProductsController.cs**  
  Exposes a GET API endpoint `/api/Products/GetProducts` that:
  - Supports pagination (`pageNumber`, `pageSize`)
  - Supports sorting by `id`, `name`, or `price`

- **Models/Product.cs**  
  A simple model with:  
  ```csharp
  public int Id { get; set; }
  public string Name { get; set; }
  public decimal Price { get; set; }
  ```

##  How It Works

1. On application Program, the `IHostedService` reads `products.txt`.
2. Products are loaded in batches (1000 records), parsed, sorted, and written to 3 different output files.
3. The API reads from these sorted files and returns the requested page of products.

##  API Endpoint

### `GET /api/Products/GetProducts`

#### Query Parameters:
- `pageNumber` (default: 1)  
- `pageSize` (default: 10)  
- `sortBy` (default: `"id"`, optional: `"name"` or `"price"`)

#### Example:
```
GET /api/Products/GetProducts?pageNumber=2&pageSize=5&sortBy=price
```

###  Response:
```json
[
  {
    "id": 1,
    "name": "Apple",
    "price": 10.99
  },
  ...
]
```

## 🗂 Required Files

- Place a `products.txt` file in the `Data/` directory at the root of the project.
- Each line in the file should be in this format:
  ```
  1,Apple,10.99
  2,Banana,4.75
  ```

##  Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/shadiaal/Backend_DataHandling.git
   cd Backend_DataHandling

2. Run the application using Visual Studio or:
   ```bash
   dotnet run
   ```

3. Use **Postman**, **Swagger**, or browser to access:
   ```
   https://localhost:7269/api/Products/GetProducts
   ```

---


