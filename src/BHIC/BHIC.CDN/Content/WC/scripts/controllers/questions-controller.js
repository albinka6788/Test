(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module("BHIC.WC.Controllers").controller('questionsController', ['$scope', '$filter', 'restServiceConsumer', 'questionsService', '$location', 'loadingService', '$timeout', 'coreUtils', 'appConfig', questionsControllerFn]);

        //controller function
        function questionsControllerFn(scope, filter, restServiceConsumer, questionsService, location, loadingService, timeout, coreUtilityMethod, appConfig) {
            var self = this;

            var _questions = [];
            var _taxIdNumber = null;
            var _taxIdType = null;
            var _listOfErrors = [];

            //Comment : Here to enable submit button & allow submission of question form responses
            var _acceptFlag;
            var _formSubmitted = false;
            var _hasValidForm = false;
            var _formSubmittedSaveForLater = false;
            var _saveForLaterProcessing = false;

            //Comment : Here increment seed value for "Pecentage" type questions
            var _PercentageSeedValue = 10;
            var _isFeinApplicable = false;
            var _userEmailId = '';

            function _init(QuesionsVm) {
                //debugger;

                //self.saveForLaterProcessing = false;
                loadingService.hideLoader();

                //Comment : Here must check for any user system errors if any
                var errorMessages = QuesionsVm.Messages;

                if (errorMessages.length == 0) {
                    _getQuestions(QuesionsVm.Questions);
                    _setFeinApplicability(QuesionsVm.FeinApplicable, QuesionsVm.TaxIdNumber, QuesionsVm.TaxIdType);
                    scope.selectedTaxIdType = QuesionsVm.TaxIdType;
                    scope.updateTaxIdOrSSN(QuesionsVm.TaxIdType);

                    if (QuesionsVm.TaxIdType == 'E')
                        scope.feinNumber = QuesionsVm.TaxIdNumber;
                    else
                        scope.ssnNumber = QuesionsVm.TaxIdNumber;
                }
                else {
                    //Comment : Here iterate errors list and display it to screen
                    self.listOfErrors = errorMessages;
                }

                if (QuesionsVm.TaxIdType == null || QuesionsVm.TaxIdType == "") {
                    scope.selectedTaxIdType = 'E';
                    scope.updateTaxIdOrSSN("E");
                }

                scope.NavLinks = [];
                scope.NavLinks = QuesionsVm.NavigationLinks;
                timeout(function () {
                    scope.quoteId = appConfig.quoteId;
                })
            };

            var _setFeinApplicability = function (isApplicable, taxIdNumber, taxIdType) {
                //debugger;

                //Comment : Here set FEIN applicability based on then only show this control
                if (isApplicable != null && isApplicable != 'null') {
                    self.isFeinApplicable = isApplicable;
                    //self.feinNumber = "510400307";

                    //Comment : Here set FEIN applicability based on then only show this control
                    if (taxIdNumber != null && taxIdNumber != 'null' && taxIdNumber != '') {
                        self.taxIdNumber = taxIdNumber;
                        self.taxIdType = taxIdType;
                        //scope.form_Questions.feinControlText.$setValidity("required", true);
                    }
                }

            };

            scope.showTaxId = true;
            scope.updateTaxIdOrSSN = function (senderType) {
                switch (senderType) {
                    case "E": scope.showTaxId = true;
                        break;
                    case "S": scope.showTaxId = false;
                        break;
                }
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

            scope.moreText = false;
            scope.displayText = "More about XMOD";
            scope.expandAndCollapse = function () {
                if (scope.moreText)
                {
                    scope.moreText = false;
                    scope.displayText = "More about XMOD";
                }
                else
                {
                    scope.moreText = true;
                    scope.displayText = "Less about XMOD";
                }
            }
            var _getQuestions = function (data) {
                //Comment : Here reset 
                self.questions = [];

                if (data != null && data != 'null' && data != '') {
                    self.questions = JSON.parse(JSON.stringify(data));
                }
            };

            var _showConditionalQuestion = function (thisQuestion, userResponse) {
                //debugger;
                //Comment : Here rather than iterating all question just get target question list based user responded question-id
                var filteredQuestions = filter('filter')(self.questions, { WhenQuestion: thisQuestion.questionId });

                angular.forEach(filteredQuestions, function (question) {
                    //Comment : Here get dependent questions list and make then visible/invisble
                    if (question.WhenQuestion == thisQuestion.questionId) {
                        //Comment : Here default set false
                        question.RenderFlag = false;

                        //Comment : Here based on WhenCondition like '=,>,<,!=' do operation
                        if (question.WhenCondition == '=') {
                            if (userResponse == question.WhenResponse)
                                question.RenderFlag = true;
                        }
                        else if (question.WhenCondition == '>') {
                            if (userResponse > question.WhenResponse)
                                question.RenderFlag = true;
                        }
                        else if (question.WhenCondition == '<') {
                            if (userResponse > question.WhenResponse)
                                question.RenderFlag = true;
                        }
                        else if (question.WhenCondition == '!=') {
                            if (userResponse != question.WhenResponse)
                                question.RenderFlag = true;
                        }

                        //Comment : Here add sub-qestion class for shown dependent questions
                        //var element = angular.element('#quesionNumber' + question.questionId);
                        //element.removeClass('question');
                        //element.addClass('sub-question');
                    }
                });
            };

            var _getConditionalQuestions = function (thisQuestion) {

                //Comment : Here rather than iterating all question just get target question list based user responded question-id
                var filteredQuestions = filter('filter')(self.questions, { WhenQuestion: thisQuestion.questionId });

                var retArray = [];
                angular.forEach(filteredQuestions, function (question) {
                    //Comment : Here get dependent questions list and make then visible/invisble
                    if (question.WhenQuestion == thisQuestion.questionId) {
                        retArray.push(question);
                    }
                });

                return retArray;
            }

            var _attemptedQuestions = [];

            var _submitQuestionsResponses = function (hasValidForm, questionListObject)
            {
                //debugger;
                var feinValidated = false;
                //reset error list
                self.listOfErrors = [];
                //console.log(scope.form_Questions.$valid);

                //Comment : here call service to validate FEIn number before submitting form data

                var taxId = "";
                var taxIdType = "";
                if (self.isFeinApplicable) {
                    if (scope.form_Questions.feinControlText) {
                        taxId = scope.form_Questions.feinControlText.$modelValue
                        taxIdType = "E";
                    }
                    else {
                        taxId = scope.form_Questions.ssnControlText.$modelValue
                        taxIdType = "S";
                    }
                    taxId = coreUtilityMethod.Trim(taxId);
                }

                if (taxId != undefined && taxId != '' && taxId.length > 0)
                {
                    loadingService.showLoader();
                    //GUIN-238 : true in following promise will denote if feinNumber/taxId value will be updated in session or not 
                    var promise = restServiceConsumer.isValidTaxIdNumber(taxId,true);
                    promise
                        .then(function (data) {
                            if (data != null) {
                                //Comment : Here if taxid-number not-valid then 
                                if (data.resultStatus == 'False') {
                                    //scope.form_Questions.feinControlText.$setValidity('validTaxIdNumber', false);
                                    feinValidated = false;
                                }
                                else {
                                    feinValidated = true;                                    
                                }
                            }
                        })
                        .catch(function (exception) {
                        })
                        .finally(function ()
                        {
                            if (feinValidated)
                            {
                                if (hasValidForm)
                                {
                                    _postQuestions(hasValidForm, questionListObject, taxId, taxIdType);
                                }
                                else
                                {
                                    loadingService.hideLoader();

                                    self.scrollFormOnError();
                                    //_showQuestionsFormErrorMsg();
                                }
                            }
                            else
                            {
                                loadingService.hideLoader();
                                //scope.form_Questions.feinControlText.$setValidity('validTaxIdNumber', false);

                                //Comment : Here check for all validation are successfully processed then proceed further
                                self.scrollFormOnError();
                                //_showQuestionsFormErrorMsg();
                            }
                        });
                }
                else if (taxId == '')
                {
                    //Comment : Here in case 
                    feinValidated = true;
                    _postQuestions(hasValidForm, questionListObject, taxId, taxIdType);
                }

            };

            function _showQuestionsFormErrorMsg() { alert('Please fill the complete questionnaire.') };

            function _postQuestions(hasValidForm, questionListObject, taxId, taxIdType)
            {
                if (hasValidForm && questionListObject.length > 0) {
                    //STEP - 1. Show loader
                    loadingService.showLoader();

                    //STEP - 2. In "Percentage" questions are not mandatory so set thier question.UserResponse = 0 by-default
                    angular.forEach(questionListObject, function (question) {
                        //Comment : Here get dependent questions list and make then visible/invisble
                        if (question.QuestionType == 'P' && (question.UserResponse == null || question.UserResponse == undefined)) {
                            question.UserResponse = 0;
                        }
                    });

                    //Comment : here call service to submit questions response                    
                    var promise = questionsService.submitQuestionsResponse(questionListObject, taxId, taxIdType);

                    promise
                        .then(function (data) {
                            //alert(data.resultText);

                            if (data.resultStatus == 'OK') {
                                // ***** questions ok: successful quote - user is presented with a payment information page
                                if (data.resultText == 'Quote') {
                                    // return the quote results
                                    location.path("/QuoteSummary/");
                                }
                                    // ***** questions result in SOFT REFERRAL
                                else if (data.resultText == 'Soft Referral') {
                                    location.path("/ReferralQuote/");
                                }
                                    // ***** questions result in HARD REFERRAL OR DECLINE
                                else if (data.resultText == 'Hard Referral') {
                                    location.path("/DeclinedQuote/");
                                }
                            }
                            else if (data.resultStatus == 'NOK') {
                                //Comment : Here must check for any user system errors if any
                                if (data.resultMessages != null && data.resultMessages != undefined) {
                                    var errorMessages = data.resultMessages;

                                    if (errorMessages != undefined && errorMessages != null && errorMessages.length == 0) {
                                        location.path("/AppError/");
                                    }
                                    else {
                                        //Comment : Here iterate errors list and display it to screen
                                        self.listOfErrors = errorMessages;
                                    }
                                }
                            }
                            else {
                                //console.log(data);
                                // Change by Guru on 16-Dec-2015 at 22:14
                                //alert(data);
                            }
                        })
                        .catch(function (exception) {
                            //console.log(exception.toUpperCase());
                            location.path("/AppError/");
                        })
                        .finally(function () {
                            self.attemptedQuestions = [];
                            loadingService.hideLoader();
                            //console.log("finally finished questionnaire generation");
                        });
                }
                else {
                    //Comment : Here iterate errors list and display it to screen
                    self.scrollFormOnError();
                    //_showQuestionsFormErrorMsg();
                }
            }

            var _manipulatePercentageControls = function (question, inputValue, minValue, maxValue) {
                scope.form_Questions.$dirty = true;
                var intVal = parseInt(inputValue);

                //Comment : Here get "Percentage" input control value
                var element = angular.element('#questionTypePercentage' + question.questionId);

                var userResponse = parseInt(element.val() || 0) + (intVal * _PercentageSeedValue);

                //Comment : Here if userResponse is in between Permissable range then allow
                if (userResponse >= minValue && userResponse <= maxValue) {
                    element.val(userResponse);
                    question.UserResponse = userResponse;

                    //comment : Here call common functions to take specified action
                    self.showConditionalQuestion(question, userResponse);
                }
                else if (userResponse > maxValue) {
                    element.val(maxValue);
                    question.UserResponse = maxValue;
                }
                else if (userResponse <= minValue) {
                    element.val(minValue);
                    question.UserResponse = minValue;
                }
            }

            var _convertToNumeric = function (input) {
                return parseInt(input);
            };

            var _validateFeinNumber = function (input) {
                //debugger;
                if (input != null) {
                    scope.form_Questions.feinControlText.$setValidity("minLength", self.feinNumber >= 10);
                }
            };

            var _saveForLater = function (controllercontext) {
                //debugger;  
                controllercontext.formSubmitted = true;
                //reset error list
                self.listOfErrors = [];

                if (self.isFeinApplicable) {
                    if (scope.form_Questions.feinControlText) {
                        self.taxIdNumber = scope.form_Questions.feinControlText.$modelValue
                        self.taxIdType = "E";
                    }
                    else {
                        self.taxIdNumber = scope.form_Questions.ssnControlText.$modelValue
                        self.taxIdType = "S";
                    }
                    self.taxIdNumber = coreUtilityMethod.Trim(self.taxIdNumber);
                }

                //Comment : Here check for all validation are successfully processed then proceed further
                if (controllercontext.hasValidForm) {
                    //Comment : Here OPEN remodal pop-up
                    coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);

                    ////Comment : Here if form has been validated then show model POP-UP for further information
                    //var modelBox = $('[data-remodal-id=ModelSaveForLater]').remodal();

                    ////if not undefined
                    //if (modelBox) {
                    //    modelBox.open();
                    //    $('.remodal-overlay').removeClass("overlay-light");
                    //}
                }
                else {
                    self.scrollFormOnError();
                    //alert('Please fill the complete questionnaire.');
                    //_showQuestionsFormErrorMsg();
                }                    
            }

            var _submitSaveForLater = function (userEmailId)
            {
                //debugger;
                self.formSubmittedSaveForLater = true;
                //reset error list
                self.listOfErrors = [];

                //self.saveForLaterProcessing = true;
                var feinNumber = self.taxIdNumber;
                self.taxIdNumber = coreUtilityMethod.ConvertToNumeric(coreUtilityMethod.Trim(feinNumber));

                //Comment : Here form is validate then check is EmailId entered then post this data and send user link ro revoke this quote
                if (userEmailId != null && userEmailId != '')
                {
                    //Comment : Here show app data processing loader
                    loadingService.showLoader();

                    //Comment : here call service to submit questions response                    
                    var promise = questionsService.submitSaveForLater(self.questions, self.taxIdNumber, self.taxIdType, userEmailId);

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
                            loadingService.hideLoader();
                        });
                }
            }

            var _submitSaveForLaterOLD = function (hasValidForm, controllercontext)
            {
                //debugger;
                self.formSubmittedSaveForLater = true;
                //reset error list
                self.listOfErrors = [];

                //Comment : Here check for all validation are successfully processed then proceed further
                if (hasValidForm)
                {
                    //self.saveForLaterProcessing = true;
                    var feinNumber = controllercontext.taxIdNumber;
                    controllercontext.taxIdNumber = coreUtilityMethod.ConvertToNumeric(coreUtilityMethod.Trim(feinNumber));

                    //Comment : Here form is validate then check is EmailId entered then post this data and send user link ro revoke this quote
                    if (controllercontext.userEmailId != null && controllercontext.userEmailId != '')
                    {
                        //Comment : Here show app data processing loader
                        loadingService.showLoader();

                        //Comment : here call service to submit questions response                    
                        var promise = questionsService.submitSaveForLater(controllercontext.questions, controllercontext.taxIdNumber, controllercontext.taxIdType, controllercontext.userEmailId);

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
                                setTimeout(function ()
                                {
                                    coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                                }, 1500);
                            })
                            .catch(function (exception) {
                                //console.log(exception.toUpperCase());
                            })
                            .finally(function ()
                            {
                                //self.saveForLaterProcessing = false;
                                //return;
                                loadingService.hideLoader();
                            });                        
                    }
                }
            }

            var _closeSaveForLaterModel = function (modelName)
            {
                //Comment : Here CLOSE remodal pop-up
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=' + modelName + ']'), false);

                //Comment : Here only when ModelSaveForLaterThankYou is closed
                if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou')
                {
                    loadingService.showLoader();
                }

                //reset to default
                //if (self.userEmailId != '')
                //    self.userEmailId = '';

                //Comment : Here if form has been validated then show model POP-UP for further information
                //var modelBox = $('[data-remodal-id=' + modelName + ']').remodal();
                //modelBox.close();

                ////Comment : Here only when ModelSaveForLaterThankYou is closed
                //if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou')
                //{
                //    loadingService.showLoader();
                //}

                //Comment : Here explicitly navigate user to application HOME page after SaveForLater submission only when ThankYou model is closed
                /*if (modelName != null && modelName != '' && modelName == 'ModelSaveForLaterThankYou')
                {
                    location.path("/Home");
                }*/
            }

            var _scrollFormOnError = function ()
            {
                //Comment : Here iterate errors list and display it to screen
                self.listOfErrors = ['Please fill the complete questionnaire.'];
                coreUtilityMethod.ScrollToTop();
                //$(window).scrollTop(0);
            }

            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(self, {
                init: _init,
                questions: _questions,
                showConditionalQuestion: _showConditionalQuestion,
                manipulatePercentageControls: _manipulatePercentageControls,
                acceptFlag: _acceptFlag,
                formSubmitted: _formSubmitted,
                hasValidForm: _hasValidForm,
                convertToNumeric: _convertToNumeric,
                submitQuestionnaireForm: _submitQuestionsResponses,
                saveForLater: _saveForLater,
                submitSaveForLater: _submitSaveForLater,
                isFeinApplicable: _isFeinApplicable,
                taxIdType: _taxIdType,
                taxIdNumber: _taxIdNumber,
                userEmailId: _userEmailId,
                formSubmittedSaveForLater: _formSubmittedSaveForLater,
                //saveForLaterProcessing: _saveForLaterProcessing,
                closeSaveForLaterModel: _closeSaveForLaterModel,
                validateFeinNumber: _validateFeinNumber,
                listOfErrors: _listOfErrors,
                scrollFormOnError: _scrollFormOnError
            });

        };
    }
)();