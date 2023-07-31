"use strict"
var Wfm = {};
Wfm.App = {};
Wfm.App.GatePass = {};

(function () {
    Wfm.App.GatePass = {
        Create: function () {
            if (!Wfm.App.GatePass.Validate()) {
                event.preventDefault();
                return;
            }
            var $form = $(this).parents('form');

            $('#submit').prop('disabled', true);

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
            if (!Wfm.App.GatePass.Validate()) {
                event.preventDefault();
                return;
            }

            var id = $('#ID').val().trim();
            var deptId = $('#DEPT_ID').val().trim();
            var subDeptId = $('#SUBDEPT_ID').val().trim();
            var empType = $('#WF_EMP_TYPE').val().trim();
            var wfId = $('#WF_ID').val().trim();
            var startDate = $('#START_DATE').val().trim();
            var endDate = $('#END_DATE').val().trim();
            var outTime = $('#OUT_TIME').val().trim();
            var inTime = $('#IN_TIME').val().trim();
            var actualInTime = $('#ACTUAL_INTIME').val().trim();
            var actualOutTime = $('#ACTUAL_OUTTIME').val().trim();
            var mobileNo = $('#WORKFORCE_MOBILE_NO').val().trim();
            var purpose = $('#PURPOSE').val().trim();
            var remark = $('#REMARK').val().trim();
            var status = $('#STATUS').val().trim();

            //var $form = $(this).parents('form');

            $('#edit').prop('disabled', true);
            var gatepass = {
                ID: id,
                DEPT_ID: deptId,
                SUBDEPT_ID: subDeptId,
                WF_EMP_TYPE: empType,
                WORKFORCE: { WF_ID: wfId },                
                START_DATE: startDate,
                END_DATE: endDate,
                OUT_TIME: outTime,
                IN_TIME: inTime,
                ACTUAL_INTIME: actualInTime,
                ACTUAL_OUTTIME: actualOutTime,
                MOBILE_NO: mobileNo,
                PURPOSE: purpose,
                REMARK: remark,
                STATUS: status
            };

            gatepass = JSON.stringify({ 'gatepass': gatepass });

            $.ajax({
                type: "POST",
                url: "/GatePass/Edit",
                data: gatepass,
                contentType: "application/json",
                dataType: "json",
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    window.location.href = response.Url;
                }
            });
        },
        Delete: function () {
            var gatePassId = $("#hiddenGatePassId").val();

            $.ajax({
                type: "POST",
                url: "/GatePass/Delete",
                data: { Id: gatePassId },
                success: function (response) {
                    $("#myModal").modal("hide");                    
                    window.location.href = response.Url;
                }
            })
        },
        Search: function () {
            var validate = function () {
                var return_val = true;
                if ($('#WF_ID').val().trim() == '') {
                    var errorMessage = "The Emp Id field is required.";
                    $('#WF_ID').next('span').text(errorMessage).show();
                    $('#WF_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#WF_ID').next('span').text(errorMessage).hide();
                    $('#WF_ID').next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var gatePassId = $('#ID').val();
            var wfId = $('#WF_ID').val();
            var gatepass = { ID: gatePassId, WORKFORCE: { WF_ID: wfId } };
            $.ajax({
                type: "POST",
                url: "/GatePass/FindWorkforce",
                data: JSON.stringify(gatepass),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    SetWorkForceData(result);
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });            
        },
        Out: function(){
            var gatePassId = $("#hiddenGatePassId").val();

            $.ajax({
                type: "POST",
                url: "/GatePass/Out",
                data: { Id: gatePassId },
                success: function (response) {                    
                    window.location.href = response.Url;
                }
            })
        },
        In: function(){
            var gatePassId = $("#hiddenGatePassId").val();

            $.ajax({
                type: "POST",
                url: "/GatePass/In",
                data: { Id: gatePassId },
                success: function (response) {
                    window.location.href = response.Url;
                }
            })
        },
        Cancel: function () {            
            window.location.href = "/GatePass/AllItems";                
        },
        Validate: function () {            
            var return_val = true;
            var startDate = $('#START_DATE').val().trim();
            var endDate = $('#END_DATE').val().trim();
            var outTime = $('#OUT_TIME').val().trim();
            var inTime = $('#IN_TIME').val().trim();
            var timeregex = /\d{2}:\d{2}:\d{2}/;

            if ($('#DEPT_ID option:selected').text() == '--Select--') {
                var errorMessage = "Department is required.";
                $('#DEPT_ID').parent().siblings('span').text(errorMessage).show();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else
            {
                $('#DEPT_ID').parent().siblings('span').text("").hide();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-valid");
                return_val = true;
            }

            if ($('#SUBDEPT_ID option:selected').text() == '--Select--') {
                var errorMessage = "Sub Department is required.";
                $('#SUBDEPT_ID').parent().siblings('span').text(errorMessage).show();
                $('#SUBDEPT_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SUBDEPT_ID').parent().siblings('span').text("").hide();
                $('#SUBDEPT_ID').parent().siblings('span').addClass("field-validation-valid");                
            }

            if ($('#WF_EMP_TYPE option:selected').text() == '--Select--') {
                var errorMessage = "Employee Type is required.";
                $('#WF_EMP_TYPE').parent().siblings('span').text(errorMessage).show();
                $('#WF_EMP_TYPE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#WF_EMP_TYPE').parent().siblings('span').text("").hide();
                $('#WF_EMP_TYPE').parent().siblings('span').addClass("field-validation-valid");                
            }

            //if ($('#EMP_NAME').text() == '') {
            //    var errorMessage = "Employee Name is required.";
            //    $('#EMP_NAME').next('span').text(errorMessage).show();
            //    $('#EMP_NAME').next('span').addClass("field-validation-error");                
            //}
            //else
            //{
            //    $('#EMP_NAME').next('span').text(errorMessage).hide();
            //    $('#EMP_NAME').next('span').addClass("field-validation-valid");
            //}

            if (startDate == '') {
                var errorMessage = $('#START_DATE').attr('data-val-required');
                $('#START_DATE').next('span').text(errorMessage).show();
                $('#START_DATE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#START_DATE').next('span').text(errorMessage).hide();
                $('#START_DATE').next('span').addClass("field-validation-valid");
            }

            if (endDate == '') {
                var errorMessage = $('#END_DATE').attr('data-val-required');
                $('#END_DATE').next('span').text(errorMessage).show();
                $('#END_DATE').next('span').addClass("field-validation-error");               
                return_val = false;
            }
            else {
                $('#END_DATE').next('span').text(errorMessage).hide();
                $('#END_DATE').next('span').addClass("field-validation-valid");
            }

            if (outTime == '') {
                var errorMessage = $('#OUT_TIME').attr('data-val-required');
                $('#OUT_TIME').next('span').text(errorMessage).show();
                $('#OUT_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else if (!outTime.match(timeregex)) {
                var errorMessage = "Time format must be {00:00:00}.";
                $('#OUT_TIME').next('span').text(errorMessage).show();
                $('#OUT_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#OUT_TIME').next('span').text(errorMessage).hide();
                $('#OUT_TIME').next('span').addClass("field-validation-valid");
            }


            if (inTime == '') {
                var errorMessage = $('#IN_TIME').attr('data-val-required');
                $('#IN_TIME').next('span').text(errorMessage).show();
                $('#IN_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else if (!inTime.match(timeregex)) {
                var errorMessage = "Time format must be {00:00:00}.";
                $('#IN_TIME').next('span').text(errorMessage).show();
                $('#IN_TIME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#IN_TIME').next('span').text(errorMessage).hide();
                $('#IN_TIME').next('span').addClass("field-validation-valid");
            }

            if ($('#PURPOSE option:selected').text() == 'Purpose') {
                var errorMessage = "Purpose is required.";
                $('#PURPOSE').parent().siblings('span').text(errorMessage).show();
                $('#PURPOSE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else
            {
                $('#PURPOSE').parent().siblings('span').text("").hide();
                $('#PURPOSE').parent().siblings('span').addClass("field-validation-valid");
            }

            return return_val;
        }
    };

    function initialize() {
        $('#submit').on("click", Wfm.App.GatePass.Create);
        $('#edit').on("click", Wfm.App.GatePass.Edit);
        $('#cancel').on("click", Wfm.App.GatePass.Cancel);
        $('.btn-tbl-delete').each(function(){
            $(this).on("click", ConfirmDelete);
        });
        $('#gatepass_delete_confirm').on("click", Wfm.App.GatePass.Delete);

        $('.btn-primary').each(function () {
            $(this).on("click", OutIn);
        });
        $('.btn-danger').each(function () {
            $(this).on("click", OutIn);
        });
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var gatePassId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenGatePassId").val(gatePassId);        
    }

    function OutIn() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var gatePassId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenGatePassId").val(gatePassId);
        if ($(this).text() === "Out") {
            Wfm.App.GatePass.Out();
        }
        else if ($(this).text() === "In") {
            Wfm.App.GatePass.In();
        }
    }

    function SetWorkForceData(result) {
        if (result === null || result === undefined)
        {
            var errorMessage = "Emp Id " + $('#WORKFORCE_EMP_ID').val() + " not found.";
            $('#WORKFORCE_EMP_ID').next('span').text(errorMessage).show();
            $('#WORKFORCE_EMP_ID').next('span').addClass("field-validation-error");            
            return;
        }

        $('#WF_ID').val(result.WORKFORCE.WF_ID);
        $('#START_DATE').val($.MyAdmin.ToDate(result.START_DATE));
        $('#END_DATE').val($.MyAdmin.ToDate(result.END_DATE));
        $('#OUT_TIME').val("00:00:00");
        $('#IN_TIME').val("00:00:00");
        $('#ACTUAL_OUTTIME').val("00:00:00");
        $('#ACTUAL_INTIME').val("00:00:00");
        $('#WORKFORCE_EMAIL_ID').val(result.WORKFORCE.EMAIL_ID);
        $('#WORKFORCE_MOBILE_NO').val(result.WORKFORCE.MOBILE_NO);
        $('#WORKFORCE_EMP_NAME').val(result.WORKFORCE.EMP_NAME);
        $('#WORKFORCE_FATHER_NAME').val(result.WORKFORCE.FATHER_NAME);
        $('#PURPOSE').val(result.PURPOSE);
        $('#WORKFORCE_REMARK').val(result.REMARK);
    }        

    initialize();
})();