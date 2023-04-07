using com.ultrabit.bitheroes.fromflash;

namespace com.ultrabit.bitheroes.parsing.model.utility;

public class TextFieldData
{
	private Point _position;

	private int _size;

	private Bitmap _bitmap;

	public Point position => _position;

	public int size => _size;

	public Bitmap bitmap => _bitmap;

	public TextFieldData(Point position, int size, Bitmap bitmap = null)
	{
		_position = position;
		_size = size;
		setBitmap(bitmap);
	}

	public void setBitmap(Bitmap bitmap)
	{
		_bitmap = bitmap;
	}

	public void clearBitmap()
	{
		if (_bitmap != null)
		{
			_bitmap.Dispose();
			_bitmap = null;
		}
	}
}
