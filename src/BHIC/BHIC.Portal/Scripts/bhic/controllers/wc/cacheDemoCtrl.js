(
    function () {

        'use strict';

        angular.module('bhic-app.wc.cacheDemo', ['bhic-app.services', 'bhic-app.wc.controls'])
            .controller('cacheDemoCtrl', ['$scope', 'bhic-app.wc.services.cacheData', cacheDemoControllerFn]);

        function cacheDemoControllerFn($scope, cacheDataProvider) {

            function _init() {
                _getIndustry();
            };

            $scope.industryModel = {
                IndustryId: '',
                Description: ''
            };

            $scope.isUpdated = '';

            //Comment : Here get list of business type
            var _getIndustry = function () {

                var promise = cacheDataProvider.getIndustryData();

                promise.then(function (response) {
                    $scope.industryModel = angular.copy(response.industry);
                    $scope.isUpdated = response.isUpdated;
                });
            };

            _init();
        };
    }
)();