$( function () {
    if ( $( 'input.tag' ).length > 0 ) {
        $( 'input.tag' ).each(
            function ( i, obj ) {
                $( obj ).selectize( {
                    plugins: ['remove_button'],
                    persist: false,
                    createOnBlur: true,
                    create: true
                } );
            } );

        $( 'input.tagify' ).each(
            function ( i, obj ) {
                var url = window.location.pathname.split( "/" );
                var controller = url[2];
                var action = url[3];
                var tagify = new Tagify( obj, {
                    mode: "input",
                    //whitelist: ["first option", "second option", "third option"],
                    enabled: 1,
                    enforceWhitelist: true,
                    keepInvalidTags: false,   // do not auto-remove invalid tags
                    dropdown: {
                        closeOnSelect: false,
                        enabled: 0
                    }
                } );
                if ( obj.id.toLowerCase() === "cityviewmodel_value" ) {
                    initTagify( tagify, "Cities", "create" );
                    if ( controller.toLowerCase() == "packages" && action.toLowerCase() == "edit" ) {
                        initTagify( tagify, "GetPackageCities", "Edit" );

                    } else if ( controller.toLowerCase() == "daytours" && action.toLowerCase() == "edit" ) {
                        initTagify( tagify, "GetDayTourCities", "Edit" );

                    }


                }

                if ( obj.id.toLowerCase() === "journeytypeviewmodel_value" ) {
                    initTagify( tagify, "JourneyType", "create" );
                    if ( controller.toLowerCase() == "packages" && action.toLowerCase() == "edit" ) {
                        initTagify( tagify, "GetPackageJourneyType", "Edit" );

                    } else if ( controller.toLowerCase() == "daytours" && action.toLowerCase() == "edit" ) {
                        initTagify( tagify, "GetDayTourJourneyType", "Edit" );

                    }

                }

            } );

    }
} );
function initTagify( tagify, tagifyAction, getAction ) {
    if ( getAction == "create" ) {
        var url = "/admin/Tagify/" + tagifyAction;
        $.ajax( {
            type: "POST",
            url: url,
            success: function ( data ) {
                tagify.settings.whitelist = data;
            }
        } );
    } else {
        if ( $( '#propId' ).val() != null ) {
            var url = "/admin/Tagify/" + tagifyAction + "?id=" + $( '#propId' ).val();
            $.ajax( {
                type: "POST",
                url: url,
                success: function ( data ) {
                    tagify.addTags( data );

                }
            } );
        }

    }

}
$( function () {
    $( "#menu" ).metisMenu();
    $( ".main-content" ).css( 'min-height', $( window ).height() - 120 );
    $( '.slim-scroll' ).slimScroll( {
        height: "auto"
    } );

    $( window ).resize( function () {
        $( ".main-content" ).css( 'min-height', $( window ).height() - 120 );
        $( '.slim-scroll' ).slimScroll( {
            height: "auto"
        } );
    } );

    $( 'img[data-toggle="tooltip"]' ).tooltip( {
        animated: 'fade',
        placement: 'bottom',
        html: true
    } );

    //console.log($(".breadcrumb-item.main>a").length);

    $( ".metismenu span:contains('" + $( ".breadcrumb-item.active" ).text() + "')" ).parent().addClass( 'active' )
        .parent().parent( '.my-second-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );

    $( ".metismenu span:contains('" + $( ".breadcrumb-item.active" ).text() + "')" ).parent().addClass( 'active' )
        .parent().parent( '.my-third-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );


    $( ".metismenu span:contains('" + $( ".breadcrumb-item.active" ).text() + "')" ).parent().addClass( 'active' )
        .parent().parent( '.custom-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );

    if ( $( ".breadcrumb-item.main>a" ).length > 0 ) {
        $( ".metismenu span:contains('" + $( ".breadcrumb-item.main>a" ).text() + "')" ).parent().addClass( 'active' )
            .parent().parent( '.my-second-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );

        $( ".metismenu span:contains('" + $( ".breadcrumb-item.main>a" ).text() + "')" ).parent().addClass( 'active' )
            .parent().parent( '.my-third-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );

        $( ".metismenu span:contains('" + $( ".breadcrumb-item.main>a" ).text() + "')" ).parent().addClass( 'active' )
            .parent().parent( '.custom-level' ).addClass( 'in' ).parent().first( '.has-arrow.link' ).addClass( 'active' ).attr( 'aria-expanded', true );
    }

    loadDate();


    if ( $( '.birthdate' ).length > 0 ) {

        $( '.birthdate' ).bootstrapBirthday( {
            text: {
                dateFormat: 'littleEndian',
                year: "Year",
                month: "Month",
                day: "Day",
                months: {
                    short: [
                        "Jan",
                        "Feb",
                        "Mar",
                        "Apr",
                        "May",
                        "Jun",
                        "Jul",
                        "Aug",
                        "Sep",
                        "Oct",
                        "Nov",
                        "Dec"
                    ],
                    long: [
                        "January",
                        "February",
                        "March",
                        "April",
                        "May",
                        "June",
                        "July",
                        "August",
                        "September",
                        "October",
                        "November",
                        "December"
                    ]
                }
            }
        } );
    }


    $( ".open-close" ).click( function () {
        $( ".sidebar" ).toggleClass( "my_nav" );
    } );
    $( ".openmenu" ).click( function () {
        $( ".page-wrapper" ).toggleClass( "showmenu" );
    } );

    /*menu toggle*/
    $( ".open-close" ).on( 'click',
        function () {
            if ( $( "body" ).hasClass( "content-wrapper" ) ) {
                $( "body" ).trigger( "resize" );
                $( "body" ).removeClass( "content-wrapper" );
                $( ".sidebar-nav, .slimScrollDiv" ).css( "overflow", "hidden" ).parent().css( "overflow", "visible" );
                $( ".open-close i" ).removeClass( "icon-arrow-left-circle" );
                $( ".main-header" ).removeClass( "small-width" );
                $( ".spark-logo" ).removeClass( "small-width-size" );
                $( "main" ).removeClass( "small-width" );
                $( ".big-logo" ).show();
                $( ".small-logo" ).hide();
                $( ".logo span" ).show();
                setTimeout( function () {
                    window.dispatchEvent( new Event( 'resize' ) );
                },
                    200 );
            } else {
                $( "body" ).trigger( "resize" );
                $( "body" ).addClass( "content-wrapper" );
                $( ".sidebar-nav, .slimScrollDiv" ).css( "overflow-x", "visible" ).parent().css( "overflow", "visible" );
                $( ".open-close i" ).removeClass( "icon-arrow-left-circle" );
                $( ".spark-logo" ).addClass( "small-width-size" );
                $( ".main-header" ).addClass( "small-width" );
                $( "main" ).addClass( "small-width" );
                $( ".big-logo" ).hide();
                $( ".small-logo" ).show();
                $( ".logo span" ).hide();
                setTimeout( function () {
                    window.dispatchEvent( new Event( 'resize' ) );
                },
                    200 );
            }
        } );
    /*end menu toggle~*/
} );

function loadDate() {
    $( '.datetimepicker,.datetimepicker-input' ).datetimepicker( {
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-angle-left',
            next: 'fa fa-angle-right',
            today: 'fa fa-circle-thin',
            clear: 'fa fa-trash',
            close: 'fa fa-minus'
        }
    } );
}
function formatDate( d ) {
    var day = String( d.getDate() );
    if ( day.length === 1 ) {
        day = '0' + day;
    }
    var month = String( ( d.getMonth() + 1 ) )
    //add leading zero if month is is single digit
    if ( month.length === 1 ) {
        month = '0' + month;
    }
    return day + "-" + month + "-" + d.getFullYear();
}
function loadSpecificDates( startDate, endDate ) {
    var getDisableDays = $( "#selected_Dates" ).data( "selected" );
    var disableDays = "";
    if ( getDisableDays != null && getDisableDays != "" ) {
        disableDays = getDisableDays.split( ',' );
    }

    $( '.SpecificDates,.SpecificDates-input' ).datetimepicker( {
        minDate: startDate,
        maxDate: endDate,
        disabledDates: disableDays,
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-angle-left',
            next: 'fa fa-angle-right',
            today: 'fa fa-circle-thin',
            clear: 'fa fa-trash',
            close: 'fa fa-minus'
        }
    } );
}

function loadAvailableDates( startDate, endDate ) {
    var getAvailableDays = $( "#Date" ).data( "days" );
    var availableDays = "";
    if ( getAvailableDays != null && getAvailableDays != "" ) {
        availableDays = getAvailableDays.split( ',' );
    }



    if ( availableDays != null && availableDays.length > 0 ) {

        //var date = ( availableDays[0] >= new Date.now() ) ? startDate : startDate;
        $( '.datetimepicker,.datetimepicker-input' ).datetimepicker( {
            minDate: availableDays[0],
            maxDate: endDate,
            enabledDates: availableDays,
            icons: {
                time: 'fa fa-clock-o',
                date: 'fa fa-calendar',
                up: 'fa fa-chevron-up',
                down: 'fa fa-chevron-down',
                previous: 'fa fa-angle-left',
                next: 'fa fa-angle-right',
                today: 'fa fa-circle-thin',
                clear: 'fa fa-trash',
                close: 'fa fa-minus'
            }
        } );
    } else {

        $( '.datetimepicker,.datetimepicker-input' ).datetimepicker( {
            minDate: startDate,
            maxDate: endDate,
            icons: {
                time: 'fa fa-clock-o',
                date: 'fa fa-calendar',
                up: 'fa fa-chevron-up',
                down: 'fa fa-chevron-down',
                previous: 'fa fa-angle-left',
                next: 'fa fa-angle-right',
                today: 'fa fa-circle-thin',
                clear: 'fa fa-trash',
                close: 'fa fa-minus'
            }
        } );
    }
}

//Add_Room_Function

$( "#add_Rooms" ).click( function () {
    btnAddRoom();
} );

function btnAddRoom( roomIndex, adult, underOneYear, betweenTwoToFiveYears, betweenSixToElevenYears, paymentId ) {
    var count = ( roomIndex != null && roomIndex > 0 ) ? roomIndex : $( "[id*=RoomParent_]" ).length + 1;
    var html = addRoom( count, roomIndex, adult, underOneYear, betweenTwoToFiveYears, betweenSixToElevenYears, paymentId );

    $( "#SeasonPaymen" ).append( html );
}
function removeRoom( id ) {
    $( id ).remove();
}
var priceid = 0;
function addRoom( id, roomIndex, adult, underOneYear, betweenTwoToFiveYears, betweenSixToElevenYears, paymentId ) {
    var RoomTypeIdOptions = $( "#PackageSeasonPaymentId option" );

    var options = "<option value='' selected disabled>Choose Room...</option>";
    console.log( RoomTypeIdOptions.length )
    if ( RoomTypeIdOptions.length < 1 ) {
        if ( document.getElementById( "PackageSeasonPayment_1" ) != null ) {
            RoomTypeIdOptions = $( document.getElementById( 'PackageSeasonPayment_1' ).options );
        }
    }

    if ( roomIndex > 0 && roomIndex != null ) {
        id = roomIndex;

    }
    priceid = paymentId;
    RoomTypeIdOptions.each( function ( index, value ) {

        if ( index > 0 ) {
            var selected = ( $( this ).attr( "value" ) != null && $( this ).attr( "value" ) == paymentId ? " selected " : "" );
            var disabled = $( this ).attr( "disabled" ) != null ? " disabled " : "";
            var person = $( this ).data( "person" ),
                adult = $( this ).data( "adult" ),
                price = $( this ).data( "price" ),
                underOneYearQuantity = $( this ).data( "underoneyearquantity" ),
                underOneYearPrice = $( this ).data( "underoneyearprice" ),
                betweenTwoToFiveYearsQuantity = $( this ).data( "betweentwotofiveyearsquantity" ),
                betweenTwoToFiveYearsPrice = $( this ).data( "betweentwotofiveyearsprice" ),
                betweenSixToElevenYearsQuantity = $( this ).data( "betweensixtoelevenyearsquantity" ),
                betweensixtoelevenyearsprice = $( this ).data( "betweensixtoelevenyearsprice" );

            var option = `<option 
                                            data-person="`+ person + `"
                                            data-adult="`+ adult + `"
                                            data-price="`+ price + `"
                                            data-underOneYearQuantity="`+ underOneYearQuantity + `"
                                            data-underOneYearPrice="`+ underOneYearPrice + `"
                                            data-betweenTwoToFiveYearsQuantity="`+ betweenTwoToFiveYearsQuantity + `"
                                            data-betweenTwoToFiveYearsPrice="`+ betweenTwoToFiveYearsPrice + `"
                                            data-betweenSixToElevenYearsQuantity="`+ betweenSixToElevenYearsQuantity + `"
                                            data-betweensixtoelevenyearsprice="`+ betweensixtoelevenyearsprice + `"
                                            value="` + $( this ).attr( "value" ) + `"  ` + selected + " " + disabled + `>` + $( this ).text() + `</option>`;
            options += option;
        }
    } );

    var html = `
                <div id="RoomParent_`+ id + `" class="room  form-group  p-5 rounded mb-2">
                    <h2 > Room `+ id + ` `
        + ( id == 1 ? `` : `<span onclick="removeRoom('#RoomParent_` + id + `')"><i class="btn btn-danger fa fa-close "style="font-size: 16px;padding: 5px;"></i></span>` ) + `</h2>

                    <div class="form-group row">
                        <label asp-for="PackageSeasonPayment_`+ id + `" class="col-xl-2 col-lg-3 col-form-label"></label>
                        <div class="col-xl-10 col-lg-9">
                            <select class="form-control" name="PackageSeasonPayment_`+ id + `" id="PackageSeasonPayment_` + id + `">
                                `+ options + `
                            </select>
                        </div>
                    </div>

                    <div class="form-group row d-none" id="Room_Adult_`+ id + `">
                        <label for="Adult_`+ id + `" class="col-xl-2 col-lg-3 col-form-label">Adult</label>
                        <div class="col-xl-10 col-lg-9">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text children-number-decrement input-number-decrement" onclick="decrement('#Adult_`+ id + `')"><i class="fa fa-minus"></i></span>
                                </div>
                                <input name="Adult_`+ id + `" id="Adult_` + id + `" class="children-number  input-number  form-control" required readonly min="1" max="99" value="` + ( adult > 0 ? adult : 1 ) + `">
                                <div class="input-group-append">
                                    <span class="input-group-text children-number-increment  input-number-increment" onclick="increment('#Adult_`+ id + `')"><i class="fa fa-plus"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row d-none" id="Room_UnderOneYear_`+ id + `">
                        <label for="UnderOneYear_`+ id + `" class="col-xl-2 col-lg-3 col-form-label">( <= 1)</label>
                        <div class="col-xl-10 col-lg-9">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text children-number-decrement input-number-decrement" onclick="decrement('#UnderOneYear_`+ id + `')"><i class="fa fa-minus"></i></span>
                                </div>
                                <input name="UnderOneYear_`+ id + `" id="UnderOneYear_` + id + `" class="children-number  input-number  form-control" readonly min="0" max="99" value="` + ( underOneYear > 0 ? underOneYear : 0 ) + `">
                                <div class="input-group-append">
                                    <span class="input-group-text children-number-increment  input-number-increment" onclick="increment('#UnderOneYear_`+ id + `')"><i class="fa fa-plus"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row d-none" id="Room_BetweenTwoToFiveYears_`+ id + `">
                        <label for="BetweenTwoToFiveYears_`+ id + `" class="col-xl-2 col-lg-3 col-form-label">(2 - 5)</label>
                        <div class="col-xl-10 col-lg-9">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text children-number-decrement input-number-decrement" onclick="decrement('#BetweenTwoToFiveYears_`+ id + `')"><i class="fa fa-minus"></i></span>
                                </div>
                                <input name="BetweenTwoToFiveYears_`+ id + `" id="BetweenTwoToFiveYears_` + id + `" class="children-number  input-number  form-control" readonly min="0" max="99" value="` + ( betweenTwoToFiveYears > 0 ? betweenTwoToFiveYears : 0 ) + `">
                                <div class="input-group-append">
                                    <span class="input-group-text children-number-increment  input-number-increment" onclick="increment('#BetweenTwoToFiveYears_`+ id + `')"><i class="fa fa-plus"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row d-none" id="Room_BetweenSixToElevenYears_`+ id + `">
                        <label for="BetweenSixToElevenYears_`+ id + `" class="col-xl-2 col-lg-3 col-form-label">(6 - 11)</label>
                        <div class="col-xl-10 col-lg-9">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text children-number-decrement input-number-decrement" onclick="decrement('#BetweenSixToElevenYears_`+ id + `')"><i class="fa fa-minus"></i></span>
                                </div>
                                <input name="BetweenSixToElevenYears_`+ id + `" id="BetweenSixToElevenYears_` + id + `" class="children-number  input-number  form-control" readonly min="0" max="99" value="` + ( betweenSixToElevenYears > 0 ? betweenSixToElevenYears : 0 ) + `">
                                <div class="input-group-append">
                                    <span class="input-group-text children-number-increment  input-number-increment" onclick="increment('#BetweenSixToElevenYears_`+ id + `')"><i class="fa fa-plus"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                `;
    return html;
}


$( document ).on( "change blur", "[id*=PackageSeasonPayment_]", function () {
    CheckOnRoom( this );

} );
function CheckOnRoom( $this ) {
    if ( $( $this ).val() != null && $( $this ).val() != "" ) {
        var option = $( $this ).find( "option[value=" + $( $this ).val() + "]" );
        var thisAdult = option.data( "adult" );
        var thisUnderOneYear = option.data( "underoneyearquantity" );
        var thisBetweenTwoToFiveYears = option.data( "betweentwotofiveyearsquantity" );
        var thisBetweenSixToElevenYears = option.data( "betweensixtoelevenyearsquantity" );
        if ( thisAdult > 0 ) {
            var adult = $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_Adult_]" );
            adult.removeClass( "d-none" );
            adult.find( "input" ).attr( "max", thisAdult )
        } else {
            $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_Adult_]" ).addClass( "d-none" );
        }
        if ( thisUnderOneYear > 0 ) {
            var underOneYear = $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_UnderOneYear]" )
            underOneYear.removeClass( "d-none" );
            underOneYear.find( "input" ).attr( "max", thisUnderOneYear )
        } else {
            $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_UnderOneYear]" ).addClass( "d-none" );
        }
        if ( thisBetweenTwoToFiveYears > 0 ) {
            var betweenTwoToFiveYears = $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenTwoToFiveYears]" )
            betweenTwoToFiveYears.removeClass( "d-none" );
            betweenTwoToFiveYears.find( "input" ).attr( "max", thisBetweenTwoToFiveYears )
        } else {
            $( this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenTwoToFiveYears]" ).addClass( "d-none" );
        }
        if ( thisBetweenSixToElevenYears > 0 ) {
            var betweenSixToElevenYears = $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenSixToElevenYears]" )
            betweenSixToElevenYears.removeClass( "d-none" );
            betweenSixToElevenYears.find( "input" ).attr( "max", thisBetweenSixToElevenYears )
        } else {
            $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenSixToElevenYears]" ).addClass( "d-none" );
        }
        $( "[id*=Field_]" ).removeClass( "d-none" );

    } else {
        $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_Adult_]" ).addClass( "d-none" );
        $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_UnderOneYear_]" ).addClass( "d-none" );
        $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenSixToElevenYears_]" ).addClass( "d-none" )
        $( $this ).parents( "[id*=RoomParent_]" ).find( "[id*=Room_BetweenTwoToFiveYears_]" ).addClass( "d-none" )
        $( "[id*=Field_]" ).addClass( "d-none" )
    }
}

//function checkOnRoom( adult, underOneYearQuantity, betweenTwoToFiveYearsQuantity, betweenSixToElevenYearsQuantity, rootId ) {

//    if ( adult > 0 ) {
//        var adultInput = $( "#" + rootId + " [id*=Adult]" );
//        adultInput.each( function () {
//            $( this ).val( 1 );
//        } );
//        adultInput.parents( ".selectRoom" ).removeClass( "d-none" );
//        adultInput.attr( "max", adult );
//    } else {
//        $( "#" + rootId + " [id*=Adult]" ).each( function () {
//            $( this ).val( 0 );
//        } );
//        $( "#" + rootId + " [id*=Adult]" ).parents( ".selectRoom" ).addClass( "d-none" );
//    }
//    if ( underOneYearQuantity > 0 ) {
//        var underSixInput = $( "#" + rootId + " [id*=UnderOneYearQuantity]" );
//        underSixInput.each( function () {
//            $( this ).val( 0 );
//        } );
//        underSixInput.parents( ".selectRoom" ).removeClass( "d-none" );
//        underSixInput.attr( "max", underOneYearQuantity );
//    } else {
//        $( "#" + rootId + " [id*=UnderOneYearQuantity]" ).each( function () {
//            $( this ).val( 0 );
//        } );
//        $( "#" + rootId + " [id*=UnderOneYearQuantity]" ).parents( ".selectRoom" ).addClass( "d-none" );
//    }
//    if ( betweenTwoToFiveYearsQuantity > 0 ) {
//        var underSixInput2 = $( "#" + rootId + " [id*=BetweenTwoToFiveYearsQuantity]" );
//        underSixInput2.each( function () {
//            $( this ).val( 0 );
//        } );
//        underSixInput2.parents( ".selectRoom" ).removeClass( "d-none" );
//        underSixInput2.attr( "max", betweenTwoToFiveYearsQuantity );
//    } else {
//        $( "#" + rootId + " [id*=BetweenTwoToFiveYearsQuantity]" ).each( function () {
//            $( this ).val( 0 );
//        } );
//        $( "#" + rootId + " [id*=BetweenTwoToFiveYearsQuantity]" ).parents( ".selectRoom" ).addClass( "d-none" );
//    }
//    if ( betweenSixToElevenYearsQuantity > 0 ) {
//        var underTwelveInput = $( "#" + rootId + " [id*=BetweenSixToElevenYearsQuantity]" );
//        underTwelveInput.each( function () {
//            $( this ).val( 0 );
//        } );
//        underTwelveInput.parents( ".selectRoom" ).removeClass( "d-none" );
//        underTwelveInput.attr( "max", betweenSixToElevenYearsQuantity );
//    } else {
//        $( "#" + rootId + " [id*=BetweenSixToElevenYearsQuantity]" ).each( function () {
//            $( this ).val( 0 );
//        } );
//        $( "#" + rootId + " [id*=BetweenSixToElevenYearsQuantity]" ).parents( ".selectRoom" ).addClass( "d-none" );
//    }
//}
var getClosest = function ( elem, selector ) {

    // Element.matches() polyfill
    if ( !Element.prototype.matches ) {
        Element.prototype.matches =
            Element.prototype.matchesSelector ||
            Element.prototype.mozMatchesSelector ||
            Element.prototype.msMatchesSelector ||
            Element.prototype.oMatchesSelector ||
            Element.prototype.webkitMatchesSelector ||
            function ( s ) {
                var matches = ( this.document || this.ownerDocument ).querySelectorAll( s ),
                    i = matches.length;
                while ( --i >= 0 && matches.item( i ) !== this ) { }
                return i > -1;
            };
    }

    // Get the closest matching element
    for ( ; elem && elem !== document; elem = elem.parentNode ) {
        if ( elem.matches( selector ) ) return elem;
    }
    return null;

};