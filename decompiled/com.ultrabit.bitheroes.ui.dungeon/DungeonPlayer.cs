using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.grid;
using DG.Tweening;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonPlayer : GridObject
{
	private const float MOVEMENT_SPEED = 1.1f;

	private const float ENTITY_OFFSET = 15f;

	public DungeonExclamationPoint exclamationIcon;

	private int _row;

	private int _column;

	private int _charID;

	private List<DungeonEntity> _entities;

	private Dungeon _dungeon;

	private List<DungeonFollower> _followers = new List<DungeonFollower>();

	private List<DungeonNode> _footsteps = new List<DungeonNode>();

	private List<DungeonNode> _path = new List<DungeonNode>();

	public int charID => _charID;

	public List<DungeonEntity> entities => _entities;

	public List<DungeonNode> footsteps => _footsteps;

	public List<DungeonFollower> followers => _followers;

	public void Create(int charID, int row, int column, List<DungeonEntity> entities)
	{
		LoadDetails(null, null, 250f, clickable: false);
		_charID = charID;
		_entities = entities;
		_row = row;
		_column = column;
		SetExclamation();
		UpdateMovementSpeedMult();
	}

	public void SetDungeon(Dungeon dungeon)
	{
		_dungeon = dungeon;
		DungeonNode nodeByPosition = _dungeon.GetNodeByPosition(_column, _row);
		SetGrid(_dungeon);
		SetTile(nodeByPosition.GetTile(), tween: false);
		CreateFollowers();
		UpdateFormation();
	}

	public override void SetPath(List<Tile> path)
	{
		UpdateMovementSpeedMult();
		base.SetPath(path);
	}

	public void UpdateMovementSpeedMult()
	{
		if (_charID != GameData.instance.PROJECT.character.id)
		{
			return;
		}
		float num = 1f + GameData.instance.PROJECT.character.getGameModifierValueTotal(9);
		SetSpeedMult(num);
		foreach (DungeonFollower follower in _followers)
		{
			follower.SetSpeedMult(num);
		}
	}

	private void CreateFollowers()
	{
		for (int i = 0; i < _entities.Count; i++)
		{
			if (i != 0)
			{
				GridObject target = ((i == 1) ? ((GridObject)this) : ((GridObject)_followers[i - 2]));
				DungeonFollower dungeonFollower = new GameObject
				{
					name = "DungeonFollower" + i
				}.AddComponent<DungeonFollower>();
				dungeonFollower.Create(_dungeon, base.tile, target, _followers.Count);
				_followers.Add(dungeonFollower);
				_dungeon.AddObject(dungeonFollower);
				_dungeon.AddDungeonObject(dungeonFollower);
			}
		}
	}

	public void MoveFollowersToDungeonScene()
	{
		foreach (DungeonFollower follower in _followers)
		{
			SceneManager.MoveGameObjectToScene(follower.gameObject, _dungeon.dungeonScene);
		}
	}

	public void ShowFollowers(bool enabled)
	{
		foreach (DungeonFollower follower in _followers)
		{
			follower.gameObject.SetActive(enabled);
		}
	}

	public override void SetExclamation(bool enabled = false)
	{
		base.SetExclamation(enabled);
		if (enabled)
		{
			exclamationIcon.gameObject.SetActive(value: true);
			exclamationIcon.DoFlash();
			exclamationIcon.ZoomIn(HideExclamation);
		}
		else
		{
			RemoveExclamation();
		}
	}

	private void HideExclamation()
	{
		float num = 1f;
		float num2 = 1f;
		if (AppInfo.TESTING)
		{
			num /= 3f;
			num2 /= 3f;
		}
		exclamationIcon.ChangeAlpha(null, 0f, num2, Ease.Unset, num, RemoveExclamation);
	}

	private void RemoveExclamation()
	{
		exclamationIcon.gameObject.SetActive(value: false);
	}

	public void UpdateFormation()
	{
		for (int i = 0; i < _entities.Count; i++)
		{
			DungeonEntity dungeonEntity = _entities[i];
			GridObject gridObject = ((i == 0) ? ((GridObject)this) : ((GridObject)_followers[i - 1]));
			gridObject.SetAsset(dungeonEntity.getAsset(2f, gridObject.transform));
			_ = gridObject.asset != null;
		}
	}

	public DungeonEntity GetEntity(int index)
	{
		foreach (DungeonEntity entity in _entities)
		{
			if (entity.index == index)
			{
				return entity;
			}
		}
		return null;
	}

	public void SetEntityOrder(int[] order)
	{
		List<DungeonEntity> list = new List<DungeonEntity>();
		foreach (int index in order)
		{
			list.Add(GetEntity(index));
		}
		_entities.Clear();
		foreach (DungeonEntity item in list)
		{
			_entities.Add(item);
		}
		UpdateFormation();
	}

	public void ClearFootsteps()
	{
		if (_footsteps != null)
		{
			_footsteps.Clear();
		}
	}

	public void AddFootstep(DungeonNode node)
	{
		if (_footsteps.Count <= 0 || !(_footsteps[_footsteps.Count - 1] == node))
		{
			_footsteps.Add(node);
		}
	}

	public bool HasFootstep(DungeonNode node)
	{
		foreach (DungeonNode footstep in _footsteps)
		{
			if (footstep == node)
			{
				return true;
			}
		}
		return false;
	}

	public Vector2 GetNodeTargetPoint(DungeonNode node, Vector2? nullableOffset = null)
	{
		int num = 90;
		Vector2 vector = (nullableOffset.HasValue ? nullableOffset.Value : new Vector2(0f, 0f));
		if (vector.x > (float)num && !node.right)
		{
			vector.x = num;
		}
		if (vector.x < (float)(-num) && !node.left)
		{
			vector.x = -num;
		}
		if (vector.y > (float)num && !node.down)
		{
			vector.y = num;
		}
		if (vector.y < (float)(-num) && !node.up)
		{
			vector.y = -num;
		}
		if (vector.x > (float)num && vector.y < (float)(-num))
		{
			DungeonNode node2 = _dungeon.GetNode(node.row - 1, node.column + 1);
			if (!node.AreCornersConnected(_dungeon.GetNode(node.row - 1, node.column), node2, node, _dungeon.GetNode(node.row, node.column + 1)))
			{
				if (base.x <= node.x + (float)num)
				{
					vector.x = num;
				}
				else
				{
					vector.y = -num;
				}
			}
		}
		if (vector.x < (float)(-num) && vector.y < (float)(-num))
		{
			DungeonNode node3 = _dungeon.GetNode(node.row - 1, node.column - 1);
			if (!node.AreCornersConnected(node3, _dungeon.GetNode(node.row - 1, node.column), _dungeon.GetNode(node.row, node.column - 1), node))
			{
				if (base.x >= node.x - (float)num)
				{
					vector.x = -num;
				}
				else
				{
					vector.y = -num;
				}
			}
		}
		if (vector.x > (float)num && vector.y > (float)num)
		{
			DungeonNode node4 = _dungeon.GetNode(node.row + 1, node.column + 1);
			if (!node.AreCornersConnected(node, _dungeon.GetNode(node.row, node.column + 1), _dungeon.GetNode(node.row + 1, node.column), node4))
			{
				if (base.x <= node.x + (float)num)
				{
					vector.x = num;
				}
				else
				{
					vector.y = num;
				}
			}
		}
		if (vector.x < (float)(-num) && vector.y > (float)num)
		{
			DungeonNode node5 = _dungeon.GetNode(node.row + 1, node.column - 1);
			if (!node.AreCornersConnected(_dungeon.GetNode(node.row, node.column - 1), node, node5, _dungeon.GetNode(node.row + 1, node.column)))
			{
				if (base.x >= node.x - (float)num)
				{
					vector.x = -num;
				}
				else
				{
					vector.y = num;
				}
			}
		}
		return new Vector2(node.x + vector.x, node.y - vector.y);
	}

	public bool IsDead()
	{
		foreach (DungeonEntity entity in _entities)
		{
			if (entity.IsAlive())
			{
				return false;
			}
		}
		return true;
	}

	public bool IsAlive()
	{
		return !IsDead();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	public static DungeonPlayer FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		int int2 = sfsob.GetInt("dun9");
		int int3 = sfsob.GetInt("dun10");
		List<DungeonEntity> list = new List<DungeonEntity>();
		ISFSArray sFSArray = sfsob.GetSFSArray("dun17");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(DungeonEntity.FromSFSObject(sFSObject));
		}
		DungeonPlayer dungeonPlayer = Object.Instantiate(Dungeon.instance.dungeonPlayerPrefab);
		dungeonPlayer.Create(@int, int2, int3, list);
		return dungeonPlayer;
	}

	public void AddShrinePrompts(int entityIndex, string color, string text)
	{
		_ = _entities[entityIndex];
		GridObject gridObject = ((entityIndex == 0) ? ((GridObject)this) : ((GridObject)_followers[entityIndex - 1]));
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextFix"));
		obj.SetParent(_dungeon.transform, worldPositionStays: false);
		obj.GetComponent<TextMeshPro>().fontSize = 200f;
		obj.GetComponent<BattleText>().LoadDetails(text, color, 3f, 0f, gridObject.x, gridObject.y - 0f);
	}

	public bool CheckTeamFullHealth()
	{
		int num = 0;
		for (int i = 0; i < _entities.Count; i++)
		{
			DungeonEntity dungeonEntity = _entities[i];
			if (dungeonEntity.healthCurrent == dungeonEntity.healthTotal)
			{
				num++;
			}
		}
		return num == _entities.Count;
	}

	public bool CheckTeamFullSP()
	{
		int num = 0;
		for (int i = 0; i < _entities.Count; i++)
		{
			if (_entities[i].meter == VariableBook.battleMeterMax)
			{
				num++;
			}
		}
		return num == _entities.Count;
	}
}
