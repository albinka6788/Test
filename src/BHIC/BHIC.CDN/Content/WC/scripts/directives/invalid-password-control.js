(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('invalidPasswordControl', ['$timeout','quotePurchaseService', invalidPasswordControlFn]);    

    //Comment : Here validate password control directive implementation
    function invalidPasswordControlFn(timeout,quotePurchaseService)
    {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, elem, attr, ctrl)
            {                
                //Comment : Here validate text on Element blur
                elem.on('blur', function ()
                {
                    //debugger;
                    
                    //Comment : Here get user email-id from directive attribute
                    var emailId = attr.userEmailId.replace(/[\s]/g, '');

                    //Comment : Here get control value
                    var password = elem.val().replace(/[\s]/g, '');

                    if (emailId.length == 0 || password.length == 0)
                        return;

                    //Comment : Here show control level loader/progress
                    quotePurchaseService.controlLevelLoader(elem, true);

                    //Comment : here call service to submit questions response                    
                    var promise = quotePurchaseService.hasValidPassword(emailId, password);

                    promise
                        .then(function (data)
                        {
                            timeout(function ()
                            {
                                //debugger;

                                if (data != null)
                                {
                                    //Comment : Here if not-validated then 
                                    if (data.resultStatus == 'False')
                                    {
                                        //it means InValid password
                                        ctrl.$setValidity('invalidPassword', false);
                                        //console.log('supplied pwd is invalid !');
                                    }
                                    else if (data.resultStatus == 'True')
                                    {
                                        ctrl.$setValidity('invalidPassword', true);
                                    }
                                }
                            });
                        })
                        .catch(function (exception)
                        {
                            ctrl.$setValidity('invalidPassword', true);
                            //console.log(exception.toUpperCase());                            
                        })
                        .finally(function () {
                            //console.log("finally block called !");
                            //Comment : Here hide control level loader/progress
                            quotePurchaseService.controlLevelLoader(elem, false);
                        });
                });
            }
        };
    }
    
})();


