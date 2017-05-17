var $time = document.getElementById("time");
var js = 9;
var $interval = setInterval("myInterval()", 1000);
function myInterval() {
    if ( --js==0) {
        this.location.href = "/"; clearTimeout($interval);
    } else {
        $time.innerText = js;
    }
}