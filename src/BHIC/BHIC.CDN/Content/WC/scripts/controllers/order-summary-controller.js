(
    function () {

        'use strict';

        //module controller declaration and injection
        angular.module("BHIC.WC.Controllers")
            .controller('orderSummaryController', ['$scope', 'orderSummaryService', '$location', 'loadingService', orderSummaryControllerFn]);

        //controller function
        function orderSummaryControllerFn(scope, orderSummaryDataProvider, location, loadingService) {

            // Variable declaration
            scope.order = {
                ProductName: '',
                MgaCode: '',
                PaymentConfirmationNumber: '',
                PhoneNumber: '',
                CompanyDomain:'',
                IsPaymentSuccess: '',
                PolicyCentreURL: '',
                IsSuccess: false,
                IsProcessing: true
            };

            scope.listOfErrors = [];

            function _init() {
                loadingService.hideLoader();
                scope.order.IsProcessing = true;
                _getOrderDetails();
            }

            //Comment : Here default count down timer seconds
            var _timeCountDown = 20;
            var _updateTimerAfter = 1000;
            var _i = 1;

            var _removeHistory = function () {
                //debugger;
                _timeCountDown--;

                //console.log(_timeCountDown);

                var history_api = typeof history.pushState !== 'undefined';
                var path = '';
                path = window.location.href;

                // history.pushState must be called out side of AngularJS Code
                if (history_api) {
                    while (_i < 25) {
                        history.pushState(null, '', path);
                        _i++;
                    }
                };
            }


            //initialize values on load
            var _getOrderDetails = function () {

                //get payment plans
                var promise = orderSummaryDataProvider.getOrderSummary();

                promise
                      .then(function (result) {

                          if (result.isSuccess) {
                              scope.order = angular.copy(result.data);
                              scope.order.IsProcessing = false;
                              scope.order.IsSuccess = true;

                              //Add Google Anaytics tracker for Policy Creation
                              if (scope.order.IsPaymentSuccess == 2) {
                                  window.ga('send', 'pageview', { page: '/PolicyIssued' });
                              }
                              else {
                                  window.ga('send', 'pageview', { page: '/PolicyCreationFailed' });
                              }

                              //Comment : Here on page load show session-expired view with timer then redirect user to home page
                              setTimeout(_removeHistory(), _updateTimerAfter);
                          }
                          else {

                              //Comment : Here must check for any user system errors if any
                              var errorMessages = result.resultMessages;

                              if (errorMessages != undefined && errorMessages != null && errorMessages.length > 0)
                              {
                                  scope.listOfErrors = errorMessages;
                                  scope.order.IsProcessing = false;
                                  scope.order.IsSuccess = true;
                              }
                              else if (result.isSessionExpired) {
                                  location.path("/SessionExpired/");
                              }
                              else {
                                  location.path("/AppError/");
                              }
                          }
                      })
                      .catch(function (exception) {
                          location.path("/AppError/");
                      })
                      .finally(function () {
                          scope.order.IsProcessing = false;
                          loadingService.hideLoader();
                      });
            };

            _init();
        };
    }
)();