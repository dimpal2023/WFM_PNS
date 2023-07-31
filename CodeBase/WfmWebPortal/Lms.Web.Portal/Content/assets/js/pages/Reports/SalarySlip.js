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
    $("#particalDiv").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMPLOYMENT_TYPE = $('#EMPLOYMENT_TYPE').val();
    var MONTH_ID = $('#MONTH_ID').val();
    var YEAR_ID = $('#YEAR_ID').val();

    if (BUILDING_ID == "") {
        toastr["error"]('Please Select Unit.');
        return;
    } else if (WF_EMP_TYPE == "") {
        toastr["error"]('Please Select Workforce Type.');
        return;
    } else if (EMPLOYMENT_TYPE == "") {
        toastr["error"]('Please Select Employment Type.');
        return;
    } else if (MONTH_ID == "") {
        toastr["error"]('Please Select Month.');
        return;
    } else if (YEAR_ID == "") {
        toastr["error"]('Please Select Year.');
        return;
    }

    if (BUILDING_ID == "" || WF_EMP_TYPE == "" || MONTH_ID == "" || YEAR_ID == "" || EMPLOYMENT_TYPE == "") {
        alert('All fields are mendatory.');
        return;
    }
    $.ajax(
        {
            type: 'Get',
            url: '/Reports/Get_SalarySlip?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&MONTH_ID=' + MONTH_ID + '&YEAR_ID=' + YEAR_ID + '&EMPLOYMENT_TYPE=' + EMPLOYMENT_TYPE,
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
                        for (var i = 0; i < Cols.length; i++) {
                            var data = Cols[i];
                            var MonthYear = $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text();
                            
                            html += '<table class="display table table-bordered table-checkable order-column m-t-20 width-per-100 table2excel" id=' + data.EMP_ID + '_' + data.EMP_NAME + ' data-SheetName=' + data.EMP_ID + '_' + data.EMP_NAME + '><thead>';

                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important; font-size:30px;">'
                            html += '<th class="bold" colspan="27" style="text-align: center;">' + result.COMPANY_NAME + '</th>'
                            html += '</tr>'
                            html += '<tr style="background: #29c1c9;color:white;font-weight:400!important;font-size:21px;">'
                            html += '<th class="bold" colspan="27" style="text-align: center; font-size: 1.7rem;">' + result.ADDRESS1 + '</th>'
                            html += '</tr>'
                            html += '<tr style="background: #29c1c9; color: white; font-weight: 400 !important;;font-size:21px;">';
                            html += '<th class="bold" colspan="27" style="text-align: center; font-size: 1.7rem;">SALARY SLIP FOR THE MONTH OF ' + MonthYear + '</th>';
                            html += '</tr>';
                            html += '<tr  style="border: 1px solid black;font-weight:bold;font-size:16px">'
                          
                            html += '<td rowspan="2" class="bold">Name</td>'
                            html += '<td rowspan="2" class="bold">Employee Code</td>'
                           
                            html += '<td rowspan="2" class="bold">Department</td>'
                            html += '<td rowspan="2" class="bold">Sub Department</td>'
                            html += '<td rowspan="2" class="bold">ESI A/C No.</td>'
                            html += '<td rowspan="2" class="bold">UAN No.</td>'
                            html += '<td colspan="4" class="bold">WAGES  RATE</td>'
                            html += '<td rowspan="2" class="bold">Paid Days</td>'
                            
                            html += '<td colspan="5" class="bold">Earned Wage</td>'
                            html += '<td colspan="5" class="bold">Deduction</td>'
                            html += '<td rowspan="2" class="bold">Actual Wages Paid</td>'
                            html += '<td colspan="3" class="bold">Employer Contribution</td >'
                            html += '<td rowspan="2" class="bold">Total Leave Taken ' + MonthYear + '</td>'
                            html += '<td rowspan="2" class="bold">Total Leave balance ' + MonthYear + '</td>'
                            html += '</tr>';

                            html += '<tr  style="border: 1px solid black;font-weight:bold;font-size:16px">'
                            html += '<td class="bold">Basic + DA</td>'
                            html += '<td class="bold">HRA</td>'
                            html += '<td class="bold">Spl Allow</td>'
                            html += '<td class="bold">Actual Gross</td>'

                            html += '<td class="bold">Basic + DA</td>'
                            html += '<td class="bold">HRA</td>'
                            html += '<td class="bold">Spl Allow</td>'
                            html += '<td class="bold">Production Incentive Bonus</td>'
                            html += '<td class="bold">Gross Salary</td>'
                            html += '<td class="bold">PF</td>'
                            html += '<td class="bold">ESI</td>'
                            html += '<td class="bold">TDS</td>'
                            html += '<td class="bold">Fine</td>'
                            html += '<td class="bold">Advance</td>'
                            html += '<td class="bold">EPF</td>'
                            html += '<td class="bold">ESI</td>'
                            html += '<td class="bold">Admin Charges</td>'
                         
                            html += '</tr>'

                            html += '</thead>'
                            html += '<tbody>'
                          
                            html += '<tr style="border: 1px solid black;">'
                          
                            html += '<td>' + data.EMP_NAME + '</td>'
                            html += '<td>' + data.EMP_ID + '</td>'
                           
                            html += '<td>' + data.Department + '</td>'
                            html += '<td>' + data.SubDepartment + '</td>'
                            html += '<td>' + data.ESIC_NO + '</td>'
                            html += '<td>' + data.UAN_NO + '</td>'
                          
                            //html += '<td>' + data.GROSS + '</td>'
                            html += '<td>' + data.BASIC_DA + '</td>'
                            html += '<td>' + data.HRA + '</td>'
                            html += '<td>' + data.spclAllow + '</td>'

                            var Total = Number(data.Total);
                          

                            html += '<td>' + Total + '</td>'
                           
                            html += '<td>' + data.PaidDay + '</td>'
                            

                          
                            html += '<td>' + data.EW_BasicDA + '</td>'
                            html += '<td>' + data.EW_HRA + '</td>'
                            html += '<td>' + data.EW_SplAllow + '</td>'

                            html += '<td>' + data.PRODUCTION_INCENTIVE_BONUS + '</td>'

                            html += '<td>' + Number(data.DeductionTotal2) + '</td>'

                            html += '<td>' + data.PF + '</td>'
                            html += '<td>' + data.ESI + '</td>'
                            html += '<td>' + data.TDS + '</td>'
                           if (data.OtherAllowance == 0) {
                                html += '<td>0</td>'
                            } else {
                                var Amt = String(data.OtherAllowance)[0];
                                if (Amt[0] == '-') {
                                    html += '<td>' + data.OtherAllowance.toString().substring(1) + '</td>'
                                   
                                } else {
                                    html += '<td>0</td>'
                                }
                            }
                           
                            html += '<td>' + data.ADVANCE + '</td>'
                            html += '<td>' + data.ActualWagesPaid + '</td>'
                            html += '<td>' + data.EmpEPF + '</td>'
                            html += '<td>' + data.EmpESI + '</td>'
                            html += '<td>' + data.AdminCharges + '</td>'
                            html += '<td>' + data.TotalLeaveTakenMonthWise + '</td>'
                            html += '<td>' + data.EW_TotalLeavebalanceMonth + '</td>'
                            html += '</tr>'
                            html += '</tbody >'
                            html += '<tfoot>'
                            html += '<tr><td colspan="27"></td></tr>'
                            html += '<tr><td colspan="5" style="font-weight:bold;font-size:17px;">FORM X RULE-26 (2) WAGES SLIP</td><td colspan="22"></td></tr>'
                            html += '<tr><td colspan="5" style="font-weight:bold;font-size:17px">Establishment Code :</td><td colspan="22"></td></tr>'
                            html += '<tr><td colspan="5" style="font-weight:bold;font-size:17px">UP/LKO/2100404000</td><td colspan="22"></td></tr>'
                            html += '<tr><td colspan="5"><img src="/Content/assets/images/pages/Signature.png" alt="" style="width:70px;height:60px" /></td><td colspan="22" style="text-align:center;font-weight:bold;font-size:17px">SIGNATURE OF EMPLOYEE</td></tr>'
                            html += '</tfoot>'
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

                            var txt = dd + '_' + $("#MONTH_ID option:selected").text() + ' ' + $("#YEAR_ID option:selected").text();

                        }
                        

                        Idname = Idname.replace(/,\s*$/, "");
                        if (images.length > 0) {
                            tablesToExcel(Idname, 'WorkSheet.xls');
                        }
                    });
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

