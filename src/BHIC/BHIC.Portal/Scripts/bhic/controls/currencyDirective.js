(function () {
    'use strict';

    //currency control directive definition
    angular.module('bhic-app.wc.controls').directive('currencyControl', ['bhic-app.services.restServiceConsumer', '$timeout', currencyControlFn]);
    function currencyControlFn(restServiceConsumer,$timeout) {
        return {
            restrict: 'AE',
            require: 'ngModel',
            scope: {
                amount: "="
            },
            link: function (scope, element, attrs, ctrl) {
                var res;
                var validRegex;
                if (attrs.decimalAllowed === true)
                    validRegex = /[^\d\.',']/g;
                else
                    validRegex = /[^\d\',']/g;
                scope.$watch('amount', function (val) {
                    if (val == '') {
                        ctrl.$error.required = true;
                        ctrl.$invalid = true;
                    }
                    else {
                        ctrl.$setViewValue(val);
                        ctrl.$render();
                    }
                });
                element.bind('blur', function () {

                    var exposureAmt = element.val();

                    if (exposureAmt != '' && exposureAmt != undefined)
                    {
                        var validRegex;
                        if (attrs.decimalAllowed === true)
                            validRegex = /[^\d\.',']/g;
                        else
                            validRegex = /[^\d\',']/g;

                        //Comment : Here by default set false
                        ctrl.$invalid = false;

                        //Comment : Here get stripped amount value
                        exposureAmt = exposureAmt.replace(/\D/g, '');

                        if (exposureAmt)
                        {
                            var classDescId = null, classDescKeywordId = null;

                            if (scope.$parent.quoteCtrl.class)
                                classDescId = scope.$parent.quoteCtrl.class.ClassDescriptionId;

                            if (scope.$parent.selectedClassDescKeyId)
                                classDescKeywordId = scope.$parent.selectedClassDescKeyId;

                            var queryString = '?exposureAmt=' + exposureAmt;

                            if (classDescId != undefined && classDescId != null)
                                queryString += '&classDescriptionId=' + classDescId + "&classDescKeywordId=null";
                            else if (classDescKeywordId != undefined && classDescKeywordId != null)
                                queryString += '&classDescKeywordId=' + classDescKeywordId + "&classDescriptionId=null";

                            //Comment : Here if both ids are null then don't call API 
                            if (classDescId != null || classDescKeywordId != null) {
                                //Comment : Here prepare final request string
                                var url = '/WcHome/ValidateExposureAmount' + queryString;

                                //Comment : Here raise server call and get validation resultant data
                                restServiceConsumer.getData(url).then(function (data) {

                                    //has some retuned data then check is validated or failed
                                    if (data) {

                                        if (data.resultStatus == 'NOK') {
                                            ctrl.$invalid = true;
                                            scope.$parent.quoteCtrl.expoAmtValidationMsg = data.resultText;
                                            element.focus();
                                        }
                                    }
                                });
                            }
                            else {
                                ctrl.$invalid = true;
                                scope.$parent.quoteCtrl.expoAmtValidationMsg = 'Either ClassDescriptionId or ClassDescKeywordId must be specified.';
                            }                                
                        }
                    }                    

                });
                element.bind('focus', function () {
                    $('.currencySymbol').show();
                });
                scope.amount = res;

                ctrl.$parsers.push(function (inputValue) {

                    var inputVal = element.val();
                    //clearing left side zeros

                    while (inputVal.charAt(0) == '0') {
                        inputVal = inputVal.substr(1);
                    }
                    inputVal = inputVal.replace(validRegex, '');

                    var point = inputVal.indexOf(".");
                    if (point >= 0) {
                        inputVal = inputVal.slice(0, point + 3);
                    }

                    var decimalSplit = inputVal.split(".");
                    var intPart = decimalSplit[0];
                    var decPart = decimalSplit[1];
                    intPart = intPart.replace(/[^\d]/g, '');
                    if (intPart.length > 3) {
                        var intDiv = Math.floor(intPart.length / 3);
                        while (intDiv > 0) {
                            var lastComma = intPart.indexOf(",");
                            if (lastComma < 0) {
                                lastComma = intPart.length;
                            }

                            if (lastComma - 3 > 0) {
                                intPart = intPart.slice(0, lastComma - 3) + "," + intPart.slice(lastComma - 3);
                            }
                            intDiv--;
                        }
                    }

                    if (decPart === undefined) {
                        decPart = "";
                    }
                    else {
                        decPart = "." + decPart;
                    }
                    res = intPart + decPart;

                    if (res != inputValue) {
                        ctrl.$setViewValue(res);
                        ctrl.$render();
                    }
                    $timeout(function () {
                        scope.amount = res;
                    });
                });
                element.bind('keypress', function (event) {
                    if (event.keyCode === 32) {
                        event.preventDefault();
                    }
                });
            }
        };
    }
})();


