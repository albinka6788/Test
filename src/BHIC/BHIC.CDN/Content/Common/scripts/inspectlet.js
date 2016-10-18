﻿
window.recordSession = function (inspectletWid, inspectletQuoteid, inspectletSessionid, resetSession, googleAdId) {
    window.__insp = window.__insp || [];
    if (resetSession) {
        __insp.push(["resetSession"]);
    }

    __insp.push(['wid', inspectletWid]);
    __insp.push(['tagSession', {
        SessionID: inspectletSessionid, QuoteID: inspectletQuoteid,
        QuoteStatus: (typeof inspectletQuoteStatus != 'undefined') ? inspectletQuoteStatus : '',
        GoogleAdId: (googleAdId != null && googleAdId != undefined && googleAdId != '') ? googleAdId : ''
    }]);

    function ldinsp() {
        if (typeof window.__inspld != "undefined")
            return; window.__inspld = 1;
        var insp = document.createElement('script');
        insp.type = 'text/javascript';
        insp.async = true;
        insp.id = "inspsync";
        insp.src = ('https:' == document.location.protocol ? 'https' : 'http') + '://cdn.inspectlet.com/inspectlet.js';
        var x = document.getElementsByTagName('script')[0];
        x.parentNode.insertBefore(insp, x);
    };
    setTimeout(ldinsp, 500);
    document.readyState != "complete" ? (window.attachEvent ? window.attachEvent('onload', ldinsp) : window.addEventListener('load', ldinsp, false)) : ldinsp();
}