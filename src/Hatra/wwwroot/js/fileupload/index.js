// Write your Javascript code.


var $form = null;
$(function () {

    $form = $('#fileupload').fileupload({
        dataType: 'json',
        autoUpload: false,
        maxFileSize: 99900000, // 99.9MB
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|svg|tif|pdf|xls|xlsx|zip|rar|apk)$/i
    });

});

//$('#fileupload').addClass('fileupload-processing');