using InitialApplication.Interfaces;
using InitialApplication.Models;

namespace InitialApplication.Repositories
{
    public class ProductRepo : IProductRepo<Product, int>
    {
        ICollection<Product> products;
        public ProductRepo()
        {
            products = new List<Product>();
        }
        public Product Add(Product item)
        {
            products.Add(item);
            return item;
        }

        public Task<Product> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Get(int key)
        {
            throw new NotImplementedException();
        }

        public ICollection<Product> GetAll()
        {
            Product product = new Product() { Id = 1, Name = "Pencil", Price = 5, Stock = 34, PhotoFileName = "FileName" };
            products.Add(product);
            return products;
        }

        public Task<Product> Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
