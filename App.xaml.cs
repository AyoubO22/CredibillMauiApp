using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace CredibillMauiApp;

public partial class App : Application
{
	public static IServiceProvider Services { get; private set; }

	public App()
	{
		InitializeComponent();
	Services = ((IPlatformApplication)Current).Services;
		// Do not set MainPage here, as it's deprecated.
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Initialize the application window with AppShell as the root page.
		return new Window(new AppShell());
	}
}