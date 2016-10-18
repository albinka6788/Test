
(
    function () {
        'use strict';
        angular.module('bhic-app.wc.quoteResult', ['bhic-app.services', 'bhic-app.wc.controls'])
            .controller('quoteResultCtrl', ['$scope', 'bhic-app.wc.services.quoteResultData', quoteResultControllerFn]);

        function quoteResultControllerFn($scope, quoteResultDataProvider) {


            function _init() {
                _getQuoteResult();
            }

            $scope.quoteResult = {
                DsQuoteId: "",
                LowestDownPayment: "",
                Premium: "",
                CurrentDue: "",
                FutureInstallmentAmount: "",
                NoOfInstallment: "",
                InstallmentFee: "",
                PaymentPlanId: null,
                Frequency: "",
                FrequencyCode: ""
            };

            var _getQuoteResult = function () {

                var promise = quoteResultDataProvider.getQuoteResult();

                promise.then(function (response) {
                    if (response && response.data) {
                        var result = response.data;

                        $scope.quoteResult.DsQuoteId = result.quoteRefNo;
                        $scope.quoteResult.InstallmentFee = result.installmentFee;
                        $scope.quoteResult.PaymentOptions = result.paymentPlan;


                        //Comment : Here get and set Premium & InstallmentFee values
                        var quotePremium = result.premiumAmt;
                        $scope.quoteResult.Premium = '$' + quotePremium;
                        $scope.quoteResult.InstallmentFee = '$' + result.installmentFee;

                        //Comment : Here set PaymentPlan option list
                        var paymentPlans = result.paymentPlan;
                        $scope.quoteResult.LowestDownPayment = '$' + ((parseFloat(quotePremium) * paymentPlans[paymentPlans.length - 1].Down) / 100);
                        $scope.quoteResult.PaymentOptions = paymentPlans;

                        //Comment : Here set default Item in option list
                        $scope.selectedItem = paymentPlans[paymentPlans.length - 1];

                        //Comment : Here call this method to show related values of Defualt selected PaymentPlan
                        $scope.changeQuoteParams();
                    }
                });
            };

            var _submit = function () {
                var promise = quoteResultDataProvider.submitQuoteResult($scope.quoteResult);

                promise.then(function (data) {
                    //Comment : Here successfully submitted 
                    if (data.status == 'Success') {
                        window.location.assign('/Landing/WcHome/Purchase');
                    }
                });

            };

            $scope.changePlan = function ()
            {
                var quotePremium = $scope.quoteResult.Premium.replace('$', '');

                var installmentAmount = parseInt(quotePremium);
                var currentDue = ((installmentAmount * $scope.selectedItem.Down) / 100);
                $scope.quoteResult.CurrentDue = "$" + currentDue;

                //Comment : Here set paymentPlanId & frequency related models
                $scope.quoteResult.Frequency = '';
                $scope.quoteResult.FrequencyCode = $scope.selectedItem.Freq;
                $scope.quoteResult.PaymentPlanId = $scope.selectedItem.PaymentPlanId;

                if ($scope.selectedItem.Pays == 0) {
                    $scope.IsSinglePay = true;
                }
                else
                {
                    $scope.IsSinglePay = false;
                    var futureInstallment = 0;
                    futureInstallment = parseFloat(parseFloat(installmentAmount - parseFloat(currentDue)) / parseFloat($scope.selectedItem.Pays)).toFixed(2);
                    $scope.quoteResult.FutureInstallmentAmount = "$" + futureInstallment;
                    $scope.quoteResult.NoOfInstallment = $scope.selectedItem.Pays;
                }
            }

            angular.extend($scope, {
                submitQuoteResult: _submit,
            });

            _init();
        };
    }
)();