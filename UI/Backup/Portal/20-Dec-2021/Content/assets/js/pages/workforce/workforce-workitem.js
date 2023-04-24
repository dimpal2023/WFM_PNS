$("[id*=tblDailyWorkItems] > tbody tr:first-child").addClass("Hide");

function AppendRow(row, workItem) {
    //Bind item.
    $(".ItemId", row).find("span").html(workItem.ITEM.Value);
    $(".ItemId", row).find("label").html(workItem.ITEM.ID);

    //Bind operation.
    $(".UniqueOperationId", row).find("span").html(workItem.OPERATION.Value);
    $(".UniqueOperationId", row).find("label").html(workItem.OPERATION.ID);

    //Bind quantity.
    $(".Qty", row).find("span").html(workItem.QTY);        
    $("[id*=tblDailyWorkItems]").append(row);
}

//Add event handler.
$("body").on("click", "[id*=btnAdd]", function () {
    var itemID = $('select#ITEM_ID option:selected').text();
    var uniqueOperationID = $('select#UNIQUE_OPERATION_ID option:selected').text();
    var qty = $("[id*=QTY]").val();

    var return_val = true;
    if (itemID === '-- Select--' || itemID === '' || itemID === null) {
        var errorMessage = "Please select item.";
        $('#ITEM_ID').parent().next('span').text(errorMessage).show();
        $('#ITEM_ID').parent().next('span').addClass("field-validation-error");
        return_val = false;
    }
    else {
        $('#ITEM_ID').parent().next('span').text(errorMessage).hide();
        $('#ITEM_ID').parent().next('span').addClass("field-validation-valid");
    }

    if (uniqueOperationID === '-- Select--' || uniqueOperationID === '' || uniqueOperationID === null) {
        var errorMessage = "Please select operation.";
        $('#UNIQUE_OPERATION_ID').next('span').text(errorMessage).show();
        $('#UNIQUE_OPERATION_ID').next('span').addClass("field-validation-error");
        return_val = false;
    }
    else {
        $('#UNIQUE_OPERATION_ID').next('span').text(errorMessage).hide();
        $('#UNIQUE_OPERATION_ID').next('span').addClass("field-validation-valid");
    }

    if (qty === '' || qty === null) {
        var errorMessage = "Please select quantity.";
        $('#QTY').next('span').text(errorMessage).show();
        $('#QTY').next('span').addClass("field-validation-error");
        return_val = false;
    }
    else {
        $('#QTY').next('span').text(errorMessage).hide();
        $('#QTY').next('span').addClass("field-validation-valid");
    }

    if (!return_val) return;
   
    var workItem = {
        ITEM: { 
            ID: $('select#ITEM_ID option:selected').val(),
            Value: itemID
        },
        OPERATION: {
            ID: $('select#UNIQUE_OPERATION_ID option:selected').val(),
            Value: uniqueOperationID
        },
        QTY: qty
    }
    $("[id*=tblDailyWorkItems] > tbody tr:first-child").removeClass("Hide");
    var row = $("[id*=tblDailyWorkItems] > tbody tr:last-child").clone(true);
    $("[id*=tblDailyWorkItems] > tbody tr:first-child").addClass("Hide");
    AppendRow(row, workItem);        
    
    return false;
});

//Delete event handler.
$("body").on("click", "[id*=tblDailyWorkItems] .Delete", function () {
    if (confirm("Do you want to delete this record?")) {
        var row = $(this).closest("tr");
        var itemId = row.find("span").html();
        if (itemId === null || itemId === '') return;
        row.remove();
    }

    return false;
});