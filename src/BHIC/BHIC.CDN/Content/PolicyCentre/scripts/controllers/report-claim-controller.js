(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('reportClaimController', ['$scope', '$http', '$filter', '$window', '$location', 'loadingService', 'sharedUserService', 'authorisedUserWrapperService', reportClaimControllerFn]);
    function reportClaimControllerFn($scope, $http, $filter, $window, $location, loadingService, sharedUserService, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.           
        var _self = this;
        $scope.responseMsg = false;
        $scope.focusField = true;
        $scope.emptyString = "";
        $scope.status = $scope.emptyString;
        $scope.errorMessage = $scope.emptyString;
        var maxDate = new Date();
        $scope.maxDate = sharedUserService.FormatDate(maxDate);
        $scope.hasError = false;
        $scope.fileException = $scope.emptyString;
        $scope.defaultOrganization = $scope.emptyString;
        var formdata, count = 0, valid = true;
        $scope.defaultForm = function () {
            count = 0;
            $scope.errMsg = $scope.emptyString;
            $scope.nameOfBusiness = $scope.defaultOrganization;
            $scope.phone = $scope.emptyString;
            $scope.nameOfInjuredWorker = $scope.emptyString;
            $scope.dateOfIllness = $scope.emptyString;
            $scope.location = $scope.emptyString;
            $scope.description = $scope.emptyString;
            if (navigator.userAgent.indexOf('MSIE') > 0) {
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            }
            else {
                angular.element(document.getElementById('filesel')).val('');
            }
            formdata = $scope.emptyString;
            $scope.phoneerrormsg = $scope.emptyString;
            $scope.fileselected = $scope.emptyString;
            $scope.submitted = false;
            $scope.staticDocument = {};
        };

        //Clearing the file upload 
        $("#filesel").click(function () {
            formdata = $scope.emptyString;
            if (navigator.userAgent.indexOf('MSIE') > 0) {
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            } else {
                angular.element(document.getElementById('filesel')).val('');
            }
        });

        //validating and appending the files to to FormData
        $scope.getTheFiles = function ($files) {
            count = 0;
            formdata = $scope.emptyString;
            $scope.errorMessage = $scope.emptyString;
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
                    formdata.append(value.name, value);
                    $scope.$apply(function () {
                        $scope.fileException = "";
                    });
                }
            })

        };
              
        // Report Claim From side menu, View Data Binding.
        // method to use report claim form submission.
        $scope.submitReprotForm = function () {
            var number = sharedUserService.CheckPhoneNumber($scope.phone);
            if (number.length < 10) {
                $scope.phoneerrormsg = "Please Enter Valid Phone Number!";
                $('html, body').animate({ scrollTop: 0 }, 800);
                $("#reportphone").focus();
                return;
            }
            loadingService.showLoader();
            //Making the display messages false
            $scope.responseMsg = false;
            $scope.errorMessage = $scope.emptyString;
            $scope.fileException = $scope.emptyString;
            $scope.phoneerrormsg = $scope.emptyString;
            var currentDate = new Date();
            _self.reportClaim = {};
            _self.reportClaim.Id = ($scope.Id == undefined) ? 0 : $scope.Id;
            _self.reportClaim.NameOfBusiness = $scope.nameOfBusiness;
            _self.reportClaim.PhoneNumber = $scope.phone;
            _self.reportClaim.NameOfInjuredWorker = ($scope.Type == "wc") ? $scope.nameOfInjuredWorker : "bp";
            _self.reportClaim.DateOfIllness = new Date($scope.dateOfIllness);
            _self.reportClaim.EffectiveDate = $scope.dateOfIllness;
            _self.reportClaim.Location = $scope.location;
            _self.reportClaim.Description = $scope.description;
            _self.reportClaim.PolicyCode = window.name.split("CYB")[0];
            _self.reportClaim.ClaimType = ($scope.Type == "wc") ? "WC" : "BP";

            var request = {
                method: 'POST',
                url: baseUrl + "ReportClaim/UploadDocuments?CYBKey=" + window.name.split("CYB")[0],
                data: formdata,
                headers: {
                    'Content-Type': undefined
                },
                transformRequest: angular.identity
            };
            // Method used to call service.
            // SEND THE FILES.
            var wrapper = { "method": "RequestReportClaim", "postData": _self.reportClaim };
            if (formdata) {
                $http(request).success(function (response) {
                    if (response.success) {

                        authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (innerResponse) {
                            if (innerResponse.success) {
                                $scope.responseMsg = true;
                                $scope.defaultForm();
                                $scope.reprotForm.$setPristine();
                            } else if (response.redirectStatus) {
                                $location.path("/Error/");
                            } else {
                                $scope.hasErrorMain = true;
                                //$scope.errorMessage = response.errorMessage;
                                $scope.errMsg = response.errorMessage;
                                formdata = $scope.emptyString;
                                if (navigator.userAgent.indexOf('MSIE') > 0) {
                                    $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                                }
                                else {
                                    angular.element(document.getElementById('filesel')).val('');
                                }
                            }
                            $('html, body').animate({ scrollTop: 0 }, 800);
                            loadingService.hideLoader();
                            $scope.focusField = true;
                        });
                    }
                    else {
                        loadingService.hideLoader();
                        ////$scope.errorMessage = response.errorMessage;
                        $scope.errMsg = response.errorMessage;
                        $('html, body').animate({ scrollTop: 0 }, 800);
                        $scope.reprotForm.$setPristine();
                        $scope.focusField = true;
                        formdata = $scope.emptyString;
                        if (navigator.userAgent.indexOf('MSIE') > 0) {
                            $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                        }
                        else {
                            angular.element(document.getElementById('filesel')).val('');
                        }
                    }
                }).error(function (data, status, header, config) {

                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                    $scope.errorMessage = "something went wrong";
                    $scope.focusField = true;
                });
            }
            else {
                // For Registering new user
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        $scope.responseMsg = true;
                        $scope.defaultForm();
                        $scope.reprotForm.$setPristine();
                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {
                        $scope.hasErrorMain = true;
                        $scope.errorMessage = response.errorMessage;
                    }
                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                    $scope.focusField = true;
                });
            }
            // Method used to call service.

        }

        // Report Claim View Data Binding.           

        $scope.OpenLink = function (url) {
            window.open(url, "_blank");
        }


        // Report Claim From Header View Data Binding.
        // Static List
        $scope.drpTypeList = [{ Id: 1, Type: "Workers' Compensation" }, { Id: 2, Type: "Business Owner's Policy" }];

        //$scope.filterFunction is no longer in use
        // Filter method used to filter by line of business, & filter takesplace from root element to leaf element.
        $scope.filterFunction = function (src, value, except) {
            var key;
            switch (typeof src) {
                case 'string':
                case 'number':
                case 'boolean':
                    return String(src).indexOf(value) > -1;
                case 'object':
                    except = except || [];
                    for (key in src) {
                        if (src.hasOwnProperty(key) &&
                            except.indexOf(key) < 0 &&
                            $scope.filterFunction(src[key], value, except)
                        ) {
                            return true;
                        }
                    }
            }
            return false;
        }

        //store all the policy
        var mainList = [];

        //filter all the policy by Lob
        $scope.filterList = function (data, LOB) {
            var lobData = [];
            for (var i = 0; i < data.length; i++) {
                if (data[i].LOB === LOB) {
                    lobData.push(data[i]);
                }
            }
            return lobData;
        }

        // Method is used to combine the polic code, policy begin and policy expaires dates in sinlge object.
        $scope.combinePolicyCodeAndDate = function (list) {
            var returnList = [];
            if (list.length > 0) {
                $.each(list, function (ind, obj) {
                    //if (obj.Status.indexOf('No Coverage') == -1 && obj.Status.indexOf('Active Soon')) {
                    returnList.push({
                        Id: obj.CYBPolicyNumber,
                        PolicyCode: obj.PolicyCode + ' (' + $filter('date')(new Date(parseInt(obj.PolicyBegin.match(/\d/g).join(""))), 'dd-MMM-yyyy') + ' to ' + $filter('date')(new Date(parseInt(obj.PolicyExpires.match(/\d/g).join(""))), 'dd-MMM-yyyy') + ')',
                        Status: obj.Status
                    });
                    //}
                });
            }
            return returnList;
        }

        // Method is used to filter the policies based on line of business
        $scope.FilterByLineOfBusiness = function () {
            if ($scope.lineType != undefined) {
                loadingService.showLoader();

                if (mainList.length > 0) {
                    $scope.drpPolicyList = $scope.combinePolicyCodeAndDate($scope.filterList(mainList, $scope.lineType.Id));
                    $scope.hasErrorMain = false;
                    //if ($scope.drpPolicyList.length == 0) {
                    //    $scope.hasErrorMain = true;
                    //}
                    loadingService.hideLoader();
                } else {
                    var wrapper = { "method": "GetYourPolicies" };
                    authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                        if (response.success) {
                            var data = response.yourPolicies;
                            mainList = data;
                            if (data != null) {
                                $scope.drpPolicyList = $scope.combinePolicyCodeAndDate($scope.filterList(data, $scope.lineType.Id));
                            }

                            $scope.hasErrorMain = false;
                            //if ($scope.drpPolicyList.length == 0) {
                            //    $scope.hasErrorMain = true;
                            //}
                            loadingService.hideLoader();

                        } else {
                            loadingService.hideLoader();
                            window.location.href = baseUrl;
                        }
                    });
                }
            }
        }

        // Rediect to respected module.
        $scope.GoTo = function () {
            if ($scope.rptFrm.$valid) {
                $scope.hasErrorMain = false;
                // Allow Report a claim based on below policy status
                if (!($scope.policyType.Status == 'Active' || $scope.policyType.Status.indexOf('Expired') > -1 || $scope.policyType.Status.indexOf('Pending Cancellation') > -1 || $scope.policyType.Status.indexOf('Cancelled') > -1)){
                    $scope.hasErrorMain = true;
                } else {
                    loadingService.showLoader();
                    window.name = $scope.policyType.Id + "CYB" + $scope.policyType.PolicyCode.split("(")[0].trim() + "CYB" + $scope.policyType.Status;
                    $location.path("/ReportClaim");
                    sideMenuReload = true;
                    //SetNavigation('ReportClaim');
                }
            }
        }

        $scope.getPolicyInfo = function () {
            sharedUserService.decryptCYBKey(window.name.split("CYB")[0]).then(function (response) {
                if (response.status) {
                    $scope.status = true;
                    $scope.policyCode = response.policyInformation.PolicyCode;
                    $scope.Type = response.policyInformation.LOB === 1 ? "wc" : "bp"
                    $scope.nameOfBusiness = response.policyInformation.BusinessName;
                    $scope.defaultOrganization = response.policyInformation.BusinessName;
                    $scope.minDate = sharedUserService.convertToDate(response.policyInformation.EffectiveDate);

                    $scope.nameOfInjuredWorker = ($scope.Type == 'wc') ? null : 'bp';

                } else if (response.redirectStatus) {
                    $location.path("/Error/");
                } else {
                    $scope.status = false;
                    $scope.errorMessage = response.errorMessage;
                }
            });
        }

        $scope.loadUrl = function () {
            if (currenturl == "/ReportClaim") {
                $scope.getPolicyInfo();
                sidemenustate('ReportClaim');

                //var wrapper = { "method": "GetCurrentUserData" };
                //authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                //    if (response.success) {
                //        $scope.nameOfBusiness = response.user.OrganizationName;
                //        $scope.defaultOrganization = response.user.OrganizationName;
                //        sidemenustate('ReportClaim');
                //    }
                //    loadingService.hideLoader();
                //});

            } else if (currenturl == "/ReportClaimFromHeader") {
                nosidemenu(true);                
            }

            loadingService.hideLoader();
        }
        

        // Controler funciton End Please dont write any methods/functions after this line.         
    }
})();