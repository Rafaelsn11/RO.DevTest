using RO.DevTest.Domain.Abstract;

namespace RO.DevTest.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public virtual ICollection<SaleItem> SaleItems { get; set; }
        = new List<SaleItem>();
}
