(
    function () {
        'use strict';

        function questionsController(scope, http, filter, restServiceConsumer, questionsPageDataProvider)
        {

            var self = this;

            var _questions = [];

            //Comment : Here to enable submit button & allow submission of question form responses
            var _acceptFlag = false;
            var _formSubmitted = false;

            //Comment : Here to show processing request loader message
            var _showLoader = false;
            var _showNoDataFound = false;

            function _init() {                
                _getQuestions();
            };

            var _emailModel = {
                email: '',
            };


            //Comment : Here redirect user to "SaveForProcess" process flow
            var _redirectToSaveForLater = function () {
                window.location.assign('/Landing/WcHome/SaveForLater');
            };

            var _saveForLaterSubmit = function (emailModel) {
                questionsPageDataProvider.submitSaveForLaterResponse(emailModel).
                then(function (response) {
                    window.location.href = '/Landing/WcHome/SavedForLater';
                });
            };

            var _getQuestions = function () {
                //landingPageDataProvider.getQuestions().
                //then(function (data) {
                //    self.questions = data;
                //});

                //http.get('/Scripts/bhic/contents/wc/questions.json').success(function (responseData) {
                //    //alert(JSON.stringify(responseData));
                //    console.log(responseData);
                //    self.questions = responseData;
                //});

                //restServiceConsumer.getData('/Scripts/bhic/contents/wc/questions.json').then(function (responseData) {
                //    console.log(responseData);
                //    self.questions = responseData;
                //});

                scope.showLoader = true;
                self.showNoDataFound = false;
                //alert(scope.showLoader);

                var promise = questionsPageDataProvider.getQuestions();

                promise.then(function (data) {

                    //Comment : Here reset 
                    self.questions = [];

                    if (data != null && data != 'null' && data != '') {
                        self.questions = JSON.parse(data);
                    }
                    else {
                        self.showNoDataFound = true;
                    }

                    scope.showLoader = false;
                    //alert(scope.showLoader);
                });
            };

            var _showConditionalQuestion = function (thisQuestion, userResponse) {
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
                    }
                });
            };

            var _attemptedQuestions = [];
            var _saveQuestionsResponse = function (hasValidForm, questionListObject) {

                //Comment : Here check for all validation are successfully processed then proceed further
                if (hasValidForm) {
                    angular.forEach(questionListObject, function (question) {
                        //Comment : Here get dependent questions list and make then visible/invisble
                        if (question.DecisionEngineResponsesValid == 'Y') {
                            self.attemptedQuestions.push({ qId: question.questionId, qResponse: question.UserResponse });
                        }
                    });

                    //Comment : here call service to submit questions response                    
                    var promise = questionsPageDataProvider.submitQuestions(questionListObject);

                    promise.then(function (data) {
                        //alert(data.resultText);

                        // ----------------------------------------
                        // return the view required by the decision / results above
                        // ----------------------------------------
                        if (data == null)
                            window.location.assign('/Landing/WcHome/Index');

                        if (data.resultStatus == 'OK')
                        {
                            // ***** questions ok: successful quote - user is presented with a payment information page
                            if (data.resultText == 'Quote') {
                                // return the quote results
                                window.location.assign('/Landing/WcHome/ResultsQuoteGet');
                            }

                                // ***** questions result in SOFT REFERRAL
                            else if (data.resultText == 'Soft Referral') {
                                window.location.assign('/Landing/WcHome/ResultsReferQGet');
                            }

                                // ***** questions result in HARD REFERRAL
                            else if (data.resultText == 'Hard Referral') {
                                window.location.assign('/Landing/WcHome/ResultsReferQGet');
                            }
                        }
                        else {
                            window.location.assign('/Landing/WcHome/Error');
                        }
                        
                    });

                    //alert('Submitted');
                    self.attemptedQuestions = [];
                }
                else
                    alert('Please correct all shown errors.');
            };

            var _addDefaultItem = function (data) {
                var c = data.length + 1;
                var item = {
                    "name": "--Select--"
                };
                return data.splice(0, 0, item);
            };

            var _convertToNumeric = function (input)
            {
                return parseInt(input);
            };


            angular.extend(scope, {
                showLoader: _showLoader
            });

            angular.extend(self, {
                init: _init(),
                questions: _questions,
                showConditionalQuestion: _showConditionalQuestion,
                acceptFlag: _acceptFlag,
                formSubmitted: _formSubmitted,
                submitQuestionsResponse: _saveQuestionsResponse,
                attemptedQuestions: _attemptedQuestions,
                redirectToSaveForLater:_redirectToSaveForLater,
                emailModel: _emailModel,
                saveForLaterSubmit: _saveForLaterSubmit,
                showNoDataFound: _showNoDataFound,
                convertToNumeric: _convertToNumeric
            });            
        }

        /**
           * @module : bhic-app.wc
           * @moduledesc : Worker compensation (wc) application module will register this controllers to take quesionnaire page related activities
                        this will generate dynamic list of question before generating quote based question response it will show quote in next step
           * @ngInject : inject $scope module into controller for certain Moel-View communication
        */
        angular
           .module('bhic-app.wc')
           .controller('questionsPageCtrl', ['$scope', '$http', '$filter', 'bhic-app.services.restServiceConsumer', 'bhic-app.wc.services.questionsPageData', questionsController]);
           //.filter('num', function ()
           //{
           //     return function (input) {
           //         return parseInt(input);
           //     };
           // });

        angular
           .module('bhic-app.wc')
           .directive('sliderDays', function () {
               return {
                   require: '?ngModel',
                   link: function (scope, elem, attrs) {
                       console.log(scope.days);
                       $(elem).slider({
                           range: false,
                           min: scope.days[1],
                           max: scope.days[scope.days.length - 1],
                           step: parseInt(1),
                           slide: function (event, ui) {
                               scope.$apply(function (val) {
                                   scope.UserResponse = ui.value;
                                   scope[attrs.callback]();
                               });
                           }
                       });
                   }
               }
           });

        //Select default required directive
        angular
           .module('bhic-app.wc')
           .directive('requireSelect', function () {
               return {
                   restrict: 'AE',
                   scope: {},
                   require: 'ngModel',
                   link: function (scope, elem, attr) {
                       if (elem.val() == "") {
                           $(elem).addClass('ng-invalid').removeClass('ng-valid');
                       } else {
                           return true;
                       }
                   }
               }
           });

    }
)();