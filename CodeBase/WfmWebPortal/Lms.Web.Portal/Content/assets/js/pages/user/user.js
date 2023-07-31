(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};
    Wfm.App.User = {
        Delete: function () {
            var gatePassId = $("#hiddenUserId").val();

            $.ajax({
                type: "POST",
                url: "/User/Delete",
                data: { Id: gatePassId },
                success: function (response) {
                    window.location.href = response.Url;
                }
            })
        },
        Edit: function () {
            var userId = $("#hiddenUserId").val();

            $.ajax({
                type: "POST",
                url: "/User/MarkActive",
                data: { Id: userId },
                success: function (response) {
                    window.location.href = response.Url + "/" + userId;
                }
            })
        },
        Cancel: function () {
            window.location.href = "/User/Index";
        },
        
    };

    function initialize() {
        $('#cancel').on("click", Wfm.App.User.Cancel);
        $('.btn-tbl-delete').each(function () {
            $(this).on("click", ConfirmDelete);
        });
        $('#user_delete_confirm').on("click", Wfm.App.User.Delete);

        //edit
        $('.btn-tbl-edit').each(function () {
            $(this).on("click", ConfirmEdit);
        });
        $('#user_edit_confirm').on("click", Wfm.App.User.Edit);
    }

    function ConfirmDelete() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var userId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenUserId").val(userId);
    }

    function ConfirmEdit() {
        var rowId = $(this).parent().parent().attr("id");
        var startIndex = rowId.indexOf('_');
        var userId = rowId.substr(startIndex + 1, rowId.length);
        $("#hiddenUserId").val(userId);
    }
   
    initialize();
})();