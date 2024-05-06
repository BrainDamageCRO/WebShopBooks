using WebShopBooks.Models.Models;

namespace WebShopBooks.Models.ViewModels;

public class OrderViewModel
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetail> OrderDetail { get; set; }
}
