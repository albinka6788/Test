(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module("BHIC.WC.Controllers").controller('businessInfoController', ['$scope', '$filter', 'businessInfoService', '$location', 'loadingService', '$timeout', 'coreUtils', 'appConfig', businessInfoControllerFn]);

        //controller function
        function businessInfoControllerFn(scope, filter, businessInfoService, location, loadingService, timeout, coreUtilityMethod, appConfig) {
            var self = this;

            var _businessInfoVm =
            {
                companyName: '',
                zipCode: '',
                contactEmail: '',
                contactPhone: '',
                contactName: '',
                address1: '',
                cityName: '',
                stateCode: '',
                lobId: '',
                lobList: [],
                cityList: [],
                lobStatus: '',
                invalidLobId: 0,
                selectedCity: {},
                showCity: false
            };

            //Comment : Here to enable submit button & allow submission of question form responses
            var _formSubmitted = false;
            var _listOfErrors = [];
            var _emailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
            var _hasValidForm = false;

            function _init(BusinessInfoVm) {
                //debugger;
                //Comment : Here on page load DEFAULT setting
                _resetToDefault();
                loadingService.hideLoader();

                //Comment : Here must check for any user system errors if any
                var errorMessages = BusinessInfoVm.Messages;

                if (errorMessages != null && errorMessages.length > 0) {
                    //Comment : Here iterate errors list and display it to screen
                    self.listOfErrors = errorMessages;
                }
                else {
                    //Comment : Here bind VM into scope
                    _getBusinessInfo(BusinessInfoVm);
                    //updateSelectedLobStatus(BusinessInfoVm.LobList, BusinessInfoVm.LobId) //pending filter
                }
            };

            var _getBusinessInfo = function (data) {
                //Comment : Here reset 
                self.businessInfoVm = {};

                if (data != null && data != 'null' && data != '') {
                    var businessInfo = JSON.parse(JSON.stringify(data));
                    //console.log(businessInfo.LobList);
                    //console.log(JSON.parse(businessInfo.LobList));

                    //get lists
                    var lobList = businessInfo.LobList;
                    /*
                    var cityList = businessInfo.CityList;
                    var cities = (cityList != null & cityList != 'undefined' & cityList != '') ? JSON.parse(cityList) : [];

                    //Comment : Here get previuosly selected City name from this dat to show auto-selected dropdwon
                    var filteredCity = (cities != undefined & cities.length > 0 & businessInfo.City != '') ? filter('filter')(cities, { City: businessInfo.City }) : {};*/

                    //Comment : Here to resolve caption mapping problem e.g. localObject.campanyName : serverObject.CompnayName
                    self.businessInfoVm =
                    {
                        companyName: businessInfo.CompanyName,
                        zipCode: businessInfo.ZipCode,
                        contactEmail: businessInfo.Email,
                        contactPhone: businessInfo.PhoneNumber,
                        contactName: businessInfo.ContactName,
                        //address1: businessInfo.Address1,
                        //cityName: businessInfo.City,
                        stateCode: businessInfo.StateCode,
                        lobId: businessInfo.LobId,
                        lobList: (lobList != null & lobList != 'undefined' & lobList != '') ? JSON.parse(lobList) : [],
                        //cityList: cities,
                        lobStatus: '',
                        invalidLobId: 0
                        //selectedCity: (typeof filteredCity != 'undefined' & filteredCity.length > 0) ? filteredCity[0] : {},
                        //showCity: (typeof cities != 'undefined' & cities.length > 1) ? true : false
                    };

                    //set other default values
                    _resetToDefault();
                }
            };

            var _submitBusinessInfo = function (form_businessInfo, businessInfoVm) {
                //debugger;

                //reset error list
                self.listOfErrors = [];

                //Comment : Here check for all validation are successfully processed then proceed further
                var hasPageAjaxCallPending = coreUtilityMethod.BackgroundProcessExecuting();
                var isValidZipCode = (businessInfoVm.zipCode != '' && coreUtilityMethod.ConvertToNumeric(coreUtilityMethod.Trim(businessInfoVm.zipCode))).length == 5;
                var hasValidForm = form_businessInfo.$valid && self.businessInfoVm.invalidLobId == 0 && isValidZipCode;

                if (hasValidForm && !hasPageAjaxCallPending)  //And also no invalid LOB selection made by user
                {
                    loadingService.showLoader();

                    //Comment : Here map feild data
                    var businessInfoMappedVm = {
                        CompanyName: businessInfoVm.companyName,
                        ZipCode: businessInfoVm.zipCode,
                        Email: businessInfoVm.contactEmail,
                        PhoneNumber: businessInfoVm.contactPhone,
                        ContactName: businessInfoVm.contactName,
                        //Address1: businessInfoVm.address1,
                        //City: businessInfoVm.cityName,
                        City: (businessInfoVm.selectedCity != null & businessInfoVm.selectedCity != undefined) ? businessInfoVm.selectedCity.City : '',
                        StateCode: businessInfoVm.stateCode,
                        LobId: businessInfoVm.lobId
                    };

                    //Comment : here call service to submit questions response                    
                    var promise = businessInfoService.submitBusinessInfo(businessInfoMappedVm);

                    promise
                        .then(function (data) {
                            if (data.resultStatus == 'OK') {
                                if (typeof data.resultUrl != 'undefined') {
                                    //If WC then just change next view '/RelativePath'
                                    if (businessInfoVm.lobId == '1') {
                                        location.path(data.resultUrl);
                                    }
                                    else {
                                        window.location.href = data.resultUrl;
                                    }
                                }
                                //location.path("/GetExposureDetails/");
                            }
                            else if (data.resultStatus == 'NOK') {
                                //Comment : Here must check for any user system errors if any
                                var errorMessages = data.resultMessages;

                                if (errorMessages != undefined && errorMessages != null && errorMessages.length == 0) {
                                    location.path("/AppError/");
                                }
                                else {
                                    //Comment : Here iterate errors list and display it to screen
                                    self.listOfErrors = errorMessages;
                                    coreUtilityMethod.ScrollToTop();
                                }
                            }

                            //Comment : Here reset form model to clear and then set from state as PRISTINE to hide all validation messages
                            //self.businessInfoVm = {};
                            //form_businessInfo.$setPristine();
                            self.formSubmitted = false;
                        })
                        .catch(function (exception) {
                            //console.log(exception.toUpperCase());
                            location.path("/AppError/");
                        })
                        .finally(function () {
                            loadingService.hideLoader();
                        });
                }
                else if (!isValidZipCode || form_businessInfo.zipCodeText.$invalid) {
                    if (!form_businessInfo.zipCodeText.$invalid) {
                        form_businessInfo.zipCodeText.$setValidity('invalidZipCode', false);
                    }
                    coreUtilityMethod.ScrollToTop();
                }
                else if (hasPageAjaxCallPending) {
                    console.log('Wait');
                }
                else if (form_businessInfo.companyNameText.$invalid || form_businessInfo.contactEmailText.$invalid) {
                    coreUtilityMethod.ScrollToTop();
                }
            };

            var _updateSelectedLobStatus = function (selectedLob) {
                //Comment : Here in bigning RESET master decision values for "LOB Selection"
                _resetToDefault();

                if (selectedLob) {
                    var lobId = $(selectedLob.target).val();

                    //get lob list
                    var lobList = self.businessInfoVm.lobList;

                    //Comment : Here rather than iterating all LobStatus just get target LOB based on user selected lob-id
                    var filteredLob = (typeof lobList != 'undefined' & lobList.length > 0) ? filter('filter')(lobList, { Id: lobId | 0 }) : {};
                    //filter('filter')(self.businessInfoVm.lobList, { Id: lobId | 0 });

                    if (typeof filteredLob != 'undefined' & filteredLob.length > 0) {
                        //Comment : Here based selected LOB/Item set "Product Availibility Status"
                        var lobStatus = filteredLob[0].Status;
                        if (lobStatus != null & lobStatus != '' & lobStatus.toUpperCase() != 'AVAILABLE') {
                            self.businessInfoVm.lobStatus = lobStatus;
                            self.businessInfoVm.invalidLobId = filteredLob[0].Id;
                        }
                    }
                }
            };

            var _setLobsStatus = function (lobId) {
                var retStatus = false;

                if (lobId) {
                    //get lob list
                    var lobList = self.businessInfoVm.lobList;

                    //Comment : Here rather than iterating all LobStatus just get target LOB based on user selected lob-id
                    var filteredLob = (typeof lobList != 'undefined' & lobList.length > 0) ? filter('filter')(lobList, { Id: lobId | 0 }) : {};

                    if (typeof filteredLob != 'undefined' & filteredLob.length > 0) {
                        //Comment : Here based selected LOB/Item set "Product Availibility Status"
                        var lobStatus = filteredLob[0].Status;
                        if (lobStatus != null & lobStatus != '' & lobStatus.toUpperCase() != 'AVAILABLE') {
                            //self.businessInfoVm.lobStatus = lobStatus;
                            //self.businessInfoVm.invalidLobId = filteredLob[0].Id;
                            retStatus = true;
                        }
                    }

                    return ((typeof lobList == 'undefined' | lobList.length == 0) ? true : retStatus);
                }
            };

            var _setLobsMessages = function (lobId) {
                var retStatus = '';

                if (lobId) {
                    //get lob list
                    var lobList = self.businessInfoVm.lobList;

                    //Comment : Here rather than iterating all LobStatus just get target LOB based on user selected lob-id
                    var filteredLob = (typeof lobList != 'undefined' & lobList.length > 0) ? filter('filter')(lobList, { Id: lobId | 0 }) : {};

                    if (typeof filteredLob != 'undefined' & filteredLob.length > 0) {
                        //Comment : Here based selected LOB/Item set "Product Availibility Status"
                        var lobStatus = filteredLob[0].Status;
                        if (lobStatus != null & lobStatus != '' & lobStatus.toUpperCase() != 'AVAILABLE') {
                            retStatus = ' - ' + lobStatus;
                        }
                    }

                    return retStatus;
                }
            };

            function _resetToDefault() {
                self.businessInfoVm.lobStatus = '';
                self.businessInfoVm.invalidLobId = 0;
            }


            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(self, {
                init: _init,
                businessInfoVm: _businessInfoVm,
                formSubmitted: _formSubmitted,
                submitBusinessInfoData: _submitBusinessInfo,
                listOfErrors: _listOfErrors,
                ValidEmailRegEx: _emailRegEx,
                updateSelectedLobStatus: _updateSelectedLobStatus,
                setLobsStatus: _setLobsStatus,
                setLobsMessages: _setLobsMessages
            });

        };
    }
)();