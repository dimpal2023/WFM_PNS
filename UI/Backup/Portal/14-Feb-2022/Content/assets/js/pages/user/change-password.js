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