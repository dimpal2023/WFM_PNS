"use strict"
var Wfm = {};
Wfm.App = {};
Wfm.App.Report = {};

(function () {
    Wfm.App.Report = {
        GetReportData: function () {
            if (!Wfm.App.Report.Validate()) {
                event.preventDefault();
                return;
            }
            
            var reportType = $("#REPORT_TYPE").val().trim();
            var deptId = $('#DEPT_ID').val().trim();
            var subDeptId = $('#SUBDEPT_ID').val().trim();
            var empType = $('#WF_EMP_TYPE option:selected').text() !== '--Select--' ? $('#WF_EMP_TYPE').val().trim() : 3;
            var wfId = ($('#EMP_NAME').val() !== '' ? $('#WF_ID').val().trim() : null);
            var startDate = $('#START_DATE').val().trim();
            var endDate = $('#END_DATE').val().trim();
            var pieceWager = false;
            $('input:checkbox').each(function () {
                if (this.checked && this.id === "pieceWager") {                    
                    pieceWager = this.checked;
                }
            });
            
            var inputObj = {
                REPORT_TYPE: reportType,
                DEPT_ID: deptId,
                SUBDEPT_ID: subDeptId,
                EMP_TYPE: empType,
                WF_ID: wfId,
                START_DATE: startDate,
                END_DATE: endDate,
                ISPIECEWAGER: pieceWager
            };

            inputObj = JSON.stringify({ 'inputObj': inputObj });

            $.ajax({
                type: "POST",
                url: "/Report/GetReportData",
                data: inputObj,
                contentType: "application/json",
                dataType: "html",
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    $('#partialPlaceHolder').css("style", "overflow-x: scroll");
                    $('#partialPlaceHolder').html(response);
                    $('#partialPlaceHolder').fadeIn('fast');

                    $('#tableExport').DataTable({
                        dom: 'Bfrtip',
                        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'
                        ]
                    });
                }
            });
        },           
        Validate: function () {            
            var return_val = true;
            var startDate = $('#START_DATE').val().trim();
            var endDate = $('#END_DATE').val().trim();
            var empName = $('#EMP_NAME').val().trim();

            if ($('#REPORT_TYPE option:selected').text() == '--Select--') {
                var errorMessage = "Report type is required.";
                $('#REPORT_TYPE').parent().siblings('span').text(errorMessage).show();
                $('#REPORT_TYPE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#REPORT_TYPE').parent().siblings('span').text("").hide();
                $('#REPORT_TYPE').parent().siblings('span').addClass("field-validation-valid");
                return_val = true;
            }

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

            return return_val;
        }
    };              

    function initialize() {
        $('#submit').on("click", Wfm.App.Report.GetReportData);        
    }

    initialize();
})();