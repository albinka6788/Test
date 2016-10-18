(function () {

    angular.module("BHIC.WC.Controllers").controller('exposureController', [
        '$scope',
        'exposureService',
        'navigationService',
        'coreUtils',
        //'quotePageDefaults',
        //'companionCodes',
        //'quoteViewModel',
        '$routeParams',
        'loadingService',
        '$location',
        '$filter',
        '$window',
        '$timeout',
        'appConfig',
        '$q',
        quoteControllerFn]);
    function quoteControllerFn(scope, exposureService, navigationService, coreUtils, routeParams, loadingService, $location, $filter, $window, $timeout, appConfig, $q) {
        var quoteViewModel = {};
        scope.coreUtils = coreUtils;
        scope.companionErrors = {};

        scope.init = function (quoteVm) {
            /*Added for showing other list after 4 secs*/
            scope.keywordsearchstarttime = new Date();
            scope.keywordsearchcounter = 0;
            /*End */
            quoteViewModel = quoteVm;
            scope.quoteViewModel = quoteViewModel;
            scope.directSales = '';
            
            if (scope.quoteViewModel.SelectedSearch && !coreUtils.IsEmptyObject(scope.quoteViewModel.Industry)) {
                //scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
            }
            else {
                if (!coreUtils.IsEmptyString(scope.quoteViewModel.ClassDescriptionKeywordId)) {
                    //scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                }
                scope.quoteViewModel.Industry = null;
                scope.quoteViewModel.SubIndustry = null;
                scope.quoteViewModel.Class = null;
                scope.quoteViewModel.SelectedSearch = 0;
            }
            scope.directSales = scope.quoteViewModel.BusinessClassDirectSales;
            if (scope.quoteViewModel && scope.quoteViewModel.Exposures && scope.quoteViewModel.Exposures.length > 0) {
                //scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                if (coreUtils.GetValidAmount(scope.quoteViewModel.TotalPayroll) > 50000) {
                    scope.minExpValidation = !scope.quoteViewModel.IsGoodStateApplicable;
                    scope.directSales = scope.quoteViewModel.SelectedSearch == 0 ? scope.quoteViewModel.BusinessClassDirectSales : scope.quoteViewModel.Class.DirectOK;
                }
                _.each(scope.quoteViewModel.CompClassData, function (item) {
                    scope.companionErrors[item.ClassCode] = {
                        amountExceedingTotalPayroll: false,
                        sumExceedingTotalPayroll: false
                    }
                })
            }
            if (scope.quoteViewModel.SelectedSearch == 1) {
                if (scope.quoteViewModel.SubIndustry) {
                    scope.showSubIndustry = true;
                }
                if (scope.quoteViewModel.Class) {
                    scope.showClass = true;
                }
            }

            //Comment : Here fetched current running QuoteId value
            appConfig.quoteId = scope.quoteViewModel.QuoteId;
            //Comment : Here as per Krishna's "Navigation Bar" implementation assigned list of links into scope
            scope.quoteViewModel.NavLinks = scope.quoteViewModel.NavigationLinks;

            //Comment : Here set user email-id from "BusinessInfo Page"
            appConfig.prospectInfoEmailId = scope.quoteViewModel.ProspectInfoEmail;
        }

        scope.validateBusinessName = function () {
            scope.businessBlured = true;
        }

        scope.getSubIndustries = function (industryId) {
            scope.quoteViewModel.CompClassData = [];
            scope.companionErrors = {};
            coreUtils.controlLevelLoader($('#ddlIndustries'), true);
            scope.showMinPayRollFirstWarning = false;
            scope.showMinPayRollSecondWarning = false;
            scope.quoteViewModel.OtherClassDesc = '';
            scope.quoteViewModel.TotalPayroll = '';
            //Comment : Here default set to hidden
            scope.quoteViewModel.SubIndustries = null;
            scope.quoteViewModel.SubIndustry = null;
            scope.showSubIndustry = false;
            scope.showClass = false;

            if (industryId && industryId > 0) {
                exposureService.getSubIndustries(industryId).
                then(function (data) {
                    scope.quoteViewModel.SubIndustries = data;
                    scope.showSubIndustry = true;
                    coreUtils.controlLevelLoader($('#ddlIndustries'), false);
                });
            }
            else {
                scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                coreUtils.controlLevelLoader($('#ddlIndustries'), false);
            }
            scope.$$phase || scope.$digest();
        };

        scope.getClass = function (subIndustryId) {
            scope.quoteViewModel.CompClassData = [];
            scope.companionErrors = {};
            coreUtils.controlLevelLoader($('#ddlSubIndustries'), true);
            scope.showMinPayRollFirstWarning = false;
            scope.showMinPayRollSecondWarning = false;
            scope.quoteViewModel.TotalPayroll = '';
            scope.quoteViewModel.Classes = null;
            scope.quoteViewModel.Class = null;
            scope.showClass = false;
            scope.class = null;
            if (scope.captureQuote && scope.captureQuote.class) {
                scope.captureQuote.class.$pristine = true
            }

            if (subIndustryId > 0) {
                scope.quoteViewModel.OtherClassDesc = '';
                exposureService.getClass(subIndustryId).
                then(function (data) {
                    scope.quoteViewModel.Classes = data;
                    scope.showClass = true;
                    scope.processingClass = false;
                    scope.quoteViewModel.Class = null;
                    coreUtils.controlLevelLoader($('#ddlSubIndustries'), false);
                });
            }
            else {
                coreUtils.controlLevelLoader($('#ddlSubIndustries'), false);
                scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                scope.quoteViewModel.Classes = null;
                scope.showClass = false;
            }
        };

        scope.validateExposureAmount = function (ctrlId) {
            var deferred = $q.defer();
            _isGoodState = null;
            scope.showMinPayRollFirstWarning = false;
            scope.showMinPayRollSecondWarning = false;
            scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
            scope.disableSubmit = true;
            var _classDescId, _classDescKeyId, _classCode, _industryId, _subIndustryId;
            if (scope.quoteViewModel.SelectedSearch == 0) {
                _classDescId = scope.quoteViewModel.ClassDescriptionId;
                _classDescKeyId = scope.quoteViewModel.ClassDescriptionKeywordId;
                _classCode = scope.quoteViewModel.ClassCode;
                scope.directSales = scope.quoteViewModel.BusinessClassDirectSales;
                _industryId = null;
                _subIndustryId = null;
            }
            else if (scope.quoteViewModel.SelectedSearch == 1 && scope.quoteViewModel.Class) {
                _classDescId = scope.quoteViewModel.Class ? scope.quoteViewModel.Class.ClassDescriptionId : null;
                _classDescKeyId = '';
                _classCode = scope.quoteViewModel.Class.ClassCode;
                scope.directSales = scope.quoteViewModel.Class.DirectOK;

                /*The below line of code will remain till the previous implementation of exposure page doesnt roll out*/
                scope.quoteViewModel.BusinessClassDirectSales = scope.quoteViewModel.Class.DirectOK;

                _industryId = scope.quoteViewModel.Industry.IndustryId;
                _subIndustryId = _industryId > 0 ? scope.quoteViewModel.SubIndustry.SubIndustryId : null;
            }

            if (!coreUtils.IsEmptyString(_classDescId) && _classDescId > 0) {
                scope.searchOptionNotSelected = false;
                /*If business direct sales is !=N then only make companion list call*/
                if (!coreUtils.IsEmptyString(scope.quoteViewModel.TotalPayroll)) {
                    coreUtils.controlLevelLoader($('#' + ctrlId), true);
                    exposureService.validateMinExposurePayroll(_classDescId, _classDescKeyId, scope.quoteViewModel.TotalPayroll, _classCode, scope.directSales, _industryId, _subIndustryId).then(function (data) {
                        scope.minExpValidation = data.resultStatus;
                        scope.showMinPayRollFirstWarning = !scope.minExpValidation;
                        scope.quoteViewModel.MinPayrollAllResponseRecieved = !scope.showMinPayRollFirstWarning;
                        scope.minExpValidationMessage = data.resultText;
                        scope.quoteViewModel.MinExpValidationAmount = data.resultMinAmount;
                        if (_classDescId != '-1') {
                            scope.quoteViewModel.IsGoodStateApplicable = !scope.minExpValidation;
                            scope.quoteViewModel.OtherClassDesc = '';
                        }

                        /* show companion field only if
                            1.if minimum payroll validation is valid then 
                            2.if minimum payroll validation is not valid i.e. (good state rule is applicable) && the state is a good state */
                        scope.disableSubmit = false;
                        coreUtils.controlLevelLoader($('#' + ctrlId), false);
                        scope.$$phase || scope.$apply();
                        deferred.resolve(scope.minExpValidation);

                    });
                }
            }
            else {
                /*Since Class descriptionId is not selected that means either industry or search option hasnt been selected*/
                scope.searchOptionNotSelected = false;
                if (scope.showProfession) {
                    /*search option hasnt been selected*/
                    scope.searchOptionNotSelected = true;
                }
                else {
                    scope.searchOptionSelected = false;
                }
                scope.disableSubmit = false;
                scope.minExpValidation = true;
                scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                deferred.resolve(scope.minExpValidation);

            }
            return deferred.promise;
        };

        /*updates and processes the response for first warning after minimum payroll validation fails*/
        scope.updateMinPayRollFirstAlertResponse = function (val) {
            scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
            scope.showMinPayRollSecondWarning = false;
            if (val) {
                scope.showMinPayRollFirstWarning = false;
                scope.showMinPayRollSecondWarning = true;
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
            }
            else {
                scope.showMinPayRollFirstWarning = false;
                scope.quoteViewModel.TotalPayroll = '';
                scope.showMinPayRollSecondWarning = false;
                scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
            }
        }

        /*updates and processes the response for second warning after the updating response of first minimum payroll alert*/
        scope.updateMinPayRollSecondAlertResponse = function (val) {
            if (!val) {
                loadingService.showLoader();
                exposureService.getStateType().then(function (response) {
                    _isGoodState = response == "True" ? true : false;
                    scope.showMinPayRollSecondWarning = false;
                    scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                    loadingService.hideLoader();
                });
            }
            else {
                scope.quoteViewModel.TotalPayroll = $filter('number', 0)(parseInt(scope.quoteViewModel.MinExpValidationAmount));
                scope.showMinPayRollSecondWarning = false;
                scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                //scope.validateExposureAmount();
                _isGoodState = null;
                _isGoodStateApplicable = null;
                scope.minExpValidation = true;
            }
        };

        scope.submit = function () {
            scope.listOfErrors = [];
            $timeout();
            if (!coreUtils.BackgroundProcessExecuting() && _IsValidModel()) {
                _proceedToExposure();
            }
            else {
                if (!_IsValidModel(false)) {
                    //scope.listOfErrors = ['Please fill all the fields.'];
                    coreUtils.ScrollToTop();
                }
                else if (coreUtils.BackgroundProcessExecuting()) {
                    scope.listOfErrors = ['Please wait for background process to complete'];
                    coreUtils.ScrollToTop();
                }
            }
        }

        scope.IsValidCompanionPayroll = function (data) {
            var totalPayroll = coreUtils.GetValidAmount(scope.quoteViewModel.TotalPayroll);
            var selectedCompClassPayroll = coreUtils.GetValidAmount(data.PayrollAmount);

            var companionPayrollSum = 0;
            $timeout(function () {
                if (scope.quoteViewModel.CompClassData && scope.quoteViewModel.CompClassData.length > 0) {
                    _.each(scope.quoteViewModel.CompClassData, function (val) {
                        scope.companionErrors[val.ClassCode].amountExceedingTotalPayroll = false;
                        scope.companionErrors[val.ClassCode].sumExceedingTotalPayroll = false;
                        if (val.IsExposureAmountRequired && val.PayrollAmount && val.PayrollAmount != '')
                            companionPayrollSum = companionPayrollSum + coreUtils.GetValidAmount(val.PayrollAmount);
                    })
                }
                if (data && data.IsExposureAmountRequired) {
                    if (selectedCompClassPayroll > totalPayroll) {
                        data.PayrollAmount = '';
                        scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll = true;
                    }
                }
                if (companionPayrollSum > totalPayroll && !scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll && scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll == false) {
                    data.PayrollAmount = '';
                    scope.companionErrors[data.ClassCode].sumExceedingTotalPayroll = true;
                }
            });
            //if (scope.quoteViewModel.CompClassData && scope.quoteViewModel.CompClassData.length > 0) {
            //    _.each(scope.quoteViewModel.CompClassData, function (val) {
            //        scope.companionErrors[val.ClassCode].amountExceedingTotalPayroll = false;
            //        scope.companionErrors[val.ClassCode].sumExceedingTotalPayroll = false;
            //        if (val.IsExposureAmountRequired && val.PayrollAmount && val.PayrollAmount != '')
            //            companionPayrollSum = companionPayrollSum + coreUtils.GetValidAmount(val.PayrollAmount);
            //    })
            //}
            //if (data && data.IsExposureAmountRequired) {
            //    if (selectedCompClassPayroll > totalPayroll) {
            //        data.PayrollAmount = '';
            //        scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll = true;
            //    }
            //}
            //if (companionPayrollSum > totalPayroll && !scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll && scope.companionErrors[data.ClassCode].amountExceedingTotalPayroll == false) {
            //    data.PayrollAmount = '';
            //    scope.companionErrors[data.ClassCode].sumExceedingTotalPayroll = true;
            //}
        }

        /*finds out the data source for companion class*/
        scope.setCompanionClassData = function (classDescId, ctrlId, directSales) {
            if (!coreUtils.IsEmptyString(directSales)) {
                scope.directSales = directSales;
            }
            var selectedCompanionClasses;
            $timeout(function () {
                if (!coreUtils.IsEmptyString(classDescId) && classDescId > 0 && scope.quoteViewModel.SelectedSearch == 0 && coreUtils.GetValidAmount(classDescId) > 0) {
                    exposureService.getCompanionCodes(classDescId, scope.quoteViewModel.TotalPayroll).then(function (res) {
                        selectedCompanionClasses = res.companionClassList;
                        _populateCompClassDataInVm(selectedCompanionClasses, ctrlId);
                    });
                }
                else {
                    if (scope.quoteViewModel.Class) {
                        selectedCompanionClasses = scope.quoteViewModel.Class.CompanionClasses;
                        _populateCompClassDataInVm(selectedCompanionClasses, ctrlId);
                    }
                }
            });
        }

        /*populates the data source for companion class in quote view model*/
        function _populateCompClassDataInVm(selectedCompanionClasses, ctrlId) {
            if (selectedCompanionClasses && selectedCompanionClasses.length > 0) {
                scope.quoteViewModel.CompClassData = [];
                scope.companionErrors = {};
                $timeout(function () {
                    var count = 0;
                    _.each(selectedCompanionClasses, function (item) {
                        scope.quoteViewModel.CompClassData[count] = item;
                        scope.companionErrors[item.ClassCode] = {
                            amountExceedingTotalPayroll: false,
                            sumExceedingTotalPayroll: false
                        }
                        count++;
                    });
                })
            }
            coreUtils.controlLevelLoader($('#' + ctrlId), false);
        }

        /*Validates model to be sent*/
        function _IsValidModel(fromSaveForLater) {
            /*To be done later : Push the messages for each and every scenario separately in list Of Errors*/
            scope.listOfErrors = [];
            if (scope.quoteViewModel.SelectedSearch == "1") {
                if (!scope.quoteViewModel.Industry) {
                    scope.listOfErrors.push('Industry not selected');
                    //return false;
                }
                else if (scope.quoteViewModel.Industry.IndustryId > 0 && !scope.quoteViewModel.SubIndustry) {
                    scope.listOfErrors.push('SubIndustry not selected');
                    //return false;
                }
                else if (scope.quoteViewModel.Industry.IndustryId == -1 && coreUtils.IsEmptyString(scope.quoteViewModel.OtherClassDesc)) {
                    scope.listOfErrors.push('Industry selected as Others but Other business name is not entered');
                    //return false;
                }
                else if (scope.quoteViewModel.SubIndustry && scope.quoteViewModel.SubIndustry.SubIndustryId > 0 && !scope.quoteViewModel.Class) {
                    scope.listOfErrors.push('Business Class not selected');
                    //return false;
                }
                else if (scope.quoteViewModel.SubIndustry && scope.quoteViewModel.SubIndustry.SubIndustryId == -1 && coreUtils.IsEmptyString(scope.quoteViewModel.OtherClassDesc)) {
                    scope.listOfErrors.push('SubIndustry selected as Others but Other business name is not entered');
                    //return false;
                }
                else if (scope.quoteViewModel.Class && coreUtils.IsEmptyString(scope.quoteViewModel.Class.ClassDescriptionId) && scope.quoteViewModel.Class.ClassDescriptionId == 0) {
                    scope.listOfErrors.push('Business Class not selected');
                    //return false;
                }
                else if (scope.quoteViewModel.Class && scope.quoteViewModel.Class.ClassDescriptionId == "-1" && coreUtils.IsEmptyString(scope.quoteViewModel.OtherClassDesc)) {
                    scope.listOfErrors.push('Please tell us about your business');
                    //return false;
                }
            }
            else if (coreUtils.IsEmptyString(scope.quoteViewModel.ClassDescriptionId) || coreUtils.IsEmptyString(scope.quoteViewModel.ClassDescriptionKeywordId)) {
                scope.listOfErrors.push('Business Description is required');
            }

            if (scope.quoteViewModel.ClassDescriptionId == '-1' && coreUtils.IsEmptyString(scope.quoteViewModel.OtherClassDesc)) {
                scope.listOfErrors.push('Please describe your business in your own words');

            }
            if (!fromSaveForLater) {
                if (scope.showMinPayRollFirstWarning) {
                    scope.listOfErrors.push('Please select at least one of the options in the box');
                }
                if (scope.showMinPayRollSecondWarning) {
                    scope.listOfErrors.push('Please select at least one of the options in the box');
                }
            }
            if (coreUtils.IsEmptyString(scope.quoteViewModel.InceptionDate)) {               
                    scope.listOfErrors.push('Policy start date is required');                    
            }
            if (!scope.quoteViewModel.SelectedYearInBusiness || coreUtils.IsEmptyString(scope.quoteViewModel.SelectedYearInBusiness.value)) {
                scope.listOfErrors.push('Select the number of years you have been in business');
            }
            if (coreUtils.IsEmptyString(scope.quoteViewModel.TotalPayroll) || coreUtils.GetValidAmount(scope.quoteViewModel.TotalPayroll) <= 0) {
                if (coreUtils.IsEmptyString(scope.quoteViewModel.TotalPayroll)) {
                    scope.listOfErrors.push('Total annual employee payroll is required');
                }
                if (!coreUtils.IsEmptyString(scope.quoteViewModel.TotalPayroll) && coreUtils.GetValidAmount(scope.quoteViewModel.TotalPayroll) <= 0) {
                    scope.listOfErrors.push('Total annual employee payroll should be greater than 0');
                }                 
            }          

            var totalPayroll = coreUtils.GetValidAmount(scope.quoteViewModel.TotalPayroll);
            var invalid = false;
            if (scope.quoteViewModel.CompClassData && scope.quoteViewModel.CompClassData.length > 0) {
                if (_.some(scope.quoteViewModel.CompClassData, function (data) {
                    return coreUtils.IsEmptyObject(data.IsExposureAmountRequired)
                }))
                {
                    scope.listOfErrors.push('Please select either YES or NO for special employee descriptions asked ');
                }

                if (scope.quoteViewModel.BusinessClassDirectSales != 'N' && totalPayroll >= 50000) {
                    var companionPayrollSum = 0;
                    _.each(scope.quoteViewModel.CompClassData, function (val) {
                        if (val.IsExposureAmountRequired)
                            companionPayrollSum = companionPayrollSum + coreUtils.GetValidAmount(val.PayrollAmount);
                    })
                    if (_.some(scope.quoteViewModel.CompClassData, function (data) {
                        return (data.IsExposureAmountRequired && coreUtils.IsEmptyString(data.PayrollAmount));
                    })) {
                        scope.listOfErrors.push('Please enter payroll for special employee descriptions');
                    }
                    if (
                        //Comment: removed field value check GUIN-311
                        //    _.some(scope.companionErrors, function (data) {\\\
                        //    return data.amountExceedingTotalPayroll || data.sumExceedingTotalPayroll;
                        //}) ||
                        (companionPayrollSum > totalPayroll)) {
                        scope.listOfErrors.push('The payroll for special employee descriptions should already be counted within the total. The amount entered is too large.');
                    }
                }
            }
            if (scope.listOfErrors.length == 0) {
                return true;
            }
            else {
                return false;
            }
        }

        scope.IsValidModel = function () {
            return _IsValidModel();
        };

        /*Method to scroll up the view page and show the respective error */
        function _scrollFormOnError() {
            //Comment : Here iterate errors list and display it to screen
            //scope.listOfErrors = ['Please fill all the fields.'];
            coreUtils.ScrollToTop();
        }

        function _proceedToExposure() {
            if (_IsValidModel()) {
                loadingService.showLoader();
                /*Nishank : GUIN-272 After validating model below condition is must as was handled in case 
                of Other selection in any case (Industry Sub Industry and Class and Search by keyword)*/
                if (scope.quoteViewModel.ClassDescriptionId == '-1') {
                    scope.quoteViewModel.MinPayrollAllResponseRecieved = true;
                    scope.quoteViewModel.IsGoodStateApplicable = false;
                }
                exposureService.submitExposureData(scope.quoteViewModel).then(function (data) {
                    if (data.response) {
                        scope.responseRecieved = true;
                        $location.path(data.path);
                    }
                    else if (!data.response && !data.resultStatus) {
                        scope.minExpValidation = data.resultStatus;
                        scope.showMinPayRollFirstWarning = !scope.minExpValidation;
                        scope.showMinPayRollSecondWarning = false;
                        scope.quoteViewModel.MinPayrollAllResponseRecieved = !scope.showMinPayRollFirstWarning;
                        scope.minExpValidationMessage = data.resultText;
                        scope.quoteViewModel.MinExpValidationAmount = data.resultMinAmount;
                        if (scope.quoteViewModel.ClassDescriptionId != '-1') {
                            scope.quoteViewModel.IsGoodStateApplicable = !scope.minExpValidation;
                            //scope.quoteViewModel.OtherClassDesc = '';
                        }
                        loadingService.hideLoader();
                        $('#minPayrollAlerts').focus();
                        $('html, body').animate({
                            scrollTop: $('#minPayrollAlerts').offset().top
                        }, 500);
                    }
                    else {
                        //Comment : Here if any user error messages found then show it to user
                        if (data.errorList && data.errorList.length > 0) {
                            loadingService.hideLoader();
                            scope.listOfErrors = data.errorList;
                        }
                        else {
                            //Comment : Here convert stringified JSON to JSON object
                            if (
                                    typeof (data) == 'string' &&
                                    (
                                        (data.indexOf('<meta name="AppSessionTimedOut" content="true">') != -1)         //In case app session or cookies has been expired
                                        || (data.indexOf('<meta name="AppAuthTokenNotFound" content="true">') != -1)    //In case app antoforgory toekn not found
                                    )
                                ) {
                                //do nothing
                            }
                            else {
                                scope.responseRecieved = true;
                                $location.path('/AppError');
                            }
                        }
                    }
                })
            }
            else {
                //scope.listOfErrors = ['Please fill all the fields.'];
                coreUtils.ScrollToTop();
            }
        }

        // Progress bar navigation
        scope.goTo = function (sender) {
            if ($(sender.target).attr("url") != "#" && ($(sender.target).attr("url").indexOf('GetExposureDetails')) == -1) {
                coreUtils.setNavigation($(sender.target).attr("url"), true);

                //navigation =
                //{
                //    path: $(sender.target).attr("url") || '',
                //    status: true
                //};

                $location.path($(sender.target).attr("url"));
            }
        }

        scope.navigateToBusinessInfo = function ()
        {
            $location.path('/GetBusinessInfo/');
        }

        scope.submitSaveForLater = function (userEmailId) {
            //reset error list
            scope.quoteViewModel.listOfErrors = [];

            //Comment : Here check for all validation are successfully processed then proceed further
            if (_IsValidModel(true)) {
                //Comment : Here do not wait for data submission because it will create a long delay in displaying thank you message
                exposureService.submitSaveForLaterExposureData(scope.quoteViewModel, userEmailId).then(function (data) {
                    if (data.resultStatus == "OK") {
                    }
                    else if (data.resultStatus === undefined) {
                        coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), false);
                    }
                    setTimeout(function () {
                        coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                    }, 1500);
                });
            }
            else {
                scope.submitted = true;
            }
        }

        scope.$watch("quoteViewModel.Industry", function (newVal, oldVal) {
            if (JSON.stringify(oldVal) !== JSON.stringify(newVal)) {
                scope.quoteViewModel.TotalPayroll = '';
                scope.quoteViewModel.SubIndustry = null;
                scope.quoteViewModel.Class = null;
                scope.quoteViewModel.CompClassData = [];
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
            }
        });

        scope.$watch("quoteViewModel.SubIndustry", function (newVal, oldVal) {
            if (JSON.stringify(oldVal) !== JSON.stringify(newVal)) {
                scope.quoteViewModel.TotalPayroll = '';
                scope.quoteViewModel.Class = null;
                scope.quoteViewModel.CompClassData = [];
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
            }
        });

        scope.$watch("quoteViewModel.Class", function (newVal, oldVal) {
            if (JSON.stringify(oldVal) !== JSON.stringify(newVal)) {
                scope.quoteViewModel.TotalPayroll = '';
                scope.quoteViewModel.CompClassData = [];
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
            }
        });

        scope.$watch("quoteViewModel.BusinessName", function (newVal, oldVal) {
            if (oldVal != newVal) {
                scope.quoteViewModel.TotalPayroll = '';
                scope.quoteViewModel.CompClassData = [];
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
            }
        })

        scope.$watch("quoteViewModel.SelectedSearch", function (newVal, oldVal) {
            if (oldVal != newVal) {
                $timeout(function () {
                    scope.quoteViewModel.TotalPayroll = '';
                    if (newVal == 0 && scope.quoteViewModel.ClassDescriptionId > 0) {
                        scope.setCompanionClassData(scope.quoteViewModel.ClassDescriptionId, 'business', scope.quoteViewModel.BusinessClassDirectSales);
                    }
                    else if (scope.quoteViewModel.Class && scope.quoteViewModel.Class.ClassDescriptionId > 0) {
                        scope.setCompanionClassData(scope.quoteViewModel.Class.ClassDescriptionId, 'ddlClass', scope.quoteViewModel.Class.DirectOK)
                    }
                    else {
                        scope.quoteViewModel.CompClassData = [];
                    }
                })
            }
        })

        scope.$watch("quoteViewModel.TotalPayroll", function (newVal, oldVal) {
            if (oldVal != newVal) {
                scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
                scope.minExpValidation = true;
                scope.showMinPayRollFirstWarning = false;
                scope.quoteViewModel.IsGoodStateApplicable = false;
                scope.showMinPayRollFirstWarning = false;
                scope.showMinPayRollSecondWarning = false;
                if (coreUtils.GetValidAmount(newVal) <= 0) {
                    scope.frmExposureInput.TotalPayroll.$setValidity("range", false);
                }
                else {
                    scope.frmExposureInput.TotalPayroll.$setValidity("range", true);
                }
            }
        })

        scope.voidValidity = function () {
            scope.quoteViewModel.MinPayrollAllResponseRecieved = false;
        }

    }
}());