# Curse.NET
A C#/.NET Curse Client API


## Usage

Connecting to Curse:
```csharp
var client = new CurseClient();
client.OnMessageReceived += (message) => {
    var channel = client.ChannelMap[message.ConversationID];
    Console.WriteLine($"[{channel.GroupTitle}] {message.SenderName}: {message.Body}")
}
client.Connect("username", "password");

```