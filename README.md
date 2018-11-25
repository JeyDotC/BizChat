### Before build

Restore the database:

```cmd
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context BizchatDbContext
```

> Avoid using the Nuget package manager to run the above commands, it uses to cause problems.

### Running the solution

This is a multi app solution, it should start the **Bizchat.Web** and **Bizchat.DeliverToChatRoomApp.NServicebus** projects when you hit 'Start' button.

Take into account that UI is not too sophisticated, so, actions like, sending the form info when hitting enter, or performing searchs while typing in a search box, are not present. You'll have to click on buttons.

### Missing parts

#### There are no unit tests

I made so many changes to the architecture while developing that syncing them would have taken too much time.

#### The transport for messages is not RabbitMQ

This will appply to the bonus part (The bot service) which I'll add in a few moments. 

Though since I used an abstraction (NServiceBus), configuring RabbitMQ should be easy for anyone with good knowledge of that technology.

### SignalR has no Authentication or Groups

SignalR was the last part I added and it gave me a lot of trouble (Due to its interaction with NServiceBus). So, I didn't have the time for those aspects :(

### Regions of code to ignore

You may ignore these regions of code, since those are auto-generated:

* Bizchat.Web/Areas/Identity
* Bizchat.Web/Data
* Bizchat.Web/Migrations
* Bizchat.Web/wwwroot/lib (obviously)