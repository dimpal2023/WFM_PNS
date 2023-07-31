'use strict';
$(function () {

    $('select').formSelect();

    //Dropzone
    Dropzone.options.frmFileUpload = {
        paramName: "file",
        maxFilesize: 10
    };

    $('.datepicker').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY',
        clearButton: true,
        weekStart: 1,
        time: false
    });
    $('.datepicker2').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY',
        clearButton: true,
        weekStart: 1,
        time: false
    });
    $('.timepicker').bootstrapMaterialDatePicker({
        format: 'HH:mm:ss',
        clearButton: true,
        date: false
    });

});
