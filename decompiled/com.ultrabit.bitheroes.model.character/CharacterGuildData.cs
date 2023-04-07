using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterGuildData : Messenger
{
	private int _id;

	private int _rank;

	private string _name;

	private string _initials;

	private List<GuildMemberData> _members;

	private List<bool> _permissions;

	private List<ChatData> _messages;

	private GuildPerks _perks;

	public int id => _id;

	public int rank => _rank;

	public string name => _name;

	public string initials => _initials;

	public List<GuildMemberData> members => _members;

	public List<bool> permissions => _permissions;

	public GuildPerks perks => _perks;

	public List<ChatData> messages => _messages;

	public CharacterGuildData(int id, int rank, string name, string initials, List<GuildMemberData> members, List<bool> permissions, GuildPerks perks)
	{
		_id = id;
		_rank = rank;
		_name = name;
		_initials = initials;
		_members = members;
		_permissions = permissions;
		_perks = perks;
	}

	public CharacterGuildInfo toGuildInfo()
	{
		return new CharacterGuildInfo(_id, _name, _initials);
	}

	public void setMessages(List<ChatData> data)
	{
		if (_messages == null)
		{
			_messages = new List<ChatData>();
		}
		foreach (ChatData datum in data)
		{
			addMessage(datum);
		}
	}

	public void addMessage(ChatData chatData)
	{
		if (_messages != null)
		{
			_messages.Add(chatData);
			while (_messages.Count > VariableBook.guildChatMessageLimit)
			{
				_messages.RemoveAt(0);
			}
		}
	}

	public void addMember(GuildMemberData memberData)
	{
		for (int i = 0; i < _members.Count; i++)
		{
			if (_members[i].characterData.charID == memberData.characterData.charID)
			{
				return;
			}
		}
		_members.Add(memberData);
	}

	public void removeMember(int charID)
	{
		for (int i = 0; i < _members.Count; i++)
		{
			if (_members[i].characterData.charID == charID)
			{
				_members.RemoveAt(i);
				break;
			}
		}
	}

	public void UpdateMemberDataRank(int charID, int rank)
	{
		for (int i = 0; i < _members.Count; i++)
		{
			if (_members[i].characterData.charID == charID)
			{
				_members[i].setRank(rank);
			}
		}
	}

	public GuildMemberData getMember(int charID, int teamType = -1, bool duplicateData = true)
	{
		for (int i = 0; i < _members.Count; i++)
		{
			GuildMemberData guildMemberData = _members[i];
			if (guildMemberData.characterData.charID != charID)
			{
				continue;
			}
			if (!duplicateData)
			{
				if (teamType == 6)
				{
					return new GuildMemberData(guildMemberData.characterData.Duplicate(), guildMemberData.online, guildMemberData.rank);
				}
				return guildMemberData;
			}
			return new GuildMemberData(guildMemberData.characterData.Duplicate(), guildMemberData.online, guildMemberData.rank);
		}
		return null;
	}

	public List<GuildMemberData> getOnlineMembers(bool excludeMyself = false)
	{
		List<GuildMemberData> list = new List<GuildMemberData>();
		foreach (GuildMemberData member in _members)
		{
			if ((!excludeMyself || member.characterData.charID != GameData.instance.PROJECT.character.id) && member.online)
			{
				list.Add(member);
			}
		}
		return list;
	}

	public int GetUniqueMembersCount(bool excludeMyself = false)
	{
		List<int> list = new List<int>();
		foreach (GuildMemberData member in _members)
		{
			if ((!excludeMyself || member.characterData.playerID != GameData.instance.PROJECT.character.playerID) && !list.Contains(member.characterData.playerID))
			{
				list.Add(member.characterData.playerID);
			}
		}
		D.Log("GetUniqueMembersCount.ALL: " + _members.Count);
		D.Log("GetUniqueMembersCount.uniqueMembers: " + list.Count);
		return list.Count;
	}

	public long getMutinyMilliseconds()
	{
		return _rank switch
		{
			1 => VariableBook.guildMutinyOfficerMilliseconds, 
			2 => VariableBook.guildMutinyMemberMilliseconds, 
			3 => VariableBook.guildMutinyRecruitMilliseconds, 
			_ => 0L, 
		};
	}

	public bool hasPermission(int action)
	{
		if (_rank == 0)
		{
			return true;
		}
		return Guild.hasPermission(action, _permissions);
	}

	public static CharacterGuildData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("gui0");
		int int2 = sfsob.GetInt("gui1");
		string utfString = sfsob.GetUtfString("gui2");
		string utfString2 = sfsob.GetUtfString("gui3");
		List<GuildMemberData> list = GuildMemberData.listFromSFSObject(sfsob);
		List<bool> booleanVectorFromArray = Util.getBooleanVectorFromArray(sfsob.GetBoolArray("gui4"));
		GuildPerks guildPerks = GuildPerks.fromSFSObject(sfsob);
		return new CharacterGuildData(@int, int2, utfString, utfString2, list, booleanVectorFromArray, guildPerks);
	}

	public static CharacterGuildData fromJsonString(string theString)
	{
		return fromSFSObject(SFSObject.NewFromJsonData(theString));
	}

	public void setRank(int rank)
	{
		_rank = rank;
		Broadcast("GUILD_RANK_CHANGE");
	}

	public void setPermissions(List<bool> perms)
	{
		_permissions = perms;
		Broadcast("GUILD_PERMISSIONS_CHANGE");
	}

	public void setPerks(GuildPerks perks)
	{
		_perks = perks;
		Broadcast("GUILD_PERKS_CHANGE");
	}
}
