(function () {
    'use strict';

    angular.module('BHIC.Dashboard.Directives').directive('dateControl', dateControlFn);
    function dateControlFn() {
        return {
            restrict: 'AE',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                attrs.$observe('startdate', function (val) {
                    if ($.datepicker.parseDate(attrs.format, val) != null) {
                        element.datepicker('option', 'minDate', val);
                    }                    
                }),
                attrs.$observe('enddate', function (val) {
                    if ($.datepicker.parseDate(attrs.format, val) != null) {
                        element.datepicker('option', 'maxDate', val);
                    }
                }),
                element.datepicker({
                    minDate: attrs.startdate,
                    maxDate: attrs.enddate,
                    daysOfWeekDisabled: attrs.disabledDays, //0 for sunday and 6 for saturdays
                    dateFormat: attrs.format,
                    autoclose: true,
                    onSelect: function (dateInput) {
                        scope.$apply(function () {
                            ngModel.$setViewValue(dateInput);
                            $(this).val(dateInput);
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
                        }
                        var selectedDate = $(this).datepicker("getDate");
                        var minDate = new Date(attrs.startdate);
                        var maxDate = new Date(attrs.enddate);
                        //Report Claim
                        if (attrs.reportclaim) {
                            minDate.setDate(minDate.getDate());
                            minDate.setHours(0, 0, 0, 0);
                            maxDate.setDate(maxDate.getDate());
                            maxDate.setHours(23, 59, 59, 0);
                            if (minDate <= selectedDate && selectedDate < maxDate) {
                                ngModel.$setValidity('invalidRange', true);
                                $(this).val(datetext);
                            }
                            else {
                                ngModel.$setValidity('invalidRange', false);
                                //$(this).val('');
                            }

                        }
                        //Report Claim Out Side PC
                        if (attrs.outsidereportclaim) {
                            maxDate.setDate(maxDate.getDate());
                            maxDate.setHours(23, 59, 59, 0);
                            if (selectedDate < maxDate) {
                                ngModel.$setValidity('invalidRange', true);
                                $(this).val(datetext);
                            }
                            else {
                                ngModel.$setValidity('invalidRange', false);
                                //$(this).val('');
                            }

                        }
                        //Cancel Policy
                        if (attrs.cancelpolicy) {
                            minDate.setDate(minDate.getDate());
                            minDate.setHours(0, 0, 0, 0);
                            maxDate.setDate(maxDate.getDate());
                            maxDate.setHours(23, 59, 59, 0);
                            if (minDate <= selectedDate && selectedDate < maxDate) {
                                ngModel.$setValidity('invalidRange', true);
                                $(this).val(datetext);
                            }
                            else {
                                ngModel.$setValidity('invalidRange', false);
                                //$(this).val('');
                            }
                        }
                        //Change Policy
                        if (attrs.changepolicy) {
                            minDate.setDate(minDate.getDate() + 0);
                            minDate.setHours(0, 0, 0, 0);
                            maxDate.setDate(maxDate.getDate());
                            maxDate.setHours(23, 59, 59, 0);
                            if (minDate <= selectedDate && selectedDate < maxDate) {
                                ngModel.$setValidity('invalidRange', true);
                                $(this).val(datetext);
                            }
                            else {
                                ngModel.$setValidity('invalidRange', false);
                                //$(this).val('');
                            }
                        }
                        //Check date format end
                        scope.$apply();
                    }
                })
            }
        };
    }
})();
