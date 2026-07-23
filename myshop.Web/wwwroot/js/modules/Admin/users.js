
$(function () {
    $("#js-usersSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Admin/User/GetData`,
        type: "GET",
        dataSrc: "data"
    },
    columns: [
        {
            title: "Full Name",
            data: "fullName"
        },
        {
            title: "Email",
            data: "email"
        },
        {
            title: "Role",
            data: "roleName"
        },
        {
            title: "Lock Status",
            data: "isLocked",
            render: function (isLocked) {
                return isLocked
                    ? `<i class="fa-solid fa-lock lock"></i>`
                    : ``;
            }
        },
        {
            title: "Actions",
            data: "id",
            render: function (data, type, row) {
                return `
                    <a href="${appBasePath}/Admin/User/Edit/${data}" class="btn btn-primary btn-sm">
                        <i class="fa-solid fa-pen"></i> Edit Role
                    </a>
                    ${row.isLocked
                        ? `<a href="${appBasePath}/Admin/User/ChangeStatus/${data}" class="btn btn-success btn-sm">
                        <i class="fa-solid fa-lock-open"></i>  Unlock
                        </a>`

                    : `<a href="${appBasePath}/Admin/User/ChangeStatus/${data}" class="btn btn-secondary btn-sm">
                        <i class="fa-solid fa-lock"></i>  Lock
                        </a>`
                    }
                    <a href="${appBasePath}/Admin/User/Delete/${data}" class="btn btn-danger btn-sm">
                        <i class="fa-solid fa-trash"></i> Delete    
                    </a>`;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

