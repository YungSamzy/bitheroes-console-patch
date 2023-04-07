using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleRequestEntry
{
	public string name;

	public string url;

	public Hash128 hash;

	public UnityWebRequest www;
}
