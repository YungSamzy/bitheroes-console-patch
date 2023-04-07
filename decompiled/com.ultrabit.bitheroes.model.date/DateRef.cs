using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.model.date;

[DebuggerDisplay("{desc} (DateRef)")]
public class DateRef : IEquatable<DateRef>, IComparable<DateRef>
{
	protected DateTime _startDate;

	protected DateTime _endDate;

	private string desc => "[" + startDate.ToShortDateString() + "] to [" + endDate.ToShortDateString() + "]";

	public long startMilliseconds => getMillisecondsUntilStart();

	public DateTime startDate => _startDate;

	public DateTime endDate => _endDate;

	public DateRef(string startDate, string endDate)
	{
		SetDates(startDate, endDate);
	}

	public void SetDates(DateTime startDate, DateTime endDate)
	{
		_startDate = startDate;
		_endDate = endDate;
	}

	public void SetDates(string startDate, string endDate)
	{
		if (Util.GetStringFromStringProperty(startDate, null) != null && Util.GetStringFromStringProperty(endDate, null) != null)
		{
			_startDate = Util.GetDateFromString(startDate);
			_endDate = Util.GetDateFromString(endDate);
		}
	}

	public long getMillisecondsUntilStart(DateTime? customDate = null)
	{
		DateTime dateTime = (customDate.HasValue ? customDate.Value : ServerExtension.instance.GetDate());
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan timeSpan = _startDate.Subtract(value);
		TimeSpan ts = dateTime.Subtract(value);
		return (long)timeSpan.Subtract(ts).TotalMilliseconds;
	}

	public long getMillisecondsUntilEnd(DateTime? customDate = null)
	{
		DateTime dateTime = (customDate.HasValue ? customDate.Value : ServerExtension.instance.GetDate());
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan timeSpan = _endDate.Subtract(value);
		TimeSpan ts = dateTime.Subtract(value);
		return (long)timeSpan.Subtract(ts).TotalMilliseconds;
	}

	public long getMillisecondsDuration()
	{
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan ts = _startDate.Subtract(value);
		return (long)_endDate.Subtract(value).Subtract(ts).TotalMilliseconds;
	}

	public bool getActive(DateTime? customDate = null)
	{
		if (getMillisecondsUntilStart(customDate) < 0 && getMillisecondsUntilEnd(customDate) > 0)
		{
			return true;
		}
		return false;
	}

	public bool Equals(DateRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (startDate.Equals(other.startDate))
		{
			return endDate.Equals(other.endDate);
		}
		return false;
	}

	public int CompareTo(DateRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = startDate.CompareTo(other.startDate);
		if (num == 0)
		{
			return endDate.CompareTo(other.endDate);
		}
		return num;
	}
}
