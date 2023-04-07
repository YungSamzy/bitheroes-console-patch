using System;

namespace com.ultrabit.bitheroes.ui.assets;

public class ImageAsset : Asset
{
	public bool _loaded;

	public bool _loading;

	public int _bmpData;

	public string _url;

	public new bool loaded
	{
		get
		{
			throw new Exception("Error --> CONTROL");
		}
		set
		{
			_loaded = value;
		}
	}

	public int bmpData
	{
		get
		{
			throw new Exception("Error --> CONTROL");
		}
		set
		{
			_bmpData = value;
		}
	}

	public bool loading
	{
		get
		{
			throw new Exception("Error --> CONTROL");
		}
		set
		{
			_loading = value;
		}
	}

	public string url
	{
		get
		{
			throw new Exception("Error --> CONTROL");
		}
		set
		{
			_url = value;
		}
	}

	public ImageAsset(string url, bool center = false, bool loadLocal = true, float scale = 1f)
	{
		throw new Exception("Error --> CONTROL");
	}

	public void createShape(int bmpData)
	{
		throw new Exception("Error --> CONTROL");
	}

	public void listenForComplete(ImageAsset imageAsset)
	{
		throw new Exception("Error --> CONTROL");
	}

	public void loadAsset(bool condition)
	{
		throw new Exception("Error --> CONTROL");
	}
}
