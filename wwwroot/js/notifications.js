let conn = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("/bidnotifications").build();

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

conn.on("ReceiveOutbidNotification", function (names) {
    if (names.split(",").includes(curname)) {
        notif.error('You have been outbid<br><a href ="test.com">Placeholder link</a>');
    }
});

conn.start()