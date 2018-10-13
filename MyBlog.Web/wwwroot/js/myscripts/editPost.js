$(function () {
    load();
})


function load() {

    //var editor = new E('#my_editor')
    //// 或者 var editor = new E( document.getElementById('editor') )
    //editor.create()

    //var editor = new wangEditor('my_editor');
    //editor.create()

    var E = window.wangEditor;
    var editor = new E(document.getElementById('my_editor'))
    editor.create();
    editor.customConfig.showLinkImg = false;
    //// 普通的自定义菜单
    //editor.config.menus = [
    //    'source',
    //    '|',
    //    'bold',
    //    'underline',
    //    'italic',
    //    'strikethrough',
    //    'eraser',
    //    'forecolor',
    //    'bgcolor',
    //    '|',
    //    'quote',
    //    'fontfamily',
    //    'fontsize',
    //    'head',
    //    'unorderlist',
    //    'orderlist',
    //    'alignleft',
    //    'aligncenter',
    //    'alignright',
    //    '|',
    //    'link',
    //    'unlink',
    //    'table',
    //    'emotion',
    //    '|',
    //    'img',
    //    'insertcode',
    //    '|',
    //    'undo',
    //    'redo',
    //    'fullscreen'
    //];
    //// 取消粘贴过滤
    //editor.config.pasteFilter = false;


    //// 上传图片（举例）
    //editor.config.uploadImgUrl = "/File/UpLoadImg";
    //// 配置自定义参数（举例）
    //editor.config.uploadParams = {
    //    token: 'abcdefg',
    //    user: 'wangfupeng1988'
    //};

    //// 设置 headers（举例）
    //editor.config.uploadHeaders = {
    //    'Accept': 'text/x-json'
    //};


    //// 隐藏掉插入网络图片功能。该配置，只有在你正确配置了图片上传功能之后才可用。
    //editor.config.hideLinkImg = true;

    //editor.create();
}



function success(res) {
    if (res.code == 1) {
        location.href = res.url;
    } else {
        alert(res.msg);
    }
}