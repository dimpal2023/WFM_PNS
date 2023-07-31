$(document).ready(function () {
    $("select[required]").css({ position: "absolute", display: "inline", height: 0, padding: 0, width: 0 });
    $('#DEPT_ID').find('option').not(':first').remove();
    var ddl = $("#EMPLOYMENT_TYPE option").html();
    ddl += '<option value="3">Both</option>';
    $("#EMPLOYMENT_TYPE").append(ddl);
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


function GetReport() {
    debugger
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = 2;
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (WF_EMP_TYPE == "") {
        toastr["error"]('Please Select Employee Type.');
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
            url: '/Reports/Get_MasterSalary?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                        //var AtList = result.AttendanceSheet;
                        var MonthYear = $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text();
                        html += '<div class="table-responsive">';
                        html += '<table id="tableExports" class="display table table-bordered table-checkable order-column m-t-20 width-per-100" width="100%"> <thead>';

                        html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                        html += '<th class="bold" colspan="50" style="text-align: center; font-size: 1.7rem; ">' + result.COMPANY_NAME + '</th>'
                        html += '</tr>'
                        html += '<tr style="background: #29c1c9;color:white;font-weight:400!important">'
                        html += '<th class="bold" colspan="50" style="text-align: center; font-size: 1.7rem; ">' + result.ADDRESS1 + '</th>'
                        html += '</tr>'
                        html += '<tr style="background: #29c1c9; color: white; font-weight: 400 !important;">';
                        html += '<th class="bold" colspan="50" style="text-align: center; font-size: 1.7rem; ">WORKERS SALARY SHEET FOR THE MONTH OF ' + MonthYear + '</th>';
                        html += '</tr>';
                        html += '<tr  style="border: 1px solid black;">'
                        html += '<td rowspan="2" class="bold">S.No.</td>'
                        html += '<td rowspan="2" class="bold">Name</td>'
                        html += '<td rowspan="2" class="bold">Employee Code</td>'
                       
                        html += '<td rowspan="2" class="bold">Department</td>'
                        html += '<td rowspan="2" class="bold">Sub Department</td>'
                        html += '<td rowspan="2" class="bold">NATURE OF EMPLOYMENT</td>'
                        html += '<td rowspan="2" class="bold">Actual Gross Salary</td>'
                        
                        html += '<td rowspan="2" class="bold">PR Payable Amount</td>'
                        html += '<td rowspan="2" class="bold">Paid Days</td>'

                        
                        /*html += '<td rowspan="2" class="bold">OT Hours</td>'*/
                        html += '<td rowspan="2" class="bold">Total Leave Taken ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Leave Carry forwarded from Previous Month</td>'
                        html += '<td rowspan="2" class="bold">Leave Allowed</td>'
                        html += '<td rowspan="2" class="bold">Total Available Leave till ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Leave Adjusted in ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Total Leave balance ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Leave Amount</td>'
                        html += '<td class="bold">Dinner Allowance</td>'
                        html += '<td class="bold">Lunch Allowance</td>'
                        html += '<td class="bold">Total</td>'
                        html += '<td rowspan="2" class="bold">Actual Wages Paid</td>'


                       
                        
                        html += '</tr>'

                        html += '</thead>'
                        html += '<tbody>'
                        var T_AGrossSalary = 0, T_WRBasicDA = 0, T_WRHRA = 0, T_WRSpclAllow = 0, T_Total = 0, T_ASalaryPDay = 0;
                        var T_ASalPHour = 0, T_ExtraEarnSal = 0, T_PaybleAmt = 0, T_OTHours = 0, T_TLeaveTaken = 0, T_CarryForward = 0, T_LeaveAllowed = 0;
                        var T_AvailLeave = 0, T_AdjustLeave = 0, T_BalanceLeave = 0, T_EBasicDA = 0, T_EHRA = 0, T_ESpclAllow = 0, T_ProductINCBonus = 0;
                        var T_ShopFloorFine = 0, T_PF = 0, T_ESI = 0, T_TDS = 0, T_ActualWagesPaid = 0;
                        for (var i = 0; i < Cols.length; i++) {
                            var data = Cols[i];
                            html += '<tr style="border: 1px solid black;">'
                            html += '<td>' + (i + 1) + '</td>'
                            html += '<td>' + data.EMP_NAME + '</td>'
                            html += '<td>' + data.EMP_ID + '</td>'
                           
                            html += '<td>' + data.Department + '</td>'
                            html += '<td>' + data.SubDepartment + '</td>'
                            html += '<td>' + data.NEType + '</td>'
                            html += '<td>' + data.GROSS + '</td>'

                            var ActualSalaryPerDay = 0;
                            var ActualsalaryPerHour = 0;
                            ActualSalaryPerDay = Number(data.GROSS) / 26;

                           
                            html += '<td>' + data.PRPayableAmount + '</td>'
                           

                            html += '<td>' + data.PaidDay + '</td>'
                            html += '<td>' + data.TotalLeaveTakenMonthWise + '</td>'
                            html += '<td>' + data.LeaveCarryforwarded + '</td>'
                            html += '<td>' + data.LeaveAllowed + '</td>'
                            html += '<td>' + data.TotalAvailableLeavetillMonth + '</td>'
                            html += '<td>' + data.LeaveAdjustedinMonth + '</td>'
                            html += '<td>' + data.EW_TotalLeavebalanceMonth + '</td>'
                            html += '<td>' + Number(ActualSalaryPerDay * Number(data.LeaveAdjustedinMonth)).toFixed(2) + '</td>'
                            html += '<td>' + data.DinnerAllowance + '</td>'
                            html += '<td>' + data.LunchAllowance + '</td>'
                            html += '<td>' + Number(data.DeductionTotal2) + '</td>'
                            html += '<td>' + data.ActualWagesPaid + '</td>'
                            
                            html += '</tr>'

                            //T_AGrossSalary += Number(data.GROSS);
                            //T_WRBasicDA += Number(data.BASIC_DA);
                            //T_WRHRA += Number(data.HRA);
                            //T_WRSpclAllow += Number(data.spclAllow);
                            //T_Total += Number(Total);
                            //T_ASalaryPDay += Number(ActualSalaryPerDay);
                            //T_ASalPHour += Number(ActualsalaryPerHour);
                            //T_ExtraEarnSal += Number(ExtraEarnSal);

                            //T_PaybleAmt += Number(data.PRPayableAmount);

                            //T_OTHours += Number(data.OTHours);
                            //T_TLeaveTaken += Number(data.TotalLeaveTakenMonthWise);
                            //T_CarryForward += Number(data.LeaveCarryforwarded);
                            //T_LeaveAllowed += Number(data.LeaveAllowed);
                            //T_AvailLeave += Number(data.TotalAvailableLeavetillMonth);
                            //T_AdjustLeave += Number(data.LeaveAdjustedinMonth);
                            //T_BalanceLeave += Number(data.EW_TotalLeavebalanceMonth);
                            //T_EBasicDA += Number(data.EW_BasicDA);
                            //T_EHRA += Number(data.EW_HRA);
                            //T_ESpclAllow += Number(data.EW_SplAllow);
                            //if (EMPLOYMENT_TYPE == 2) {
                            //    T_ProductINCBonus += Number(data.EW_ProductionIncentiveBonus);
                            //} else {
                            //    T_ProductINCBonus += Number(data.PRODUCTION_INCENTIVE_BONUS);
                            //}
                            //T_ShopFloorFine += Number(data.SHOPFLOORFINE);
                            //T_PF += Number(data.PF);
                            //T_ESI += Number(data.ESI);
                            //T_TDS += Number(data.TDS);
                            //T_ActualWagesPaid += Number(data.ActualWagesPaid);
                        }
                        html += '</tbody >'
                        //html += '<tfoot>'
                        //html += '<tr style="border: 1px solid black;">'
                        //html += '<td class="bold" colspan="10">GRAND TOTAL</td>'
                        //html += '<td>' + T_AGrossSalary + '</td>'
                        //html += '<td>' + T_WRBasicDA + '</td>'
                        //html += '<td>' + T_WRHRA + '</td>'
                        //html += '<td>' + T_WRSpclAllow + '</td>'
                        //html += '<td>' + T_Total + '</td>'
                        //html += '<td>' + T_ASalaryPDay.toFixed(0) + '</td>'
                        //html += '<td>' + T_ASalPHour.toFixed(0) + '</td>'
                        //html += '<td>' + T_ExtraEarnSal.toFixed(0) + '</td>'
                        //html += '<td></td>'
                        //html += '<td>' + T_OTHours.toFixed(2) + '</td>'
                        //if (EMPLOYMENT_TYPE == 2) {
                        //    html += '<td>' + T_PaybleAmt.toFixed(0) + '</td>'
                        //} else {
                        //    html += '<td></td>'
                        //}
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td>' + T_TLeaveTaken + '</td>'
                        //html += '<td>' + T_CarryForward + '</td>'
                        //html += '<td>' + T_LeaveAllowed + '</td>'
                        //html += '<td>' + T_AvailLeave + '</td>'
                        //html += '<td>' + T_AdjustLeave + '</td>'
                        //html += '<td>' + T_BalanceLeave + '</td>'
                        //html += '<td>' + T_EBasicDA + '</td>'
                        //html += '<td>' + T_EHRA + '</td>'
                        //html += '<td>' + T_ESpclAllow + '</td>'
                        //html += '<td>' + T_ProductINCBonus + '</td>'
                        //html += '<td>0</td>'
                        //html += '<td>0</td>'
                        //html += '<td>0</td>'
                        //html += '<td>' + T_ShopFloorFine + '</td>'
                        //html += '<td></td>'
                        //html += '<td>' + T_PF + '</td>'
                        //html += '<td>' + T_ESI + '</td>'
                        //html += '<td>' + T_TDS + '</td>'
                        //html += '<td>' + T_ActualWagesPaid + '</td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '<td></td>'
                        //html += '</tr>'
                        //html += '</tfoot>'
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
    });
}

