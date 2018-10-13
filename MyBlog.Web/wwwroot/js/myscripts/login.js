var $timestamp, $img = $("#img"), $txtVerifyCode = $("input[name='VerifyCode']"), js = 0;


$img.click(function () {
    $timestamp = new Date().getTime();
    $(this).attr("src", "/Account/VerifyCode?time=" + $timestamp)
});

$txtVerifyCode.focus(function () {
    if (js++ == 0)
        $img.click();
});

function success(res) {
    if (res.code == 1) {
        location.href = res.url;
    }
    else {
        alert(res.msg);
        $("#img").click();
    }
}