using Microsoft.AspNetCore.Mvc;
using WebShopBooks.DataAccess.Data;

namespace WebShopBooks.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}
