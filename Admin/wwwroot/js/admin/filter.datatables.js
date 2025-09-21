var template = "<'d-flex justify-content-between align-items-center my-dt-header 'fB<'btn-create'>>" +
    "<'table-responsive table-container'tr>" +
    "<'d-flex justify-content-between align-items-center my-dt-footer'lpi>";

var template2 = "<'row'<'col-sm-6'f><'col-sm-6'B>>" +
    "<'row'<'col-sm-12'<'table-responsive'tr>>>" +
    "<'row'<'col-sm-4 text-left'p><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>";

$.fn.dataTable.Api.register('column().searchable()', function () {
    var ctx = this.context[0];
    return ctx.aoColumns[this[0]].bSearchable;
});

$.fn.dataTable.moment('M/D/YYYY HH:mm a');


$(function () {
    $('table.datatablefilter').each(
        function (i, obj) {
            var sort = true;
            var filename = "excel";
            var language = "";
            var pageLength = 10;
            var exportTitle = '';

            if (obj.hasAttribute('data-sort') === true) {
                sort = $(obj).attr('data-sort');
            }
            if (obj.hasAttribute('data-filename') === true) {
                filename = $(obj).attr('data-filename');
            }
            if (obj.hasAttribute('data-language') === true) {
                language = $(obj).attr('data-language');
            }
            if (obj.hasAttribute('data-pageLength') === true) {
                pageLength = parseInt($(obj).attr('data-pageLength'));
            }
            if (obj.hasAttribute('data-reportTitle') === true) {
                exportTitle = $(obj).attr('data-reportTitle');
            }
            var txtOrder = (language === 'ar') ? 'الترتيب' : 'Order';
            var txtId = (language === 'ar') ? 'الكود' : 'Id';
            // Create the DataTable
            var dt = $(obj).DataTable({
                dom: template,
                buttons: [{
                    extend: 'excelHtml5',
                    footer: true,
                    filename: filename,
                    className: 'btn btn-secondary btn-sm',
                    exportOptions: {
                        columns: [$('thead th:not(.noexport):not(:empty)')]
                    },
                    text: (language === 'ar') ? 'إكسيل' : 'excel'
                },
                    {
                        extend: 'print',
                        footer: true,
                        className: 'btn  btn-secondary btn-sm',
                        exportOptions: {
                            columns: [$('thead th:not(.noexport):not(:empty)')]
                        },
                        text: (language === 'ar') ? 'طباعة' : 'print',
                        //title: exportTitle
                    }
                ],
                fixedHeader: true,
                pageLength: pageLength,
                orderCellsTop: true,
                stateSave: true
            });

            // Add filtering
            dt.columns().every(function () {
                var column = this;
                var myColumn = column.header();

                if (!myColumn.hasAttribute('data-item')) {
                    if (this.searchable()) {
                        var that = this;

                        // Create the `select` element                   
                        var select = $('<select><option value=""></option></select>')
                            .appendTo(
                                $('#' + $(obj).attr('id') + ' thead tr:eq(1) td').eq(this.index())
                            )
                            .on('change', function () {
                                that
                                    .search($(this).val(), false, false)
                                    .draw();
                            });
                    }
                    // Add data
                    this
                        .data()
                        .sort()
                        .unique()
                        .each(function (d) {
                            select.append($('<option>' + d + '</option>'));
                        });
                    // Restore state saved values
                    var state = this.state.loaded();
                    if (state) {
                        var val = state.columns[this.index()];
                        select.val(val.search.search);
                    }
                }


            });


            //fix drop down
            $('.table.dataTable').on('order.dt search.dt page.dt', function () {
                var searchVal = $('.dataTables_filter input').val().length;

                setTimeout(function () {

                    if ($(".table-responsive .datatable").length > 0) {

                        if ($(".table-responsive .datatable").height() < 250) {

                            $(document).on('shown.bs.dropdown', '.table-responsive', function (e) {
                                // The .dropdown container
                                var $container = $(e.target);

                                // Find the actual .dropdown-menu
                                var $dropdown = $container.find('.dropdown-menu');
                                if ($dropdown.length) {
                                    // Save a reference to it, so we can find it after we've attached it to the body
                                    $container.data('dropdown-menu', $dropdown);
                                } else {
                                    $dropdown = $container.data('dropdown-menu');
                                }

                                $dropdown.css('top', ($container.offset().top + $container.outerHeight()) + 'px');
                                $dropdown.css('left', $container.offset().left + 'px');
                                $dropdown.css('position', 'absolute');
                                $dropdown.css('display', 'block');
                                $dropdown.appendTo('body');
                            });

                            $(document).on('hide.bs.dropdown', '.table-responsive', function (e) {
                                // Hide the dropdown menu bound to this button
                                $(e.target).data('dropdown-menu').css('display', 'none');
                            });

                        }
                    }
                }, 100);


            });

            if (sort) {
                var columns = [];
                if ($('#' + $(obj).attr('id') + ' th:contains(' + txtOrder + ')').length > 0) columns.push([dt.column($('#' + $(obj).attr('id') + ' th:contains(' + txtOrder + ')')[0])[0], 'asc']);
                if ($('#' + $(obj).attr('id') + ' th:contains(' + txtId + ')').length > 0) columns.push([dt.column($('#' + $(obj).attr('id') + ' th:contains(' + txtId + ')')[0])[0], 'desc']);
                dt.order(columns).draw();
            } else {
                dt.order([]).draw();
            }
        });

    if ($(".btn.btn-create").length > 0) {
        $("div.btn-create").html($(".btn.btn-create"));
    } else {
        $("div.btn-create").css('display', 'none');
    }
    if ($(".dataTables_wrapper").length > 0) {
        // $(".dataTables_wrapper").addClass("table-responsive");
        $(".dataTables_wrapper").removeClass("container-fluid");
    }
    if ($(".pagination").length > 0) {
        $(".pagination").addClass("pagination-sm");
    }
});