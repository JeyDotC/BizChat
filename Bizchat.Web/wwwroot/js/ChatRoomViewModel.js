function ChatRoomViewModel() {
    var self = this;

    self.Id = ko.observable();

    self.Title = ko.observable();

    self.Members = ko.observableArray([]);

    self.ListMembers = () => $.get(
        '/api/ChatRooms/' + self.Id() + '/Members',
        result => self.Members($.map(result, ChatUserViewModel.FromData)));

    self.AddMember = user => $.post(
        '/api/ChatRooms/' + self.Id() + '/Add/' + user.Name()
    );
}

ChatRoomViewModel.FromData = data => {
    var chatRoom = new ChatRoomViewModel();

    chatRoom.Id(data.id);
    chatRoom.Title(data.title);

    return chatRoom;
};