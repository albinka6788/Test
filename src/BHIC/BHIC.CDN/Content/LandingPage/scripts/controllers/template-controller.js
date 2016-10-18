(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('templateController', ['$scope', '$compile', '$sce', '$routeParams', '$location', 'loadingService', 'resultSet', 'authorisedUserWrapperService', templateControllerFn])
    function templateControllerFn($scope, $compile, $sce, $routeParams, $location, loadingService, resultSet, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        $scope.TokenId = 0;
        $scope.zipflag = false;
        $scope.islobexists = false;
        $scope.isExecuted = true;
        $scope.active1 = "active";
        $scope.errorText = "Please enter zip code";
        $scope.DefaultHeadingText = "Fast. Easy. <span class='small'>Upto</span> 20% savings.";
        $scope.DefaultSubHeadingText = "Cover Your Business with a policy from <span> Warren Buffett's Berkshire Hathaway Insurance Group.</span>";
        $scope.blankInit = function () {
            var wrapper = { "method": "GetCurrentEnvironment" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;
                }
            });
        }

        $scope.GetDefaultHeadings = function (TemplateId) {
            var headingList = [];
            switch (TemplateId) {
                case 1: headingList.push('MAXIMUM CONTROL <br> MINIMUM HASSLE'); break;
                case 2: headingList.push('Insurance for your Business from <br> a <span> Berkshire Hathaway Comapany. <span>'); break;
                case 3: headingList.push('Get <span> Workers’ Compensation </span>  Insurance <br> with Maximum Control and Minimum Hassle.'); break;
                case 4: headingList.push('Insurance for your Business <br> with <strong>Maximum Control</strong> and <br> <strong>Minimum Hassle</strong>.'); break;
                case 5: headingList.push('Get <span>Business Owner’s Policy</span> Insurance <br> with Maximum Control and Minimum Hassle.'); break;
            }

            headingList.push('An <strong>A++ financial strength</strong> insurance company.')
            headingList.push('An <strong>easy</strong>, <strong>reliable</strong>, and <strong>quick</strong> way to Cover Your Business.');
            return headingList;
        }

        $scope.TemplateInit = function () {
            debugger;
            $('[name=site]').removeAttr('href');
            //$('[name=bootstrap1]').removeAttr('href');
            $scope.TokenId = $routeParams.TokenId;
            var landingPageDetails = resultSet.landingPageDetails;
            $scope.lobId = landingPageDetails.lob;
            $scope.lob = (landingPageDetails.lob == "1") ? "Workers' Compensation – Cover Your Business" : (landingPageDetails.lob == "2") ? "Business Owner's Policy – Cover Your Business" : "Commercial Auto – Cover Your Business";
            document.title = (landingPageDetails.lob == "WC") ? "Workers' Compensation – Cover Your Business" : (landingPageDetails.lob == "BOP") ? "Business Owner's Policy – Cover Your Business" : "Commercial Auto – Cover Your Business";
            $scope.logo = window.location.origin + landingPageDetails.logo;
            $scope.StaticMsg = (landingPageDetails.CTAMsgList.length == 0) ? null : landingPageDetails.CTAMsgList[0].Message;
            $scope.CTAMsgList = (landingPageDetails.CTAMsgList.length == 0) ? null : (landingPageDetails.CTAMsgList[0].Message == "") ? null : landingPageDetails.CTAMsgList;
            $scope.PageName = (landingPageDetails.PageName.length == 0) ? null : landingPageDetails.PageName;
            $scope.Heading = (landingPageDetails.Heading.length == 0) ? null : landingPageDetails.Heading;
            $scope.SubHeading = (landingPageDetails.SubHeading.length == 0) ? null : landingPageDetails.SubHeading;
            $scope.ProductName = (landingPageDetails.ProductName.length == 0) ? null : landingPageDetails.ProductName;
            if (landingPageDetails.ProductHighlight != null && landingPageDetails.ProductHighlight != "") {
                $scope.ProductHighlight = landingPageDetails.ProductHighlight.split('|');
            }
            else {
                $scope.ProductHighlight = null;
            }
            $scope.RedirectURL = window.location.origin + "/PurchasePath/Quote/Index#/GetBusinessInfo";
            $scope.BTNText = (landingPageDetails.BTNText.length == 0) ? null : landingPageDetails.BTNText;
            $scope.CalloutText = (landingPageDetails.CalloutText.length == 0) ? null : landingPageDetails.CalloutText;
            
            $scope.phone = window.location.origin + "/LandingPage/Images/neon-phone.png";
            $scope.MainImage = landingPageDetails.MainImage;

            //$('[name=site]').removeAttr('href');
            //$('[name=bootstrap1]').removeAttr('href');
            //$('[name=dyanamicCSS]').attr('href', landingPageDetails.TemplateCss);
            $('[name=dyanamicCSS]').removeAttr('href');
            for (var i = 1; i <= 2; i++) {
                if (i != landingPageDetails.TemplateId)
                    $('[name=dyanamicCSS' + i + ']').removeAttr('href');
            }

            //if ($scope.CTAMsgList == null)
            //    $scope.CTAMsgList = $scope.GetDefaultHeadings(landingPageDetails.TemplateId);
           
            $scope.content = [];
            //$.each($scope.CTAMsgList, function (ind, obj) {
            //    if ($scope.StaticMsg == null || $scope.StaticMsg == "")
            //        $scope.content.push(obj);
            //    else
            //        $scope.content.push(obj.Message);
            //})
            $scope.HeadingList = [];
            $scope.SubHeadingList = [];
            if (landingPageDetails.CTAMsgList.length != 0) {
                $scope.StaticMsg = (landingPageDetails.CTAMsgList.length == 0) ? null : landingPageDetails.CTAMsgList[0].Message;
                angular.forEach(landingPageDetails.CTAMsgList, function (value, index) {
                    if (index == 0 && value.Message == "") {
                        $scope.CTAMsgList = null;
                        $scope.HeadingList = null;
                    }
                    else {
                        $scope.HeadingList.push(value.Message.split('|')[0]);
                        $scope.SubHeadingList.push(value.Message.split('|').splice(1));
                    }
                });
            }
            else {
                $scope.CTAMsgList = null;
                $scope.HeadingList = null;
            }
        }


        $scope.ValidateZipCode = function (zipCode) {
             $scope.isExecuted = false;
            if (!$scope.zipflag) {
                var wrapper = { "method": "GetValidZipDetail", "queryString": zipCode };
                $scope.islobexists = false;
                $("#zipcode").addClass("btn-loading");
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        $("#zipcode").removeClass("btn-loading");
                             $scope.isExecuted = true;
                        angular.forEach(response.lobResult.Data.lob, function (obj, ind) {
                            if (obj.Abbreviation == $scope.lobId  && obj.Status == "Available") {
                                $scope.StateCode = response.county[0].StateCode;
                                $scope.islobexists = true;
                                $scope.zipflag = false;
                            }

                            if (!$scope.islobexists && obj.Abbreviation == $scope.lobId) {
                                $scope.zipflag = true;
                                $("#zipcode").removeClass("btn-loading");
                                $scope.errorText= obj.Status;
                            }
                        });
                    }
                    else {
                        $scope.zipflag = true;
                        $scope.isExecuted = true;
                        $("#zipcode").removeClass("btn-loading");
                        $scope.errorText = "Please enter valid zip code";
                        console.log("Please enter valid zip code");
                    }
                });
            }
        }

        $scope.GetYourQuote = function () {
            $scope.zipflag = false;
            if ($scope.isExecuted) {
            if ($scope.islobexists) {
                var dynamicObj = { "TokenId": $scope.TokenId, "ZipCode": $("#zipcode").val(), "StateCode": $scope.StateCode };
                var wrapper = { "method": "GetAd", "postData": dynamicObj };

                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        window.location = response.redirectUrl;
                    }
                    else {
                        $scope.zipflag = true;
                        if (response.errorMsg == "Home")
                            window.location.href = response.redirectUrl;
                        else
                            $scope.errorText = response.errorMsg;
                    }
                    loadingService.hideLoader();
                });
            }
            else {
                $scope.zipflag = true;
                $scope.errorText = ($("#zipcode").val() == "") ? "Please enter zip code" : (!$scope.islobexists) ? $scope.errorText : "Please enter valid zipcode";
                }
            }
            else
            {
            $scope.zipflag = true;
            $scope.errorText ="Proccesing...";
            }
        }

        // Template 2 Purpose only.
        $scope.SwitchText = function (index) {
            $scope.active1 = "";
            $scope.active2 = "";
            $scope.active3 = "";
            switch (index) {
                case "1": $scope.active1 = "active"; break;
                case "2": $scope.active2 = "active"; break;
                case "3": $scope.active3 = "active"; break;
            }
        }

        loadingService.hideLoader();
        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
