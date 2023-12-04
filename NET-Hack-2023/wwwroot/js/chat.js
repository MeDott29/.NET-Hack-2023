import * as signalR from "@microsoft/signalr";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

document.getElementById("sendButton").addEventListener("click", function (event) {
    let message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(err => console.error(err.toString()));
    event.preventDefault();
});

connection.on("ReceiveMessage", function (message) {
    let li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(err => console.error(err.toString()));
