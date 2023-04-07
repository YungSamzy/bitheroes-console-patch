using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.invasion;

public class InvasionEventLevelRef
{
	private int _id;

	private long _points;

	private List<GameModifier> _modifiers;

	public int id => _id;

	public long points => _points;

	public List<GameModifier> modifiers => _modifiers;

	public InvasionEventLevelRef(BaseEventBookData.Level data)
	{
		_id = data.id;
		_points = data.points;
		_modifiers = GameModifier.GetGameModifierFromData(null, data.lstModifier);
	}
}
