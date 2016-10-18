(
function () {

    'use strict';

    //module controller declaration and injection
    angular.module("BHIC.WC.Controllers")
        .controller('buyPolicyController', ['$scope', 'policyPaymentService', 'navigationService', '$filter', '$location', 'loadingService', 'coreUtils', 'appConfig', buyPolicyControllerFn]);

    //controller function
    function buyPolicyControllerFn(scope, policyPaymentDataProvider, navigationService, filter, location, loadingService, coreUtils, appConfig) {

        // Variable declaration
        scope.paymentPlan = {
            product: '',
            premiumAmt: '',
            paymentOptions: '',
            currentDue: '',
            futureInstallmentAmount: '',
            noOfInstallments: '',
            installmentFee: '',
            totalDue: '',
            isPaymentDisabled: true,
            isValidYear: '',
            isEmptyExpiration: '',
            totalAmount: '',
            userEmailId: ''
        };

        scope.init = function (buyPolicyVM) {

            loadingService.hideLoader();

            //assign user email id to appConfig variable,
            //This email id is provided by user in user info page
            appConfig.prospectInfoEmailId = buyPolicyVM.UserEmailId;
            //appConfig.loggedUserEmailId = buyPolicyVM.UserEmailId; //Old Line

            scope.getPlans(buyPolicyVM);
        }

        //Comment : Here to show form level errors
        scope.listOfErrors = [];

        // Progress bar navigation
        scope.goTo = function (sender) {
            if ($(sender.target).attr("url") != "#") {
                coreUtils.setNavigation($(sender.target).attr("url"), true);

                location.path($(sender.target).attr("url"));
            }
        }

        scope.getPlans = function (buyPolicyVM) {

            if (buyPolicyVM != undefined && buyPolicyVM != null) {

                //clear all fields initially
                scope.resetModel();

                var paymentPlan = buyPolicyVM.PaymentOptions

                scope.paymentPlan.product = buyPolicyVM.ProductName;
                scope.paymentPlan.paymentOptions = paymentPlan;
                scope.paymentPlan.NavLinks = buyPolicyVM.NavLinks;

                //set last item as selected
                // scope.selectedItem = paymentPlan[paymentPlan.length - 1];

                if (paymentPlan != null && paymentPlan != undefined) {
                    //Comment : Here get previuosly selected paymentPlan from this dat to show auto-selected dropdwon
                    var filteredPlan = filter('filter')(paymentPlan, { PaymentPlanId: buyPolicyVM.PaymentTerms.PaymentPlanId });

                    //set last item as selected
                    scope.selectedItem = filteredPlan[0];
                }

                //get and set Premium & InstallmentFee values
                scope.paymentPlan.premiumAmt = parseFloat(buyPolicyVM.PremiumAmt).toFixed(2);
                scope.paymentPlan.installmentFee = parseFloat(buyPolicyVM.InstallmentFee).toFixed(2);
                scope.paymentPlan.totalAmount = parseFloat(parseFloat(buyPolicyVM.PremiumAmt) + (parseInt(buyPolicyVM.PaymentTerms.Installments) * parseFloat(buyPolicyVM.InstallmentFee))).toFixed(2);

                //initialize values on load
                calculatePreminum();
            }
        }

        function calculatePreminum() {

            //Clear fields
            scope.paymentPlan.currentDue = '';
            scope.paymentPlan.totalDue = '';
            scope.paymentPlan.futureInstallmentAmount = '';
            scope.paymentPlan.noOfInstallments = '';

            var premium = scope.paymentPlan.premiumAmt.replace('$', '');

            var installmentAmount = parseInt(premium);
            var currentDue = ((installmentAmount * scope.selectedItem.Down) / 100).toFixed(2);
            scope.paymentPlan.currentDue = currentDue;

            var installmentFee = scope.paymentPlan.installmentFee.replace('$', '');

            var totalDue = (parseFloat(currentDue) + parseFloat(installmentFee)).toFixed(2);
            scope.paymentPlan.totalDue = totalDue;

            if (scope.selectedItem.Pays == 0) {
                scope.IsSinglePay = true;
            }
            else {
                scope.IsSinglePay = false;
                var futureInstallment = 0;
                futureInstallment = parseFloat(parseFloat(installmentAmount - parseFloat(currentDue)) / parseFloat(scope.selectedItem.Pays)).toFixed(2);
                scope.paymentPlan.futureInstallmentAmount = futureInstallment;
                scope.paymentPlan.noOfInstallments = scope.selectedItem.Pays;
            }
        }

        //Change plan based on select item
        scope.changePlan = function () {
            loadingService.showLoader();

            calculatePreminum();

            //get payment plans
            var promise = policyPaymentDataProvider.updatePaymentPlans(scope.selectedItem.PaymentPlanId);

            promise
                  .then(function (result) {
                      if (result.isSuccess) {
                          scope.$broadcast("REFRESH");
                      }
                      else {
                          //Comment : Here must check for any user system errors if any
                          var errorMessages = result.resultMessages;

                          if (result.isSessionExpired) {
                              location.path("/SessionExpired/");
                          }
                          else if (errorMessages != undefined && errorMessages != null && errorMessages.length > 0) {
                              scope.listOfErrors = errorMessages;
                          }
                          else {
                              location.path("/AppError/");
                          }
                      }
                  })
                  .catch(function (exception) {
                      loadingService.hideLoader();
                      //console.log(exception.toUpperCase());
                      location.path("/AppError/");
                  })
                  .finally(function () {

                      loadingService.hideLoader();
                  });
        }

        scope.resetModel = function () {
            scope.paymentPlan.product = '';
            scope.paymentPlan.paymentOptions = '';
            scope.paymentPlan.premiumAmt = '';
            scope.paymentPlan.installmentFee = '';
        }

        scope.redirectToSummaryPage = function () {
            location.path("/OrderSummary/");
        }

        scope.acceptTerms = function (acceptTermModel) {
            if (acceptTermModel) {
                scope.paymentPlan.isPaymentDisabled = false;
            }
            else {
                scope.paymentPlan.isPaymentDisabled = true;
            }
        }

        $('.creditcardform').submit(function (e) {

            scope.paymentPlan.isValidYear = true;
            scope.paymentPlan.isEmptyExpiration = true;

            var ccMonth = this.ccMonth.value;
            var ccYear = this.ccYear.value;

            if (ccMonth != "" && ccMonth != undefined && ccYear != "" && ccYear != undefined) {
                this.x_exp_date.value = ccMonth + '/' + ccYear;
                scope.paymentPlan.isEmptyExpiration = false;
            }
            else {
                return false;
            }

            var month = parseInt(ccMonth, 10);
            var year = parseInt(ccYear, 10);

            if (isNaN(parseInt(month)) || isNaN(parseInt(year))) {
                scope.paymentPlan.isValidYear = false;
            }
            else {
                var minMonth = new Date().getMonth() + 1;
                var minYearFull = new Date().getFullYear();
                var minYear = parseInt(minYearFull.toString().substring(2, 4));

                scope.paymentPlan.isValidYear = (year > minYear || (year === minYear && month >= minMonth));
            }

            if (!scope.paymentPlan.isValidYear) {
                return false;
            }

            scope.$apply();

            //e.preventDefault();
            return scope.paymentForm.$valid;
        });

        var refresh = function () {
            location.path("/BuyPolicy/");
        };

        scope.$on("REFRESH", refresh); // re-initialize on signal

        /* end of payment form custom validation */
    };
}
)();