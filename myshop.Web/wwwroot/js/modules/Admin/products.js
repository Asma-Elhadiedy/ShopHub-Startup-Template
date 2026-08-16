
$(function () {
    $("#js-productsSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Admin/Product/GetData`,
        type: "POST",
        data: function (data) {
            // console.log(data);
            return data;
        },
        // dataSrc: function (json) {
        //     console.log(json);
        //     return json.data;
        // }
    },
    columns: [
        {
            title:
                "Name",
            data: "name"
        },
        {
            title: "Description",
            data: "description"
        },
        {
            title: "Price",
            data: "price"
        },
        {
            title: "Category",
            data: "categoryName"
        },
        {
            title: "Actions",
            data: "id",
            render: function (data) {
                return `
                        <a href="${appBasePath}/Admin/Product/Edit/${data}" class="btn btn-success btn-sm">
                            <i class="fas fa-pen"></i> Edit
                        </a>

                        <a href="${appBasePath}/Admin/Product/Delete/${data}" class="btn btn-danger btn-sm">
                            <i class="fas fa-trash"></i> Delete
                        </a>
                    `;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

