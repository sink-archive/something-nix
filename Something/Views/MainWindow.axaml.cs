using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Something.Core;
using Something.ViewModels;

namespace Something.Views
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			DataContext = new MainWindowViewModel();
			
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif

			// this is slightly cancer, and for that i am sorry
			// for the UI to update correctly, we need to allow one render before starting scanning.
			// not my problem, AvaloniaUI's MVC implementation is the root cause of this.
			async void AfterFirstRender()
			{
				// this async delay is the secret sauce to wait for a render. It's also cancer.
				await Task.Delay(1);
				var listener = StoreInitializer.Initialize("/home/cain/Documents");

				listener.StoreUpdateEvent += (_, _) => ((MainWindowViewModel) DataContext!).RaiseFileListChanged();

				((MainWindowViewModel) DataContext!).FileStore = listener;
			};
			// don't await, just fire it off and let .NET do its thing
			// hence the constructor exits before this function completes, and a render can occur
			AfterFirstRender();
		}

		private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
	}
}