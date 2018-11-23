function ChatMessagesViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.Sender = ko.observable();
    self.Contents = ko.observable();
    self.Destination = ko.observable();
    self.DateSent = ko.observable();
    self.DateReceived = ko.observable();

}