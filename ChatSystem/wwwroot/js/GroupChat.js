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
