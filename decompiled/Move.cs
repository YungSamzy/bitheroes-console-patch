using System.Collections.Generic;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.grid;
using UnityEngine;

public class Move : MonoBehaviour
{
	public List<Tile> _path = new List<Tile>();

	private float WRadius = 1f;

	public float speed = 30f;

	private Vector3 _de;

	private Vector3 _a;

	private bool walking;

	public NPC_Move targetNPC;

	public Vector3 offset = new Vector3(0f, 0f, 0f);

	public CharacterDisplay characterDisplay;

	private Vector2 _direction;

	public Tile currentTile;

	public void Awake()
	{
	}

	public void InitMovement()
	{
	}

	public void Path(List<Tile> p)
	{
		_path.Clear();
		_path.AddRange(p);
	}
}
