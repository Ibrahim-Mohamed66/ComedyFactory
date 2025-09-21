$(() => {
    drawDounat('PaymentGraph', data.paymentMethods);
    drawDounat('paymentStatus', data.paymentStatus);
    drawOrderGraph(data.orders);
    drawSalesGraph(data.orders);


});

function drawDounat(id, data) {

    Morris.Donut({
        element: id,
        data: data.length > 0 ? data : [{ label: "None", value: 0 }],
        colors: ['#5295ab', '#cc3a5e', '#774874', '#cb4764', '#dd7d53'],
        resize: true,
        labelColor: '#878787',
    });
}


function drawSalesGraph(data) {
    Morris.Bar({
        element: "SalesGraph",
        data: data.length > 0 ? data : [{ label: "None", value: 0 }],
        xkey: ["label"],
        ykeys: ["value"],
        labels: ["Sales"],
        barRatio: 0.4,
        xLabelAngle: 35,
        pointSize: 1,
        barOpacity: 1,
        pointStrokeColors: ["#cc3a5e"],
        behaveLikeLine: true,
        grid: false,
        gridTextColor: "#878787",
        hideHover: "auto",
        barColors: ["#cc3a5e"],
        resize: true,
        gridTextFamily: "Montserrat",
    });
}
function drawOrderGraph(data) {
    Morris.Area({
        element: "orderGraph",
        data: data.length > 0 ? data : [{ period: moment(), element1: 0, element2: 0 }],
        xkey: "period",
        ykeys: ["element1", "element2","element3"],
        labels: ["Succeeded  Orders", "Canceled Orders","Refunded Orders"],
        pointSize: 0,
        lineWidth: 0,
        fillOpacity: 1,
        pointStrokeColors: ["#76c880", "#cb4764", "#774874"],
        behaveLikeLine: true,
        grid: false,
        hideHover: "auto",
        lineColors: ["#76c880", "#cb4764", "#774874"],
        resize: true,
        redraw: true,
        smooth: true,
        gridTextColor: "#878787",
        gridTextFamily: "Montserrat",
    });

}




$("#paymentMethodDate").change(function () {
    var dates = SplitDates(this.value);

    $.ajax({
        type: "POST",
        url: "/Home/GetPaymentMethods",
        data: { startDate: dates.startDate, endDate: dates.endDate },
        success: function (data) {
            $('#PaymentGraph').html("");

            drawDounat('PaymentGraph', data);
        }
    });
});

$("#paymentStatusDate").change(function () {
    var dates = SplitDates(this.value);

    $.ajax({
        type: "POST",
        url: "/Home/GetPaymentStatus",
        data: { startDate: dates.startDate, endDate: dates.endDate },
        success: function (data) {
            $('#paymentStatus').html("");

            drawDounat('paymentStatus', data);
        }
    });
});

$("#orderGraphDate").change(function () {
    var dates = SplitDates(this.value);

    $.ajax({
        type: "POST",
        url: "/Home/GetOrderGraphPerMonth",
        data: { startDate: dates.startDate, endDate: dates.endDate },
        success: function (data) {
            $('#orderGraph').html("");
            drawOrderGraph(data)

        }
    });
});

$("#SalesGraphDate").change(function () {

    var dates = SplitDates(this.value);

    $.ajax({
        type: "POST",
        url: "/Home/GetSalesGraph",
        data: { startDate: dates.startDate, endDate: dates.endDate },
        success: function (data) {
            $('#SalesGraph').html("");
            drawSalesGraph(data)

        }
    });

}); $("#totalStatisticsDate").change(function () {

    var dates = SplitDates(this.value);

    $.ajax({
        type: "POST",
        url: "/Home/GetTotalStatistics",
        data: { startDate: dates.startDate, endDate: dates.endDate },
        success: function (data) {
            $('#totalStats').html("");
            $('#totalStats').html(data);

        }
    });
});



function SplitDates(dates) {

    const dateParts = dates.split(' - ');

    var obj = { startDate: dateParts[0], endDate: dateParts[1] };

    return obj;
}

// Order Datatable
$(() => {

    if ($('#order').length !== 0) {
        var template = "<'d-flex justify-content-between align-items-center my-dt-header 'Bf<'btn-clean'>>" +
            "<'overflow-x slim-scroll-w'tr>" +
            "<'d-flex justify-content-between align-items-center my-dt-footer'lpi>";

        var table = $('#order').DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: false,
            deferRender: true,
            lengthMenu: [10, 50, 100, 1000],
            order: [[0, "desc"]],
            dom: template,
            buttons: [],
            ajax: {
                type: "POST",
                url: `/Orders/LoadTable?status=${$("#PaymentStatus").val()}&method=${$("#PaymentMethod").val()}&type=${$("#CartType").val()}`,
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    let additionalValues = [];
                    data.AdditionalValues = additionalValues;

                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Id",
                    title: "OrderId",
                    name: "eq",
                },
                {
                    data: "PaymentStatus",
                    title: "PaymentStatus",
                    name: "co",
                    render: function (data, type, row) {
                        if (data == "Canceled")
                            return `<div class="badge badge-danger">` + data + `</div>`;
                        else if (data == "Success")
                            return `<div class="badge badge-success">` + data + `</div>`;

                        else
                            return `<div class="badge badge-secondary">` + data + `</div>`;

                    },
                },
                {
                    data: "PaymentMethod",
                    title: "PaymentMethod",
                    name: "co"
                },

                {
                    data: "PaymentSuccess",
                    title: "PaymentSuccess",
                    render: function (data, type, row) {
                        if (data)
                            return "<input checked disabled type='checkbox' />";
                        else
                            return "<input disabled type='checkbox' />";
                    },
                    name: "co"
                },
                {
                    data: "OrderCanceled",
                    title: "OrderCanceled",
                    render: function (data, type, row) {
                        if (data)
                            return "<input checked disabled type='checkbox' />";
                        else
                            return "<input disabled type='checkbox' />";
                    },
                    name: "co"
                },
                {
                    data: "BookingType",
                    title: "BookingType",
                    name: "co"
                },


                {
                    data: "Tax",
                    title: "Tax",
                    name: "co"
                },

                {
                    data: "SubTotal",
                    title: "SubTotal",
                    name: "co"
                },

                {
                    data: "Total",
                    title: "Total",
                    name: "co"
                },
                {
                    data: "Date",
                    title: "Date",
                    render: function (data, type, row) {
                        if (data)
                            return window.moment(data).format("DD/MM/YYYY hh:mm a");
                        else
                            return null;
                    },
                    name: "co"
                },

                {
                    data: "Time",
                    title: "Time",
                    name: "co"
                },

                {
                    data: "User",
                    title: "User",
                    name: "co"
                },

                {
                    data: "CourtName",
                    title: "CourtName",
                    name: "co"
                },

                {
                    data: "CourtType",
                    title: "CourtType",
                    name: "co"
                },
                {
                    data: "TransactionId",
                    title: "TransactionId",
                    name: "co"
                },

                {
                    data: "IpAddress",
                    title: "IpAddress",
                    name: "co"
                },

                {
                    data: "IpCountry",
                    title: "IpCountry",
                    name: "co"
                },

                {
                    data: "IpCity",
                    title: "IpCity",
                    name: "co"
                },

                {
                    data: "IpLocation",
                    title: "IpLocation",
                    name: "co"
                },




                {
                    data: "CreatedOnUtc",
                    title: "CreatedOnUtc",
                    render: function (data, type, row) {
                        if (data)
                            return window.moment(data).format("DD/MM/YYYY hh:mm a");
                        else
                            return null;
                    },
                    name: "gt",
                },

            ]
        });

        table.columns().every(function (index) {
            $('#order thead tr:last th:eq(' + index + ') input')
                .on('keyup',
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ':visible').search(this.value).draw();
                        }
                    });
            $('#order thead tr:last th:eq(' + index + ') input')
                .on('blur',
                    function () {
                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                    });
            if ($(".btn.my_btn.btn-clean").length > 0) {
                $("div.btn-clean").html($(".btn.my_btn.btn-clean"));
            }
        });

        $("#ordersDataTableDate").change(function () {
            var dates = SplitDates(this.value);

            $('#order').DataTable().ajax.url(`Orders/LoadTable?startDate=` + dates.startDate + `&endDate=` + dates.endDate + ``).load();

        });
    }
});

