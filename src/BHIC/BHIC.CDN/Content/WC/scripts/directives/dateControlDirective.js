(function () {
    'use strict';

    angular.module('BHIC.WC.Directives').directive('dateControl', ['$timeout', 'coreUtils', dateControlFn]);
    function dateControlFn($timeout, coreUtils) {
        return {
            restrict: 'AE',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                var tElement = element;
                var disableDatePicker = false;

                function _attachDatePicker() {
                    tElement.datepicker({
                        minDate: attrs.startdate,
                        maxDate: attrs.enddate,
                        daysOfWeekDisabled: attrs.disabledDays, //0 for sunday and 6 for saturdays
                        dateFormat: attrs.format,
                        autoclose: true,
                        //showOn: "both",
                        onSelect: function (dateInput) {
                            $timeout(function () {
                                ngModel.$setViewValue(dateInput);
                                $(this).val(dateInput);
                                _hideDatePicker();
                                element.blur();
                            });
                        },
                        onClose: function (datetext) {

                            //Check date format start
                            try {
                                $.datepicker.parseDate(attrs.format, datetext);
                                ngModel.$setViewValue(datetext);
                                ngModel.$setValidity('invalidFormat', true);
                            }
                            catch (e) {
                                ngModel.$setViewValue('');
                                ngModel.$setValidity('invalidFormat', false);
                                return;
                            }

                            var selectedDate = $(this).datepicker("getDate");
                            var maxDate = new Date();
                            maxDate.setDate(maxDate.getDate() + 60);
                            maxDate.setHours(23, 59, 59, 0);
                            var minDate = new Date();
                            minDate.setDate(minDate.getDate() + 1);
                            minDate.setHours(0, 0, 0, 0);
                            if (minDate <= selectedDate && selectedDate < maxDate) {
                                ngModel.$setValidity('invalidRange', true);
                                $(this).val(datetext);
                            }
                            else {
                                ngModel.$setValidity('invalidRange', false);
                                scope.minDate = minDate;
                                scope.maxDate = maxDate;
                                //$(this).val('');
                            }
                            scope.$apply();
                            //Check date format end
                        }
                    })
                }

                function _hideDatePicker() {
                    $(tElement).datepicker('hide');
                }

                function _showDatePicker() {
                    $(tElement).datepicker('show');
                }

                attrs.$observe('disableDatePicker', function (newValue) {
                    disableDatePicker = newValue;
                    $timeout(function () {
                        if (!coreUtils.IsEmptyString(newValue) && !coreUtils.IsEmptyObject(newValue)) {
                            if (newValue) {
                                _hideDatePicker();
                            }
                            else {
                                _showDatePicker();
                            }
                        }
                    }, 500)
                });

                tElement.on("click focus", function () {
                    _showDatePicker();
                });

                _attachDatePicker();
            }
        };
    }
})();
