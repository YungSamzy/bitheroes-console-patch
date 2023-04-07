using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.fusion;
using TMPro;

namespace com.ultrabit.bitheroes.ui.fusion;

public class FusionResultWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public FusionTween fusionTween;

	private FusionRef _fusionRef;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(FusionRef fusionRef)
	{
		_fusionRef = fusionRef;
		topperTxt.text = fusionRef.tradeRef.resultItem.itemRef.coloredName;
		fusionTween.LoadDetails(fusionRef, OnFusionTweenComplete);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		fusionTween.SetAssetLayers(layer);
	}

	private void OnFusionTweenComplete()
	{
		GameData.instance.windowGenerator.NewFusionCompleteWindow(_fusionRef, fusionTween.resultAsset).DESTROYED.AddListener(OnCompleteWindowClosed);
		GameData.instance.PROJECT.character.updateAchievements();
	}

	private void OnCompleteWindowClosed(object e)
	{
		(e as WindowsMain).DESTROYED.RemoveListener(OnCompleteWindowClosed);
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
