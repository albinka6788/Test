(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.Dashboard.Directives').directive('filesUpload', ['$parse', filesUploadFn]);
    function filesUploadFn($parse) {
        function fn_link(scope, element, attrs) {
            var onChange = $parse(attrs.filesUpload);
            element.on('change', function (event) {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            require: 'ngModel',
            link: fn_link
        }
    }
})();
