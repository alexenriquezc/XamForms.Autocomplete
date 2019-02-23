using Collection.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamforms.Autocomplete.Test.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region  Properties
        FastObservableCollection<string> _items;
        public FastObservableCollection<string> Items
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

        List<string> dataSource;

        #endregion

        #region Ctr
        public MainViewModel()
        {
            Items = new FastObservableCollection<string>();

            // This simulates a data source that can be obtained by web service or read from database.
            // It is advisable
            dataSource = new List<string>();
            dataSource.Add("A");
            dataSource.Add("B");
            dataSource.Add("C");
            dataSource.Add("D");
            dataSource.Add("E");
        }
        #endregion

        #region Commands
        public ICommand OnAppear
        {
            get
            {
                return new Command((o) =>
                {
                    LoadItems();
                });
            }
        }

        public ICommand SerchCommand
        {
            get
            {
                return new Command((o) =>
                {
                    Items.Clear();
                    if (!string.IsNullOrEmpty(Text))
                        Items.AddRange(dataSource.Where(x => x == Text.ToLower() || x == Text.ToUpper()));
                    else
                        Items.AddRange(dataSource);
                });
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command((o) =>
                {
                    if (SelectedItem != null)
                    {
                        App.Current.MainPage.DisplayAlert("Test", $"You have selected {SelectedItem} item", "Ok");

                        // this deselects the item in the list.
                        SelectedItem = null;
                    }
                });
            }
        }
        #endregion

        #region Methods
        void LoadItems()
        {
            Items.Clear();
            Items.AddRange(dataSource);
        }
        #endregion
    }
}
