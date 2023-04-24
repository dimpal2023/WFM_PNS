(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};  

    Wfm.App.Shift = {
        Create: function () {
            if (!Wfm.App.Shift.Validate()) {
                event.preventDefault();
                return;
            }
            var $form = $(this).parents('form');          

            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                data: $form.serialize(),                
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    window.location.href = response.Url;
                }
            });
        },
        Edit: function () {
            if (!Wfm.App.Shift.Validate()) {
                event.preventDefault();
                return;
            }

            var $form = $(this).parents('form');

            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                data: $form.serialize(),
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    window.location.href = response.Url;
                }
            });
        },
        Delete: function () {
            var gatePassId = $("#hiddenShiftId").val();

            $.ajax({
                type: "POST",
                url: "/Shift/Delete",
                data: { Id: gatePassId },
                success: function (response) {                                  
                    window.location.href = response.Url;
                }
            })
        },                
        Cancel: function () {            
            window.location.href = "/Shift/AllItems";
        },
        Validate: function () {            
            var return_val = true;
            var shiftName = $('#SHIFT_NAME').val().trim();
            var companyName = $('#COMPANY_ID').val().trim();
            var startTime = $('#SHIFT_START_TIME').val().trim();
            var endTime = $('#SHIFT_END_TIME').val().trim();
            var timeregex = /\d{2}:\d{2}:\d{2}/;

            if (shiftName == '') {
                var errorMessage = "The Name field is required.";
                $('#SHIFT_NAME').next('span').text(errorMessage).show();
                $('#SHIFT_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_NAME').next('span').text("");
                $('#SHIFT_NAME').next('span').addClass("field-validation-valid");
            }

            if (companyName == '') {
                var errorMessage = "The Company field is required";
                $('#COMPANY_NAME').next('span').text(errorMessage).show();
                $('#COMPANY_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#COMPANY_NAME').next('span').text("");
                $('#COMPANY_NAME').next('span').addClass("field-validation-valid");
            }

            if (startTime == '') {
                var errorMessage = "The Start Time field is required";
                $('#SHIFT_START_TIME').next('span').text(errorMessage).show();
                $('#SHIFT_START_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else if (!startTime.match(timeregex)) {
                var errorMessage = "Time format must be {00:00:00}.";
                $('#SHIFT_START_TIME').next('span').text(errorMessage).show();
                $('#SHIFT_START_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_START_TIME').next('span').text("");
                $('#SHIFT_START_TIME').next('span').addClass("field-validation-valid");
            }


            if (endTime == '') {
                var errorMessage = "The End Time field is required";
                $('#SHIFT_END_TIME').next('span').text(errorMessage).show();
                $('#SHIFT_END_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else if (!endTime.match(timeregex)) {
                var errorMessage = "Time format must be {00:00:00}.";
                $('#SHIFT_END_TIME').next('span').text(errorMessage).show();
                $('#SHIFT_END_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_END_TIME').next('span').text("");
                $('#SHIFT_END_TIME').next('span').addClass("field-validation-valid");
            }           

            return return_val;
        }
    };

    function initialize() {        
        $('#submit').on("click", Wfm.App.Shift.Create);
        $('#edit').on("click", Wfm.App.Shift.Edit);
        $('#cancel').on("click", Wfm.App.Shift.Cancel);
        $('.btn-tbl-delete').each(function(){
            $(this).on("click", ConfirmDelete);
        });
        $('#shift_delete_confirm').on("click", Wfm.App.Shift.Delete);
        $("#COMPANIES").on("change", function () {
            var companiesId = $("input.select-dropdown").next().attr("id");
            var selectOptionId = $('#' + companiesId + " li.selected").attr("id");
            var selectedCompanyId = selectOptionId.split("options-")[1];

            //setting selected company id to hidden field
            $("#COMPANY_ID").val(selectedCompanyId);
        });

        if ($("#COMPANY_ID").val() !== '') {
                      
        }
        else if ($("#COMPANIES") !== undefined) {
            //setting default compnay id to hidden field
            var defaultCompanyId = $("#COMPANIES option:first").attr("value");
            $("input[type=hidden]").val(defaultCompanyId);
        }
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var shiftId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenShiftId").val(shiftId);        
    }        

    function ToDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear();
    }

    initialize();
})();