function ChatMessagesReceivedByChatRoomsViewModel() {
    var self = this;

    self.Listed = ko.observableArray();

    self.LoadByChatRoom = chatRoomId => $.get(
        '/api/ChatRooms/' + chatRoomId + '/Received',
        data => self.Listed($.map(data, ChatMessageReceivedByChatRoomViewModel.FromData)));
}