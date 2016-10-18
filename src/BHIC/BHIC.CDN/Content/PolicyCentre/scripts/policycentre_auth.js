function checkForSession() {
    $.ajax({
        url: baseUrl + "Login/IsPCSessionAlive", success: function (result) {
            if (!result.success) {
                window.location.href = baseUrl;
            }
        }
    });
}

$(document).ready(function () {
    //To delete the cookie of purchase path if session is time out.    
    setInterval(function () {
        checkForSession();
    }, (sessionTimeOut * 60 * 1000) + 15000);
});

function scrollTop() {
    $('html, body').animate({ scrollTop: 0 }, 800);
}