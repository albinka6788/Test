(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('policyDocumentController', ['$scope', '$location', 'loadingService', 'policyDocumentData', 'authorisedUserWrapperService', policyDocumentControllerFn]);
        function policyDocumentControllerFn($scope, $location, loadingService, policyDocumentData, authorisedUserWrapperService) {

            // Controler function Start  write any methods/functions after this line.
            $scope.CoveragesForm = "in";
            if (policyDocumentData.status) {
                $('html, body').animate({ scrollTop: 0 }, 800);
                $scope.policyDocuments = policyDocumentData.policyDocuments;
                $scope.policyStatus = ($scope.policyDocuments.length > 0) ? true : false;
            }
            else if (policyDocumentData.redirectStatus) {
                $location.path("/Error/");
                loadingService.hideLoader();
            }
            loadingService.hideLoader();

            $scope.goToWebsite = function () {
                $location.url('http://www.nlf-info.com');
            }
            
            $scope.DownloadDocument = function (docID) {
                window.location.href = OriginLocation() + baseUrl + "Document/DownloadApiDocument?docId=" + docID + "&CYBKey=" + window.name.split("CYB")[0];
            }

            // Controler funciton End Please dont write any methods/functions after this line.
        }
    })();