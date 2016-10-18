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

    $(document).ajaxComplete(function () {
        $('.main-menu li:nth-child(2)').addClass("active");
    });
});