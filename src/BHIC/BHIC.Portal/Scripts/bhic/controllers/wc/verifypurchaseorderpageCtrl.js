(
    function () {
        'use strict';

        function verifyPurchaseOrderPageController(scope, purchasepolicyPageDataProvider) {
            var self = this;

            //Comment : Here to enable submit button & allow submission of verify-order form
            var _formSubmitted = false;
            var _iAccept = false;

            //Comment : Here validation messages
            var _validationMessage = {
                ccNumRequired: 'The credit card number is required.',
                ccNumInvalid: 'The credit card number is invalid.',
                ccNameRequired: 'You must enter the name from your credit card.',
                ccExpMonthRequired: 'The credit card expiration month is required.',
                ccExpYearRequired: 'The credit card expiration year is required.'
            }

            var _paymentInfo = {
                ccType: '',
                ccNumber: '',
                ccName: '',
                ccExpireMonth: '',
                ccExpireYear: ''
            };

            //Comment : Here Purcahse model
            var _purchaseModel = {

                policy: {
                    date: ''
                },

                personalContact: {
                    sameAsContact: '',
                    email: '',
                    confirmEmail: '',
                    firstName: '',
                    lastName: '',
                    phoneNumber: ''
                },

                businessContact: {
                    bEmail: '',
                    bConfirmEmail: '',
                    bFirstName: '',
                    bLastName: '',
                    bPhoneNumber: ''
                },

                account: {
                    password: '',
                    confirmPassword: ''
                },

                businessInfo: {
                    businessName: '',
                    businessType: '',
                    taxIdType: '',
                    taxIdOrSSN: ''
                },

                mailingAddress: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    state: '',
                    zip: '',
                }
            };

            var _resultQuote = {
                CurrentDue: "",
                FutureInstallmentAmount: "",
                InstallmentFee: "",
                NoOfInstallment: "",
                Premium:""
            };

            function _init() {
                _getPurchaseData();
            };

            //Comment : Here get list of business type
            var _getPurchaseData = function () {

                var promise = purchasepolicyPageDataProvider.getPurchaseData();

                promise.then(function (data) {

                    if (data.success == true && data.purchaseModel.length > 0) {
                        //Comment : Here get purchase model string data into jsonObject
                        var purchaseModelData = angular.fromJson(data.purchaseModel);
                        var resultQuoteData = angular.fromJson(data.quoteResultData);

                        //Comment : Here copy retrieved jsonObject into controller model object
                        self.purchaseModel = angular.copy(purchaseModelData);
                        self.resultQuote = angular.copy(resultQuoteData);
                    }

                    console.log(self.purchaseModel);
                });
            };

            //Comment : Here redirect back to Purchase view/page to make editable again to update/correct information
            var _redirectToPurchase = function () {
                window.location.href = '/Landing/Wchome/Purchase';
            }

            function _createPolicy() {
                purchasepolicyPageDataProvider.createPolicy();
            };

            //Comment : Here extend slef as well as scope scope object into controller
            angular.extend(self, {
                init: _init(),
                formSubmitted: _formSubmitted,
                validationMessage: _validationMessage,
                iAccept: _iAccept,
                paymentInfo: _paymentInfo,
                purchaseModel: _purchaseModel,
                redirectToPurchase: _redirectToPurchase,
                resultQuote: _resultQuote,
                createPolicy: _createPolicy
            });
        }

        function validCreditCardNumber() {
            return {
                restrict: 'EA',
                require: '?ngModel',
                link: function (scope, elem, attr, ctrl) {

                    ctrl.$parsers.unshift(function (value) {

                        var cardType =
                          (/^5[1-5]/.test(value)) ? "mastercard"
                          : (/^4/.test(value)) ? "visa"
                          : (/^3[47]/.test(value)) ? 'amex'
                          : (/^6011|65|64[4-9]|622(1(2[6-9]|[3-9]\d)|[2-8]\d{2}|9([01]\d|2[0-5]))/.test(value)) ? 'discover'
                          : undefined;

                        ctrl.$setValidity('invalid', cardType === undefined ? false : true);
                        return (cardType === undefined ? '' : value);
                    });

                    //elem.bind('blur', function (event) {
                    //    alert(elem.val());
                    //});

                    //elem.bind('keypress', function (event) {
                    //    if (event.keyCode != 32) {
                    //        console.log(event.keyCode);
                    //    }
                    //});
                }
            }
        };

        /**
           * @controller : verifyPurchaseOrderPageCtrl
           * @Ctrldesc : this controller will show all information collected in previus purchase form.
                        allow user to review all information before making payment
           * @ngInject : inject $scope module into controller for certain Moel-View communication
        */
        angular
           .module('bhic-app.wc')
           .controller('verifyPurchaseOrderPageCtrl', ['$scope', 'bhic-app.wc.services.purchasePolicyPageData', verifyPurchaseOrderPageController])
           .directive('creditCardNumber', [validCreditCardNumber]);

    }
)();