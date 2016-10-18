$(document).ready(function () {
    //Comment : Here on page load show session-expired view with timer then redirect user to home page
    setTimeout(removeHistory, updateTimerAfter);

    //Comment : Here default count down timer seconds
    var timeCountDown = 20;
    var updateTimerAfter = 1000;
    var i = 1;

    function removeHistory() {
        //debugger;
        timeCountDown--;

        //console.log(timeCountDown);

        var history_api = typeof history.pushState !== 'undefined';
        var path = '';

        // history.pushState must be called out side of AngularJS Code
        if (history_api) {
            while (i < 20) {
                history.pushState(null, '', path);
                i++;
            }
        };
    }

    // disables backspace on page except on input fields and textarea.
    $(document.body).keydown(function (e) {
        var elm = e.target.nodeName.toLowerCase();
        //alert(elm);
        //alert(e.which);
        if (e.which == 8 && elm !== 'input' && elm !== 'textarea') {
            e.preventDefault();
        }
        // stopping event bubbling up the DOM tree..
        e.stopPropagation();
    });

})