# Curse.NET
Curse.NET is a C#/.NET library for interfacing with the Curse Client API.

This library has been created by reverse engineering the Curse client protocol,
since Curse does not officially expose an API for developers. Because of this,
there is no API documentation available yet.

## Installation

Install the NuGet package to your project.
```
Install-Package Curse.NET
```
Alternatively (in case you need the latest version), clone the project, build the DLL, and reference it.

## Usage

Connecting to Curse:
```csharp
var client = new CurseClient();
client.OnMessageReceived += (server, channel, message) => {
    Console.WriteLine($"[{channel.GroupTitle}] {message.SenderName}: {message.Body}")
}
client.Connect("username", "password");

```
## Development

The library is still in active development, so its public API can and will change between commits.

Features will mostly be added on an as-needed basis.
If you require a specific feature that isn't yet implemented, feel free to create an issue for it.
