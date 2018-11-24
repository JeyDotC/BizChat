function ChatMessageReceivedByChatRoomViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.ChatMessage = ko.observable(new ChatMessageViewModel());
    self.ChatRoom = ko.observable(new ChatRoomViewModel());
    self.DateReceived = ko.observable();
}

ChatMessageReceivedByChatRoomViewModel.FromData = data => {
    var receivedMessage = new ChatMessageReceivedByChatRoomViewModel();

    receivedMessage.Id(data.id);
    receivedMessage.ChatMessage(ChatMessageViewModel.FromData(data.chatMessage));
    receivedMessage.ChatRoom(ChatRoomViewModel.FromData(data.chatRoom));
    receivedMessage.DateReceived(data.dateReceived);

    return receivedMessage;
};