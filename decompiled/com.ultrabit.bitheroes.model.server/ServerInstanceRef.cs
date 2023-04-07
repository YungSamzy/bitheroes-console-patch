namespace com.ultrabit.bitheroes.model.server;

public class ServerInstanceRef
{
	private string _id;

	private string _url;

	public string id => _id;

	public string url => _url;

	public ServerInstanceRef(string id, string url)
	{
		_id = id;
		_url = url;
	}
}
