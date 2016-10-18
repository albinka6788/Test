(function () {
    'use strict'
    angular.module("BHIC.WC.Controllers")
    .controller('purchaseQuote', [
        'purchaseQuotePageDefaults',
        '$scope',
        'quotePurchaseService',
        'navigationService',
        '$timeout',
        'coreUtils',
        '$location',
        'loadingService',
        'savedQuotePurchaseData',
        'restServiceConsumer',
        '$q',
        purchaseQuoteFn])
    function purchaseQuoteFn(purchaseQuotePageDefaults, scope, quotePurchaseService, navigationService, $timeout, coreUtils, $location, loadingService, savedQuotePurchaseData, restServiceProvider, q) {

        function _init() {

            //add complete class on quote menu if user comes directly to purchaseQuote
            // navigationService.enableNavigation('lnkQuote');

            //add complete class on quote-summary menu if user comes directly to purchaseQuote
            // navigationService.enableNavigation('lnkQuoteSummary');

            //add complete class on purchase menu if user comes directly to purchaseQuote
            // navigationService.enableNavigation('lnkPurchaseQuote');

            _purchaseQuoteViewModel.purchaseQuotePageDefaults = JSON.parse(purchaseQuotePageDefaults.data);
            if (_purchaseQuoteViewModel.purchaseQuotePageDefaults && _purchaseQuoteViewModel.purchaseQuotePageDefaults.countyData) {
                _purchaseQuoteViewModel.businessMailingAddress = {
                    state: _purchaseQuoteViewModel.purchaseQuotePageDefaults.countyData.State,
                    //city: _purchaseQuoteViewModel.purchaseQuotePageDefaults.countyData.City,
                    zipCode: _purchaseQuoteViewModel.purchaseQuotePageDefaults.countyData.ZipCode
                }
            }
            _purchaseQuoteViewModel.acceptFlag = true;
            _purchaseQuoteViewModel.taxIdType = [
                { Text: "Federal Tax ID Number (TIN)", Value: 'E', selected: true },
                { Text: "Social Security Number (SSN)", Value: 'S', selected: false }
            ];
            /*Set the default values*/

            _purchaseQuoteViewModel.selectedTaxIdType = 'E';
            _purchaseQuoteViewModel.showTaxId = true;

            _purchaseQuoteViewModel.pageTitle = "Provide contact information for you and your business"

            _purchaseQuoteViewModel.submitButtonCaption = "Continue";
            _purchaseQuoteViewModel.taxId = '';
            _purchaseQuoteViewModel.SSN = '';
            if (!coreUtils.IsEmptyString(_purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdType)) {
                scope.isDisabled = true;
                _purchaseQuoteViewModel.selectedTaxIdType = _purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdType;
                _purchaseQuoteViewModel.showTaxId = (_purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdType == 'E') ? true : false;
                _purchaseQuoteViewModel.taxId = (_purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdType == 'E') ? _purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdNumber : '';
                _purchaseQuoteViewModel.SSN = (_purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdType == 'S') ? _purchaseQuoteViewModel.purchaseQuotePageDefaults.taxIdNumber : '';

            }
            else {
                scope.isDisabled = false;
                _purchaseQuoteViewModel.selectedTaxIdType = 'E';
                _purchaseQuoteViewModel.showTaxId = true;
            }

            _purchaseQuoteViewModel.primaryContactInformation = { copyPrimaryContactInfoToBusinessContact: true }
            //loadingService.hideLoader();
            if (_purchaseQuoteViewModel.purchaseQuotePageDefaults.loggedInUserDetails) {
                scope.disableUserAccountDetails = true;
                _purchaseQuoteViewModel.account = {
                    email: _purchaseQuoteViewModel.purchaseQuotePageDefaults.loggedInUserDetails.Email,
                    password: _purchaseQuoteViewModel.purchaseQuotePageDefaults.loggedInUserDetails.Password,
                    confirmPassword: _purchaseQuoteViewModel.purchaseQuotePageDefaults.loggedInUserDetails.Password
                }
            }
            if (savedQuotePurchaseData.model) {
                _purchaseQuoteViewModel.retrievedPurchaseQuoteData = savedQuotePurchaseData;
                _loadQuotePurchaseData(_purchaseQuoteViewModel.retrievedPurchaseQuoteData);
            }


            //Comment : here call service to get valid email id reg-ex
            //scope.validEmailRegEx = coreUtils.ValidEmailRegEx();
            scope.validEmailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

            $('.tooltip').tooltip();
            
            if (_purchaseQuoteViewModel.account) {
                _purchaseQuoteViewModel.checkUserExistence(_purchaseQuoteViewModel.account.email, _purchaseQuoteViewModel.account.password)
            }
            $timeout(function () {
                scope.purchaseQuoteViewModel = _purchaseQuoteViewModel;
            });
        }

        // Progress bar navigation
        scope.goTo = function (sender) {
            if ($(sender.target).attr("url") != "#") {
                coreUtils.setNavigation($(sender.target).attr("url"), true);

                //navigation = 
                //{
                //    path: $(sender.target).attr("url") || '',
                //    status: true
                //};

                $location.path($(sender.target).attr("url"));
            }
        }

        var _purchaseQuoteViewModel = [];
        var _isValidTaxIdOrSSN = false;
        /*Set the data recieved from Server */

        /*Set state, zip and city as recieved from Session*/

        //Comment : Here if any user or functional errors while posting data then show form level errors
        _purchaseQuoteViewModel.listOfErrors = [];

        //Comment : Here by-default set existing user flag as FALSE
        _purchaseQuoteViewModel.isExistingUser = true;

        /*Private Methods Start*/

        function _loadQuotePurchaseData(data) {
            //_purchaseQuoteViewModel.personalContact.sameAsContact = data.model.PersonalContact.SameAsContact;
            if (data.model.PersonalContact != null && data.model.PersonalContact != undefined) {
                _purchaseQuoteViewModel.primaryContactInformation = {
                    email: data.model.PersonalContact.Email,
                    confirmEmail: data.model.PersonalContact.ConfirmEmail,
                    firstName: data.model.PersonalContact.FirstName,
                    lastName: data.model.PersonalContact.LastName,
                    phone: data.model.PersonalContact.PhoneNumber,
                    copyPrimaryContactInfoToBusinessContact: data.model.PersonalContact.SameAsContact
                }
            }
            if (data.model.BusinessContact != null && data.model.BusinessContact != undefined) {
                _purchaseQuoteViewModel.businessContactInformation = {
                    email: data.model.BusinessContact.Email,
                    bConfirmEmail: data.model.BusinessContact.ConfirmEmail,
                    firstName: data.model.BusinessContact.FirstName,
                    lastName: data.model.BusinessContact.LastName,
                    phone: data.model.BusinessContact.PhoneNumber
                }
            }

            if (data.model.Account && !_purchaseQuoteViewModel.account) {
                _purchaseQuoteViewModel.account = {
                    email: data.model.Account.Email,
                    password: '',//data.model.Account.Password,
                    confirmPassword: '',//data.model.Account.ConfirmPassword
                }
            }

            if (data.model.BusinessInfo != null && data.model.BusinessInfo != undefined) {
                _purchaseQuoteViewModel.businessInfo = {
                    businessName: data.model.BusinessInfo.BusinessName,
                    businessType: data.model.BusinessInfo.BusinessType,
                    taxIdType: data.model.BusinessInfo.TaxIdType || 'E'
                }

                if (scope.isDisabled) {
                    _purchaseQuoteViewModel.selectedTaxIdType = (_purchaseQuoteViewModel.showTaxId) ? 'E' : 'S';
                    data.model.BusinessInfo.TaxIdType = (_purchaseQuoteViewModel.showTaxId) ? 'E' : 'S';
                    data.model.BusinessInfo.TaxIdOrSSN = (_purchaseQuoteViewModel.showTaxId) ? _purchaseQuoteViewModel.taxId : _purchaseQuoteViewModel.SSN;
                }
                _purchaseQuoteViewModel.businessName = data.model.BusinessInfo.BusinessName;
                _purchaseQuoteViewModel.selectedTaxIdType = data.model.BusinessInfo.TaxIdType || 'E';
                if (_purchaseQuoteViewModel.selectedTaxIdType != null && _purchaseQuoteViewModel.selectedTaxIdType != undefined && _purchaseQuoteViewModel.selectedTaxIdType != '') {
                    switch (_purchaseQuoteViewModel.selectedTaxIdType) {
                        case 'E': _purchaseQuoteViewModel.taxId = data.model.BusinessInfo.TaxIdOrSSN;
                            break;
                        case 'S': _purchaseQuoteViewModel.SSN = data.model.BusinessInfo.TaxIdOrSSN;
                            break;
                    }
                }
                else {
                    _purchaseQuoteViewModel.taxId = data.model.BusinessInfo.TaxIdOrSSN;
                }
                var SelectedBusinessType = _purchaseQuoteViewModel.purchaseQuotePageDefaults.businessTypeList.filter(function (obj) {
                    return (obj.BusinessTypeCode === data.model.BusinessInfo.BusinessType);
                });
                if (SelectedBusinessType.length > 0) {
                    _purchaseQuoteViewModel.selectedBusiness = SelectedBusinessType[0];
                    _purchaseQuoteViewModel.updateBusinessType(_purchaseQuoteViewModel.selectedBusiness.BusinessTypeCode);
                }
            }
            if (data.model.BusinessInfo != null && data.model.BusinessInfo != undefined) {
                _purchaseQuoteViewModel.individual = {
                    firstName: data.model.BusinessInfo.FirstName,
                    middleName: data.model.BusinessInfo.MiddleName,
                    lastName: data.model.BusinessInfo.LastName
                }
            }

            _purchaseQuoteViewModel.updateTaxIdOrSSN();
            //_purchaseQuoteViewModel.businessInfo.taxIdOrSSN = data.model.BusinessInfo.TaxIdOrSSN;
            if (data.model.MailingAddress || !_purchaseQuoteViewModel.businessMailingAddress) {

                _purchaseQuoteViewModel.businessMailingAddress = {
                    addressLine1: data.model.MailingAddress.AddressLine1,
                    addressLine2: data.model.MailingAddress.AddressLine2,
                    city: data.model.MailingAddress.City,
                    state: data.model.MailingAddress.State,
                    zipCode: data.model.MailingAddress.Zip
                }
            }
            _purchaseQuoteViewModel.SUIN = data.model.SUIN;
        }

        function _runValidations() {
            if (_purchaseQuoteViewModel.primaryContactInformation.copyPrimaryContactInfoToBusinessContact) {
                _purchaseQuoteViewModel.businessContactInformation = _purchaseQuoteViewModel.primaryContactInformation;
            }
            /*If business Type is not selected then return false */
            if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.selectedBusiness.BusinessTypeCode)) {
                return false;
            }
            else {
                /*If business Type is Individual */
                if (_purchaseQuoteViewModel.showIndividualFields) {
                    if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.individual)) {
                        return false;
                    }
                    else if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.individual.firstName)
                        || coreUtils.IsEmptyString(_purchaseQuoteViewModel.individual.lastName))
                        return false;
                }
                else {
                    /*If business Type is other than Individual */
                    if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessName)) {
                        return false;
                    }
                }
            }

            /*Check for taxId and SSN*/
            if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.taxId) && coreUtils.IsEmptyString(_purchaseQuoteViewModel.SSN))
                return false;

            /*Check for Account information*/
            if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.account))
                return false;
            else if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.account.email)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.account.password))
                return false;

            /*Check for Business Mailing address*/
            if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.businessMailingAddress))
                return false;
            else if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessMailingAddress.addressLine1)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessMailingAddress.city)) {
                return false;
            }

            /*Check for Business contact information*/
            if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.businessContactInformation))
                return false;
            else if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessContactInformation.email)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessContactInformation.firstName)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessContactInformation.lastName)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.businessContactInformation.phone))
                return false;

            /*Check for Primary contact information*/
            if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.businessContactInformation))
                return false;
            else if (coreUtils.IsEmptyString(_purchaseQuoteViewModel.primaryContactInformation.email)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.primaryContactInformation.firstName)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.primaryContactInformation.lastName)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.primaryContactInformation.phone))
                return false;

            else if (!scope.disableUserAccountDetails &&
                (coreUtils.IsEmptyString(_purchaseQuoteViewModel.account.email)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.account.password)
                || coreUtils.IsEmptyString(_purchaseQuoteViewModel.account.confirmPassword)
                || _purchaseQuoteViewModel.account.confirmPassword != _purchaseQuoteViewModel.account.password))
                return false;
            else if (_purchaseQuoteViewModel.purchaseQuotePageDefaults.stateUINDetails && coreUtils.IsEmptyString(_purchaseQuoteViewModel.SUIN) && !scope.invalidSUIN)
                return false;
            return true;
        }

        /*Private Methods End*/

        /*Public methods Start*/
        _purchaseQuoteViewModel.updateBusinessType = function (option) {
            switch (option) {
                case "I": _purchaseQuoteViewModel.showIndividualFields = true;
                    _purchaseQuoteViewModel.showBusinessFields = false;
                    break;
                default: _purchaseQuoteViewModel.showIndividualFields = false;
                    _purchaseQuoteViewModel.showBusinessFields = true;
                    //if (_purchaseQuoteViewModel.individual != undefined &&
                    //    (_purchaseQuoteViewModel.individual.firstName != undefined || _purchaseQuoteViewModel.individual.lastName != undefined)
                    //   )
                    //{
                    //    _purchaseQuoteViewModel.individual.firstName = '';
                    //    _purchaseQuoteViewModel.individual.lastName = '';
                    //    _purchaseQuoteViewModel.primaryContactInformation.lastName = '';
                    //    _purchaseQuoteViewModel.primaryContactInformation.firstName = '';                        
                    //    _purchaseQuoteViewModel.businessContactInformation = {
                    //        email: '',
                    //        bConfirmEmail: '',
                    //        firstName: '',
                    //        lastName: '',
                    //        phone: ''
                    //    };
                    //}
                    break;
            }
        };

        _purchaseQuoteViewModel.updateTaxIdOrSSN = function () {
            _purchaseQuoteViewModel.finalSelectedTaxIdType = _.find(_purchaseQuoteViewModel.taxIdType,
                function (val) {
                    if (val.Value == _purchaseQuoteViewModel.selectedTaxIdType)
                        return val;
                })
            switch (_purchaseQuoteViewModel.selectedTaxIdType) {
                case "E": _purchaseQuoteViewModel.showTaxId = true;
                    break;
                case "S": _purchaseQuoteViewModel.showTaxId = false;
                    break;
            }
        };

        _purchaseQuoteViewModel.validateCity = function () {

            //debugger;
            //Comment : Here do action only when city has some value
            var enteredCity = _purchaseQuoteViewModel.businessMailingAddress.city;
            if (enteredCity != null && enteredCity != undefined && enteredCity != '') {
                coreUtils.controlLevelLoader($('#bmCity'), true);
                var promise = quotePurchaseService.validateCityStateZip(_purchaseQuoteViewModel.businessMailingAddress.zipCode, enteredCity, _purchaseQuoteViewModel.businessMailingAddress.state);
                promise.then(function (response) {
                    $timeout(function () {
                        _purchaseQuoteViewModel.invalidZipCityState = response.data == "True" ? true : false;
                        if (_purchaseQuoteViewModel.invalidZipCityState) {
                            //  _purchaseQuoteViewModel.businessMailingAddress.city = "";
                            scope.purchaseQuoteForm.bmCity.$pristine = true;
                        }
                    });
                }).finally(function () {
                    coreUtils.controlLevelLoader($('#bmCity'), false);
                });
            }
        };

        _purchaseQuoteViewModel.updateBusinessContactInformation = function () {
            if (!_purchaseQuoteViewModel.primaryContactInformation.copyPrimaryContactInfoToBusinessContact) {
                if (scope.purchaseQuoteViewModel.selectedBusiness && scope.purchaseQuoteViewModel.selectedBusiness.BusinessTypeCode == "I") {
                    _purchaseQuoteViewModel.businessContactInformation = {
                        email: '',
                        bConfirmEmail: '',
                        firstName: scope.purchaseQuoteViewModel.individual.firstName,
                        lastName: scope.purchaseQuoteViewModel.individual.lastName,
                        phone: ''
                    };
                }
                else {
                    _purchaseQuoteViewModel.businessContactInformation = null;
                }
            }
            else if (coreUtils.IsEmptyObject(_purchaseQuoteViewModel.primaryContactInformation.copyPrimaryContactInfoToBusinessContact) ||
                _purchaseQuoteViewModel.primaryContactInformation.copyPrimaryContactInfoToBusinessContact) {
                _purchaseQuoteViewModel.businessContactInformation = _purchaseQuoteViewModel.primaryContactInformation;
            }
        }

        _purchaseQuoteViewModel.submitPurchaseData = function () {
            /*Mean while till asyncValidator is not implemented in FEIN directive
            We will have to check FEIN and SSN here */

            //reset error list
            _purchaseQuoteViewModel.listOfErrors = [];

            if (!scope.invalidSUIN) {
                _validateTaxIDandFEIN().then(function (data) {
                    if (data) {
                        if (_runValidations() && !_purchaseQuoteViewModel.invalidZipCityState) {
                            _purchaseQuoteViewModel.checkUserExistence(scope.purchaseQuoteViewModel.account.email, scope.purchaseQuoteViewModel.account.password).then(function (res) {
                                if (res) {
                                    loadingService.showLoader();
                                    quotePurchaseService.submitPurchaseData(_purchaseQuoteViewModel).then(function (data) {
                                        if (data.response) {

                                            //loadingService.hideLoader();
                                            $location.path('/BuyPolicy');
                                        }
                                        else {
                                            //Comment : Here if any user error messages found then show it to user
                                            if (data.errorList && data.errorList.length == 0)
                                                $location.path('/AppError');
                                            else {
                                                loadingService.hideLoader();
                                                _purchaseQuoteViewModel.listOfErrors = data.errorList;
                                                coreUtils.ScrollToTop();
                                                //_purchaseQuoteViewModel.listOfErrors = data.errorList;    //Old line
                                            }
                                        }
                                    });
                                }
                            });
                        }
                        else {
                            loadingService.hideLoader();
                            _purchaseQuoteViewModel.scrollFormOnError();
                            //alert("Please provide complete information in order to continue.")
                        }
                    }
                    else {
                        loadingService.hideLoader();
                        _purchaseQuoteViewModel.scrollFormOnError();
                    }
                });
            }
        }

        _purchaseQuoteViewModel.saveForLater = function (controllercontext) {
            //reset error list
            _purchaseQuoteViewModel.listOfErrors = [];

            //Comment : Here if form has been validated then show model POP-UP for further information
            if (_runValidations()) {
                //Comment : Here if form has been validated then show model POP-UP for further information
                coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);
            }
            else {
                scope.submitted = true;
                _purchaseQuoteViewModel.scrollFormOnError();
                //alert("Please provide complete information in order to continue.")
            }
            _purchaseQuoteViewModel.acceptFlag = true;
        }

        _purchaseQuoteViewModel.closeSaveForLaterModel = function (modelName) {
            //Comment : Here CLOSE remodal pop-up
            coreUtils.RemodelPopUp($('[data-remodal-id=' + modelName + ']'), false);

            //Comment : Here only when ModelSaveForLaterThankYou is closed
            if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou') {
                loadingService.showLoader();
            }
        }

        _purchaseQuoteViewModel.submitSaveForLater = function (userEmailId) {
            //reset error list
            _purchaseQuoteViewModel.listOfErrors = [];

            if (!scope.invalidSUIN) {
                _validateTaxIDandFEIN().then(function (res) {
                    if (res) {

                        if (_runValidations()) {

                            _purchaseQuoteViewModel.checkUserExistence(scope.purchaseQuoteViewModel.account.email, scope.purchaseQuoteViewModel.account.password).then(function (res) {
                                if (res) {
                                    quotePurchaseService.saveForLaterPurchaseQuoteData(_purchaseQuoteViewModel, userEmailId).then(function (data) {
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
                            });

                        }
                        else {
                            //_purchaseQuoteViewModel.scrollFormOnError();
                            //alert("Please provide complete information in order to continue.")
                        }
                    }
                })
            }
            //Comment : Here check for all validation are successfully processed then proceed further
        }

        _purchaseQuoteViewModel.submitSaveForLaterOLD = function (hasValidForm, controllercontext) {
            //reset error list
            _purchaseQuoteViewModel.listOfErrors = [];

            if (!scope.invalidSUIN) {
                _validateTaxIDandFEIN().then(function (res) {
                    if (res) {

                        if (hasValidForm && _runValidations()) {

                            _purchaseQuoteViewModel.checkUserExistence(scope.purchaseQuoteViewModel.account.email, scope.purchaseQuoteViewModel.account.password).then(function (res) {
                                if (res) {
                                    quotePurchaseService.saveForLaterPurchaseQuoteData(_purchaseQuoteViewModel, controllercontext.userEmailId).then(function (data) {
                                        if (data.resultStatus == "OK") {
                                        }

                                        setTimeout(function () {
                                            coreUtils.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                                        }, 1500);
                                    });
                                }
                            });

                        }
                        else {
                            _purchaseQuoteViewModel.scrollFormOnError();
                            //alert("Please provide complete information in order to continue.")
                        }
                    }
                })
            }
            //Comment : Here check for all validation are successfully processed then proceed further
        }

        _purchaseQuoteViewModel.updateEmailId = function (val) {
            $timeout(function () {
                if (scope.purchaseQuoteViewModel.account)
                    scope.purchaseQuoteViewModel.account.email = val;
                else {
                    scope.purchaseQuoteViewModel.account = {
                        email: val,
                        password: ''
                    }
                }
            });
        }

        _purchaseQuoteViewModel.updateFirstName = function (val) {
            $timeout(function () {
                if (scope.purchaseQuoteViewModel.selectedBusiness && scope.purchaseQuoteViewModel.selectedBusiness.BusinessTypeCode == "I") {
                    scope.purchaseQuoteViewModel.primaryContactInformation.firstName = val;
                    _purchaseQuoteViewModel.businessContactInformation = {
                        email: '',
                        bConfirmEmail: '',
                        firstName: scope.purchaseQuoteViewModel.individual.firstName,
                        lastName: scope.purchaseQuoteViewModel.individual.lastName,
                        phone: ''
                    };
                }
            });
        }

        _purchaseQuoteViewModel.updateLastName = function (val) {
            $timeout(function () {
                if (scope.purchaseQuoteViewModel.selectedBusiness && scope.purchaseQuoteViewModel.selectedBusiness.BusinessTypeCode == "I") {
                    scope.purchaseQuoteViewModel.primaryContactInformation.lastName = val;
                    _purchaseQuoteViewModel.businessContactInformation = {
                        email: '',
                        bConfirmEmail: '',
                        firstName: scope.purchaseQuoteViewModel.individual.firstName,
                        lastName: scope.purchaseQuoteViewModel.individual.lastName,
                        phone: ''
                    };
                }
            });
        }
        _purchaseQuoteViewModel.checkUserExistence = function (val, pwd) {
            var deferred = q.defer();
            var emailId = !coreUtils.IsEmptyString(val) ? val.replace(/[\s]/g, '') : null;

            //Comment : Here if empty email then break execution
            if (emailId == undefined || emailId == null || emailId.length == 0)
                return;
            var promise = quotePurchaseService.isExistingUser(emailId);
            promise.then(function (data) {
                scope.mailSentSuccessfully = false;
                $timeout(function () {
                    if (data != null) {
                        //Comment : Here if user exists/found then 
                        if (data.resultStatus == 'True') {
                            //it means user-account found
                            scope.userEmailExists = true;
                            if (!coreUtils.IsEmptyString(pwd)) {
                                quotePurchaseService.hasValidPassword(emailId, pwd).then(function (data) {
                                    if (data != null) {
                                        //Comment : Here if not-validated then 
                                        if (data.resultStatus == 'False') {
                                            if (scope.purchaseQuoteForm.aPassword && scope.purchaseQuoteForm.aPassword.$error)
                                                scope.purchaseQuoteForm.aPassword.$error.invalidPassword = true;
                                            loadingService.hideLoader();
                                            deferred.resolve(false);

                                        }
                                        else if (data.resultStatus == 'True') {
                                            if (scope.purchaseQuoteForm.aPassword && scope.purchaseQuoteForm.aPassword.$error)
                                                scope.purchaseQuoteForm.aPassword.$error.invalidPassword = false;
                                            deferred.resolve(true);
                                        }
                                    }
                                }).catch(function (exception) {
                                    if (scope.purchaseQuoteForm.aPassword && scope.purchaseQuoteForm.aPassword.$error)
                                        scope.purchaseQuoteForm.aPassword.$error.invalidPassword = false;
                                    deferred.resolve(true);
                                });
                            }
                        }
                        else if (data.resultStatus == 'False') {
                            scope.userEmailExists = false;
                            deferred.resolve(true);
                        }
                    }
                })
            }).catch(function (exception) {
                scope.userEmailExists = false;
                deferred.resolve(false);
            });

            return deferred.promise;
        }

        function _validateTaxIDandFEIN() {
            loadingService.showLoader();
            var deferred = q.defer();

            var val = _purchaseQuoteViewModel.showTaxId ? _purchaseQuoteViewModel.taxId : _purchaseQuoteViewModel.SSN;
            if (!coreUtils.IsEmptyString(val)) {
                var taxIdNumber = val.replace(/[\s]/g, '');

                //Comment : Here if empty email then break execution
                if (taxIdNumber.length == 0)
                    return;

                //Comment : here call service to validate entered FEIn number
                var promise = restServiceProvider.isValidTaxIdNumber(taxIdNumber, true);

                promise.then(function (data) {
                    if (data != null) {
                        //Comment : Here if taxid-number valid then 
                        if (data.resultStatus == 'True') {
                            _purchaseQuoteViewModel.showTaxId ? scope.purchaseQuoteForm.taxId.$setValidity('validTaxIdNumber', true) : scope.purchaseQuoteForm.ssn.$setValidity('validTaxIdNumber', true)
                            deferred.resolve(true);
                        }
                        else if (data.resultStatus == 'False') {
                            _purchaseQuoteViewModel.showTaxId ? scope.purchaseQuoteForm.taxId.$setValidity('validTaxIdNumber', false) : scope.purchaseQuoteForm.ssn.$setValidity('validTaxIdNumber', false)
                            deferred.resolve(false);
                            loadingService.hideLoader();
                        }
                    }
                });

            }
            else {
                deferred.resolve(false);
                loadingService.hideLoader();
            }
            return deferred.promise;
        };

        _purchaseQuoteViewModel.forgotPassword = function () {
            if (scope.purchaseQuoteViewModel.account && !coreUtils.IsEmptyString(scope.purchaseQuoteViewModel.account.email)) {
                loadingService.showLoader();
                quotePurchaseService.forgotPassword(scope.purchaseQuoteViewModel.account.email).then(function (res) {
                    scope.mailSentSuccessfully = res.success;
                    loadingService.hideLoader();
                });
            }
        }

        _purchaseQuoteViewModel.validateSUIN = function (val) {
            if (!coreUtils.IsEmptyString(val)) {
                if (/^(\d)\1+$/.test(val)) {
                    scope.invalidSUIN = true;
                }
                else if (val.split('_').length > 1) {
                    scope.invalidSUIN = true;
                }
                else if (/^0*1*2*3*4*5*6*7*8*9*0*$/.test(val)) {
                    scope.invalidSUIN = true;
                }
                else
                    scope.invalidSUIN = false;
            }
        }

        _purchaseQuoteViewModel.scrollFormOnError = function () {
            //Comment : Here iterate errors list and display it to screen
            _purchaseQuoteViewModel.listOfErrors = ['Please provide complete information in order to continue.'];
            coreUtils.ScrollToTop();
        }

        /*Public methods End*/
        _init();
    }

}());