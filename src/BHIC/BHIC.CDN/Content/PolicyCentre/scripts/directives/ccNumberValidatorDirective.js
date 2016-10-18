(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.Dashboard.Directives').directive('creditCardNumber', [creditCardNumberFn]);

    function creditCardNumberFn() {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {

                element.bind('blur', function () {

                    //set intitial value to false
                    modelCtrl.$setValidity('validCard', false);

                    var inputtxt = element.val();
                    if (!inputtxt) { return ''; }

                    //validate American Express credit card
                    var americanCardNumber = /^(?:3[47][0-9]{13})$/;
                    if (inputtxt.match(americanCardNumber)) {
                        modelCtrl.$setValidity('validCard', true);
                    }

                    //validate Visa credit card
                    var visaCardNumber = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
                    if (inputtxt.match(visaCardNumber)) {
                        modelCtrl.$setValidity('validCard', true);
                    }

                    //validate a MasterCard
                    var masterCardNumber = /^(?:5[1-5][0-9]{14})$/;
                    if (inputtxt.match(masterCardNumber)) {
                        modelCtrl.$setValidity('validCard', true);
                    }

                    //validate a Discover Card
                    var discoverCardNumber = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
                    if (inputtxt.match(discoverCardNumber)) {
                        scope.paymentPlan.isInvalidCard = false;
                        modelCtrl.$setValidity('validCard', true);
                    }

                    //validate a Diners Club Card
                    var dinersClubCardNumber = /^(?:3(?:0[0-5]|[68][0-9])[0-9]{11})$/;
                    if (inputtxt.match(dinersClubCardNumber)) {
                        modelCtrl.$setValidity('validCard', true);
                    }

                    //validate a JCB Card
                    var jcbCardNumber = /^(?:(?:2131|1800|35\d{3})\d{11})$/;
                    if (inputtxt.match(jcbCardNumber)) {
                        modelCtrl.$setValidity('validCard', true);
                    }
                    scope.$apply();
                });
            }
        };
    }

})();




//function creditCardNumberFn() {
//    return {
//        link: function (scope, elem, attrs) {
//            elem.bind('blur', function () {
//                scope.isInvalidCard = true;
//               
//            });
//        }
//    }
//}