// Write your Javascript code.


var $form = null;
$(function () {

    $form = $('#fileupload').fileupload({
        dataType: 'json',
        autoUpload: false,
        maxFileSize: 99900000, // 99.9MB
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|pdf|xls|xlsx|zip|rar)$/i
    });

});

//$('#fileupload').addClass('fileupload-processing');