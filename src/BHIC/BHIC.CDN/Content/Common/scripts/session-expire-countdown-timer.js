$(document).ready(function ()
{
    /*
    # =============================================================================
     * Make session expired timer available in loaded page DOM element
     # =============================================================================
    */
    //var history_api = typeof history.pushState !== 'undefined';
    //// history.pushState must be called out side of AngularJS Code
    //if (history_api) history.pushState(null, '', path);
    //var path = '';

    showTimer = function ()
    {
        //Comment : Here default count down timer seconds
        var timeCountDown = 10;
        var updateTimerAfter = 1000;

        //Comment : Here on page load show session-expired view with timer then redirect user to home page
        setTimeout(redirectUser, updateTimerAfter);

        function redirectUser() {
            timeCountDown--;

            //console.log(timeCountDown);

            //Comment : Here refresh page counter after interval of time
            $('#timerCount').html(timeCountDown.toString() + '  secs...');
            $(".loader-overlay").css('display', 'none');

            //Comment : Here time spent then redirect user to home page
            if (timeCountDown == 0) {
                window.location.href = appBaseUrl;
                //window.location.href = '/PurchasePath/Home/Index';
            }
            else {
                setTimeout(redirectUser, updateTimerAfter);
            }
        }
    };

    showTimer();
});