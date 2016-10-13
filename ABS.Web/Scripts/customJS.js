(function () {
    var app = angular.module('chat-app', []);
    debugger
    app.controller('ChatController', function ($scope) {
        debugger
        // scope variables
        $scope.name = 'Guest'; // holds the user's name
        $scope.message = ''; // holds the new message
        $scope.messages = []; // collection of messages coming from server
        $scope.chatHub = null; // holds the reference to hub

        $scope.chatHub = $.connection.chatHub; // initializes hub
        $.connection.hub.start(); // starts hub

        // register a client method on hub to be invoked by the server
        $scope.chatHub.client.broadcastMessage = function (name, message) {
            debugger
            var newMessage = name + ' says: ' + message;

            // push the newly coming message to the collection of messages
            $scope.messages.push(newMessage);
            $scope.$apply();
            debugger
        };

        $scope.newMessage = function () {
            debugger
            // sends a new message to the server
            $scope.chatHub.server.sendMessage($scope.name, $scope.message);

            $scope.message = '';
            debugger
        }
    })
}());