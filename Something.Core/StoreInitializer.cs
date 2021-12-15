using System.Diagnostics;

namespace Something.Core;

public static class StoreInitializer
{
	public static StoreListener Initialize(string path)
	{
#if DEBUG
		var s        = Stopwatch.StartNew();
#endif
		var store    = DiskScanner.ScanToNewStore(new DirectoryInfo(path));
		var listener = new StoreListener(path, store);
#if DEBUG
		s.Stop();
		Console.WriteLine($"Found {listener.Store.Count} files in {s.ElapsedMilliseconds} ms");
#endif
		return listener;
	}

	public static StoreListener InitializeRoot() => Initialize("/");

	public static StoreListener InitializeHome()
		=> Initialize(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
}