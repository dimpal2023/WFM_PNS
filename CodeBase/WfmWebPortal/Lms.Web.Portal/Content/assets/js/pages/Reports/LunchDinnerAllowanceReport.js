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
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val() ||'';
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val()||0;
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();
    var EMP_NAMES = "";
    if ($('#EMP_NAME').val() != "") {
        EMP_NAMES = $('#WF_ID').val();
    }
    EMP_NAME = EMP_NAMES;
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

    if (EMP_NAME == "0") {
        toastr["error"]('Please Select Employee.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/GetLunchDinnerReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                            //var DateList = result.EmpList[0].AttendanceList;
                            var col = 9;

                            html += '<div class="table-responsive">';
                            html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';
                            html += '<tr  style="background: #29c1c9;color:white;font-weight:400!important">';
                            html += '<th class="bold" colspan=' + col +' style="text-align: center; font-size: 1.7rem; ">Lunch/ Dinner Allowance Report</th>';
                            html += '</tr>';
                            html += '<tr>'
                            html += '<th class="bold" colspan=' + col +' style="font-size: 1rem; "> Company : ' + result.COMPANY_NAME + '</th>'
                            html += '</tr>'
                            //html += '<tr>'
                            //html += '<th class="bold" colspan=' + col + ' style="font-size: 1rem; "> From : ' + FROM_DATE + ' To : ' + TO_DATE + '</th>'
                            //html += '</tr>'
                            html += '<tr >'
                            html += '<th class="bold" colspan=' + col +' style="font-size: 1rem; "> Department : ' + result.DEPT_NAME + '</th>'
                            html += '</tr>'
                            html += '<tr  style="border: 1px solid black;">'

                            html += '<td class="bold">S.No.</td>'
                            html += '<td class="bold">Employee Code</td>'
                            html += '<td class="bold">Bio. Code</td>'
                            html += '<td class="bold">Employee Name</td>'
                            html += '<td class="bold">Department </td>'
                            html += '<td class="bold">Sub Department </td>'
                            html += '<td class="bold">Lunch Days</td>'
                            html += '<td class="bold">Dinner Days</td>'
                            html += '<td class="bold">Total Amount </td>'

                            //for (var i = 0; i < DateList.length; i++) {
                            //    html += '<td class="bold">' + DateList[i].Dates + '</td>'
                            //}

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
                                html += '<td style="text-align:left">' + data.DEPT_NAME + '</td>'
                                html += '<td style="text-align:left">' + data.SUBDEPT_NAME + '</td>'
                                html += '<td style="text-align:left">' + Number(data.LunchDays)/105 + '</td>'
                                html += '<td style="text-align:left">' + data.DinnerDays + '</td>'
                                var Total = Number(data.LunchDays) + Number(data.DinnerDays)*95
                                html += '<td style="text-align:left">' + Total + '</td>'
                               
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



