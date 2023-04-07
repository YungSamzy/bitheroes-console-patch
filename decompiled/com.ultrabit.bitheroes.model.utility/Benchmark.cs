using System.Collections.Generic;
using System.Diagnostics;

namespace com.ultrabit.bitheroes.model.utility;

public class Benchmark
{
	private static Dictionary<string, Stopwatch> _watchs = new Dictionary<string, Stopwatch>();

	public static void Start(string key)
	{
		if (_watchs.ContainsKey(key))
		{
			_watchs[key].Restart();
			return;
		}
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		_watchs.Add(key, stopwatch);
	}

	public static void Stop(string key)
	{
		if (_watchs.ContainsKey(key))
		{
			_watchs[key].Stop();
		}
		else
		{
			D.LogError("Benchmark::Tried to stop a non existant key: " + key);
		}
	}

	public static double Elapsed(string key, bool stop = false)
	{
		if (stop)
		{
			Stop(key);
		}
		if (_watchs.ContainsKey(key))
		{
			return _watchs[key].Elapsed.TotalSeconds;
		}
		D.LogError("Benchmark::Dictionary does not contain the key: " + key);
		return -1.0;
	}

	public static void Clear()
	{
		_watchs.Clear();
		_watchs = new Dictionary<string, Stopwatch>();
	}
}
