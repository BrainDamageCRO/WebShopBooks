using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebShopBooks.DataAccess.Repository.IRepository;
using WebShopBooks.Models.Models;

namespace WebShopBooks.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return View(productList);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCart shoppingCart = new()
        {
            Product = _unitOfWork.Product.Get(p => p.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId
        };
        
        return View(shoppingCart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        shoppingCart.ApplicationUserId = userId;

        // Logic for not having duplicates
        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart
            .Get(sp => sp.ApplicationUserId == userId 
            && sp.ProductId == shoppingCart.ProductId);

        
        if (cartFromDb != null)
        {
            // If true, shoppingCart already exists
            cartFromDb.Count += shoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        else
        {
            // If we get here, then we are adding new shoppingCart
            _unitOfWork.ShoppingCart.Add(shoppingCart);
        }
        
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
