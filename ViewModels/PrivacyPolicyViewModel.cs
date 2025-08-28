using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using CredibillMauiApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace CredibillMauiApp.ViewModels
{
    public class PrivacyPolicyViewModel : BaseViewModel
    {
        private readonly DatabaseService? _dbService;
        public ICommand? DeleteDataCommand { get; }

        public PrivacyPolicyViewModel() {
            // For XAML instantiation, no DB service available
            DeleteDataCommand = new AsyncRelayCommand(() => Task.CompletedTask);
        }

        public PrivacyPolicyViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            DeleteDataCommand = new AsyncRelayCommand(DeleteDataAsync);
        }

        private async Task DeleteDataAsync()
        {
            if (_dbService != null)
                await _dbService.DeleteAllDataAsync();
            SecureStorage.RemoveAll();
        }
    }
}
