
$(function () {
    $("#js-productsSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Product/GetData`,
        type: "GET",
        dataSrc: "data"
    },
    columns: [
        { title: "Name", data: "name" },
        { title: "Description", data: "description" },
        { title: "Price", data: "price" },
        { title: "Category", data: "categoryName" },
        {
            title: "Actions",
            data: "id",
            render: function (data) {
                return `
                        <a href="${appBasePath}/Product/Edit/${data}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i> Edit
                        </a>

                        <a href="${appBasePath}/Product/Delete/${data}" class="btn btn-danger btn-sm">
                            <i class="fa-solid fa-trash"></i> Delete
                        </a>
                    `;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

