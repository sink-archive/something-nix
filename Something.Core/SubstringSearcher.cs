using System.Diagnostics.CodeAnalysis;

namespace Something.Core;

// DO NOT USE LINQ ITS REALLY SLOW
[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public static class SubstringSearcher
{
	private static int CountMatches(string pattern, string item)
	{
		var upperBound = item.Length - pattern.Length;
		var matches    = 0;
		for (var i = 0; i <= upperBound; i++)
		{
			var match = true;
			for (var j = 0; j < pattern.Length; j++)
			{
				if (pattern[j] == item[i + j])
					continue;
				
				match = false;
				break;
			}

			if (match) matches++;
		}

		return matches;
	}

	public static string[] Search(string pattern, string[] items)
	{
		if (string.IsNullOrWhiteSpace(pattern)) return items;

		var lcPattern = pattern.ToLower();
		
		var working = new List<string>();
		for (var i = 0; i < items.Length; i++)
		{
			var matches = CountMatches(pattern, items[i].ToLower());
			if (matches > 0)
				working.Add(items[i]);
		}

		var arr = new string[working.Count];
		for (var i = 0; i < working.Count; i++)
			arr[i] = working[i];

		return arr;
	}
}