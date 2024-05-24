using UsersProducts.Models;

namespace UsersProducts.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task UpdateProduct(Product product);
        Task DeleteProductAsync(int id);
    }
}
