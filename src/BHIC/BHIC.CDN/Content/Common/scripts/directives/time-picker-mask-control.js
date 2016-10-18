(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.Directives').directive('timePickerMaskControl', ['$parse', timePickerMaskControlFn]);

    //Comment : Here this directive will check user uploaded files
    function timePickerMaskControlFn($parse)
    {
        function fn_link(scope, element, attrs, ctrl)
        {            
            //Comment : Here on file upload control file selection call method
            element.on('blur', function (event)
            {
                //debugger;
                var retStatus = false;
                var inputValue = element.val();

                //not undefined
                if (inputValue)
                {
                    //default reset 
                    retStatus = true;

                    //get splitted HH & MM values
                    var splittedValues = inputValue.split(':');
                    var hh = parseInt(splittedValues[0]); var mm = parseInt(splittedValues[1]);                    

                    //check hrs that should be 8 to 19
                    if (hh >= 8 && hh <= 19) {
                        if (hh == 19) {
                            if (mm > 30) {
                                retStatus = false;
                            }
                        }
                    }
                    else {
                        retStatus = false;
                    }
                }
                
                ctrl.$setValidity('validScheduleTime', retStatus);
            });
        };

        return {
            require: 'ngModel',
            link: fn_link
        }
    }
})();
