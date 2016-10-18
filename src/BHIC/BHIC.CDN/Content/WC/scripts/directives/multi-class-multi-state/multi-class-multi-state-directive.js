(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('multiClassMultiState', ['quoteService', '$timeout', 'coreUtils', '$filter', 'appConfig', multiClassMultiStateFn]);
    function multiClassMultiStateFn(quoteService, $timeout, coreUtils, $filter, appConfig) {
        return {
            restrict: 'AE',
            templateUrl: appConfig.appCdnDomain + '/Content/WC/scripts/directives/multi-class-multi-state/multi-class-multi-state-template.html', //appConfig.appBaseDomain
            scope: {
                msmcdata: "=",
                totalpayrollamount: "=",
                submitdata: "&"
            },
            link: function (scope, element, attrs, ctrl) {
                scope.error_ = {};
                scope.appCdnDomain = appConfig.appCdnDomain;
                scope.payrollamount = scope.totalpayrollamount;
                var numberFilter = $filter('number');
                var fractionSize = parseInt(attrs['fractionSize']) || 0;
                var _exposurePayrollAmount = coreUtils.GetValidAmount(scope.totalpayrollamount);
                scope.$parent.quoteViewModel.minPayrollPercentageFactor = null;
                function _init() {
                    $timeout(function () {
                        scope.minExpValidationAmount = scope.$parent.quoteViewModel.minExpValidationAmount;
                        quoteService.getPrimaryClassCodeData(scope.msmcdata.primaryClassData.state, scope.msmcdata.primaryClassData.classDescId).then(function (resp) {
                            scope.$parent.quoteViewModel.minPayrollPercentageFactor = resp.primaryClassCodeData.MinimumPayrollThreshold;
                            scope.msmcdata.primaryClassData.description = coreUtils.IsEmptyString(resp.FriendlyName) ? scope.msmcdata.primaryClassData.description : resp.FriendlyName;
                            $('.tooltip').tooltip();
                        });
                        _.each(scope.msmcdata.companionCodeData, function (val) {
                            if (val.PayrollAmount) {
                                //scope.companionPayroll_[val.ClassCode] = val.PayrollAmount;
                                scope.payrollamount = coreUtils.GetValidAmount(scope.payrollamount) - coreUtils.GetValidAmount(val.PayrollAmount);
                                scope.payrollamount = numberFilter(scope.payrollamount, fractionSize);
                            }
                            else {
                                //scope.companionPayroll_[val.ClassCode] = '';
                                scope.error_[val.ClassCode] = false;
                            }
                        });
                        scope.$parent.quoteViewModel.updatedPayrollAmount = scope.payrollamount;
                    })
                    $('.tooltip').tooltip();
                }

                scope.companionPayroll_ = {};
                scope.processing = false;
                scope.updatePayrollAmounts = function (data) {
                    scope.processing = true;

                    scope.payrollamount = _exposurePayrollAmount;
                    var _payrollSum = 0;
                    $timeout(function () {
                        angular.forEach(scope.msmcdata.companionCodeData, function (item) {
                            scope.error_[item.ClassCode] = 0;

                            var val = coreUtils.GetValidAmount(item.PayrollAmount);
                            if (val) {
                                _payrollSum = _payrollSum + val;

                                if (val >= _exposurePayrollAmount) {
                                    //TODO Show validation Error
                                    item.PayrollAmount = '';
                                    scope.error_[data.ClassCode] = 1;
                                    scope.processing = false;
                                }
                                else if (_payrollSum >= _exposurePayrollAmount) {
                                    data.PayrollAmount = '';
                                    scope.error_[data.ClassCode] = 1;
                                    scope.processing = false;
                                }
                                else if ((coreUtils.GetValidAmount(scope.payrollamount) - val) < coreUtils.GetValidAmount(scope.$parent.quoteViewModel.minExpValidationAmount)) {
                                    data.PayrollAmount = '';
                                    scope.error_[data.ClassCode] = 2;
                                    scope.processing = false;
                                }
                                else {
                                    scope.payrollamount = coreUtils.GetValidAmount(scope.payrollamount) - val;
                                    scope.$parent.quoteViewModel.updatedPayrollAmount = scope.payrollamount;
                                    delete scope.error_[item.ClassCode];
                                    delete scope.error_[data.ClassCode];
                                    item.PayrollAmount = numberFilter(val, fractionSize);
                                    scope.processing = false;
                                }
                            }
                            else {
                                //scope.payrollamount = numberFilter(scope.payrollamount, fractionSize);
                                scope.$parent.quoteViewModel.updatedPayrollAmount = scope.payrollamount;
                            }

                        });
                        scope.payrollamount = numberFilter(scope.payrollamount, fractionSize);
                        scope.$parent.quoteViewModel.updatedPayrollAmount = scope.payrollamount;
                    });

                }
                scope.submitTotalData = function () {
                    var _invalid = false;
                    if (!coreUtils.IsEmptyObject(scope.error_)) {
                        _invalid = _.some(scope.error_), function (val) { return val != 0; };
                    }
                    if (!_invalid && coreUtils.GetValidAmount(scope.payrollamount) >= coreUtils.GetValidAmount(scope.$parent.quoteViewModel.minExpValidationAmount)) {
                        scope.submitdata();
                    }
                }
                scope.goToCapturePage = function () {
                    $timeout(function () {
                        scope.$parent.quoteViewModel.minPayrollPercentageFactor = null;
                        scope.payrollamount = _exposurePayrollAmount;
                        scope.$parent.quoteViewModel.showCompanionClass = false;
                    });
                }

                _init();

            }
        };
    }
})();


