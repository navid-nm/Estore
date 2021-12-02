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

conn.on("ReceiveOutbidNotification", function (names, itemid, itemname) {
    if (names.split(",").includes(curname) && curname != "") {
        notif.error('You have been outbid on the following item:<br><a href ="/listing/' + itemid + '">' +
            itemname + '</a>');
    }
});

conn.start()