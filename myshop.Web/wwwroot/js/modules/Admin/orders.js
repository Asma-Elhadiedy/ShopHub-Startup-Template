
$(function () {
    $("#js-ordersSidebar").addClass("menu-open");
    dataTable = initializeDataTable("mytable", datatableOptions);
});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Admin/Order/GetData`,
        type: "POST",
        dataSrc: "data"
    },
    columns: [
        {
            title: "Order Id",
            data: "id"
        },
        {
            title: "Total Price",
            data: "totalPrice"
        },
        {
            title: "Order Status",
            data: "orderStatus"
        },
        {
            title: "Payment Status",
            data: "paymentStatus"
        },
        {
            title: "Payment Method",
            data: "paymentMethod"
        },
        {
            title: "Order Date",
            data: "orderDate",
            render: function (data) {
                return new Date(data).toLocaleString("en-GB");
            }
        },
        {
            title: "Actions",
            data: "id",
            render: function (data) {
                return `
                        <a href="${appBasePath}/Admin/Order/Details/${data}" class="btn btn-success btn-sm">
                             <i class="fas fa-pen"></i> View Details
                        </a>`;
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

