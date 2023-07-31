(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};

    Wfm.App.WorkForce = {
        LoadAttendance: function () {
            var validate = function () {
                var return_val = true;
                if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                    var errorMessage = "Please select department.";
                    $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#MONTH_ID").val() === '' || $("#MONTH_ID").val() === null) {
                    var errorMessage = "Please select month.";
                    $('#MONTH_ID').parent().next('span').text(errorMessage).show();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#MONTH_ID').parent().next('span').text(errorMessage).hide();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#YEAR_ID").val() === '' || $("#YEAR_ID").val() === null) {
                    var errorMessage = "Please select year.";
                    $('#YEAR_ID').parent().next('span').text(errorMessage).show();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#YEAR_ID').parent().next('span').text(errorMessage).hide();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#EMP_ID").val() === '' || $("#EMP_ID").val() === null) {
                    var errorMessage = "Please select Employee.";
                    $('#EMP_ID').parent().next('span').text(errorMessage).show();
                    $('#EMP_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMP_ID').parent().next('span').text(errorMessage).hide();
                    $('#EMP_ID').parent().next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }

            var wfid = $("#EMP_ID").val();
            var deptId = $("#DEPT_ID").val();
            var monthId = $("#MONTH_ID").val();
            var yearId = $("#YEAR_ID").val();

            var workforceatt = { WF_ID: wfid, DEPT_ID: deptId, MONTH_ID: monthId, YEAR_ID: yearId };
            $.ajax({
                type: "POST",
                url: "/Attendance/AttendanceList",
                data: JSON.stringify(workforceatt),
                contentType: "application/json",
                dataType: "html",
                success: function (data) {
                    if (data != null) {
                        $('#partialPlaceHolder').css("style", "overflow: scroll; display: block");
                        $('#partialPlaceHolder').html(data);
                        $('#partialPlaceHolder').fadeIn('fast');

                        $('#tableExport').DataTable({
                                dom : 'Bfrtip',
                                    buttons: [
                            'copy', 'csv', 'excel', 'pdf', 'print'
                            ]
                        });
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        LoadFaultyAttendance: function () {
            var validate = function () {
                var return_val = true;
                if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                    var errorMessage = "Please select department.";
                    $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
                }               

                if ($("#MONTH_ID").val() === '' || $("#MONTH_ID").val() === null) {
                    var errorMessage = "Please select month.";
                    $('#MONTH_ID').parent().next('span').text(errorMessage).show();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#MONTH_ID').parent().next('span').text(errorMessage).hide();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#YEAR_ID").val() === '' || $("#YEAR_ID").val() === null) {
                    var errorMessage = "Please select year.";
                    $('#YEAR_ID').parent().next('span').text(errorMessage).show();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#YEAR_ID').parent().next('span').text(errorMessage).hide();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var deptId = $("#DEPT_ID").val();
            var monthId = $("#MONTH_ID").val();
            var yearId = $("#YEAR_ID").val();

            var workforceSalary = { DEPT_ID: deptId, MONTH_ID: monthId, YEAR_ID: yearId };
            $.ajax({
                type: "POST",
                url: "/Attendance/FaultyAttendanceList",
                data: JSON.stringify(workforceSalary),
                contentType: "application/json",
                dataType: "html",
                success: function (data) {
                    if (data != null) {
                        $('#partialPlaceHolder').css("style", "overflow: scroll; display: block");
                        $('#partialPlaceHolder').html(data);
                        $('#partialPlaceHolder').fadeIn('fast');

                        $('#tableExport').DataTable({
                                dom: 'Bfrtip',
                                buttons: [
                            'copy', 'csv', 'excel', 'pdf', 'print'
                            ]
                        });
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        LoadSalary: function () {
            var validate = function () {
                var return_val = true;

                if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                    var errorMessage = "Please select department.";
                    $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#MONTH_ID").val() === '' || $("#MONTH_ID").val() === null) {
                    var errorMessage = "Please select month.";
                    $('#MONTH_ID').parent().next('span').text(errorMessage).show();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#MONTH_ID').parent().next('span').text(errorMessage).hide();
                    $('#MONTH_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#WF_EMP_TYPE").val() === '' || $("#WF_EMP_TYPE").val() === null) {
                    var errorMessage = "Please select employee type.";
                    $('#WF_EMP_TYPE').parent().next('span').text(errorMessage).show();
                    $('#WF_EMP_TYPE').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#WF_EMP_TYPE').parent().next('span').text(errorMessage).hide();
                    $('#WF_EMP_TYPE').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#YEAR_ID").val() === '' || $("#YEAR_ID").val() === null) {
                    var errorMessage = "Please select year.";
                    $('#YEAR_ID').parent().next('span').text(errorMessage).show();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#YEAR_ID').parent().next('span').text(errorMessage).hide();
                    $('#YEAR_ID').parent().next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var deptId = $("#DEPT_ID").val();
            var monthId = $("#MONTH_ID").val();
            var yearId = $("#YEAR_ID").val();
            var wfEmpType = $("#WF_EMP_TYPE").val();
            var sub_dept_id = $("#SUBDEPT_ID").val();

            var workforceSalary = { DEPT_ID: deptId, SUBDEPT_ID: sub_dept_id, MONTH_ID: monthId, YEAR_ID: yearId, WF_EMP_TYPE: wfEmpType };
            $.ajax({
                type: "POST",
                url: "/Workforce/SalaryList",
                data: JSON.stringify(workforceSalary),
                contentType: "application/json",
                dataType: "html",
                beforeSend: function () {
                    $('.page-loader-wrapper').show();
                },
                complete: function () {
                    $('.page-loader-wrapper').hide();
                },
                success: function (data) {
                    if (data != null) {
                        $('#partialPlaceHolder').css("style", "overflow: scroll; display: block");
                        $('#partialPlaceHolder').html(data);
                        $('#partialPlaceHolder').fadeIn('fast');

                        $('#tableExport').DataTable({
                            dom: 'Bfrtip',
                            buttons: [
                                'copy', 'csv', 'excel', 'pdf', 'print'
                            ]
                        });
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        DailyWorkItemSave: function () {
            var validate = function () {
                var return_val = true;
                var workDate;
                if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                    var errorMessage = "Please select department.";
                    $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#EMP_NAME").val() === '' || $("#EMP_NAME").val() === null) {
                    var errorMessage = "Please select workforce.";
                    $('#EMP_NAME').next('span').text(errorMessage).show();
                    $('#EMP_NAME').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMP_NAME').next('span').text(errorMessage).hide();
                    $('#EMP_NAME').next('span').addClass("field-validation-valid");
                }

                if ($('#WORK_DATE').val().trim() === '') {
                    var errorMessage = "Please select work date.";
                    $('#WORK_DATE').next('span').text(errorMessage).show();
                    $('#WORK_DATE').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#WORK_DATE').next('span').text(errorMessage).hide();
                    $('#WORK_DATE').next('span').addClass("field-validation-valid");

                    var workDate = $('#WORK_DATE').val().trim();
                    workDate = new Date(workDate);
                    if (workDate > GetTodayDate()) {
                        var errorMessage = "Work date can not be greater than today's date";
                        $('#WORK_DATE').next('span').text(errorMessage).show();
                        $('#WORK_DATE').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                }
                
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }

            if ($('#tblDailyWorkItems tr').length <= 1) {
                alert("Please add daily work item");
                return;
            }

            var dailywork = new Array();            
            var workDate = new Date($("#WORK_DATE").val());
            var workDateString = workDate.getFullYear() + '-' + workDate.getMonth() + '-' + workDate.getDay()

            //collect data from daily work items
            $("[id*=tblDailyWorkItems] > tbody tr").each(function () {
                var itemID = $(".ItemId", $(this)).find("label").text();
                var uniQueOperationId = $(".UniqueOperationId", $(this)).find("label").text();
                var qty = $(".Qty", $(this)).find("span").text();

                var workItem = {
                    DEPT_ID: $("#DEPT_ID").val(),
                    EMP_ID: $("#EMP_ID").val(),
                    WORK_DATE: workDateString,
                    ITEM_ID: itemID,
                    UNIQUE_OPERATION_ID: uniQueOperationId,
                    QTY: qty
                };

                if (itemID !== '' || uniQueOperationId !== '' || qty !== '') {
                    dailywork.push(workItem);
                }
            });

            dailywork = JSON.stringify(dailywork);
           
            $.ajax({
                type: "POST",
                url: "/Workforce/DailyWork",
                data: dailywork,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    //SetDWEmpData(result);
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        DailyWorkItemAdd: function () {
            var validate = function () {
                var return_val = true;
                if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                    var errorMessage = "Please select department.";
                    $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                    $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
                }

                if ($("#EMP_NAME").val() === '' || $("#EMP_NAME").val() === null) {
                    var errorMessage = "Please select workforce.";
                    $('#EMP_NAME').next('span').text(errorMessage).show();
                    $('#EMP_NAME').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMP_NAME').next('span').text(errorMessage).hide();
                    $('#EMP_NAME').next('span').addClass("field-validation-valid");
                }

                if ($('#WORK_DATE').val().trim() === '') {
                    var errorMessage = "Please select work date.";
                    $('#WORK_DATE').next('span').text(errorMessage).show();
                    $('#WORK_DATE').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#WORK_DATE').next('span').text(errorMessage).hide();
                    $('#WORK_DATE').next('span').addClass("field-validation-valid");
                }

                var workDate = Date.parse($('#WORK_DATE').val().trim());
                if (workDate > startDate) {
                    var errorMessage = "Work date can not be greater than today's date";
                    $('#WORK_DATE').next('span').text(errorMessage).show();
                    $('#WORK_DATE').next('span').addClass("field-validation-error");
                    return_val = false;
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
                url: "/Workforce/DWEmpSearch",
                data: JSON.stringify(wfObj),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    SetDWEmpData(result);
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        EmpSearch: function () {
            $('#editsubmit').click();
            //var validate = function () {
            //    var return_val = true;
            //    if ($("#WF_ID").val() == '') {
            //        var errorMessage = "Select Employee.";
            //        $('#WF_ID').next('span').text(errorMessage).show();
            //        $('#WF_ID').next('span').addClass("field-validation-error");
            //        return_val = false;
            //    }
            //    else {
            //        $('#WF_ID').next('span').text(errorMessage).hide();
            //        $('#WF_ID').next('span').addClass("field-validation-valid");
            //    }
            //    return return_val;
            //}

            //if (!validate()) {
            //    event.preventDefault();
            //    return;
            //}
            //var empId = $("#WF_ID").val();
            //var wfObj = { WF_ID: empId };
            //$.ajax({
            //    type: "POST",
            //    url: "/Workforce/GetEmployeeProfile",
            //    data: JSON.stringify(wfObj),
            //    contentType: "application/json",
            //    dataType: "json",
            //    success: function (result) {
            //        SetEmpProfileData(result);
            //    },
            //    error: function (responseText) {
            //        alert(responseText);
            //    }
            //});
        },
        EmpSalaryDetailSearch: function () {
            EmpSearchClick();
        },
        CreateSalDetail: function () {
            if (!Wfm.App.WorkForce.ValidateSalDetail()) {
                event.preventDefault();
                return;
            }

            var $form = $(this).parents('form');

            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                data: $form.serialize(),
                beforeSend: function () {
                    $('.page-loader-wrapper').show();
                },
                complete: function () {
                    $('.page-loader-wrapper').hide();
                },
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    if (response.ID == '1') {
                        alert("Salary Details Added Successfully.");
                        window.location.href = response.Url;
                    } else {
                        alert(response.ID);
                    }
                   
                }
            });
        },
        Create: function () {
            debugger;
            if ($("#EmployeeCreate").valid()) { //check if form is valid using model annotation
            }
            else {
                return false;
            }
            if (!Wfm.App.WorkForce.Validate()) {
                event.preventDefault();
                return;
            }

            var form = $("#EmployeeCreate");
            var isvalid = form.valid();
           
            var formData = new FormData(form[0]);
            $.ajax({
                type: "POST",
                url: '/Workforce/Create',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    $('.page-loader-wrapper').show();
                    $("#submit").prop('disabled', true);
                },
                complete: function () {
                    $('.page-loader-wrapper').hide();
                },
                error: function (xhr, status, error) {
                    $("#submit").prop('disabled', false);
                },
                success: function (response) {
                    debugger;
                    $("#AADHAR_NO").val('');
                    $("#BIOMETRIC_CODE").val('');
                    if (response.id == '1') {
                        alert("Employee details submitted Successfully.");
                        window.location.href = response.Url;
                    }
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: $form.attr('action'),
            //    data: $form.serialize(),
            //    error: function (xhr, status, error) {
            //    },
            //    success: function (response) {
            //        window.location.href = response.Url;
            //    }
            //});
        },
        Edit: function () {
            if (!Wfm.App.WorkForce.ValidateEdit()) {
                event.preventDefault();
                return;
            }

            var form = $("#EmployeeEdit");
            var formData = new FormData(form[0]);
            $.ajax({
                type: "POST",
                url: '/Workforce/Edit',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    $('.page-loader-wrapper').show();
                },
                complete: function () {
                    $('.page-loader-wrapper').hide();
                },
                error: function (xhr, status, error) {
                },
                success: function (response) {
                    if (response.id == '1') {
                        alert("Employee details updated Successfully.");
                        window.location.href = response.Url;
                    } else if (response.id == '3') {
                        alert("Employee ID Already Available.");
                    }else if (response.id == '4') {
                        alert("Biometric Already Available.");
                    }
                }
            });
            
        },
        Search: function () {
            var validate = function () {

                var return_val = true;
                if ($("#MRF_ID option:selected").val() == '') {
                    var errorMessage = "The MRF Id field is required.";
                    $('#MRF_ID').next('span').text(errorMessage).show();
                    $('#MRF_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#MRF_ID').next('span').text(errorMessage).hide();
                    $('#MRF_ID').next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var mrfId = $("#MRF_ID option:selected").val();
            var wfObj = { MRF_ID: mrfId };
            $.ajax({
                type: "POST",
                url: "/Workforce/Search",
                data: JSON.stringify(wfObj),
                contentType: "application/json",
                dataType: "json",
                beforeSend: function () {
                    $('.page-loader-wrapper').show();
                },
                complete: function () {
                    $('.page-loader-wrapper').hide();
                },
                success: function (result) {
                    SetWorkForceData(result);
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });
        },
        Cancel: function () {
            window.location.href = "/WorkForce/AllItems";
        },
        ValidateEdit: function () {

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

            if ($('#BIOMETRIC_CODE').val().trim() == '') {
                $('#BIOMETRIC_CODE').next('span').text("Enter Bio Metric Code.").show();
                $('#BIOMETRIC_CODE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BIOMETRIC_CODE').next('span').text("").hide();
                $('#BIOMETRIC_CODE').next('span').addClass("field-validation-valid");
            }

            if ($('#EMP_NAME').val().trim() == '') {
                $('#EMP_NAME').next('span').text("Enter Employee Name.").show();
                $('#EMP_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_NAME').next('span').text("").hide();
                $('#EMP_NAME').next('span').addClass("field-validation-valid");
            }

            if ($('#FATHER_NAME').val().trim() == '') {
                $('#FATHER_NAME').next('span').text("Enter Father Name.").show();
                $('#FATHER_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#FATHER_NAME').next('span').text("Enter Father Name.").hide();
                $('#FATHER_NAME').next('span').addClass("field-validation-valid");
            }

            if ($('#GENDER option:selected').text() == 'Gender') {
                $('#GENDER').parent().siblings('span').text("Select Gender.").show();
                $('#GENDER').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#GENDER').parent().siblings('span').text("Select Gender.").hide();
                $('#GENDER').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#DEPT_ID option:selected').text() == 'Department') {
                $('#DEPT_ID').parent().siblings('span').text("Select Department.").show();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().siblings('span').text("Select Department.").hide();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            var dob = $('#DATE_OF_BIRTH').val().trim();
            if (dob == '') {
                $('#DATE_OF_BIRTH').next('span').text("Please select Date of Birth.").show();
                $('#DATE_OF_BIRTH').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var d = new Date();
                var curr_day = d.getDate();
                var curr_month = d.getMonth();
                var curr_year = d.getFullYear();
                curr_month++; // In js, first month is 0, not 1
                var shortdate = curr_month + "-" + curr_day + "-" + curr_year


                if (Date.parse(dob) >= Date.parse(shortdate)) {
                    $('#DATE_OF_BIRTH').next('span').text("Date of Birth should be less than current date.").show();
                    $('#DATE_OF_BIRTH').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DATE_OF_BIRTH').next('span').text("Please select Date of Birth.").hide();
                    $('#DATE_OF_BIRTH').next('span').addClass("field-validation-valid");
                }
            }

            if ($('#WF_EMP_TYPE_TEXT').val() == "Agency Role" && $('#AGENCY_ID option:selected').text() == 'Agency') {
                $('#AGENCY_ID').next('span').text("Please select Agency.").show();
                $('#AGENCY_ID').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#AGENCY_ID').next('span').text("").hide();
                $('#AGENCY_ID').next('span').addClass("field-validation-valid");
            }


            if ($('#EMP_STATUS_ID option:selected').text() == 'Employee Status') {
                $('#EMP_STATUS_ID').parent().siblings('span').text("Please select Employee Status.").show();
                $('#EMP_STATUS_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_STATUS_ID').parent().siblings('span').text("Please select Employee Status.").hide();
                $('#EMP_STATUS_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#NATIONALITY option:selected').text() == 'Nationality') {
                var errorMessage = "Please select Nationality.";
                $('#NATIONALITY').parent().siblings('span').text(errorMessage).show();
                $('#NATIONALITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#NATIONALITY').parent().siblings('span').text("Please select Nationality.").hide();
                $('#NATIONALITY').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#WF_EDUCATION_ID option:selected').text() == 'Education') {
                var errorMessage = "Please select Education.";
                $('#WF_EDUCATION_ID').parent().siblings('span').text(errorMessage).show();
                $('#WF_EDUCATION_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#WF_EDUCATION_ID').parent().siblings('span').text("Please select Education.").hide();
                $('#WF_EDUCATION_ID').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#MARITAL_ID option:selected').text() == 'Marital Status') {
                var errorMessage = "Please select Marital Status.";
                $('#MARITAL_ID').parent().siblings('span').text(errorMessage).show();
                $('#MARITAL_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#MARITAL_ID').parent().siblings('span').text("Please select Marital Status.").hide();
                $('#MARITAL_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#DOJ').val().trim() == '') {
                $('#DOJ').next('span').text("Please select Date of Joining.").show();
                $('#DOJ').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DOJ').next('span').text("Please select Date of Joining.").hide();
                $('#DOJ').next('span').addClass("field-validation-valid");
            }

            if ($('#DOJ_AS_PER_EPF').val().trim() == '') {
                $('#DOJ_AS_PER_EPF').next('span').text("Please select EPF Date of Joining.").show();
                $('#DOJ_AS_PER_EPF').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DOJ_AS_PER_EPF').next('span').text("Please select EPF Date of Joining.").hide();
                $('#DOJ_AS_PER_EPF').next('span').addClass("field-validation-valid");
            }

            if ($('#PRESENT_ADDRESS').val().trim() == '') {
                $('#PRESENT_ADDRESS').next('span').text("Enter Present Address.").show();
                $('#PRESENT_ADDRESS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS').next('span').text("Enter Present Address.").hide();
                $('#PRESENT_ADDRESS').next('span').addClass("field-validation-valid");
            }

            if ($('#PRESENT_ADDRESS_STATE option:selected').text() == 'Present Address State') {
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').text("Please select Present Address State.").show();
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').text("Please select Present Address State.").hide();
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PRESENT_ADDRESS_CITY option:selected').text() == 'Present Address City') {
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').text("Please select Present Address City.").show();
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').text("Please select Present Address City.").hide();
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PRESENT_ADDRESS_PIN').val().trim() == '') {

                $('#PRESENT_ADDRESS_PIN').next('span').text("").hide();
                $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#PRESENT_ADDRESS_PIN').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#PRESENT_ADDRESS_PIN').next('span').text("Enter only numbers in Present Address Pincode.").show();
                    $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 6) {
                        $('#PRESENT_ADDRESS_PIN').next('span').text("Enter 6 digit Present Address Pincode.").show();
                        $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#PRESENT_ADDRESS_PIN').next('span').text("Enter only numbers in Present Address Pincode.").hide();
                        $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#PERMANENT_ADDRESS').val().trim() == '') {
                $('#PERMANENT_ADDRESS').next('span').text("Enter Permanent Address.").show();
                $('#PERMANENT_ADDRESS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS').next('span').text("Enter Permanent Address.").hide();
                $('#PERMANENT_ADDRESS').next('span').addClass("field-validation-valid");
            }

            if ($("#PERMANENT_ADDRESS_STATE option:selected").val().trim() == 'Permanent Address State') {
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').text("Please select Permanent Address State.").show();
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').text("Please select Permanent Address State.").hide();
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($("#PERMANENT_ADDRESS_CITY option:selected").val().trim() == 'Permanent Address City') {
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').text("Please select Permanent Address City.").show();
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').text("Please select Permanent Address City.").hide();
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PERMANENT_ADDRESS_PIN').val().trim() == '') {

                $('#PERMANENT_ADDRESS_PIN').next('span').text("").hide();
                $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#PERMANENT_ADDRESS_PIN').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter only numbers in Permanent Address Pincode.").show();
                    $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 6) {
                        $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter 6 digit Permanent Address Pincode.").show();
                        $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter only numbers in Permanent Address Pincode.").hide();
                        $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
                    }
                }
            }

            if ($('#MOBILE_NO').val().trim() == '') {
                $('#MOBILE_NO').next('span').text("Enter Mobile No.").show();
                $('#MOBILE_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#MOBILE_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#MOBILE_NO').next('span').text("Enter only numbers in Mobile No.").show();
                    $('#MOBILE_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length < 10) {
                        $('#MOBILE_NO').next('span').text("Enter 10 digit Mobile No.").show();
                        $('#MOBILE_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#MOBILE_NO').next('span').text("Enter only numbers in Mobile No.").hide();
                        $('#MOBILE_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#ALTERNATE_NO').val().trim() == '') {

                $('#ALTERNATE_NO').next('span').text("").hide();
                $('#ALTERNATE_NO').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#ALTERNATE_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#ALTERNATE_NO').next('span').text("Enter only numbers in Alternate No.").show();
                    $('#ALTERNATE_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 10) {
                        $('#ALTERNATE_NO').next('span').text("Enter 10 digit Alternate Mobile No.").show();
                        $('#ALTERNATE_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#ALTERNATE_NO').next('span').text("Enter only numbers in Alternate No.").hide();
                        $('#ALTERNATE_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#EMAIL_ID').val().trim() == '') {

                $('#EMAIL_ID').next('span').text("").hide();
                $('#EMAIL_ID').next('span').addClass("field-validation-valid");
            }
            else {
                var inputvalues = $('#EMAIL_ID').val();
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var valid = regex.test(inputvalues);

                if (!valid) {
                    $('#EMAIL_ID').next('span').text("Enter Valid Email address.").show();
                    $('#EMAIL_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMAIL_ID').next('span').text("").hide();
                    $('#EMAIL_ID').next('span').addClass("field-validation-valid");
                }
            }


            if ($('#AADHAR_NO').val().trim() == '') {
                $('#AADHAR_NO').next('span').text("Enter Aadhar No.").show();
                $('#AADHAR_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#AADHAR_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#AADHAR_NO').next('span').text("Enter only numbers in Aadhar No.").show();
                    $('#AADHAR_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 12) {
                        $('#AADHAR_NO').next('span').text("Enter 12 digit Aadhar No.").show();
                        $('#AADHAR_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#AADHAR_NO').next('span').text("").hide();
                        $('#AADHAR_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }

            return return_val;
        },
        Validate: function () {
            var return_val = true;

            if ($('#MRF_ID option:selected').text() == 'MRF ID') {
                $('#MRF_ID').parent().siblings('span').text("Select and Search MRF ID.").show();
                $('#MRF_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#MRF_ID').parent().siblings('span').text("Select Gender.").hide();
                $('#MRF_ID').parent().siblings('span').addClass("field-validation-valid");
            }
            if ($('#BIOMETRIC_CODE').val().trim() === '') {
                $('#BIOMETRIC_CODE').next('span').text("Enter Bio Metric Code.").show();
                $('#BIOMETRIC_CODE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BIOMETRIC_CODE').next('span').text("").hide();
                $('#BIOMETRIC_CODE').next('span').addClass("field-validation-valid");
            }

            if ($('#EMP_NAME').val().trim() == '') {
                $('#EMP_NAME').next('span').text("Enter Employee Name.").show();
                $('#EMP_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_NAME').next('span').text("").hide();
                $('#EMP_NAME').next('span').addClass("field-validation-valid");
            }

            if ($('#FATHER_NAME').val().trim() == '') {
                $('#FATHER_NAME').next('span').text("Enter Father Name.").show();
                $('#FATHER_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#FATHER_NAME').next('span').text("Enter Father Name.").hide();
                $('#FATHER_NAME').next('span').addClass("field-validation-valid");
            }

            if ($('#GENDER option:selected').text() == 'Gender') {
                $('#GENDER').parent().siblings('span').text("Select Gender.").show();
                $('#GENDER').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#GENDER').parent().siblings('span').text("Select Gender.").hide();
                $('#GENDER').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#DEPT_ID option:selected').text() == 'Department') {
                $('#DEPT_ID').parent().siblings('span').text("Select Department.").show();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().siblings('span').text("Select Department.").hide();
                $('#DEPT_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            var dob = $('#DATE_OF_BIRTH').val().trim();
            if (dob == '') {
                $('#DATE_OF_BIRTH').next('span').text("Please select Date of Birth.").show();
                $('#DATE_OF_BIRTH').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var d = new Date();
                var curr_day = d.getDate();
                var curr_month = d.getMonth();
                var curr_year = d.getFullYear();
                curr_month++; // In js, first month is 0, not 1
                var shortdate = curr_month + "-" + curr_day + "-" + curr_year


                if (Date.parse(dob) >= Date.parse(shortdate)) {
                    $('#DATE_OF_BIRTH').next('span').text("Date of Birth should be less than current date.").show();
                    $('#DATE_OF_BIRTH').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DATE_OF_BIRTH').next('span').text("Please select Date of Birth.").hide();
                    $('#DATE_OF_BIRTH').next('span').addClass("field-validation-valid");
                }
            }

            if ($('#WF_EMP_TYPE_TEXT').val() == "Agency Role" && $('#AGENCY_ID option:selected').text() == 'Agency') {
                $('#AGENCY_ID').next('span').text("Please select Agency.").show();
                $('#AGENCY_ID').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#AGENCY_ID').next('span').text("").hide();
                $('#AGENCY_ID').next('span').addClass("field-validation-valid");
            }


            if ($('#EMP_STATUS_ID option:selected').text() == 'Employee Status') {
                $('#EMP_STATUS_ID').parent().siblings('span').text("Please select Employee Status.").show();
                $('#EMP_STATUS_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_STATUS_ID').parent().siblings('span').text("Please select Employee Status.").hide();
                $('#EMP_STATUS_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#NATIONALITY option:selected').text() == 'Nationality') {
                var errorMessage = "Please select Nationality.";
                $('#NATIONALITY').parent().siblings('span').text(errorMessage).show();
                $('#NATIONALITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#NATIONALITY').parent().siblings('span').text("Please select Nationality.").hide();
                $('#NATIONALITY').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#WF_EDUCATION_ID option:selected').text() == 'Education') {
                var errorMessage = "Please select Education.";
                $('#WF_EDUCATION_ID').parent().siblings('span').text(errorMessage).show();
                $('#WF_EDUCATION_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#WF_EDUCATION_ID').parent().siblings('span').text("Please select Education.").hide();
                $('#WF_EDUCATION_ID').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#MARITAL_ID option:selected').text() == 'Marital Status') {
                var errorMessage = "Please select Marital Status.";
                $('#MARITAL_ID').parent().siblings('span').text(errorMessage).show();
                $('#MARITAL_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#MARITAL_ID').parent().siblings('span').text("Please select Marital Status.").hide();
                $('#MARITAL_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#DOJ').val().trim() == '') {
                $('#DOJ').next('span').text("Please select Date of Joining.").show();
                $('#DOJ').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DOJ').next('span').text("Please select Date of Joining.").hide();
                $('#DOJ').next('span').addClass("field-validation-valid");
            }

            if ($('#DOJ_AS_PER_EPF').val().trim() == '') {
                $('#DOJ_AS_PER_EPF').next('span').text("Please select EPF Date of Joining.").show();
                $('#DOJ_AS_PER_EPF').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DOJ_AS_PER_EPF').next('span').text("Please select EPF Date of Joining.").hide();
                $('#DOJ_AS_PER_EPF').next('span').addClass("field-validation-valid");
            }

            if ($('#PRESENT_ADDRESS').val().trim() == '') {
                $('#PRESENT_ADDRESS').next('span').text("Enter Present Address.").show();
                $('#PRESENT_ADDRESS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS').next('span').text("Enter Present Address.").hide();
                $('#PRESENT_ADDRESS').next('span').addClass("field-validation-valid");
            }

            if ($('#PRESENT_ADDRESS_STATE option:selected').text() == 'Present Address State') {
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').text("Please select Present Address State.").show();
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').text("Please select Present Address State.").hide();
                $('#PRESENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PRESENT_ADDRESS_CITY option:selected').text() == 'Present Address City') {
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').text("Please select Present Address City.").show();
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').text("Please select Present Address City.").hide();
                $('#PRESENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PRESENT_ADDRESS_PIN').val().trim() == '') {

                $('#PRESENT_ADDRESS_PIN').next('span').text("").hide();
                $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#PRESENT_ADDRESS_PIN').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#PRESENT_ADDRESS_PIN').next('span').text("Enter only numbers in Present Address Pincode.").show();
                    $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 6) {
                        $('#PRESENT_ADDRESS_PIN').next('span').text("Enter 6 digit Present Address Pincode.").show();
                        $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#PRESENT_ADDRESS_PIN').next('span').text("Enter only numbers in Present Address Pincode.").hide();
                        $('#PRESENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#PERMANENT_ADDRESS').val().trim() == '') {
                $('#PERMANENT_ADDRESS').next('span').text("Enter Permanent Address.").show();
                $('#PERMANENT_ADDRESS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS').next('span').text("Enter Permanent Address.").hide();
                $('#PERMANENT_ADDRESS').next('span').addClass("field-validation-valid");
            }

            if ($("#PERMANENT_ADDRESS_STATE option:selected").val().trim() == 'Permanent Address State') {
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').text("Please select Permanent Address State.").show();
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').text("Please select Permanent Address State.").hide();
                $('#PERMANENT_ADDRESS_STATE').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($("#PERMANENT_ADDRESS_CITY option:selected").val().trim() == 'Permanent Address City') {
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').text("Please select Permanent Address City.").show();
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').text("Please select Permanent Address City.").hide();
                $('#PERMANENT_ADDRESS_CITY').parent().siblings('span').addClass("field-validation-valid");
            }


            if ($('#PERMANENT_ADDRESS_PIN').val().trim() == '') {

                $('#PERMANENT_ADDRESS_PIN').next('span').text("").hide();
                $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#PERMANENT_ADDRESS_PIN').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter only numbers in Permanent Address Pincode.").show();
                    $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 6) {
                        $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter 6 digit Permanent Address Pincode.").show();
                        $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#PERMANENT_ADDRESS_PIN').next('span').text("Enter only numbers in Permanent Address Pincode.").hide();
                        $('#PERMANENT_ADDRESS_PIN').next('span').addClass("field-validation-valid");
                    }
                }
            }

            if ($('#MOBILE_NO').val().trim() == '') {
                $('#MOBILE_NO').next('span').text("Enter Mobile No.").show();
                $('#MOBILE_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#MOBILE_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#MOBILE_NO').next('span').text("Enter only numbers in Mobile No.").show();
                    $('#MOBILE_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length < 10) {
                        $('#MOBILE_NO').next('span').text("Enter 10 digit Mobile No.").show();
                        $('#MOBILE_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#MOBILE_NO').next('span').text("Enter only numbers in Mobile No.").hide();
                        $('#MOBILE_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#ALTERNATE_NO').val().trim() == '') {

                $('#ALTERNATE_NO').next('span').text("").hide();
                $('#ALTERNATE_NO').next('span').addClass("field-validation-valid");
            }
            else {
                var inputVal = $('#ALTERNATE_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#ALTERNATE_NO').next('span').text("Enter only numbers in Alternate No.").show();
                    $('#ALTERNATE_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 10) {
                        $('#ALTERNATE_NO').next('span').text("Enter 10 digit Alternate Mobile No.").show();
                        $('#ALTERNATE_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#ALTERNATE_NO').next('span').text("Enter only numbers in Alternate No.").hide();
                        $('#ALTERNATE_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }


            if ($('#EMAIL_ID').val().trim() == '') {

                $('#EMAIL_ID').next('span').text("").hide();
                $('#EMAIL_ID').next('span').addClass("field-validation-valid");
            }
            else {
                var inputvalues = $('#EMAIL_ID').val();
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var valid = regex.test(inputvalues);

                if (!valid) {
                    $('#EMAIL_ID').next('span').text("Enter Valid Email address.").show();
                    $('#EMAIL_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#EMAIL_ID').next('span').text("").hide();
                    $('#EMAIL_ID').next('span').addClass("field-validation-valid");
                }
            }


            if ($('#AADHAR_NO').val().trim() == '') {
                $('#AADHAR_NO').next('span').text("Enter Aadhar No.").show();
                $('#AADHAR_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#AADHAR_NO').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#AADHAR_NO').next('span').text("Enter only numbers in Aadhar No.").show();
                    $('#AADHAR_NO').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    if (inputVal.length !== 12) {
                        $('#AADHAR_NO').next('span').text("Enter 12 digit Aadhar No.").show();
                        $('#AADHAR_NO').next('span').addClass("field-validation-error");
                        return_val = false;
                    }
                    else {
                        $('#AADHAR_NO').next('span').text("").hide();
                        $('#AADHAR_NO').next('span').addClass("field-validation-valid");
                    }
                }
            }

            return return_val;
        },
        ValidateSalDetail: function () {

            var return_val = true;

            if ($('#EMP_ID').val() == 'EMP ID') {
                $('#EMP_ID').next('span').text("Select and Search EMP ID.").show();
                $('#EMP_ID').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EMP_ID').next('span').text("").hide();
                $('#EMP_ID').next('span').addClass("field-validation-valid");
            }

            if ($('#UAN_NO').val().trim() == '') {
                $('#UAN_NO').next('span').text("Enter UAN No.").show();
                $('#UAN_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#UAN_NO').next('span').text("").hide();
                $('#UAN_NO').next('span').addClass("field-validation-valid");
            }

            if ($('#PAN_CARD').val().trim() == '') {
                $('#PAN_CARD').next('span').text("Enter PAN CARD No.").show();
                $('#PAN_CARD').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#PAN_CARD').next('span').text("").hide();
                $('#PAN_CARD').next('span').addClass("field-validation-valid");
            }

            if ($('#EPF_NO').val().trim() == '') {
                $('#EPF_NO').next('span').text("Enter EPF NO.").show();
                $('#EPF_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#EPF_NO').next('span').text("").hide();
                $('#EPF_NO').next('span').addClass("field-validation-valid");
            }

            if ($('#ESIC_NO').val().trim() == '') {
                $('#ESIC_NO').next('span').text("Enter ESIC NO.").show();
                $('#ESIC_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#ESIC_NO').next('span').text("").hide();
                $('#ESIC_NO').next('span').addClass("field-validation-valid");
            }
            debugger
            if ($('#BANK_ID option:selected').val() == '') {
                $('#BANK_ID').parent().siblings('span').text("Select Bank.").show();
                $('#BANK_ID').parent().siblings('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BANK_ID').parent().siblings('span').text("Select Gender.").hide();
                $('#BANK_ID').parent().siblings('span').addClass("field-validation-valid");
            }

            if ($('#BANK_IFSC').val().trim() == '') {
                $('#BANK_IFSC').next('span').text("Enter BANK IFSC code.").show();
                $('#BANK_IFSC').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BANK_IFSC').next('span').text("").hide();
                $('#BANK_IFSC').next('span').addClass("field-validation-valid");
            }

            if ($('#BANK_BRANCH').val().trim() == '') {
                $('#BANK_BRANCH').next('span').text("Enter Bank Branch.").show();
                $('#BANK_BRANCH').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BANK_BRANCH').next('span').text("").hide();
                $('#BANK_BRANCH').next('span').addClass("field-validation-valid");
            }

            if ($('#BANK_ACCOUNT_NO').val().trim() == '') {
                $('#BANK_ACCOUNT_NO').next('span').text("Enter Bank Branch.").show();
                $('#BANK_ACCOUNT_NO').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BANK_ACCOUNT_NO').next('span').text("").hide();
                $('#BANK_ACCOUNT_NO').next('span').addClass("field-validation-valid");
            }

            if ($('#BASIC_DA').val().trim() == '') {
                $('#BASIC_DA').next('span').text("Enter Basic DA").show();
                $('#BASIC_DA').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#BASIC_DA').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#BASIC_DA').next('span').text("Enter only numbers in BASIC DA").show();
                    $('#BASIC_DA').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#BASIC_DA').next('span').text("").hide();
                    $('#BASIC_DA').next('span').addClass("field-validation-valid");
                }
            }

            if ($('#HRA').val().trim() == '') {
                $('#HRA').next('span').text("Enter HRA").show();
                $('#HRA').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#HRA').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#HRA').next('span').text("Enter only numbers in HRA").show();
                    $('#HRA').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#HRA').next('span').text("").hide();
                    $('#HRA').next('span').addClass("field-validation-valid");
                }
            }

            if ($('#SPECIAL_ALLOWANCES').val().trim() == '') {
                $('#SPECIAL_ALLOWANCES').next('span').text("Enter Special Allowances").show();
                $('#SPECIAL_ALLOWANCES').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#SPECIAL_ALLOWANCES').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#SPECIAL_ALLOWANCES').next('span').text("Enter only numbers in Special Allowance").show();
                    $('#SPECIAL_ALLOWANCES').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#SPECIAL_ALLOWANCES').next('span').text("").hide();
                    $('#SPECIAL_ALLOWANCES').next('span').addClass("field-validation-valid");
                }
            }

            if ($('#GROSS').val().trim() == '') {
                $('#GROSS').next('span').text("Enter Gross Salary").show();
                $('#GROSS').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                var inputVal = $('#GROSS').val();
                var valid = $.isNumeric(inputVal);

                if (!valid) {
                    $('#GROSS').next('span').text("Enter only numbers in Gross Salary").show();
                    $('#GROSS').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#GROSS').next('span').text("").hide();
                    $('#GROSS').next('span').addClass("field-validation-valid");
                }
            }
            return return_val;
        },
        SearchEmpbyDept: function () {
            var validate = function () {
                var return_val = true;
                if ($("#DEPT_ID option:selected").val() == '') {
                    var errorMessage = "Please select Department.";
                    $('#DEPT_ID').next('span').text(errorMessage).show();
                    $('#DEPT_ID').next('span').addClass("field-validation-error");
                    return_val = false;
                }
                else {
                    $('#DEPT_ID').next('span').text(errorMessage).hide();
                    $('#DEPT_ID').next('span').addClass("field-validation-valid");
                }
                return return_val;
            }

            if (!validate()) {
                event.preventDefault();
                return;
            }
            var deptId = $("#DEPT_ID option:selected").val();
            $.get('/Workforce/GetEmployeesBydeptId?deptId=' + deptId, function (data) {
                $('#WF_ID').find('option').not(':first').remove();
                $.each(data, function (i, item) {
                    $('#WF_ID').append($('<option>', {
                        value: item.WF_ID,
                        text: item.EMP_NAME + '-' + item.EMP_ID
                    }));
                });

                $('#WF_ID').formSelect();
            });
        }
    };

    function initialize() {
        $('#mrf-search').on("click", Wfm.App.WorkForce.Search);
        $('#emp-search').on("click", Wfm.App.WorkForce.EmpSalaryDetailSearch);
        $('#dailyworkitem-add').on("click", Wfm.App.WorkForce.DailyWorkItemAdd);
        $('#workforce-loadsalary').on("click", Wfm.App.WorkForce.LoadSalary);

        $('#submit').on("click", Wfm.App.WorkForce.Create);
        $('#submitSal').on("click", Wfm.App.WorkForce.CreateSalDetail);
        $('#editsubmit').on("click", Wfm.App.WorkForce.Edit);
        $('#cancel').on("click", Wfm.App.WorkForce.Cancel);
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
        $('#WorkForce_delete_confirm').on("click", Wfm.App.WorkForce.Delete);
        $('#workforce-loadfaultyattendance').on("click", Wfm.App.WorkForce.LoadFaultyAttendance);
        $('#workforce-loadattendance').on("click", Wfm.App.WorkForce.LoadAttendance);
        
        $('#btnWorkItemSave').on("click", Wfm.App.WorkForce.DailyWorkItemSave)
        $('#emplst-search').on("click", Wfm.App.WorkForce.SearchEmpbyDept)
        $('#WF_ID').on("change", Wfm.App.WorkForce.EmpSearch)
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var WorkForceId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenWorkForceId").val(WorkForceId);
    }

   

    function SetDWEmpData(result) {
        if (result === null || result === undefined || result.ACTION == 'NO DATA') {
            var errorMessage = "EMP ID " + $('#EMP_ID').val() + " not found.";
            $('#EMP_ID').next('span').text(errorMessage).show();
            $('#EMP_ID').next('span').addClass("field-validation-error");

            $('#WF_ID').val("");
            $('#EMP_NAME').val("");
            $('#DEPT_ID').val("");
            $('#DEPT_NAME').val("");
            $('#COMPANY_ID').val("");
            $('#SEARCHEDEMP_ID').val("");

            return;
        }
        else
        {
            $('#WF_ID').val(result.WF_ID);
            $('#EMP_NAME').val(result.EMP_NAME);
            $('#COMPANY_ID').val(result.COMPANY_ID);
            $('#DEPT_ID').val(result.DEPT_ID);
            $('#DEPT_NAME').val(result.DEPT_NAME);
            $('#SEARCHEDEMP_ID').val(result.EMP_ID);
            $("#DEPT_NAME").focus();
            $("#WORK_DATE").focus();
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

    function SetEmpProfileData(result) {

        if (result === null || result === undefined) {
            var errorMessage = "Employee not found.";
            $('#WF_ID').next('span').text(errorMessage).show();
            $('#WF_ID').next('span').addClass("field-validation-error");
            return;
        }
        $('#EMP_NAME').val(result.EMP_NAME);
        $('#FATHER_NAME').val(result.FATHER_NAME);
        var idx = $('#GENDER').find('option[value="' + result.GENDER + '"]').index()
        $('#GENDER').get(0).selectedIndex = idx

        $.each(result.WF_EMP_TYPEList, function (a) {
            $('#WF_EMP_TYPE_TEXT').val(result.WF_EMP_TYPEList[a].Text);
            $('#WF_EMP_TYPE').val(result.WF_EMP_TYPEList[a].Value);
        });

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

    function GetTodayDate() {
        var tdate = new Date();
        var dd = tdate.getDate(); //yields day
        var MM = tdate.getMonth(); //yields month
        var yyyy = tdate.getFullYear(); //yields year
        var currentDate = dd + "-" + (MM + 1) + "-" + yyyy;

        return currentDate;
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

function EmpSearchClick() {
    debugger
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
        url: "/Workforce/EmpSalaryDetailSearch",
        data: JSON.stringify(wfObj),
        contentType: "application/json",
        dataType: "json",
        beforeSend: function () {
            $('.page-loader-wrapper').show();
        },
        complete: function () {
            $('.page-loader-wrapper').hide();
        },
        success: function (result) {
            SetEmpData(result);
        },
        error: function (responseText) {
            alert(responseText);
        }
    });
}
function SetEmpData(result) {

    if (result === null || result === undefined || result.ACTION == 'NO DATA') {
        var errorMessage = "EMP ID " + $('#EMP_ID').val() + " not found.";
        $('#EMP_ID').next('span').text(errorMessage).show();
        $('#EMP_ID').next('span').addClass("field-validation-error");

        $('#WF_ID').val("");
        $('#EMP_NAME').val("");
        $('#ACTION').val("");
        $('#COMPANY_ID').val("");
        $("#submitSal").html("SUBMIT");

        $('#UAN_NO').val("");
        $('#PAN_CARD').val("");
        $('#EPF_NO').val("");
        $('#ESIC_NO').val("");
        $('#SELECTEDBANKNAME').val("");
        $('#SELECTEDBANKID').val("");
        $('#BANK_IFSC').val("");
        $('#BANK_BRANCH').val("");
        $('#BANK_ACCOUNT_NO').val("");
        $('#BASIC_DA').val("");
        $('#HRA').val("");
        $('#SPECIAL_ALLOWANCES').val("");
        $('#GROSS').val("");

        return;
    }

    if (result.ACTION == 'ADD') {
        $('#WF_ID').val(result.WF_ID);
        $('#EMP_NAME').val(result.EMP_NAME);
        $('#ACTION').val(result.ACTION);
        $('#COMPANY_ID').val(result.COMPANY_ID);
        $("#submitSal").html(result.ACTION);

        $('#UAN_NO').val("");
        $('#PAN_CARD').val("");
        $('#EPF_NO').val("");
        $('#ESIC_NO').val("");
        $('#SELECTEDBANKNAME').val("");
        $('#SELECTEDBANKID').val("");
        $('#BANK_IFSC').val("");
        $('#BANK_BRANCH').val("");
        $('#BANK_ACCOUNT_NO').val("");
        $('#BASIC_DA').val("");
        $('#HRA').val("");
        $('#SPECIAL_ALLOWANCES').val("");
        $('#GROSS').val("");

        return;
    }


    if (result.ACTION == 'UPDATE') {

        $('#WF_ID').val(result.WF_ID);
        $('#EMP_NAME').val(result.EMP_NAME);
        $('#UAN_NO').val(result.UAN_NO);
        $('#PAN_CARD').val(result.PAN_CARD);
        $('#EPF_NO').val(result.EPF_NO);
        $('#ESIC_NO').val(result.ESIC_NO);
        debugger;
        $('#BANK_ID').val(result.BANK_ID).change();
        $('#BANK_ID').formSelect();
        //var idx = $('#BANK_ID option[value="50AEFDE4-004D-EB11-9473-8CDCD4D2C4BC"]').index();
        $('#SELECTEDBANKNAME').val(result.SELECTEDBANKNAME);
        $('#SELECTEDBANKID').val(result.SELECTEDBANKID);
        $('#BANK_IFSC').val(result.BANK_IFSC);
        $('#BANK_BRANCH').val(result.BANK_BRANCH);
        $('#BANK_ACCOUNT_NO').val(result.BANK_ACCOUNT_NO);
        $('#BASIC_DA').val(result.BASIC_DA);
        $('#HRA').val(result.HRA);
        $('#SPECIAL_ALLOWANCES').val(result.SPECIAL_ALLOWANCES);
        /* SalCalculation();*/
        $('#GROSS').val(result.GROSS);
        $('#ACTION').val(result.ACTION);
        $('#COMPANY_ID').val(result.COMPANY_ID);
        $("#submitSal").html(result.ACTION);
        return;
    }
}