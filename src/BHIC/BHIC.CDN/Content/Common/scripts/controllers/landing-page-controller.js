(function () {
    'use strict';

    angular.module('BHIC.Controllers').controller('landingPageController', ['$http', '$scope', '$timeout', 'landingPageData', 'loadingService', 'appConfig', 'coreUtils', landingPageControllerFunction]);

    function landingPageControllerFunction($http, $scope, $timeout, landingPageDataProvider, loadingServiceProvider, appConfig, coreUtils) {
        /* Variable declaration */

        //to check button click
        $scope.formSubmitted = false;

        $scope.changeButtonText = function (sender) {
            var control = $(sender.target);
            if (typeof control != 'undefined') {
                control.addClass('btn-loading');
                control.val('Please wait ...');
            }
        }

        $scope.submitLandingPageWC = function () {
            //debugger;
            var promise = landingPageDataProvider.getNewQuote();

            promise
                .then(function (result) {
                    if (result.isSuccess) {
                        window.location.href = '/PurchasePath/Quote/Index';
                    }
                    else {
                        window.location.href = '/PurchasePath/Home/Index#/AppError';
                    }
                })
                .catch(function (exception) {
                    console.log(exception.toUpperCase());
                    window.location.href = '/PurchasePath/Home/Index#/AppError';
                })
                .finally(function () {
                });
        }

        $scope.landingModel = {
            zip: '',
            county: '',
            product: '',
            productList: [{
                id: 1,
                name: "WC",
                value: 1,
                imgSrc: appConfig.appBaseDomain + '/Content/Common/images/ico-wc.png',
                description: "Workers' Compensation",
                status: "",
                isDisabled: "false"
            }, {
                id: 2,
                name: "BOP",
                value: 2,
                imgSrc: appConfig.appBaseDomain + '/Content/Common/images/ico-bop.png',
                description: "Business Owner's Policy",
                status: "",
            }, {
                id: 3,
                name: "CA",
                value: 3,
                imgSrc: appConfig.appBaseDomain + '/Content/Common/images/ico-ca.png',
                description: "Commercial Auto",
                status: "",
            }],
            isStateVisible: false,
            isValidLob: true,
            existingLob: '',
            isDisabled: true,
            isBOPEnabled: false,
            isCAEnabled: false,
            availableWC: "",
            availableBOP: "",
            availableCA: "",
            hideElement: "hide",
            lobErrorMsg: "",
        };


        function _init() {
            //$http.get('/PurchasePath/Home/GetValidZipDetail?zipCode=99572').then(
            //    function (res) {
            //        console.log('Successfully executed !!');
            //    });

            loadingServiceProvider.hideLoader();

            $scope.landingModel.zip = '';
            $scope.selectedItem = '';
            $scope.isComingSoonProduct = false;
            $scope.landingModel.availableWC = "";
            $scope.landingModel.availableBOP = "";
            $scope.landingModel.availableCA = "";

            $scope.setProductAvailability(enableBOP, enableCA);

            //Comment : Here check for session exists then set LOB
            if (homeVM != null && homeVM != undefined && homeVM.sessionData != null & homeVM.sessionData != undefined) {

                coreUtils.controlLevelLoader($('#txtZipCode'), true);

                //Comment : Here collect all session realted data into local variable
                var result = homeVM.sessionData.data;

                //If result found then 
                if (result.isSuccess) {
                    $scope.landingModel.hideElement = "";
                    $scope.landingModel.isDisabled = false;
                    $scope.landingModel.zip = result.county[0].zipCode;

                    //Comment : Here get all counties list
                    var counties = result.county;
                    $scope.landingModel.county = counties;

                    if (counties.length > 1) {

                        angular.forEach(counties, function (value, key) {

                            var selectedItem = _.find(counties, function (item) {
                                return item.StateCode == result.selectedState;
                            });

                            if (selectedItem != undefined) {
                                $scope.selectedItem = selectedItem;
                            }
                        });

                        $scope.landingModel.isStateVisible = true;
                    }
                    else if (counties.length == 1) {
                        //set first item as default
                        $scope.selectedItem = counties[0];
                    }

                    SetLobData(result.lobResult.data.lob);

                    $scope.landingModel.product = result.selectedLob;
                    // $scope.changeState();
                }

                coreUtils.controlLevelLoader($('#txtZipCode'), false);
            }

            //var promise = landingPageDataProvider.getSessionData();
            //promise
            //        .then(function (result) {
            //            // $scope.landingModel.isValidLob = false;

            //            if (result.isSuccess) {
            //                $scope.landingModel.hideElement="";
            //                $scope.landingModel.isDisabled = false;
            //                $scope.landingModel.zip = result.county[0].ZipCode;

            //                $scope.landingModel.county = result.county;

            //                if (result.county.length > 1) {

            //                    angular.forEach(result.county, function (value, key) {

            //                        var selectedItem = _.find(result.county, function (item) {
            //                            return item.StateCode == result.selectedState;
            //                        });

            //                        if (selectedItem != undefined) {
            //                            $scope.selectedItem = selectedItem;
            //                        }
            //                    });

            //                    $scope.landingModel.isStateVisible = true;
            //                }
            //                else {
            //                    //set first item as default
            //                    $scope.selectedItem = result.county[0];
            //                }

            //                SetLobData(result.lobResult.Data.lob);

            //                $scope.landingModel.product = result.selectedLob;
            //                // $scope.changeState();
            //            }
            //        })
            //        .catch(function (exception) {
            //            console.log(exception.toUpperCase());
            //            window.location.href = '/PurchasePath/Quote/Index#/AppError';
            //        })
            //        .finally(function () {
            //        });
        }

        /* End of variable declaration */
        /* Public Methods */

        //Send zip code value to controller
        function submit(productId) {

            if (productId == "1") {

                var stateCode = '';
                if ($scope.selectedItem.stateCode == undefined) {
                    stateCode = $scope.selectedItem.StateCode;
                }
                else {
                    stateCode = $scope.selectedItem.stateCode;
                }
                var promise = landingPageDataProvider.saveZipAndState($scope.landingModel.zip, stateCode, productId);
                promise
                        .then(function (result) {
                            if (result.isSuccess) {
                                window.location.href = appConfig.appBaseDomain + '/Quote/Index';
                            }
                            else {
                                window.location.href = appConfig.appBaseDomain + '/Quote/Index#/AppError';
                            }
                        })
                        .catch(function (exception) {
                            console.log(exception.toUpperCase());
                            window.location.href = appConfig.appBaseDomain + '/Quote/Index#/AppError';
                        })
                        .finally(function () {
                        });
            }
            else if (productId == "2") {

                var promise = landingPageDataProvider.getBOPUrl($scope.landingModel.zip, $scope.selectedItem.StateCode);
                promise
                        .then(function (result) {
                            if (result.isSuccess) {
                                window.location.href = result.bopPath;
                            }
                            else {
                                window.location.href = '/PurchasePath/Quote/Index#/AppError';
                            }
                        })
                        .catch(function (exception) {
                            console.log(exception.toUpperCase());
                            window.location.href = '/PurchasePath/Quote/Index#/AppError';
                        })
                        .finally(function () {
                        });
            }
            else if (productId == "3") {
                var promise = landingPageDataProvider.saveCASession($scope.landingModel.zip);
                promise
                        .then(function (result) {
                            if (result.isSuccess) {
                                window.location.href = '/PurchasePath/CAQuote/Index';
                            }
                            else {
                                window.location.href = '/PurchasePath/Home/Index#/AppError';
                            }
                        })
                        .catch(function (exception) {
                            console.log(exception.toUpperCase());
                            window.location.href = '/PurchasePath/Home/Index#/AppError';
                        })
                        .finally(function () {
                        });

            }
        };

        function EnableAllButtons() {
            //enable all line of business
            angular.forEach($scope.landingModel.productList, function (item, key) {
                // item.isDisabled = false;
            });
        }

        function DisableAllButtons() {
            //disable all line of business
            angular.forEach($scope.landingModel.productList, function (item, key) {
                item.isDisabled = true;
            });
        }

        $scope.changeState = function () {
            $scope.landingModel.product = '';
            $scope.landingModel.existingLob = '';
            $scope.landingModel.isDisabled = true;
            $scope.isComingSoonProduct = false;
            if ($scope.selectedItem != undefined) {

                var lobPromise = landingPageDataProvider.getLobList($scope.selectedItem.StateCode);

                lobPromise
                       .then(function (result) {
                           $timeout(function () {
                               if (result.isSuccess) {
                                   $scope.landingModel.isDisabled = false;
                                   SetLobData(result.lob);
                               }
                           });
                       })
                       .catch(function (exception) {
                           console.log(exception.toUpperCase());
                           window.location.href = '/PurchasePath/Quote/Index#/AppError';
                       })
                       .finally(function () {
                       });
            }
        }

        $scope.setProductAvailability = function (enableBOP, enableCA) {
            $scope.landingModel.isBOPEnabled = enableBOP;
            $scope.landingModel.isCAEnabled = enableCA;
        }

        //$scope.validateLob = function (targetId) {
        //    $scope.isComingSoonProduct = false;
        //    $scope.landingModel.isValidLob = true;

        //    if ($scope != undefined) {
        //        if ($scope.landingModel != undefined) {
        //            if (targetId != undefined && $scope.landingModel.zip.length == 5) {

        //                var isLob = _.find($scope.landingModel.existingLob, function (item) {
        //                    return item.Id == targetId;
        //                });

        //                $scope.landingModel.isValidLob = false;

        //                if (!((targetId == "2" && $scope.landingModel.isBOPEnabled.toUpperCase() == 'FALSE') || (targetId == "3" && $scope.landingModel.isCAEnabled.toUpperCase() == 'FALSE'))) {

        //                    if (isLob) {
        //                        $scope.landingModel.isValidLob = true;
        //                        submit(targetId);
        //                    }
        //                }
        //                else {
        //                    $scope.isComingSoonProduct = true;
        //                }
        //            }
        //        }
        //    }
        //}

        $scope.validateLob = function (targetId) {

            if (!coreUtils.BackgroundProcessExecuting()) {
                $scope.isComingSoonProduct = false;
                $scope.landingModel.isValidLob = true;

                if ($scope != undefined) {
                    if ($scope.landingModel != undefined) {
                        if (targetId != undefined && $scope.landingModel.zip != undefined && $scope.landingModel.zip.length == 5 && $scope.landingModel.existingLob != undefined && $scope.landingModel.existingLob != "") {

                            var currentLob = _.find($scope.landingModel.existingLob, function (item) {
                                if (item.Id != undefined && item.Id == targetId) {
                                    return item;
                                }
                                if (item.id != undefined && item.id == targetId) {
                                    return item;
                                }
                            });

                            if (currentLob != undefined &&
                                    (!((targetId == "2" && $scope.landingModel.isBOPEnabled.toUpperCase() == 'FALSE') || (targetId == "3" && $scope.landingModel.isCAEnabled.toUpperCase() == 'FALSE')))) {

                                if (currentLob.status == undefined) {
                                    if (currentLob.Status.toLowerCase() != "available") {
                                        $scope.landingModel.isValidLob = false;
                                        $scope.landingModel.lobErrorMsg = currentLob.Status;
                                    }
                                    else if (currentLob.Status.toLowerCase() == "available") {
                                        $scope.landingModel.isValidLob = true;
                                        submit(targetId);
                                    }
                                }
                                else {
                                    if (currentLob.status.toLowerCase() != "available") {
                                        $scope.landingModel.isValidLob = false;
                                        $scope.landingModel.lobErrorMsg = currentLob.status;
                                    }
                                    else if (currentLob.status.toLowerCase() == "available") {
                                        $scope.landingModel.isValidLob = true;
                                        submit(targetId);
                                    }
                                }
                            }
                            else {
                                $scope.landingModel.isValidLob = false;
                                $scope.landingModel.lobErrorMsg = "Coming Soon";
                            }
                        }
                    }
                }
            }
        }

        function SetLobData(lob) {

            //debugger;
            $scope.landingModel.existingLob = lob;

            angular.forEach(lob, function (value, key) {

                //add available class to all lob, which are availables in current state
                if ((value.Id == "1" && value.Status.toLowerCase() == "available") || (value.id == "1" && value.status.toLowerCase() == "available")) {
                    $scope.landingModel.availableWC = "available";
                }
                else if (((value.Id == "2" && value.Status.toLowerCase() == "available") || (value.id == "2" && value.status.toLowerCase() == "available")) &&
                    ($scope.landingModel.isBOPEnabled.toUpperCase() != 'FALSE')) {
                    $scope.landingModel.availableBOP = "available";
                }
                else if (((value.Id == "3" && value.Status.toLowerCase() == "available") || (value.id == "3" && value.status.toLowerCase() == "available")) &&
                ($scope.landingModel.isCAEnabled.toUpperCase() != 'FALSE')) {
                    $scope.landingModel.availableCA = "available";
                }
            });
        }

        /* End of public methods declaration */

        //angular.extend($scope, {
        //    submitModel: _submit,
        //});

        //_init();
    }
})();