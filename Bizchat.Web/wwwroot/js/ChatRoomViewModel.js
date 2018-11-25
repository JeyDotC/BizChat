function ChatRoomViewModel() {
    var self = this;

    // ViewModels
    self.ChatMessages = new ChatMessagesViewModel();

    // Observables
    self.Id = ko.observable();
    self.Title = ko.observable();
    self.Members = ko.observableArray([]);

    self.NewMessageText = ko.observable();

    // Actions
    self.ListMembers = () => $.get(
        '/api/ChatRooms/' + self.Id() + '/Members',
        result => self.Members($.map(result, ChatUserViewModel.FromData)));

    self.AddMember = user => $.post(
        '/api/ChatRooms/' + self.Id() + '/Add/' + user.Name()
    );

    self.SendMessage = () => self.ChatMessages.SendMessage({
        contents: self.NewMessageText(),
        destination: CalculateDestination(self.NewMessageText()) + '://' + self.Id()
    }).done(() => self.NewMessageText(''));

    function CalculateDestination(contents) {
        return /^\/([a-z]+)/ig.test(contents) ? 'bot' : 'chatroom';
    }
}

ChatRoomViewModel.FromData = data => {
    var chatRoom = new ChatRoomViewModel();

    chatRoom.Id(data.id);
    chatRoom.Title(data.title);

    return chatRoom;
};