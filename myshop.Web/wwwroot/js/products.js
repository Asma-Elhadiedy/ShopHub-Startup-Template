$(document).ready(function () {

    var table = $("#mytable").DataTable({
        ajax: {
            url: "/Product/GetData",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "name" },
            { data: "description" },
            { data: "price" },
            { data: "categoryName" },
            {
                data: "id",
                render: function (id) {
                    return `
                        <a href="/Product/Edit/${id}" class="btn btn-sm btn-success me-1">
                            <i class="fa-solid fa-pen"></i>
                        </a>
                        <a href="/Product/Delete/${id}" class="btn btn-sm btn-danger">
                            <i class="fa-solid fa-trash"></i>
                        </a>
                    `;
                }
            }
        ],
        autoWidth: false,
        scrollX: true,

        colResize: {
            realtime: true
        },

        createdRow: function (row, data, dataIndex) {
            $('td', row).css('white-space', 'normal');
        }
    });

    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var selectedCategory = $('#categoryFilter').val();
            var category = data[3]; 
            var searchTerm = $('#mytable_filter input').val().toLowerCase();

            var name = data[0].toLowerCase();
            var description = data[1].toLowerCase();
            var cat = category.toLowerCase();

            var matchesSearch = name.includes(searchTerm) || description.includes(searchTerm) || cat.includes(searchTerm);
            var matchesCategory = selectedCategory === "" || category === selectedCategory;

            return matchesSearch && matchesCategory;
        }
    );

    $('#categoryFilter').on('change', function () {
        table.draw();
    });

    $('#mytable_filter input').on('keyup', function () {
        table.draw();
    });

});
