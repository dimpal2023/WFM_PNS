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
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val() || 3;
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val();
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (EMPLOYMENT_TYPE == "") {
        toastr["error"]('Please Select Employment Type.');
        return;
    }else if (MONTH_ID == "") {
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
                        html += '<td rowspan="2" class="bold">For Supplier Entry</td>'
                        html += '<td rowspan="2" class="bold">Supplier Site</td>'
                        html += '<td rowspan="2" class="bold">Department</td>'
                        html += '<td rowspan="2" class="bold">Sub Department</td>'
                        html += '<td rowspan="2" class="bold">ESI A/C No.</td>'
                        html += '<td rowspan="2" class="bold">UAN No.</td>'
                        html += '<td rowspan="2" class="bold" style="text-align: center;">Bank A/C No.</td>'
                        html += '<td rowspan="2" class="bold">Actual Gross Salary</td>'
                        html += '<td colspan="3" class="bold">WAGES  RATE</td>'
                        html += '<td rowspan="2" class="bold">Total</td>'
                        html += '<td rowspan="2" class="bold">Actual Salary Per Day</td>'
                        html += '<td rowspan="2" class="bold">Actual salary Per Hour</td>'
                        html += '<td rowspan="2" class="bold">Extra Earn Salary</td>'
                        html += '<td rowspan="2" class="bold">Payable Days</td>'
                        html += '<td rowspan="2" class="bold">OT Hours</td>'
                        html += '<td rowspan="2" class="bold">PR Payable Amount</td>'
                        html += '<td rowspan="2" class="bold">Paid Days</td>'

                        html += '<td rowspan="2" class="bold">Permission</td>'

                        html += '<td rowspan="2" class="bold">Actual Present days for PR Workers</td>'
                        html += '<td rowspan="2" class="bold">NATURE OF EMPLOYMENT</td>'
                        /*html += '<td rowspan="2" class="bold">OT Hours</td>'*/
                        html += '<td rowspan="2" class="bold">Total Leave Taken ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Leave Carry forwarded from Previous Month</td>'
                        html += '<td rowspan="2" class="bold">Leave Allowed</td>'
                        html += '<td rowspan="2" class="bold">Total Available Leave till ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Leave Adjusted in ' + MonthYear + '</td>'
                        html += '<td rowspan="2" class="bold">Total Leave balance ' + MonthYear + '</td>'
                        html += '<td colspan="9" class="bold">Earned Wage</td>'
                        html += '<td colspan="3" class="bold">Deduction</td>'
                        html += '<td rowspan="2" class="bold">Actual Wages Paid</td>'
                        html += '<td colspan="5" class="bold">Employer Contribution</td >'
                        html += '<td rowspan="2" class="bold">Payment Mode</td>'
                        html += '</tr>';

                        html += '<tr  style="border: 1px solid black;">'
                        html += '<td class="bold">Basic + DA</td>'
                        html += '<td class="bold">HRA</td>'
                        html += '<td class="bold">Spl Allow</td>'
                      /*  html += '<td class="bold">Total Leave balance ' + MonthYear + '</td>'*/
                        html += '<td class="bold">Basic + DA</td>'
                        html += '<td class="bold">HRA</td>'
                        html += '<td class="bold">Spl Allow</td>'
                        html += '<td class="bold">Production Incentive Bonus</td>'
                        html += '<td class="bold">Other Allowance</td>'
                        html += '<td class="bold">Other Allowance Type</td>'
                        html += '<td class="bold">Dinner Allowance</td>'
                        html += '<td class="bold">Lunch Allowance</td>'
                        /*html += '<td class="bold">Shop Floor Fine</td>'*/
                        html += '<td class="bold">Total</td>'
                        html += '<td class="bold">PF</td>'
                        html += '<td class="bold">ESI</td>'
                        html += '<td class="bold">TDS</td>'
                        html += '<td class="bold">Advance</td>'
                        html += '<td class="bold">EPF</td>'
                        html += '<td class="bold">ESI</td>'
                        html += '<td class="bold">Admin Charges</td>'
                        html += '<td class="bold">EDLI Charges</td>'
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
                            html += '<td>' + data.ForSupplierEntry + '</td>'
                            html += '<td>' + data.SupplierSite + '</td>'
                            html += '<td>' + data.Department + '</td>'
                            html += '<td>' + data.SubDepartment + '</td>'
                            html += '<td>' + data.ESIC_NO + '</td>'
                            html += '<td>' + data.UAN_NO + '</td>'
                            html += '<td>' + data.AccountNo + '</td>'
                            html += '<td>' + data.GROSS + '</td>'
                            html += '<td>' + data.BASIC_DA + '</td>'
                            html += '<td>' + data.HRA + '</td>'
                            html += '<td>' + data.spclAllow + '</td>'

                            var Total = Number(data.Total);
                            var ActualSalaryPerDay = 0;
                            var ActualsalaryPerHour = 0;

                            html += '<td>' + Total + '</td>'
                           /* if (EMPLOYMENT_TYPE == 2) {*/
                                ActualSalaryPerDay = Number(data.GROSS) / 26;
                                ActualsalaryPerHour = Number(ActualSalaryPerDay) / 8;

                                html += '<td>' + ActualSalaryPerDay.toFixed(2) + '</td>'
                                html += '<td>' + ActualsalaryPerHour.toFixed(2) + '</td>'
                            //} else {
                            //    html += '<td>0</td>'
                            //    html += '<td>0</td>'
                            //}


                            var ExtraEarnSal = 0, CountSalary = 0;


                            if (EMPLOYMENT_TYPE == 2) {
                                /*CountSalary = Number(data.EW_BasicDA) + Number(data.EW_HRA) + Number(data.EW_SplAllow);*/
                                CountSalary = Number(data.BASIC_DA) + Number(data.HRA) + Number(data.spclAllow);
                                ExtraEarnSal = Number(data.ExtraEarnSalary) - CountSalary;
                                if (ExtraEarnSal <= 0) {
                                    ExtraEarnSal = 0;
                                }
                                html += '<td>' + ExtraEarnSal.toFixed(0) + '</td>'
                                html += '<td>' + data.PaidDay + '</td>'
                            } else {
                                html += '<td>0</td>'
                                html += '<td>0</td>'
                            }

                            html += '<td>' + data.OTHours + '</td>'
                            if (EMPLOYMENT_TYPE == 2) {
                                html += '<td>' + data.PRPayableAmount + '</td>'
                            } else {
                                html += '<td>SALARIED</td>';
                            }

                            html += '<td>' + data.PaidDay + '</td>'
                             html += '<td>' + data.Permission + '</td>'
                            if (EMPLOYMENT_TYPE == 2) {
                                html += '<td>' + data.PaidDays + '</td>'
                            } else {
                                html += '<td>0</td>'
                            }

                           
                            html += '<td>' + data.NEType + '</td>'
                            html += '<td>' + data.TotalLeaveTakenMonthWise + '</td>'
                            html += '<td>' + data.LeaveCarryforwarded + '</td>'
                            html += '<td>' + data.LeaveAllowed + '</td>'
                            html += '<td>' + data.TotalAvailableLeavetillMonth + '</td>'
                            html += '<td>' + data.LeaveAdjustedinMonth + '</td>'
                            html += '<td>' + data.EW_TotalLeavebalanceMonth + '</td>'
                            html += '<td>' + data.EW_BasicDA + '</td>'
                            html += '<td>' + data.EW_HRA + '</td>'
                            html += '<td>' + data.EW_SplAllow + '</td>'

                            //if (EMPLOYMENT_TYPE == 2) {
                            //    html += '<td>' + ExtraEarnSal.toFixed(0) + '</td>'
                            //} else {
                                html += '<td>' + data.PRODUCTION_INCENTIVE_BONUS + '</td>'
                            //}

                            if (data.OtherAllowance == 0) {
                                html += '<td>' + data.OtherAllowance + '</td>'
                                html += '<td>-</td>'
                            } else {
                                var Amt = String(data.OtherAllowance)[0];
                                if (Amt[0] == '-') {
                                    html += '<td>' + data.OtherAllowance.toString().substring(1) + '</td>'
                                    html += '<td>Deduction</td>'
                                } else {
                                    html += '<td>' + data.OtherAllowance + '</td>'
                                    html += '<td>Addition</td>'
                                }
                            }
                            html += '<td>' + data.DinnerAllowance + '</td>'
                            html += '<td>' + data.LunchAllowance + '</td>'
                            /*html += '<td>' + data.SHOPFLOORFINE + '</td>'*/
                            debugger
                            //if (EMPLOYMENT_TYPE == 2) {
                            html += '<td>' + Number(data.DeductionTotal2)+ '</td>'
                            //} else {
                            //    html += '<td>' + data.DeductionTotal1 + '</td>'
                            //}
                            html += '<td>' + data.PF + '</td>'
                            html += '<td>' + data.ESI + '</td>'
                            html += '<td>' + data.TDS + '</td>'
                            html += '<td>' + data.ActualWagesPaid + '</td>'
                            html += '<td>' + data.ADVANCE + '</td>'
                            html += '<td>' + data.EmpEPF + '</td>'
                            html += '<td>' + data.EmpESI + '</td>'
                            html += '<td>' + data.AdminCharges + '</td>'
                            html += '<td>' + data.EDLICharges + '</td>'
                            html += '<td>Bank Transfer</td>'
                            html += '</tr>'

                            T_AGrossSalary += Number(data.GROSS);
                            T_WRBasicDA += Number(data.BASIC_DA);
                            T_WRHRA += Number(data.HRA);
                            T_WRSpclAllow += Number(data.spclAllow);
                            T_Total += Number(Total);
                            T_ASalaryPDay += Number(ActualSalaryPerDay);
                            T_ASalPHour += Number(ActualsalaryPerHour);
                            T_ExtraEarnSal += Number(ExtraEarnSal);

                            T_PaybleAmt += Number(data.PRPayableAmount);

                            T_OTHours += Number(data.OTHours);
                            T_TLeaveTaken += Number(data.TotalLeaveTakenMonthWise);
                            T_CarryForward += Number(data.LeaveCarryforwarded);
                            T_LeaveAllowed += Number(data.LeaveAllowed);
                            T_AvailLeave += Number(data.TotalAvailableLeavetillMonth);
                            T_AdjustLeave += Number(data.LeaveAdjustedinMonth);
                            T_BalanceLeave += Number(data.EW_TotalLeavebalanceMonth);
                            T_EBasicDA += Number(data.EW_BasicDA);
                            T_EHRA += Number(data.EW_HRA);
                            T_ESpclAllow += Number(data.EW_SplAllow);
                            if (EMPLOYMENT_TYPE == 2) {
                                T_ProductINCBonus += Number(data.EW_ProductionIncentiveBonus);
                            } else {
                                T_ProductINCBonus += Number(data.PRODUCTION_INCENTIVE_BONUS);
                            }
                            T_ShopFloorFine += Number(data.SHOPFLOORFINE);
                            T_PF += Number(data.PF);
                            T_ESI += Number(data.ESI);
                            T_TDS += Number(data.TDS);
                            T_ActualWagesPaid += Number(data.ActualWagesPaid);
                        }
                        html += '</tbody >'
                        html += '<tfoot>'
                        html += '<tr style="border: 1px solid black;">'
                        html += '<td class="bold" colspan="10">GRAND TOTAL</td>'
                        html += '<td>' + T_AGrossSalary + '</td>'
                        html += '<td>' + T_WRBasicDA + '</td>'
                        html += '<td>' + T_WRHRA + '</td>'
                        html += '<td>' + T_WRSpclAllow + '</td>'
                        html += '<td>' + T_Total + '</td>'
                        html += '<td>' + T_ASalaryPDay.toFixed(0) + '</td>'
                        html += '<td>' + T_ASalPHour.toFixed(0) + '</td>'
                        html += '<td>' + T_ExtraEarnSal.toFixed(0) + '</td>'
                        html += '<td></td>'
                        html += '<td>' + T_OTHours.toFixed(2) + '</td>'
                        if (EMPLOYMENT_TYPE == 2) {
                            html += '<td>' + T_PaybleAmt.toFixed(0) + '</td>'
                        } else {
                            html += '<td></td>'
                        }
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td>' + T_TLeaveTaken + '</td>'
                        html += '<td>' + T_CarryForward + '</td>'
                        html += '<td>' + T_LeaveAllowed + '</td>'
                        html += '<td>' + T_AvailLeave + '</td>'
                        html += '<td>' + T_AdjustLeave + '</td>'
                        html += '<td>' + T_BalanceLeave + '</td>'
                        html += '<td>' + T_EBasicDA + '</td>'
                        html += '<td>' + T_EHRA + '</td>'
                        html += '<td>' + T_ESpclAllow + '</td>'
                        html += '<td>' + T_ProductINCBonus + '</td>'
                        html += '<td>0</td>'
                        html += '<td>0</td>'
                        html += '<td>0</td>'
                        html += '<td>' + T_ShopFloorFine + '</td>'
                        html += '<td></td>'
                        html += '<td>' + T_PF + '</td>'
                        html += '<td>' + T_ESI + '</td>'
                        html += '<td>' + T_TDS + '</td>'
                        html += '<td>' + T_ActualWagesPaid + '</td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '<td></td>'
                        html += '</tr>'
                        html += '</tfoot>'
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

