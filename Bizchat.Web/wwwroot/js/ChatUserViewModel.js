function ChatUserViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.UserId = ko.observable();
    self.Name = ko.observable();
}

ChatUserViewModel.FromData = data => {
    var chatUser = new ChatUserViewModel();

    chatUser.Id(data.id);
    chatUser.UserId(data.userId);
    chatUser.Name(data.name);

    return chatUser;
};