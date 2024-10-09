using Inventory.Models;
using Inventory.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Inventory.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartPage : Page, INotifyPropertyChanged
    {
        private readonly PartPageViewModel _viewModel;

        public bool IsSaveEnabled
        {
            get
            {
                var isFormValid = true;
                if (!_viewModel.IsCreationMode)
                {
                    isFormValid = !IdBox.Text.IsNullOrEmpty();
                }

                if (isFormValid)
                {
                    isFormValid = !NameBox.Text.IsNullOrEmpty();
                }

                if (isFormValid)
                {
                    isFormValid = !InventoryBox.Text.IsNullOrEmpty() && IsNumeric(InventoryBox.Text);
                }

                if (isFormValid)
                {
                    isFormValid = !PriceBox.Text.IsNullOrEmpty() && IsDouble(PriceBox.Text);
                }

                if (isFormValid)
                {
                    isFormValid = !MaxEntryBox.Text.IsNullOrEmpty() && IsNumeric(MaxEntryBox.Text);
                }

                if (isFormValid)
                {
                    isFormValid = !MinEntryBox.Text.IsNullOrEmpty() && IsNumeric(MinEntryBox.Text);
                }

                if (isFormValid)
                {
                    isFormValid = !DescriptorBox.Text.IsNullOrEmpty();
                }

                return isFormValid;
            }
            private set => OnPropertyChanged();
        }

        private readonly InventoryService _inventoryService;

        public PartPage()
        {
            this.InitializeComponent();
            _inventoryService = (InventoryService)App.ServiceProvider.GetRequiredService(typeof(InventoryService));
            _viewModel = new PartPageViewModel(true, _inventoryService);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is NavigationArgs args)
            {
                _viewModel.IsCreationMode = args.IsCreating;
                _viewModel.PartId = args.ItemId;
            }

            base.OnNavigatedTo(e);
        }

        private void CancelPart(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SavePart(object sender, RoutedEventArgs e)
        {
            var id = !IdBox.Text.IsNullOrEmpty() ? int.Parse(IdBox.Text) : _inventoryService.GetNextPartId();
            switch (PartTypeSelector.SelectedItem as string)
            {
                case "In-House":
                    if (int.TryParse(DescriptorBox.Text, out var machineId))
                    {
                        var inHouse = new InHousePart
                        {
                            InStock = int.Parse(InventoryBox.Text),
                            MachineId = machineId,
                            Max = int.Parse(MaxEntryBox.Text),
                            Min = int.Parse(MinEntryBox.Text),
                            Name = NameBox.Text,
                            PartID = id,
                            Price = decimal.Parse(PriceBox.Text)
                        };
                        if (_viewModel.IsCreationMode)
                        {
                            _inventoryService.addPart(inHouse);
                        }
                        else
                        {
                            _inventoryService.updatePart(id, inHouse);
                        }
                    }

                    break;
                case "Outsourced":
                    var part = new OutsourcedPart
                    {
                        CompanyName = DescriptorBox.Text,
                        InStock = int.Parse(InventoryBox.Text),
                        Max = int.Parse(MaxEntryBox.Text),
                        Min = int.Parse(MinEntryBox.Text),
                        Name = NameBox.Text,
                        PartID = id,
                        Price = decimal.Parse(PriceBox.Text)
                    };
                    if (_viewModel.IsCreationMode)
                    {
                        _inventoryService.addPart(part);
                    }
                    else
                    {
                        _inventoryService.updatePart(id, part);
                    }

                    break;
            }

            Frame.Navigate(typeof(MainPage));
        }

        private void ValueSelected(object sender, SelectionChangedEventArgs e)
        {
            if (PartTypeSelector.SelectedItem == null)
                return;
            var partType = PartTypeSelector.SelectedItem as string;
            _viewModel.IsInHouse = partType == "In-House";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsSaveEnabled = true;
        }

        private bool IsNumeric(string value)
        {
            try
            {
                _ = int.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsDouble(string value)
        {
            try
            {
                _ = double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}