$(document).ready(function () {

    var url = "/PurchasePath/OrderSummary/ConfirmationContent";
    $.get(url, null, function (data) {

        $(".placeHolderContent").empty().html(data);
        $(".loader-overlay").css('display', 'none');
    });

})