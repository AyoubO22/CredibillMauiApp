using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views
{
    public partial class PrivacyPolicyPage : ContentPage
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
            BindingContext = App.Services.GetService(typeof(CredibillMauiApp.ViewModels.PrivacyPolicyViewModel));
        }
    }
}
