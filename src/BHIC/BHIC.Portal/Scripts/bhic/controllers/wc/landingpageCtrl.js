
(
    function () {
        'use strict';

        angular
            .module('bhic-app.wc')
            .controller('landingPageCtrl', ['$scope', '$http', 'bhic-app.wc.services.landingPageData', 'bhic-app.services.restServiceConsumer', '$filter', landingpageController]);

        function landingpageController($scope, $http, landingPageDataProvider, restServiceConsumer,filter)
        {

            var _exposureModel = {
                isLandingSaved: false,
                industryId: 0,
                subIndustryId : 0,
                classDescriptionId: 0,
                classDescriptionKeywordId: null,
                stateAbbr: '',
                zipCode : ''
            };

            var _expoAmtValidationMsg ='';

            function _init()
            {                
                landingPageDataProvider.getLandingPageDefaults().
                    then(function (data) {
                        $scope.industryDefaults = data.industryDefaults;
                        $scope.subIndustryDefaults = data.subIndustryDefaults;
                        $scope.businessDefaults = data.businessDefaults;
                        $scope.categories = data.categories;
                        $scope.keywordSearchAllowed = data.keywordSearchAllowed;
                        $scope.classOrIndustry = 0;
                        _updateSearchSelection($scope.classOrIndustry);
                    });

                //Comment : Here add get exposure data mode;
               _getWcExposureModel();
            };
            /*Private Methods*/
            var _getIndustries = function (exposure) {
                
                //Comment : here in case exposure not passed then set it to null
                exposure = exposure || null;                

                landingPageDataProvider.getIndustries().
                then(function (data) {
                    $scope.industries = data;

                    //Comment : Here in case of Exposure data exist then auto bind based Exposure
                    if (exposure != null)
                    {                        
                        //Comment : Here get filtered industry to auto select IndustryList
                        var filteredIndustry = filter('filter')($scope.industries, { IndustryId: exposure.IndustryId });
                        if (filteredIndustry.length >0)
                            $scope.quoteCtrl.industry = filteredIndustry[0];

                        //STEP - 2 Get Sub-Industries based on auto selected IndustryId
                        $scope.getSubIndustries(exposure.IndustryId, exposure);
                    }
                });
            };

            /*Public Methods*/
            var _getSubIndustries = function (industryId, exposure) {

                //Comment : here in case exposure not passed then set it to null
                exposure = exposure || null;

                //Comment : Here default set to hidden
                $scope.subIndustries = null;
                $scope.showSubIndustry = false;
                $scope.showClass = false;

                if (industryId && industryId > 0) {
                    $scope.processingSubIndustry = true;

                    landingPageDataProvider.getSubIndustries(industryId).
                    then(function (data) {
                        $scope.subIndustries = data;
                        $scope.showSubIndustry = true;
                        $scope.processingSubIndustry = false;

                        //Comment : Here in case of Exposure data exist then auto bind based Exposure
                        if (exposure != null) {

                            //STEP - 2.1 set sub-indutry ModelValue
                            $scope.quoteCtrl.subIndustry = exposure.SubIndustryId;

                            //Comment : Here get filtered industry to auto select IndustryList
                            var filteredSubIndustry = filter('filter')($scope.subIndustries, { SubIndustryId: exposure.SubIndustryId });
                            if (filteredSubIndustry.length > 0)
                                $scope.quoteCtrl.subIndustry = filteredSubIndustry[0];

                            //STEP - 3 Get ClassDesciption based on auto selected SubIndustryId
                            $scope.getClass(exposure.SubIndustryId, exposure);                            
                        }
                    });
                }
                //else {
                //    $scope.subIndustries = null;
                //    $scope.showSubIndustry = false;
                //    $scope.showClass = false;
                //}
            };

            var _getClass = function (subIndustryId, exposure) {

                //Comment : here in case exposure not passed then set it to null
                exposure = exposure || null;

                //default setting
                $scope.processingClass = true;

                //Comment : Here default set to hidden
                $scope.classes = null;
                $scope.showClass = false;

                if (subIndustryId > 0) {
                    landingPageDataProvider.getClass(subIndustryId).
                    then(function (data) {
                        $scope.classes = data;
                        $scope.showClass = true;
                        $scope.processingClass = false;

                        //Comment : Here in case of Exposure data exist then auto bind based Exposure
                        if (exposure != null) {

                            //STEP - 2.1 set sub-indutry ModelValue
                            $scope.quoteCtrl.class = exposure.ClassDescriptionId;

                            //Comment : Here get filtered industry to auto select IndustryList
                            var filteredClassDescription = filter('filter')($scope.classes, { ClassDescriptionId: exposure.ClassDescriptionId });
                            if (filteredClassDescription.length > 0)
                                $scope.quoteCtrl.class = filteredClassDescription[0];

                            //STEP - Finally show industry seelction flow 
                            if (exposure.ClassDescriptionKeywordId == null) {
                                $scope.classOrIndustry = 1;
                                _updateSearchSelection($scope.quoteCtrl.classOrIndustry);
                            }
                        }
                    });
                }
                else {
                    $scope.classes = null;
                    $scope.showClass = false;
                }
            };

            var _submitQuote = function () {
                if ($scope.showKeywordSearch) {
                    $scope.quoteCtrl.industry = null;
                    $scope.quoteCtrl.subIndustry = null;
                    $scope.quoteCtrl.class = null;
                    $scope.quoteCtrl["classDescKeyId"] = $scope.selectedClassDescKeyId;
                }
                else {
                    if ($scope.quoteCtrl.hasOwnProperty('classDescKeyId'))
                        $scope.quoteCtrl["classDescKeyId"] = null;
                }
                landingPageDataProvider.submitExposureData($scope.quoteCtrl);
            };
            var _updateSearchSelection = function (val) {
                if (val == 0)
                    $scope.showKeywordSearch = true;
                else
                    $scope.showKeywordSearch = false;
            };

            var getResultPage = function () {
                window.location.assign('/QuestionEngine/MyQuestions/ResultsQuoteGet');
            }

            var _getWcExposureModel = function () {
                restServiceConsumer.getData('/Quote/GetExposures').then(function (data) {

                    if (data.success == true)
                    {
                        //Comment : Here if user has not saved any details on landing page earlier than show default setting 
                        if (!data.isLandingSaved) {
                            //console.log('landing not saved !');
                            _getIndustries();
                        }
                        else {
                            //Comment : Here for Auto-polpulate controls
                            var exposure = data.exposureModel;
                            if (exposure != null)
                            {
                                //console.log(data);

                                $scope.quoteCtrl.zipCode = exposure.ZipCode;
                                if (exposure.ExposureAmt > 0)
                                {
                                    $scope.quoteCtrl.exposureAmount = exposure.ExposureAmt;
                                    angular.element('.currencySymbol').show();
                                    $scope.amount = $scope.quoteCtrl.exposureAmount;
                                }

                                //Comment : Here policyData model object
                                var policyData = data.policyModel;
                                if (policyData != null)
                                {
                                    //console.log(policyData);
                                    $scope.quoteCtrl.policyDate = policyData;
                                }

                                //Comment : Here first check is this KeywordSerched exposures or IndustrySelection
                                if (exposure.ClassDescriptionKeywordId != null) {
                                    
                                    //console.log('Exposure type : KeywordSearch !');                                    
                                    var keySrchId = exposure.ClassDescriptionKeywordId;

                                    var url = "/Quote/SearchBusiness?classDescriptionId=" + keySrchId;
                                    restServiceConsumer.getData(url).then(function (data) {

                                        //console.log(data);
                                        if (data != null)
                                        {
                                            //Comment : Here get & set first item[ClassDescriptionName/Keyword]
                                            $scope.classDescriptionKeywordId = data[0].Keyword;

                                            //Comment : here set auto selected item keyId into parent scope model 
                                            $scope.$parent.selectedClassDescKeyId = data[0].ClassDescKeywordId;
                                            //console.log($scope.$parent.selectedClassDescKeyId);

                                            //Comment : Here call industry,subIndustry,classDescription in hidden way
                                            _getIndustries(exposure);

                                            //Comment : Here explicitly change view for KeywordSearch/IndustrySelection
                                            //$scope.classOrIndustry = 0;
                                            //_updateSearchSelection($scope.classOrIndustry);
                                        }
                                    });
                                }
                                else {
                                    //console.log('Exposure type : IndustrySelection !');
                                    
                                    //STEP - 1 Get Industries
                                    _getIndustries(exposure);
                                }
                            }
                        }
                    }
                });
            };

            angular
                .extend($scope,
                {
                    getSubIndustries: _getSubIndustries,
                    getClass: _getClass,
                    show: false,
                    showSubIndustry: false,
                    showClass: false,
                    processingSubIndustry: false,
                    processingClass: false,
                    submitQuote: _submitQuote,
                    updateSearchSelection: _updateSearchSelection,
                    getResultPage: getResultPage,
                    expoAmtValidationMsg: _expoAmtValidationMsg
                });

            _init();
        };
    }
)();