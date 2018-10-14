var testEditor;
$(function () {
    testEditor = editormd("test-editormd", {
        width: "100%",
        height: 640,
        syncScrolling: "single",
        saveHTMLToTextarea: true,
        path: "/packages/editormd/lib/",
        htmlDecode: true,
        imageUpload: true,
        imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
        imageUploadURL: "/file/UpLoadImg",
        /*
             上传的后台只需要返回一个 JSON 数据，结构如下：
             {
                success : 0 | 1,           // 0 表示上传失败，1 表示上传成功
                message : "提示的信息，上传成功或上传失败及错误信息等。",
                url     : "图片地址"        // 上传成功时才返回
             }
             */


    });



    $("form").submit(function (e) {

        $("#Content").val(testEditor.getHTML());
    });

});