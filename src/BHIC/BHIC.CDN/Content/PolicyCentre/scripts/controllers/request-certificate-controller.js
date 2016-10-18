(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('requestCertificateController', ['$scope', '$http', '$location', '$filter', 'loadingService', 'certificatesData', 'sharedUserService', 'authorisedUserWrapperService', recquestCertificateControllerFn]);
        function recquestCertificateControllerFn($scope, $http, $location, $filter, loadingService, certificatesData, sharedUserService, authorisedUserWrapperService) {

            // Controler funciton Start  write any methods/functions after this line.         
            var _self = this;
            $scope.certificateMsg = false;
            $scope.focusField = true;
            $scope.emptyString = "";
            $scope.defaultState = $scope.emptyString;
            $scope.certificatesList = [];
            $scope.PolicyCode = "";
            $scope.errorMessage = "";
            $scope.state = "F";

            $scope.getPolicyInfo = function () {
                sharedUserService.decryptCYBKey(window.name.split("CYB")[0]).then(function (response) {
                    if (response.status) {
                        $scope.status = true;
                        $scope.PolicyCode = response.policyInformation.PolicyCode;
                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {
                        $scope.status = false;
                        $scope.errorMessage = response.errorMessage;
                    }
                });
            }

            $scope.getPolicyInfo();

            $scope.convertToDate = function (inputString) {
                var convertedDate = new Date(parseInt(inputString.match(/\d/g).join("")));
                return $filter('date')(convertedDate, 'MM/dd/yyyy');
            }

            //On state change
            $scope.onStateChange = function () {
                if ($scope.state != undefined) {
                    $scope.state = $filter('uppercase')($scope.state);
                    if ($scope.state.length == 2) {
                        $scope.state === "NY" ? $scope.stateNY = true : $scope.stateNY = false;
                    }
                }
            };

            //Default Certificate Form for reset
            $scope.defaultForm = function () {
                $scope.fname = $scope.emptyString;
                $scope.address1 = $scope.emptyString;
                $scope.address2 = $scope.emptyString;
                $scope.city = $scope.emptyString;
                $scope.state = $scope.defaultState;
                $scope.zipCode = $scope.emptyString;
                $scope.submitted = false;
                $scope.phoneerrormsg = $scope.emptyString;
            };

            $scope.getContactInfo = function () {
                //$scope.username = certificatesData.user.FirstName + ' ' + certificatesData.user.LastName;
                $scope.username = certificatesData.user.Name;
                $scope.cphone = certificatesData.user.PhoneNumber;
                $scope.email = certificatesData.user.Email;

                var policystate = certificatesData.state;
                $scope.state = policystate;
                $scope.defaultState = policystate;
                $scope.state === "NY" ? $scope.stateNY = true : $scope.stateNY = false;
                loadingService.hideLoader();
            };


            // method to use report claim form submission.
            $scope.submitReportForm = function () {
                var currentDate = new Date();
                _self.CertificateOfInsurance = {};
                _self.CertificateOfInsurance.RequestId = ($scope.Id == undefined) ? 0 : $scope.Id;
                _self.CertificateOfInsurance.PolicyCode = window.name.split("CYB")[0];
                _self.CertificateOfInsurance.Type = null;
                _self.CertificateOfInsurance.UseLocNum = null;
                _self.CertificateOfInsurance.StartDate = null;
                _self.CertificateOfInsurance.EndDate = null;
                _self.CertificateOfInsurance.Name = $scope.fname;
                _self.CertificateOfInsurance.Address1 = $scope.address1;
                _self.CertificateOfInsurance.Address2 = $scope.address2;
                _self.CertificateOfInsurance.City = $scope.city;
                _self.CertificateOfInsurance.State = $scope.state;
                _self.CertificateOfInsurance.ZipCode = $scope.zipCode;
                _self.CertificateOfInsurance.UseLocNumDescription = null;
                _self.CertificateOfInsurance.RequestDate = null;
                _self.CertificateOfInsurance.EmailTo = $scope.cemail;

                // Method used to call service.
                loadingService.showLoader();
                var wrapper = { "method": "RequestCertificate", "postData": _self.CertificateOfInsurance };
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    loadingService.hideLoader();
                    if (response.success) {
                        $scope.status = true;
                        $scope.defaultForm();
                        $scope.certificateForm.$setPristine();

                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {
                        $scope.status = false;
                        $scope.phoneerrormsg = $scope.emptyString;
                        $scope.errorMessage = response.errorMessage;
                    }
                    $scope.certificateMsg = true;
                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                    $scope.focusField = true;
                });
            }


            $scope.DownloadDocument = function (docID) {
                window.location.href = OriginLocation() + baseUrl + "Document/DownloadApiDocument?docId=" + docID + "&CYBKey=" + window.name.split("CYB")[0];
            }


            if (certificatesData.status) {
                $scope.certificatesList = (certificatesData.status) ? certificatesData.certificates : [];
                $scope.errorMessage = (certificatesData.status) ? null : "Error in Response : " + certificatesData.errorMessage;

                //sorting and paging for table, -1 is index start from right side, to disable sorting for a particular column
                bindDatatable('certificate', null, null);
                $scope.getContactInfo();
            } else if (certificatesData.redirectStatus) {
                $location.path("/Error/");
            }
            else {

            }
            // Controller function End Please dont write any methods/functions after this line.

        }
    })();