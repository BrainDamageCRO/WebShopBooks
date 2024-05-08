using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebShopBooks.DataAccess.Repository.IRepository;
using WebShopBooks.Models.Models;

namespace WebShopBooks.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region API Calls

    [HttpGet]
    public IActionResult GetAll()
    {
        List<OrderHeader> orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
        return Json(new { data = orderHeaderList });
    }

    #endregion
}
