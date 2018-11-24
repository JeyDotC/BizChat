function ChatRoomsIndexViewModel() {
    var self = this;

    // Elements
    var $addChatroomModal = $('#add-chatroom-modal');
    var $chatroomMembersModal = $('#chatroom-members-modal');
    var $receivedMessages = $("#received-messages");

    // ViewModels
    self.ChatRooms = new ChatRoomsViewModel();
    self.ChatUsers = new ChatUsersViewModel();
    self.ReceivedMessages = new ChatMessagesReceivedByChatRoomsViewModel();

    // Observables
    self.NewChatRoom = ko.observable(new ChatRoomViewModel());
    self.ActiveChatRoom = ko.observable(new ChatRoomViewModel());
    self.UserSearchQuery = ko.observable();
    self.UserSearchResult = ko.computed(() => self.ChatUsers.LoadedUsers().filter(u => {
        return self.ActiveChatRoom().Members().findIndex(m => m.Id() === u.Id()) === -1;
    }));

    // Actions
    self.AddChatRoom = () => self.ChatRooms.Add(self.NewChatRoom())
        .done(() => {
            $addChatroomModal.modal('hide');
            self.NewChatRoom(new ChatRoomViewModel());
            self.ChatRooms.List();
        });

    self.SearchUsers = () => self.ChatUsers.SearchUsers(self.UserSearchQuery());

    self.AddUserToChatRoom = user => self.ActiveChatRoom().AddMember(user)
        .done(() => {
            self.ActiveChatRoom().Members.push(user);
        });

    self.LoadChatRoomMessages = () => {
        if (!self.ActiveChatRoom().Id()) {
            return;
        }

        var oldLength = self.ReceivedMessages.Listed().length;

        self.ReceivedMessages.LoadByChatRoom(self.ActiveChatRoom().Id())
            .done(() => {
                if (oldLength === self.ReceivedMessages.Listed().length) {
                    return;
                }
                ScrollChatWindowDown();
            });
    };

    function ScrollChatWindowDown() {
        var receivedMessages = $receivedMessages[0];
        receivedMessages.scrollTo(0, receivedMessages.scrollHeight);
    }

    function Init() {

        self.ReceivedMessages.OnNewMessage = ScrollChatWindowDown;

        self.ChatRooms.List().done(() => {
            Sammy(function () {
                this.get('#:id', function () {
                    var id = parseInt(this.params.id);
                    var activeChatRooms = self.ChatRooms.Listed().filter(c => c.Id() === id);
                    if (activeChatRooms.length > 0) {
                        self.ActiveChatRoom(activeChatRooms[0]);
                        self.ActiveChatRoom().ListMembers();
                        
                        self.LoadChatRoomMessages();
                    }
                });

            }).run();

            self.ReceivedMessages.SetupHub();
        });

        $addChatroomModal.on('hidden.bs.modal', function (e) {
            self.NewChatRoom(new ChatRoomViewModel());
        });
    }

    Init();
}

ko.applyBindings(new ChatRoomsIndexViewModel());