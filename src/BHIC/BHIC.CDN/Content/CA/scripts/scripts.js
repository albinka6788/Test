var pageNumber = 0;
$('#ca-geico').width('100%');
$('#ca-geico').load(function () {
    $('#divLoader').show();
    
    var currentUrl = document.referrer;
    var iFrame = document.getElementById('ca-geico');
    location.hash = pageNumber;
    if (pageNumber == 0) {
        $('#ca-geico').height(750);
        
    }
    if ((pageNumber>0 && pageNumber<4) ||(pageNumber>5)) {
        $('#ca-geico').height(2400);
        $('#ca-geico').css("margin-top", "-65px");
    }

    if (pageNumber == 4) {
        $('#ca-geico').height(1600);

    }

    if (pageNumber == 5) {
        $('#ca-geico').height(950);

    }
    
    $('#divLoader').hide();
    window.parent.scrollTo(0, 0);
    pageNumber = pageNumber + 1
});


$(window).on('beforeunload', function () {
    $('#divLoader').show();
});

$(window).on('hashchange', function () {
    var hashValue = location.hash;
    hashValue = hashValue.replace('#', '');
    
    var currentPageNo = parseInt(hashValue);
    
    if ((pageNumber - 1) > currentPageNo) {
        pageNumber = currentPageNo;
        history.go(-1);
    }


});