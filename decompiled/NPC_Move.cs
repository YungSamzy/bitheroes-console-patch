using System.Collections;
using com.ultrabit.bitheroes.ui.grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPC_Move : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public enum OBJECT_TILES
	{
		craft = 1270,
		leaderboard = 2237,
		pvp = 1589,
		quest = 1073,
		shop = 2482,
		fusion = 520,
		familiarstable = 3359,
		rifts = 3329,
		gauntlet = 2118,
		fishing = 2636,
		enchants = 241,
		brawl = 3600,
		augments = 656,
		pvpstatuesItem = 1869,
		raidItem = 2627,
		dailyquestsItem = 1464,
		craftItem = 1493,
		fusionItem = 479,
		riftsItem = 3590,
		runeItem = 1620,
		guildItem = 407,
		fishingItem = 2900
	}

	private float WRadius = 0.1f;

	private float speed = 50f;

	private Vector3 _a;

	private bool walking;

	public OBJECT_TILES tileObject;

	private float distanceOpenMenu;

	private string movesValues;

	private int idTiles;

	private Tile nextTile;

	public Tile currentTile;

	public bool isNPC;

	public Vector3 offset;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private IEnumerator ChangePosition()
	{
		yield return new WaitForSeconds(Random.Range(0f, 2.8f));
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OpenMenuNPC()
	{
	}
}
