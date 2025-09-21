window.getFormData = (formId) => {
    if ($("form" + formId).valid()) {
        var data = JSON.stringify(jQuery("form" + formId).serialize());
        $("#InterestedError").text("");

        return data;
    }
    return null;
};
window.pushError = (inputId, message) => {
    $("#" + inputId).text(message);
};

function showAlert(status, message) {
    Swal.fire({
        icon: status,
        title: status == "error" ? "Oops..." : "",
        html: message,
        timer: 2000,
        timerProgressBar: true,
        showConfirmButton: false,
    });
}

function restForm(formId) {
    if ($("form#" + formId).length > 0) {
        document.getElementById(formId).reset();
        $(
            ".dz-preview.dz-processing.dz-success.dz-complete.dz-image-preview"
        ).remove();
        reloadloadRecaptcha();
    }
}

//Recaptcha generate token with each form load
function reloadloadRecaptcha() {
    if (typeof grecaptcha !== "undefined") {
        grecaptcha.ready(function () {
            grecaptcha
                .execute("6LcUbmUpAAAAACW0vZW8BTchJ76jJc44NLXP_4rs", {
                    action: "homepage",
                })
                .then(function (token) {
                    if ($("#g-recaptcha-response").length > 0) {
                        document.getElementById("g-recaptcha-response").value = token;
                    }
                });
        });
    }
}

function loadFormScripts(formType) {
    reloadloadRecaptcha();
    var btn = document.getElementById("form_btn");
    var form = document.getElementById(formType);
    $(btn).click(function () {
        $(form).updateValidation();
        if ($(form).valid()) {
            var data = jQuery("form" + "#" + formType).serialize();
            $("#InterestedError").text("");

            $.ajax({
                type: "POST",
                url: $(form).attr("action"),
                data: data,
                success: function (e) {
                    if (e.status == "failed") {
                        showAlert(e.status, e.message);
                    } else if (e != null) {
                        restForm(formType);
                        showAlert(e.status, e.message);
                    }
                },
            });
        }
    });
}

$.fn.updateValidation = function () {
    var $this = $(this);
    var form = $this.removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    return $this;
};
$(function (){
    loadFormScripts("formContact");
});