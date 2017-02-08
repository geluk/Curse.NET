using System;
using System.Threading;
using System.Threading.Tasks;
using Curse.NET.SocketModel;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Curse.NET
{

	internal class SocketApi
	{
		//private readonly ClientWebSocket webSocket = new ClientWebSocket();
		private WebSocket webSocket;

		public event SocketMessageReceivedEvent MessageReceived;
		public event ChannelMarkedReadEvent ChannelMarkedRead;
		public event MessageChangedEvent MessageChanged;
		public event UserActivityChangeEvent UserActivityChange;

		public void Connect(Uri wsUri, string authToken)
		{
			webSocket = new WebSocket(wsUri.ToString());
			webSocket.SetCookie(new Cookie("CurseAuthToken", authToken));
			webSocket.Origin = "https://www.curse.com";
			webSocket.OnMessage += MessageHandler;
			webSocket.OnError += SocketErrorHandler;
			webSocket.Connect();

			//webSocket.Options.SetRequestHeader("Origin", "https://www.curse.com");
			//webSocket.Options.SetRequestHeader("Cookie", "CurseAuthToken=" + WebUtility.UrlEncode(authToken));
			//webSocket.ConnectAsync(wsUri, CancellationToken.None).Wait();
		}

		private void SocketErrorHandler(object sender, ErrorEventArgs errorEventArgs)
		{
			;
		}

		private void MessageHandler(object sender, MessageEventArgs ev)
		{
			Task.Run(() =>
			{
				var response = SocketResponse.Deserialise(ev.Data);
				ProcessMessage(response);
			});
		}

		public void Login(string machineKey, string sessionId, int userId)
		{
			SendMessage(LoginRequest.Create(machineKey, sessionId, userId));
		}

		public async void SendMessage(SocketRequest message)
		{
			var semaphore = new SemaphoreSlim(0, 1);
			var messageText = JsonConvert.SerializeObject(message);
			webSocket.SendAsync(messageText, success =>
			{
				semaphore.Release();
			});
			await semaphore.WaitAsync();
		}

		public void Listen()
		{
			//Task.Run(() =>
			//{
			//	while (webSocket.CloseStatus == null)
			//	{
			//		try
			//		{
			//			var message = webSocket.ReceiveMessage().Result;
			//			var parsed = SocketResponse.Deserialise(message);
			//			ProcessMessage(parsed);
			//		}
			//		catch (WebSocketException)
			//		{
			//			Debugger.Break();
			//			// TODO: handle disconnect
			//		}

			//	}
			//});
		}

		private void ProcessMessage(SocketResponse message)
		{
			switch (message.TypeID)
			{
				case ResponseType.UserActivityChange:
					UserActivityChange?.Invoke((UserActivityChangeResponse)message.Body);
					break;
				case ResponseType.ChannelMarkedRead:
					ChannelMarkedRead?.Invoke((ChannelMarkedReadResponse)message.Body);
					break;
				case ResponseType.ChatMessage:
					MessageReceived?.Invoke((MessageResponse)message.Body);
					break;
				case ResponseType.Login:
					break;
				case ResponseType.MessageChanged:
					MessageChanged?.Invoke((MessageChangedResponse)message.Body);
					break;
				case ResponseType.ChannelStatusChanged:
					// TODO: implement ChannelStatusChanged event
					break;
				case ResponseType.Unknown1:
					break;
				default:
					break;
			}
		}
	}
}
