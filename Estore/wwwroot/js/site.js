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
    let carContainer = document.getElementById("viewed-carousel"), ap = false, itemc = 1, lp = false,
        c = carContainer.childElementCount;
    if (c > 1) { ap = true; }
    if (c > 2) { lp = true; itemc = 5; }
    var owl = $('.owl-carousel');
    owl.owlCarousel({
        items: itemc,
        margin: 10,
        nav: true,
        center: true,
        loop: lp,
        autoplay: ap,
        autoplayTimeout: 4000,
        autoplayHoverPause: true,
        stagePadding: 0.5,
        checkVisibility: false
    });
    $('.play').on('click', function () {owl.trigger('play.owl.autoplay', [1000])})
    $('.stop').on('click', function () {owl.trigger('stop.owl.autoplay')})
}