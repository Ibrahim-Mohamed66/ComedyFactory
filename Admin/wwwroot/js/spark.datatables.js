var template = "<'d-flex justify-content-between align-items-center'fB<'btn-create'>>" +
    "<'overflow-x slim-scroll-w'tr>" +
    "<'d-flex justify-content-between align-items-center'lpi>";

var template2 = "<'row'<'col-sm-6'f><'col-sm-6'B>>" +
    "<'row'<'col-sm-12'<'table-responsive'tr>>>" +
    "<'row'<'col-sm-4 text-left'p><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>";

$( function () {
    $( 'table.datatable' ).each(
        function ( i, obj ) {
            var sort = true;
            var filename = "excel";
            var language = "";
            if ( obj.hasAttribute( 'data-sort' ) === true ) {
                sort = $( obj ).attr( 'data-sort' );
            }
            if ( obj.hasAttribute( 'data-filename' ) === true ) {
                filename = $( obj ).attr( 'data-filename' );
            }
            if ( obj.hasAttribute( 'data-language' ) === true ) {
                language = $( obj ).attr( 'data-language' );
            }
            var txtOrder = ( language === 'ar' ) ? 'الترتيب' : 'Order';
            var txtId = ( language === 'ar' ) ? 'الكود' : 'Id';
            var txtDate = "Created Date";

            var dt = $( obj ).DataTable( {
                "dom": template,
                "pagingType": "simple_numbers",
                "oLanguage": ( language === 'ar' ) ? {
                    "sProcessing": "جارٍ التحميل...",
                    "sLengthMenu": "أظهر _MENU_ مدخلات",
                    "sZeroRecords": "لم يعثر على أية سجلات",
                    "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
                    "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
                    "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
                    "sInfoPostFix": "",
                    "sSearch": "ابحث:",
                    "sUrl": "",
                    "oPaginate": {
                        "sFirst": "الأول",
                        "sPrevious": "السابق",
                        "sNext": "التالي",
                        "sLast": "الأخير"
                    }
                } : {},
                buttons: [{ extend: 'excelHtml5', filename: filename, className: 'btn my_btn btn-sm', exportOptions: { columns: [$( 'thead th:not(.noexport):not(:empty)' )] }, text: ( language === 'ar' ) ? 'إكسيل' : 'excel' },
                { extend: 'print', className: 'btn my_btn btn-sm', exportOptions: { columns: [$( 'thead th:not(.noexport):not(:empty)' )] }, text: ( language === 'ar' ) ? 'طباعة' : 'print' }]
            } );

            if ( sort ) {

                var columns = [];
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtOrder + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtOrder + ')' )[0] )[0], 'asc'] );
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtId + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtId + ')' )[0] )[0], 'desc'] );
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtDate + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtDate + ')' )[0] )[0], 'desc'] );

                dt.order( columns ).draw();
            } else {
                dt.order( [] ).draw();
            }
        } );


    $( 'table#datatable' ).each(
        function ( i, obj ) {
            var sort = true;
            var filename = "excel";
            var language = "";
            if ( obj.hasAttribute( 'data-sort' ) === true ) {
                sort = $( obj ).attr( 'data-sort' );
            }
            if ( obj.hasAttribute( 'data-filename' ) === true ) {
                filename = $( obj ).attr( 'data-filename' );
            }
            if ( obj.hasAttribute( 'data-language' ) === true ) {
                language = $( obj ).attr( 'data-language' );
            }
            var txtOrder = "Posted Date";
            var txtDate = "Created Date";
            var txtRegisterDate = "Register Date";
            //(language === 'ar') ? 'الترتيب' : 'Order';
            var txtId = ( language === 'ar' ) ? 'الكود' : 'Id';

            var dt = $( obj ).DataTable( {
                "dom": template,
                "pagingType": "simple_numbers",
                "oLanguage": ( language === 'ar' ) ? {
                    "sProcessing": "جارٍ التحميل...",
                    "sLengthMenu": "أظهر _MENU_ مدخلات",
                    "sZeroRecords": "لم يعثر على أية سجلات",
                    "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
                    "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
                    "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
                    "sInfoPostFix": "",
                    "sSearch": "ابحث:",
                    "sUrl": "",
                    "oPaginate": {
                        "sFirst": "الأول",
                        "sPrevious": "السابق",
                        "sNext": "التالي",
                        "sLast": "الأخير"
                    }
                } : {},
                buttons: [{ extend: 'excelHtml5', filename: filename, className: 'btn my_btn btn-sm', exportOptions: { columns: [$( 'thead th:not(.noexport):not(:empty)' )] }, text: ( language === 'ar' ) ? 'إكسيل' : 'excel' },
                { extend: 'print', className: 'btn my_btn btn-sm', exportOptions: { columns: [$( 'thead th:not(.noexport):not(:empty)' )] }, text: ( language === 'ar' ) ? 'طباعة' : 'print' }]
            } );

            //if (sort) {
            //    var columns = [];
            //    if ($('th:contains(' + txtOrder + ')').length > 0) columns.push([dt.column($('th:contains(' + txtOrder + ')')[0])[0], 'asc']);
            //    if ($('th:contains(' + txtId + ')').length > 0) columns.push([dt.column($('th:contains(' + txtId + ')')[0])[0], 'desc']);
            //    dt.order(columns).draw();
            //} else {
            //    dt.order([]).draw();
            //}
            if ( sort ) {
                var columns = [];
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtRegisterDate + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtRegisterDate + ')' )[0] )[0], 'desc'] );
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtOrder + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtOrder + ')' )[0] )[0], 'asc'] );
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtId + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtId + ')' )[0] )[0], 'desc'] );
                if ( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtDate + ')' ).length > 0 ) columns.push( [dt.column( $( '#' + $( obj ).attr( 'id' ) + ' th:contains(' + txtDate + ')' )[0] )[0], 'asc'] );
                dt.order( columns ).draw();
            } else {
                dt.order( [] ).draw();
            }
        } );
    if ( $( ".btn.my_btn.btn-create" ).length > 0 ) {
        $( "div.btn-create" ).html( $( ".btn.my_btn.btn-create" ) );
    }
} );