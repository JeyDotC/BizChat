function ChatUsersViewModel() {
    var self = this;

    self.LoadedUsers = ko.observableArray();

    self.SearchUsers = q => $.get(
        '/api/ChatUsers/Search', 
        { q: q },
        result => self.LoadedUsers($.map(result, ChatUserViewModel.FromData)));
}
