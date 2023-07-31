(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};

    Wfm.App.ToolTalk = {
        Create: function () {
            if (!Wfm.App.ToolTalk.Validate()) {
                event.preventDefault();
                return;
            }
            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val();
            var itemName = $("#ITEM_NAME").val();
            var BUILDING_ID = $("#BUILDING_ID").val();
            var toolTalk = { DEPT_ID: deptId, SUBDEPT_ID: subDeptId, ITEM_NAME: itemName, BUILDING_ID: BUILDING_ID };

            toolTalk = JSON.stringify({ 'toolTalk': toolTalk });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Create",
                data: toolTalk,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.id == '1') {
                        alert("Check details created Successfully.");
                        window.location = result.Url;
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });


        },
        Edit: function () {
            if (!Wfm.App.ToolTalk.Validate()) {
                event.preventDefault();
                return;
            }

            var toolTalkId = $("#ID").val();
            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val();
            var itemName = $("#ITEM_NAME").val();
            var BUILDING_ID = $("#BUILDING_ID").val();

            var toolTalk = { ID: toolTalkId, DEPT_ID: deptId, SUBDEPT_ID: subDeptId, ITEM_NAME: itemName, BUILDING_ID: BUILDING_ID };

            toolTalk = JSON.stringify({ 'toolTalk': toolTalk });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Edit",
                data: toolTalk,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.id == '1') {
                        alert("Check details updated Successfully.");
                        window.location = result.Url;
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        Delete: function () {
            debugger
            var toolTalkId = $("#hiddenId").val();

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Delete",
                data: { Id: toolTalkId },
                success: function (result) {
                    $("#exampleModalCenter").modal("hide");
                    if (result === 0) {
                        $('#message').css("display", "block");
                        return;
                    }
                    if (result.id == '1') {
                        alert("Details deleted Successfully.");
                        window.location.href = result.Url;
                    }
                }
            })
        },
        DeleteConfiguration: function () {
            var configId = $("#hiddenConfigId").val();

            var configuredItem = {
                ID: configId
            };

            configuredItem = JSON.stringify({
                'configuredItem': configuredItem
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/DeleteConfiguration",
                data: configuredItem,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    $("#configuredModalCenter").modal("hide");
                    window.location.href = result.Url;
                }
            })
        },
        DeleteDailyCheckList: function () {
            var id = $("#hiddenId").val();
            $("#dailyModalCenter").modal("hide");
            var dailyCheckListItem = {
                ID: id
            };

            dailyCheckListItem = JSON.stringify({
                'dailyCheckListItem': dailyCheckListItem
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/DeleteDailyCheckList",
                data: dailyCheckListItem,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.id == '11') {

                        alert("Details Deleted Successfully.");
                        window.location.href = result.Url;
                    } else {

                    }
                }
            })
        },
        Configure: function () {
            var return_val = true;
            var errorMessage = "";
            if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Department is required.";
                $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#SUBDEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Sub Department is required.";
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).show();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#SHIFT_ID").val() === '' || $("#SHIFT_ID").val() === null) {
                errorMessage = "Shift is required.";
                $('#SHIFT_ID').parent().next('span').text(errorMessage).show();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_ID').parent().next('span').text(errorMessage).hide();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-valid");
            }

            var selected = false;

            $('input:checkbox').each(function () {
                var check = this.checked;
                if (check) {
                    selected = true;
                    return false;
                }
            });

            if ($('input:checkbox').length === 0 && return_val) {
                return_val = false;
                $('#errorCheckbox').text("No check list exist for this department and sub department");
            }

            if (!selected) {
                $('#errorCheckbox').css("display", "block");
                return_val = false;
            } else {
                $('#errorCheckbox').css("display", "none");
            }

            if (!return_val) {
                event.preventDefault(); return;
            }

            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val();
            var shiftId = $("#SHIFT_ID").val();
            var tool_talk_Id = new Array();

            $('input:checkbox').each(function () {
                if (this.checked && this.id !== "select-all") {
                    var checkBoxItem = {
                        ID: $(this).attr("id"), TOOL_TALK_ID: $(this).attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: ""
                    };
                    tool_talk_Id.push(checkBoxItem);
                }
            });

            var toolTalkConfiguration = {
                DEPT_ID: deptId, SUBDEPT_ID: subDeptId, SHIFT_ID: shiftId, TOOL_TALK_CHECK_LIST: tool_talk_Id
            };
            toolTalkConfiguration = JSON.stringify({
                'toolTalkConfiguration': toolTalkConfiguration
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Configure",
                data: toolTalkConfiguration,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result === 0) {
                        $('#errorCheckbox').text("Configuration already exist for this combination(department, sub department and shift).");
                        $('#errorCheckbox').css("display", "block");
                    }
                    else {
                        window.location = result.Url;
                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        EditConfiguration: function () {
            var return_val = true,
                errorMessage = "",
                selected = false;
            if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Department is required.";
                $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#SUBDEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Sub Department is required.";
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).show();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#SHIFT_ID").val() === '' || $("#SHIFT_ID").val() === null) {
                errorMessage = "Shift is required.";
                $('#SHIFT_ID').parent().next('span').text(errorMessage).show();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_ID').parent().next('span').text(errorMessage).hide();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-valid");
            }

            $('input:checkbox').each(function () {
                var check = this.checked;
                if (check) {
                    selected = true;
                    return false;
                }
            });

            if (!selected) {
                $('#errorCheckbox').css("display", "block");
                return_val = false;
            } else {
                $('#errorCheckbox').css("display", "none");
            }

            if (!return_val) {
                event.preventDefault(); return;
            }

            var id = $("#ID").val();
            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val();
            var shiftId = $("#SHIFT_ID").val();
            var tool_talk_Id = new Array();

            $('input:checkbox').each(function () {
                if (this.checked) {
                    var checkBoxItem = { ID: $(this).attr("id"), TOOL_TALK_ID: $(this).attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: "" };
                    tool_talk_Id.push(checkBoxItem);
                }
            });

            var configuredItem = {
                ID: id, DEPT_ID: deptId, SUBDEPT_ID: subDeptId, SHIFT_ID: shiftId, TOOL_TALK_CHECK_LIST: tool_talk_Id
            };
            configuredItem = JSON.stringify({
                'configuredItem': configuredItem
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/EditConfiguration",
                data: configuredItem,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    window.location = result.Url;
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        EditDailyCheckList: function () {

            var return_val = true;
            var errorMessage = "";

            if ($("#DELIVERED_BY").val() === '' || $("#DELIVERED_BY").val() === null) {
                errorMessage = "Delivered By is required.";
                $('#DELIVERED_BY').next('span').text(errorMessage).show();
                $('#DELIVERED_BY').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DELIVERED_BY').next('span').text(errorMessage).hide();
                $('#DELIVERED_BY').next('span').addClass("field-validation-valid");
            }

            if (!return_val) {
                event.preventDefault(); return;
            }

            if (!return_val) { event.preventDefault(); return; }
            var DELIVERED_BY = $('#DELIVERED_BY').val();
            var id = $("#ID").val();
            var tool_talk_Id = new Array();

            var EmpListing = '';
            $("#tableExport2 tbody tr").each(function (j) {
                if ($(this).find('.emplist_chk').prop('checked') == true) {
                    EmpListing += $(this).find('.hdnEmp').text() + ',';
                }
            })
            $("#EMP_NAME").val(EmpListing);

            var tool_talk_Id = new Array();
            $("#tableExports tbody tr").each(function (i) {
                if ($(this).find('.instruction_list').prop('checked') == true) {
                    var checkBoxItem = {
                        ID: $(this).find('.instruction_list').attr("id"), TOOL_TALK_ID: $(this).find('.instruction_list').attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: ""
                    };
                    tool_talk_Id.push(checkBoxItem);
                }
            });
            if (tool_talk_Id.length == 0) {
                alert("Please select Tool Talk Message.");
                return;
            }
            if ($("#EMP_NAME").val() === '' || $("#EMP_NAME").val() === null) {
                alert("Please select Employee.");
                return;
            }
            var dailyCheckListItem = { ID: id, EMP_NAME: EmpListing, TOOL_TALK_CHECK_LIST: tool_talk_Id, DELIVERED_BY: DELIVERED_BY };
            dailyCheckListItem = JSON.stringify({
                'dailyCheckListItem': dailyCheckListItem
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/EditDailyCheckList",
                data: dailyCheckListItem,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.id == '11') {
                        alert("Details Updated Successfully.");
                        window.location.href = result.Url;
                    } else {

                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        CreateDailyCheckList: function () {

            var return_val = true;
            var errorMessage = "";
            if ($("#BUILDING_ID").val() === '' || $("#BUILDING_ID").val() === null) {
                errorMessage = "Unit is required.";
                $('#BUILDING_ID').parent().next('span').text(errorMessage).show();
                $('#BUILDING_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#BUILDING_ID').parent().next('span').text(errorMessage).hide();
                $('#BUILDING_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Department is required.";
                $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            //if ($("#SUBDEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
            //    errorMessage = "Sub Department is required.";
            //    $('#SUBDEPT_ID').parent().next('span').text(errorMessage).show();
            //    $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-error");
            //    return_val = false;
            //}
            //else {
            //    $('#SUBDEPT_ID').parent().next('span').text(errorMessage).hide();
            //    $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-valid");
            //}

            if ($("#SHIFT_ID").val() === '' || $("#SHIFT_ID").val() === null) {
                errorMessage = "Shift is required.";
                $('#SHIFT_ID').parent().next('span').text(errorMessage).show();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SHIFT_ID').parent().next('span').text(errorMessage).hide();
                $('#SHIFT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#DELIVERED_BY").val() === '' || $("#DELIVERED_BY").val() === null) {
                errorMessage = "Delivered By is required.";
                $('#DELIVERED_BY').next('span').text(errorMessage).show();
                $('#DELIVERED_BY').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DELIVERED_BY').next('span').text(errorMessage).hide();
                $('#DELIVERED_BY').next('span').addClass("field-validation-valid");
            }

            if (!return_val) {
                event.preventDefault(); return;
            }

            if (!return_val) { event.preventDefault(); return; }
            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val() || '00000000-0000-0000-0000-000000000000';
            var BUILDING_ID = $('#BUILDING_ID').val();
            var SHIFT_ID = $('#SHIFT_ID').val();
            var DELIVERED_BY = $('#DELIVERED_BY').val();

            var EmpListing = '';
            $("#tableExport2 tbody tr").each(function (j) {
                if ($(this).find('.emplist_chk').prop('checked') == true) {
                    EmpListing += $(this).find('.hdnEmp').text() + ',';
                }
            })
            $("#EMP_NAME").val(EmpListing);

            var tool_talk_Id = new Array();
            $("#tableExport tbody tr").each(function (i) {
                if ($(this).find('.instruction_list').prop('checked') == true) {
                    var checkBoxItem = {
                        ID: $(this).find('.instruction_list').attr("id"), TOOL_TALK_ID: $(this).find('.instruction_list').attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: ""
                    };
                    tool_talk_Id.push(checkBoxItem);
                }
            });
            if (tool_talk_Id.length == 0) {
                alert("Please select Tool Talk Message.");
                return;
            }
            if ($("#EMP_NAME").val() === '' || $("#EMP_NAME").val() === null) {
                alert("Please select Employee.");
                return;
            }

            var toolTalkCheckList = { BUILDING_ID: BUILDING_ID, DEPT_ID: deptId, SUBDEPT_ID: subDeptId, SHIFT_AUTOID: SHIFT_ID, DELIVERED_BY: DELIVERED_BY, WF_ID: 'A8703F99-ADB1-4D86-899E-AB77EA7921FD', EMP_NAME: EmpListing, TOOL_TALK_CHECK_LIST: tool_talk_Id };
            toolTalkCheckList = JSON.stringify({
                'toolTalkCheckList': toolTalkCheckList
            });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/CreateDailyCheckList",
                data: toolTalkCheckList,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    debugger;
                    if (result === 0) {
                        $('#errorCheckbox').text("There is an issue occured during entry creation.");
                        $('#errorCheckbox').css("display", "block");
                    }
                    else if (result === 1) {
                        $('#errorCheckbox').text("Today's entires already marked for " + empName + ".");
                        $('#errorCheckbox').css("display", "block");
                    }
                    else if (result.id == '11') {
                        alert("Details Saved Successfully.");
                        window.location.href = result.Url;
                    } else {

                    }
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        Validate: function () {
            var return_val = true;
            var errorMessage = "";

            if ($("#DEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Department is required.";
                $('#DEPT_ID').parent().next('span').text(errorMessage).show();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#DEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#SUBDEPT_ID").val() === '' || $("#DEPT_ID").val() === null) {
                errorMessage = "Sub Department is required.";
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).show();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#SUBDEPT_ID').parent().next('span').text(errorMessage).hide();
                $('#SUBDEPT_ID').parent().next('span').addClass("field-validation-valid");
            }

            if ($("#ITEM_NAME").val() === '' || $("#ITEM_NAME").val() === null) {
                errorMessage = "Check List name is required.";
                $('#ITEM_NAME').next('span').text(errorMessage).show();
                $('#ITEM_NAME').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#ITEM_NAME').next('span').text(errorMessage).hide();
                $('#ITEM_NAME').next('span').addClass("field-validation-valid");
            }

            return return_val;
        },
        Cancel: function () {
            window.location.href = "/ToolTalk/AllItems";
        }
    };

    function initialize() {
        
        $('#submit').on("click", Wfm.App.ToolTalk.Create);
        $('#cancel').on("click", Wfm.App.ToolTalk.Cancel);
        $('#edit').on("click", Wfm.App.ToolTalk.Edit);
        $('.btn-tbl-delete').each(function () {
            debugger
            if ($(this).attr("data-target") === "#configuredModalCenter") {
                $(this).on("click", ConfirmDeleteConfiguration);
            }
            if ($(this).attr("data-target") === "#dailyModalCenter") {
                $(this).on("click", ConfirmDeleteDailyCheckList);
            }
            else {
                $(this).on("click", ConfirmDelete);
            }
        });
        $('#tooltalk_delete_confirm').on("click", Wfm.App.ToolTalk.Delete);
        $('#configureditems_delete_confirm').on("click", Wfm.App.ToolTalk.DeleteConfiguration);
        $('#configure').on("click", Wfm.App.ToolTalk.Configure);
        $('#editconfigure').on("click", Wfm.App.ToolTalk.EditConfiguration);
        $('#editdailychecklist').on("click", Wfm.App.ToolTalk.EditDailyCheckList);


        $('#createdailychecklist').on('click', Wfm.App.ToolTalk.CreateDailyCheckList);
        $('#dailyitems_delete_confirm').on("click", Wfm.App.ToolTalk.DeleteDailyCheckList);
    }

    //function ConfirmDelete() {
    //    debugger
    //    var rowId = $(this).parent().parent().attr("id");
    //    var startIndex = rowId.indexOf('_');
    //    var toolTalkId = rowId.substr(startIndex + 1, rowId.length);
    //    $("#hiddentoolTalkId").val(toolTalkId);
    //    $('#message').css("display", "none");
    //}

    function ConfirmDeleteConfiguration() {
        var configId = $(this).parent().parent().attr("id");
        $("#hiddenConfigId").val(configId);
    }

    function ConfirmDeleteDailyCheckList() {
        var dailyCheckListId = $(this).parent().parent().attr("id");
        $("#hiddenId").val(dailyCheckListId);
    }

    function GetTodayDate() {
        var tdate = new Date();
        var dd = tdate.getDate(); //yields day
        var MM = tdate.getMonth(); //yields month
        var yyyy = tdate.getFullYear(); //yields year
        var currentDate = dd + "/" + (MM + 1) + "/" + yyyy;

        return currentDate;
    }

    function ToDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear();
    }



    var d = new Date();
    var currMonth = d.getMonth();
    var currYear = d.getFullYear();
    var startDate = new Date(currYear, currMonth, 1);

    initialize();

})();

function GetData() {
    var DEPT_ID = $('#DEPT_ID').val();
    var SUBDEPT_ID = $('#SUBDEPT_ID').val();
    var BUILDING_ID = $('#BUILDING_ID').val();
    $('#particalDiv').html('');

    $.get('/ToolTalk/GetData?DEPT_ID=' + DEPT_ID + '&SUB_DEPT_ID=' + SUBDEPT_ID + '&BUILDING_ID=' + BUILDING_ID, function (data) {
        $('#particalDiv').html(data);
        $('#particalDiv').fadeIn('fast');
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
    });

}
function ConfirmDelete() {
    debugger
    var rowId = $(this).parent().parent().attr("id");
    var startIndex = rowId.indexOf('_');
    var toolTalkId = rowId.substr(startIndex + 1, rowId.length);
    $("#hiddenId").val(toolTalkId);
    $('#message').css("display", "none");
}
$("#DEPT_ID").change(function () {
    getSubDeptbyDeptId();
});
function getSubDeptbyDeptId() {
    var DEPARTMENT_ID = $("#DEPT_ID").val();
    if (DEPARTMENT_ID != null) {
        $.get('/Dashboard/GetSubDepartmentByDepartmentId?departmentId=' + DEPARTMENT_ID, function (data) {
            $('#SUBDEPT_ID').find('option').not(':first').remove();
            $.each(data, function (i, item) {
                $('#SUBDEPT_ID').append($('<option>', {
                    value: item.SUBDEPT_ID,
                    text: item.SUBDEPT_NAME
                }));
            });
            $('#SUBDEPT_ID').formSelect();
        });
    } else {
        $('#SUBDEPT_ID').find('option').not(':first').remove();
        $('#SUBDEPT_ID').formSelect();
        GetEmployeeData();
    }
}

function GetCheckListData() {
    var DEPT_ID = $('#DEPT_ID').val() ||'00000000-0000-0000-0000-000000000000';
    var SUBDEPT_ID = $('#SUBDEPT_ID').val() ||'00000000-0000-0000-0000-000000000000';
    var BUILDING_ID = $('#BUILDING_ID').val();
    $('#particalDiv').html('');
    if (BUILDING_ID == '') {
        return;
    }
    $.get('/ToolTalk/GetCheckListData?DEPT_ID=' + DEPT_ID + '&SUB_DEPT_ID=' + SUBDEPT_ID + '&BUILDING_ID=' + BUILDING_ID, function (data) {
        $('#particalDiv').html(data);
        $('#particalDiv').fadeIn('fast');
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
    });

}

