let dataTable;

function successMessage(messageObj) {
    swal.fire({
        title: messageObj.title,
        text: messageObj.message,
        icon: "success"
    });
}

function errorMessage(messageObj) {
    swal.fire({
        title: messageObj.title,
        text: messageObj.message,
        icon: "error"
    });
}

function initializeDataTable(tableId, userOptions) {

    const defaultOptions = {
        // serverSide: true,
        autoWidth: false,
        scrollX: true
    };

    const dtOptions = $.extend(true, {}, defaultOptions, userOptions);

    $(`#${tableId}`).DataTable(dtOptions);
}