const node = document.getElementById("search")

function go() {
    location.href = '/search/' + node.value
}

node.addEventListener("keyup", function (e) {
    if (e.key === "Enter") {
        go()
    }
})