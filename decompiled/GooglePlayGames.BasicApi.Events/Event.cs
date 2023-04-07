namespace GooglePlayGames.BasicApi.Events;

internal class Event : IEvent
{
	private string mId;

	private string mName;

	private string mDescription;

	private string mImageUrl;

	private ulong mCurrentCount;

	private EventVisibility mVisibility;

	public string Id => mId;

	public string Name => mName;

	public string Description => mDescription;

	public string ImageUrl => mImageUrl;

	public ulong CurrentCount => mCurrentCount;

	public EventVisibility Visibility => mVisibility;

	internal Event(string id, string name, string description, string imageUrl, ulong currentCount, EventVisibility visibility)
	{
		mId = id;
		mName = name;
		mDescription = description;
		mImageUrl = imageUrl;
		mCurrentCount = currentCount;
		mVisibility = visibility;
	}
}
