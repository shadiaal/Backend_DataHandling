using Backend_DataHandling.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_DataHandling.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		[HttpGet("GetProducts")]
		public IActionResult GetProducts(int pageNumber = 1, int pageSize = 10, string sortBy = "id")
		{
			if (pageNumber < 1 || pageSize < 1)
				return BadRequest("Page number and size must be greater than zero.");

			string fileName = sortBy switch
			{
				"name" => "products_sorted_by_name.txt",
				"price" => "products_sorted_by_price.txt",
				_ => "products_sorted_by_id.txt"
			};

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

			if (!System.IO.File.Exists(filePath))
				return NotFound("Sorted file not found.");

			int skip = (pageNumber - 1) * pageSize;
			var products = new List<Product>();

			foreach (var line in System.IO.File.ReadLines(filePath).Skip(skip).Take(pageSize))
			{
				var parts = line.Split(',');
				if (parts.Length != 3) continue;

				if (int.TryParse(parts[0], out int id) && decimal.TryParse(parts[2], out decimal price))
				{
					products.Add(new Product
					{
						Id = id,
						Name = parts[1],
						Price = price
					});
				}
			}

			return Ok(products);
		}
	}
}
