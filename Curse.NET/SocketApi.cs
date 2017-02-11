#define WEBSOCKET4NET
// #define NETSOCKET

using System;
using System.Threading;
using System.Threading.Tasks;
using Curse.NET.SocketModel;
using Newtonsoft.Json;
#if NETSOCKET
using System.Net;
using System.Net.WebSockets;
#elif WEBSOCKET4NET
using SuperSocket.ClientEngine;
using WebSocket4Net;
using System.Collections.Generic;
using System.Net;
#else
using WebSocketSharp;
using WebSocketSharp.Net;
#endif

namespace Curse.NET
{
	internal class SocketApi : IDisposable
	{
#if NETSOCKET
		private readonly ClientWebSocket webSocket = new ClientWebSocket();
#else
		private WebSocket webSocket;
#endif
		public event Action<MessageResponse> MessageReceived;
		public event Action<ChannelMarkedReadResponse> ChannelMarkedRead;
		public event Action<MessageChangedResponse> MessageChanged;
		public event Action<FriendshipStatusResponse> FriendshipStatusChanged;
		public event Action<UserActivityChangeResponse> UserActivityChange;
		public event Action<FriendRemovedResponse> FriendRemoved;
		public event Action<ChannelStatusChangedResponse> ChannelStatusChanged;
		public event Action<SocketLoginResponse> LoginReceived;

		public event Action SocketClosed;

		private Timer pingTimer;

#if NETSOCKET
		public void Connect(Uri wsUri, string authToken)
		{
			webSocket.Options.SetRequestHeader("Origin", "https://www.curse.com");
			webSocket.Options.SetRequestHeader("Cookie", "CurseAuthToken=" + WebUtility.UrlEncode(authToken));
			webSocket.ConnectAsync(wsUri, CancellationToken.None).Wait();
		L	Listen();
		}
#elif WEBSOCKET4NET
		public void Connect(Uri wsUri, string authToken)
		{
			var cookies = new List<KeyValuePair<string, string>>();
			cookies.Add(new KeyValuePair<string, string>("CurseAuthToken", WebUtility.UrlEncode(authToken)));
			webSocket = new WebSocket(wsUri.ToString(), cookies: cookies, origin: "https://www.curse.com");

			var semaphore = new SemaphoreSlim(0,1);
			webSocket.Opened += (sender, args) =>
			{
				semaphore.Release();
			};
			webSocket.Error += ErrorHandler;
			webSocket.Closed += ClosedHandler;
			webSocket.DataReceived += DataReceivedHandler;
			webSocket.MessageReceived += MessageHandler;
			webSocket.Open();
			semaphore.Wait();
			pingTimer = new Timer(state =>
			{
				if (webSocket.State == WebSocketState.Open)
				{
					SendMessage(PingRequest.Create());
				}
			}, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
		}

		private void ErrorHandler(object sender, ErrorEventArgs errorEventArgs)
		{
			throw new NotImplementedException();
		}

		private void ClosedHandler(object sender, EventArgs eventArgs)
		{
			SocketClosed?.Invoke();
		}

		private void DataReceivedHandler(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			throw new NotImplementedException();
		}

		private void OpenedHandler(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		private void MessageHandler(object sender, MessageReceivedEventArgs args)
		{
			Task.Run(() => ProcessMessage(SocketResponse.Deserialise(args.Message)));
		}

#else
		public void Connect(Uri wsUri, string authToken)
		{
			webSocket = new WebSocket(wsUri.ToString());
			webSocket.SetCookie(new Cookie("CurseAuthToken", authToken));
			webSocket.Origin = "https://www.curse.com";
			webSocket.OnMessage += MessageHandler;
			webSocket.OnError += SocketErrorHandler;
			webSocket.Connect();
		}

		private void SocketErrorHandler(object sender, ErrorEventArgs errorEventArgs)
		{
			;
		}
		private void MessageHandler(object sender, MessageEventArgs ev)
		{
			Task.Run(() => ProcessMessage(SocketResponse.Deserialise(ev.Data)));
		}
#endif

		public void Login(string machineKey, string sessionId, int userId)
		{
			SendMessage(LoginRequest.Create(machineKey, sessionId, userId));
		}

		public async void SendMessage(SocketRequest message)
		{
			var messageText = JsonConvert.SerializeObject(message);
#if NETSOCKET
			await webSocket.SendMessage(messageText);
#elif WEBSOCKET4NET
			await Task.Run(() => webSocket.Send(messageText));
#else
			var semaphore = new SemaphoreSlim(0, 1);
			webSocket.SendAsync(messageText, success =>
			{
				semaphore.Release();
			});
			await semaphore.WaitAsync();
#endif
		}

#if NETSOCKET
		private void Listen()
		{
			Task.Run(() =>
			{
				while (webSocket.CloseStatus == null)
				{
					try
					{
						var message = webSocket.ReceiveMessage().Result;
						var parsed = SocketResponse.Deserialise(message);
						ProcessMessage(parsed);
					}
					catch (WebSocketException)
					{
						Debugger.Break();
						// TODO: handle disconnect
					}

				}
			});
		}
#endif

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
					LoginReceived?.Invoke((SocketLoginResponse)message.Body);
					break;
				case ResponseType.MessageChanged:
					MessageChanged?.Invoke((MessageChangedResponse)message.Body);
					break;
				case ResponseType.ChannelStatusChanged:
					ChannelStatusChanged?.Invoke((ChannelStatusChangedResponse)message.Body);
					break;
				case ResponseType.Unknown1:
					break;
				case ResponseType.FriendshipStatus:
					FriendshipStatusChanged?.Invoke((FriendshipStatusResponse)message.Body);
					break;
				case ResponseType.FriendRemoved:
					FriendRemoved?.Invoke((FriendRemovedResponse)message.Body);
					break;
				default:
					break;
			}
		}

		public void Dispose()
		{
			webSocket?.Dispose();
			pingTimer?.Dispose();
		}

		public void SendRawMessage(string message)
		{
			webSocket.Send(message);
		}
	}
}
