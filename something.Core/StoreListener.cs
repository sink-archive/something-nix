// this is without a doubt the most likely to break code i have ever written

namespace something.Core;

/// <summary>
/// Listens for changes in a file and updates the store as necessary.
/// </summary>
public class StoreListener : IDisposable
{
	public string RootDir => _fsWatcher.Path;

	private readonly FileSystemWatcher _fsWatcher;

	public StoreListener(string dir, FileStore store)
	{
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
		
		_fsWatcher.Created += (_, e) => store.Add(e.FullPath);
		_fsWatcher.Deleted += (_, e) => store.Remove(e.FullPath);
		_fsWatcher.Renamed += (_, e) =>
		{
			store.Remove(e.OldFullPath);
			store.Add(e.FullPath);
		};
	}

	public void Dispose() => _fsWatcher.Dispose();
}