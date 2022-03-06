const node = document.getElementById("search")

function go() {
    location.href = '/search/' + node.value
}

node.addEventListener("keyup", function (e) {
    if (e.key === "Enter") {
        go()
    }
})

if (location.pathname == "/") {
    let carContainer = document.getElementById("viewed-carousel"), ap = false, itemc = 4;
    if (carContainer.childElementCount > 3) {
        itemc = 5;
        ap = true;
    }
    var owl = $('.owl-carousel');
    owl.owlCarousel({
        items: itemc,
        margin: 10,
        nav: true,
        center: true,
        loop: true,
        autoplay: ap,
        autoplayTimeout: 4000,
        autoplayHoverPause: true,
        checkVisibility: false
    });
    $('.play').on('click', function () {owl.trigger('play.owl.autoplay', [1000])})
    $('.stop').on('click', function () {owl.trigger('stop.owl.autoplay')})
}