var $submit = $("input[type='submit']");

$submit.click(function () {
    $submit.css("disabled", "true");
});

function success(res) {
    if (res.code == 1) {
        location.href = res.url;
    } else {
        alert(res.msg);
    }
    $submit.css("disabled", "false");
}