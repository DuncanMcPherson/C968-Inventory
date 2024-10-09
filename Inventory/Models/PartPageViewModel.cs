using System.ComponentModel;
using System.Runtime.CompilerServices;
using Inventory.Services;

namespace Inventory.Models;

public class PartPageViewModel : INotifyPropertyChanged
{
    private const string InHouseDescriptor = "Machine ID";
    private const string OutsourcedDescriptor = "Company Name";
    private string _descriptor = InHouseDescriptor;
    public bool IsCreationMode { get; set; }
    public string FormTitle => IsCreationMode ? "Add Part" : "Modify Part";

    private bool _isInHouse = true;
    public bool IsInHouse
    {
        get => _isInHouse;
        set
        {
            _isInHouse = value;
            MachineCompanyDescriptor = value ? InHouseDescriptor : OutsourcedDescriptor;
            OnPropertyChanged();
        }
    }

    public string MachineCompanyDescriptor
    {
        get => _descriptor!;
        private set
        {
            _descriptor = value;
            OnPropertyChanged();
        }
    }

    public int? PartId
    {
        set
        {
            if (value is not null)
                Part = _inventoryService.lookupPart((int)value);
        }
    }

    private Part? _part;

    public Part? Part
    {
        get => _part;
        set
        {
            _part = value;
            OnPropertyChanged();
        }
    }

    private readonly InventoryService _inventoryService;

    public PartPageViewModel(bool isCreationMode, InventoryService inventoryService, int? partId = null)
    {
        IsCreationMode = isCreationMode;
        _inventoryService = inventoryService;
        if (partId is not null)
        {
            Part = inventoryService.lookupPart((int)partId);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}