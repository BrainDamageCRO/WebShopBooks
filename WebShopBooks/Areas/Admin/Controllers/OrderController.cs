using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShopBooks.DataAccess.Repository.IRepository;
using WebShopBooks.Models.Models;
using WebShopBooks.Models.ViewModels;
using WebShopBooks.Utility;

namespace WebShopBooks.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public OrderViewModel OrderViewModel { get; set; }

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int orderId)
    {
        OrderViewModel = new()
        {
            OrderHeader = _unitOfWork.OrderHeader.Get(oh => oh.Id == orderId, includeProperties: "ApplicationUser"),
            OrderDetail = _unitOfWork.OrderDetail.GetAll(od => od.OrderHeaderId == orderId, includeProperties: "Product")
        };
        return View(OrderViewModel);
    }

    [HttpPost]
    [Authorize(Roles = Role.Role_Admin + "," + Role.Role_Employee)]
    public IActionResult UpdateOrderDetail()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(oh => oh.Id == OrderViewModel.OrderHeader.Id);

        orderHeaderFromDb.Name = OrderViewModel.OrderHeader.Name;
        orderHeaderFromDb.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = OrderViewModel.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = OrderViewModel.OrderHeader.City;
        orderHeaderFromDb.State = OrderViewModel.OrderHeader.State;
        orderHeaderFromDb.PostalCode = OrderViewModel.OrderHeader.PostalCode;

        if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.Carrier))
        {
            orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
        }

        if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.TrackingNumber))
        {
            orderHeaderFromDb.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
        }

        _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
        _unitOfWork.Save();

        TempData["Success"] = "Order Details Updated Successfully";

        return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id});
    }

    #region API Calls

    [HttpGet]
    public IActionResult GetAll(string orderStatus)
    {
        IEnumerable<OrderHeader> orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

        switch (orderStatus)
        {
            case "pending":
                orderHeaderList = orderHeaderList.Where(ohl => ohl.PaymentStatus == PaymentStatus.Pending);
                break;
            case "inprocess":
                orderHeaderList = orderHeaderList.Where(ohl => ohl.OrderStatus == OrderStatus.InProcess);
                break;
            case "completed":
                orderHeaderList = orderHeaderList.Where(ohl => ohl.OrderStatus == OrderStatus.Shipped);
                break;
            case "approved":
                orderHeaderList = orderHeaderList.Where(ohl => ohl.OrderStatus == OrderStatus.Approved);
                break;
            default:
                break;
        }

        return Json(new { data = orderHeaderList });
    }

    #endregion
}
