namespace Something.Core;

public static class StoreInitializer
{
	public static StoreListener Initialize(string path)
	{
		var store    = DiskScanner.ScanToNewStore(new DirectoryInfo(path));
		var listener = new StoreListener(path, store);
		return listener;
	}

	public static StoreListener InitializeRoot() => Initialize("/");

	public static StoreListener InitializeHome()
		=> Initialize(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
}