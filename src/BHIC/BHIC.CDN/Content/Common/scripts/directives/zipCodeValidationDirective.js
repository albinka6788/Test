(function () {
    'use strict';

    //valid number directive definition
    angular.module('BHIC.Controllers').directive('validateZip', ['landingPageData', 'loadingService', '$timeout', 'coreUtils', 'appConfig', validateZipControlFn]);

    function validateZipControlFn(landingPageDataProvider, loadingServiceProvider, $timeout, coreUtils, appConfig) {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (inputValue) {

                    var transformedInput = inputValue.replace(/[^0-9]/g, '');
                    if (transformedInput != inputValue) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }

                    //Set validation properties to true, by default
                    modelCtrl.$setValidity('invalidZip', true);

                    scope.landingModel.isDisabled = true;
                    //scope.landingModel.isValidLob = false;

                    scope.landingModel.product = '';
                    scope.landingModel.existingLob = '';
                    scope.landingModel.isStateVisible = false;
                    scope.landingModel.hideElement = "hide";
                    scope.landingModel.availableWC = "";
                    scope.landingModel.availableBOP = "";
                    scope.landingModel.availableCA = "";
                    //check whether zip code exists, using appropriate api
                    if (inputValue.length == 5) {

                        coreUtils.controlLevelLoader($('#txtZipCode'), true);

                        //validate zip code
                        var promise = landingPageDataProvider.getValidZip(inputValue);

                        promise
                              .then(function (result) {
                                  //Comment : Here otherwise do as it was
                                  if (result.isSuccess) {

                                      modelCtrl.$setValidity('invalidZip', true);
                                      scope.landingModel.county = result.county;
                                      scope.landingModel.hideElement = "";

                                      if (result.county.length > 1) {
                                          scope.landingModel.isStateVisible = true;
                                      }
                                      else {

                                          scope.landingModel.isDisabled = false;
                                          scope.landingModel.isStateVisible = false;

                                          //hide state drop down and set first item as default
                                          scope.selectedItem = result.county[0];

                                          if (scope.selectedItem != undefined) {

                                              scope.landingModel.existingLob = result.lobResult.Data.lob;

                                              angular.forEach(result.lobResult.Data.lob, function (value, key) {

                                                  //add available class to all lob, which are availables in current state
                                                  if (value.Id == "1" && value.Status.toLowerCase() == "available") {
                                                      scope.landingModel.availableWC = "available";
                                                  }
                                                  else if (value.Id == "2" && value.Status.toLowerCase() == "available" && scope.landingModel.isBOPEnabled.toUpperCase() == 'TRUE') {
                                                      scope.landingModel.availableBOP = "available";
                                                  }
                                                  else if (value.Id == "3" && value.Status.toLowerCase() == "available" && scope.landingModel.isCAEnabled.toUpperCase() == 'TRUE') {
                                                      scope.landingModel.availableCA = "available";
                                                  }
                                              });
                                          }
                                      }

                                  }
                                  else {

                                      //Comment : Here must intercept the response if any ForgoryToken not found issue 
                                      //if (typeof (result) == 'string' && (result.indexOf('<meta name="AppAuthTokenNotFound" content="true">') != -1))
                                      if (typeof (result) == 'object' && (result.isSuccess == undefined || result.isSuccess == 'undefined'))
                                      {
                                          scope.landingModel.isStateVisible = false;
                                          modelCtrl.$setValidity('invalidZip', true);
                                          window.location.href = appConfig.appBaseUrl;
                                          //window.location.href = '/PurchasePath/Home/Index';
                                      }
                                      else {
                                          scope.landingModel.isStateVisible = false;
                                          modelCtrl.$setValidity('invalidZip', false);
                                      }
                                  }
                              })
                              .catch(function (exception) {
                                  console.log(exception.toUpperCase());
                                  window.location.href = '/PurchasePath/Quote/Index#/AppError';
                              })
                              .finally(function () {
                                  coreUtils.controlLevelLoader($('#txtZipCode'), false);
                              });
                    }
                    return transformedInput;
                });
            }
        };
    }

})();