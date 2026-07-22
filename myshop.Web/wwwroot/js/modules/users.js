
$(function () {
    $("#js-usersSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/User/GetData`,
        type: "GET",
        dataSrc: "data"
    },
    columns: [
        { title: "Full Name", data: "fullName" },
        { title: "Email", data: "email" },
        { title: "Role", data: "roleName" },
        { title: "Lock Status", data: "isLocked" },
        {
            title: "Actions",
            data: "id",
            render: function (data) {
                return `
                        <a href="${appBasePath}/User/Edit/${data}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i> Edit
                        </a>

                        <a href="${appBasePath}/User/Delete/${data}" class="btn btn-danger btn-sm">
                            <i class="fa-solid fa-trash"></i> Delete
                        </a>
                    `;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

