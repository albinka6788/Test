(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('uploadDocumentsController', ['$scope', '$http', '$filter', '$window', '$location', 'loadingService', 'authorisedUserWrapperService', uploadDocumentsControllerFn]);
    function uploadDocumentsControllerFn($scope, $http, $filter, $window, $location, loadingService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.

        // Controler funciton Start  write any methods/functions after this line.           
        var _self = this;
        $scope.responseMsg = false;
        $scope.focusField = true;
        $scope.emptyString = "";
        $scope.status = $scope.emptyString;
        $scope.errorMessage = $scope.emptyString;
        $scope.fileException = $scope.emptyString;
        var formdata, count = 0, valid = true;
        loadingService.hideLoader();

        $scope.defaultForm = function () {
            count = 0;
            $scope.description = $scope.emptyString;
            if (navigator.userAgent.indexOf('MSIE') > 0) {
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            }
            else {
                angular.element(document.getElementById('filesel')).val('');
            }
            formdata = $scope.emptyString;
            $scope.fileselected = $scope.emptyString;
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
        // method to use report claim form submission.
        $scope.submituploadForm = function () {
            loadingService.showLoader();
            //Making the display messages false
            $scope.responseMsg = false;
            $scope.errorMessage = $scope.emptyString;;
            $scope.fileException = $scope.emptyString;;
            $scope.phoneerrormsg = $scope.emptyString;

            _self.uploadDoc = {};
            _self.uploadDoc.Description = $scope.description;
            _self.uploadDoc.PolicyCode = window.name.split("CYB")[0];
            var request = {
                method: 'POST',
                url: baseUrl + "UploadDocuments/UploadDocuments?CYBKey=" + window.name.split("CYB")[0],
                data: formdata,
                headers: {
                    'Content-Type': undefined
                },
                transformRequest: angular.identity
            };

            var wrapper = { "method": "RequestUploadDocuments", "postData": _self.uploadDoc };
            if (formdata) {
                $http(request).success(function (response) {
                    if (response.success) {
                        authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (innerResponse) {
                            if (innerResponse.success) {
                                $scope.responseMsg = true;
                                $scope.defaultForm();
                                $scope.uploadForm.$setPristine();
                            } else if (response.redirectStatus) {
                                $location.path("/Error/");
                            } else {
                                $scope.hasErrorMain = true;
                                $scope.errorMessage = response.errorMessage;
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
                        $scope.errorMessage = response.errorMessage;
                        $('html, body').animate({ scrollTop: 0 }, 800);
                        $scope.uploadForm.$setPristine();
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
                $('html, body').animate({ scrollTop: 0 }, 800);
                loadingService.hideLoader();
                $scope.fileException = "Please select the file.";
            }
        }
    }
})();