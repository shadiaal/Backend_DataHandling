using Backend_DataHandling.Models;
using System.Text;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Backend_DataHandling.Service
{
	public class ProductSorterHostedService : IHostedService
	{
		private readonly string _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
		private readonly string _sourceFile;
		private readonly int _batchSize = 1000;

		public ProductSorterHostedService()
		{
			_sourceFile = Path.Combine(_dataDir, "products.txt");
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (!File.Exists(_sourceFile))
			{
				Console.WriteLine("Source file not found.");
				return Task.CompletedTask;
			}

			Directory.CreateDirectory(_dataDir);

			var productsBatch = new List<Product>();

			var idWriter = new StreamWriter(Path.Combine(_dataDir, "products_sorted_by_id.txt"), false, Encoding.UTF8);
			var nameWriter = new StreamWriter(Path.Combine(_dataDir, "products_sorted_by_name.txt"), false, Encoding.UTF8);
			var priceWriter = new StreamWriter(Path.Combine(_dataDir, "products_sorted_by_price.txt"), false, Encoding.UTF8);

			foreach (var line in File.ReadLines(_sourceFile))
			{
				var parts = line.Split(',');
				if (parts.Length != 3) continue;

				if (int.TryParse(parts[0], out int id) && decimal.TryParse(parts[2], out decimal price))
				{
					productsBatch.Add(new Product
					{
						Id = id,
						Name = parts[1],
						Price = price
					});

					if (productsBatch.Count >= _batchSize)
					{
						WriteBatch(productsBatch, idWriter, nameWriter, priceWriter);
						productsBatch.Clear();
					}
				}
			}

			// Write remaining products
			if (productsBatch.Count > 0)
			{
				WriteBatch(productsBatch, idWriter, nameWriter, priceWriter);
			}

			idWriter.Close();
			nameWriter.Close();
			priceWriter.Close();

			Console.WriteLine(" Sorted product files created.");
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		private void WriteBatch(List<Product> batch, StreamWriter idWriter, StreamWriter nameWriter, StreamWriter priceWriter)
		{
			foreach (var product in batch.OrderBy(p => p.Id))
				idWriter.WriteLine($"{product.Id},{product.Name},{product.Price}");

			foreach (var product in batch.OrderBy(p => p.Name))
				nameWriter.WriteLine($"{product.Id},{product.Name},{product.Price}");

			foreach (var product in batch.OrderBy(p => p.Price))
				priceWriter.WriteLine($"{product.Id},{product.Name},{product.Price}");
		}
	}
}