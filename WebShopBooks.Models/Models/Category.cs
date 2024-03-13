using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebShopBooks.Models.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [MaxLength(30)]
    public string Name { get; set; }
    [DisplayName("Display Order")]
    [Range(1, 30, ErrorMessage = "E ovo je error range-a")]
    public int DisplayOrder { get; set; }
}
