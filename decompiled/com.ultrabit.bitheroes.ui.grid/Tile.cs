using com.ultrabit.bitheroes.model.sound;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.grid;

public class Tile
{
	private readonly string[] FOOTSTEP_COLORS = new string[6] { "#000000", "#919e45", "#f2cf89", "#65707f", "#5f492b", "#000000" };

	public int g;

	public int h;

	public int f;

	public Tile parentTile;

	public SpriteRenderer sprite;

	public TextMeshPro text;

	private int _id;

	private GridMap _grid;

	private int _xPos;

	private int _yPos;

	private bool _walkable;

	private bool _spawn;

	private Vector2? _offset;

	private int _x;

	private int _y;

	private SoundPoolRef _soundPoolRef;

	private int _footstepID;

	public int id => _id;

	public GridMap grid => _grid;

	public int xPos => _xPos;

	public int yPos => _yPos;

	public bool walkable => _walkable;

	public bool spawn => _spawn;

	public int footstepID => _footstepID;

	public SoundPoolRef soundPoolRef => _soundPoolRef;

	public Vector2? offset => _offset;

	public int x
	{
		get
		{
			return _x;
		}
		set
		{
			_x = value;
		}
	}

	public int y
	{
		get
		{
			return _y;
		}
		set
		{
			_y = value;
		}
	}

	public Tile(int id, GridMap grid, int xPos, int yPos, bool walkable = true, bool spawn = false, Vector2? offset = null)
	{
		_id = id;
		_grid = grid;
		_xPos = xPos;
		_yPos = yPos;
		SetWalkable(walkable);
		SetSpawn(spawn);
		SetOffset(offset);
	}

	public void SetWalkable(bool walkable)
	{
		_walkable = walkable;
		DrawDisplay();
	}

	public void SetSpawn(bool spawn)
	{
		_spawn = spawn;
		DrawDisplay();
	}

	public void SetFootstep(SoundPoolRef soundPoolRef)
	{
		_soundPoolRef = soundPoolRef;
		SetFootstepID(soundPoolRef.id);
	}

	public void SetOffset(Vector2? offset)
	{
		_offset = offset;
	}

	public void SetFootstepID(int id)
	{
		_footstepID = id;
		DrawDisplay();
	}

	public int NextFootstepID()
	{
		int result = ((_footstepID != 1) ? 1 : 0);
		SetFootstepID(result);
		return result;
	}

	private void DrawDisplay()
	{
		if (!(sprite == null))
		{
			sprite.sortingLayerName = "Overall";
			if (_spawn)
			{
				SetColor(new Color(0f, 1f, 0f));
				return;
			}
			Color color = (_walkable ? new Color(0f, 0f, 0f) : new Color(1f, 0f, 0f));
			SetColor(color);
		}
	}

	private void SetColor(Color color)
	{
		sprite.color = color;
	}

	public Vector2 GetPosition(GridObject obj)
	{
		return new Vector2((float)x + (_offset.HasValue ? _offset.Value.x : 0f), (float)y + (_offset.HasValue ? (_offset.Value.y * -1f) : 0f));
	}
}
