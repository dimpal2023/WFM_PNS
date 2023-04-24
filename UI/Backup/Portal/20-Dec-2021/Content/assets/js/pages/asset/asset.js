(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};

    Wfm.App.Asset = {
        Create: function () {
            var $form = $(this).parents('form');
            if ($("#AssetManagementCreate").valid()) {
                $.ajax({
                    type: "POST",
                    url: $form.attr('action'),
                    data: $form.serialize(),
                    error: function (xhr, status, error) {
                    },
                    success: function (response) {
                        window.location.href = response.Url;
                    }
                });
            } else {
                return false;
            }
        },
        Edit: function () {
            if (!Wfm.App.Asset.Validate()) {
                event.preventDefault();
                return;
            }

            var $form = $(this).parents('form');
            if ($("#AssetManagementCreate").valid()) {
                $.ajax({
                    type: "POST",
                    url: $form.attr('action'),
                    data: $form.serialize(),
                    error: function (xhr, status, error) {
                    },
                    success: function (response) {
                        window.location.href = response.Url;
                    }
                });
            } else {
                return false;
            }
        },
        Delete: function () {
            var gatePassId = $("#hiddenAsset").val();

            $.ajax({
                type: "POST",
                url: "/AssetManagement/Delete",
                data: { Id: gatePassId },
                success: function (response) {
                    window.location.href = response.Url;
                }
            })
        },
        Cancel: function () {
            window.location.href = "/AssetManagement/AllAssets";
        },
        Validate: function () {
            var return_val = true;

            return return_val;
        }
    };

    function initialize() {
        $('#DEPT_ID').on("change", departmentonChange);
        //$('#EMP_ID').on("change", employeeOnChange);

        $('#submit').on("click", Wfm.App.Asset.Create);
        $('#edit').on("click", Wfm.App.Asset.Edit);
        $('#cancel').on("click", Wfm.App.Asset.Cancel);
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
        $('#asset_delete_confirm').on("click", Wfm.App.Asset.Delete);
        $("#COMPANIES").on("change", function () {
            var companiesId = $("input.select-dropdown").next().attr("id");
            var selectOptionId = $('#' + companiesId + " li.selected").attr("id");
            var selectedCompanyId = selectOptionId.split("options-")[1];

            //setting selected company id to hidden field
            $("#COMPANY_ID").val(selectedCompanyId);
        });

        if ($("#COMPANY_ID").val() !== '') {

        }
        else if ($("#COMPANIES") !== undefined) {
            //setting default compnay id to hidden field
            var defaultCompanyId = $("#COMPANIES option:first").attr("value");
            $("input[type=hidden]").val(defaultCompanyId);
        }
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var assetId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenAsset").val(assetId);
    }

    function ToDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear();
    }
    function departmentonChange() {
        $('#emplyeedetail').empty();
        $('#rowValue').val(0);
    }
    
    function addAsset() {
        var departId = $('#DEPT_ID').val();
        var rowNumber = $('#rowValue').val();
        if (rowNumber => 0) {
            var url = "/AssetManagement/GetAssetBydeptId/" + rowNumber + '/' + departId;
            $.get(url, function (data) {
                $("#tblParticipantList tbody").append(data);

                if (rowNumber >= 1) {
                    var removeTd = parseInt(rowNumber) + 1;
                    $('#remove_' + removeTd).html('<button type="button" onclick="remodeAsset(' + rowNumber + ')" id="btnRemoveAsset" value="' + rowNumber + '">Remove</button>');
                    if (rowNumber > 1) {
                        $('#remove_' + rowNumber).empty();
                    }
                } rowNumber = parseInt(rowNumber) + 1;
                $('#rowValue').val(rowNumber);

            });
        }

    }
    function remodeAsset(tr) {
        var removeTd = parseInt(tr) + 1;
        $('#trRemove_' + removeTd).remove();
        if (tr > 0) {
            removeTd = removeTd - 2
            if (removeTd > 0)
                $('#remove_' + tr).html('<button type="button" onclick="remodeAsset(' + removeTd + ')" id="btnRemoveAsset" value="' + removeTd + '">Remove</button>');
            var rowNumber = $('#rowValue').val();
            rowNumber = parseInt(rowNumber) - 1;
            $('#rowValue').val(rowNumber);
        }
    }



    initialize();
})();

