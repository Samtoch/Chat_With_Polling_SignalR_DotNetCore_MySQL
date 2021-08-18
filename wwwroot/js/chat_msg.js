let conectChat = new signalR.HubConnectionBuilder()
.withUrl("/chathub")
.configureLogging(signalR.LogLevel.Information)
.build();

document.getElementById("SendMessageBtn").disabled = true;

conectChat.start().then(function () {
    document.getElementById("SendMessageBtn").disabled = false;
    console.log("[signalR Conected]");
}).catch(function (err) {
    return console.error(err.toString());
});


conectChat.on("ReceiveMessage", function (user, message) {
    let msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let encodedMsg = user + " :   " + msg;
    let li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").replaceChild(li);
    //document.getElementById("messagesList").appendChild(li);
});


document.getElementById("SendMessageBtn").addEventListener("click", function (event) {
    event.preventDefault();
    let user = document.getElementById("userInput").value;
    let message = document.getElementById("messageInput").value;
    conectChat.invoke("SendMessage", user, message).then(value => {
        console.log(value);
        console.log("msg send")
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});