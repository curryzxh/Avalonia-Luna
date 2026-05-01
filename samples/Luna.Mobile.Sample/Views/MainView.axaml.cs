using Avalonia.Controls;
using Avalonia.Interactivity;
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
    }

    private void OnCatalogItemRequested(object? sender, CatalogItemViewModel item)
    {
        var view = CreateSampleView(item.Path);
        if (view is null)
        {
            return;
        }

        ShellHost.Page = new ContentPage
        {
            Header = item.Name,
            AutomaticallyApplySafeAreaPadding = true,
            Content = view,
        };

        BackButton.IsVisible = true;
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ShellHost.Page = CatalogPage;
        BackButton.IsVisible = false;
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
