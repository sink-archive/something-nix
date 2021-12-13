namespace something.Core;

public static class DiskScanner
{
	public static void ScanToStore(DirectoryInfo dir, FileStore store)
	{
		foreach (var file in dir.EnumerateFiles()) 
			store.Add(file.FullName);

		foreach (var subdir in dir.EnumerateDirectories())
			// If this blows the stack, use less nested folders on your system. Skill issue lmao.
			ScanToStore(subdir, store);
	}

	public static FileStore ScanToNewStore(DirectoryInfo dir)
	{
		var store = new FileStore();
		ScanToStore(dir, store);
		return store;
	}
}