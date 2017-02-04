using Curse.NET.Model;
using Curse.NET.SocketModel;
using Group = Curse.NET.Model.Group;

namespace Curse.NET
{
	public delegate void MessageReceivedEvent(Group group, Channel channel, MessageResponse message);
}
