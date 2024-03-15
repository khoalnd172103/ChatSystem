"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/friendRequest").build();

connection.on("ReceiveFriendRequestNotification", function () {
    console.log("ReceiveFriendRequestNotification work");
    toastr.info("You have received a new friend request.");
});

connection.on("OnSendFriendRequest", function () {
    console.log("ReceiveFriendRequestNotification work");
    toastr.info("You have received a new friend request.");
});

function fulfilled() {
}

function rejected() {

}

connection.start()
    .then(fulfilled, rejected)
    .catch(err => console.error(err.toString()));

//document.addEventListener("click", function (event) {
//    if (event.target && event.target.id === "addFriend") {
//        var recipientUserId = event.target.dataset.recipientUserId;
//        connection.invoke("SendFriendRequestNotification", recipientUserId).catch(function (err) {
//            return console.error(err.toString());
//        });
//    }
//});