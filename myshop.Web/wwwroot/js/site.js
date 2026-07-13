let dataTable;

function initializeDataTable(tableId, userOptions) {

    const defaultOptions = {
        // serverSide: true,
        autoWidth: false,
        scrollX: true
    };

    const dtOptions = $.extend(true, {}, defaultOptions, userOptions);

    $(`#${tableId}`).DataTable(dtOptions);
}