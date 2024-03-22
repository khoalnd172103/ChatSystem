const connectionGroupChat = new signalR.HubConnectionBuilder()
    .withUrl("/groupChat")
    .build();

connectionGroupChat.start().then(function () {
    console.log("SignalR connection established.");
}).catch(function (err) {
    console.error("Error starting SignalR connection:", err.toString());
});

connectionGroupChat.on("ReceiveUpdatedConversationName", function (conversationId, newGroupName) {
    console.log("Received updated conversation name:", newGroupName);

    console.log($(".card-title"));

    $(".card-title").text(newGroupName);
});


connectionGroupChat.on("UserKickedFromGroup", function (conversationId, userId) {
    console.log("User kicked from group. Conversation ID:", conversationId, "User ID:", userId);

    // Find the li element containing the kicked participant's information
    const participantLi = $(`#group-members-list li[id="user-list"] a.kick-participant[data-participant-id="${userId}"][data-conversation-id="${conversationId}"]`).closest("li");

    console.log("Element to remove:", participantLi);

    if (participantLi.length > 0) {
        // Remove the li element from the DOM
        participantLi.remove();
        console.log("Element removed successfully.");
    } else {
        console.error("Error: Element not found or unable to remove.");
    }
});