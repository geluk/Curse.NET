#define WEBSOCKET4NET
// #define NETSOCKET

using System;
using System.Threading;
using System.Threading.Tasks;
using Curse.NET.SocketModel;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
#if NETSOCKET
using System.Net;
using System.Net.WebSockets;
#elif WEBSOCKET4NET
using WebSocket4Net;
using System.Collections.Generic;
using System.Net;
#else
using WebSocketSharp;
using WebSocketSharp.Net;
#endif

namespace Curse.NET
{
	internal class SocketApi
	{
#if NETSOCKET
		private readonly ClientWebSocket webSocket = new ClientWebSocket();
#else
		private WebSocket webSocket;
#endif

		public event SocketMessageReceivedEvent MessageReceived;
		public event ChannelMarkedReadEvent ChannelMarkedRead;
		public event MessageChangedEvent MessageChanged;
		public event UserActivityChangeEvent UserActivityChange;
		public event Action SocketClosed;

#if NETSOCKET
		public void Connect(Uri wsUri, string authToken)
		{
			webSocket.Options.SetRequestHeader("Origin", "https://www.curse.com");
			webSocket.Options.SetRequestHeader("Cookie", "CurseAuthToken=" + WebUtility.UrlEncode(authToken));
			webSocket.ConnectAsync(wsUri, CancellationToken.None).Wait();
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

		public void Listen()
		{
#if NETSOCKET
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
#endif
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
