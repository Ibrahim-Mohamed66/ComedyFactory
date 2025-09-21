
$(document).ajaxError(function (event, xhr, settings) {
    if (xhr.status === 401) {
        console.warn("401 detected → trying to refresh token...");

        $.ajax({
            url: '/Account/Refresh',
            type: 'POST',
            success: function (data) {
                console.log("Token refreshed successfully");
                $.ajax(settings);
            },
            error: function () {
                console.error("Refresh failed → redirecting to login");
                window.location.href = '/Account/Login';
            }
        });
    }
});
