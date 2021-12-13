namespace Something.Core;

/// <summary>
/// A data structure to store the files, optimised for millions of unique files.
/// </summary>
public class FileStore
{
	private readonly HashSet<string> _store = new();

	public bool Add(string f) => _store.Add(f);

	public bool Remove(string f) => _store.Remove(f);

	/// <summary>
	/// SLOW! Store the result of this.
	/// </summary>
	public string[] Files
	{
		get
		{
			// faster than ToArray
			var working = new string[_store.Count];

			var i = 0;
			foreach (var s in _store) 
				working[i++] = s;

			return working;
		}
	}
}