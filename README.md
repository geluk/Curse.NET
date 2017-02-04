# Curse.NET
A C#/.NET Curse Client API

This library has been created by reverse engineering the Curse client protocol,
since Curse does not officially expose an API for developers. Because of this,
there is no API documentation available yet.

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
