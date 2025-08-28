using CommunityToolkit.Mvvm.ComponentModel;

namespace CredibillMauiApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string? errorMessage;
}