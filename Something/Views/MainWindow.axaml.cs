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
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif

			// this is slightly cancer, and for that i am sorry
			Task.Run(() =>
			{
				var listener = StoreInitializer.InitializeHome();
				var vm       = new MainWindowViewModel { FileStore = listener };

				listener.StoreUpdateEvent += (_, _) => vm.RaiseFileListChanged();

				DataContext = vm;
			});
		}

		private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
	}
}