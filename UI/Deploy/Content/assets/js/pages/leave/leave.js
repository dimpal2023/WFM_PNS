(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};

    Wfm.App.WorkForce = {
        EmpSearch: function () {
            var validate = function () {

                var return_val = true;
                if ($("#EMP_ID").val() == '') {
                    var errorMessage = "Enter Employee ID.";
                    $('#EMP_ID').next('span').text(errorMessage).show();
                    $('#EMP_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMP_ID').next('span').text(errorMessage).hide();
                    $('#EMP_ID').next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var empId = $("#EMP_ID").val();
            var wfObj = { EMP_ID: empId };
            $.ajax({
                type: "POST",
                url: "/Leave/EmpSearch",
                data: JSON.stringify(wfObj),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    SetEmpData(result);
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        Create: function () {

            debugger;

            if (!Wfm.App.WorkForce.Validate()) {
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
            if (!Wfm.App.WorkForce.Validate()) {
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
        Validate: function () {

            var return_val = true;

            if ($('#EMP_ID').val().trim() == '') {
                $('#EMP_ID').next('span').text("Enter Employee Code.").show();
                $('#EMP_ID').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_ID').next('span').text("").hide();
                $('#EMP_ID').next('span').addClass("field-validation-valid");
            }

            var fdate = $('#FROM_DATE').val().trim();
            if (fdate == '') {
                $('#FROM_DATE').next('span').text("Please select From Date.").show();
                $('#FROM_DATE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#FROM_DATE').next('span').text("").hide();
                $('#FROM_DATE').next('span').addClass("field-validation-valid");
            }

            var tdate = $('#TO_DATE').val().trim();
            if (tdate == '') {
                $('#TO_DATE').next('span').text("Please select To Date.").show();
                $('#TO_DATE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#TO_DATE').next('span').text("").hide();
                $('#TO_DATE').next('span').addClass("field-validation-valid");
            }

            if (tdate != '' && fdate != '') {
                if (Date.parse(fdate) > Date.parse(tdate)) {
                    $('#FROM_DATE').next('span').text("From Date should be less than To Date.").show();
                    $('#FROM_DATE').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#FROM_DATE').next('span').text("").hide();
                    $('#FROM_DATE').next('span').addClass("field-validation-valid");
                }
            }



            if ($('#REMARKS').val().trim() == '') {
                $('#REMARKS').next('span').text("Please enter Remarks").show();
                $('#REMARKS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#REMARKS').next('span').text("").hide();
                $('#REMARKS').next('span').addClass("field-validation-valid");
            }
            return return_val;
        }
    };

    function initialize() {
        $('#emp-search').on("click", Wfm.App.WorkForce.EmpSearch);
        $('#submit').on("click", Wfm.App.WorkForce.Create);
        $('#edit').on("click", Wfm.App.WorkForce.Edit);
        $('#cancel').on("click", Wfm.App.WorkForce.Cancel);
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
        $('#WorkForce_delete_confirm').on("click", Wfm.App.WorkForce.Delete);

    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var WorkForceId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenWorkForceId").val(WorkForceId);
    }

    function SetEmpData(result) {

        debugger;

        if (result === null || result === undefined || result.ACTION == 'NO DATA') {
            var errorMessage = "EMP ID " + $('#EMP_ID').val() + " not found.";
            $('#EMP_ID').next('span').text(errorMessage).show();
            $('#EMP_ID').next('span').addClass("field-validation-error");
            return;
        }
        else {
            $('#WF_ID').val(result.WF_ID);
            $('#EMP_NAME').val(result.EMP_NAME);
            $('#REMARKS').val(result.REMARKS);
            $('#HIDDENEMP_ID').val(result.HIDDENEMP_ID);
            return;
        }
    }



    function SetWorkForceData(result) {

        if (result === null || result === undefined) {
            var errorMessage = "MRF Id " + $('#MRF_ID').val() + " not found.";
            $('#MRF_ID').next('span').text(errorMessage).show();
            $('#MRF_ID').next('span').addClass("field-validation-error");
            return;
        }

        $('#MRFINTERNALID').val(result.MRF_INTERNAL_ID);
        $('#MRF_INTERNAL_ID').val(result.MRF_INTERNAL_ID);

        $.each(result.WF_EMP_TYPEList, function (a) {
            $('#WF_EMP_TYPE_TEXT').val(result.WF_EMP_TYPEList[a].Text);
            $('#WF_EMP_TYPE').val(result.WF_EMP_TYPEList[a].Value);
        });

        $.each(result.EmpTypeList, function (j) {
            $('#EMP_TYPE').val(result.EmpTypeList[j].Text);
            $('#EMP_TYPE_ID').val(result.EmpTypeList[j].Value);
        });

        $.each(result.DesignationList, function (k) {
            $('#DESIGNATION').val(result.DesignationList[k].Text);
            $('#WF_DESIGNATION_ID').val(result.DesignationList[k].Value);
        });

        $.each(result.SkillList, function (l) {
            $('#SKILL').val(result.SkillList[l].Text);
            $('#SKILL_ID').val(result.SkillList[l].Value);
        });

    }

    function ToDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear();
    }

    initialize();

    var d = new Date();
    var currMonth = d.getMonth();
    var currYear = d.getFullYear();
    var startDate = new Date(currYear, currMonth, 1);
})();