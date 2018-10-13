var $txtNewPwd, $txtConfirmNewPwd, $txtOldPwd;

$txtNewPwd = $("#txtNewPwd");
$txtConfirmNewPwd = $("#txtConfirmNewPwd");
$txtOldPwd = $("#txtOldPwd");


function begin() {
    if ($txtNewPwd.val() != $txtConfirmNewPwd.val()) {
        alert("两次输入密码不一致");
        return false;
    }
}

function success(res) {
    if (res.code == 1) {
        location.href = res.url;
    } else {
        alert(res.msg);
    }
}