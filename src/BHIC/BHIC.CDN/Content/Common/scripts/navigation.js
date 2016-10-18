$(document).ready(function () {
    //$(document).on('click', '.progress-bar', function () {
    //    navigation = {
    //        path: event.target.attributes['url'].value || '',
    //        status : true
    //    };
    //    //navigation = "true";
    //});

    //$('.progress-bar').click(function (event) {
    //    navigation = {
    //        path: event.target.attributes['url'].value || '',
    //        status: true
    //    };
    //});

    $('.disable-navigate').click(function (event) {
        navigation = {
            path: '',
            status: false
        };
    });
});