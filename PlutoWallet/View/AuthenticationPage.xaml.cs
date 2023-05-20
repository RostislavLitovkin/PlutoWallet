namespace PlutoWallet.View;

public partial class AuthenticationPage : ContentPage
{
	public AuthenticationPage()
	{
		InitializeComponent();
	}

    async void OnAuthenticateClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new Uri("https://mysite.com/mobileauth/Microsoft"),
                new Uri("myapp://"));

            string accessToken = authResult?.AccessToken;

            valueLabel.Text = "Value: " + accessToken;
            // Do something with the token
        }
        catch (TaskCanceledException ex)
        {
            // Use stopped auth
        }
    }
}
