(
    function ()
    {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.Services').factory('questionsService', ['restServiceConsumer', '$resource','$q', questionsServiceFn]);

        function questionsServiceFn(restServiceConsumer, resource, q)
        {
            //Comment : Here create resource object to make RESTful API calls
            var questionsResource = resource('/Questions/PostQuestionResponse/');

            /**
             * @description
             * saves the questionnaire (questions responses) submitted by user
            */
            function _submitQuestionsResponse(questionsData, taxIdNumber, taxIdType) {
                var deferred = q.defer();

                var promise = questionsResource.save({ questionsList: questionsData, taxIdNumber: taxIdNumber, taxIdType: taxIdType }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
                //return questionsResource.save({ questionsList: questionsData }).$promise;
            }

            //Comment : Here create resource object to make RESTful API calls
            var saveForLaterResource = resource('/Questions/SaveForLater/');

            /**
             * @description
             * saves the questionnaire (questions responses) submitted by user
            */
            function _submitSaveForLaterResponse(questionsData, taxIdNumber, taxIdType, emailId) {
                var deferred = q.defer();

                var promise = saveForLaterResource.save({ questionsList: questionsData, taxIdNumber: taxIdNumber, taxIdType: taxIdType, emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            //Comment : Here expose factory public methods 
            return {
                submitQuestionsResponse: _submitQuestionsResponse,
                submitSaveForLater: _submitSaveForLaterResponse
            };
        }
    }
)();