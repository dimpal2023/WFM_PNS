function levelChange() {
    var levelId = $("#LEVEL_ID option:selected").val();
    var workFlowId = $("#WORKFLOW_ID option:selected").val();
    $.get('/WorkFlowMapping/BindLevelOfApproavl?workFlowId=' + workFlowId + '&levelid=' + levelId, function (data) {
        $('#partialPlaceHolder').html(data);
        $('#partialPlaceHolder').fadeIn('fast');
    });
}

function roleOnChange(t) {
    var roleId = $('#ROLE_ID' + t).val();
    $.get('/User/GetUserByRoleId?roleId=' + roleId, function (data) {
        $('#EMP_ID' + t).find('option').not(':first').remove();

        $.each(data, function (i, item) {
            $('#EMP_ID' + t).append($('<option>', {
                value: item.USER_ID,
                text: item.USER_NAME
            }));
        });
    });
}

function autoApprovalChange(t) {
    var val = $('#AUTO_APPROVE' + t).val();
    if (val === 'N') {
        $('#AUTO_APPROVE_DAY' + t).val($('#AUTO_APPROVE_DAY' + t).prop('defaultSelected'));
        $('#AUTO_APPROVE_DAY' + t).prop('required', false);

    } else {
        $('#AUTO_APPROVE_DAY' + t).prop('required', true);
    }

}

function autoRejectChange(t) {
    var val = $('#AUTO_REJECT' + t).val();
    if (val === 'N') {
        $('#AUTO_REJECT_DAY' + t).val($('#AUTO_REJECT_DAY' + t).prop('defaultSelected'));
        $('#AUTO_REJECT_DAY' + t).prop('required', false);

    } else {
        $('#AUTO_REJECT_DAY' + t).prop('required', true);
    }
}