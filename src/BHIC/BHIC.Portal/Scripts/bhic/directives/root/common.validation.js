
(
    function ()
    {
        'use strict';

        /**
           * @directive : ZipCode
           * @classdesc : This directive will validate/check valid ZipCode in INPUT field,
        */

        function validZipCode()
        {
            debugger;
            return {
                restrict: 'EA',
                require : '?ngModel',
                link: function (scope, elem, attr,ngModel) {
                    if (ngModel == null)
                        true;
                }
            }
        };

        function validCreditCardNumber() {
            return {
                restrict: 'EA',
                require: '?ngModel',
                link: function (scope, elem, attr, ngModel) {
                    if (ngModel == null)
                        alert('undefined');
                    else
                        alert(elem.val());
                }
            }
        };
    }
)();

