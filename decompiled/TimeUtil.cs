using System;

public class TimeUtil
{
	private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public static DateTime GetUnixDateTime()
	{
		return epoch;
	}

	public static int ToUnixTimestamp(DateTime date)
	{
		return (int)date.Subtract(GetUnixDateTime()).TotalSeconds;
	}

	public static int GetDeviceTime()
	{
		return ToUnixTimestamp(DateTime.UtcNow);
	}

	public static DateTime GetLocalDateTime(int eventTime)
	{
		return GetUnixDateTime().AddSeconds(eventTime).ToLocalTime();
	}

	public static string FormatClockRemaining(int secondsRemaining)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(secondsRemaining);
		int minutes = timeSpan.Minutes;
		int seconds = timeSpan.Seconds;
		string text = ((seconds < 10) ? "0" : "");
		return (minutes + ":" + text + seconds).Trim();
	}

	public static string FormatCalendarRemaining(int secondsRemaining, int displayPrecision = 0, bool leadingZeros = true)
	{
		string text = string.Empty;
		if (secondsRemaining > 0)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(secondsRemaining);
			int num = 0;
			int days = timeSpan.Days;
			if (days > 0 && (displayPrecision <= 0 || num < displayPrecision))
			{
				num++;
				text = text + days + "d";
			}
			int hours = timeSpan.Hours;
			if (hours > 0 && (displayPrecision <= 0 || num < displayPrecision))
			{
				text += ((hours < 10 && leadingZeros) ? (" 0" + hours + "h") : (" " + hours + "h"));
				num++;
			}
			int minutes = timeSpan.Minutes;
			if (minutes > 0 && (displayPrecision <= 0 || num < displayPrecision))
			{
				text += ((minutes < 10 && leadingZeros) ? (" 0" + minutes + "m") : (" " + minutes + "m"));
				num++;
			}
			int seconds = timeSpan.Seconds;
			if (seconds > 0 && (displayPrecision <= 0 || num < displayPrecision))
			{
				text += ((seconds < 10 && leadingZeros) ? (" 0" + seconds + "s") : (" " + seconds + "s"));
				num++;
			}
		}
		return text;
	}
}
