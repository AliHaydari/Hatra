// Write your Javascript code.


var $form = null;
$(function () {

    $form = $('#fileupload').fileupload({
        dataType: 'json',
        autoUpload: true,
        maxFileSize: 99900000,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
    });

});

//$('#fileupload').addClass('fileupload-processing');