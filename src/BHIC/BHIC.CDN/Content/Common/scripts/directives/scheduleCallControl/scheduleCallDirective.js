/// <reference path="scheduleCallControlTemplate.html" />
(function () {
    'use strict';

    angular.module('BHIC.Directives')
     .directive('scheduleCall', ['appConfig', 'scheduleCallService', scheduleCallControlFn]);

    function scheduleCallControlFn(appConfig, scheduleCallService) {

        var scheduleCallController = ['$scope', function ($scope) {
            $scope.scheduleCallModel = {
                selectedCallOptions: [
                    {
                        id: "1",
                        value: "Now"
                    },
                    {
                        id: "2",
                        value: "in 15 mins"
                    },
                    {
                        id: "3",
                        value: "in 30 mins"
                    },
                    {
                        id: "4",
                        value: "in an hour"
                    },
                    {
                        id: "5",
                        value: "Enter time between 8.00am - 7.30 pm EST"
                    }],
                fullName: '',
                contactNumber: '',
                selectedRequestTimeOption: '1',
                selectedRequestTime: '',
                scheduleCallTime: '',
                isCustomCallRequest: false,
                submitted: false,
            };

            //Comment : Here to show form level errors
            $scope.scListOfErrors = [];

            $('.hoursBusiness').html(appConfig.hoursBusiness);

            $scope.resetModel = function () {
                $scope.scheduleCallModel.fullName = '';
                $scope.scheduleCallModel.contactNumber = '';
                $scope.scheduleCallModel.selectedRequestTimeOption = '1';
                $scope.scheduleCallModel.selectedRequestTime = '';
                $scope.scheduleCallModel.scheduleCallTime = '';
                $scope.scheduleCallModel.isCustomCallRequest = false;
                $scope.scheduleCallModel.submitted = false;
            }

            $scope.setCustomRequestedTime = function (selectedRequestTimeOption) {
                $scope.scheduleCallModel.selectedRequestTime = '';
                if (selectedRequestTimeOption == "5") {
                    $scope.scheduleCallModel.isCustomCallRequest = true;
                }
                else {
                    $scope.scheduleCallModel.isCustomCallRequest = false;
                }
            }

            //    $scope.validateScheduleCallTime = function () {

            //        var dt = new Date();

            //        $scope.scheduleCallFrm.selectedRequestTime.$setValidity("validScheduleCallTime", true);

            //        var minScheduledCall = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 8, 0, 0);
            //        var maxScheduledCall = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 19, 30, 0);

            //        $scope.scheduleCallModel.scheduleCallTime = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), $scope.scheduleCallModel.selectedRequestTime.getHours(),
            //        $scope.scheduleCallModel.selectedRequestTime.getMinutes(), $scope.scheduleCallModel.selectedRequestTime.getSeconds());

            //        if ($scope.scheduleCallModel.scheduleCallTime == undefined || $scope.scheduleCallModel.scheduleCallTime == "") {

            //            $scope.scheduleCallFrm.selectedRequestTime.$setValidity("validScheduleCallTime", false);
            //        }
            //        else {
            //            var scheduleCallDay = $scope.scheduleCallModel.scheduleCallTime.getDay();

            //            //set schedule call validity false, if given time does not lies between Monday - Friday, 8:00 am - 7:30 pm EST
            //            if (scheduleCallDay < 1 || scheduleCallDay > 5) {
            //                $scope.scheduleCallFrm.selectedRequestTime.$setValidity("validScheduleCallTime", false);
            //            }
            //            else if ($scope.scheduleCallModel.scheduleCallTime < minScheduledCall || $scope.scheduleCallModel.scheduleCallTime > maxScheduledCall) {
            //                $scope.scheduleCallFrm.selectedRequestTime.$setValidity("validScheduleCallTime", false);
            //        }
            //    }
            //}

            $scope.closeThankyouForm = function () {
                //Comment : Here first blank current page model object
                $scope.resetModel();

                //empty all errors
                $scope.scListOfErrors = [];

                //Comment : Here get model DIV object for ScheduleACall
                var modelBox = $('[data-remodal-id=scheduleCallModel]').remodal();

                //if not undefined
                if (modelBox) {
                    modelBox.close();
                }

                setTimeout(function () {
                    $("div.schedule-call-content").removeAttr("style");
                    $("div.d-tcell").addClass("hide");
                }, 100);

                return false;
            }

            $scope.submitModel = function () {
                //console.log($scope.scheduleCallModel.contactNumber);

                var promise = scheduleCallService.saveScheduleCall($scope.scheduleCallModel.fullName, $scope.scheduleCallModel.contactNumber,
                    $scope.scheduleCallModel.selectedRequestTimeOption, $scope.scheduleCallModel.scheduleCallTime);

                promise
                      .then(function (result) {
                          if (result.isSuccess) {
                              $('.schedule-call-content').hide();
                              $('.d-tcell').removeClass("hide");
                              return false;
                          }
                          else {

                              //Comment : Here must check for any user system errors if any
                              var errorMessages = result.resultMessages;

                              if (errorMessages != undefined && errorMessages != null && errorMessages.length > 0) {
                                  $scope.scListOfErrors = errorMessages;
                              }
                              else {
                                  window.location.href = '/PurchasePath/Quote/Index#/AppError';
                              }
                          }
                      })
                      .catch(function (exception) {
                          window.location.href = '/PurchasePath/Quote/Index#/AppError';
                      })
                      .finally(function () {
                      });
            }
        }];

        return {
            restrict: 'E', //Default in 1.3+
            templateUrl: appConfig.appCdnDomain + '/Content/Common/scripts/directives/scheduleCallControl/scheduleCallControlTemplate.html',
            controller: scheduleCallController
        };
    };
}());