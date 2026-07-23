
$(function () {
    $("#js-ordersSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    // ajax: {
    //     url: `${appBasePath}/Admin/Order/GetData`,
    //     type: "GET",
    //     dataSrc: "data"
    // },
    columns: [
        {
            title: "Total Price",
            data: "totalPrice"
        },
        {
            title: "Actions",
            data: "id",
            render: function (data) {
                return `
                        <a href="${appBasePath}/Admin/Order/Edit/${data}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i> Edit
                        </a>

                        <a href="${appBasePath}/Admin/Order/Delete/${data}" class="btn btn-danger btn-sm">
                            <i class="fa-solid fa-trash"></i> Delete
                        </a>
                    `;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

