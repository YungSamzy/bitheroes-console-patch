using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.mount;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterMountTile : MonoBehaviour
{
	private Mounts _mounts;

	private bool _editable;

	private ItemIcon _itemIcon;

	private Button button;

	private MountSelectWindow _mountSelectWindow;

	public ItemRef itemRef => _itemIcon.itemRef;

	public ItemIcon itemIcon => _itemIcon;

	public void Create(Mounts mounts, bool editable = false, MountSelectWindow mountSelectWindow = null)
	{
		_mounts = mounts;
		_editable = editable;
		_mountSelectWindow = mountSelectWindow;
		DoUpdate();
	}

	public void OnButtonClick()
	{
		GameData.instance.windowGenerator.NewMountSelectWindow(GameData.instance.PROJECT.character.mounts, changeable: true, equippable: true, _mountSelectWindow.gameObject);
	}

	public void DoUpdate()
	{
		MountData mountEquipped = _mounts.getMountEquipped();
		MountRef mountRef = mountEquipped?.mountRef;
		MountRef mountRef2 = ((mountEquipped != null) ? _mounts.cosmetic : mountRef);
		if (mountRef == null)
		{
			if (_mounts.mounts.Count > 0)
			{
				_ = _editable;
			}
		}
		else
		{
			_ = mountRef2 != null;
		}
	}

	private void OnDestroy()
	{
		if (button != null)
		{
			button.onClick.RemoveListener(OnButtonClick);
		}
	}
}
