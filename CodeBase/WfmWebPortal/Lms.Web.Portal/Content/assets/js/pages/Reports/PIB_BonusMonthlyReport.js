const { image } = require("d3-fetch");

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

var Idname = '';
function GetReport() {
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val() || '';
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val() || 0;
    var MONTH_ID = $('#MONTH_ID').val() || 0;
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
            url: '/Reports/GetPIB_BonusMonthlyReport?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                        debugger;
                        var List = $.parseJSON(response);
                        html += '<div class="table-responsive">';
                        for (var k = 0; k < List.length; k++) {
                            var details = List[k];
                            /* html = '';*/
                            var col1 = 0,col2 = 0, row = 0;
                            var TotalSalary = (Number(details.BASIC_DA) + Number(details.SPECIAL_ALLOWANCES) + Number(details.HRA)) / 26;
                            var TotalSal_PerHour = TotalSalary / 8;
                            html += '<table class="display table table-bordered table-checkable order-column m-t-20 width-per-100 table2excel" width="100%" id=' + details.EMP_ID + '_' + details.EMP_NAME + ' data-SheetName=' + details.EMP_ID + '_' + details.EMP_NAME + '>';
                            html += '<tr  style="background: #29c1c9;color:white;font-weight:400!important">';
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                col1 = 7;
                                col2 = 7;
                                row = 14;
                            } else {
                                col1 = 4;
                                col2 = 5;
                                row = 9;
                            }
                            html += '<th class="bold" colspan='+row+' style="text-align: center; font-size: 1.7rem; ">Production Incentive Bonus Monthly Report</th>';

                            html += '</tr>';
                            html += '<tr>'
                            html += '<th class="bold" colspan=' + col1 + ' style="font-size: 1rem; "> Worker Name : ' + details.EMP_NAME + '</th>'
                            html += '<th class="bold" colspan=' + col2 + ' style="font-size: 1rem; "> Department : ' + details.DEPT_NAME + '</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th class="bold" colspan=' + col1 + ' style="font-size: 1rem; ">Sub Department : ' + details.SUBDEPT_NAME + '</th>'
                            html += '<th class="bold" colspan=' + col2 + ' style="font-size: 1rem; "> Employee Code : ' + details.EMP_ID + '</th>'
                            html += '</tr>'
                            html += '<tr >'

                            html += '<th class="bold" colspan=' + col1 + ' style="font-size: 1rem; "> Month : ' + details.MonthN + '</th>'
                            html += '<th class="bold" colspan=' + col2 + ' style="font-size: 1rem; "> Year : ' + details.YearID + '</th>'
                            html += '</tr>'
                            html += '<tr  style="border: 1px solid black;">'
                            html += '<td class="bold">Date</td>'
                            html += '<td class="bold">Item Code</td>'
                            html += '<td class="bold">Item Name</td>'
                            html += '<td class="bold">Operation Name</td>'
                            html += '<td class="bold">Quantity</td>'
                            html += '<td class="bold">Piece Rate</td>'
                            html += '<td class="bold">Total Piece <br/> Rate Amount</td>'
                            html += '<td class="bold">Remark</td>'
                            html += '<td class="bold">Total Hours</td>';
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td class="bold">Machine No.</td>';
                                html += '<td class="bold">Average Percentage</td>';
                                html += '<td class="bold">WASTE</td>';
                                html += '<td class="bold">REJECTION ON LOOM</td>';
                                html += '<td class="bold">REJECTION ON FINISHING</td>';
                            }
                            html += '</tr>';
                            html += '</thead>'
                            html += '<tbody>'
                            var totalprice = 0; var DateWiseTotal = 0;
                            var flagDate = 0;
                            for (var i = 0; i < details.items.length; i++) {

                                var data = details.items[i];
                                debugger;
                                if (flagDate != data.WORK_DATE && i > 0) {
                                    html += '<tr style="background-color: aliceblue; font-weight: bold;">'
                                    html += '<td></td>'
                                    html += '<td>Total</td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td  style="text-align:right">' + DateWiseTotal.toFixed(3) + '</td>'
                                    html += '<td></td>'
                                    html += '<td>' + conversion(Number(details.items[i - 1].Duration)) + '</td>'
                                    if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                    }
                                    html += '</tr>'
                                    DateWiseTotal += data.TotalPrice;
                                }

                                html += '<tr style="border: 1px solid black;">'
                                html += '<td style="text-align:center">' + data.WORK_DATE + '</td>'
                                html += '<td style="text-align:left">' + data.ITEM_CODE + '</td>'
                                html += '<td style="text-align:left">' + data.ITEM_NAME + '</td>'
                                html += '<td style="text-align:left">' + data.Operation + '</td>'
                                html += '<td style="text-align:center">' + data.QTY + '</td>'
                                html += '<td style="text-align:right">' + data.RATE.toFixed(2) + '</td>'
                                html += '<td style="text-align:right">' + data.TotalPrice.toFixed(3) + '</td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                    html += '<td  style="text-align:center">' + data.MachineNo + '</td>'
                                    html += '<td  style="text-align:center">' + data.AvgPercentage + '</td>'
                                    html += '<td  style="text-align:center">' + data.WASTE + '</td>'
                                    html += '<td  style="text-align:center">' + data.REJECTION_ON_LOOM + '</td>'
                                    html += '<td  style="text-align:center">' + data.REJECTION_ON_FINISHING + '</td>'
                                }
                                html += '</tr>'

                                totalprice += Number(data.TotalPrice);
                                if (flagDate != data.WORK_DATE) {
                                    flagDate = data.WORK_DATE;
                                    DateWiseTotal = data.TotalPrice;
                                } else {
                                    DateWiseTotal += data.TotalPrice;
                                }
                                if ((i + 1) == details.items.length) {
                                    html += '<tr style="background-color: aliceblue; font-weight: bold;">'
                                    html += '<td></td>'
                                    html += '<td>Total</td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td></td>'
                                    html += '<td style="text-align:right">' + DateWiseTotal.toFixed(3) + '</td>'
                                    html += '<td></td>'
                                    html += '<td>' + conversion(Number(details.items[i].Duration)) + '</td>'
                                    if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                        html += '<td></td>'
                                    }
                                    html += '</tr>'
                                    DateWiseTotal += data.TotalPrice;
                                }
                            }
                            html += '<tr>'
                            html += '<td></td>'
                            html += '<td></td>'
                            html += '<td></td>'
                            html += '<td></td>'
                            html += '<td></td>'
                            html += '<td class="bold" style="text-align:right">TOTAL VALUE </td>'
                            html += '<td class="bold" style="text-align:right">' + totalprice.toFixed(3) + '</td>'
                            html += '<td class="bold"></td>'
                            html += '<td class="bold"></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'

                            html += '<tr>'
                            html += '<td colspan="6" style="text-align:right" class="bold">PRODUCTION INCENTIVE BONUS CALCULATION</td>'
                            html += '<td></td>'
                            html += '<td></td>'
                            html += '<td></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'

                            html += '<tr>'
                            html += '<td colspan="6" class="bold"  style="text-align:right;font-size: 1rem;">Total piece Rate Value</td>'
                            html += '<td class="bold">' + totalprice.toFixed(3) + '</td>'
                            html += '<td class="bold"></td>'
                            html += '<td class="bold"></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'

                            html += '<tr>'
                            html += '<td colspan="6" class="bold"  style="text-align:right;font-size: 1rem;">Basic Salary</td>'
                            html += '<td class="bold">' + details.BASIC_DA.toFixed(2) + '</td>'
                            html += '<td class="bold"></td>'
                            html += '<td class="bold"></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'

                            html += '<tr>'
                            html += '<td colspan="6" class="bold" style="text-align:right;font-size: 1rem;">Special Allowance</td>'
                            html += '<td class="bold">' + details.SPECIAL_ALLOWANCES.toFixed(2) + '</td>'
                            html += '<td class="bold"></td>'
                            html += '<td class="bold"></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'

                            html += '<tr>'
                            html += '<td colspan="6" class="bold" style="text-align:right;font-size: 1rem;">Production Incentive Bonus</td>'
                            html += '<td class="bold">0</td>'
                            html += '<td class="bold"></td>'
                            html += '<td class="bold"></td>'
                            if ($("#BUILDING_ID option:selected").val().toLowerCase() == "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef") {
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                                html += '<td></td>'
                            }
                            html += '</tr>'


                            html += '</tbody >'
                            html += '</table>'


                        }
                        html += '</div>'

                    } else {

                        html += '<div class="text-center"><span>No data found.</span></div>';
                    }
                    $("#particalDiv").append(html + '<br>');

                    Idname = '';

                    $("#particalDiv").each(function () {
                        var images = $(this).find(".table2excel");

                        for (var m = 0; m < images.length; m++) {
                            if (m == images.length) {
                                Idname += '#' + images[m].id;
                            } else {
                                Idname += '#' + images[m].id + ',';
                            }

                            var dd = images[m].id;

                            var txt =  $("#MONTH_ID option:selected").text() + '_' + $("#YEAR_ID option:selected").text();

                        }

                        Idname = Idname.replace(/,\s*$/, "");
                        if (images.length > 0) {
                            debugger;
                           tablesToExcel(Idname, txt+'_PIBBonusMonthlyReport.xls');
                        }

                    });


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


