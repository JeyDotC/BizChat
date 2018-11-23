function ChatRoomsViewModel() {
    var self = this;

    self.Listed = ko.observableArray();

    self.List = () => $.get(
        '/api/ChatRooms/my',
        results => self.Listed($.map(results, ChatRoomViewModel.FromData)));

    self.Add = chatRoom => $.post('/api/ChatRooms/' + chatRoom.Title());
}