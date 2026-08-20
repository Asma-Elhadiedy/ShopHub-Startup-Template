
$(function () {
    dataTable = initializeDataTable("mytable", datatableOptions);

    const params = new URLSearchParams(window.location.search);
    let paymentStatus = params.get('redirect_status');
    let orderId = params.get('orderId');

    if (paymentStatus && paymentStatus === "succeeded") {
        $.post(`${appBasePath}/Customer/Order/ConfirmedPayment/${orderId}`)
            .done((success) => {
                successMessage(success);
                dataTable.ajax.reload();
            }).fail((error) => {

            });
    }

    updateNavBadge();

});

const datatableOptions = {
    ajax: {
        url: `${appBasePath}/Customer/Order/GetData`,
        type: "POST",
        dataSrc: "data"
    },
    columns: [
        {
            title: "Order Date",
            data: "orderDate",
            render: function (data) {
                return new Date(data).toLocaleString("en-GB");
            }
        },
        {
            title: "Total Price",
            data: "totalPrice"
        },
        {
            title: "Payment Method",
            data: "paymentMethod"
        },
        {
            title: "Order Status",
            data: "orderStatus"
        },
        {
            title: "Delivery Date",
            data: "deliveryDate",
            render: function (data) {
                if (data)
                    return new Date(data).toLocaleString("en-GB");
                return "-";
            }
        }
    ],
    autoWidth: false,
    scrollX: true
};

