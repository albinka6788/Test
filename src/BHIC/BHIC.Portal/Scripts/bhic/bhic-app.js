(
    function ()
    {
        'use sctrict';

        /**
           * @module : bhic-app.wc
           * @moduledesc : This module is core application object which will include all 4 kind of business line
                            like 1) Worker Compensation 2) Others ..
           * @ngInject : 
        */
        angular.module('bhic-app',
            [
                'bhic-app.wc',
                'bhic-app.Constants',
                'bhic-app.services',
                'bhic-app.wc.services'
                //'ngResource'
            ]
           );


        /**
           * @class : bhicRootAppConfiguration
           * @classdesc : All configuration related to all business line categories and thier modules,
                        function with certain Models,Properties
           * @ngInject : $provide, $routeProvider, $locationProvider
        */
        function bhicRootAppConfiguration($provide)
        {
            /**
               * @desc : Here Configure html5 to get links working
                        Summary - If you don't do this, you URLs will be base.com/#/home rather than base.com/home
               * @ngInject : 
            
            $locationProvider.html5Mode(true);*/


            /**
               * @desc : Here root application level $provide objects like .constant(),.value(),.provider(),.factory()
                        Summary - these will be used in through-out application, so please define only generic for all business lines
               * @ngInject : 
            */
            var rootAppVerDetails =
                {
                    'inceptionDate': '2015-06-01',
                    'version': 'v1.001',
                    'vendor':'Xceedance'
                };

            $provide.constant('config', rootAppVerDetails);
        };


        /**
           * @desc : Here define top level bhic application root module & proveders configuration 
           * @ngInject : 
        
        angular
            .module('bhic-app')
            .config(['$provider', bhicRootAppConfiguration]);*/        



        /**
           * @class : bhicRootAppRunSetup
           * @classdesc : All core application bootstrap related to all business line categories and thier modules,
                        function with certain Models,Properties,any common/generic function which can we reusable trough-out application can be declared in $rootScope
                        so $scope inject in any controller will be able to utilize common functionalities
           * @ngInject : $rootScope,$http
        */
        function bhicRootAppRunSetup($http, $rootScope)
        {

            //Comment : Here http-header setup at application level
            var apiVersion = '';
            $http.defaults.headers.common.Accept = 'application/jsonp;v=' + apiVersion + ', text/plain, */*';
            $http.defaults.useXDomain = true;
            $http.defaults.withCredentials = true;

            //Comment : Here this header inclusion will allow $http provider request to be trated as IsAjaxRequeset in MVC
            $http.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';


            /**
               * @desc : The $rootScope is a global, which means that anything you add here, automatically becomes available in $scope in all controller
            */
            $rootScope.hello = function (name)
            {
                console.log('Hello ' + name + ' !');
            }
        };


        /**
           * @desc : Here define some module and providers at bootstrap level of application
           * @ngInject : 
        */
        angular
            .module('bhic-app')
            .run(['$http', '$rootScope', bhicRootAppRunSetup]);
        
    }
)();


