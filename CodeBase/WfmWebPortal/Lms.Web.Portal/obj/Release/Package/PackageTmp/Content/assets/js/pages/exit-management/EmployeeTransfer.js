$(document).ready(function () {
    $("select[required]").css({ position: "absolute", display: "inline", height: 0, padding: 0, width: 0 });
    $('#DEPT_ID').find('option').not(':first').remove();
    $('#DEPT_ID').formSelect();
})

function onBuildingChange() {
    var buildingId = $("#BUILDING_ID option:selected").val();
    $.get('/ManPowerRequest/GetFloorByBuildingId?buildingId=' + buildingId, function (data) {
        $('#DEPT_ID').find('option').not(':first').remove();
        $.each(data, function (i, item) {
            $('#DEPT_ID').append($('<option>', {
                value: item.DEPT_ID,
                text: item.DEPT_NAME
            }));
        });
        $('#DEPT_ID').formSelect();
       
    });
}
function AllonBuildingChange() {
    var buildingId = $("#BUILDING_ID option:selected").val();
    $.get('/ManPowerRequest/GetAllFloorByBuildingId?buildingId=' + buildingId, function (data) {
        $('#DEPT_ID').find('option').not(':first').remove();
        $.each(data, function (i, item) {
            $('#DEPT_ID').append($('<option>', {
                value: item.DEPT_ID,
                text: item.DEPT_NAME
            }));
        });
        $('#DEPT_ID').formSelect();
       
    });
}

function TransferEmployee() {
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID option:selected').val();
    var DEPT_ID = $('#DEPT_ID option:selected').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID option:selected').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE option:selected').val();
    var EMP_NAME = $('#EMP_NAME option:selected').val();

    if (EMP_NAME == "" || EMP_NAME == "0") {
        alert('Please select Employee.');
        return;
    }
    else if (BUILDING_ID == "") {
        alert('Please select New Unit.');
        return;
    }
    else if (DEPT_ID == "") {
        alert('Please select New Department.');
        return;
    }
    else if (SUBDEPT_ID == "") {
        alert('Please select New Sub Department.');
        return;
    }
    else if (EMPLOYMENT_TYPE == "") {
        alert('Please select New Employment Type.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/ExitManagement/Employee_Transfer?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE + '&WF_ID=' + EMP_NAME,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $('.page-loader-wrapper').show();
            },
            complete: function () {
                $('.page-loader-wrapper').hide();
            },
            success: function (response) {
                debugger;
                if (response == true) {
                    alert("Transfer request added Successfully.");
                    window.location.href = "/ExitManagement/EmployeeTransfer";
                } else {
                    alert("Transfer request failed.");
                }
            },
            error: function (response) {
                console.log("Error: " + response);
            }
        });
}

function onDepartmentChange() {


    var departmentId = $("#DEPT_ID option:selected").val();
    $.get('/Attendance/GetSubDepartmentByDepartmentId?departmentId=' + departmentId, function (data) {
        $('#SUBDEPT_ID').find('option').not(':first').remove();
        $.each(data, function (i, item) {
            $('#SUBDEPT_ID').append($('<option>', {
                value: item.SUBDEPT_ID,
                text: item.SUBDEPT_NAME
            }));
        });
        $('#SUBDEPT_ID').formSelect();
    });
}
function AllonDepartmentChange() {


    var departmentId = $("#DEPT_ID option:selected").val();
    $.get('/Attendance/GetAllSubDepartmentByDepartmentId?departmentId=' + departmentId, function (data) {
        $('#SUBDEPT_ID').find('option').not(':first').remove();
        $.each(data, function (i, item) {
            $('#SUBDEPT_ID').append($('<option>', {
                value: item.SUBDEPT_ID,
                text: item.SUBDEPT_NAME
            }));
        });
        $('#SUBDEPT_ID').formSelect();
    });
}
