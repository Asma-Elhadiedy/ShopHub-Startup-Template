let dataTable;

$(function () {

    if ($(".select2").length > 0)
        $('.select2').select2({
            width: "100%"
        });
});

function successMessage({ title = "Success!", message = "Your action is applied successfully." } = {}) {
    swal.fire({
        title,
        text: message,
        icon: "success"
    });
}

function errorMessage({ title = "Error", message = "Contact technical support." } = {}) {
    Swal.fire({
        title,
        text: message,
        icon: "error"
    });
}

function confirmationMessage({ title = "Are You Sure?", message = "You won't be able to revert this!", confirmButtonText = "Yes", cancelButtonText = "Cancel", confirmationCallBack = null } = {}) {
    Swal.fire({
        title,
        text: message,
        icon: "warning",
        cancelButtonText,
        confirmButtonText,
        showCancelButton: true,
    }).then((result) => {
        if (result.isConfirmed && typeof (confirmationCallBack) === "function")
            confirmationCallBack();
        else
            return;
    });
}

function initializeDataTable(tableId, userOptions) {

    const defaultOptions = {
        serverSide: true,
        autoWidth: false,
        scrollX: true,
        ajax: {
            error: function (xhr, error, thrown) {
                // console.error(` Ajax Error`, thrown);
                if (typeof errorMessage === 'function') {
                    errorMessage(xhr.responseJSON);
                }
            }
        }
    };

    const dtOptions = $.extend(true, {}, defaultOptions, userOptions);

    return $(`#${tableId}`).DataTable(dtOptions);
}
function initializeClientSideDataTable(tableId, userOptions) {

    const defaultOptions = {
        // serverSide: true,
        autoWidth: false,
        scrollX: true,
        ajax: {
            error: function (xhr, error, thrown) {
                // console.error(` Ajax Error`, thrown);
                if (typeof errorMessage === 'function') {
                    errorMessage(xhr.responseJSON);
                }
            }
        }
    };

    const dtOptions = $.extend(true, {}, defaultOptions, userOptions);

    $(`#${tableId}`).DataTable(dtOptions);
}


window.addEventListener("offline", () => {
    window.location.href = `${appBasePath}/offline.html`;
})


