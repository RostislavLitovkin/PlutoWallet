using PlutoWallet.Model;

namespace PlutoWallet.Components.Card;

public partial class BottomPopupCard : AbsoluteLayout
{
    private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

    public BottomPopupCard()
	{
		InitializeComponent();
	}

    public Microsoft.Maui.Controls.View View { set { contentView.Content = value; } }

    private async void OnPanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            //protectiveLayout.IsVisible = true;

            _positions = new Queue<(float, float)>();
        }

        if (e.StatusType == GestureStatus.Running)
        {
            _positions.Enqueue(((float)(e.TotalX), (float)(e.TotalY)));
            if (_positions.Count > 10)
                _positions.Dequeue();

            float yAverage = _positions.Average(item => item.y);

            if (yAverage >= 0)
            {
                border.TranslationY = yAverage;
                dragger.TranslationY = yAverage;
                contentView.TranslationY = yAverage;
            }
        }

        if (e.StatusType == GestureStatus.Completed)
        {
            if (border.TranslationY < 50)
            {
                await Task.WhenAll(
                    border.TranslateTo(0, 0, 250, Easing.CubicOut),
                    dragger.TranslateTo(0, 0, 250, Easing.CubicOut),
                    contentView.TranslateTo(0, 0, 250, Easing.CubicOut)
                    );
            }
            else
            {
                await Task.WhenAll(
                    border.TranslateTo(0, border.Height, 250, Easing.CubicOut),
                    dragger.TranslateTo(0, border.Height, 250, Easing.CubicOut),
                    contentView.TranslateTo(0, border.Height, 250, Easing.CubicOut)
                );

                try
                {

                    // This is a great workaround.
                    // Most of the times, you will use this inside of a ContentView that has got a IPopup BindingContext.
                    // If not, then nothing will happen
                    ((IPopup)((ContentView)this.Parent).BindingContext).IsVisible = false;
                }
                catch
                {
                    
                }

                border.TranslationY = 0;
                dragger.TranslationY = 0;
                contentView.TranslationY = 0;
            }
        }
    }
}
