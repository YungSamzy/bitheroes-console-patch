using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.language;
using frame8.Logic.Misc.Other.Extensions;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class FriendRequestViewTileVH : BaseVH
{
	private TextMeshProUGUI NameTxt;

	private Button button;

	public override void CollectViews()
	{
		base.CollectViews();
		button = root.GetComponent<Button>();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out NameTxt);
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(FriendRequestViewTileModel);
	}

	public override void UpdateViews(BaseModel model)
	{
		base.UpdateViews(model);
		FriendRequestViewTileModel _model = model as FriendRequestViewTileModel;
		if (_model.FriendRequestCount != "1")
		{
			NameTxt.text = Language.GetString("ui_friend_requests_count", new string[1] { _model.FriendRequestCount });
		}
		else
		{
			NameTxt.text = Language.GetString("ui_friend_request_count", new string[1] { _model.FriendRequestCount });
		}
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(delegate
		{
			OnItemClick(_model);
		});
	}

	public void OnItemClick(FriendRequestViewTileModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFriendRequestWindow();
	}

	private void DoClick()
	{
		GameData.instance.main.ShowLoading();
	}

	private void OnClick(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}
}
