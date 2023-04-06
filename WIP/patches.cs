public void SetMeter(int meter)
		{
			if (this.attacker)
			{
				this._meter = VariableBook.battleMeterMax;
				float meterPerc = (float)this._meter / (float)VariableBook.battleMeterMax;
				this._overlay.SetMeterPerc(meterPerc);
				return;
			}
			this._meter = meter;
			float meterPerc2 = (float)this._meter / (float)VariableBook.battleMeterMax;
			this._overlay.SetMeterPerc(meterPerc2);
		}

using System;
using System.Threading;
using UnityEngine.UI;

namespace com.samzydev.mod
{
	// Token: 0x020009AF RID: 2479
	public class SDMod
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06005388 RID: 21384
		// (set) Token: 0x06005389 RID: 21385
		public static bool spmod { get; set; }

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x0600538A RID: 21386
		// (set) Token: 0x0600538B RID: 21387
		public static bool newpatchmod { get; set; }

		// Token: 0x0600538C RID: 21388
		public static void Start()
		{
			SDMod.spmod = false;
			SDMod.newpatchmod = false;
			new Thread(new ThreadStart(SDMod.ListerKeyBoardEvent)).Start();
		}

		// Token: 0x0600538D RID: 21389
		public static void ListerKeyBoardEvent()
		{
			for (;;)
			{
				if (Console.ReadKey(true).Key == ConsoleKey.F2)
				{
					if (!SDMod.spmod)
					{
						SDMod.spmod = true;
						Console.WriteLine("Max SP Enabled!");
					}
					else
					{
						SDMod.spmod = false;
						Console.WriteLine("Max SP Disabled!");
					}
				}
				if (Console.ReadKey(true).Key == ConsoleKey.F3)
				{
					if (!SDMod.newpatchmod)
					{
						SDMod.newpatchmod = true;
						Console.WriteLine("Patch Enabled!");
					}
					else
					{
						SDMod.newpatchmod = false;
						Console.WriteLine("Patch Disabled!");
					}
				}
			}
		}

		// Token: 0x0600538E RID: 21390
		public SDMod()
		{
		}
	}
}






using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

// Token: 0x020009AF RID: 2479
internal static class WinConsole
{
	// Token: 0x06005383 RID: 21379 RVA: 0x0015115C File Offset: 0x0014F35C
	public static void Initialize(bool alwaysCreateNewConsole = true)
	{
		bool flag = true;
		if (alwaysCreateNewConsole || (WinConsole.AttachConsole(4294967295U) == 0U && (long)Marshal.GetLastWin32Error() != 5L))
		{
			flag = (WinConsole.AllocConsole() != 0);
		}
		if (flag)
		{
			WinConsole.InitializeOutStream();
			WinConsole.InitializeInStream();
		}
	}

	// Token: 0x06005384 RID: 21380 RVA: 0x00151198 File Offset: 0x0014F398
	private static void InitializeOutStream()
	{
		FileStream fileStream = WinConsole.CreateFileStream("CONOUT$", 1073741824U, 2U, FileAccess.Write);
		if (fileStream != null)
		{
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.AutoFlush = true;
			Console.SetOut(streamWriter);
			Console.SetError(streamWriter);
		}
	}

	// Token: 0x06005385 RID: 21381 RVA: 0x001511D4 File Offset: 0x0014F3D4
	private static void InitializeInStream()
	{
		FileStream fileStream = WinConsole.CreateFileStream("CONIN$", 2147483648U, 1U, FileAccess.Read);
		if (fileStream != null)
		{
			Console.SetIn(new StreamReader(fileStream));
		}
	}

	// Token: 0x06005386 RID: 21382 RVA: 0x00151204 File Offset: 0x0014F404
	private static FileStream CreateFileStream(string name, uint win32DesiredAccess, uint win32ShareMode, FileAccess dotNetFileAccess)
	{
		SafeFileHandle safeFileHandle = new SafeFileHandle(WinConsole.CreateFileW(name, win32DesiredAccess, win32ShareMode, IntPtr.Zero, 3U, 128U, IntPtr.Zero), true);
		if (!safeFileHandle.IsInvalid)
		{
			return new FileStream(safeFileHandle, dotNetFileAccess);
		}
		return null;
	}

	// Token: 0x06005387 RID: 21383
	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int AllocConsole();

	// Token: 0x06005388 RID: 21384
	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	private static extern uint AttachConsole(uint dwProcessId);

	// Token: 0x06005389 RID: 21385
	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	// Token: 0x04003E77 RID: 15991
	private const uint GENERIC_WRITE = 1073741824U;

	// Token: 0x04003E78 RID: 15992
	private const uint GENERIC_READ = 2147483648U;

	// Token: 0x04003E79 RID: 15993
	private const uint FILE_SHARE_READ = 1U;

	// Token: 0x04003E7A RID: 15994
	private const uint FILE_SHARE_WRITE = 2U;

	// Token: 0x04003E7B RID: 15995
	private const uint OPEN_EXISTING = 3U;

	// Token: 0x04003E7C RID: 15996
	private const uint FILE_ATTRIBUTE_NORMAL = 128U;

	// Token: 0x04003E7D RID: 15997
	private const uint ERROR_ACCESS_DENIED = 5U;

	// Token: 0x04003E7E RID: 15998
	private const uint ATTACH_PARRENT = 4294967295U;
}

int int2 = sfsob.GetInt("bat11"); //BATTLE_ENTITY_HEALTH_CURRENT
			int int3 = sfsob.GetInt("bat13"); //BATTLE_ENTITY_HEALTH_CHANGE
			int int4 = sfsob.GetInt("bat51"); //BATTLE_ENTITY_SHIELD_CURRENT
			int int5 = sfsob.GetInt("bat53"); //BATTLE_ENTITY_SHIELD_CHANGE
			int int6 = sfsob.GetInt("bat56"); //BATTLE_ENTITY_DAMAGE_GAINED
			int int7 = sfsob.GetInt("bat63"); //BATTLE_ENTITY_DAMAGE_GAINED_CHANGE
			int int8 = sfsob.GetInt("bat16"); //BATTLE_ENTITY_VALUE
			int num = int3 + int5 + int7; //Health Change +  Sheild Change + Damage Gained Change | THEY DO NOT JUST GET DAMAGED FROM HERE
			bool flag = sfsob.ContainsKey("bat17") && sfsob.GetBool("bat17"); //BATTLE_BLOCK
			bool flag2 = sfsob.ContainsKey("bat18") && sfsob.GetBool("bat18"); //BATTLE_EVADE
			bool flag3 = /*sfsob.ContainsKey("bat19") && */sfsob.GetBool("bat19"); //BATTLE_CRIT
			bool flag4 = sfsob.ContainsKey("bat55") && sfsob.GetBool("bat55"); //BATTLE_DOUBLE_HIT
			bool flag5 = sfsob.ContainsKey("bat40") && sfsob.GetBool("bat40"); //BATTLE_DEFLECT
			bool flag6 = sfsob.ContainsKey("bat48") && sfsob.GetBool("bat48"); //BATTLE_BONUS
			bool flag7 = sfsob.ContainsKey("bat49") && sfsob.GetBool("bat49"); //BATTLE_REDUCE
			bool flag8 = sfsob.ContainsKey("bat50") && sfsob.GetBool("bat50"); //BATTLE_REDIRECT
			bool flag9 = sfsob.ContainsKey("bat54") && sfsob.GetBool("bat54"); //BATTLE_ABSORB
			float value = sfsob.ContainsKey("bat61") ? sfsob.GetFloat("bat61") : 0f; //BATTLE_CHANGE_PERC
			AbilityActionRef abilityActionRef = sfsob.ContainsKey("bat23") ? AbilityBook.LookupAction(sfsob.GetInt("bat23")) : null; //BATTLE_ACTION_ID
			if (abilityActionRef != null && int8 >= 0)
			{
				ParticleRef effectEnd = abilityActionRef.effectEnd;
			}
			entity.SetShieldCurrent(int4); //set shield meter to BATTLE_ENTITY_SHIELD_CURRENT
			entity.SetHealthCurrent(int2); //set health meter to BATTLE_ENTITY_HEALTH_CURRENT

			if (entity.attacker && com.samzydev.mod.SDMod.newpatchmod)
			{
				sfsob.PutInt("bat11", entity.healthTotal);
				sfsob.PutInt("bat13", 0);
				sfsob.PutBool("bat19", true);
				sfsob.PutInt("bat56", 0); //set entity damage gained to 0
				sfsob.PutInt("bat63", 0); //set entity damage gained change to 0
			}



#############################

using System;
using System.Collections;
using System.Collections.Generic;
using com.samzydev.mod;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.particle;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.battle
{
	// Token: 0x02000511 RID: 1297
	public partial class Battle : MonoBehaviour
	{
		// Token: 0x06002D3D RID: 11581
		private void DoActionHealthChange(SFSObject sfsob)
		{
			int @int = sfsob.GetInt("bat7");
			BattleEntity entity = this.GetEntity(@int);
			if (!entity)
			{
				this.RunQueue();
				return;
			}
			if (entity.attacker && SDMod.newpatchmod)
			{
				sfsob.PutInt("bat11", entity.healthTotal);
				sfsob.PutInt("bat13", 0);
				sfsob.PutBool("bat19", true);
				sfsob.PutInt("bat56", 0);
				sfsob.PutInt("bat63", 0);
			}
			int int2 = sfsob.GetInt("bat11"); //BATTLE_ENTITY_HEALTH_CURRENT
			int int3 = sfsob.GetInt("bat13"); //BATTLE_ENTITY_HEALTH_CHANGE
			int int4 = sfsob.GetInt("bat51"); //BATTLE_ENTITY_SHIELD_CURRENT
			int int5 = sfsob.GetInt("bat53"); //BATTLE_ENTITY_SHIELD_CHANGE
			int int6 = sfsob.GetInt("bat56"); //BATTLE_ENTITY_DAMAGE_GAINED
			int int7 = sfsob.GetInt("bat63"); //BATTLE_ENTITY_DAMAGE_GAINED_CHANGE
			int int8 = sfsob.GetInt("bat16"); //BATTLE_ENTITY_VALUE
			int num = int3 + int5 + int7; //Health Change +  Sheild Change + Damage Gained Change | THEY DO NOT JUST GET DAMAGED FROM HERE
			bool flag = sfsob.ContainsKey("bat17") && sfsob.GetBool("bat17"); //BATTLE_BLOCK
			bool flag2 = sfsob.ContainsKey("bat18") && sfsob.GetBool("bat18"); //BATTLE_EVADE
			bool flag3 = /*sfsob.ContainsKey("bat19") && */sfsob.GetBool("bat19"); //BATTLE_CRIT
			bool flag4 = sfsob.ContainsKey("bat55") && sfsob.GetBool("bat55"); //BATTLE_DOUBLE_HIT
			bool flag5 = sfsob.ContainsKey("bat40") && sfsob.GetBool("bat40"); //BATTLE_DEFLECT
			bool flag6 = sfsob.ContainsKey("bat48") && sfsob.GetBool("bat48"); //BATTLE_BONUS
			bool flag7 = sfsob.ContainsKey("bat49") && sfsob.GetBool("bat49"); //BATTLE_REDUCE
			bool flag8 = sfsob.ContainsKey("bat50") && sfsob.GetBool("bat50"); //BATTLE_REDIRECT
			bool flag9 = sfsob.ContainsKey("bat54") && sfsob.GetBool("bat54"); //BATTLE_ABSORB
			float value = sfsob.ContainsKey("bat61") ? sfsob.GetFloat("bat61") : 0f; //BATTLE_CHANGE_PERC
			AbilityActionRef abilityActionRef = sfsob.ContainsKey("bat23") ? AbilityBook.LookupAction(sfsob.GetInt("bat23")) : null; //BATTLE_ACTION_ID
			if (abilityActionRef != null && int8 >= 0)
			{
				ParticleRef effectEnd = abilityActionRef.effectEnd;
			}
			entity.SetShieldCurrent(int4); //set shield meter to BATTLE_ENTITY_SHIELD_CURRENT
			entity.SetHealthCurrent(int2); //set health meter to BATTLE_ENTITY_HEALTH_CURRENT
			if (this.MustSaveBattleStats() && this._currentEntity && entity)
			{
				string.Concat(new string[]
				{
					"[",
					this._currentEntity.id.ToString(),
					"|",
					this._currentEntity.index.ToString(),
					"|",
					this._currentEntity.attacker.ToString(),
					"]"
				});
				string.Concat(new string[]
				{
					"[",
					entity.id.ToString(),
					"|",
					entity.index.ToString(),
					"|",
					entity.attacker.ToString(),
					"]"
				});
				int index = this._currentEntity.index;
				int index2 = entity.index;
				int num2 = Mathf.Abs(num); //returns absolute value of change in battle
				if (num < 0) //if change of battle is negative, meaning we took damage
				{
					if (flag) //if BATTLE_BLOCK
					{
						this._battleStats[index2].damageBlocked += num2; //block...
					}
					this._battleStats[index].damageDone += num2; //set damage done to current entity
					this._battleStats[index2].damageTaken += num2; //set damage taken to local entity
				}
				else if (int3 > 0) //if health change is positive show healing
				{
					this._battleStats[index].healingDone += num2;
					this._battleStats[index2].healingTaken += num2;
				}
				if (int5 > 0) //if shield change is positive, show getting shield
				{
					this._battleStats[index].shielding += num2;
				}
			}
			string str = (int8 < 0) ? "+" : ""; //string for health change
			string text = BattleText.COLOR_GREEN; //text color
			float scale = 1f; //text scale
			Vector2 center = Util.GetCenter(entity.transform, true); //get center of screen in Vector2
			if (int8 >= 0) //if battle entity value is equal to or greater than 0
			{
				if ((flag3 || flag4) && !flag) //if crit or double hit, and not block
				{
					text = BattleText.COLOR_ORANGE; //change text to orange
					scale = 1.25f; //set scale to 1.25
				}
				else if (!flag3 && !flag4 && flag) //else if not crit or double hit, but block
				{
					text = BattleText.COLOR_PURPLE; //change color of text to purple
				}
				else
				{
					text = BattleText.COLOR_RED; //otherwise, default to red
				}
			}
			if (int5 > 0) //if gained sheild
			{
				text = BattleText.COLOR_PINK; //change text to pink
			}
			else if (int7 != 0) //if damage gained change is not 0
			{
				text = BattleText.COLOR_TEAL; //change color to teal
			}
			/* Animation stuff start */
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y * componentInChildren3.transform.localScale.y / 2f;
				}
			}
			else
			{
				BoxCollider2D componentInChildren4 = entity.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren4 != null)
				{
					center.y += componentInChildren4.size.y / 2f;
				}
			}
			float num3 = center.y + 30f;
			float num4 = 0.1f;
			float num5 = 0f;
			if (flag3 && num != 0)
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_critical", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("critical", 1f);
			}
			if (flag4 && num != 0)
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_empowered", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("critical", 1f);
			}
			if (flag)
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_block", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("block", 1f);
			}
			if (flag2)
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_evade", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("evade", 1f);
			}
			if (flag5)
			{
				if (this._battleAnimations)
				{
					entity.PlayAnimation("hit");
				}
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_deflect", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("block", 1f);
			}
			if (flag6 && this._battleText)
			{
				this.AddBattleTextObj().LoadDetails(Language.GetString("battle_bonus", null, false) + this.GetChangePercText(value), BattleText.COLOR_CYAN, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			/* end animation stuff */
			string text2 = ""; //sets empty string
			if (sfsob.ContainsKey("bat64")) //if BATTLE_ENTITY_FIRE_HEALTH_CHANGE
			{
				text2 = "bat64"; //set BATTLE_ENTITY_FIRE_HEALTH_CHANGE
			}
			else if (sfsob.ContainsKey("bat65")) //if BATTLE_ENTITY_WATER_HEALTH_CHANGE
			{
				text2 = "bat65"; //set BATTLE_ENTITY_WATER_HEALTH_CHANGE
			}
			else if (sfsob.ContainsKey("bat66")) //if BATTLE_ENTITY_ELECTRIC_HEALTH_CHANGE
			{
				text2 = "bat66"; //set BATTLE_ENTITY_ELECTRIC_HEALTH_CHANGE
			}
			else if (sfsob.ContainsKey("bat67")) //if BATTLE_ENTITY_EARTH_HEALTH_CHANGE
			{
				text2 = "bat67"; //set BATTLE_ENTITY_EARTH_HEALTH_CHANGE
			}
			else if (sfsob.ContainsKey("bat68")) //if BATTLE_ENTITY_AIR_HEALTH_CHANGE
			{
				text2 = "bat68"; //set BATTLE_ENTITY_AIR_HEALTH_CHANGE
			}
			if (flag6 && this._battleText) //if battle bonus and text showing
			{
				string link = "battle_bonus"; //creates link string
				string color = BattleText.COLOR_CYAN; //sets text color to cyan
				/* I am not going through all of that but it just changes the text color based on different bonuses */
				if (text2 != null)
				{
					if (!(text2 == "bat64"))
					{
						if (!(text2 == "bat65"))
						{
							if (!(text2 == "bat66"))
							{
								if (!(text2 == "bat67"))
								{
									if (text2 == "bat68")
									{
										color = "#B5D4F0";
									}
								}
								else
								{
									color = "#5FC608";
								}
							}
							else
							{
								color = "#E7DA83";
							}
						}
						else
						{
							color = "#1342EF";
						}
					}
					else
					{
						color = "#FD5400";
					}
				}
				this.AddBattleTextObj().LoadDetails(Language.GetString(link, null, false) + this.GetChangePercText(value), color, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
				num3 -= 30f;
				num5 += num4;
				/* end that thing */
			}
			if (flag7 && this._battleText) //if battle reduce and text
			{
				this.AddBattleTextObj().LoadDetails(Language.GetString("battle_reduced", null, false) + this.GetChangePercText(value), BattleText.COLOR_CYAN, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			if (flag8) //if redict
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_redirect", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("block", 1f);
			}
			if (flag9) //if absorb
			{
				if (this._battleText)
				{
					this.AddBattleTextObj().LoadDetails(Language.GetString("battle_absorb", null, false), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
					num3 += 30f;
					num5 += num4;
				}
				GameData.instance.audioManager.PlaySoundLink("block", 1f);
			}
			if (num != 0) //if change of battle is not zero
			{
				/* more animatin stuff regarding elements */
				string text3 = "";
				string color2 = text;
				float num6 = 20f;
				float num7 = center.y + num6;
				if (num < 0)
				{
					color2 = BattleText.COLOR_RED;
					if (text2 != null)
					{
						if (!(text2 == "bat64"))
						{
							if (!(text2 == "bat65"))
							{
								if (!(text2 == "bat66"))
								{
									if (!(text2 == "bat67"))
									{
										if (text2 == "bat68")
										{
											text3 = "battle_air_damage";
											color2 = "#B5D4F0";
											num7 -= num6;
										}
									}
									else
									{
										text3 = "battle_earth_damage";
										color2 = "#5FC608";
										num7 -= num6;
									}
								}
								else
								{
									text3 = "battle_electric_damage";
									color2 = "#E7DA83";
									num7 -= num6;
								}
							}
							else
							{
								text3 = "battle_water_damage";
								color2 = "#1342EF";
								num7 -= num6;
							}
						}
						else
						{
							text3 = "battle_fire_damage";
							color2 = "#FD5400";
							num7 -= num6;
						}
					}
				}
				text3 = Language.GetString(text3, null, false);
				if (this._battleText)
				{
					string text4 = str + Util.NumberFormat(-(float)int8, true, false, 100f) + " " + text3;
					this.AddBattleTextObj().LoadDetails(text4, color2, 3f, 10f, center.x, num7, scale, -30f, num5);
				}
				/* end that */
				if (this._battleEffects) //if action has effect animation
				{
					SpritesFlash spritesFlash = entity.GetComponent<SpritesFlash>(); //play it
					if (spritesFlash == null)
					{
						spritesFlash = entity.gameObject.AddComponent<SpritesFlash>();
					}
					spritesFlash.DoFlash();
					SpritesFlash spritesFlash2 = entity.overlay.GetComponent<SpritesFlash>();
					if (spritesFlash2 == null)
					{
						spritesFlash2 = entity.overlay.gameObject.AddComponent<SpritesFlash>();
					}
					spritesFlash2.DoFlash(); //same here
				}
				if (num < 0) //if battle change is negative
				{
					if (this._battleAnimations) //if has animatins enabled
					{
						entity.PlayAnimation("hit"); //play animation
					}
					GameData.instance.audioManager.PlaySoundPoolLink("damage", 1f); //play sound
				}
				else if (int3 > 0) //else if BATTLE_ENTITY_HEALTH_CHANGE is positive
				{
					GameData.instance.audioManager.PlaySoundLink("heal", 1f); //play heal
				}
				else if (int5 > 0) //else if BATTLE_ENTITY_SHIELD_CHANGE is positive
				{
					GameData.instance.audioManager.PlaySoundLink("sheen", 1f); //play sheen?
				}
			}
			if (int7 != 0) //if BATTLE_ENTITY_DAMAGE_GAINED_CHANGE is not zero
			{
				entity.SetDamageGained(int6); //set damage gained BATTLE_ENTITY_DAMAGE_GAINED
			}
			float seconds = ((abilityActionRef != null) ? abilityActionRef.duration : 0.05f) / this.GetSpeed(null);
			if (flag3 && int3 != 0)
			{
				Util.Shake(entity.gameObject, this.GetSpeed(null), new Vector2?(new Vector2(entity.x, entity.y)));
			}
			base.StartCoroutine(this.DoTimer(seconds, new UnityAction(this.RunQueue)));
		}
	}
}

#########################################

using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.victory;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.battle
{
	// Token: 0x02000511 RID: 1297
	public partial class Battle : MonoBehaviour
	{
		// Token: 0x06002D5A RID: 11610
		private void DoActionVictory(SFSObject sfsob)
		{
			if (this._replay) //if replay enabled
			{
				this.DoReplayComplete(); //restart dungeon
				return;
			}
			if (sfsob.GetInt("bat5") != GameData.instance.PROJECT.character.id) //if current entity is not player
			{
				this.RunQueue(); //run queue again
				return;
			}
			this._results = true; //we have results! lets change them :)
			long @long = sfsob.GetLong("cha5"); //CHARACTER_EXP
			int @int = sfsob.GetInt("cha9"); //CHARACTER_GOLD
			int int2 = sfsob.GetInt("cha4"); //CHARACTER_LEVEL
			int int3 = sfsob.GetInt("cha19"); //CHARACTER_POINTS
			List<ItemData> list = ItemData.listFromSFSObject(sfsob);//get items from sfsob
			long num = @long - GameData.instance.PROJECT.character.exp; //num equals character exp minus character exp
			int num2 = @int - GameData.instance.PROJECT.character.gold; //num2 equals character gold minus character gold
			int creditsGained = 0; //no credits
			int level = GameData.instance.PROJECT.character.level; //bruyh
			int points = GameData.instance.PROJECT.character.points; //yep
			if (GameData.instance.PROJECT.dungeon != null) //if we in a dungeon
			{
				//make sure we not hackin
				KongregateAnalytics.checkEconomyTransaction(KongregateAnalytics.getBattleEconomyType(this), null, list, sfsob, KongregateAnalytics.getBattleEconomyContext(this), 2, this._type != 2, null, true);
			}
			Dungeon dungeon = GameData.instance.PROJECT.dungeon; //define dungeon?
			if (dungeon != null) //if not null
			{
				GameData.instance.PROJECT.character.addDungeonLoot(new ItemData(ItemBook.Lookup(3, 3), (int)num), dungeon.type, dungeon.dungeonRef.id); //character xp loot
				GameData.instance.PROJECT.character.addDungeonLoot(new ItemData(ItemBook.Lookup(1, 3), num2), dungeon.type, dungeon.dungeonRef.id); //character gold loot
			}
			GameData.instance.PROJECT.character.exp = @long; //new character xp
			GameData.instance.PROJECT.character.gold = @int; //new character gold
			GameData.instance.PROJECT.character.level = int2; //new character level
			GameData.instance.PROJECT.character.points = int3; //new character poitns
			GameData.instance.PROJECT.character.addItems(list, true); //new items
			Transform transform = GameData.instance.windowGenerator.NewVictoryWindow(-1); //show victory window
			transform.GetComponent<VictoryWindow>().ON_CLOSE.AddListener(new UnityAction<object>(this.OnDialogClose));
			string @string;
			bool flag;
			switch (this._type)
			{
			case 2:
			case 3:
			case 6:
			case 7:
			case 9:
				@string = Language.GetString("ui_town", null, false);
				flag = true;
				goto IL_285;
			}
			@string = Language.GetString("ui_continue", null, false);
			flag = false;
			IL_285:
			VictoryWindow component = transform.GetComponent<VictoryWindow>();
			int type = this._type;
			long exp = num;
			int gold = num2;
			List<ItemData> items = list;
			List<BattleStat> battleStats = this._battleStats;
			bool shouldPlayMusic = true;
			string customShieldText = null;
			string customLootText = null;
			bool isCloseRed = flag;
			component.LoadDetails(type, exp, gold, items, battleStats, shouldPlayMusic, customShieldText, customLootText, @string, true, isCloseRed, false);
			if (this._type == 2)
			{
				GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.PVP_BATTLES_WON, true);
			}
			this.TrackEnd("Win", creditsGained, num2);
			this.RunQueue();
		}
	}
}
############################
Dungeon Extension patch
if (GameData.instance.PROJECT.character.toCharacterData(false).nftIsAdFree)
			{
				sfsobject.PutBool("cha130", true);
			}
>>
if (true)
			{
				sfsobject.PutBool("cha130", true);
			}

patch
public void checkCurrencyChanges(SFSObject sfsob, bool update = false)
		{
			KongregateAnalytics.updateCommonFields();
		}