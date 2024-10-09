using System.ComponentModel;
using Inventory.Services;

namespace Inventory.Models;

public class MainPageViewModel
{
    public BindingList<Part> Parts { get; init; }
    public BindingList<Product> Products { get; init; }

    public MainPageViewModel(InventoryService inventoryService)
    {
        Parts = inventoryService.AllParts;
        Products = inventoryService.Products;
    }
}