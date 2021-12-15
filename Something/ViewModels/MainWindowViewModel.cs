using System;
using ReactiveUI;
using Something.Core;

namespace Something.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private StoreListener? _fileStore;

		public StoreListener? FileStore
		{
			get => _fileStore;
			set
			{
				// this double-raises FileStore but its really not an issue
				this.RaiseAndSetIfChanged(ref _fileStore, value);
				RaiseFileListChanged();
			}
		}

		public string[] SafeFiles => SubstringSearcher.Search(SearchTerm, _fileStore?.Store.Files ?? Array.Empty<string>());

		public void RaiseFileListChanged()
		{
			this.RaisePropertyChanged(nameof(FileStore));
			this.RaisePropertyChanged(nameof(SafeFiles));
		}

		private string _searchTerm = "";

		public string SearchTerm
		{
			get => _searchTerm;
			set
			{
				this.RaiseAndSetIfChanged(ref _searchTerm, value);
				RaiseFileListChanged();
			}
		}

		private bool _scanning;

		public bool Scanning
		{
			get => _scanning;
			set => this.RaiseAndSetIfChanged(ref _scanning, value);
		}
	}
}