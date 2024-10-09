using System.ComponentModel;
using System.Runtime.CompilerServices;
using Inventory.Models;
using Inventory.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Inventory.Pages
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private readonly InventoryService _inventoryService;
        public readonly MainPageViewModel ViewModel;
        private Part? _selectedPart;
        public Part? SelectedPart
        {
            get => _selectedPart;
            set
            {

            }
        }

        public bool EnablePartButtons => SelectedPart != null;

        public MainPage()
        {
            this.InitializeComponent();
            _inventoryService = (InventoryService)App.ServiceProvider.GetService(typeof(InventoryService))!;
            ViewModel = new MainPageViewModel(_inventoryService);
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void AddPart(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PartPage), new NavigationArgs { IsCreating = true });
        }

        private void UpdatePart(object sender, RoutedEventArgs e)
        {
            // TODO: Get part id
            Frame.Navigate(typeof(PartPage), new NavigationArgs { IsCreating = false, ItemId = 0 });
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            // TODO: ProductPage
            // Frame.Navigate()
        }

        private void UpdateProduct(object sender, RoutedEventArgs e)
        {
            // TODO: ProductPage
            // Frame.Navigate()
        }

        private void DeletePart(object sender, RoutedEventArgs e)
        {
            if (SelectedPart == null)
                return;
            _inventoryService.deletePart(SelectedPart.PartID);
        }

        private void SelectPart(object sender, SelectionChangedEventArgs e)
        {
            if (PartsView.SelectedItem != null)
            {
                SelectedPart = (Part)PartsView.SelectedItem;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
