$(document).ready(function () {
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
    debugger
    if ($('#EMP_NAME').val() == '') {
        $('#WF_ID').val('');
    }
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val()||3;
    var EMP_NAME = $('#WF_ID').val();
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (DEPT_ID == "") {
        toastr["error"]('Please Select Department.');
        return;
    } else if (WF_EMP_TYPE == "") {
        toastr["error"]('Please Select Workforce Type.');
        return;
    } else if (EMP_NAME == "") {
        toastr["error"]('Please Select Employee.');
        return;
    } else if (MONTH_ID == "") {
        toastr["error"]('Please Select Month.');
        return;
    } else if (YEAR_ID == "") {
        toastr["error"]('Please Select Year.');
        return;
    }

    
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                        //var Cols = result.AttendanceSheet.length;
                        var Cols = Number(daysInMonth(MONTH_ID, YEAR_ID));
                        var AtList = result.AttendanceSheet;
                        var col = Number(Cols) + 1;
                        if (result.AttendanceSheet.length > 0) {
                            var CountHrs = 0;
                            for (var l = 0; l < AtList.length; l++) {

                                //-------------------------- calulate eary+late 
                                if (AtList[l].Permission > 0) {
                                    CountHrs += Number(AtList[l].Permission);
                                }

                            }
                            debugger
                            var calPermissionInDays = ConvertInHours(CountHrs) / 8;

                            html += '<div class="table-responsive">';
                            html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                            html += '<tr style="background: #29c1c9; color: white; font-weight: 400 !important;">';

                            html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">  Monthly Attendance Report </td>';

                            html += '</tr>';
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">' + $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text() + '</td>'
                            html += '</tr>';
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">Company : ' + result.COMPANY_NAME + '</td>'
                            html += '</tr>'
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Department : ' + result.DEPT_NAME + '</td>'
                            html += ' </tr>'
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            html += '<td colspan="' + col + '" style="font-size: 1.2rem; "> Employee : ' + result.EMP_NAME + '</td>'
                            html += '</tr>'
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Biometric Code : ' + result.BIOMETRIC_CODE + '</td>'
                            html += '</tr>'
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                            debugger;
                            var TotalOTHours = 0, totalAbset = 0;
                            if (result.DEPT_NAME == 'FORGING SHOP') {
                                TotalOTHours = Number(result.OTHours_Forging) / 60;
                            } else {
                                TotalOTHours = Number(result.OTHours) / 60;
                            }
                            
                            if (Number(result.Absent) > 0) {
                                totalAbset = Number(result.Absent) - 1;
                            }
                            const d = new Date();
                            let month = d.getMonth() + 1;
                            var TotalPaidDays = 0;
                            if (MONTH_ID < month) {
                                 TotalPaidDays = 26 - (totalAbset);
                                var TotalPaidDays = TotalPaidDays - Number(calPermissionInDays).toFixed(2);
                            } else {
                                TotalPaidDays = result.Present + result.WeeklyOff + result.Absent + result.Holiday;
                                if (TotalPaidDays > 26) {
                                    TotalPaidDays = 26 - (totalAbset);
                                    var TotalPaidDays = TotalPaidDays - Number(calPermissionInDays).toFixed(2);
                                }
                                
                                var TotalPaidDays = TotalPaidDays - totalAbset - Number(calPermissionInDays).toFixed(2);
                            }
                            /* var PermissionINHours = CountHrs/60;*/
                            html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Days Present : ' + result.Present + ' &nbsp; &nbsp; &nbsp; Days Paid : ' + TotalPaidDays + ' &nbsp; &nbsp; &nbsp;  Absent : ' + result.Absent + ' &nbsp; &nbsp; &nbsp;  WeeklyOff : ' + result.WeeklyOff + ' &nbsp; &nbsp; &nbsp; Holiday : ' + result.Holiday + ' &nbsp; &nbsp; &nbsp; Leaves : ' + 0 + ' &nbsp; &nbsp; &nbsp; OT Hrs :' + TotalOTHours.toFixed(2) + ' &nbsp; &nbsp; &nbsp; Early By Days : ' + result.EarlyByDay + ' &nbsp; &nbsp; &nbsp; Late By Days : ' + result.LateByDay + ' &nbsp; &nbsp; &nbsp; Permission By Hrs : ' + (ConvertInHours(CountHrs) == '' ? '00:00' : ConvertInHours(CountHrs)) + ' &nbsp; &nbsp; &nbsp; Permission By Days: ' + Number(calPermissionInDays).toFixed(2) + ' &nbsp; &nbsp; &nbsp;</td>'
                            html += '</tr>'
                            html += '</thead>'

                            html += '<tbody>'



                            html += '<tr style="border: 1px solid black; ">';
                            html += '<td class="bold">Dates</td>';
                            for (var i = 1; i <= Cols; i++) {
                                html += '<td class="bold" style = "text-align:center">' + i + '</td>';
                            }
                            html += '</tr>';

                            html += '<tr style="border: 1px solid black;background:#ecf5f9;">';
                            html += '<td class="bold">Status</td>';
                            for (var i = 1; i <= Cols; i++) {

                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        html += '<td style = "text-align:center">' + AtList[j].StatusCode + '</td>';
                                        break;
                                    }
                                }

                            }
                            html += '</tr>'

                            html += '<tr style="border: 1px solid black; ">'
                            html += '<td class="bold">In</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        if (AtList[j].SHIFT_STARTTIME != "12:00") {
                                            html += '<td style = "text-align:center">' + AtList[j].SHIFT_STARTTIME + '</td>';
                                        } else {
                                            html += '<td style = "text-align:center">00:00</td>';
                                        }
                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black;background:#ecf5f9;">'
                            html += '<td class="bold">Out</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        if (AtList[j].SHIFT_ENDTIME != "12:00") {
                                            html += '<td style = "text-align:center">' + AtList[j].SHIFT_ENDTIME + '</td>';
                                        } else {
                                            html += '<td style = "text-align:center">00:00</td>';
                                        }
                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black; ">'
                            html += '<td class="bold">Duration</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        if (result.DEPT_NAME == 'FORGING SHOP' && AtList[j].WorkDuration >= 510) {
                                            html += '<td style = "text-align:center">8:00</td>';
                                            break;
                                        }
                                        else {
                                            html += '<td style = "text-align:center">' + AtList[j].Duration + '</td>';
                                            break;
                                        }
                                        
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black;background:#ecf5f9;">'
                            html += '<td class="bold">Early By</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        html += '<td style = "text-align:center">' + AtList[j].EarlyBy + '</td>';
                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black; ">'
                            html += '<td class="bold">Late By</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        html += '<td style = "text-align:center">' + AtList[j].LateBy + '</td>';
                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black;background:#ecf5f9;">'
                            html += '<td class="bold">OT</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        if (result.DEPT_NAME == 'FORGING SHOP') {
                                            html += '<td style = "text-align:center">' + AtList[j].OT_ForgingShop + '</td>';
                                            break;
                                        } else {
                                            html += '<td style = "text-align:center">' + AtList[j].OT + '</td>';
                                            break;
                                        }
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black; ">'
                            html += '<td class="bold">Permission</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        var outminutes = 0;

                                        if (AtList[j].Permission > 0) {
                                            outminutes += Number(AtList[j].Permission);
                                        }
                                        html += '<td style = "text-align:center">' + conversion(outminutes) + '</td>';

                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '<tr style="border: 1px solid black;background:#ecf5f9;">'
                            html += '<td class="bold">Shift</td>'
                            for (var i = 1; i <= Cols; i++) {
                                for (var j = 0; j < AtList.length; j++) {
                                    if (AtList[j].Date == i) {
                                        html += '<td style = "text-align:center">' + AtList[j].shifts + '</td>';
                                        break;
                                    }
                                }
                            }
                            html += '</tr>'
                            html += '</tbody >'
                            html += '</table>'
                            html += '</div>'

                        }
                        else {
                            html += '<div class="text-center"><span>No data found.</span></div>';
                        }
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

function test(empId) {
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val();
    var EMP_NAME = empId;
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "" || DEPT_ID == "" || SUBDEPT_ID == "" || WF_EMP_TYPE == "" || EMPLOYMENT_TYPE == "" || EMP_NAME == "" || MONTH_ID == "" || YEAR_ID == "") {
        alert('All fields are mendatory.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetReport1?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
            //beforeSend: function () {
            //    $('.page-loader-wrapper').show();
            //},
            //complete: function () {
            //    $('.page-loader-wrapper').hide();
            //},
            success:
                function (response) {
                    $("#particalDivAll").append(response);
                    $("#monthwithyear").text($("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text());
                },
            error:
                function (response) {
                    debugger;
                    alert("Error: " + response);
                }
        });
}


function GetAll() {
    if ($('#EMP_NAME').val() == '') {
        $('#WF_ID').val('');
    }
    $("#particalDivAll").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val()||3;
    var EMP_NAME = "";
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (DEPT_ID == "") {
        toastr["error"]('Please Select Department.');
        return;
    } else if (WF_EMP_TYPE == "") {
        toastr["error"]('Please Select Workforce Type.');
        return;
    } else if (MONTH_ID == "") {
        toastr["error"]('Please Select Month.');
        return;
    } else if (YEAR_ID == "") {
        toastr["error"]('Please Select Year.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
            beforeSend: function () {
                $('.page-loader-wrapper').show();
            },
            complete: function () {
                $('.page-loader-wrapper').hide();
            },
            success:
                function (response) {
                    var List = $.parseJSON(response);
                    var html = '';
                    for (var t = 0; t < List.length; t++) {
                        debugger;
                        var result = List[t];
                        //var Cols = result.AttendanceSheet.length;
                        var Cols = Number(daysInMonth(MONTH_ID, YEAR_ID));
                        var AtList = result.AttendanceSheet;
                       
                        if (result.Present > 0) {
                            var col = Number(Cols) + 1;
                            if (Cols > 0) {
                                var CountHrs = 0;
                                for (var l = 0; l < AtList.length; l++) {
                                    if (AtList[l].Permission > 0) {
                                        CountHrs += Number(AtList[l].Permission);
                                    }
                                }
                                var calPermissionInDays = ConvertInHours(CountHrs) / 8;
                                html += '<div class="table-responsive">';
                                html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                                html += '<tr style="background: #29c1c9; color: white; font-weight: 400 !important;">';

                                html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">  Monthly Attendance Report </td>';

                                html += '</tr>';
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                                html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">' + $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text() + '</td>'
                                html += '</tr>';
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                                html += '<td class="bold" colspan="' + col + '" style="text-align: center; font-size: 1.7rem; ">Company : ' + result.COMPANY_NAME + '</td>'
                                html += '</tr>'
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                                html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Department : ' + result.DEPT_NAME + '</td>'
                                html += ' </tr>'
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                                html += '<td colspan="' + col + '" style="font-size: 1.2rem; "> Employee : ' + result.EMP_NAME + '</td>'
                                html += '</tr>'
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                                html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Biometric Code : ' + result.BIOMETRIC_CODE + '</td>'
                                html += '</tr>'
                                html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'

                                var TotalOTHours = 0, totalAbset = 0;
                                if (result.DEPT_NAME == 'FORGING SHOP') {
                                    TotalOTHours = Number(result.OTHours_Forging) / 60;
                                } else {
                                    TotalOTHours = Number(result.OTHours) / 60;
                                }
                              
                                if (Number(result.Absent) > 0) {
                                    totalAbset = Number(result.Absent) - 1;
                                }
                                const d = new Date();
                                let month = d.getMonth() + 1;
                                var TotalPaidDays = 0;
                                if (MONTH_ID < month) {
                                    TotalPaidDays = 26 - (totalAbset);
                                    var TotalPaidDays = TotalPaidDays - Number(calPermissionInDays).toFixed(2);
                                } else {
                                    TotalPaidDays = result.Present + result.WeeklyOff + result.Absent + result.Holiday;
                                    if (TotalPaidDays > 26) {
                                        TotalPaidDays = 26 - (totalAbset);
                                        var TotalPaidDays = TotalPaidDays - Number(calPermissionInDays).toFixed(2);
                                    }

                                    var TotalPaidDays = TotalPaidDays - totalAbset - Number(calPermissionInDays).toFixed(2);
                                }
                                //var TotalPaidDays = 26 - (totalAbset);
                                //var TotalPaidDays = TotalPaidDays - Number(calPermissionInDays).toFixed(2);
                                /* var PermissionINHours = CountHrs/60;*/
                                html += '<td colspan="' + col + '" style="font-size: 1.2rem; ">Days Present : ' + result.Present + ' &nbsp; &nbsp; &nbsp; Days Paid : ' + TotalPaidDays + ' &nbsp; &nbsp; &nbsp;  Absent : ' + result.Absent + ' &nbsp; &nbsp; &nbsp;  WeeklyOff : ' + result.WeeklyOff + ' &nbsp; &nbsp; &nbsp; Holiday : ' + result.Holiday + ' &nbsp; &nbsp; &nbsp; Leaves : ' + result.Holiday + ' &nbsp; &nbsp; &nbsp; OT Hrs :' + TotalOTHours.toFixed(2) + ' &nbsp; &nbsp; &nbsp; Early By Days : ' + result.EarlyByDay + ' &nbsp; &nbsp; &nbsp; Late By Days : ' + result.LateByDay + ' &nbsp; &nbsp; &nbsp; Gate Pass Hrs : ' + (ConvertInHours(CountHrs) == '' ? '00:00' : ConvertInHours(CountHrs)) + ' &nbsp; &nbsp; &nbsp; Gate Pass Days: ' + Number(calPermissionInDays).toFixed(2) + ' &nbsp; &nbsp; &nbsp;</td>'
                                html += '</tr>'
                                html += '</thead>'

                                html += '<tbody>'



                                html += '<tr style="border: 1px solid black; ">';
                                html += '<td class="bold">Dates</td>';
                                for (var i = 1; i <= Cols; i++) {
                                    html += '<td class="bold" style = "text-align:center">' + i + '</td>';
                                }
                                html += '</tr>';

                                html += '<tr style="border: 1px solid black;background:#ecf5f9; ">';
                                html += '<td class="bold">Status</td>';
                                for (var i = 1; i <= Cols; i++) {

                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            html += '<td style = "text-align:center">' + AtList[j].StatusCode + '</td>';
                                            break;
                                        }
                                    }

                                }
                                html += '</tr>'

                                html += '<tr style="border: 1px solid black; ">'
                                html += '<td class="bold">In</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            if (AtList[j].SHIFT_STARTTIME != "12:00") {
                                                html += '<td style = "text-align:center">' + AtList[j].SHIFT_STARTTIME + '</td>';
                                            } else {
                                                html += '<td style = "text-align:center">00:00</td>';
                                            }
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black;background:#ecf5f9; ">'
                                html += '<td class="bold">Out</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            if (AtList[j].SHIFT_ENDTIME != "12:00") {
                                                html += '<td style = "text-align:center">' + AtList[j].SHIFT_ENDTIME + '</td>';
                                            } else {
                                                html += '<td style = "text-align:center">00:00</td>';
                                            }
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black; ">'
                                html += '<td class="bold">Duration</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            if (result.DEPT_NAME == 'FORGING SHOP' && AtList[j].WorkDuration >= 510) {
                                                html += '<td style = "text-align:center">8:00</td>';
                                                break;
                                            }
                                            else {
                                                html += '<td style = "text-align:center">' + AtList[j].Duration + '</td>';
                                                break;
                                            }
                                          
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black;background:#ecf5f9; ">'
                                html += '<td class="bold">Early By</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            html += '<td style = "text-align:center">' + AtList[j].EarlyBy + '</td>';
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black; ">'
                                html += '<td class="bold">Late By</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            html += '<td style = "text-align:center">' + AtList[j].LateBy + '</td>';
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black;background:#ecf5f9; ">'
                                html += '<td class="bold">OT</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            if (result.DEPT_NAME == 'FORGING SHOP') {
                                                html += '<td style = "text-align:center">' + AtList[j].OT_ForgingShop + '</td>';
                                                break;
                                            } else {
                                                html += '<td style = "text-align:center">' + AtList[j].OT + '</td>';
                                                break;
                                            }
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black; ">'
                                html += '<td class="bold">Permission</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            var outminutes = 0;

                                            if (AtList[j].Permission > 0) {
                                                outminutes += Number(AtList[j].Permission);
                                            }
                                            html += '<td style = "text-align:center">' + conversion(outminutes) + '</td>';
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '<tr style="border: 1px solid black; background:#ecf5f9;">'
                                html += '<td class="bold">Shift</td>'
                                for (var i = 1; i <= Cols; i++) {
                                    for (var j = 0; j < AtList.length; j++) {
                                        if (AtList[j].Date == i) {
                                            html += '<td style = "text-align:center">' + AtList[j].shifts + '</td>';
                                            break;
                                        }
                                    }
                                }
                                html += '</tr>'
                                html += '</tbody >'
                                html += '</table>'
                                html += '</div><br/><br/>'
                            }
                        }
                    }
                    $("#particalDivAll").append(html + '<br>');
                    if ($("#particalDivAll").html() != "") {
                        var contents = $("#particalDivAll").html();
                        //window.open('data:application/vnd.ms-excel,' + encodeURIComponent(contents));
                        var result = 'data:application/vnd.ms-excel,' + encodeURIComponent(contents);
                        var link = document.createElement("a");
                        document.body.appendChild(link);
                        var txt = $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text();
                        link.download = "All Monthly Attendance Report - " + txt + ".xls"; //You need to change file_name here.
                        link.href = result;
                        link.click();
                    }


                },
            error:
                function (response) {

                    alert("Error: " + response);
                }
        });
}

function GetAll1() {
    debugger;
    var k = 0;
    var len = $("#EMP_NAME option").length;
    $("#EMP_NAME option").each(function () {
        if (this.value != '0') {
            debugger;
            test(this.value);

        } else if (this.value == '288c2973-8b68-408c-95d7-a686ecc64632') {
            var contents = $("#particalDivAll").html();
            /*  $('.page-loader-wrapper').hide();*/
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent(contents));
        }
        //k++;
        //if (k == 10) {
        //    return;
        //}

    })
    //if (k == len) {
    //    var contents = $("#particalDivAll").html();
    //    /*  $('.page-loader-wrapper').hide();*/
    //    window.open('data:application/vnd.ms-excel,' + encodeURIComponent(contents));
    //}

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

