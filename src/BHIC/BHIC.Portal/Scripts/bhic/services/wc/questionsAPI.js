(
    function () {
        'use strict';

        /**
        @factory : this function will be used to render/show questions based on exposure data which includes ZipCode,Industry and other home page key params
        @description : user can submit his response also to get Quote Status next step/page in flow to buy a online policy.
        */
        var dependencies = ['bhic-app.services'];

        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.questionsPageData', ['bhic-app.services.restServiceConsumer', '$q', questionsPageDataFn]);
        function SaveForLater(email) {
            this.Email = email || null;
        };


        //Comment : Here factory method implementation
        function questionsPageDataFn(restServiceConsumer, q) {
            //declration portion
            var _getQuestions = _getQuestions;

            //implementation portion
            function _getQuestions() {
                var deferred = q.defer();
                //restServiceConsumer.getData('/Scripts/bhic/contents/wc/questions.json')
                restServiceConsumer.getData('/WcHome/GetQuestions')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            function _submitQuestionResponse(questionsList) {
                var deferred = q.defer();
                restServiceConsumer.postData("/WcHome/PostQuestionResponse", questionsList).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }

            function _submitSaveForLaterResponse(data) {
                var model = new SaveForLater(data.email);
                var deferred = q.defer();

                restServiceConsumer.postData("/Landing/WcHome/SaveForLater", model).
                    then(function (response) {
                        deferred.resolve(response);
                        return response;
                    });
                return deferred.promise;
            }


            //Comment : Here expose factory methods & properties
            return {
                getQuestions: _getQuestions,
                submitQuestions: _submitQuestionResponse,
                submitSaveForLaterResponse: _submitSaveForLaterResponse
            };
        };
    }
)();