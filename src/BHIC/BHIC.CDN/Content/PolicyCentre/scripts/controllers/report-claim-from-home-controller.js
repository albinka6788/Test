(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('reportClaimFromHomeController', ['$scope', '$http', '$filter', '$window', '$location', 'loadingService', 'unauthorisedUserWrapperService', 'captchaService', 'sharedUserService',reportClaimFromHomeControllerFn]);
    function reportClaimFromHomeControllerFn($scope, $http, $filter, $window, $location, loadingService, unauthorisedUserWrapperService, captchaService, sharedUserService) {

        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        var formdata, count = 0, valid = true;
        $scope.focusField = true;
        $scope.emptyString = "";
        $scope.responseMsg = false;
        $scope.errorMsg = false;
        var maxDate = new Date();
        $scope.maxDate = sharedUserService.FormatDate(maxDate);
        $scope.defaultForm = function () {
            $scope.rpCode = $scope.emptyString;
            $scope.rpbusiness = $scope.emptyString;
            $scope.rpname = $scope.emptyString;
            $scope.rpphone = $scope.emptyString;
            $scope.rpworker = $scope.emptyString;
            $scope.rpDateInjury = $scope.emptyString;
            $scope.rplocation = $scope.emptyString;
            $scope.rpdescription = $scope.emptyString;
            $scope.prCapthcaError = $scope.emptyString;
            $scope.fileerror = $scope.emptyString;
            if (navigator.userAgent.indexOf('MSIE') > 0) {
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            }
            else {
                angular.element(document.getElementById('filesel')).val('');
            }
            formdata = $scope.emptyString;
            $scope.submitted = false;
        };

        loadingService.hideLoader();

        //Clearing the file upload 
        $("#filesel").click(function () {
            formdata = $scope.emptyString;
            if (navigator.userAgent.indexOf('MSIE') > 0) {
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            } else {
                angular.element(document.getElementById('filesel')).val('');
            }
        });

        //For validating Phone Number
        $scope.CheckPhoneNumber = function (number) {
            var formattedNumber;
            var area = number.substring(0, 3);
            var front = number.substring(4, 7);
            var middle = number.substring(8, 12);
            var end = number.substring(14, 18);

            formattedNumber = area + front + middle;
            formattedNumber = formattedNumber.replace(/_/g, '');
            if (formattedNumber.length < 10) {
                return formattedNumber;
            }
            else {
                formattedNumber += end;
                formattedNumber = formattedNumber.replace(/_/g, '');
            }
            return formattedNumber;
        };

        // Allow Anonymous Submit Report Claim data from Home page.
        $scope.CallService = function (policyCode) {
            // For Registering new user
            if (policyCode != null)
                _self.reportClaim.policyCode = policyCode;

            var wrapper = { "method": "RequestReportClaimFromHome", "postData": _self.reportClaim }
            unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                if (response && response.success) {
                    $scope.responseMsg = true;
                    $scope.errorMsg = false;
                    $scope.hasErrorMain = false;
                    $scope.defaultForm();
                    $scope.rpForm.$setPristine();
                } else if (response.redirectStatus) {
                    $location.path("/Error/");
                } else {
                    $scope.responseMsg = false;                   
                    $scope.errorMsg = true;
                }
                ResetCaptcha();
                loadingService.hideLoader();
                $('html, body').animate({ scrollTop: 0 }, 800);
                $scope.focusField = true;
            });
        }

        //validating and appending the files to to FormData
        $scope.getUploadedFiles = function ($files) {
            count = 0;
            formdata = $scope.emptyString;
            $scope.fileException = $scope.emptyString;
            if ($files.length > 3) {
                if (navigator.userAgent.indexOf('MSIE') > 0) {
                    $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                }
                else {
                    angular.element(document.getElementById('filesel')).val('');
                }
                $scope.$apply(function () {
                    $scope.fileException = "Only 3 files can be uploaded";
                });
                return;
            }
            $scope.fileerr = $scope.emptyString;
            
            angular.forEach($files, function (value, key) {
                var filetype = value.name.split('.').pop();
                count += value.size;
                if (filetype == "exe" || filetype == "msi" || filetype == "dll" || filetype == "ocx" || filetype == "com" ||
                    filetype == "jar" || filetype == ".bat") {
                    if (navigator.userAgent.indexOf('MSIE') > 0) {
                        $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                    }
                    else {
                        angular.element(document.getElementById('filesel')).val('');
                    }
                    $scope.$apply(function () {
                        $scope.fileException = "The file format is not supported";
                    });
                    return;
                }
                else if (count >= 10485760) {
                    if (navigator.userAgent.indexOf('MSIE') > 0) {
                        $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                    }
                    else {
                        angular.element(document.getElementById('filesel')).val('');
                    }
                    count = 0;
                    $scope.$apply(function () {
                        $scope.fileException = "The file size should not be greater then 10 MB.";
                    });
                    return;
                }
                else {
                    if (valid == true || formdata == "") {
                        formdata = new FormData();
                        valid = false;
                    }
                    formdata.append(key, value);
                    $scope.$apply(function () {
                        $scope.fileException = "";
                    });
                }
            });

        };

        // Send a request for claim
        $scope.RequestForClaim = function () {
            var number = $scope.CheckPhoneNumber($scope.rpphone);
            if (number.length < 10) {
                $scope.phoneerrormsg = "Please Enter Valid Phone Number!";
                $('html, body').animate({ scrollTop: 0 }, 500);
                $("#reportphone").focus();
                loadingService.hideLoader();
                ResetCaptcha();
                return;
            }
            $scope.phoneerrormsg = $scope.emptyString;
            var currentDate = new Date();
            _self.reportClaim = {};
            _self.reportClaim.Id = 0;
            _self.reportClaim.NameOfBusiness = $scope.rpbusiness;
            _self.reportClaim.PhoneNumber = $scope.rpphone;
            _self.reportClaim.policyCode = $scope.rpCode;            
            _self.reportClaim.YourName = $scope.rpname;
            _self.reportClaim.NameOfInjuredWorker = $scope.rpworker;
            _self.reportClaim.DateOfIllness = new Date($scope.rpDateInjury);
            _self.reportClaim.EffectiveDate = $scope.rpDateInjury;
            _self.reportClaim.Location = $scope.rplocation;
            _self.reportClaim.Description = $scope.rpdescription;
            _self.reportClaim.ClaimType = ($scope.Type == "wc") ? "WC" : "BP";

            var request = {
                method: 'POST',
                url: baseUrl + '/ReportClaim/UploadDocuments?pCode=' + $scope.rpCode,
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };
            $scope.responseMsg = false;
            if (formdata) {
                $http(request).success(function (response) {
                    if (response.success) {
                        $scope.CallService(response.policyCode);
                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {                        
                        $scope.hasErrorMain = true;                        
                        loadingService.hideLoader();
                        $('html, body').animate({ scrollTop: 0 }, 800);
                        ResetCaptcha();
                        formdata = $scope.emptyString;
                        if (navigator.userAgent.indexOf('MSIE') > 0) {
                            $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                        }
                        else {
                            angular.element(document.getElementById('filesel')).val('');
                        }
                        $scope.errorMessage = response.errorMessage;
                    }

                }).error(function (data, status, header, config) {

                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                    //$scope.fileerror = "something went wrong";
                    $scope.hasErrorMain = true;
                    $scope.fileerr = "something went wrong";
                });
            }
            else {
                $scope.CallService(null);
            }

        }

        // Send a request for claim
        $scope.ReportClaimFromHome = function () {

            loadingService.showLoader();
            var code = GetCaptchaResponse();
            if (code != '') {
                captchaService.validateCaptcha(code).then(function (res) {
                    if (res.status)
                        $scope.RequestForClaim()
                    else {
                        ResetCaptcha();
                        loadingService.hideLoader();
                    }
                });
            }
            else {
                loadingService.hideLoader();
                $scope.prCapthcaError = "Recaptcha is required!";
            }
        }

        $scope.setDynamicText = function () {
            if ($location.$$path == "/ReportClaimFromHome")
                $scope.Type = "wc";
            else
                $scope.Type = "bp";
        }
        
        // Controler funciton End Please dont write any methods/functions after this line.
    }
})();