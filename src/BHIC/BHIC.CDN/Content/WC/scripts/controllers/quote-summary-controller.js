(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module("BHIC.WC.Controllers").controller('quoteSummaryController', ['$scope', 'quoteSummaryService', 'navigationService', '$location', 'loadingService', 'coreUtils', quoteSummaryControllerFn]);

        //controller function
        function quoteSummaryControllerFn(scope, quoteSummaryService, navigationService, location, loadingService, coreUtilityMethod) {
            var self = this;

            var _quoteSummaryVm =
            {
                quoteReferenceNo: 0,
                lowestInstallmentPremium: 0,
                totalEstimatedPremim: 0,
                xModValue: 0.0,
                dueNow: 0,
                futureInstallmentAmount: 0,
                noOfInstallment: 0,
                installmentFee: 0,
                selectedPaymentOption: {},
                mandatoryDeductibleApplicable: false
            };

            var _paymentPlans = [];
            var _isSinglePay = false;
            var _acceptFlag = false;
            var _hasValidForm = false;
            var _userEmailId = '';
            var _formSubmittedSaveForLater = false;
            var _listOfErrors = [];

            function _init(quoteSummaryVm) {

                //add complete class on quote menu if user comes directly to purchaseQuote
                //navigationService.enableNavigation('lnkQuote');

                //add complete class on quote-summary menu if user comes directly to purchaseQuote
                //navigationService.enableNavigation('lnkQuoteSummary');

                //Comment : Here by-default set it to true
                self.acceptFlag = true;

                loadingService.hideLoader();
                _getPaymentPlans(quoteSummaryVm.PaymentPlans);
                _setQuoteVm(quoteSummaryVm);
                scope.Navlinks = quoteSummaryVm.NavigationLinks;
                //Comment : Here user saved state is revoked then select previously selected payment plan
                //debugger;
                var defaultSelectedPlan = quoteSummaryVm.SelectedPaymentPlan;
                if (defaultSelectedPlan != null && defaultSelectedPlan.PaymentPlanId > 0) {
                    //only if selected payment plan is greater than 0
                    //self.quoteSummaryVm.selectedPaymentPlan = quoteSummaryVm.SelectedPaymentPlan;

                    //get the index
                    var index = self.paymentPlans.map(function (defaultSelectedPlan) {
                        return defaultSelectedPlan.PaymentPlanId;
                    }).indexOf(defaultSelectedPlan.PaymentPlanId);

                    self.quoteSummaryVm.selectedPaymentPlan = self.paymentPlans[index];
                }
                _changePaymentPlan();

                scope.xmodeText = scope.getText(quoteSummaryVm.XModValue);

                //Coverage Details 
                scope.stateName = quoteSummaryVm.stateName;
                scope.employeeLimitText = quoteSummaryVm.employeeLimitText;
                scope.employeeLimitValue = quoteSummaryVm.employeeLimitValue;
                scope.deductibleList = scope.getDisplayText(quoteSummaryVm);
                scope.btnText = quoteSummaryVm.btnText;

            };

            scope.getText = function (xModValue) {
                var val = (xModValue - 1) * 100;
                var txt = '';
                if (parseInt(val) < 0)
                    txt = 'This premium includes an adjustment based on your claims history (your Experience Modification Factor).  Your premium went down by ' + Math.abs(val.toPrecision(2)) + '% based on your past claims.';
                else if (parseInt(val) > 0)
                    txt = 'This premium includes an adjustment based on your claims history (your Experience Modification Factor).  Your premium went up by ' + val.toPrecision(2) + '% based on your past claims.';

                return txt;
            }
          

            //Coverage Details Method for Set DisplayText in dropdownlist
            scope.getDisplayText = function (quoteSummaryVm) {
                $.each(quoteSummaryVm.Deductibles, function (i, o) {
                    o.DisplayText = (o.Names == "None") ? "None" : o.Names.split("-")[0] + " - $" + o.DeductAmt;

                    if (quoteSummaryVm.selectedDeductible.Vals != null && o.Vals == quoteSummaryVm.selectedDeductible.Vals) {
                        scope.defaultSelectedDeductible = i;
                        scope.btnText = "Recalculate Quote";
                    }

                })
                return quoteSummaryVm.Deductibles;
            }

            scope.continueToCalculate = function (obj) {
                scope.btnText = "Recalculate Quote";
                self.quoteSummaryVm.selectedDeductible = (obj == null) ? null : obj;
            };


            // Progress bar navigation
           scope.goTo = function (sender) 
            {
                if ($(sender.target).attr("url") != "#")
                {
                    coreUtilityMethod.setNavigation($(sender.target).attr("url"), true);

                    //navigation = 
                    //{
                    //    path: $(sender.target).attr("url") || '',
                    //    status: true
                    //};

                    location.path($(sender.target).attr("url"));
                }
            }



            var _getPaymentPlans = function (data) {
    //Comment : Here reset 
                self.paymentPlans = [];
                if (data != null && data != 'null' && data != '') {
                    self.paymentPlans = JSON.parse(JSON.stringify(data));
                }
            };

            var _setQuoteVm = function (data) {
                //debugger;
                //Comment : Here reset 
                self.quoteSummaryVm = {};

                if (data != null && data != 'null' && data != '') {
                    //Comment : Here get 
                    var dataVM = JSON.parse(JSON.stringify(data));
                    var paymentPlans = self.paymentPlans;

                    self.quoteSummaryVm =
                    {
                        quoteReferenceNo: dataVM.QuoteReferenceNo,
                        lowestInstallmentPremium: dataVM.LowestInstallmentPremium,
                        totalEstimatedPremim: dataVM.PremiumAmt,
                        xModValue: dataVM.XModValue,
                        dueNow: 0,
                        futureInstallmentAmount: 0,
                        noOfInstallment: 0,
                        installmentFee: dataVM.InstallmentFee,
                        selectedPaymentPlan: paymentPlans[paymentPlans.length - 1],
                        mandatoryDeductibleApplicable: dataVM.HasMandatoryDeductible
                    };
                }
            };

            var _changePaymentPlan = function () {
                //debugger;

                //Comment : Here TotalEstimatedPremim for this Quote
                var totalPremium = self.quoteSummaryVm.totalEstimatedPremim;
                var selectedPaymentPlan = self.quoteSummaryVm.selectedPaymentPlan;

                //Comment : Here calculate due now
                var dueNow = ((parseFloat(totalPremium) * selectedPaymentPlan.Down) / 100);

                //set core VM
                self.quoteSummaryVm.dueNow = dueNow,

                //Comment : Here look for SinglePay payment plan option
                self.isSinglePay = false;
                if (selectedPaymentPlan.Pays == 0) {
                    self.isSinglePay = true;
                    return;
                }

                //Comment : Here calculate remaining premium amount
                var remainingPremiumAmt = parseFloat(totalPremium) - parseFloat(dueNow);

                //Comment : Here get remaining future no of installement
                var noOfInstallment = selectedPaymentPlan.Pays;

                //Comment : Here calculate remaining future installement amount
                var futureInstallmentAmount = parseFloat(parseFloat(remainingPremiumAmt) / parseFloat(noOfInstallment)).toFixed(2);

                //Comment : Here finally set it into core VM                
                self.quoteSummaryVm.futureInstallmentAmount = futureInstallmentAmount,
                self.quoteSummaryVm.noOfInstallment = noOfInstallment;
            }

            var _submitQuotePaymentPlan = function (selectedPaymentPlan) {
                //debugger;
                loadingService.showLoader();
                //Comment : here call service to submit questions response                    
                var promise = quoteSummaryService.submitQuotePaymentPlan(selectedPaymentPlan);

                promise
                    .then(function (data) {
                        //alert(data.resultText);

                        if (data.resultStatus == 'OK') {


                            location.path("/PurchaseQuote/");
                        }
                        else if (data.resultStatus == 'NOK') {
                            //Comment : Here must check for any user system errors if any
                            var errorMessages = data.resultMessages;

                            if (errorMessages != undefined && errorMessages != null && errorMessages.length == 0) {
                                location.path("/AppError/");
                            }
                            else {
                                //Comment : Here iterate errors list and display it to screen
                                self.listOfErrors = errorMessages;
                            }
                        }
                    })
                    .catch(function (exception) {
                        //console.log(exception));
                        location.path("/AppError/");
                    })
                    .finally(function () {
                        loadingService.hideLoader();
                        //console.log("finally finished quote payment plan submission.");
                    });

                //alert('Submitted');
                self.attemptedQuestions = [];
            };

            var _submitReCalculateQuote = function (selectedWCDeductible) {
                //debugger;
                loadingService.showLoader();
                //Comment : here call service to submit questions response                    
                var promise = quoteSummaryService.submitReCalculateQuote(selectedWCDeductible);

                promise
                    .then(function (data) {
                        //alert(data.resultText);

                        if (data.resultStatus == 'OK') {
                            location.path("/QuoteSummary/");
                        }
                        else if (data.resultStatus == 'NOK') {
                            //Comment : Here must check for any user system errors if any
                            var errorMessages = data.resultMessages;

                            if (errorMessages != undefined && errorMessages != null && errorMessages.length == 0) {
                                location.path("/AppError/");
                            }
                            else {
                                //Comment : Here iterate errors list and display it to screen
                                self.listOfErrors = errorMessages;
                            }
                        }
                    })
                    .catch(function (exception) {
                        //console.log(exception));
                        location.path("/AppError/");
                    })
                    .finally(function () {
                        loadingService.hideLoader();
                        //console.log("finally finished quote payment plan submission.");
                    });

                //alert('Submitted');
                self.attemptedQuestions = [];
            };

            var _saveForLater = function (controllercontext) {
                //Comment : Here if form has been validated then show model POP-UP for further information
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);
            }

            var _submitSaveForLater = function (userEmailId)
            {
                //debugger;
                self.formSubmittedSaveForLater = true;

                //Comment : Here form is validate then check is EmailId entered then post this data and send user link ro revoke this quote
                if (userEmailId != null && userEmailId != '')
                {
                    //Comment : here call service to submit questions response                    
                    var promise = quoteSummaryService.submitSaveForLater(self.quoteSummaryVm.selectedPaymentPlan, self.quoteSummaryVm.selectedDeductible ,  userEmailId);

                    promise
                        .then(function (data) {
                            if (data.resultStatus == 'OK') {
                                //console.log(data.resultStatus);

                                //Comment : Here if form has been validated then show model POP-UP for further information
                                //alert('data posted');                                                                    
                            }
                            else if (data.resultStatus == 'NOK') {
                                //console.log('Could not able to submit !');
                            }
                            else if (data.resultStatus === undefined) {
                                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), false);
                            }
                            //Comment : Here do not wait for data submission because it will create a long delay in displaying thank you message
                            setTimeout(function () {
                                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                            }, 1500);
                        })
                        .catch(function (exception) {
                            //console.log(exception.toUpperCase());
                        })
                        .finally(function () {
                            //self.saveForLaterProcessing = false;
                            //return;
                        });
                }
            }

            var _submitSaveForLaterOLD = function (hasValidForm, controllercontext) {
                //debugger;
                self.formSubmittedSaveForLater = true;

                //Comment : Here check for all validation are successfully processed then proceed further
                if (hasValidForm) {
                    //Comment : Here form is validate then check is EmailId entered then post this data and send user link ro revoke this quote
                    if (controllercontext.userEmailId != null && controllercontext.userEmailId != '') {
                        //Comment : here call service to submit questions response                    
                        var promise = quoteSummaryService.submitSaveForLater(controllercontext.quoteSummaryVm.selectedPaymentPlan, controllercontext.userEmailId);

                        promise
                            .then(function (data) {
                                if (data.resultStatus == 'OK') {
                                    //console.log(data.resultStatus);

                                    //Comment : Here if form has been validated then show model POP-UP for further information
                                    //alert('data posted');                                                                    
                                }
                                else if (data.resultStatus == 'NOK') {
                                    //console.log('Could not able to submit !');
                                }

                                //Comment : Here do not wait for data submission because it will create a long delay in displaying thank you message
                                setTimeout(function () {
                                    coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                                }, 1500);
                            })
                            .catch(function (exception) {
                                //console.log(exception.toUpperCase());
                            })
                            .finally(function () {
                                //self.saveForLaterProcessing = false;
                                //return;
                            });
                    }
                }
            }

            var _closeSaveForLaterModel = function (modelName) {
                //Comment : Here CLOSE remodal pop-up
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=' + modelName + ']'), false);

                //Comment : Here only when ModelSaveForLaterThankYou is closed
                if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou') {
                    loadingService.showLoader();
                }

                ////Comment : Here if form has been validated then show model POP-UP for further information
                //var modelBox = $('[data-remodal-id=' + modelName + ']').remodal();
                //modelBox.close();

                ////Comment : Here only when ModelSaveForLaterThankYou is closed
                //if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou') {
                //    loadingService.showLoader();
                //}
            }

            // Action Perform based on Button Text
            scope.PerformAction = function () {
                if (scope.btnText == "Continue")
                    _submitQuotePaymentPlan(self.quoteSummaryVm.selectedPaymentPlan, self.quoteSummaryVm.selectedDeductible);
                else
                    _submitReCalculateQuote(self.quoteSummaryVm.selectedDeductible);
            }

            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(self, {
                init: _init,
                quoteSummaryVm: _quoteSummaryVm,
                paymentPlans: _paymentPlans,
                changePaymentPlan: _changePaymentPlan,
                isSinglePay: _isSinglePay,
                submitQuotePaymentPlan: _submitQuotePaymentPlan,
                submitReCalculateQuote: _submitReCalculateQuote,
                saveForLater: _saveForLater,
                submitSaveForLater: _submitSaveForLater,
                hasValidForm: _hasValidForm,
                acceptFlag: _acceptFlag,
                userEmailId: _userEmailId,
                formSubmittedSaveForLater: _formSubmittedSaveForLater,
                closeSaveForLaterModel: _closeSaveForLaterModel,
                listOfErrors: _listOfErrors
            });

        };
    }
)();