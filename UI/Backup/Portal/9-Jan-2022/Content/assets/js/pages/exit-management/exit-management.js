function departmentonChange() {
    $('#emplyeedetail').empty();
}

function employeeOnChange() {
    var emp_id = $('#WF_ID').val();
    if (emp_id === "") {
        $('#emplyeedetail').empty();
        $('#rowValue').val(0);
    } else {
        $.get('/ExitManagement/GetEmployeeAssetById?emp_id=' + emp_id, function (data) {
            $('#emplyeedetail').html(data);
            $('#emplyeedetail').fadeIn('fast');
            $('.dateddmmyyyy').bootstrapMaterialDatePicker({
                format: 'DD/MM/YYYY',
                clearButton: true,
                weekStart: 1,
                time: false,
                autoClose: true
            }).on('change', function (e, date) {
                getByResignationDateExitDate(date._d);
            });
            $('#NOTICE_DAYS').on("keyup", getByNoticeDayExitDate);

        });
    }
}

function exitEmployeeOnChange() {
    var emp_id = $('#WF_ID').val();
    if (emp_id === "00000000-0000-0000-0000-000000000000") {
        $('#emplyeedetail').empty();
    } else {
        $.get('/ExitManagement/EmployeesExitApprovalDetails?emp_id=' + emp_id, function (data) {
            $('#emplyeedetail').html(data);
            $('#emplyeedetail').fadeIn('fast');
        });
    }
}

function getEmployeeOnLoad() {
    $('#emplyeedetail').empty();
    exitEmployeeOnChange();
}


