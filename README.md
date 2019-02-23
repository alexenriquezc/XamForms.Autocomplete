# XamForms.Autocomplete
Xamarin forms bindable autocomplete control

Example

# XAML
``` XAML
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:views="clr-namespace:XamForms.Controls;assembly=XamForms"
             xmlns:vm="clr-namespace:Xamforms.Autocomplete.Test.ViewModels"
             x:Class="Xamforms.Autocomplete.Test.MainPage">

    <ContentPage.BindingContext>
        <vm:MainViewModel/>
    </ContentPage.BindingContext>

    <views:AutoComplete Text="{Binding Text}"
                        ItemsSource="{Binding Items}"
                        SearchCommand="{Binding SerchCommand}"
                        SelectedItemCommand="{Binding SelectItemCommand}"
                        SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
                        Padding="10"/>

</ContentPage>
```

# ViewModel
```C#
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
                    if(SelectedItem != null)
                    {
                        App.Current.MainPage.DisplayAlert("Test", $"You have selected {SelectedItem} item", "Ok");

                        // this deselects the item in the list.
                        SelectedItem = null;
                    }
                });
            }
        }
    }
```
