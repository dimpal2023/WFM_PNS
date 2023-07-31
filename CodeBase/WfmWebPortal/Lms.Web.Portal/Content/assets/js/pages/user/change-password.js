(function () {
    "use strict"
    var Wfm = {};
    Wfm.App = {};
    Wfm.App.ChangePassword = {
        Cancel: function () {
            window.location.href = "/Dashboard/Index";
        }
    };

    function initialize() {
        $('#cancel').on("click", Wfm.App.ChangePassword.Cancel);
    }

    initialize();
})();

$("#submit").click(function () {
    var CURRENT_PASSWORD = $("#CURRENT_PASSWORD").val();
    var New_PASSWORD = $("#New_PASSWORD").val();
    var Confirm_PASSWORD = $("#Confirm_PASSWORD").val();
    var resetPassword = { CURRENT_PASSWORD: CURRENT_PASSWORD, New_PASSWORD: New_PASSWORD, Confirm_PASSWORD: Confirm_PASSWORD};

    resetPassword = JSON.stringify({ 'resetPassword': resetPassword });
    if (New_PASSWORD != Confirm_PASSWORD) {
        alert('New Password and Confirm password do not match.');
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Account/ChangePassword",
        data: resetPassword,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        beforeSend: function () {
            $('.page-loader-wrapper').show();
        },
        complete: function () {
            $('.page-loader-wrapper').hide();
        },
        success: function (result) {
            if (result == 'True') {
                alert("Your password have been changed.");
                 window.location = "/";
            } else {
                alert("Your Current password is wrong.");
            }
        },
        error: function (responseText) {
            alert(responseText.d);
        }
    });
})