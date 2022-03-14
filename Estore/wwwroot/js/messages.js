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
        setTimeout(function () { }, 1000);
        document.getElementById("message-form").submit();
    }
)