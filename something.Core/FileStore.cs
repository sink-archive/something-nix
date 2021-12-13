namespace something.Core;

/// <summary>
/// A data structure to store the files, optimised for millions of unique files.
/// </summary>
public class FileStore
{
	private readonly HashSet<string> _store = new();

	public bool Add(string f) => _store.Add(f);

	public bool Remove(string f) => _store.Remove(f);

	public string[] Files => _store.ToArray();
}