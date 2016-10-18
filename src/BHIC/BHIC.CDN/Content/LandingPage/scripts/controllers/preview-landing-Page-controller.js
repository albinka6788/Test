(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('previewLandingPageController', ['$scope', '$compile', '$sce', '$location', '$routeParams', 'loadingService', 'authorisedUserWrapperService', previewLandingPageControllerFn])
    function previewLandingPageControllerFn($scope, $compile, $sce, $location, $routeParams, loadingService, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        $scope.active1 = "active";
        $scope.DefaultHeadingText = "Fast. Easy. <span class='small'>Upto</span> 20% savings.";
        $scope.DefaultSubHeadingText = "Cover Your Business with a policy from <span> Warren Buffett's Berkshire Hathaway Insurance Group.</span>";
        $scope.GoToBack = function () {
            window.back(-1);
        }

        //default text not required
        $scope.GetDefaultHeadings = function (TemplateId) {
            var headingList = [];
            TemplateId = TemplateId.split(".")[0];
            switch (TemplateId) {
                case "Template1": headingList.push('MAXIMUM CONTROL <br> MINIMUM HASSLE'); break;
                case "Template2": headingList.push('Insurance for your Business from <br> a <span> Berkshire Hathaway Comapany. <span>'); break;
                case "Template3": headingList.push('Get <span> Workers’ Compensation </span>  Insurance <br> with Maximum Control and Minimum Hassle.'); break;
                case "Template4": headingList.push('Insurance for your Business <br> with <strong>Maximum Control</strong> and <br> <strong>Minimum Hassle</strong>.'); break;
                case "Template5": headingList.push('Get <span>Business Owner’s Policy</span> Insurance <br> with Maximum Control and Minimum Hassle.'); break;
                case "Template6": headingList.push('Insurance for your Business <br> with <strong>Maximum Control</strong> and <br> <strong>Minimum Hassle</strong>.'); break;
                case "Template7": headingList.push('Fast. Easy. <span class="small">Upto</span> 20% savings.'); break;
            }

            headingList.push('An <strong>A++ financial strength</strong> insurance company.')
            headingList.push('An <strong>easy</strong>, <strong>reliable</strong>, and <strong>quick</strong> way to Cover Your Business.');
            return headingList;
        }

        $scope.TemplateInit = function () {
            //$('[name=bootstrap1]').removeAttr('href');
            $('[name=site]').removeAttr('href');
            var previewObj = JSON.parse(window.localStorage.getItem("preview"));
            $scope.logo = (previewObj.TemplateId == "Template1.cshtml") ? window.location.origin + "/LandingPage/Images/Logo/logo_1.png"
                : window.location.origin + "/LandingPage/Images/Logo/logo_2.png";
            $scope.Heading = previewObj.Heading;
            $scope.SubHeading = previewObj.SubHeading;
            $scope.ProductName = previewObj.ProductName;
            if (previewObj.ProductHighlight != null && previewObj.ProductHighlight != "") {
                $scope.ProductHighlight = previewObj.ProductHighlight.split('|');//(previewObj.ProductHighlight.length == 0) ? null :
            }
            else {
                $scope.ProductHighlight = null;
            }

            $scope.BTNText = previewObj.BTNText;
            $scope.CalloutText = previewObj.CalloutText;
            $scope.HeadingList = [];
            $scope.SubHeadingList = [];
            if (previewObj.CTAMsgList.length != 0) {
                $scope.StaticMsg = (previewObj.CTAMsgList.length == 0) ? null : previewObj.CTAMsgList[0].Message;
                angular.forEach(previewObj.CTAMsgList, function (value, index) {
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


            $scope.phone = window.location.origin + "/LandingPage/Images/neon-phone.png"
            var id = $routeParams.TokenId;
            $scope.MainImage = previewObj.MainImage;
            $scope.lob = (previewObj.lob == "1") ? "Workers' Compensation" : (previewObj.lob == "2") ? "Business Owner's Policy" : "Commercial Auto";
            //$('[name=dyanamicCSS]').attr('href', previewObj.TemplateCss);
            $('[name=dyanamicCSS]').removeAttr('href');
            for (var i = 1; i <= 7; i++) {
                if ('Template' + i + '.cshtml' != previewObj.TemplateId)
                    $('[name=dyanamicCSS' + i + ']').removeAttr('href');
            }
            //Comment : default text done at html --Sreeram
            //if ($scope.CTAMsgList == null)
            //    $scope.CTAMsgList = $scope.GetDefaultHeadings(previewObj.TemplateId);

            $scope.content = [];
            if ($scope.CTAMsgList != null) {
                $.each($scope.CTAMsgList, function (ind, obj) {
                    if ($scope.StaticMsg == null || $scope.StaticMsg == "")
                        $scope.content.push(obj);
                    else
                        $scope.content.push(obj.Message);
                })
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
