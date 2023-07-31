$(document).ready(function () {
    var date = new Date();
    var TO = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    var FROM = new Date(date.getFullYear(), date.getMonth(), 1);

    $("select[required]").css({ position: "absolute", display: "inline", height: 0, padding: 0, width: 0 });
    $('.dateddmmyyyy').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY',
        clearButton: true,
        weekStart: 1,
        time: false,
        autoClose: true,
    });
    $('#FROM_DATE').bootstrapMaterialDatePicker('setDate', FROM);
    $('#TO_DATE').bootstrapMaterialDatePicker('setDate', TO);
    $("select[required]").css({ position: "absolute", display: "inline", height: 0, padding: 0, width: 0 });
    $('#DEPT_ID').find('option').not(':first').remove();
})

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

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
    var SUBDEPT_ID = $('#SUBDEPT_ID').val() || '';
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val() || 0;
    /* var EMP_NAME = $('#WF_ID').val();*/
    var FROM_DATE = $('#FROM_DATE').val();
    var TO_DATE = $('#TO_DATE').val();
    var EMP_NAMES = "";
    if ($('#EMP_NAME').val() != "") {
        EMP_NAMES = $('#WF_ID').val();
    }
    EMP_NAME = EMP_NAMES;
    debugger
    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (DEPT_ID == "") {
        toastr["error"]('Please Select Department.');
        return;
    } else if (WF_EMP_TYPE == "") {
        toastr["error"]('Please Select Workforce Type.');
        return;
    } else if (FROM_DATE == "") {
        toastr["error"]('Please Select From Date.');
        return;
    } else if (TO_DATE == "") {
        toastr["error"]('Please Select To Date.');
        return;
    }else if ((FROM_DATE.substring(3, 5)) != (TO_DATE.substring(3, 5))) {
        toastr["error"]('Please select same month in From and To Date.');
        return;
    }
   
    if (EMP_NAME == "0") {
        toastr["error"]('Please Select Employee.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/APReports?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&FROM_DATE=' + FROM_DATE + '&TO_DATE=' + TO_DATE + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                        var EmpList = result.EmpList;
                        var DateList = result.EmpList[0].AttendanceList;
                        var col = Number(DateList.length) + 21;

                        html += '<div class="table-responsive">';
                        html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                        html += '<tr  style="background: #29c1c9;color:white;font-weight:400!important">';
                        html += '<th class="bold" colspan=' + col + ' style="text-align: center; font-size: 1.7rem; ">Consolidated Absent Present Report</th>';
                        html += '</tr>';
                        html += '<tr>'
                        html += '<th class="bold" colspan=' + col + ' style="font-size: 1rem; "> Company : ' + result.COMPANY_NAME + '</th>'
                        html += '</tr>'
                        html += '<tr>'
                        html += '<th class="bold" colspan=' + col + ' style="font-size: 1rem; "> From : ' + FROM_DATE + ' To : ' + TO_DATE + '</th>'
                        html += '</tr>'
                        html += '<tr >'
                        html += '<th class="bold" colspan=' + col + ' style="font-size: 1rem; "> Department : ' + result.DEPT_NAME + '</th>'
                        html += '</tr>'
                        html += '<tr  style="border: 1px solid black;">'

                        html += '<td class="bold">S.No.</td>'
                        html += '<td class="bold">Employee Code</td>'
                        html += '<td class="bold">Bio. Code</td>'
                        html += '<td class="bold">Employee Name</td>'

                        for (var i = 0; i < DateList.length; i++) {
                            html += '<td class="bold">' + DateList[i].Dates + '</td>'
                        }
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold"></td>'
                        html += '<td class="bold">P</td>'
                        html += '<td class="bold">A</td>'
                        html += '<td class="bold">L</td>'
                        html += '<td class="bold">H</td>'
                        html += '<td class="bold">HP</td>'
                        html += '<td class="bold">WO</td>'
                        html += '<td class="bold">WOP</td>'

                        html += '</tr>';


                        html += '</thead>'
                        html += '<tbody>'

                        for (var i = 0; i < EmpList.length; i++) {
                            /* console.log(EmpList);*/
                            var data = EmpList[i];
                            html += '<tr style="border: 1px solid black;">'
                            html += '<td>' + (i + 1) + '</td>'
                            html += '<td style="text-align:left">' + data.EMP_ID + '</td>'
                            html += '<td>' + data.BIOMETRIC_CODE + '</td>'
                            html += '<td style="text-align:left">' + data.EMP_NAME + '</td>'
                            var sdate = FROM_DATE.substring(0, 2);
                            var tdate = TO_DATE.substring(0, 2);
                            var P = 0, A = 0, L = 0, H = 0, HP = 0, WO = 0, WOP = 0;
                            for (var j = 0; j < data.AttendanceList.length; j++, sdate++) {
                                var dt = data.AttendanceList[j];
                                if (dt.Dates == sdate) {
                                    html += '<td>' + dt.OT + '</td>'
                                    if (dt.OT == 'P') {
                                        P += 1;
                                    } if (dt.OT == 'A') {
                                        A += 1;
                                    } if (dt.OT == 'L') {
                                        L += 1;
                                    } if (dt.OT == 'H') {
                                        H += 1;
                                    } if (dt.OT == 'HP') {
                                        HP += 1;
                                    } if (dt.OT == 'WO') {
                                        WO += 1;
                                    } if (dt.OT == 'WOP') {
                                        WOP += 1;
                                    }

                                }
                               
                            }
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>'
                            html += '<td>' + P + '</td>'
                            html += '<td>' + A + '</td>'
                            html += '<td>' + L + '</td>'
                            html += '<td>' + H + '</td>'
                            html += '<td>' + HP + '</td>'
                            html += '<td>' + WO + '</td>'
                            html += '<td>' + WOP + '</td>'
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

