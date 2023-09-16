using InitialApplication.Interfaces;
using InitialApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InitialApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductRepo<Product, int> _productRepo;

        public IndexModel(ILogger<IndexModel> logger,
                          IProductRepo<Product,int> productRepo)
        {
            _logger = logger;
            _productRepo=productRepo;
        }

        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>(); //It is like string.Empty

        public void OnGet()
        {
            Products = _productRepo.GetAll();
        }
    }
}