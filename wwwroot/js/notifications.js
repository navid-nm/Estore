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

conn.on("ReceiveOutbidNotification", function () {
    notif.error('You have been outbid' +
        '<br><a href ="test.com">Placeholder link</a>');
});

conn.start().then(function () {
    itemid = "2"
    conn.invoke("JoinBiddersGroup", itemid).catch(function (err) {
        return console.error(err.toString());
    });
    //Then notify everyone else in the group that they have been outbid
    conn.invoke("NotifyOutbid", itemid)
})

function joinbidders() {
    //Add the bidder to the group
    var itemid = document.getElementById("itemid").innerHTML;
    console.log(itemid)
}