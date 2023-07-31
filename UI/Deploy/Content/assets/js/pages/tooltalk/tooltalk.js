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

            var toolTalk = { DEPT_ID: deptId, SUBDEPT_ID: subDeptId, ITEM_NAME: itemName };

            toolTalk = JSON.stringify({ 'toolTalk': toolTalk });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Create",
                data: toolTalk,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    window.location = result.Url
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

            var toolTalk = { ID: toolTalkId, DEPT_ID: deptId, SUBDEPT_ID: subDeptId, ITEM_NAME: itemName };

            toolTalk = JSON.stringify({ 'toolTalk': toolTalk });

            $.ajax({
                type: "POST",
                url: "/ToolTalk/Edit",
                data: toolTalk,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    window.location = result.Url
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        Delete: function () {
            var toolTalkId = $("#hiddentoolTalkId").val();

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
                    window.location.href = result.Url;
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
                    $("#dailyModalCenter").modal("hide");
                    window.location.href = result.Url;
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
                $('#errorCheckbox').text("Please check at least one item from list.");
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
                $('#errorCheckbox').text("Please check at least one item from list.");
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

                    if (checkBoxItem.TOOL_TALK_ID !== undefined) {
                        tool_talk_Id.push(checkBoxItem);
                    }
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

            if ($("#DATE").val() === '' || $("#DATE").val() === null) {
                errorMessage = "Date is required.";
                $('#DATE').next('span').text(errorMessage).show();
                $('#DATE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DATE').next('span').text(errorMessage).hide();
                $('#DATE').next('span').addClass("field-validation-valid");
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
                $('#errorCheckbox').text("Please check at least one item from list.");
                return_val = false;
            } else {
                $('#errorCheckbox').css("display", "none");
            }

            if (!return_val) { event.preventDefault(); return; }

            var id = $("#ID").val();
            var shiftId = $("#SHIFT_ID").val();
            var dailyDate = $("#DATE").val();
            var tool_talk_Id = new Array();

            $('input:checkbox').each(function () {
                if (this.checked) {
                    var checkBoxItem = {
                        ID: $(this).attr("id"), TOOL_TALK_ID: $(this).attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: ""
                    };

                    if (checkBoxItem.TOOL_TALK_ID !== undefined) {
                        tool_talk_Id.push(checkBoxItem);
                    }
                }
            });

            var dailyCheckListItem = { ID: id, SHIFT_ID: shiftId, DATE: dailyDate, TOOL_TALK_CHECK_LIST: tool_talk_Id };
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
                    window.location = result.Url;
                },
                error: function (responseText) {
                    alert(responseText);
                }
            });

        },
        CreateDailyCheckList: function () {
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

            if ($("#DATE").val() === '' || $("#DATE").val() === null) {
                errorMessage = "Please select date.";
                $('#DATE').next('span').text(errorMessage).show();
                $('#DATE').next('span').addClass("field-validation-error");
                return_val = false;
            }
            else {
                $('#DATE').next('span').text(errorMessage).hide();
                $('#DATE').next('span').addClass("field-validation-valid");
            }

            if (!return_val) {
                event.preventDefault(); return;
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
                $('#errorCheckbox').text("Please check at least one item from list.");
                return_val = false;
            } else {
                $('#errorCheckbox').css("display", "none");
            }

            if (!return_val) { event.preventDefault(); return; }

            var deptId = $("#DEPT_ID").val();
            var subDeptId = $("#SUBDEPT_ID").val();
            var shiftId = $("#SHIFT_ID").val();
            var shiftName = $("#SHIFT_ID option:selected").text();
            var checkListDate = $("#DATE").val();
            var tool_talk_Id = new Array();

            $('input:checkbox').each(function () {
                if (this.checked && this.id !== "select-all") {
                    var checkBoxItem = {
                        ID: $(this).attr("id"), TOOL_TALK_ID: $(this).attr("tool-talk-id"), CHECK: this.checked, ITEM_NAME: ""
                    };
                    tool_talk_Id.push(checkBoxItem);
                }
            });

            var toolTalkCheckList = { DEPT_ID: deptId, SUBDEPT_ID: subDeptId, SHIFT_ID: shiftId, DATE: checkListDate, TOOL_TALK_CHECK_LIST: tool_talk_Id };
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
                    if (result === 0) {
                        $('#errorCheckbox').text("There is an issue occured during entry creation.");
                        $('#errorCheckbox').css("display", "block");
                    }
                    else if (result === 1) {
                        $('#errorCheckbox').text("Today's entires already marked for " + shiftName + ".");
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
        $('#configure').on("click", Wfm.App.ToolTalk.Configure);
        $('#editconfigure').on("click", Wfm.App.ToolTalk.EditConfiguration);
        $('#editdailychecklist').on("click", Wfm.App.ToolTalk.EditDailyCheckList);
        $('.btn-tbl-delete').each(function () {
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

        $('#createdailychecklist').on('click', Wfm.App.ToolTalk.CreateDailyCheckList);
        $('#dailyitems_delete_confirm').on("click", Wfm.App.ToolTalk.DeleteDailyCheckList);
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var toolTalkId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddentoolTalkId").val(toolTalkId);
        $('#message').css("display", "none");
    }

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

    initialize();

    var d = new Date();
    var currMonth = d.getMonth();
    var currYear = d.getFullYear();
    var startDate = new Date(currYear, currMonth, 1);
})();

