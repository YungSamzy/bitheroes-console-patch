using System.Collections.Generic;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.model.game;

public class GameModifierHelper
{
	public static List<GameModifier> GetGameModifierBase()
	{
		return VariableBook.modifiersBase;
	}

	public static List<GameModifier> GetGameModifierCharacterBase()
	{
		return VariableBook.characterBase.modifiers;
	}
}
