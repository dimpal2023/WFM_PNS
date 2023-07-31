function departmentonChange() {
    $('#emplyeedetail').empty();
}

function employeeOnChange() {
    var emp_id = $('#WF_ID').val();
    if (emp_id === "") {
        $('#emplyeedetail').empty();
        $('#rowValue').val(0);
    } else {
        $.get('/ExitManagement/GetEmployeeAssetById?emp_id=' + emp_id, function (data) {
            $('#emplyeedetail').html(data);
            $('#emplyeedetail').fadeIn('fast');
            $('.dateddmmyyyy').bootstrapMaterialDatePicker({
                format: 'DD/MM/YYYY',
                clearButton: true,
                weekStart: 1,
                time: false,
                autoClose: true
            }).on('change', function (e, date) {
                getByResignationDateExitDate(date._d);
            });
            $('#NOTICE_DAYS').on("keyup", getByNoticeDayExitDate);

        });
    }
}

function exitEmployeeOnChange() {
    var emp_id = $('#WF_ID').val();
    if (emp_id === "00000000-0000-0000-0000-000000000000") {
        $('#emplyeedetail').empty();
    } else {
        $.get('/ExitManagement/EmployeesExitApprovalDetails?emp_id=' + emp_id, function (data) {
            $('#emplyeedetail').html(data);
            $('#emplyeedetail').fadeIn('fast');
        });
    }
}

function getEmployeeOnLoad() {
    $('#emplyeedetail').empty();
    exitEmployeeOnChange();
}

function GetAll() {
    $("#tabledata tbody").empty();
    var BUILDING_ID = $('#BUILDING_ID').val();
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var WF_EMP_TYPE = $('#WF_EMP_TYPE').val();
    var EMP_NAME = "";
    if ($('#EMP_NAME').val() != "") {
        EMP_NAME = $('#WF_ID').val() ;
    }
    
    if (BUILDING_ID == "") {
        alert('Please select Unit.');
        return;
    }
    //else if (DEPT_ID == "") {
    //    alert('Please select Department.');
    //    return;
    //}
    $.ajax(
        {
            type: 'Get',
            url: '/ExitManagement/GetAllAssetAllocation_Details?BUILDING_ID=' + BUILDING_ID + '&DEPT_ID=' + DEPT_ID + '&SUBDEPT_ID=' + SUBDEPT_ID + '&WF_EMP_TYPE=' + WF_EMP_TYPE + '&EMP_NAME=' + EMP_NAME,
            beforeSend: function () {
                $('.page-loader-wrapper').show();
            },
            complete: function () {
                $('.page-loader-wrapper').hide();
            },
            success:
                function (response) {
                    debugger
                    if (response == "") {
                        alert('No data found.');
                        return;
                    }
                    var result = $.parseJSON(response);
                    var html = '';
                    if (result.length == 0) {
                        alert('No data found.');
                        return;
                    }
                    for (var i = 0; i < result.length; i++) {
                        html += '<tr>';
                        html += '<td>' + Number(i + 1) + '</td>';

                        html += '<td>' + result[i].EMP_ID + '</td>';
                        html += '<td>' + result[i].EMP_NAME + '</td>';
                        html += '<td>' + result[i].MOBILE_NO + '</td>';
                        html += '<td>' + result[i].WF_Type + '</td>';
                        html += '<td>' + result[i].EMP_TYPE + '</td>';

                        html += '<td>' + result[i].BUILDING_NAME +'</td>';
                        html += '<td>' + result[i].DEPT_NAME + '</td>';
                        html += '<td>' + result[i].SUBDEPT_NAME + '</td>';

                        html += '<td>' + result[i].ASSET_NAME + '</td>';
                        html += '<td>' + result[i].ASSET_LIFE + '</td>';
                        html += '<td>' + result[i].QUANTITY + '</td>';
                        html += '<td>' + result[i].ASSET_ASSIGN_DATE + '</td>';
                        html += '<td>' + result[i].PURPOSE + '</td>';
                        html += '<td>' + result[i].ASSET_TYPE + '</td>';
                        html += '<td>' + result[i].IsReturn + '</td>';
                        html += '<td>' + result[i].ASSET_ASSIGN_BY + '</td>';
                        html += '</tr>';
                    }
                    $("#tabledata tbody").append(html);
                    var contents = $("#particalDiv").html();
                    var result = 'data:application/vnd.ms-excel,' + encodeURIComponent(contents);
                    var link = document.createElement("a");
                    document.body.appendChild(link);
                    link.download = "AssetAllocationList.xls";
                    link.href = result;
                    link.click();
                },
            error:
                function (response) {
                    alert("Error: " + response);
                }
        });
}

