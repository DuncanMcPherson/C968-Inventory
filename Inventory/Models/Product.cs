using System.ComponentModel;
using System.Linq;

namespace Inventory.Models;

public class Product
{
    public BindingList<Part> AssociatedParts { get; }
    public int ProductID { get; }
    public string? Name { get; private set; }
    public decimal? Price { get; private set; }
    public int? InStock { get; private set; }
    public int? Min { get; private set; }
    public int? Max { get; private set; }

    public Product(int id, string? name, decimal? price, int? inStock, int? min, int? max)
    {
        AssociatedParts = [];
        ProductID = id;
        Name = name;
        Price = price;
        InStock = inStock;
        Min = min;
        Max = max;
    }

    public void addAssociatedPart(Part part)
    {
        AssociatedParts.Add(part);
    }

    public bool removeAssociatedPart(int partId)
    {
        return AssociatedParts.Remove(AssociatedParts.FirstOrDefault(p => p.PartID == partId)!);
    }

    public Part lookupAssociatedPart(int partId)
    {
        return AssociatedParts.FirstOrDefault(p => p.PartID == partId)!;
    }

    public void updateProduct(string? name, decimal? price, int? inStock, int? min, int? max)
    {
        if (!name.IsNullOrEmpty())
            Name = name;

        if (price != null)
            Price = price;

        if (inStock != null)
            InStock = inStock;

        if (min != null)
            Min = min;

        if (max != null) Max = max;
    }
}