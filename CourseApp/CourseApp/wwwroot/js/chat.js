"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
document.getElementById("groupAddButton").disabled = true;
document.getElementById("groupSendButton").disabled = true;
//document.getElementById("directSendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessageGroup", function (user, group, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "Group message in " + group + " from " + user + ": " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessageBlank", function (message) { //Used to Recieve a message without appending "User says ...."
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = "";
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {  //Invokes SendMessage in ChatHub.cs
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("groupSendButton").addEventListener("click", function (event) {
    var group = document.getElementById("groupMessageGroup").value;
    var message = document.getElementById("groupMessageInput").value;
    connection.invoke("SendGroupMessage", group, message).catch(function (err) {  //Invokes SendGroupMessage in ChatHub.cs
        return console.error(err.toString());
    });
    event.preventDefault();
});



document.getElementById("groupAddButton").addEventListener("click", function (event) {
    var user = "";
    var group = document.getElementById("groupInput").value;
    connection.invoke("AddToGroup", group).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});




connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("groupAddButton").disabled = false;
    document.getElementById("groupSendButton").disabled = false;
    //document.getElementById("directSendButton").disabled = false;


}).catch(function (err) {
    return console.error(err.toString());
});