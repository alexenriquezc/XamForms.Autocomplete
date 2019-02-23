using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamForms.Templates;

namespace XamForms.Controls
{
    public class AutoComplete : ContentView
    {
        #region  BindableProperties        
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(AutoComplete), default(object), BindingMode.TwoWay, null, propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(AutoComplete), null, BindingMode.TwoWay, null, propertyChanged: OnSearchCommandChanged);
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(AutoComplete), null, BindingMode.TwoWay, null, propertyChanged: OnSelectedItemCommandChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoComplete), null, BindingMode.TwoWay, null, propertyChanged: OnSelectedItemChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoComplete), new DataTemplate(typeof(AutocompleteDefaultTemplate)), BindingMode.TwoWay, null, propertyChanged: OnItemTemplateChanged);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoComplete), default(string), BindingMode.TwoWay, null, propertyChanged: OnTextChanged);
        public static readonly BindableProperty IsPullToRefreshEnabledProperty = BindableProperty.Create(nameof(IsPullToRefreshEnabled), typeof(bool), typeof(AutoComplete), false, BindingMode.TwoWay, null, propertyChanged: OnIsPulltoRefreshEnabledChanged);
        public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(AutoComplete), false, BindingMode.TwoWay, null, propertyChanged: OnIsrefreshingChanged);
        public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(AutoComplete), null, BindingMode.TwoWay, null, propertyChanged: OnRefreshCommandChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoComplete), default(string), BindingMode.TwoWay, null, propertyChanged: OnPlaceholderChanged);
        public static readonly BindableProperty FocusEnableProperty = BindableProperty.Create(nameof(FocusEnabled), typeof(bool), typeof(AutoComplete), false, BindingMode.TwoWay, null, propertyChanged: OnFocusEnabledChanged);
        public static readonly BindableProperty ShowCountProperty = BindableProperty.Create(nameof(ShowCount), typeof(bool), typeof(AutoComplete), false, BindingMode.TwoWay, null, propertyChanged: OnShowCountChange);
        public static readonly BindableProperty CounterProperty = BindableProperty.Create(nameof(CounterProperty), typeof(int), typeof(AutoComplete), 0, BindingMode.TwoWay, null, propertyChanged: OnCounterChange);
        #endregion

        #region  Properties
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsPullToRefreshEnabled
        {
            get { return (bool)GetValue(IsPullToRefreshEnabledProperty); }
            set { SetValue(IsPullToRefreshEnabledProperty, value); }
        }

        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public bool FocusEnabled
        {
            get { return (bool)GetValue(FocusEnableProperty); }
            set { SetValue(FocusEnableProperty, value); }
        }

        public bool ShowCount
        {
            get => (bool)GetValue(ShowCountProperty);
            set { SetValue(ShowCountProperty, value); }
        }

        public int Counter
        {
            get => (int)GetValue(CounterProperty);
            set { SetValue(CounterProperty, value); }
        }

        private Entry SearchInput;
        private ListView ListView;
        private Label Label;
        private Span ItemsCount;

        #endregion

        #region Ctr
        public AutoComplete()
        {
            Initialize();
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }
        #endregion

        #region Methods
        void Initialize()
        {
            Content = DrawMainContainer();
            SearchInput.TextChanged += OnTextChanged;
            SearchInput.Completed += OnCompleted;
            ListView.ItemSelected += OnItemSelected;
        }

        StackLayout DrawMainContainer()
        {
            var container = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };
            Label = DrawCounter(ShowCount);
            SearchInput = DrawInputControl();
            ListView = DrawListView();
            container.Children.Add(Label);
            container.Children.Add(SearchInput);
            container.Children.Add(ListView);
            return container;
        }

        Label DrawCounter(bool showCount)
        {
            var counter = new Label { HorizontalOptions = LayoutOptions.EndAndExpand };
            var formattedSttring = new FormattedString();
            formattedSttring.Spans.Add(new Span { Text = "Items ", FontSize = 14 });
            ItemsCount = new Span { Text = $"({Counter})", FontSize = 14 };
            formattedSttring.Spans.Add(ItemsCount);
            counter.FormattedText = formattedSttring;
            counter.SetValue(IsVisibleProperty, showCount);
            return counter;
        }

        Entry DrawInputControl()
        {
            var input = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand };
            return input;
        }

        ListView DrawListView()
        {
            var listview = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                SeparatorVisibility = SeparatorVisibility.None,
                HasUnevenRows = true
            };
            return listview;
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.ItemsSource = (IEnumerable<object>)newValue;
                autocomplete.ListView.ItemsSource = autocomplete.ItemsSource;
            }
        }

        private static void OnSearchCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.SearchCommand = (ICommand)newValue;
            }
        }

        private static void OnSelectedItemCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.SelectedItemCommand = (ICommand)newValue;
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.SelectedItem = newValue;
                autocomplete.ListView.SelectedItem = autocomplete.SelectedItem;
            }
        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.Text = (string)newValue;
                autocomplete.SearchInput.Text = autocomplete.Text;
            }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.ItemTemplate = (DataTemplate)newValue;
                autocomplete.ListView.ItemTemplate = autocomplete.ItemTemplate;
            }
        }

        private static void OnIsPulltoRefreshEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.IsPullToRefreshEnabled = (bool)newValue;
                autocomplete.ListView.IsPullToRefreshEnabled = autocomplete.IsPullToRefreshEnabled;
            }
        }

        private static void OnIsrefreshingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.IsRefreshing = (bool)newValue;
                autocomplete.ListView.IsRefreshing = autocomplete.IsRefreshing;
            }
        }

        private static void OnRefreshCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.RefreshCommand = (ICommand)newValue;
                autocomplete.ListView.RefreshCommand = autocomplete.RefreshCommand;
            }
        }

        private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.Placeholder = (string)newValue;
                autocomplete.SearchInput.Placeholder = autocomplete.Placeholder;
            }
        }

        private static void OnFocusEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.FocusEnabled = (bool)newValue;
                if (autocomplete.FocusEnabled)
                    autocomplete.SearchInput.Focus();
            }
        }

        private static void OnShowCountChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                var value = (bool)newValue;
                autocomplete.ShowCount = value;
                autocomplete.Label.SetValue(IsVisibleProperty, value);
            }
        }

        private static void OnCounterChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autocomplete)
            {
                autocomplete.Counter = (int)newValue;
                autocomplete.ItemsCount.Text = $"({autocomplete.Counter})";
            }
        }

        void OnTextChanged(object _, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
            SearchCommand?.Execute(Text);
        }

        void OnItemSelected(object _, SelectedItemChangedEventArgs e)
        {
            SelectedItem = e.SelectedItem;
            SelectedItemCommand?.Execute(e.SelectedItem);
        }

        void OnCompleted(object _, EventArgs e)
        {
            SearchCommand?.Execute(e);
        }
        #endregion
    }
}