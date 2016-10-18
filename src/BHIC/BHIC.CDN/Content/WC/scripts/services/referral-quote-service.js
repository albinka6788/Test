(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.Services').factory('referralQuoteService', ['$resource', '$q','$http', referralQuoteServiceFn]);

        function referralQuoteServiceFn(resource, q,http)
        {
            //Comment : Here create resource object to make RESTful API calls
            var referralQuoteResourceOLd = resource('/ReferralQuote/PostReferralQuoteData/');

            /**
             * @description
             * saves the business-contact information for this ReferralQuote submitted by user
            */
            function _submitReferralQuoteData(referralData,uploadedFilesFormData) {

                //debugger;
                var deferred = q.defer();

                var referralQuoteResource = resource('/ReferralQuote/PostReferralQuoteData/', {}, {
                    save: {
                        method: 'POST',
                       
                        headers: { 'Content-Type': undefined },

                        //This method will allow us to change how the data is sent up to the server
                        // for which we'll need to encapsulate the model data in 'FormData'
                        transformRequest: function (data)
                        {
                            var formData = new FormData();

                            //now add all of the assigned files
                            for (var i = 0; i < data.uploadedFiles.length; i++)
                            {
                                //add each file to the form data and iteratively name them
                                formData.append("File" + (i + 1), data.uploadedFiles[i]);
                            }

                            //Comment : Here add PostedForm details
                            if (data.referralQuoteModel != null)// && data.uploadedFiles.length > 0)
                            {
                                formData.append('DataModel', JSON.stringify(data.referralQuoteModel));
                            }
                            
                            return formData || null;
                        },

                        data: { referralQuoteModel: referralData, uploadedFiles: uploadedFilesFormData }
                    }
                });

                var promise = referralQuoteResource.save({ referralQuoteModel: referralData, uploadedFiles: uploadedFilesFormData }).$promise;

                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            //Comment : Here expose factory public methods 
            return {
                submitReferralQuoteData: _submitReferralQuoteData,
            };
        }
    }
)();