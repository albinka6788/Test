(function () {
    'use strict';
    angular.module("BHIC.Dashboard.Controllers").controller('policyPaymentController', ['$scope', '$routeParams', '$location', 'loadingService', 'policyPremiumData', 'authorisedUserWrapperService', policyPaymentControllerFn])

    function policyPaymentControllerFn(scope, $routeParams, $location, loadingService, policyPremiumData, authorisedUserWrapperService) {       
        // Variable declaration

        var selected = $routeParams.CYBKey.substr($routeParams.CYBKey.length - 1, 1);

        scope.paymentPlan = {
            selectedOption:
                {
                    id: '',
                    value: '',
                    isEnabled: ''
                },
            isPaymentDisabled: true,
            isValidYear: '',
            isEmptyExpiration: '',
            amountReceived: ''
        };
     
        if (policyPremiumData.payments != null && policyPremiumData.payments != undefined) {

            angular.forEach(policyPremiumData.payments, function (input, index) {
                if (input.PaymentDate != null && input.PaymentDate != undefined) {
                    input.PaymentDate = getDate(input.PaymentDate);
                }
            });
        }

        if (policyPremiumData.futureBills != null && policyPremiumData.futureBills != undefined) {

            angular.forEach(policyPremiumData.futureBills, function (input, index) {
                if (input.DueDate != null && input.DueDate != undefined) {
                    input.DueDate = getDate(input.DueDate);
                }
            });
        }

        scope.payOption = [];
        if (policyPremiumData.payOptions != null && policyPremiumData.payOptions != undefined) {
            angular.forEach(policyPremiumData.payOptions, function (obj, ind) {
                if (selected == obj.Id)
                    scope.payOption.push({ "Id": obj.Id, "Value": obj.Value, "IsEnabled": true });
                else
                    scope.payOption.push({ "Id": obj.Id, "Value": obj.Value, "IsEnabled": false });
            });
        }

        scope.payments = policyPremiumData.payments;
        scope.futureBills = policyPremiumData.futureBills;
        scope.totalPaid = policyPremiumData.totalPaid;
        scope.remainingBalance = policyPremiumData.totalRemaining;
        scope.documents = policyPremiumData.documents;
        scope.policyCode = policyPremiumData.policyCode;
        scope.currentDue = policyPremiumData.currentDue;
        scope.amountReceived = policyPremiumData.amountReceived;
        scope.currentDueLabel = policyPremiumData.lable;

        scope.dynamicBalance = (selected == 1) ? policyPremiumData.currentDue : policyPremiumData.totalRemaining;

        //bindDatatable('paymentHistory', null, null);
        loadingService.hideLoader();

        function getDate(input, format) {
            if (angular.isUndefined(input))
                return;

            // first 6 character is the date
            var date = new Date(parseInt(input.substr(6)));

            // default date format
            var format = "MM/DD/YYYY";

            format = format.replace("DD", (date.getDate() < 10 ? '0' : '') + date.getDate()); // Pad with '0' if needed
            format = format.replace("MM", (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1)); // Months are zero-based
            format = format.replace("YYYY", date.getFullYear());

            return format;
        }

        scope.acceptTerms = function (acceptTermModel) {
            if (acceptTermModel && scope.amountReceived != undefined && scope.amountReceived > 0) {
                scope.paymentPlan.isPaymentDisabled = false;
            }
            else {
                scope.paymentPlan.isPaymentDisabled = true;
            }
        }

        scope.changeOption = function () {
            // scope.amountReceived.replace('$', '');            

            if (scope.paymentPlan.selectedOption == "1") {
                scope.paymentPlan.amountReceived = scope.currentDue;
            }
            else if (scope.paymentPlan.selectedOption == "2") {
                scope.paymentPlan.amountReceived = scope.remainingBalance;
            }      
            
            $location.path("/MakePayment/" + window.name.split("CYB")[0] + scope.paymentPlan.selectedOption);

        }

        var refresh = function () {
            $location.path("/MakePayment/" + window.name.split("CYB")[0] + scope.paymentPlan.selectedOption);
        };

        scope.$on("REFRESH", refresh); // re-initialize on signal

        $('.creditcardform').submit(function (e) {

            scope.paymentPlan.isValidYear = true;
            scope.paymentPlan.isEmptyExpiration = true;

            var ccMonth = this.ccMonth.value;
            var ccYear = this.ccYear.value;

            if (ccMonth != "" && ccMonth != undefined) {
                if (ccYear != "" && ccYear != undefined) {
                    this.x_exp_date.value = ccMonth + '/' + ccYear;
                    scope.paymentPlan.isEmptyExpiration = false;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }

            var minMonth = new Date().getMonth() + 1;
            var minYearFull = new Date().getFullYear();
            var minYear = parseInt(minYearFull.toString().substring(2, 4));
            var month = parseInt(ccMonth, 10);
            var year = parseInt(ccYear, 10);

            if (isNaN(parseInt(month)) || isNaN(parseInt(year))) {
                scope.paymentPlan.isValidYear = false;
            }
            else {
                scope.paymentPlan.isValidYear = (year > minYear || (year === minYear && month >= minMonth));
            }

            scope.$apply();

            if (!scope.paymentPlan.isValidYear || scope.paymentPlan.isEmptyExpiration) {
                return false;
            }
            else {
                return scope.paymentForm.$valid;
            }
        });

    }
})();
