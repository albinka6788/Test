(function ()
{
    'use strict';

    //directive definition
    angular.module('BHIC.Directives').directive('mutipleFileUploadControl', ['$parse', mutipleFileUploadControlFn]);

    //Comment : Here this directive will check user uploaded files
    function mutipleFileUploadControlFn($parse)
    {
        function fn_link(scope, element, attrs)
        {
            var onChange = $parse(attrs.mutipleFileUploadControl);

            //Comment : Here on file upload control file selection call method
            element.on('change', function (event)
            {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            require: 'ngModel',
            link: fn_link
        }
    }
})();
