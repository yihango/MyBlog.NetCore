window.onload = function () {
    var oDiv = $(".tag-cloud")[0];
    var aA = $('.tag');
    var i = 0;
    for (i = 0; i < aA.length; i++) {
        aA[i].pause = 1;
        aA[i].time = null;
        initialize(aA[i]);
        aA[i].onmouseover = function () {
            this.pause = 0;
        };
        aA[i].onmouseout = function () {
            this.pause = 1;
        };
    }
    setInterval(starmove, 50);

    function starmove() {
        for (i = 0; i < aA.length; i++) {
            if (aA[i].pause) {
                domove(aA[i]);
            }
        }
    }

    function domove(obj) {
        if (obj.offsetTop <= -obj.offsetHeight) {
            obj.style.top = oDiv.offsetHeight + "px";
            initialize(obj);
        } else {
            obj.style.top = obj.offsetTop - obj.ispeed + "px";
        }
    }

    function initialize(obj) {
        var iLeft = parseInt(Math.random() * oDiv.offsetWidth);
        var scale = Math.random() * 1 + 1;
        var iTimer = parseInt(Math.random() * 1500);
        obj.pause = 0;

        obj.style.fontSize = 12 * scale + 'px';

        if ((iLeft - obj.offsetWidth) > 0) {
            obj.style.left = iLeft - obj.offsetWidth + "px";
        } else {
            obj.style.left = iLeft + "px";
        }
        clearTimeout(obj.time);
        obj.time = setTimeout(function () {
            obj.pause = 1;
        }, iTimer);
        obj.ispeed = Math.ceil(Math.random() * 4) + 1;
    }
};