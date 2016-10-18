(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        var dependencies = ['bhic-app.services'];
        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.landingPageData', ['bhic-app.services.restServiceConsumer', '$q', landingPageDataFn]);

        function landingPageDataFn(restServiceConsumer, $q) {
            function Exposure(IndustryId, SubIndustryId, ClassDescriptionId, ClassDescriptionKeywordId, ZipCode, ExposureAmt, ExposureId, QuoteId, SimpleFlow, ClassCode, ClassSuffix, StateAbbr, InceptionDate) {
                this.ExposureId = ExposureId || null;
                this.QuoteId = QuoteId || null;
                this.IndustryId = IndustryId || null;
                this.SubIndustryId = SubIndustryId || null;
                this.ClassDescriptionId = ClassDescriptionId || null;
                this.ClassDescriptionKeywordId = ClassDescriptionKeywordId || null;
                this.ZipCode = ZipCode || null;
                this.SimpleFlow = SimpleFlow || null;
                this.ClassCode = ClassCode || null;
                this.ClassSuffix = ClassSuffix || null;
                this.ExposureAmt = ExposureAmt || null;
                this.StateAbbr = StateAbbr || null;
                this.InceptionDate = InceptionDate || null;
            };


            /*Private Methods*/
            function createExposureRequest(data) {
                var res = new Exposure();
                if (data.industry) {
                    res.IndustryId = data.industry.IndustryId;
                    res.SubIndustryId = data.subIndustry.SubIndustryId;
                    res.ClassDescriptionId = data.class.ClassDescriptionId;
                }
                else {
                    res.ClassDescriptionKeywordId = data.classDescKeyId;
                }
                res.ExposureAmt = data.exposureAmount.split(',').join("");
                res.ZipCode = data.zipCode;
                res.InceptionDate = new Date(data.policyDate).toUTCString();

                return res;
            };

            /*Public Methods*/
            var _getIndustries = function () {
                var deferred = $q.defer();
                restServiceConsumer.getData('/WcHome/GetIndustries')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            var _getSubIndustries = function (industryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/WcHome/GetSubIndustries?IndustryId=' + industryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getClass = function (subIndustryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/WcHome/GetClassDescriptions?SubIndustryId=' + subIndustryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getLandingPageDefaults = function () {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Scripts/bhic/contents/wc/landingPage.js').then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _submitExposureData = function (data) {
                restServiceConsumer.postData("/WcHome/SaveLandingPageData", createExposureRequest(data)).then(function (response) {
                    window.location.assign(response);                    
                });
            };
            return {
                getIndustries: _getIndustries,
                getSubIndustries: _getSubIndustries,
                getClass: _getClass,
                getLandingPageDefaults: _getLandingPageDefaults,
                submitExposureData: _submitExposureData
            };
        }
    }
)();