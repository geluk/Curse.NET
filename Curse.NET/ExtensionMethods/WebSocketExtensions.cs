using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Curse.NET.ExtensionMethods
{
	internal static class WebSocketExtensions
	{
		public static async Task SendMessage(this ClientWebSocket socket, string message)
		{
			var buffer = Encoding.UTF8.GetBytes(message);
			await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public static async Task<string> ReceiveMessage(this ClientWebSocket socket)
		{
			var buffer = new ArraySegment<byte>(new byte[4096]);
			var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
			if (result.CloseStatus != null)
			{
				Debugger.Break();
			}
			if (result.EndOfMessage)
			{
				return Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
			}
			else
			{
				throw new InvalidOperationException("Buffer size too small");
			}
		}
	}
}
