(
    function ()
    {
        'use strict';

        function quoteReferralController(scope, quotereferralPageDataProvider)
        {
            var self = this;

            //comment : here model to get & store validation messages
            var _validationMessages = {};
            var _modelErrors = {};
            var _wcQuoteModel = {};
            var _businessTypes = [];

            function _init() {                
                _getQuoteReferralValidation();
                _getBusinessTypes();
            };

            var _getWcQuoteModel = function (model) {
                //self.wcQuoteModel.state = model.MailState;
                //self.wcQuoteModel.zipCode = model.MailZip;

                self.quoteReferralInfo.businessMailingInfo.state = model.MailState;
                self.quoteReferralInfo.businessMailingInfo.zipCode = model.MailZip;
            };

            //Comment : Here get all validation messages for this form
            var _getQuoteReferralValidation = function () {

                var promise = quotereferralPageDataProvider.getQuoteReferralValidation();

                promise.then(function (data) {

                    //Comment : Here get all errors then only get/set required errors for this form
                    _validationMessages = data;

                    var msgErrors = {};
                    msgErrors = _validationMessages.errors;

                    self.modelErrors = {
                        businessType: msgErrors['BusinessType'][0],
                        businessName: msgErrors['BusinessName'][0],
                        businessContactEmail: msgErrors['Email'][0],
                        businessContactFName: msgErrors['ContactFirstName'][0],
                        businessContactLName: msgErrors['ContactLastName'][0],
                        businessContactPhone: msgErrors['ContactPhone'][0],
                        businessMailAddressLine1: msgErrors['MailAddr1'][0],
                        businessMailCity: msgErrors['MailCity'][0],
                    };

                    _validationMessages = {};
                });
            };

            //Comment : Here get list of business type
            var _getBusinessTypes = function () {

                var promise = quotereferralPageDataProvider.getBusinessTypes();

                promise.then(function (data) {

                    //Comment : Here get all errors then only get/set required errors for this form
                    self.businessTypes = data.businessTypes;
                });
            };

            //Comment : Here submit form data
            var _saveQuoteReferralData = function (hasValidForm, wcQuoteReferralModel)
            {

                //Comment : Here check for all validation are successfully processed then proceed further
                if (hasValidForm)
                {
                    var config =
                    {
                        headers: {
                            '__RequestVerificationToken': angular.element("input[name='__RequestVerificationToken']").val()
                        }
                    };

                    //Comment : here call service to submit questions response                    
                    var promise = quotereferralPageDataProvider.submitQuoteReferralData(wcQuoteReferralModel, config);

                    promise.then(function (data) {
                        
                        alert(data.resultText);

                        //Comment : here reset form after submit
                        self.formSubmitted = false;
                        self.quoteReferralInfo = {};
                        //form_quoteReferral.reset();
                        scope.form_quoteReferral.$setPristine(true);

                        //Comment : Here check operation status
                        if (data.resultStatus == 'OK') {
                            //Comment : here redirect to response page
                            window.location.assign('/Landing/WcHome/ResultsReferQResponse');
                        }
                    });                    
                }
                else
                    alert('Please correct all shown errors.');
            };

            var _quoteReferralInfo = {

                //Comment : Here "Business Information" object
                businessInfo: {
                    businessType: '',
                    businessName: ''//Process Engineering
                },

                //Comment : Here "Business Contact Information" object
                businessContactInfo: {
                    businessContactEmail: '',
                    businessContactFName: '',
                    businessContactLName: '',
                    businessContactPhone: null
                },

                //Comment : Here "Business Mailing Address" object
                businessMailingInfo: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    //state: '',
                    //zipCode: '',
                    state: 'NY',
                    zipCode: '13736',
                },

            };

            //Comment : Here to enable submit button & allow submission of quote-referral form
            var _formSubmitted = false;

            angular.extend(self, {
                init: _init(),
                modelErrors: _modelErrors,
                quoteReferralInfo: _quoteReferralInfo,
                formSubmitted: _formSubmitted,
                submitQuoteReferralForm: _saveQuoteReferralData,
                getWcQuoteModel: _getWcQuoteModel,
                businessTypes: _businessTypes
            });
        }

        /**
           * @controller : quoteReferralPageCtrl
           * @Ctrldesc : this controller will collect contact information of user when questionnaire response will not direct allow user to buy Policy online. 
                        this conatct information will be later use for offline process 
           * @ngInject : inject $scope module into controller for certain Moel-View communication
        */
        angular
           .module('bhic-app.wc')
           .controller('quoteReferralPageCtrl', ['$scope', 'bhic-app.wc.services.quotereferralPageData', quoteReferralController]);

    }
)();