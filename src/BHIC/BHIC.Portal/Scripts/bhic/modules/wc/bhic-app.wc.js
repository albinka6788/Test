(
    function ()
    {
        'use strict';

        /**
           * @module : bhic-app.wc
           * @moduledesc : This module is seprate module of core application which will belong for 
                            Business line => Worker Compensation (wc)
           * @ngInject : 
        */
        var dependencies = ['bhic-app.services', 'bhic-app.wc.services', 'ngSanitize', 'ngMessages', 'ui.slider', 'bhic-app.wc.controls']
        angular.module('bhic-app.wc', dependencies);


        //Comment : Here register this module specific configuration
        //angular
        //    .module('bhic-app.wc')
        //    .constant('BHIC.wc.Constant.config', { 'Name': 'Karan', 'Shah': 30 });

        //Comment : Here register controller for this module       
        
    }
)();