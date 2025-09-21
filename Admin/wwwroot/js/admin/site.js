function DuplicateRow($url) {
    swal({
        title: "Your Item will be Duplicate!",
        text: "Are you sure to proceed?",
        type: "success",
        buttons: true,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Duplicate My Item!",
        cancelButtonText: "I am not sure!",
        closeOnConfirm: false,
        closeOnCancel: false,
    }).then((isConfirm) => {
        if (isConfirm) {
            $.ajax({
                type: "POST",
                url: $url,
                success: function (data) {
                    if (data == "Duplicated") {
                        swal(
                            "Item Duplicated!",
                            "Your Item is Duplicated!",
                            "success"
                        );
                        setTimeout(function () {
                            location.reload();
                        }, 500);
                    } else {
                        swal("Hurray", "Item is not Duplicated!", "error");
                    }
                },
            });
        } else {
            swal("Hurray", "Item is not Duplicated!", "error");
        }
    });
}


$(function () {
    if ($("input.tag").length > 0) {
        $("input.tag").each(function (i, obj) {
            $(obj).selectize({
                plugins: ["remove_button"],
                persist: false,
                createOnBlur: true,
                create: true,
            });
        });
    }

    $('button[data-toggle="pill"]').on("shown.bs.tab", function (event) {
        console.log("ss");
        $(".dataTable").DataTable().columns.adjust();
    });


});
 