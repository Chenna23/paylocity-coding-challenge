
function confirmDelete(recordId, isDeleteClick) {
    var deleteSpan = 'deleteSpan_' + recordId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + recordId;

    if (isDeleteClick) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}