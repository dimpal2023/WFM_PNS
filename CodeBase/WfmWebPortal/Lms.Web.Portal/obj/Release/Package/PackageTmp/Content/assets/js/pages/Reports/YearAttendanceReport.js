$(document).ready(function () {
    $("select[required]").css({ position: "absolute", display: "inline", height: 0, padding: 0, width: 0 });
    $('#DEPT_ID').find('option').not(':first').remove();
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
        $('#EMPLOYMENT_TYPE').val('');
        $('#EMPLOYMENT_TYPE').formSelect();
    });
}

function changeOnSubDept() {
    $('#EMP_NAME').find('option').not(':first').remove();
    $('#WF_EMP_TYPE').val('');
    $('#WF_EMP_TYPE').formSelect();
    $('#EMPLOYMENT_TYPE').val('');
    $('#EMPLOYMENT_TYPE').formSelect();
    $("#EMP_NAME").val('');
}

function GetReport() {
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val() || 0;
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val()||0;
    var EMP_NAMES = "";
    if ($('#EMP_NAME').val() != "") {
        EMP_NAMES = $('#WF_ID').val();
    }
    EMP_NAME = EMP_NAMES;
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (DEPT_ID == "") {
        toastr["error"]('Please Select Department.');
        return;
    } else if (SUBDEPT_ID == "") {
        toastr["error"]('Please Select Sub Department.');
        return;
    } else if (YEAR_ID == "") {
        toastr["error"]('Please Select Year.');
        return;
    } else if (EMP_NAME == "") {
        toastr["error"]('Please Select Employee.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetYearAttendanceReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
            beforeSend: function () {
                $('.page-loader-wrapper').show();
            },
            complete: function () {
                $('.page-loader-wrapper').hide();
            },
            success:
                function (response) {
                    var html = '';
                    if (response != "") {
                        var List = $.parseJSON(response);
                        
                        
                        var result = List[0];
                        var Cols = result.list;
                      
                        html += '<div class="table-responsive">';
                        html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                        html += '<tr  style="background: #29c1c9;color:white;font-weight:400!important">';
                        html += '<th class="bold" colspan="13" style="text-align: center; font-size: 1.7rem; ">Year Attendance Report (Basic Report)</th>';
                        html += '</tr>';
                        html += '<tr>'
                        html += '<th class="bold" colspan="13" style="font-size: 1rem; "> Company : ' + result.COMPANY_NAME + '</th>'
                        html += '</tr>'
                        //html += '<tr>'
                        //html += '<th class="bold" colspan="11" style="font-size: 1rem; "> Attendance Date : ' + result.ATTENDANCE_DATE + '</th>'
                        //html += '</tr>'
                        html += '<tr >'
                        html += '<th class="bold" colspan="13" style="font-size: 1rem; "> Department : ' + result.DEPT_NAME + '</th>'
                        html += '</tr>'
                       
                        html += '<tr  style="border: 1px solid black;">'
                        html += '<td class="bold">S.No.</td>'
                        html += '<td class="bold">Employee Code</td>'
                        html += '<td class="bold">Bio. Code</td>'
                        html += '<td class="bold">Employee Name</td>'
                        html += '<td class="bold">Department</td>'
                        html += '<td class="bold">Month</td>'
                        html += '<td class="bold">Year</td>'
                        html += '<td class="bold">Present</td>'
                        html += '<td class="bold">Absent</td>'
                        html += '<td class="bold">Holiday</td>'
                        html += '<td class="bold">Holiday Present</td>'
                        html += '<td class="bold">WO</td>'
                        html += '<td class="bold">WOP</td>'
                       
                        html += '</tr>';

                       
                        html += '</thead>'
                        html += '<tbody>'
                       
                        for (var i = 0; i < Cols.length; i++) {
                            var data = Cols[i];
                            html += '<tr style="border: 1px solid black;">'
                            html += '<td>' + (i + 1) + '</td>'
                            html += '<td style="text-align:left">' + data.EMP_ID + '</td>'
                            html += '<td>' + data.BIOMETRIC_CODE + '</td>'
                            html += '<td style="text-align:left">' + data.EMP_NAME + '</td>'
                            html += '<td>' + result.DEPT_NAME + '</td>'
                            html += '<td>' + data.MonthNam + '</td>'
                            html += '<td>' + data.yearN + '</td>'
                            html += '<td>' + data.Present + '</td>'
                            html += '<td>' + data.Absent + '</td>'
                            html += '<td>' + data.Holiday + '</td>'
                            html += '<td>' + data.HolidayP + '</td>'
                            html += '<td>' + data.WO + '</td>'
                            html += '<td>' + data.WOP + '</td>'
                            html += '</tr>'
                        }
                        html += '</tbody >'
                        html += '</table>'
                        html += '</div>'

                    } else {

                        html += '<div class="text-center"><span>No data found.</span></div>';
                    }
                    $("#particalDiv").append(html + '<br>');
                },
            error:
                function (response) {
                   
                    alert("Error: " + response);
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
        $('#EMP_NAME').find('option').not(':first').remove();
        $('#WF_EMP_TYPE').val('');
        $('#WF_EMP_TYPE').formSelect();
        $('#EMPLOYMENT_TYPE').val('');
        $('#EMPLOYMENT_TYPE').formSelect();
    });
}

function conversion(mins) {// get duration by minute
    // getting the hours.
    debugger
    let hrs = Math.floor(mins / 60);
    // getting the minutes.
    let min = mins % 60;
    // formatting the hours.
    hrs = hrs < 10 ? '0' + hrs : hrs;
    // formatting the minutes.
    min = min < 10 ? '0' + min : min;
    // returning them as a string.
    if (`${hrs}:${min}` == '00:00') {
        return '';
    } else {
        return `${hrs}:${min}`;
    }

}


function ConvertMinutes(num) {    // count days by minute
    days = num / 510;
    return days.toFixed(2);
    //days = Math.floor(num / 1440);
    //hours = Math.floor((num % 1440) / 60);
    //minutes = (num % 1440) % 60;
    //return `${days}:${hours}:${minutes}`;
    //{
    //    days: days,
    //    hours: hours,
    //    //minutes: minutes
    //};
}

function ConvertInHours(num) {    // count hour by minute
    //days = Math.floor(num / 1440);
    //hours = (num % 1440) / 60;
    //return hours.toFixed(2);
    const hours = (num / 60);
    //const minutes = num % 60;
    return hours.toFixed(2);
}

function diff_minutes(dt2, dt1) {  // get minute by time

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= 60;
    return Math.abs(Math.round(diff));

}

