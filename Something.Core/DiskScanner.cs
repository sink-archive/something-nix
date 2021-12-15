using System.ComponentModel;
using System.Diagnostics;

namespace Something.Core;

public static class DiskScanner
{
	public static void ScanToStore(DirectoryInfo dir, FileStore store, EventHandler<int>? handler = null)
	{
		try { FindScan(dir, store, handler); }
		catch (Win32Exception)
		{
			Console.WriteLine("Did not find `find` command, falling back to .NET scanner");
			NetScan(dir, store, handler);
		}
	}

	public static FileStore ScanToNewStore(DirectoryInfo dir, EventHandler<int>? handler = null)
	{
		var store = new FileStore();
		ScanToStore(dir, store, handler);
		return store;
	}

	/// <summary>
	/// Scans to store with the .NET IO APIs
	/// </summary>
	private static void NetScan(DirectoryInfo dir, FileStore store, EventHandler<int>? handler = null)
	{
		try
		{
			foreach (var file in dir.EnumerateFiles())
			{
				store.Add(file.FullName);
				if (store.Count % 1000 == 0) // dont event spam!
					handler?.Invoke(null, store.Count);
			}

			foreach (var subdir in dir.EnumerateDirectories())
				// If this blows the stack, use less nested folders on your system. Skill issue lmao.
				NetScan(subdir, store, handler);
		}
		catch (UnauthorizedAccessException) { }
	}

	/// <summary>
	/// Scans to store with an external command
	/// </summary>
	private static void FindScan(DirectoryInfo dir, FileStore store, EventHandler<int>? handler = null)
	{
		var processStartInfo = new ProcessStartInfo("find", $"{dir.FullName}/ -type f")
		{
			WorkingDirectory       = dir.FullName,
			RedirectStandardOutput = true
		};

		var process = Process.Start(processStartInfo);
		if (process == null) return;

		var stdout = process.StandardOutput;
		var count  = 0;
		while (!stdout.EndOfStream)
		{
			var line = stdout.ReadLine();
			if (line == null) continue;

			store.Add(line);
			count++;
		}

		process.WaitForExit();

		handler?.Invoke(null, count);
	}
}