using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamforms.Autocomplete.Test.ViewModels
{
    public class MainViewModel : ViewModel
    {
        ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged(); }
        }

        string _text;
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }

        string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<string> dataSource;

        public MainViewModel()
        {
            Items = new ObservableCollection<string>();

            // This simulates a data source that can be obtained by web service or read from database.
            // It is advisable
            dataSource = new ObservableCollection<string>();
            dataSource.Add("A");
            dataSource.Add("B");
            dataSource.Add("C");
            dataSource.Add("D");
            dataSource.Add("E");
        }

        public ICommand SerchCommand
        {
            get
            {
                return new Command((o) =>
                {
                    Items.Clear();
                    if (!string.IsNullOrEmpty(Text))
                        Items = new ObservableCollection<string>(dataSource.Where(x => x == Text.ToLower() || x == Text.ToUpper()));
                    else
                        Items = dataSource;
                });
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command((o) =>
                {
                    App.Current.MainPage.DisplayAlert("Test", $"You have selected {SelectedItem} item", "Ok");

                    // this deselects the item in the list.
                    SelectedItem = null;
                });
            }
        }
    }
}
