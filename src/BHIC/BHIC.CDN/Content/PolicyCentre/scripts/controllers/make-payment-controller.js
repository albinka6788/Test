(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('makePaymentController', ['$scope', '$http', '$location', 'loadingService', '$timeout', '$filter', makePaymentControllerFunction]);
    function makePaymentControllerFunction($scope, $http, $location, loadingService, $timeout, $filter) {       
        loadingService.hideLoader();

        $scope.DownloadDocument = function (docID) {
            window.location.href = OriginLocation() + baseUrl + "Document/DownloadApiDocument?docId=" + docID;
        }
    }
})();