"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/friendRequest").build();

connection.on("ReceiveFriendRequestNotification", function () {
    console.log("ReceiveFriendRequestNotification work");
    toastr.info("You have received a new friend request.");
});

function fulfilled() {
    console.log("Friend request connection established.");
}

function rejected() {

}

connection.start()
    .then(fulfilled, rejected)
    .catch(err => console.error(err.toString()));

document.addEventListener("click", function (event) {
    if (event.target && event.target.id === "addFriend") {
        console.log("connect to function success");
        var userId = event.target.value; // Get the userId from the button's value attribute
        connection.invoke("SendFriendRequestNotification").catch(function (err) {
            return console.error(err.toString());
        });
    }
});