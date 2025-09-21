
$(document).ready(function () {

    $("#calendarView").fullCalendar({
        defaultDate: moment(),
        timeFormat: 'h:mm a',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        visibleRange: function (currentDate) {
            var startDate = moment().startOf("week");
            var endDate = moment().endOf("week");
            return {
                start: startDate,
                end: endDate,
            };
        },
        events: data,
        eventRender: function (event, element) {
            if (event.className === "calendar-avaliable") {
                element.addClass("calendar-avaliable");
            }
            else if (event.className === "calendar-book") {
                element.addClass("disabled calendar-book");
            }
            else if (event.textColor === "calendar-block-to-event") {
                element.addClass("calendar-block-to-event");
            }
            else if (event.className === "calendar-block-to-kill") {
                element.addClass("calendar-block-to-kill");
            }
            else if (event.className === "calendar-block-to-class") {
                element.addClass("calendar-block-to-class");
            }
            else if (event.className === "bulk-blok") {
                element.addClass("disabled bg-secondary");
            }
            element.find('.fc-title').hide();
            element.attr('title', event.title);
            element.attr('data-bs-toggle', 'tooltip');
            element.attr('data-bs-placement', 'top');
        },
        eventClick: async function (event) {
            if (event.reserved) {
                $("#eventModal").modal("show");
                $("#eventModal .modal-title").text(event.title);
                $("#eventModal .modal-body .row .userName").text(event.userName);
                $("#eventModal .modal-body .row .type").text(event.bookedType);
                $("#eventModal .modal-body .row .date").text(event.date.split("T")[0]);
                $("#eventModal .modal-body .row .time").text(moment(event.time, "HH:mm:ss").format("h:mm A") + "-" + moment(event.timeTo, "HH:mm:ss").format("h:mm A"));

            } else if (event.blockType != "BulkBlock") {

                await switchBlockSlot(event).then(async res => {
                    if (res != null && res["status"] == "Success") {

                        $(this)[0].classList.forEach(className => {
                            if (className.includes("calendar")) {
                                $(this)[0].classList.remove(className);
                            }
                        });

                        $(this)[0].removeAttribute('data-bs-original-title');

                        if (res["blockFor"] == "None") {

                            $(this).addClass('calendar-block-to-kill');
                            $(this)[0].setAttribute('data-bs-original-title', 'Blocked Slot To Kill');

                        }
                        else if (res["blockFor"] == "Block For Event") {
                            $(this).addClass('calendar-block-to-event');
                            $(this)[0].setAttribute('data-bs-original-title', 'Event Blocked Slot To Reserve');

                        }
                        else if (res["blockFor"] == "Block For Training") {
                            $(this).addClass('calendar-block-to-class');
                            $(this)[0].setAttribute('data-bs-original-title', 'Class Blocked Slot To Reserve');

                        } else if (res["unblocked"] == true) {
                            $(this).addClass("calendar-avaliable");
                            $(this)[0].setAttribute('data-bs-original-title', 'Available');

                        } else {
                            $(this).addClass('calendar-avaliable');
                            $(this)[0].setAttribute('data-bs-original-title', 'Available');

                        }

                    } else if (res != null && res["status"] == "Failed") {
                        swal({
                            title: "Error!",
                            text: res["msg"],
                            icon: "error",
                            button: "Ok",
                        });
                    }
                });
            }

        },
    });

    $("#listView").fullCalendar({
        defaultView: "agendaWeek",
        defaultDate: moment(),
        timeFormat: 'h:mm a',
        visibleRange: function () {
            var startDate = moment().startOf("week");
            var endDate = moment().endOf("week");
            return {
                start: startDate,
                end: endDate,
            };
        },
        events: data,
        eventRender: function (event, element) {
            if (event.className === "calendar-avaliable") {
                element.addClass("calendar-avaliable");
            } else if (event.className === "calendar-book") {
                element.addClass("disabled calendar-book");
            } else if (event.textColor === "calendar-block-to-event") {
                element.addClass("calendar-block-to-event");
            } else if (event.className === "calendar-block-to-kill") {
                element.addClass("calendar-block-to-kill");
            } else if (event.className === "calendar-block-to-class") {
                element.addClass("calendar-block-to-class");
            } else if (event.className === "bulk-blok") {
                element.addClass("disabled bg-secondary");
            }
            element.find('.fc-title').hide();
            element.attr('title', event.title);
            element.attr('data-bs-toggle', 'tooltip');
            element.attr('data-bs-placement', 'top');

        },
        eventClick: async function (event) {
            console.log(event);

            if (event.reserved) {
                $("#eventModal").modal("show");
                $("#eventModal .modal-title").text(event.title);
                $("#eventModal .modal-body .row .userName").text(event.userName);
                $("#eventModal .modal-body .row .type").text(event.bookedType);
                $("#eventModal .modal-body .row .date").text(event.date.split("T")[0]);
                $("#eventModal .modal-body .row .time").text(moment(event.time, "HH:mm:ss").format("h:mm A") + "-" + moment(event.timeTo, "HH:mm:ss").format("h:mm A"));

            } else if (event.blockType != "BulkBlock") {

                await switchBlockSlot(event).then(async res => {

                    if (res != null && res["status"] == "Success") {

                        $(this)[0].classList.forEach(className => {
                            if (className.includes("calendar")) {
                                $(this)[0].classList.remove(className);
                            }
                        });

                        $(this)[0].removeAttribute('data-bs-original-title');

                        if (res["blockFor"] == "None") {

                            $(this).addClass('calendar-block-to-kill');
                            $(this)[0].setAttribute('data-bs-original-title', 'Blocked Slot To Kill');

                        }
                        else if (res["blockFor"] == "Block For Event") {
                            $(this).addClass('calendar-block-to-event');
                            $(this)[0].setAttribute('data-bs-original-title', 'Event Blocked Slot To Reserve');

                        }
                        else if (res["blockFor"] == "Block For Training") {
                            $(this).addClass('calendar-block-to-class');
                            $(this)[0].setAttribute('data-bs-original-title', 'Class Blocked Slot To Reserve');

                        } else if (res["unblocked"] == true) {
                            $(this).addClass("calendar-avaliable");
                            $(this)[0].setAttribute('data-bs-original-title', 'Available');

                        } else {
                            $(this).addClass('calendar-avaliable');
                            $(this)[0].setAttribute('data-bs-original-title', 'Available');

                        }

                    } else if (res != null && res["status"] == "Failed") {
                        swal({
                            title: "Error!",
                            text: res["msg"],
                            icon: "error",
                            button: "Ok",
                        });
                    }
                });
            }

        },
    });

    $('[data-bs-toggle="tooltip"]').tooltip()
});

// Block & unblock
var allowBlocking = false;
var allowUnBlocking = false;

$("#BlockTo").change(function () {
    var BlockTo = this.value;
    if (BlockTo == "1") {
        $("#BlockFor").removeClass("d-none");
    } else {
        $("#BlockFor").addClass("d-none");
    }
});

$('#btn_block').click(function (e) {

    var BlockTo = $("#BlockTo").val();
    var BlockFor = $("#BlockFor").val();

    $(this).addClass("active");
    $("#btn_unblock").removeClass("active");
    allowBlocking = true;
    allowUnBlocking = false;

    if (BlockTo == "1" && BlockFor != null && BlockFor != "") {
    } else if (BlockTo == "0") {
    } else {
        swal({
            title: "Error!",
            text: "Select Block Type!",
            icon: "error",
            button: "Ok",
        });
    }

});

$('#btn_unblock').click(function (e) {
    $(this).addClass("active");
    $("#btn_block").removeClass("active");
    allowUnBlocking = true;
    allowBlocking = false;
    document.getElementById("BlockTo").value = "";
    document.getElementById("BlockFor").value = "";
    $("#BlockFor").addClass("d-none");

});

async function switchBlockSlot(event) {
    var obj = {
        CourtId: courtId,
        Date: event.date,
        Time: event.time,
        TimeTo: event.timeTo,
        To: event.date,
        BlockFor: $("#BlockFor").val(),
        BlockTo: $("#BlockTo").val(),
    };

    if (obj != null && allowBlocking) {

        const res = await $.ajax({
            type: "POST",
            url: "/BlockTimeSlots/BlockTime",
            data: obj,
        });

        return res;

    }
    if (obj != null && allowUnBlocking) {

        const res = await $.ajax({
            type: "POST",
            url: "/BlockTimeSlots/UnBlockTime",
            data: obj,
        });

        return res;

    }
}