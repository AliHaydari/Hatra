// Write your Javascript code.


var $form = null;
$(function () {

    $form = $('#fileupload').fileupload({
        dataType: 'json',
        autoUpload: false,
        maxFileSize: 99900000,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|pdf|xls|xlsx)$/i
    });

});

//$('#fileupload').addClass('fileupload-processing');