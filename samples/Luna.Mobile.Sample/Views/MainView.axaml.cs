using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class MainView : UserControl
{
    private Control? _catalogContent;
    private MainViewModel ViewModel { get; } = new();

    public MainView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.CatalogItemRequested += OnCatalogItemRequested;
        _catalogContent = Content as Control;
    }

    private void OnCatalogItemRequested(object? sender, CatalogItemViewModel item)
    {
        Content = item.Path switch
        {
            "/actionsheet" => AttachBackHandler(new ActionSheetDemoView()),
            "/avatar" => AttachBackHandler(new AvatarDemoView()),
            "/button" => AttachBackHandler(new ButtonDemoView()),
            "/badge" => AttachBackHandler(new BadgeDemoView()),
            "/cell" => AttachBackHandler(new CellDemoView()),
            "/checkbox" => AttachBackHandler(new CheckBoxDemoView()),
            "/datetimepicker" => AttachBackHandler(new DateTimePickerDemoView()),
            "/drawer" => AttachBackHandler(new DrawerDemoView()),
            "/popover" => AttachBackHandler(new PopoverDemoView()),
            "/divider" => AttachBackHandler(new DividerDemoView()),
            "/empty" => AttachBackHandler(new EmptyDemoView()),
            "/imageviewer" => AttachBackHandler(new ImageViewerDemoView()),
            "/indexes" => AttachBackHandler(new IndexesDemoView()),
            "/input" => AttachBackHandler(new InputDemoView()),
            "/dialog" => AttachBackHandler(new DialogDemoView()),
            "/loading" => AttachBackHandler(new LoadingDemoView()),
            "/message" => AttachBackHandler(new MessageDemoView()),
            "/noticebar" => AttachBackHandler(new NoticeBarDemoView()),
            "/overlay" => AttachBackHandler(new OverlayDemoView()),
            "/picker" => AttachBackHandler(new PickerDemoView()),
            "/radio" => AttachBackHandler(new RadioDemoView()),
            "/rate" => AttachBackHandler(new RateDemoView()),
            "/search" => AttachBackHandler(new SearchDemoView()),
            "/stepper" => AttachBackHandler(new StepperDemoView()),
            "/switch" => AttachBackHandler(new SwitchDemoView()),
            "/tag" => AttachBackHandler(new TagDemoView()),
            "/toast" => AttachBackHandler(new ToastDemoView()),
            _ => _catalogContent,
        };
    }

    private UserControl AttachBackHandler(ButtonDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(SwitchDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(RadioDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(SearchDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(StepperDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(CheckBoxDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(DateTimePickerDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(DividerDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(EmptyDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(IndexesDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(InputDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(DialogDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(ToastDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(MessageDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(NoticeBarDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(OverlayDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(PickerDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(RateDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(LoadingDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(BadgeDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(TagDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(AvatarDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(ActionSheetDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(CellDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(DrawerDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(PopoverDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(ImageViewerDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }
}
