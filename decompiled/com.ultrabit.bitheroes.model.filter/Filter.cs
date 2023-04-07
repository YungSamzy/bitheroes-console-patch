using System.Text.RegularExpressions;

namespace com.ultrabit.bitheroes.model.filter;

public static class Filter
{
	private static readonly string[] LETTERS = new string[26]
	{
		"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
		"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
		"U", "V", "W", "X", "Y", "Z"
	};

	public static string filter(string text)
	{
		RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
		for (int i = 0; i < FilterBook.words.Count; i++)
		{
			FilterRef filterRef = FilterBook.words[i];
			string word = filterRef.word;
			string replacement = filterRef.replacement;
			text = new Regex(word, options).Replace(text, replacement);
		}
		return text;
	}

	private static string getWord(string text, int index)
	{
		int wordBeginning = getWordBeginning(text, index);
		int wordEnd = getWordEnd(text, index);
		if (wordBeginning < 0 || wordEnd < 0)
		{
			return "";
		}
		return text.Substring(wordBeginning, wordEnd - wordBeginning);
	}

	private static int getWordBeginning(string text, int index)
	{
		int num = 0;
		while (index >= 0 && num < 1000)
		{
			if (index <= 0)
			{
				return 0;
			}
			if (!isValidLetter(text.Substring(index, 1)))
			{
				return index + 1;
			}
			index--;
			num++;
		}
		return -1;
	}

	private static int getWordEnd(string text, int index)
	{
		int num = 0;
		while (index < text.Length && num < 1000)
		{
			index++;
			if (text.Length > index && !isValidLetter(text.Substring(index, 1)))
			{
				return index;
			}
			num++;
		}
		return -1;
	}

	private static bool isValidLetter(string theString)
	{
		string[] lETTERS = LETTERS;
		foreach (string text in lETTERS)
		{
			if (theString.ToLowerInvariant() == text.ToLowerInvariant())
			{
				return true;
			}
		}
		return false;
	}

	public static bool allowArmory(string text)
	{
		for (int i = 0; i < FilterBook.words.Count; i++)
		{
			FilterRef filterRef = FilterBook.words[i];
			int num = text.ToLowerInvariant().IndexOf(filterRef.word.ToLowerInvariant());
			int num2 = filterRef.word.ToLowerInvariant().IndexOf(text.ToLowerInvariant());
			if (num >= 0 || num2 >= 0)
			{
				string word = getWord(text, num);
				string word2 = getWord(text, num2);
				if (!FilterBook.allowWord(word) || !FilterBook.allowWord(word2))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool allow(string text)
	{
		for (int i = 0; i < FilterBook.words.Count; i++)
		{
			FilterRef filterRef = FilterBook.words[i];
			int num = text.ToLowerInvariant().IndexOf(filterRef.word.ToLowerInvariant());
			if (num >= 0 && !FilterBook.allowWord(getWord(text, num)))
			{
				return false;
			}
		}
		return true;
	}

	public static bool allowedName(string text)
	{
		return FilterBook.allowName(text);
	}
}
