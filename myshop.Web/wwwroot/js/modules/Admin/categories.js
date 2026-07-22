
$(function () {
    $("#js-categoriesSidebar").addClass("menu-open");

    dataTable = initializeDataTable("mytable", datatableOptions);

});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Admin/Category/GetData`,
        type: "GET",
    },
    columns: [
        {
            title: 'Name',
            data: "name"
        },
        {
            title: 'Description',
            data: "description"
        },
        {
            title: 'Date Created',
            data: "createdTime",
            render: function (data) {
                return data.split("T")[0];
                // return new Date(data).toLocaleDateString("en-US", {});
            }
        },
        {
            title: 'Actions',
            data: "id",
            orderable: false,
            render: function (data) {
                return `
                        <a href="${appBasePath}/Admin/Category/Edit/${data}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i> Edit
                        </a>

                        <a href="${appBasePath}/Admin/Category/Delete/${data}" class="btn btn-danger btn-sm">
                            <i class="fa-solid fa-trash"></i> Delete
                        </a>
                    `;
            }
        }
    ]
};