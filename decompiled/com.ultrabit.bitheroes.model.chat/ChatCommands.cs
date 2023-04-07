namespace com.ultrabit.bitheroes.model.chat;

public class ChatCommands
{
	public const int HELP = 0;

	public const int CLEAR = 1;

	public const int PROFILE = 2;

	public const int TIME = 3;

	private static string[][] _commands = new string[4][]
	{
		new string[6] { "help", "h", "info", "i", "all", "options" },
		new string[2] { "clear", "c" },
		new string[5] { "view", "v", "profile", "p", "player" },
		new string[4] { "time", "t", "date", "d" }
	};

	private static string[] _desc = new string[4] { "chat_command_help_desc", "chat_command_clear_desc", "chat_command_profile_desc", "chat_command_time_desc" };

	public static string[][] commands => _commands;

	public static string[] desc => _desc;

	public static int getCommand(string command)
	{
		for (int i = 0; i < _commands.Length; i++)
		{
			string[] array = _commands[i];
			foreach (string text in array)
			{
				if (command == text)
				{
					return i;
				}
			}
		}
		return -1;
	}
}
