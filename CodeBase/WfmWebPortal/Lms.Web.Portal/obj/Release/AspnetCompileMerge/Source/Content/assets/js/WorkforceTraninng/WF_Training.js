"use strict"
var Wfm = {};
Wfm.App = {};
Wfm.App.WorkforceTrainning = {};

(function () {
    alert('e');
    Wfm.App.WorkforceTrainning = {
        AddTrainningWorkforce: function () {
            var $form = $(this).parents('form');

            $('#submit').prop('disabled', true);

            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                data: $form.serialize(),
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    debugger
                    if (response.id == '1') {
                        alert("Submitted Successfully.");
                        window.location.href = response.Url;
                    }

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
            var mobileNo = $('#MOBILE_NO').val().trim();
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
                    if (response.id == '1') {
                        alert("Gate Pass updated Successfully.");
                        window.location.href = response.Url;
                    }
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
    };
})