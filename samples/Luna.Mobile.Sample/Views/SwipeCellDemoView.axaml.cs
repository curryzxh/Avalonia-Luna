using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using Luna.Mobile.Sample.ViewModels;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.Views;

public partial class SwipeCellDemoView : UserControl
{
    private SwipeCellDemoViewModel ViewModel { get; } = new();

    public SwipeCellDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnFavoriteClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show("收藏成功");
    }

    private void OnEditClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show("编辑成功");
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show("删除");
    }

    private void OnConfirmDeleteClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show("删除成功");
        EventSwipeCell.Close();
    }
}
