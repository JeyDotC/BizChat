function ChatMessageViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.Sender = ko.observable();
    self.Contents = ko.observable();
    self.Destination = ko.observable();
}

ChatMessageViewModel.FromData = data => {
    var chatMessage = new ChatMessageViewModel();

    chatMessage.Id(data.id);
    chatMessage.Sender(data.sender);
    chatMessage.Contents(data.contents);
    chatMessage.Destination(data.destination);

    return chatMessage;
};