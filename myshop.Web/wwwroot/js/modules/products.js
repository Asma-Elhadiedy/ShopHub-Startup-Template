$(function () {

    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Product/GetData`,
        type: "GET",
        dataSrc: "data"
    },
    columns: [
        { data: "name" },
        { data: "description" },
        { data: "price" },
        { data: "categoryName" },
        {
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

