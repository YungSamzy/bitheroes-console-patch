public class GridTile
{
	public int id;

	public int x;

	public int y;

	public int walkable;

	public int footsteps;

	public int f;

	public int g;

	public GridTile parent;

	public GridTile(int _id, int _x, int _y, int _walkable, int _foots)
	{
		id = _id;
		x = _x;
		y = _y;
		walkable = _walkable;
		footsteps = _foots;
	}
}
