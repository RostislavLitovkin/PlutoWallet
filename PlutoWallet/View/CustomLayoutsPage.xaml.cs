using Microsoft.Maui.Controls;
using PlutoWallet.Components.CustomLayouts;
using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class CustomLayoutsPage : ContentPage
{
    private CustomLayoutItemDragger selectedDragger;

    private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

    private float x;
    private float y;

    public CustomLayoutsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        topNavigationBar.ExtraFunc = OnExtraClicked;
    }

    async void PanGestureRecognizer_PanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            protectiveLayout.IsVisible = true;

            _positions = new Queue<(float, float)>();

            selectedDragger = (CustomLayoutItemDragger)verticalStackLayout.Children[draggerStackLayout.Children.IndexOf((IView)sender)];

            selectedDragger.ZIndex = 100;
        }

        if (e.StatusType == GestureStatus.Running)
        {
            _positions.Enqueue(((float)(e.TotalX), (float)(e.TotalY)));
            if (_positions.Count > 10)
                _positions.Dequeue();

            selectedDragger.TranslationY = _positions.Average(item => item.y);

            foreach (CustomLayoutItemDragger dragger in verticalStackLayout.Children)
            {
                if (dragger == selectedDragger)
                {
                    continue;
                }
                if (dragger.Y < selectedDragger.Y && dragger.Y + 30 > selectedDragger.Y + selectedDragger.TranslationY)
                {
                    dragger.TranslateTo(0, 65, 100);
                }
                else if (dragger.Y > selectedDragger.Y && dragger.Y - 30 < selectedDragger.Y + selectedDragger.TranslationY)
                {
                    dragger.TranslateTo(0, -65, 100);
                }
                else
                {
                    dragger.TranslateTo(0, 0, 100);
                }
            }
        }

        if (e.StatusType == GestureStatus.Completed)
        {
            int selectedIndex = verticalStackLayout.Children.IndexOf(selectedDragger);

            int index = selectedIndex;

            foreach (CustomLayoutItemDragger dragger in verticalStackLayout.Children)
            {
                if (dragger == selectedDragger)
                {
                    continue;
                }
                if (dragger.Y < selectedDragger.Y && dragger.Y + 30 > selectedDragger.Y + selectedDragger.TranslationY)
                {
                    index = verticalStackLayout.Children.IndexOf(dragger);

                    break;
                }
                else if (dragger.Y > selectedDragger.Y && dragger.Y - 30 < selectedDragger.Y + selectedDragger.TranslationY)
                {
                    index = verticalStackLayout.Children.IndexOf(dragger);
                }
            }

            await selectedDragger.TranslateTo(0, (index - selectedIndex) * 65, 500, Easing.CubicOut);

            selectedDragger.ZIndex = 0;
            selectedDragger = null;

            ((CustomLayoutsViewModel)this.BindingContext).SwapItems(selectedIndex, selectedIndex + (index - selectedIndex));

            protectiveLayout.IsVisible = false;
        }
    }

    public async Task OnExtraClicked()
    {
        var exportViewModel = DependencyService.Get<ExportPlutoLayoutQRViewModel>();

        exportViewModel.PlutoLayoutValue = Preferences.Get("PlutoLayout", Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT);
        exportViewModel.IsVisible = true;
    }

    void OnScrolled(System.Object sender, Microsoft.Maui.Controls.ScrolledEventArgs e)
    {
        draggerStackLayout.TranslationY = -((ScrollView)sender).ScrollY;
    }
}
