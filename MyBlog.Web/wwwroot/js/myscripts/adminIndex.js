// 更新数据库中的标签
$("#updataTag").click(function () {
    $.ajax({
        url: "/Admin/RefreshTag",
        type: "post",
        success: function (res) {
            alert(res.msg);
        }
    });
    return false;
});

//// 退出登录
//$("#loginOut").click(function () {
//    $.ajax({
//        url: "/Admin/LoginOut",
//        type: "get",
//        success: function (res) {
//            if (res.code == 1) {
//                location.href = res.url;
//            } else {
//                alert(res);
//            }

//        }
//    });
//});

// 清理异常文章数据
$("#clearExceptionPosts").click(function () {
    $.ajax({
        url: "/Admin/RefreshPost",
        type: "post",
        success: function (res) {
            if (res.code == 1) {
                alert(res.msg);
            } else {
                alert(res.msg);
            }
        }
    });
    return false;
});