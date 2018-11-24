function ChatMessagesViewModel() {
    var self = this;

    self.SendMessage = messageData => $.ajax({
        url: '/api/ChatMessages',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(messageData)
    });
}