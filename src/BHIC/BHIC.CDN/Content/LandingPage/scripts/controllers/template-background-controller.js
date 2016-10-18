(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('templateBackgroundController', ['$scope', '$http', '$location', 'loadingService', 'authorisedUserWrapperService', templateBackgroundControllerFn]);

    function templateBackgroundControllerFn($scope, $http, $location, loadingService, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        $scope.msg;
        $scope.flag = false;
        $scope.fileTypes = ["jpg", "jpeg", "png", "gif"];
        var formdata = "";


        $scope.GoToLandingPage = function () {
            $location.path("/LandingPages");
        }; //validating and appending the files to FormData
        $scope.getTheFiles = function ($files) {
            formdata = new FormData();
            $scope.flag = false;
            if ($files.length > 0) {
                angular.forEach($files, function (value, key) {
                    formdata.append(value.name, value);
                    if (!$scope.flag && ($.inArray(value.name.split(".")[1], $scope.fileTypes) != -1)) {
                        $scope.flag = true;
                        $scope.btnDisabled = false;
                    }
                });
            }
        };


        $scope.AddTemplateLogo = function () {
            if ($scope.flag) {
                loadingService.showLoader();
                var request = {
                    method: 'POST',
                    url: window.location.origin + "/LandingPage/LandingPage/Login/PostDataForLandingPage",
                    data: formdata,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity
                };

                $http(request).success(function (response) {
                    if (response.success) {
                        //alert("Background  Image Uploaded Successfully.");
                        $('#filesel').val("");
                        $scope.msg = "Background Image Uploaded Successfully. ";
                        //$location.path("/LandingPages");
                    }
                    else {
                        console.log("Upload Image Response Error");
                        $scope.msg = response.msg;
                        //alert("Upload Image Response Error : " + response.msg);
                    }
                    loadingService.hideLoader();
                }).error(function (data, status, header, config) {
                    console.log("Please select an image");
                    //alert("Upload Image Error : " + data.msg);
                    $scope.msg = "Please select an image";
                });
            }
            else
                $scope.msg = " Please choose valid file format e.g :[.jpg, .jpeg, .png, .gif ]";
        };
        loadingService.hideLoader();
        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
