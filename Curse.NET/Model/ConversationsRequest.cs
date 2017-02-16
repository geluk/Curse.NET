using System;
using Newtonsoft.Json;

namespace Curse.NET.Model
{
	public class ConversationsRequest : RequestObject
	{
		public Message[] Messages { get; set; }
	}
}
