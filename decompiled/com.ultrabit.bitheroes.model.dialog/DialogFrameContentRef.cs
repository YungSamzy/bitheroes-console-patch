namespace com.ultrabit.bitheroes.model.dialog;

public class DialogFrameContentRef
{
	private int _index;

	private string _text;

	public int index => _index;

	public string text => _text;

	public DialogFrameContentRef(int index, string text)
	{
		_index = index;
		_text = text;
	}
}
