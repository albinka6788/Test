var currenturl = '';
var baseUrl = '';
var appCdnDomain = '';
var appBaseDomain = '';
var sessionTimeOut = 1;
var sideMenuReload = true;
var PageDictionory = {
    'YourPolicies': 'Your Policies', 'ContactInfo': 'Contact Information', 'SavedQuotes': 'Your Saved Quotes', 'PolicyInformation': 'View Policy Information', 'ChangePassword': 'Change Your Password',
    'PolicyInformation/:policyCode': 'Payment Confirmation', 'MakePayment': 'Make Payment', 'PolicyDocument': 'View Policy Documents', 'PhysicianPanel': 'View Physician Panel',
    'RequestCertificate': 'Request Certificate of Insurance', 'RequestPolicyChange': 'Request a Policy Change', 'ReportClaim': 'Report a Claim', 'CancelPolicy': 'Request Policy Cancellation',
    'ReportClaimFromHeader': 'Report a Claim', 'EditContactInfo': 'Edit Your Contact Information', 'EditContactAddress': 'Change of Address Request', 'Error': 'Error', 'RequestLossRun': 'Request Loss Runs',
    'Resources': 'Helpful Resources', 'EmployerNotices': 'Employer Posting Notices', 'Connecticut': 'Connecticut MCP', 'TexasMPN': 'Texas MPN', 'CAClaims': 'Required CA Claims Information',
    'RestrictedAccess': '', 'UploadDocuments': 'Upload Documents'
};

String.prototype.replaceAll = function (s, r) { return this.split(s).join(r) }


var SetNavigation = function (requestUrl) {    
    var listOfUrls = ["PolicyInformation", "MakePayment", "PolicyDocument", "RequestCertificate", "RequestPolicyChange", "ReportClaim", "PhysicianPanel", "CancelPolicy", "RequestLossRun" ,"UploadDocuments"];
    if ($.inArray(requestUrl, listOfUrls) != -1) {
        $.ajax({
            type: "POST",
            url: OriginLocation() + baseUrl + "SideMenu/GetMenus",
            data: { 'CYB': window.name, 'key': requestUrl },
            success: function (menuList) {
                if (menuList != "false") {
                    $('.sidebar-menu').html(menuList);
                    sideMenuReload = false;
                    //for mobile 
                    $("ul.sidebar-menu li a[href]").click(function () {
                        "use strict";
                        if ($(window).width() < 800) {
                            $(".top-navbar").toggleClass("toggle");
                            $(".sidebar-left").toggleClass("toggle");
                            $(".page-content").toggleClass("toggle");
                            $('.btn-collapse-sidebar-left').toggleClass("toggle");
                            $(".overlay").toggleClass("on");
                            $("body").toggleClass("toggle");
                        }
                    });
                } else {
                    window.location.href = OriginLocation() + baseUrl + "Dashboard/Index#/YourPolicies";
                }
            }
        });
    }
}

function GetPhoneNoFormat(phno) {
    return phno.replaceAll("(", "").replaceAll(")", "").replaceAll("x", "").replaceAll(" ", "").replaceAll("-", "");
}

function sidemenustate(id) {
    if (id == 'PolicyInformation' || id == "MakePayment" || id == "PolicyDocument" || id == "PhysicianPanel" || id == "RequestCertificate" || id == "RequestPolicyChange" || id == "ReportClaim" ||
        id == "CancelPolicy" || id == "RequestLossRun" || id == "UploadDocuments") {
        $('.sidebar-menu a').removeClass('activeLink');
        $('.sidebar-menu a#' + id).attr('class', 'activeLink');
        $('.rootText').html(PageDictionory[id]);
    }
}

function nosidemenu(isSideMenu) {
    if (isSideMenu) {
        $('.page-content').addClass("page-content-full");
        $('.sidebar-left').css('display', 'none');
        $('.btn-collapse-sidebar-left').css('display', 'none');
    }
    else {
        $('.page-content').removeClass("page-content-full");
        $('.sidebar-left').css('display', 'block');
        $('.btn-collapse-sidebar-left').css('display', 'block');
        $('.topNavigation li').removeClass('active');
    }
}

function topmenustate(id) {
    $('.topNavigation li').removeClass('active');
    $('.topNavigation li#' + id).attr('class', 'active');
}

function GetCaptchaResponse() {
    return grecaptcha.getResponse();
}

function ResetCaptcha() {
    grecaptcha.reset();
}

function RedirectToLogin() {
    window.location.replace(OriginLocation() + baseUrl);
}

function bindDatatable(id, disableColumnSortingIndex, deleteRowIndex) {
    setTimeout(
      function () {
          if (disableColumnSortingIndex != null) {
              $('#' + id).dataTable({
                  "bPaginate": true,
                  "bAutoWidth": false,
                  "bFilter": false,
                  "bInfo": false,
                  "lengthMenu": [[5, 10, 20, -1], [5, 10, 20, "All"]],
                  "order": [["2", "desc"]],
                  "aoColumnDefs": [{
                      'bSortable': false,
                      'aTargets': [disableColumnSortingIndex]
                  }]
              });
          }
          else {
              $('#' + id).dataTable({
                  "bPaginate": true,
                  "bAutoWidth": false,
                  "bFilter": false,
                  "bInfo": false,
                  "lengthMenu": [[5, 10, 20, -1], [5, 10, 20, "All"]],
                  "order": [["2", "desc"]]
              });
          }
      }, 0);
}

function DeleteRowFromSavedQuoteTable(id, deleteRowIndex) {
    $("table#" + id).DataTable().row(deleteRowIndex).remove().draw(false);
}

function OriginLocation() {
    if (!window.location.origin) {
        return window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    }
    return window.location.origin;
}

function DownloadStaticDocument(filename) {
    window.location.href = OriginLocation() + baseUrl + "Document/DownloadStaticDocument?filename=" + filename;
}

function bindTooltip() {
    $('.tooltip').tooltip();
}


function formatBox() {

    $('.equalize').equalize({ children: '.content-box' });
    $(window).resize(function () {
        $('.equalize').equalize({ reset: true, children: '.content-box' });
    });
}
$(document).ready(function () {
    $('#main-fixed-nav ul.topNavigation li').click(function () {
        if ($(window).width() < 800) {
            if ($('div.container nav.navbar').length <= 0) {
                $('ul.topNavigation').css('display', 'none');
                $(".icon-plus").toggleClass("rotate-45");
            } else {
                $('div.container nav.navbar').toggleClass('reveal');
            }
        }
    });

//    $(window).resize(function () {
//        if ($(window).width() > 800) {
//            if ($('div.container nav.navbar').length <= 0) {
//                $('ul.topNavigation').css('display', 'block');
//            } else {
//                $('div.container nav.navbar').toggleClass('reveal');
//            }
//        } else {
//            if ($('div.container nav.navbar').length <= 0) {
//                $('ul.topNavigation').css('display', 'none');
//                $(".icon-plus").removeClass("rotate-45");
//            } else {
//                $('div.container nav.navbar').removeClass('reveal');
//            }
//        }
//    });
});

function CloseScheduleCallModel() {
    //Comment : Here if form has been validated then show model POP-UP for further information
    var modelBox = $('[data-remodal-id=scheduleCallModel]').remodal();

    //if not undefined
    if (modelBox) {
        modelBox.open();
        $('.remodal-overlay').addClass("overlay-light");
    }
}

function openCAModelPopUp() {
    var modelBox = $('[data-remodal-id=commercial-auto-info]').remodal();
    //if object not undefined then only
    if (modelBox) {
        //Comment : Here "OPEN" Remodel pop-up
        modelBox.open();
        $('.remodal-overlay').removeClass("overlay-light");
    }
}