namespace com.ultrabit.bitheroes.model.chat;

public class ChatText
{
	private string _html;

	private string _text;

	public string html
	{
		get
		{
			if (_html == null)
			{
				return string.Empty;
			}
			return _html;
		}
	}

	public string text
	{
		get
		{
			if (_text == null)
			{
				return string.Empty;
			}
			return _text;
		}
	}

	public ChatText(string html = null, string text = null)
	{
		_html = html;
		_text = text;
	}
}
