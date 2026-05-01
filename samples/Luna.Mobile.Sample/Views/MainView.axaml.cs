using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class MainView : UserControl
{
    private MainViewModel ViewModel { get; } = new();

    public MainView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        CatalogPage.DataContext = ViewModel;
        ViewModel.CatalogItemRequested += OnCatalogItemRequested;
        ShellNavigation.CurrentPageChanged += OnCurrentPageChanged;
        ShellNavigation.Pushed += OnNavigationStackChanged;
        ShellNavigation.Popped += OnNavigationStackChanged;
        ShellNavigation.PoppedToRoot += OnNavigationStackChanged;
        UpdateHeader();
    }

    private async void OnCatalogItemRequested(object? sender, CatalogItemViewModel item)
    {
        var page = CreateSamplePage(item);
        if (page is null)
        {
            return;
        }

        await ShellNavigation.PushAsync(page);
    }

    private async void BackButton_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ShellNavigation.CanGoBack)
        {
            await ShellNavigation.PopAsync();
        }
    }

    private void OnCurrentPageChanged(object? sender, System.EventArgs e)
    {
        UpdateHeader();
    }

    private void OnNavigationStackChanged(object? sender, NavigationEventArgs e)
    {
        UpdateHeader();
    }

    private void UpdateHeader()
    {
        var currentPage = ShellNavigation.CurrentPage;
        var isCatalogPage = currentPage is null || ReferenceEquals(currentPage, CatalogPage);
        var canGoBack = ShellNavigation.NavigationStack.Count > 1;

        HeaderBar.IsVisible = !isCatalogPage;
        BackButton.IsVisible = !isCatalogPage && canGoBack;
        HeaderTitleText.Text = isCatalogPage ? string.Empty : GetHeaderText(currentPage?.Header);
    }

    private static string GetHeaderText(object? header)
    {
        return header switch
        {
            null => string.Empty,
            string text => text,
            TextBlock textBlock => textBlock.Text ?? string.Empty,
            _ => header.ToString() ?? string.Empty,
        };
    }

    private static Page? CreateSamplePage(CatalogItemViewModel item)
    {
        var view = CreateSampleView(item.Path);
        if (view is null)
        {
            return null;
        }

        if (view is Page page)
        {
            NavigationPage.SetHasNavigationBar(page, false);
            NavigationPage.SetHasBackButton(page, false);

            if (page.Header is null || page.Header is string { Length: 0 })
            {
                page.Header = item.Name;
            }

            return page;
        }

        var wrappedPage = new ContentPage
        {
            Header = item.Name,
            AutomaticallyApplySafeAreaPadding = true,
            Content = view,
        };

        NavigationPage.SetHasNavigationBar(wrappedPage, false);
        NavigationPage.SetHasBackButton(wrappedPage, false);
        return wrappedPage;
    }

    private static Control? CreateSampleView(string path)
    {
        return path switch
        {
            "/actionsheet" => new ActionSheetDemoView(),
            "/avatar" => new AvatarDemoView(),
            "/button" => new ButtonDemoView(),
            "/badge" => new BadgeDemoView(),
            "/cell" => new CellDemoView(),
            "/checkbox" => new CheckBoxDemoView(),
            "/datetimepicker" => new DateTimePickerDemoView(),
            "/drawer" => new DrawerDemoView(),
            "/popover" => new PopoverDemoView(),
            "/divider" => new DividerDemoView(),
            "/empty" => new EmptyDemoView(),
            "/imageviewer" => new ImageViewerDemoView(),
            "/indexes" => new IndexesDemoView(),
            "/input" => new InputDemoView(),
            "/dialog" => new DialogDemoView(),
            "/loading" => new LoadingDemoView(),
            "/message" => new MessageDemoView(),
            "/noticebar" => new NoticeBarDemoView(),
            "/overlay" => new OverlayDemoView(),
            "/picker" => new PickerDemoView(),
            "/radio" => new RadioDemoView(),
            "/rate" => new RateDemoView(),
            "/search" => new SearchDemoView(),
            "/stepper" => new StepperDemoView(),
            "/switch" => new SwitchDemoView(),
            "/tag" => new TagDemoView(),
            "/toast" => new ToastDemoView(),
            _ => null,
        };
    }
}
