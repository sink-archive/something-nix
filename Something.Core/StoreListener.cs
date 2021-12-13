// this is without a doubt the most likely to break code i have ever written

namespace Something.Core;

/// <summary>
/// Listens for changes in a file and updates the store as necessary.
/// </summary>
public class StoreListener : IDisposable
{
	public string RootDir => _fsWatcher.Path;
	
	public FileStore Store { get; }

	private readonly FileSystemWatcher _fsWatcher;

	public StoreListener(string dir, FileStore store)
	{
		Store      = store;
		_fsWatcher = new FileSystemWatcher(dir);

		_fsWatcher.IncludeSubdirectories = true;
		// idk what the hell these do
		_fsWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName
								| NotifyFilters.FileName   | NotifyFilters.LastWrite    | NotifyFilters.Security;

		_fsWatcher.EnableRaisingEvents = true;

		_fsWatcher.Error += (_, e) =>
		{
			Dispose();
			throw new IOException("FileSystemWatcher errored, check InnerException", e.GetException());
		};
		
		// WOAH WHAT THIS ACTUALLY WORKS???
		_fsWatcher.Created += (_, e) =>
		{
			Store.Add(e.FullPath);
			StoreUpdateEvent.Invoke(this, e.FullPath);
		};
		_fsWatcher.Deleted += (_, e) =>
		{
			Store.Remove(e.FullPath);
			StoreUpdateEvent.Invoke(this, e.FullPath);
		};
		_fsWatcher.Renamed += (_, e) =>
		{
			Store.Remove(e.OldFullPath);
			Store.Add(e.FullPath);
			StoreUpdateEvent.Invoke(this, e.FullPath);
		};
	}

	// ReSharper disable once MemberInitializerValueIgnored
	public event EventHandler<string> StoreUpdateEvent = (_, _) => { };

	public void Dispose() => _fsWatcher.Dispose();
}