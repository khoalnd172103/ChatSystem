"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/friendRequest").build();

connection.start().then(res => {
}).catch(err => {
    console.log(err);
});

connection.on("OnSendFriendRequest", function () {
    console.log("OnSendFriendRequest work");
    toastr.success('You have new friend request');
}).catch(err => {
    console.log(err);
});