(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('validateTaxidNumberControl', ['$timeout', 'restServiceConsumer','quotePurchaseService', validateTaxidNumberControlFn]);

    //Comment : Here user existance check control directive implementation
    function validateTaxidNumberControlFn(timeout, restServiceProvider, quotePurchaseService)
    {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, elem, attr, ctrl)
            {
                //Comment : Here check account existance on Element blur
                elem.on('blur', function () {
                    //debugger;

                    //Comment : Here get control value
                    var taxIdNumber = elem.val().replace(/[\s]/g, '');

                    //Comment : Here if empty email then break execution
                    if (taxIdNumber.length == 0)
                        return;

                    //Comment : Here show control level loader/progress
                    quotePurchaseService.controlLevelLoader(elem, true);
                    //Comment : here call service to validate entered FEIn number
                    //GUIN-238 : attr.update is set in HTML and in following promise will denote if feinNumber/taxId value will be updated in session or not 
                    var promise = restServiceProvider.isValidTaxIdNumber(taxIdNumber, attr.update);

                    promise
                        .then(function (data)
                        {
                            timeout(function ()
                            {
                                if (data != null)
                                {
                                    //Comment : Here if taxid-number valid then 
                                    if (data.resultStatus == 'True')
                                    {
                                        //it means user-account found
                                        ctrl.$setValidity('validTaxIdNumber', true);
                                        //console.log(data.resultText);
                                    }
                                    else if (data.resultStatus == 'False')
                                    {
                                        ctrl.$setValidity('validTaxIdNumber', false);
                                    }
                                }
                            });
                        })
                        .catch(function (exception)
                        {
                            //console.log(exception.toUpperCase());
                        })
                        .finally(function ()
                        {                            
                            //Comment : Here hide control level loader/progress
                            quotePurchaseService.controlLevelLoader(elem, false);
                        });

                });
            }
        };
    }

})();