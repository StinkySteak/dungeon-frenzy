## Installation

### Prerequisites

Unity Editor version 2022 or later.

Install Netick 2 before installing this package.
https://github.com/NetickNetworking/NetickForUnity

### Steps

- Open the Unity Package Manager by navigating to Window > Package Manager along the top bar.
- Click the plus icon.
- Select Add package from git URL
- Enter https://github.com/karrarrahim/UnityTransport-Netick.git
- You can then create an instance by by double clicking in the Assets folder and going to Create->Netick->Transport->UnityTransportProvider

### Supported Protocol
- WebSocket (TCP)
- UDP

## Feature
- Run multiple protocols in a single machine
  - WebSocket & UDP
  - Allowing players from WebGL and Native device to connect to the server.

If there were multiple protocols running, the selected port would be:
```
startPort = 7777;
protocolPort = 7777 + n;

Example:
- UDP Starts on 7777
- WS Starts on 7778
```

## Relay
Unity Transport is compatible with Unity Relay. You have to clone this repo and modify it by yourself to make it compatible with Relay.

1. Add more enum type to the ClientNetworkProtocol and ServerNetworkProtocol with name of `Relay`
2. Create a public static variable to hold `Allocation` (Host) and `JoinAllocation` (Client)
   - It's up to you where to store the static variable.
4. Create a function to construct the driver for relay
```cs
private NetworkDriver ConstructDriverRelay()
{
			RelayServerData relayData;
			if (Engine.IsServer)
			{
				relayData = new RelayServerData(allocation, "udp");
			}
			else
			{
				relayData = new RelayServerData(joinAllocation, "udp");
			}
 
			var settings = GetNetworkSettings();
			settings.WithRelayParameters(ref relayData);
 
			// Create the Host's NetworkDriver from the NetworkSettings.
			var hostDriver = NetworkDriver.Create(settings);
			// Bind to the Relay server.
			return hostDriver;
}
```
4. Register the driver in `Run` method
```cs
public override void Run(RunMode mode, int port)
		{
			// ....
			NetworkDriver relayDriver = ConstructDriverRelay();
			MultiNetworkDriver multiDriver = MultiNetworkDriver.Create();
 
			if (Engine.IsServer)
			{
				if (ServerProtocol == ServerNetworkProtocol.All)
				{
					// ...
					BindAndListenDriverTo(in relayDriver, port + 2);
				}

        // ...
 
				if (ServerProtocol == ServerNetworkProtocol.Relay)
					BindAndListenDriverTo(in relayDriver, port);
			}
 
			// ...
			multiDriver.AddDriver(relayDriver);
		}
```
5. Before Launching netick, make sure to use `CreateAllocation` and `GetJoinCode` for Host and supply to the `Allocation` variable. While client needs to call `JoinAllocationAsync` and supply it to the public variable.


