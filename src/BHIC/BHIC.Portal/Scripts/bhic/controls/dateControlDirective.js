(function () {
    'use strict';

    //currency control directive definition
    angular.module('bhic-app.wc.controls').directive('dateControl', dateControlFn);
    function dateControlFn() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                $(element).datepicker({
                    startDate: attrs.startDate,
                    endDate: attrs.endDate
                    //daysOfWeekDisabled: [0, 6]
                })
                .on("change", function (e) {
                    var date = element.val() == "" ? ngModelCtrl.$$lastCommittedViewValue : element.val();
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(date);
                        $(element).val(date).datepicker('update');
                    });
                    $(this).datepicker('hide');
                });
            }
        };
    }
})();
