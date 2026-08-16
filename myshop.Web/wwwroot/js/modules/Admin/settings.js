

$(function () {
    $("#js-settingsSidebar").addClass("menu-open");
});

const restoreDeletedProducts = () => {
    $.post(`${appBasePath}/Admin/Setting/restoreDeletedProducts`)
        .done((success) => {
            successMessage(success);
        }).fail((error) => {
            errorMessage(error);
        });
}