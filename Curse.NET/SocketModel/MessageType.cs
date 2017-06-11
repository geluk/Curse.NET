namespace Curse.NET.SocketModel
{
	public enum RequestType
	{
		Login = -2101997347,
		Ping = -476754606
	}

	public enum ResponseType
	{
		Ping = -476754606,
		ChannelStatusChanged = 72981382,
		UserActivityChange = 1260535191,
		ChannelMarkedRead = -695526586,
		ChatMessage = -635182161,
		Login = -815187584,
		ClientPreferences = 937250613,
		MessageChanged = 149631008,
		MessageStatus = 705131365,
		FriendshipStatus = 580569888,
		FriendRemoved = 1216900677,
	}
}
