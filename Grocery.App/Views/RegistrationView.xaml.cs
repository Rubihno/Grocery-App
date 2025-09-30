using Grocery.App.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace Grocery.App.Views;

public partial class RegistratieView : ContentPage
{
	public RegistratieView(RegistrationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is RegistrationViewModel viewModel)
        {
            viewModel.IsPassword = !viewModel.IsPassword;
        }
    }
}