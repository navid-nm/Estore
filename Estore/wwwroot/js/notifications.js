let conn = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("/msgs").build();

let notif = new Notyf({
    duration: 9000,
    ripple: false,
    position: {
        x: 'right',
        y: 'top',
    },
    types: [
        {
            icon: false,
            type: 'error',
            background: 'indianred',
            dismissible: true
        }
    ]
});

conn.on("ReceiveMessageNotification", function (username, recipient) {
    if (recipient == document.getElementById("cur-username").innerText) {
        notif.error('You have received a message from ' + username + '<br/> <a href="/message/">Read</a>');
    }
});

conn.start()