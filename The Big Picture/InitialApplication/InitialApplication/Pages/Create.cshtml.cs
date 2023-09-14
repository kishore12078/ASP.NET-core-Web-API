using InitialApplication.Interfaces;
using InitialApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InitialApplication.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductRepo<Product, int> _productrepo;

        public CreateModel(IProductRepo<Product,int> productRepo) 
        {
            _productrepo=productRepo;
        }

        [BindProperty(SupportsGet =true)]
        public Product NewProduct { get; set; } = new();
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _productrepo.Add(NewProduct);

            return RedirectToPage("Index");
        }
    }
}
