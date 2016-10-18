(
    function ()
    {
        'use sctrict';

        /**
           * @module : bhic-app.Constants
           * @moduledesc : This module is core application Constants declaration for whole application category use
                            like 1) Worker Compensation 2) Others ..
           * @ngInject : 
        */
        angular
            .module('bhic-app.Constants', [])
            .constant('configConstant', { 'Name': 'Prem Pratap', 'Age': 31 });
    }
)();