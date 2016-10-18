(function () {

    angular.module("BHIC.WC.Controllers").controller('quoteController', [
        '$scope',
        'quoteService',
        'navigationService',
        'coreUtils',
        'quotePageDefaults',
        //'companionCodes',
        'quoteViewModel',
        '$routeParams',
        'loadingService',
        '$location',
        '$filter',
        '$window',
        'appConfig',
        quoteControllerFn]);
    function quoteControllerFn(scope, quoteService, navigationService, coreUtils, quotePageDefaults, serverQuoteVm, routeParams, loadingService, $location, $filter, $window,appConfig) {
        var _quoteViewModel = {};
        _quoteViewModel.searchOptions = [
            { "key": 0, "value": "Profession" },
            { "key": 1, "value": "Industry" }
        ];

        var loaderElements = [
         $('#ddlIndustries'),
         $('#ddlSubIndustries'),
         $('#ddlClass'),
         $('#txtMinPayrollExposureAmount'),
         $('#txtBusinessName')
        ]
        var _minExposureValidation = {
            "OK": true,
            "NOK": false
        };
        var _isGoodState = null;
        var _isGoodStateApplicable = null;
        scope.disableSubmit = false;
        //var companionCodes;
        var serverQuoteVm;

        //Comment : Here if any user or functional errors while posting data then show form level errors
        _quoteViewModel.listOfErrors = [];

        function _init() {

            //add complete class on quote menu if user comes directly to purchaseQuote
            // navigationService.enableNavigation('lnkQuote');
            
            //loadingService.hideLoader();
            serverQuoteVm = serverQuoteVm != null && serverQuoteVm != undefined ? serverQuoteVm : null;
            var maxDate = new Date();
            scope.maxDate = coreUtils.FormatDate(maxDate.setDate(maxDate.getDate() + 60));
            var minDate = new Date();
            scope.minDate = coreUtils.FormatDate(minDate.setDate(minDate.getDate() + 1));
            _quoteViewModel.quotePageDefaults = JSON.parse(quotePageDefaults.data);
            //companionCodes = companionCodes ? companionCodes.data : null;
            _quoteViewModel.showProfession = true;
            _quoteViewModel.showIndustry = false;
            _quoteViewModel.selectedSearch = 0;
            _getIndustries();
            scope.$$phase || scope.$digest();
            scope.exposureSubmittedSuccessfully = false;
            _quoteViewModel.minExpValidation = true;
            _quoteViewModel.NavLinks = serverQuoteVm.NavigationLinks;
            if (serverQuoteVm.Exposures && serverQuoteVm.Exposures.length > 0) {
                _setQuoteValues(serverQuoteVm.Exposures[0]);
                scope.disableSubmit = false;
            }
            else {
                if (!serverQuoteVm.ClassDescriptionKeywordId && serverQuoteVm.IndustryId) {
                    _quoteViewModel.showProfession = false;
                    _quoteViewModel.showIndustry = true;
                    scope.quoteViewModel.industry = { IndustryId: serverQuoteVm.IndustryId };
                    _quoteViewModel.getSubIndustries(scope.quoteViewModel.industry.IndustryId);
                    //_quoteViewModel.showSubIndustry = true;
                    scope.quoteViewModel.subIndustry = { SubIndustryId: serverQuoteVm.SubIndustryId };
                    //_quoteViewModel.showClass = true;
                    //_quoteViewModel.getClass(scope.quoteViewModel.subIndustry.SubIndustryId, exposure);
                    if (serverQuoteVm.ClassDescriptionId != 0) {
                        scope.quoteViewModel.class = {
                            ClassDescriptionId: serverQuoteVm.ClassDescriptionId,
                            ClassCode: serverQuoteVm.ClassCode,
                            Description: serverQuoteVm.BusinessName
                        }
                    }
                    _quoteViewModel.otherBusinessName = serverQuoteVm.OtherClassDesc;
                    scope.quoteViewModel.selectedSearch = "1";
                    scope.quoteViewModel.businessYears = { value: serverQuoteVm.BusinessYears };
                    scope.quoteViewModel.minPayrollExposureAmount = serverQuoteVm.TotalPayroll;
                    scope.quoteViewModel.policyStartDate = serverQuoteVm.PolicyData ? coreUtils.FormatDate(new Date(parseFloat(serverQuoteVm.PolicyData.InceptionDate.substr(6)))) : '';
                    scope.disableSubmit = false;
                }
                //else {
                //    scope.quoteViewModel.businessName = serverQuoteVm.BusinessName;
                //    scope.quoteViewModel.selectedClassDescId = serverQuoteVm.ClassDescriptionId;
                //    scope.quoteViewModel.selectedClassCode = serverQuoteVm.ClassCode;
                //}

            }

            //Comment : Here fetched current running QuoteId value
            appConfig.quoteId = serverQuoteVm.QuoteId;
        }


        /*Load Industries On Page Load*/
        function _getIndustries() {
            quoteService.getIndustries().then(function (data) {
                _quoteViewModel.industries = data;
            });
        }

        function _getCompanionClassCodeFromAPI() {
            var classDescId = scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassDescId : scope.quoteViewModel.class.ClassDescriptionId;
            if (!(classDescId == null || classDescId == undefined || classDescId == '')) {
                quoteService.getCompanionCodes(classDescId, scope.quoteViewModel.minPayrollExposureAmount).then(function (res) {
                    if (res.companionClassList && res.companionClassList.length > 0) {
                        //_.each(res.companionClassList, function (val) {
                        //    val["PayrollAmount"] = '';
                        //});
                        _getCompanionCodeData(res.companionClassList);
                        return true;
                    }
                    else {
                        return false;
                    }
                });
            }
        }

        function _setQuoteValues(exposure) {

            scope.quoteViewModel.updatedPayrollAmount = serverQuoteVm.AnnualPayroll;
            scope.quoteViewModel.selectedClassDescKeyId = exposure.ClassDescriptionKeywordId;
            scope.quoteViewModel.selectedClassDirectSales = serverQuoteVm.BusinessClassDirectSales || '';
            scope.quoteViewModel.businessYears = { value: serverQuoteVm.PolicyData.YearsInBusiness };

            if (serverQuoteVm.IsMultiClass || serverQuoteVm.IsMultiClassApplicable) {
                scope.quoteViewModel.minPayrollExposureAmount = serverQuoteVm.TotalPayroll;
            }
            else {
                scope.quoteViewModel.minPayrollExposureAmount = coreUtils.IsEmptyString(serverQuoteVm.TotalPayroll) ? exposure.ExposureAmt : serverQuoteVm.TotalPayroll;
            }
            scope.quoteViewModel.policyStartDate = coreUtils.FormatDate(new Date(parseFloat(serverQuoteVm.PolicyData.InceptionDate.substr(6))));

            if (!exposure.ClassDescriptionKeywordId) {
                _quoteViewModel.showProfession = false;
                _quoteViewModel.showIndustry = true;
                _quoteViewModel.showSubIndustry = true;
                _quoteViewModel.showClass = true;
                scope.quoteViewModel.industry = { IndustryId: !coreUtils.IsEmptyString(exposure.IndustryId) ? exposure.IndustryId : serverQuoteVm.IndustryId };
                _quoteViewModel.getSubIndustries(scope.quoteViewModel.industry.IndustryId, exposure);
                scope.quoteViewModel.subIndustry = { SubIndustryId: !coreUtils.IsEmptyString(exposure.SubIndustryId) ? exposure.SubIndustryId : serverQuoteVm.SubIndustryId };
                //_quoteViewModel.getClass(scope.quoteViewModel.subIndustry.SubIndustryId, exposure);
                scope.quoteViewModel.class = {
                    ClassDescriptionId: serverQuoteVm.ClassDescriptionId,
                    ClassCode: serverQuoteVm.ClassCode,
                    Description: serverQuoteVm.BusinessName
                }
                if (serverQuoteVm.ClassDescriptionId == "-1")
                    _quoteViewModel.otherBusinessName = exposure.OtherClassDesc;
                scope.quoteViewModel.selectedSearch = "1";
            }
            else {
                scope.quoteViewModel.businessName = serverQuoteVm.BusinessName;
                scope.quoteViewModel.selectedClassDescId = exposure.ClassDescriptionId;
                scope.quoteViewModel.selectedClassCode = serverQuoteVm.ClassCode;
            }
            if (serverQuoteVm.CompClassData && serverQuoteVm.CompClassData.length > 0) {
                scope.quoteViewModel.isMultiClass = serverQuoteVm.IsMultiClass;
                scope.quoteViewModel.isMultiState = serverQuoteVm.IsMultiStateApplicable;
                _getCompanionCodeData(serverQuoteVm.CompClassData);
                scope.msmcData.moreClassRequired = serverQuoteVm.MoreClassRequired;
                scope.quoteViewModel.showCompanionClass = true;
            }
            //else {
            //    _getCompanionClassCodeFromAPI();
            //}
            scope.quoteViewModel.isMultiClass = serverQuoteVm.IsMultiClass;
            scope.quoteViewModel.isMultiState = serverQuoteVm.IsMultiStateApplicable;
            scope.quoteViewModel.showCompanionClass = serverQuoteVm.IsMultiClassApplicable;
            scope.quoteViewModel.minExpValidationAmount = serverQuoteVm.MinExpValidationAmount;
            if (scope.quoteViewModel.minExpValidationAmount > scope.quoteViewModel.minPayrollExposureAmount) {
                scope.quoteViewModel.minExpValidation = false;
            }
            _isGoodStateApplicable = serverQuoteVm.IsGoodStateApplicable;
            _isGoodState = serverQuoteVm.IsGoodState;
        }

        _quoteViewModel.acceptFlag = true;

        /*Check validations For Data to be Submitted for Exposure*/
        _quoteViewModel.runValidations = function () {
            _quoteViewModel.acceptFlag = true;
            if (scope.quoteViewModel.selectedSearch == "1") {
                if (!scope.quoteViewModel.industry) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.industry.IndustryId > 0 && !scope.quoteViewModel.subIndustry) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.industry.IndustryId == -1 && coreUtils.IsEmptyString(scope.quoteViewModel.otherBusinessName)) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.subIndustry && scope.quoteViewModel.subIndustry.SubIndustryId > 0 && !scope.quoteViewModel.class) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.subIndustry && scope.quoteViewModel.subIndustry.SubIndustryId == -1 && coreUtils.IsEmptyString(scope.quoteViewModel.otherBusinessName)) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.class && coreUtils.IsEmptyString(scope.quoteViewModel.class.ClassDescriptionId) && scope.quoteViewModel.class.ClassDescriptionId == 0) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (scope.quoteViewModel.class && scope.quoteViewModel.class.ClassDescriptionId == '-1' && coreUtils.IsEmptyString(scope.quoteViewModel.otherBusinessName)) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
                else if (
                    coreUtils.IsEmptyString(scope.quoteViewModel.minPayrollExposureAmount) ||
                    coreUtils.IsEmptyString(scope.quoteViewModel.policyStartDate)
                    ) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
            }
            else {
                if (coreUtils.IsEmptyString(scope.quoteViewModel.selectedClassDescId) ||
                    coreUtils.IsEmptyString(scope.quoteViewModel.selectedClassDescKeyId) ||
                    coreUtils.IsEmptyString(scope.quoteViewModel.minPayrollExposureAmount) ||
                    coreUtils.IsEmptyString(scope.quoteViewModel.policyStartDate)) {
                    _quoteViewModel.acceptFlag = false;
                    return _quoteViewModel.acceptFlag;
                }
            }

            if (!scope.quoteViewModel.businessYears) {
                scope.quoteViewModel.acceptFlag = false;
                return _quoteViewModel.acceptFlag;
            }
            if (!scope.minPayrollAllResponseRecieved) {
                scope.quoteViewModel.acceptFlag = false;
                return _quoteViewModel.acceptFlag;
            }

            return _quoteViewModel.acceptFlag;
        }

        /*Prepare Exposure Data*/
        function _prepareQuoteSubmissionData() {
            return {
                ExposureId: '',
                QuoteId: '',
                IndustryId: scope.quoteViewModel.selectedSearch == "1" ? scope.quoteViewModel.industry.IndustryId : null,
                SubIndustryId: scope.quoteViewModel.selectedSearch == "1" ? (scope.quoteViewModel.subIndustry ? scope.quoteViewModel.subIndustry.SubIndustryId : null) : null,
                ClassDescriptionId: scope.quoteViewModel.selectedSearch == "1" ? (scope.quoteViewModel.class ? scope.quoteViewModel.class.ClassDescriptionId : null) : scope.quoteViewModel.selectedClassDescId,
                ClassDescriptionKeywordId: scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassDescKeyId : null,
                ZipCode: '',
                SimpleFlow: true,
                ClassCode: scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassCode : (scope.quoteViewModel.class ? scope.quoteViewModel.class.ClassCode : null),
                ClassSuffix: '',
                ExposureAmt: scope.quoteViewModel.showCompanionClass ? (scope.quoteViewModel.updatedPayrollAmount ? scope.quoteViewModel.updatedPayrollAmount : scope.quoteViewModel.minPayrollExposureAmount) : scope.quoteViewModel.minPayrollExposureAmount,
                StateAbbr: serverQuoteVm != null && serverQuoteVm.County != null && serverQuoteVm.County !== undefined ? serverQuoteVm.County.State : '',
                InceptionDate: scope.quoteViewModel.policyStartDate,
                BusinessYears: scope.quoteViewModel.businessYears.value,
                IsGoodState: _isGoodState,
                IsGoodStateApplicable: _isGoodStateApplicable,
                IsMultiClassApplicable: scope.quoteViewModel.showCompanionClass,
                IsMultiClass: scope.quoteViewModel.isMultiClass,
                TotalPayroll: scope.quoteViewModel.totalPayroll ? scope.quoteViewModel.totalPayroll : scope.quoteViewModel.minPayrollExposureAmount,
                MoreClassRequired: scope.msmcData ? scope.msmcData.moreClassRequired : false,
                IsMultiStateApplicable: scope.quoteViewModel.isMultiState,
                CompClassData: scope.quoteViewModel.showCompanionClass == true ? (scope.msmcData ? scope.msmcData.companionCodeData : null) : null,
                MinExpValidationAmount: scope.quoteViewModel.minExpValidationAmount,
                OtherClassDesc: scope.quoteViewModel.selectedSearch == "1" ? scope.quoteViewModel.otherBusinessName : '',
                BusinessClassDirectSales: scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassDirectSales : (scope.quoteViewModel.class ? scope.quoteViewModel.class.DirectOK : null),
                BusinessName: scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.businessName : (scope.quoteViewModel.class ? scope.quoteViewModel.class.Description : null),

                //Newly added by Prem on 29.03.2016 (to send information to server side)
                IndustryName: scope.quoteViewModel.selectedSearch == "1" ?
                    ($filter('filter')(scope.quoteViewModel.industries, { IndustryId: scope.quoteViewModel.industry.IndustryId }))[0].Description : null,
                //IndustryName: scope.quoteViewModel.selectedSearch == "1" ? scope.quoteViewModel.industry.Description : null, //Not working in case browser back
                SubIndustryName: scope.quoteViewModel.selectedSearch == "1" ?
                    (scope.quoteViewModel.subIndustry ? scope.quoteViewModel.subIndustry.Description : null) : null,               
                BusinessYearsText: ($filter('filter')(scope.quoteViewModel.quotePageDefaults.businessYears, { value: scope.quoteViewModel.businessYears.value }))[0].text,
            }
        };

        /*Update Search selection for Industry or profession*/
        _quoteViewModel.updateSearchSelection = function (val) {
            switch (val) {
                case 0:
                    _quoteViewModel.showProfession = true;
                    _quoteViewModel.showIndustry = false;
                    //scope.captureQuote.industry.$pristine = true
                    //_quoteViewModel.showSubIndustry = false;
                    //_quoteViewModel.showClass = false;
                    //_quoteViewModel.industry = '';
                    //_quoteViewModel.subIndustry = '';
                    //_quoteViewModel.class = '';
                    break;
                case 1:
                    _quoteViewModel.showProfession = false;
                    _quoteViewModel.showIndustry = true;
                    //scope.quoteViewModel.selectedClassDescKeyId = null;
                    //scope.quoteViewModel.selectedClassDescId = null;
                    //_quoteViewModel.businessName = '';
                    break;
            }
            scope.quoteViewModel.validateExposureAmount();
        };

        /*Get Sub Industries on the basis of Industry Ids*/
        _quoteViewModel.getSubIndustries = function (industryId, exposure) {
            coreUtils.controlLevelLoader($('#ddlIndustries'), true);
            scope.quoteViewModel.showMinPayRollFirstWarning = false;
            scope.quoteViewModel.showMinPayRollSecondWarning = false;
            //Comment : Here default set to hidden
            _quoteViewModel.subIndustries = null;
            if (scope.captureQuote) {
                if (scope.captureQuote.subIndustry) {
                    scope.captureQuote.subIndustry.$pristine = true;
                }
                if (scope.captureQuote.class) {
                    scope.captureQuote.class.$pristine = true;
                }
            }

            _quoteViewModel.showSubIndustry = false;
            _quoteViewModel.showClass = false;
            _quoteViewModel.subIndustry = null;

            if (industryId && industryId > 0) {
                _quoteViewModel.processingSubIndustry = true;
                quoteService.getSubIndustries(industryId).
                then(function (data) {
                    _quoteViewModel.subIndustries = data;
                    _quoteViewModel.showSubIndustry = true;
                    _quoteViewModel.processingSubIndustry = false;
                    if (exposure || serverQuoteVm.SubIndustryId) {
                        scope.quoteViewModel.subIndustry = exposure ? exposure.SubIndustryId : serverQuoteVm.SubIndustryId;

                        //Comment : Here get filtered industry to auto select IndustryList
                        var filteredSubIndustry = $filter('filter')(_quoteViewModel.subIndustries, { SubIndustryId: exposure ? exposure.SubIndustryId : serverQuoteVm.SubIndustryId });
                        if (filteredSubIndustry.length > 0)
                            scope.quoteViewModel.subIndustry = filteredSubIndustry[0];

                        //STEP - 3 Get ClassDesciption based on auto selected SubIndustryId
                        _quoteViewModel.getClass(scope.quoteViewModel.subIndustry.SubIndustryId, exposure);
                    }
                    else {
                        coreUtils.controlLevelLoader($('#ddlIndustries'), false);
                    }
                });
            }
            else {
                coreUtils.controlLevelLoader($('#ddlIndustries'), false);
            }
            scope.$$phase || scope.$digest();
        };

        /*Get Business on the basis of SubIndustry Ids*/
        _quoteViewModel.getClass = function (subIndustryId, exposure) {
            coreUtils.controlLevelLoader($('#ddlSubIndustries'), true);
            scope.quoteViewModel.showMinPayRollFirstWarning = false;
            scope.quoteViewModel.showMinPayRollSecondWarning = false;
            //Comment : here in case exposure not passed then set it to null
            exposure = exposure || null;

            //default setting
            _quoteViewModel.processingClass = true;

            //Comment : Here default set to hidden
            _quoteViewModel.classes = null;
            _quoteViewModel.showClass = false;
            _quoteViewModel.class = null;
            if (scope.captureQuote && scope.captureQuote.class) {
                scope.captureQuote.class.$pristine = true
            }

            if (subIndustryId > 0) {
                quoteService.getClass(subIndustryId).
                then(function (data) {
                    _quoteViewModel.classes = data;
                    _quoteViewModel.showClass = true;
                    _quoteViewModel.processingClass = false;
                    _quoteViewModel.class = null;
                    //Comment : Here in case of Exposure data exist then auto bind based Exposure
                    if (exposure || serverQuoteVm.ClassDescriptionId) {

                        //STEP - 2.1 set sub-indutry ModelValue
                        scope.quoteViewModel.class = exposure ? exposure.ClassDescriptionId : serverQuoteVm.ClassDescriptionId;

                        //Comment : Here get filtered industry to auto select IndustryList
                        var filteredClassDescription = $filter('filter')(_quoteViewModel.classes, { ClassDescriptionId: exposure ? exposure.ClassDescriptionId : serverQuoteVm.ClassDescriptionId });
                        if (filteredClassDescription.length > 0)
                            scope.quoteViewModel.class = filteredClassDescription[0];

                        //STEP - Finally show industry seelction flow 
                        //if (exposure && exposure.ClassDescriptionKeywordId == null) {
                        //    _quoteViewModel.classOrIndustry = 1;
                        //    _quoteViewModel.updateSearchSelection(scope.quoteViewModel.classOrIndustry);
                        //}
                    }
                    coreUtils.controlLevelLoader($('#ddlSubIndustries'), false);
                });
            }
            else {
                loadingService.hideLoader();
                _quoteViewModel.classes = null;
                _quoteViewModel.showClass = false;
            }
        };

        /*Validate the Minimum Exposure Amount*/
        _quoteViewModel.validateExposureAmount = function (ctrlId) {
            _isGoodState = null;
            scope.quoteViewModel.showMinPayRollFirstWarning = false;
            scope.quoteViewModel.showMinPayRollSecondWarning = false;
            scope.disableSubmit = true;
            var _classDescId, _classDescKeyId, _classCode, _directSales, _industryId, _subIndustryId;
            if (_quoteViewModel.showProfession) {
                _classDescId = scope.quoteViewModel.selectedClassDescId;
                _classDescKeyId = scope.quoteViewModel.selectedClassDescKeyId;
                _classCode = scope.quoteViewModel.selectedClassCode;
                _directSales = scope.quoteViewModel.selectedClassDirectSales;
                _industryId = null;
                _subIndustryId = null;

            }
            else if (_quoteViewModel.showIndustry && _quoteViewModel.class) {
                _classDescId = scope.quoteViewModel.class ? scope.quoteViewModel.class.ClassDescriptionId : null;
                _classDescKeyId = '';
                _classCode = scope.quoteViewModel.class.ClassCode;
                _directSales = scope.quoteViewModel.class.DirectOK;
                _industryId = scope.quoteViewModel.industry.IndustryId;
                _subIndustryId = scope.quoteViewModel.subIndustry.SubIndustryId;
            }

            if (!coreUtils.IsEmptyString(_classDescId)) {
                _quoteViewModel.searchOptionNotSelected = false;
                if (!coreUtils.IsEmptyString(scope.quoteViewModel.minPayrollExposureAmount)) {
                    coreUtils.controlLevelLoader($('#' + ctrlId), true);
                    quoteService.validateMinExposurePayroll(_classDescId, _classDescKeyId, scope.quoteViewModel.minPayrollExposureAmount, _classCode, _directSales, _industryId, _subIndustryId).then(function (data) {
                        scope.quoteViewModel.minExpValidation = _minExposureValidation[data.resultStatus];
                        scope.quoteViewModel.showMinPayRollFirstWarning = !scope.quoteViewModel.minExpValidation;
                        scope.minPayrollAllResponseRecieved = !scope.quoteViewModel.showMinPayRollFirstWarning;
                        scope.quoteViewModel.minExpValidationMessage = data.resultText;
                        scope.quoteViewModel.minExpValidationAmount = data.resultMinAmount;
                        if (_classDescId != '-1') {
                            if (coreUtils.GetValidAmount(scope.quoteViewModel.minPayrollExposureAmount) > data.resultMinAmount) {
                                _isGoodStateApplicable = false;
                            }
                            else {
                                _isGoodStateApplicable = true;
                            }
                        }
                        else {
                            _isGoodStateApplicable = false;
                        }
                        scope.disableSubmit = false;
                        coreUtils.controlLevelLoader($('#' + ctrlId), false);
                        scope.$$phase || scope.$apply();
                    });
                }
            }
            else {
                /*Since Class descriptionId is not selected that means either industry or search option hasnt been selected*/
                _quoteViewModel.searchOptionNotSelected = false;
                if (_quoteViewModel.showProfession) {
                    /*search option hasnt been selected*/
                    _quoteViewModel.searchOptionNotSelected = true;
                }
                else {
                    _quoteViewModel.searchOptionSelected = false;
                }
                scope.disableSubmit = false;
                scope.quoteViewModel.minExpValidation = true;
            }
        };

        /*Submit the Exposure Data*/
        _quoteViewModel.submit = function () {
            //debugger;

            //reset error list
            scope.quoteViewModel.listOfErrors = [];

            if (_quoteViewModel.runValidations()) {
                var businessClassDirectSalesStatus = scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassDirectSales : (_quoteViewModel.class) ? _quoteViewModel.class.DirectOK : null;

                //Comment : Here 
                //CASE-1. As per requirement in case MS is selected irrespective of MC selection, by hitting CONTINUE next screen should be Question screen
                //CASE-2. In case business class is selected as "Other/class-id == -1" then submit and forward to Question screen
                //CASE-3. In case selected business class having DirectOK property with "N" value then don't submit exposure and navigate user to DeclinedQuote screen -- Added By Prem On 23.12.2015
                if (
                        scope.quoteViewModel.isMultiState //CASE-1
                        || (_quoteViewModel.showIndustry && _quoteViewModel.class && _quoteViewModel.class.ClassDescriptionId == '-1')  //CASE-2
                        || (businessClassDirectSalesStatus == 'N') //CASE-3
                   ) {
                    _submitExposureData();
                }
                else {
                    _processRequest();
                }

            }
            else {
                scope.submitted = true;
                _scrollFormOnError();
                //alert("Please provide complete information in order to continue.")
            }
        };

        function _submitExposureData() {
            //debugger;
            //console.log(coreUtils.BackgroundProcessExecuting());
            if (!coreUtils.BackgroundProcessExecuting() && _quoteViewModel.runValidations()) {
                var businessClassDirectSalesStatus = scope.quoteViewModel.selectedSearch == "0" ? scope.quoteViewModel.selectedClassDirectSales : (_quoteViewModel.class) ? _quoteViewModel.class.DirectOK : null;
                //var declineQuote = false;
                //if ((scope.quoteViewModel.industry.IndustryId == -1)
                //    || (scope.quoteViewModel.subIndustry && scope.quoteViewModel.subIndustry.SubIndustryId == -1)
                //    || (scope.quoteViewModel.class && scope.quoteViewModel.class.ClassDescriptionId == -1)
                //    ) {
                //    declineQuote = true;
                //}
                //Comment : Here added one more condition to handle business-class DirectSales value e.g. if value is not 'N' then decline quote
                if (
                    (scope.quoteViewModel.minExpValidation || (_isGoodState && !scope.quoteViewModel.minExpValidation))
                    //&& (!declineQuote)
                    //&& (businessClassDirectSalesStatus != 'N')
                   ) {
                    loadingService.showLoader();
                    quoteService.submitExposureData(_prepareQuoteSubmissionData()).then(function (data) {
                        if (data.response) {
                            scope.responseRecieved = true;
                            $location.path(data.path);
                        }
                        else
                        {
                            //Comment : Here if any user error messages found then show it to user
                            if (data.errorList && data.errorList.length > 0)
                            {
                                loadingService.hideLoader();
                                scope.quoteViewModel.listOfErrors = data.errorList;
                                coreUtils.ScrollToTop();
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
                    });
                }
                else {
                    scope.responseRecieved = true;
                    $location.path('/DeclinedQuote')
                }
            }
            else {
                scope.submitted = true;
                _scrollFormOnError();
                //alert("Please provide complete information in order to continue.")
            }
        }

        // Progress bar navigation
        //scope.goTo = function (sender) {
        //    if ($(sender.target).attr("url") != "#") {
        //        if (!appConfig.formGotDirty) {
        //            location.path($(sender.target).attr("url"));
        //        } else {
        //            coreUtilityMethod.RemodelPopUp($('[data-remodal-id=navigationModel]'), true);
        //        }
        //    }
        //}

        // Progress bar navigation
        scope.goTo = function (sender) {
            if ($(sender.target).attr("url") != "#")
                navigation = {
                    path: event.target.attributes['url'].value || '',
                    status: true
                };
                location.path($(sender.target).attr("url"));
        }

        var _processRequest = function () {
            var referralQuote = false;
            if ((scope.quoteViewModel.selectedSearch == "1" && scope.quoteViewModel.industry.IndustryId == -1)
                || (scope.quoteViewModel.subIndustry && scope.quoteViewModel.subIndustry.SubIndustryId == -1)
                || (scope.quoteViewModel.class && scope.quoteViewModel.class.ClassDescriptionId == -1)
                ) {
                referralQuote = true;
            }
            if (scope.quoteViewModel.showCompanionClass || coreUtils.GetValidAmount(scope.quoteViewModel.minPayrollExposureAmount) <= 50000 || referralQuote) {
                _submitExposureData();
            }
            else if (coreUtils.GetValidAmount(scope.quoteViewModel.minPayrollExposureAmount) > 50000) {
                var _classDescId, _classDescKeyId;
                if (_quoteViewModel.showProfession) {
                    _classDescId = scope.quoteViewModel.selectedClassDescId;
                    _classDescKeyId = scope.quoteViewModel.selectedClassDescKeyId;
                }
                else if (_quoteViewModel.showIndustry && _quoteViewModel.class && _quoteViewModel.class.ClassDescriptionId != '-1') {
                    _classDescId = scope.quoteViewModel.class ? scope.quoteViewModel.class.ClassDescriptionId : null;
                    _classDescKeyId = '';
                }
                if (!(_classDescId == null && _classDescId == undefined)) {
                    loadingService.showLoader();
                    quoteService.getCompanionCodes(_classDescId, scope.quoteViewModel.minPayrollExposureAmount).then(function (res) {
                        if (res.companionClassList && res.companionClassList.length > 0) {
                            _getCompanionCodeData(res.companionClassList);
                            scope.quoteViewModel.showCompanionClass = true;
                            loadingService.hideLoader();
                        }
                        else {
                            scope.msmcData = null;
                            _submitExposureData();
                        }
                    });
                }

            }
        }

        var _getCompanionCodeData = function (companionClassData) {
            var classDescId;
            var classCode;
            var description;
            if (scope.quoteViewModel.selectedSearch == "1") {
                if (scope.quoteViewModel.class) {
                    classDescId = scope.quoteViewModel.class.ClassDescriptionId;
                    classCode = scope.quoteViewModel.class.ClassCode;
                    description = scope.quoteViewModel.class.Description;
                }
            }
            else {
                classDescId = scope.quoteViewModel.selectedClassDescId;
                classCode = scope.quoteViewModel.selectedClassCode;
                description = scope.quoteViewModel.businessName;
            }
            scope.msmcData = {
                primaryClassData: {
                    classDescId: classDescId,
                    state: serverQuoteVm.County.State,
                    description: description,
                    classCode: classCode
                },
                companionCodeData: companionClassData,
                isMultiClass: scope.quoteViewModel.isMultiClass,
                isMultiState: scope.quoteViewModel.isMultiState,
                moreClassRequired: false
            }
            scope.$$phase || scope.$apply();
        }

        scope.minPayrollAllResponseRecieved = true;
        _quoteViewModel.updateMinPayRollFirstAlertResponse = function (val) {
            //_quoteViewModel.showMinPayRollSecondWarning = null;
            scope.minPayrollAllResponseRecieved = true;
            _quoteViewModel.showMinPayRollSecondWarning = false;
            if (val) {
                scope.quoteViewModel.showMinPayRollFirstWarning = false;
                scope.quoteViewModel.showMinPayRollSecondWarning = true;
                scope.minPayrollAllResponseRecieved = false;
            }
            else {
                scope.quoteViewModel.showMinPayRollFirstWarning = false;
                scope.quoteViewModel.minPayrollExposureAmount = '';
                scope.quoteViewModel.showMinPayRollSecondWarning = false;
                scope.minPayrollAllResponseRecieved = true;
            }
        }

        _quoteViewModel.updateMinPayRollSecondAlertResponse = function (val) {
            if (!val) {
                loadingService.showLoader();
                quoteService.getStateType().then(function (response) {
                    _isGoodState = response == "True" ? true : false;
                    scope.quoteViewModel.showMinPayRollSecondWarning = false;
                    scope.minPayrollAllResponseRecieved = true;
                    loadingService.hideLoader();
                });
            }
            else {
                scope.quoteViewModel.minPayrollExposureAmount = $filter('number', 0)(parseInt(scope.quoteViewModel.minExpValidationAmount));
                scope.quoteViewModel.showMinPayRollSecondWarning = false;
                scope.minPayrollAllResponseRecieved = true;
                scope.quoteViewModel.validateExposureAmount();
                _isGoodState = null;
                _isGoodStateApplicable = null;
                scope.quoteViewModel.minExpValidation = true;
            }
        };

        _quoteViewModel.closeSaveForLaterModel = function (modelName)
        {
            //Comment : Here CLOSE remodal pop-up
            coreUtils.RemodelPopUp($('[data-remodal-id=' + modelName + ']'), false);

            //Comment : Here only when ModelSaveForLaterThankYou is closed
            if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou') {
                loadingService.showLoader();
            }
        }

        _quoteViewModel.saveForLater = function (controllercontext)
        {
            //reset error list
            scope.quoteViewModel.listOfErrors = [];

            //Comment : Here if form has been validated then show model POP-UP for further information
            if (_quoteViewModel.runValidations()) {

                //Comment : Here if form has been validated then show model POP-UP for further information
                coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);
            }
            else {
                scope.submitted = true;
                _scrollFormOnError();
                //alert("Please provide complete information in order to continue.")
            }
            _quoteViewModel.acceptFlag = true;
        }

        _quoteViewModel.submitSaveForLater = function (userEmailId)
        {
            //reset error list
            scope.quoteViewModel.listOfErrors = [];

            //Comment : Here check for all validation are successfully processed then proceed further
            if (_quoteViewModel.runValidations())
            {
                //_quoteViewModel.submit();
                //Comment : Here do not wait for data submission because it will create a long delay in displaying thank you message
                quoteService.submitSaveForLaterExposureData(_prepareQuoteSubmissionData(), userEmailId).then(function (data)
                {
                    if (data.resultStatus == "OK")
                    {
                    }
                    else if (data.resultStatus === undefined) {
                        coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), false);
                    }
                    setTimeout(function ()
                    {
                        coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                    }, 1500);
                });
            }
            else {
                scope.submitted = true;
                //_scrollFormOnError();
                //alert("Please provide complete information in order to continue.")
            }
        }

        scope.quoteViewModel = _quoteViewModel;

        scope.$watch("quoteViewModel.businessName", function (newValue, oldValue) {
            if (!coreUtils.IsEmptyString(newValue) && (newValue != oldValue)) {
                //scope.quoteViewModel.selectedClassDescId = null;
                //scope.quoteViewModel.selectedClassDescKeyId = null;
                //scope.quoteViewModel.selectedClassDirectSales = null;
                //scope.quoteViewModel.selectedClassCode = null;
                scope.msmcData = null;
                scope.quoteViewModel.showMinPayRollFirstWarning = false;
                scope.quoteViewModel.showMinPayRollSecondWarning = false;
            }
        });

        scope.$watch("quoteViewModel.minPayrollExposureAmount", function (newValue, oldValue) {
            if (oldValue != undefined && newValue != undefined && oldValue != newValue) {
                scope.disableSubmit = true;
            }
        });

        scope.voidValidity = function () {
            scope.disableSubmit = true;
        }

        var _scrollFormOnError = function () {
            //Comment : Here iterate errors list and display it to screen
            scope.quoteViewModel.listOfErrors = ['Please provide complete information in order to continue.'];
            coreUtils.ScrollToTop();
        }

        _init();
    }
}());