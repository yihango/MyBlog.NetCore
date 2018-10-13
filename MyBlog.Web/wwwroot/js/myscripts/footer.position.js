positionFooter();
function positionFooter() {
    var height = $(window).height() - $(document.body).height();
    if (height >= 1) {
        $("#footer").css("margin-top", (height + 30));
    } else {
        $("#footer").css("margin-top", 30);
    }
}

$(window).resize(function () {
    positionFooter();
});
