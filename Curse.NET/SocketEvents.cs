using Curse.NET.SocketModel;

namespace Curse.NET
{
	internal delegate void SocketMessageReceivedEvent(MessageResponse message);
	internal delegate void ChannelMarkedReadEvent(ChannelMarkedReadResponse channel);
	internal delegate void MessageChangedEvent(MessageChangedResponse change);
	internal delegate void FriendshipEvent(FriendshipResponse friendship);
	internal delegate void UserActivityChangeEvent(UserActivityChangeResponse activity);
}
