# Curse.NET
A C#/.NET Curse Client API

## Installation

Install the NuGet package to your project.
```
Install-Package Curse.NET
```
Alternatively, clone the project, build the DLL, and reference it.

## Usage

Connecting to Curse:
```csharp
var client = new CurseClient();
client.OnMessageReceived += (server, channel, message) => {
    Console.WriteLine($"[{channel.GroupTitle}] {message.SenderName}: {message.Body}")
}
client.Connect("username", "password");

```
