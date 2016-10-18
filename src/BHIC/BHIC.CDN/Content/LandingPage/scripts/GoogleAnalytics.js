if (isactive == null) {
    (function () {
        //Comment : Here Google Analytics Code Block ***********************************
        if (gaCode != null && gaCode != undefined) {
            (function (i, s, o, g, r, a, m) {
                i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                    (i[r].q = i[r].q || []).push(arguments)
                }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
            })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

            ga('create', gaCode, 'auto');
            //ga('send', 'pageview');
        }
        //Google Analytics Code End Here ***********************************
    })();
}