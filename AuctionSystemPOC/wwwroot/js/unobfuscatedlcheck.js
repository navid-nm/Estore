/*
 * Unobfuscated listingcheck.js
 * For reference prior to obfuscation. To be removed before deployment.
 */
document.getElementById("bidbutton").addEventListener("click", function (event) {
    event.preventDefault()
    if (document.getElementById("sellername").innerHTML != document.getElementById("sname").innerHTML
        && document.getElementById("amount").value > document.getElementById("price").innerHTML) {
        conn.invoke("NotifyOutbid", document.getElementById("itemid").innerHTML)
    }
    setTimeout(function () { }, 1000)
    document.getElementById("listform").submit()
});
