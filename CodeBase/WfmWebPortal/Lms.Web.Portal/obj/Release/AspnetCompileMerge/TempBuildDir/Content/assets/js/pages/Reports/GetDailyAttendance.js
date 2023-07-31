$(document).ready(function () {
    var date = new Date();
    var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    $('.dateddmmyyyy').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY',
        clearButton: true,
        weekStart: 1,
        time: false,
        autoClose: true,
    });
    $('.dateddmmyyyy').bootstrapMaterialDatePicker('setDate', today);
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
    var SHIFT_ID = $('#SHIFT_ID').val();
    var FROM_DATE = $('#FROM_DATE').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (DEPT_ID == "") {
        toastr["error"]('Please Select Department.');
        return;
    } else if (SUBDEPT_ID == "") {
        toastr["error"]('Please Select Sub Department.');
        return;
    } else if (FROM_DATE == "") {
        toastr["error"]('Please Select Date.');
        return;
    }
   
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetDailyAttendanceReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&SHIFT_ID=' + SHIFT_ID + '&FROM_DATE=' + FROM_DATE + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                       
                        debugger
                        var result = List[0];
                        var Cols = result.list;
                      
                        html += '<div class="table-responsive">';
                        html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                        html += '<tr  style="background: #29c1c9;color:white;font-weight:400!important">';
                        html += '<th class="bold" colspan="11" style="text-align: center; font-size: 1.7rem; ">Daily Attendance Report (Basic Report)</th>';
                        html += '</tr>';
                        html += '<tr>'
                        html += '<th class="bold" colspan="11" style="font-size: 1rem; "> Company : ' + result.COMPANY_NAME + '</th>'
                        html += '</tr>'
                        html += '<tr>'
                        html += '<th class="bold" colspan="11" style="font-size: 1rem; "> Attendance Date : ' + result.ATTENDANCE_DATE + '</th>'
                        html += '</tr>'
                        html += '<tr >'
                        html += '<th class="bold" colspan="11" style="font-size: 1rem; "> Department : ' + result.DEPT_NAME + '</th>'
                        html += '</tr>'
                       
                        html += '<tr  style="border: 1px solid black;">'
                        html += '<td class="bold">S.No.</td>'
                        html += '<td class="bold">Employee Code</td>'
                        html += '<td class="bold">Bio. Code</td>'
                        html += '<td class="bold">Employee Name</td>'
                        html += '<td class="bold">Shift</td>'
                        html += '<td class="bold">InTime</td>'
                        html += '<td class="bold">OutTime</td>'
                        html += '<td class="bold">Work Duration</td>'
                        html += '<td class="bold">OT</td>'
                        html += '<td class="bold">Total Duration</td>'
                        html += '<td class="bold">Status</td>'
                       
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
                            html += '<td>' + data.SHIFT_NAME + '</td>'
                            html += '<td>' + data.InTime + '</td>'
                            html += '<td>' + data.OutTime + '</td>'
                            var Duration = Number(data.Duration);
                            var OverTime = Number(data.OverTime);
                            var DurationInHour = conversion(Duration);

                            debugger

                            if (Duration < 510) {
                                html += '<td>' + DurationInHour + '</td>'
                                html += '<td>0:00</td>'
                                html += '<td>' + DurationInHour + '</td>';
                            } else {
                                html += '<td>8:30</td>';
                                if (OverTime > 0) {
                                    html += '<td>' + conversion(OverTime) + '</td>';
                                    html += '<td>' + DurationInHour + '</td>';
                                } else {
                                    html += '<td>0:00</td>';
                                    html += '<td>8:30</td>';
                                }
                            }

                            html += '<td>' + data.Status + '</td>'
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
                    debugger;
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

