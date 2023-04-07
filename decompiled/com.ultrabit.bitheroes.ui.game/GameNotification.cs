using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.game;

public class GameNotification : MonoBehaviour
{
	public const int TYPE_GLOBAL_MESSAGE = 0;

	public const int TYPE_PLAYER_LOGIN = 1;

	public const int TYPE_FRIEND_REQUEST = 2;

	public const int TYPE_FRIEND_ACCEPT = 3;

	public const int TYPE_ITEM_OBTAIN = 4;

	public const int TYPE_PVP_EVENT_WIN = 5;

	public const int TYPE_PVP_EVENT_LOSE = 6;

	public const int TYPE_GUILD_LEVEL = 7;

	public const int TYPE_GUILD_INVITE = 8;

	public const int TYPE_GUILD_PLAYER_ADDED = 9;

	public const int TYPE_GUILD_PLAYER_REMOVED = 10;

	public const int TYPE_GUILD_JOIN = 11;

	public const int TYPE_GUILD_LEAVE = 12;

	public const int TYPE_DAILY_QUEST_COMPLETE = 13;

	public const int TYPE_DAILY_QUEST_NEW = 14;

	public const int TYPE_PVP_DUEL_TARGET = 15;

	public const int TYPE_PVP_DUEL_SOURCE = 16;

	public const int TYPE_ITEM_ADDED = 17;

	public const int TYPE_ITEM_REMOVED = 18;

	public const int TYPE_BRAWL_INVITE = 19;

	public const int TYPE_CHARACTER_ACHIEVEMENT_COMPLETED = 20;

	private const float ANIMATION_SPEED = 0.5f;

	private const float ANIMATION_DELAY = 0.03f;

	private const float OFFSET_Y = 90f;

	private const float SCALE_TEXT_DELAY = 0.3f;

	public TextMeshProUGUI contentTxt;

	public Image bg;

	public Button leftBtn;

	public Button middleBtn;

	public Button rightBtn;

	private RectTransform rectTransform;

	public TextContainer txtcont;

	private int _yPos;

	private Image _bitmapContainer;

	private int _charID;

	private int _type;

	private object _data;

	private int _selectID;

	private Image _selectSprite;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private Color alpha0 = new Color(1f, 1f, 1f, 0f);

	private Vector3 defaultTxtScale;

	private int i;

	private bool usableStage;

	private List<GameNotificationObj> notifications = new List<GameNotificationObj>();

	private float showDelay = 3f;

	private bool first = true;

	private TextMeshProUGUI leftBtnTxt;

	private TextMeshProUGUI middleBtnTxt;

	private TextMeshProUGUI rightBtnTxt;

	private EventTrigger contentTrigger;

	private Vector2 contentTxtScale;

	public RectTransform contentFitter;

	private Coroutine _endTimer;

	public UnityEvent ON_NOTIFICATION_CLOSE = new UnityEvent();

	private UnityAction _closeNotificationByGuildRequest;

	private void Start()
	{
		leftBtnTxt = leftBtn.GetComponentInChildren<TextMeshProUGUI>();
		middleBtnTxt = middleBtn.GetComponentInChildren<TextMeshProUGUI>();
		rightBtnTxt = rightBtn.GetComponentInChildren<TextMeshProUGUI>();
		rectTransform = GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0f, 90f);
		contentTrigger = contentTxt.gameObject.GetComponent<EventTrigger>();
		defaultTxtScale = contentTxt.rectTransform.localScale;
		Disable();
		AlphaAll(isIn: false, 0f);
		Hide();
	}

	public void OnContentClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_selectID > 0)
		{
			GameData.instance.windowGenerator.ShowPlayer(_selectID);
		}
	}

	public void OnLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		switch (_type)
		{
		case 2:
			GameData.instance.PROJECT.DoAcceptRequest(_charID);
			break;
		case 8:
		{
			CharacterGuildData characterGuildData = _data as CharacterGuildData;
			GameData.instance.PROJECT.DoGuildInviteAccept(characterGuildData.id);
			break;
		}
		case 16:
			GameData.instance.PROJECT.DoDuelAccept(_charID);
			break;
		case 19:
		{
			List<string> stringVectorFromString = Util.getStringVectorFromString(_data as string);
			int index = int.Parse(stringVectorFromString[0]);
			stringVectorFromString.RemoveAt(0);
			string serverID = stringVectorFromString[0];
			stringVectorFromString.RemoveAt(0);
			DoBrawlJoinConfirm(index, serverID);
			break;
		}
		}
		ForceNotificationEnd();
	}

	public void OnMiddleBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		switch (_type)
		{
		case 2:
			GameData.instance.PROJECT.DoDenyRequest(_charID);
			break;
		case 8:
		{
			CharacterGuildData characterGuildData = _data as CharacterGuildData;
			GameData.instance.PROJECT.DoGuildInviteDecline(characterGuildData.id);
			break;
		}
		case 15:
			GameData.instance.PROJECT.DoDuelCancel(_charID);
			break;
		case 16:
			GameData.instance.PROJECT.DoDuelDecline(_charID);
			break;
		}
		ForceNotificationEnd();
	}

	public void OnRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int type = _type;
		if (type == 16 || type == 19)
		{
			CharacterProfileWindow characterProfileWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(CharacterProfileWindow)) as CharacterProfileWindow;
			if (!(characterProfileWindow != null) || characterProfileWindow.GetCharacterData().charID != _charID)
			{
				GameData.instance.windowGenerator.ShowPlayer(_charID, 9);
			}
		}
		else
		{
			ForceNotificationEnd();
		}
	}

	public void DoBrawlJoinConfirm(int index, string serverID)
	{
		if (serverID != ServerExtension.instance.serverInstanceID)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(GameData.instance.windowGenerator.GetErrorName(), Language.GetString("ui_server_confirm", new string[1] { Language.GetString("ui_join") }), null, null, delegate
			{
				OnBrawlJoinServerConfirm();
			}, null, null, 10);
		}
		else
		{
			DoBrawlJoin(index);
		}
	}

	private void OnBrawlJoinServerConfirm()
	{
		List<string> stringVectorFromString = Util.getStringVectorFromString(_data as string);
		int brawlIndex = int.Parse(stringVectorFromString[0]);
		stringVectorFromString.RemoveAt(0);
		string serverInstanceID = stringVectorFromString[0];
		stringVectorFromString.RemoveAt(0);
		ServerExtension.instance.Disconnect(null, null, relog: true, serverInstanceID, 0, brawlIndex);
	}

	private void DoBrawlJoin(int index)
	{
		GameData.instance.PROJECT.DoBrawlJoin(index, invited: true);
		RemoveNotificationType(19);
	}

	public void AddToQueue(int charID, string name, int type, object data = null)
	{
		switch (type)
		{
		case 4:
		{
			if (GameData.instance.SAVE_STATE.notificationsDisabled)
			{
				return;
			}
			bool num2 = GameData.instance.PROJECT.character.getFriendData(charID) != null;
			bool flag3 = GameData.instance.PROJECT.character.guildData != null && GameData.instance.PROJECT.character.guildData.getMember(charID) != null;
			bool flag4 = !num2 && !flag3;
			if ((num2 && !GameData.instance.SAVE_STATE.notificationsFriend) || (flag3 && !GameData.instance.SAVE_STATE.notificationsGuild) || (flag4 && !GameData.instance.SAVE_STATE.notificationsOther))
			{
				return;
			}
			break;
		}
		case 19:
		{
			if (!GameData.instance.SAVE_STATE.brawlRequests)
			{
				return;
			}
			bool num = GameData.instance.PROJECT.character.getFriendData(charID) != null;
			bool flag = GameData.instance.PROJECT.character.guildData != null && GameData.instance.PROJECT.character.guildData.getMember(charID) != null;
			bool flag2 = !num && !flag;
			if ((num && !GameData.instance.SAVE_STATE.brawlRequestsFriend) || (flag && !GameData.instance.SAVE_STATE.brawlRequestsGuild) || (flag2 && !GameData.instance.SAVE_STATE.brawlRequestsOther))
			{
				return;
			}
			break;
		}
		default:
			if (GameData.instance.SAVE_STATE.notificationsDisabled)
			{
				return;
			}
			break;
		case 0:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
			break;
		}
		string text = Util.ParseString("^" + name + "^");
		string text2 = text + " ";
		bool flag5 = false;
		float num3 = 1f;
		int result = 0;
		switch (type)
		{
		case 1:
			text2 = Language.GetString("notification_player_login", new string[1] { text });
			break;
		case 2:
			flag5 = true;
			text2 = Language.GetString("notification_friend_request", new string[1] { text });
			break;
		case 3:
			text2 = Language.GetString("notification_friend_accept", new string[1] { text });
			break;
		case 4:
		{
			Vector2 vector = Util.pointFromString(data as string);
			ItemRef itemRef = ItemBook.Lookup((int)vector.x, (int)vector.y);
			num3 = itemRef.rarityRef.notifyDuration;
			text2 = Language.GetString("notification_item_obtain", new string[2] { text, itemRef.coloredName });
			break;
		}
		case 0:
			if (data != null && data.GetType() == typeof(string))
			{
				text2 = Language.GetString(data as string, new string[1] { text });
			}
			break;
		case 5:
			if (!int.TryParse(data as string, out result))
			{
				return;
			}
			text2 = Language.GetString("notification_pvp_event_win", new string[2]
			{
				text,
				Util.colorString(Util.NumberFormat(result), "#00FF00")
			});
			break;
		case 6:
			if (!int.TryParse(data as string, out result))
			{
				return;
			}
			text2 = Language.GetString("notification_pvp_event_lose", new string[2]
			{
				text,
				Util.colorString(Util.NumberFormat(result * -1), "#FF0000")
			});
			break;
		case 7:
			if (!int.TryParse(data as string, out result))
			{
				return;
			}
			text2 = Language.GetString("notification_guild_level", new string[1] { Util.NumberFormat(result) }, color: true);
			break;
		case 8:
		{
			if (!VariableBook.GameRequirementMet(8))
			{
				return;
			}
			CharacterGuildData characterGuildData = CharacterGuildData.fromJsonString(data as string);
			flag5 = true;
			text2 = Language.GetString("notification_guild_invite", new string[2] { text, characterGuildData.name });
			data = characterGuildData;
			_closeNotificationByGuildRequest = delegate
			{
				CloseNotificationIfGuildRequestNoLongerExists(characterGuildData);
			};
			GameData.instance.PROJECT.character.AddListener("GUILD_INVITE_CHANGE", _closeNotificationByGuildRequest);
			ON_NOTIFICATION_CLOSE.AddListener(delegate
			{
				GameData.instance.PROJECT.character.RemoveListener("GUILD_INVITE_CHANGE", _closeNotificationByGuildRequest);
			});
			break;
		}
		case 9:
			text2 = Language.GetString("notification_guild_player_added", new string[1] { text });
			break;
		case 10:
			text2 = Language.GetString("notification_guild_player_removed", new string[1] { text });
			break;
		case 11:
			text2 = Language.GetString("notification_guild_join", new string[1] { data as string }, color: true);
			break;
		case 12:
			text2 = Language.GetString("notification_guild_leave");
			break;
		case 13:
			text2 = Language.GetString("notification_daily_quest_complete", new string[1] { name });
			break;
		case 20:
			text2 = Language.GetString("notification_character_achievement_complete", new string[1] { Util.ParseString("^" + Language.GetString(name) + "^") });
			break;
		case 14:
			text2 = Language.GetString("notification_daily_quest_new");
			break;
		case 15:
			flag5 = true;
			text2 = Language.GetString("notification_pvp_duel_target", new string[1] { text });
			break;
		case 16:
			flag5 = true;
			text2 = Language.GetString("notification_pvp_duel_source", new string[1] { text });
			break;
		case 17:
		{
			ItemData itemData2 = data as ItemData;
			text2 = Language.GetString("notification_item_added", new string[2]
			{
				Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(itemData2.qty) }),
				itemData2.itemRef.coloredName
			});
			break;
		}
		case 18:
		{
			ItemData itemData = data as ItemData;
			text2 = Language.GetString("notification_item_removed", new string[2]
			{
				Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(itemData.qty) }),
				itemData.itemRef.coloredName
			});
			break;
		}
		case 19:
			if (GameData.instance.PROJECT.dungeon != null || (bool)GameData.instance.PROJECT.battle || GameData.instance.windowGenerator.HasDialogByClass(typeof(BrawlRoomWindow)))
			{
				return;
			}
			flag5 = true;
			text2 = Language.GetString("notification_brawl_invite", new string[2]
			{
				text,
				Language.GetString("ui_brawl")
			});
			break;
		}
		text2 = Util.ParseString(text2);
		int num4 = ((!GameData.instance.windowGenerator.HasDialogOffset()) ? (-40) : 0);
		OnNotificationAnim(isIn: true, num4);
		float num5 = (flag5 ? 0f : ((float)text2.Length * 0.03f + 0.5f + 1.5f));
		num5 *= num3;
		notifications.Add(new GameNotificationObj(num5, charID, name, text2, type, data));
		if (notifications.Count == 1)
		{
			Show();
			ShowNotification(notifications[0]);
		}
	}

	private void CloseNotificationIfGuildRequestNoLongerExists(CharacterGuildData characterGuildData)
	{
		if (GameData.instance.PROJECT.character != null && !GameData.instance.PROJECT.character.GuildInviteExists(characterGuildData.id))
		{
			GameData.instance.PROJECT.notification.ForceNotificationEnd();
			GameData.instance.PROJECT.character.RemoveListener("GUILD_INVITE_CHANGE", _closeNotificationByGuildRequest);
		}
	}

	private void OnNotificationAnim(bool isIn, float offset = 0f)
	{
		if (!(rectTransform == null))
		{
			_ = (Vector3)rectTransform.anchoredPosition;
			Vector3 vector;
			if (isIn)
			{
				vector = new Vector3(0f, offset, 0f);
			}
			else
			{
				vector = new Vector3(0f, 90f, 0f);
				ScaleText(isIn);
			}
			rectTransform.DOAnchorPos(vector, 0.5f).SetEase(Ease.OutQuad);
			AlphaAll(isIn);
		}
	}

	private void ScaleText(bool isIn)
	{
		Color white;
		float duration;
		if (isIn)
		{
			white = Color.white;
			duration = 0.5f;
			contentTxtScale = defaultTxtScale;
			Vector2 vector = contentTxtScale;
			vector *= new Vector2(0.9f, 0.9f);
			_ = contentTxtScale * new Vector2(2f, 2f);
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, contentTxt.transform.DOScale(vector, 0.4f));
			sequence.Insert(0.4f, contentTxt.transform.DOScale(contentTxtScale, 0.1f));
			sequence.OnComplete(delegate
			{
				if (isIn && notifications[0].duration > 0f)
				{
					ClearTimer();
					_endTimer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, notifications[0].duration, CoroutineTimer.TYPE.SECONDS, OnNotificationEnd);
				}
			});
		}
		else
		{
			white = alpha0;
			duration = 0.2f;
		}
		contentTxt.DOColor(white, duration).SetEase(isIn ? Ease.InQuint : Ease.OutQuint);
	}

	public void AlphaAll(bool isIn = false, float duration = 0.5f)
	{
		Color endValue = ((!isIn) ? alpha0 : Color.white);
		Color currentColor = bg.color;
		bg.color = currentColor;
		leftBtn.image.color = currentColor;
		leftBtnTxt.color = currentColor;
		middleBtn.image.color = currentColor;
		middleBtnTxt.color = currentColor;
		rightBtn.image.color = currentColor;
		rightBtnTxt.color = currentColor;
		DOTween.To(() => currentColor, delegate(Color x)
		{
			currentColor = x;
		}, endValue, duration).SetEase(isIn ? Ease.InQuint : Ease.OutQuint).OnUpdate(delegate
		{
			bg.color = currentColor;
			leftBtn.image.color = currentColor;
			leftBtnTxt.color = currentColor;
			middleBtn.image.color = currentColor;
			middleBtnTxt.color = currentColor;
			rightBtn.image.color = currentColor;
			rightBtnTxt.color = currentColor;
		})
			.OnComplete(delegate
			{
				if (!isIn)
				{
					Hide();
				}
			});
	}

	private void ResizeBg(Vector2 endSize)
	{
		_ = bg.rectTransform.sizeDelta;
		if (endSize.x < 80f)
		{
			endSize.x = 80f;
		}
		if (endSize.x > 300f)
		{
			endSize.x = 300f;
		}
		bg.rectTransform.DOSizeDelta(endSize, 0.5f).SetEase(Ease.Linear);
	}

	private void OnNotificationEnd()
	{
		if (notifications != null && notifications.Count > 0)
		{
			notifications[0] = null;
			notifications.RemoveAt(0);
			if (notifications.Count <= 0)
			{
				Disable();
				OnNotificationAnim(isIn: false);
			}
			else
			{
				ShowNotification(notifications[0]);
			}
		}
	}

	public void ForceNotificationEnd()
	{
		if (notifications.Count > 0)
		{
			notifications[0] = null;
			notifications.RemoveAt(0);
		}
		if (notifications.Count == 0)
		{
			Disable();
			OnNotificationAnim(isIn: false);
		}
		else
		{
			ShowNotification(notifications[0]);
		}
	}

	public void RemoveNotificationType(int type)
	{
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < notifications.Count; i++)
			{
				if (notifications[i].type == type)
				{
					notifications.RemoveAt(i);
					flag = true;
					break;
				}
			}
		}
		while (flag);
		if (_type == type)
		{
			ForceNotificationEnd();
		}
	}

	private void ShowNotification(GameNotificationObj gameNotification)
	{
		if (rectTransform == null || gameNotification == null)
		{
			return;
		}
		if (!GameData.instance.windowGenerator.HasDialogOffset())
		{
			rectTransform.anchoredPosition = new Vector2(0f, -40f);
		}
		else
		{
			rectTransform.anchoredPosition = new Vector2(0f, 0f);
		}
		Enable();
		_charID = gameNotification.charID;
		_type = gameNotification.type;
		_data = gameNotification.data;
		if (contentTxt != null)
		{
			contentTxt.text = gameNotification.text;
		}
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(FixText());
		}
		ScaleText(isIn: true);
		switch (gameNotification.type)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 8:
		case 9:
		case 10:
		case 15:
		case 16:
			SetSelectID(gameNotification.charID);
			break;
		default:
			SetSelectID();
			break;
		}
		switch (gameNotification.type)
		{
		case 2:
		case 8:
			if (leftBtn != null)
			{
				leftBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
			}
			if (middleBtn != null)
			{
				middleBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
			}
			if (rightBtn != null)
			{
				rightBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ignore");
			}
			if (leftBtn != null)
			{
				leftBtn.gameObject.SetActive(value: true);
			}
			if (middleBtn != null)
			{
				middleBtn.gameObject.SetActive(value: true);
			}
			if (rightBtn != null)
			{
				rightBtn.gameObject.SetActive(value: true);
			}
			break;
		case 15:
			if (middleBtn != null)
			{
				middleBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cancel");
			}
			if (leftBtn != null)
			{
				leftBtn.gameObject.SetActive(value: false);
			}
			if (middleBtn != null)
			{
				middleBtn.gameObject.SetActive(value: true);
			}
			if (rightBtn != null)
			{
				rightBtn.gameObject.SetActive(value: false);
			}
			break;
		case 16:
		case 19:
			if (leftBtn != null)
			{
				leftBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
				leftBtn.gameObject.SetActive(value: true);
			}
			if (middleBtn != null)
			{
				middleBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
				middleBtn.gameObject.SetActive(value: true);
			}
			if (rightBtn != null)
			{
				rightBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
				rightBtn.gameObject.SetActive(value: true);
			}
			break;
		default:
			if (leftBtn != null)
			{
				leftBtn.gameObject.SetActive(value: false);
			}
			if (middleBtn != null)
			{
				middleBtn.gameObject.SetActive(value: false);
			}
			if (rightBtn != null)
			{
				rightBtn.gameObject.SetActive(value: false);
			}
			break;
		}
	}

	private IEnumerator FixText()
	{
		yield return new WaitForEndOfFrame();
		contentTxt.ForceMeshUpdate();
		if (contentTxt.text.Length > 30)
		{
			contentTxt.text = "<size=10>" + contentTxt.text + "</size>";
		}
		ResizeBg(new Vector2(contentTxt.rectTransform.sizeDelta.x + 33f, bg.rectTransform.sizeDelta.y));
	}

	private void SetSelectID(int charID = 0)
	{
		_selectID = charID;
		if (_selectID <= 0)
		{
			contentTxt.GetComponent<HoverImgTxtManual>().OnExit();
			contentTxt.GetComponent<HoverImgTxtManual>().active = false;
		}
		else
		{
			contentTxt.GetComponent<HoverImgTxtManual>().active = true;
		}
	}

	public void StopAllAnimations()
	{
	}

	private void ClearTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _endTimer);
	}

	public void OnDestroy()
	{
		ON_NOTIFICATION_CLOSE?.Invoke();
		ON_NOTIFICATION_CLOSE.RemoveAllListeners();
		StopAllAnimations();
	}

	public void Show()
	{
		bg.gameObject.SetActive(value: true);
		contentTxt.gameObject.SetActive(value: true);
		leftBtn.gameObject.SetActive(value: true);
		middleBtn.gameObject.SetActive(value: true);
		rightBtn.gameObject.SetActive(value: true);
		Main.CONTAINER.AddToLayer(base.gameObject, 9, front: false, center: false, resize: false);
		base.transform.localScale = new Vector3(3f, 3f, 3f);
	}

	public void Hide()
	{
		bg.gameObject.SetActive(value: false);
		contentTxt.gameObject.SetActive(value: false);
		leftBtn.gameObject.SetActive(value: false);
		middleBtn.gameObject.SetActive(value: false);
		rightBtn.gameObject.SetActive(value: false);
	}

	public void Enable()
	{
		leftBtn.interactable = true;
		middleBtn.interactable = true;
		rightBtn.interactable = true;
		if (contentTrigger != null && contentTrigger.gameObject.activeSelf)
		{
			contentTrigger.enabled = true;
		}
	}

	public void Disable()
	{
		leftBtn.interactable = false;
		middleBtn.interactable = false;
		rightBtn.interactable = false;
		if (contentTrigger != null && contentTrigger.gameObject.activeSelf)
		{
			contentTrigger.enabled = false;
		}
	}
}
