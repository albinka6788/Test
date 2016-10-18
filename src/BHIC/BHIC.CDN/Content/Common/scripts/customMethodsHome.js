$(document).ready(function () {

    $('.menu-trigger').on('click', function () {
        $(this).children('span').toggleClass('animate-cross');
        $('.navbar').toggleClass('reveal');
        if ($('.reveal').length) {
            $('.overlay').addClass('on');
        } else {
            $('.overlay').removeClass('on');
        }
    });

    $(document).on('click', function (event) {
        if (!$(event.target).closest('.dropdown').length) {
            $('.dropdown-menu').hide();
        }
    });

    $(window).resize(function () {
        $('.overlay').removeClass('on');
    });

    $(function () {
        if (window.location.hash) {
            var hash = window.location.hash.substring(1);
            if (hash == "faq-bop") {
                $('.faq-bop').trigger("click");
            }
        }
    });

    //Comment : Here Set focus on first text input on HomePage
    $("input:text:visible:first").focus();

    $(".owl-carousel").owlCarousel({
        autoPlay: 7000,
        singleItem: true,
        autoHeight: true,
        pagination: false,
        transitionStyle: "backSlide"
    });

    $(window).scroll(function () {
        var scrolled = $(window).scrollTop();
        if (scrolled > 100) {
            $(".scroll-down").hide();
        } else {
            $(".scroll-down").show();
        }
    });

    $(".scroll-down").click(function () {
        $('html, body').animate({
            scrollTop: $(".benefits").offset().top
        }, 1000);
    });

    //Comment : Here Google Analytics Code Block ***********************************
    if (homeVM != null && homeVM != undefined) {
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', homeVM.gaCode, 'auto');
        ga('send', 'pageview');
    }
    //Google Analytics Code End Here ***********************************

    //Comment : Here Start LiveChat Embedded Icon/Tracking Code *************************
    /*debugger;
    var __lc = {};
    __lc.license = homeVM.lcLicense;
    __lc.group =    homeVM.lcGroup;
    __lc.params = [
                    { name: homeVM.lcServerName, value: homeVM.lcServerValue },
                  ];


    //Comment : Here based on development environment attach LIVE CHAT plugin
    var flag = homeVM.environmentName;

    if((flag.toUpperCase() =='DEV'))
    {
        var lc = document.createElement('script'); lc.type = 'text/javascript'; lc.async = true;
        lc.src = homeVM.lcSrc;

        var s = document.getElementsByTagName('script')[0];
        s.parentNode.insertBefore(lc, s);
    }
    */
    //Live Chat Code End Here *************************    

    //Refresh page if user is inactive more than system idle time
    //It will implemented only on home page
    if (homeVM != undefined && homeVM != null && homeVM.systemIdleDuration != undefined && homeVM.systemIdleDuration != null && homeVM.systemIdleDuration > 0) {

        (function (seconds) {

            //console.log("seconds :" + seconds);
            //console.log("current date time :" + new Date($.now()));
            var refresh,
              intvrefresh = function () {
                  clearInterval(refresh);
                  refresh = setTimeout(function () {
                      //console.log("inactive date time :" + new Date($.now()));

                      //clear session and refresh page
                      $.post("/PurchasePath/Home/ClearSession", function (data) {
                          location.href = location.href;
                      });
                  }, seconds * 1000);
              };

            $(document).on('mousemove click keypress scroll resize', function () { intvrefresh() });
            intvrefresh();

        }(homeVM.systemIdleDuration));
    }

    //Comment : Here to process "Loss Control" hundreds of links encyption processing show loader till page load completely 
    $('#lossControlAnchor').click(function () {
        //debugger;
        $('#divLoader').show();
        $('html').addClass("is-locked");
    });

    //route to why-us div on home page whenever user clicks why-us link on header menu
    $(window).on("load", function () {
        var urlHash = window.location.href.split("#")[1];

        if (urlHash == "/why-us") {
            redirectToWhyUs(urlHash);
        }

    });

    $('#whyUs').click(function () {
        var urlHash = $(this).attr('href').split('#')[1];
        redirectToWhyUs(urlHash);

    });

    function redirectToWhyUs(urlHash) {
        if (urlHash != "") {
            urlHash = urlHash.replace('/', '');

            if (urlHash && $('#' + urlHash).length)
                $('html,body').animate({
                    scrollTop: $('#' + urlHash).offset().top
                }, 500);
        }
    }
});