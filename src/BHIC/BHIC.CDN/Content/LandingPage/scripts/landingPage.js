var currenturl = '';
var baseUrl = '';

//$(document).ready(function () {
//    $(window).keydown(function (event) {
//        if (event.keyCode == 13) {
//            event.preventDefault();
//            return false;
//        }
//    });
//});

var AddCATTemplate = function (actionType, value, CTAId) {
    var cloneCATTemplate = $("#dynamicTemplate>tbody>tr").clone();
    if (actionType == 'add') {
        if ($('#staticTable').find('tr').length == 0) {
            $(cloneCATTemplate).find('img').remove();
            var img = '<img dynamic-Template src="/LandingPage/Images/Plus.png" onclick="AddCATTemplate();" style="text-align:right; margin-left:10px; width:30px; margin-top:-35px; height:30px; " />';
            $($(cloneCATTemplate).find('td')[1]).append($(img));
            $($($(cloneCATTemplate).find('td')[1]).find('textarea')).attr("CTAId", 0);
            $('#staticTable').append($(cloneCATTemplate));
        }
        else {
            $($($(cloneCATTemplate).find('td')[1]).find('textarea')).attr("CTAId", 0);
            $('#staticTable').append($(cloneCATTemplate));
        }
    }
    else {
        if ($('#staticTable').find('tr').length == 0) {
            $(cloneCATTemplate).find('img').remove();
            var img = '<img dynamic-Template src="/LandingPage/Images/Plus.png" onclick="AddCATTemplate();" style="text-align:right; margin-left:10px; width:30px; margin-top:-35px; height:30px; " />';
            $($(cloneCATTemplate).find('td')[1]).append($(img));
            $($(cloneCATTemplate).find('td')[1]).find('textarea').text(value);
            $($($(cloneCATTemplate).find('td')[1]).find('textarea')).attr("CTAId", CTAId);
            $('#staticTable').append($(cloneCATTemplate));
        }
        else {
            CTAId = (CTAId == undefined) ? 0 : CTAId;
            value = (value == undefined) ? "" : value;
            $($(cloneCATTemplate).find('td')[1]).find('textarea').text(value);
            $($($(cloneCATTemplate).find('td')[1]).find('textarea')).attr("CTAId", CTAId);
            $('#staticTable').append($(cloneCATTemplate));
        }
    }
};
var RemoveCATTemplate = function (sender) {
    $(sender).parents(".dynamicTR").remove();
};