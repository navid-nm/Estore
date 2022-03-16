let conn = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("/notifications").build();

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
            background: '#39AEA0',
            dismissible: true
        }
    ]
});

var thisUser = document.getElementById("cur-username").innerText


//Handling received calls.
conn.on("ReceiveMessageNotification", function (username, recipient) {
    if (recipient == thisUser) {
        notif.error('You have received a message from ' + username + '<br><a href="/my/messages/">Read</a>');
    }
});

conn.on("ReceiveSaleNotification", function (seller, buyer, item, findcode) {
    if (seller == thisUser) {
        notif.error('Your item <a href="/item/' + findcode + '">"'
                                                + item + '"</a> has sold.<br>Buyer: ' + buyer);
    }
});

conn.start()


//Sending calls.
$("#message-form").on("submit",
    function (e) {
        e.preventDefault();
        $("#hidden").val($("#message-editor").find(".ql-editor").html());
        if (document.getElementById("message-editor").innerText.length > 1) {
            conn.invoke("NotifyOfMessage",
                document.getElementById("cur-recipient-name").innerText,
                document.getElementById("cur-username").innerText
            );
        }
        timedSubmit("message-form")
    }
)

$("#buy-form").on("submit",
    function (e) {
        e.preventDefault();
        conn.invoke("NotifyOfSale",
            document.getElementById("this-seller").innerText,
            document.getElementById("cur-username").innerText,
            document.getElementById("this-item").innerText,
            document.getElementById("this-item-findcode").innerText
        );
        timedSubmit("buy-form")
    }
)

function timedSubmit(form) {
    setTimeout(function () { }, 1000);
    document.getElementById(form).submit();
}