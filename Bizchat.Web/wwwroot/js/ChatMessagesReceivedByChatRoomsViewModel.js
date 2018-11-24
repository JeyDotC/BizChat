function ChatMessagesReceivedByChatRoomsViewModel() {
    var self = this;

    // Variables
    self.OnNewMessage = m => console.log(m);

    // Observables
    self.ListedFor = ko.observable();
    self.Listed = ko.observableArray();

    // Actions
    self.LoadByChatRoom = chatRoomId => {
        self.ListedFor(chatRoomId);
        return $.get(
            '/api/ChatRooms/' + chatRoomId + '/Received',
            data => self.Listed($.map(data, ChatMessageReceivedByChatRoomViewModel.FromData)));
    };

    self.SetupHub = () => {
        var hub = new signalR.HubConnectionBuilder()
            .withUrl("/chatMessagesHub")
            .build();

        hub.on("MessageReceivedByChatRoom", data => {
            if (data.chatRoom.id === self.ListedFor()) {
                var newMessage = ChatMessageReceivedByChatRoomViewModel.FromData(data);
                self.Listed.push(newMessage);
                self.OnNewMessage(newMessage);
            }
        });

        hub.start().catch(err => console.error(err.toString()));
    };

}