(function () {
    'use strict';
    angular.module("BHIC.Dashboard.Controllers").controller('savedQuotesController', ['$scope', '$location', '$filter', '$window', 'loadingService', 'savedQuotesData', 'authorisedUserWrapperService', savedQuotesControllerFn])
    function savedQuotesControllerFn($scope, $location, $filter, $window, loadingService, savedQuotesData, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.
        $scope.convertToDate = function (inputString) {
            if (inputString === null) {
                return "-";
            }
            else {
                var convertedDate = new Date(parseInt(inputString.match(/\d/g).join("")));
                return $filter('date')(convertedDate, 'MM/dd/yyyy');
            }
        }

        $scope.status = savedQuotesData.success;
        $scope.quotesList = (savedQuotesData.success) ? savedQuotesData.quotes : [];
        $scope.errorMessage = (savedQuotesData.success) ? null : " Server Response : " + savedQuotesData.errorMessage;
        nosidemenu(true);
        $scope.purchasePathLinkedQuoteUrl = '';
        loadingService.hideLoader();
                
        //sorting and paging for table, -1 is index start from right side, to disable sorting for a particular column
        bindDatatable('savedQuotes', -1, null);
                
        $scope.checkUserStatus = function (quoteId,lob, retrieveQuoteURL) {
            loadingService.showLoader();
            if (lob==1)
                window.location.href = $scope.purchasePathLinkedQuoteUrl += quoteId;
            else if (lob == 2 && retrieveQuoteURL != null && retrieveQuoteURL!="")
                window.location.href = retrieveQuoteURL;
        }

        $scope.init = function (baseUrl) {
            $scope.purchasePathLinkedQuoteUrl = baseUrl;
        }

        $scope.setDeleteQuote = function (quoteId, index) {
            $scope.deleteQuoteId = quoteId;
            $scope.deleteRowIndex = index;
        }

        $scope.deleteQuote = function () {
            var _userdetails = {};
            _userdetails.EncryptedQuoteID = $scope.deleteQuoteId;
            loadingService.showLoader();            
            var wrapper = { "method": "DeleteQuote", "postData": _userdetails }
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.quotesList.splice($scope.deleteRowIndex, 1);
                }
                DeleteRowFromSavedQuoteTable('savedQuotes', $scope.deleteRowIndex);
                loadingService.hideLoader();
            });
        }

        $scope.removeCookie = function (url) {
            loadingService.showLoader();
            var wrapper = { "method": "DeleteCookie" }
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    window.location.href = url;
                }
                //loadingService.hideLoader();
            });

        }

        // Controler funciton End Please dont write any methods/functions after this line.
    }
})();