namespace Inventory.Models;

public abstract class Part
{
    public int PartID { get; init; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? InStock { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
}

public class InHousePart : Part
{
    public int? MachineId { get; set; }
}

public class OutsourcedPart : Part
{
    public string? CompanyName { get; set; }
}