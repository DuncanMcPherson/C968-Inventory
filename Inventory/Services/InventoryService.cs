using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Inventory.Models;

namespace Inventory.Services;

public class InventoryService
{
    public BindingList<Product> Products { get; } = [];
    public BindingList<Part> AllParts { get; } = [];

    public void addProduct(Product product)
    {
        Products.Add(product);
    }

    public bool removeProduct(int productId)
    {
        return Products.Remove(Products.FirstOrDefault(p => p.ProductID == productId)!);
    }

    public Product? lookupProduct(int productId)
    {
        return Products.FirstOrDefault(p => p.ProductID == productId);
    }

    public void updateProduct(int productId, Product updatedProduct)
    {
        var currentProduct = Products.FirstOrDefault(p => p.ProductID == productId);
        currentProduct?.updateProduct(updatedProduct.Name, updatedProduct.Price, updatedProduct.InStock, updatedProduct.Min, updatedProduct.Max);
    }

    public void addPart(Part part)
    {
        AllParts.Add(part);
    }

    public bool deletePart(int partId)
    {
        return AllParts.Remove(AllParts.FirstOrDefault(p => p.PartID == partId)!);
    }

    public Part? lookupPart(int partId)
    {
        return AllParts.FirstOrDefault(p => p.PartID == partId);
    }

    public void updatePart(int partId, Part updatedPart)
    {
        if (updatedPart is InHousePart inHousePart)
        {
            ValidateAndUpdateInHousePart(inHousePart, partId);
        }
        else
        {
            ValidateAndUpdateOutsourcedPart((OutsourcedPart)updatedPart, partId);
        }
    }

    private void ValidateAndUpdateInHousePart(InHousePart part, int partId)
    {
        var existing = AllParts.FirstOrDefault(p => p.PartID == partId);

        if (existing is not InHousePart inHouse)
        {
            return;
        }

        if (part.MachineId != null)
            inHouse.MachineId = part.MachineId;
        UpdateCommonProperties(inHouse, part);
    }

    private void ValidateAndUpdateOutsourcedPart(OutsourcedPart part, int partId)
    {
        var existing = AllParts.FirstOrDefault(p => p.PartID == partId);

        if (existing is not OutsourcedPart outsourced)
        {
            return;
        }

        if (!part.CompanyName.IsNullOrEmpty())
            outsourced.CompanyName = part.CompanyName;
        UpdateCommonProperties(outsourced, part);
    }

    private void UpdateCommonProperties(Part existing, Part updated)
    {
        if (updated.InStock != null)
            existing.InStock = updated.InStock;
        if (updated.Max != null)
            existing.Max = updated.Max;
        if (updated.Min != null)
            existing.Min = updated.Min;
        if (!updated.Name.IsNullOrEmpty())
            existing.Name = updated.Name;
        if (updated.Price != null)
            existing.Price = updated.Price;

    }

    public int GetNextPartId()
    {
        var maxId = AllParts.Select(part => part.PartID).Prepend(0).Max();

        return maxId + 1;
    }
}