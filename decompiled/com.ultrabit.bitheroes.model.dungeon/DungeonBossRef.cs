using com.ultrabit.bitheroes.model.encounter;

namespace com.ultrabit.bitheroes.model.dungeon;

public class DungeonBossRef
{
	private EncounterRef _encounter;

	public EncounterRef encounter => _encounter;

	public DungeonBossRef(EncounterRef encounter)
	{
		_encounter = encounter;
	}
}
