using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WebShopBooks.Models.Models;

namespace WebShopBooks.Models.ViewModels;

public class ShoppingCartViewModel
{
    [ValidateNever]
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
