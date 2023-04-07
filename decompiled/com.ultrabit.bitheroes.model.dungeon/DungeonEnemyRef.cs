using com.ultrabit.bitheroes.model.encounter;

namespace com.ultrabit.bitheroes.model.dungeon;

public class DungeonEnemyRef
{
	private int _id;

	private EncounterRef _encounter;

	public int id => _id;

	public EncounterRef encounter => _encounter;

	public DungeonEnemyRef(int id, EncounterRef encounter)
	{
		_id = id;
		_encounter = encounter;
	}
}
