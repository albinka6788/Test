(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('checkUserExistenceControl', ['$timeout', 'quotePurchaseService', checkUserExistenceControlFn]);

    //Comment : Here user existance check control directive implementation
    function checkUserExistenceControlFn(timeout, quotePurchaseService) {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, elem, attr, ctrl)
            {
                //Comment : Here check account existance on Element blur
                elem.on('blur', function ()
                {
                    //Comment : Here get control value
                    var emailId = elem.val().replace(/[\s]/g, '');

                    //Comment : Here if empty email then break execution
                    if (emailId.length == 0)
                        return;

                    //Comment : Here show control level loader/progress
                    quotePurchaseService.controlLevelLoader(elem, true);

                    //Comment : here call service to submit questions response                    
                    var promise = quotePurchaseService.isExistingUser(emailId);

                    promise
                        .then(function (data) {
                            timeout(function () {
                                if (data != null) {
                                    //Comment : Here if user exists/found then 
                                    if (data.resultStatus == 'True') {
                                        //it means user-account found
                                        ctrl.$setValidity('userExists', false);
                                        //console.log(data.resultText);
                                    }
                                    else if (data.resultStatus == 'False') {
                                        ctrl.$setValidity('userExists', true);
                                    }
                                }
                            });
                        })
                        .catch(function (exception) {
                            ctrl.$setValidity('userExists', true);
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