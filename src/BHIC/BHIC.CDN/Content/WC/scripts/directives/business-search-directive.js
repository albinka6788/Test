(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('businessSearch', ['restServiceConsumer', '$timeout', 'coreUtils', function (restServiceConsumer, $timeout) {
        return {
            require: '?ngModel',
            scope: {


            },

            link: function (scope, element, attrs, ngModel) {
                scope.myModel = null;
                scope.myModelId = [];
                scope.isDisabled = true;

                scope.validateModel = function () {
                    scope.isDisabled = ($scope.myModelId === null);
                };
                element.on('input', function () {
                    scope.$parent.quoteViewModel.ClassDescriptionKeywordId = '';
                    scope.$parent.quoteViewModel.ClassDescriptionId = '';
                    scope.$parent.quoteViewModel.BusinessClassDirectSales = '';
                    scope.$parent.directSales = null;
                    scope.$parent.quoteViewModel.ClassCode = '';
                    scope.$parent.quoteViewModel.OtherClassDesc = '';

                });


                element.autocomplete({
                    minLength: 2,
                    appendTo: element.closest('.form-group'),
                    source: function (request, response) {
                        var queryString = "?searchString=" + request.term;
                        if (!isNaN(request.term)) {
                            queryString = "?classDescriptionId=" + request.term;
                        }

                        /*To check whether search is first time or not. If it is first time setting the current time*/
                        if (scope.$parent.keywordsearchcounter == 0) {
                            scope.$parent.keywordsearchstarttime = new Date();
                        }
                        /*To check whether the time is elapsed for the second search to show the default list*/
                        var url = "/Exposure/SearchBusiness" + queryString;
                        //Declaring Default List for search results
                        var otherClassList = { ClassCode: "-1", ClassDescKeywordId: "-1", ClassDescriptionId: "-1", ClassSuffix: "", DirectOK: "Y", Keyword: "I can't find my business description" }
                        var noDataFoundClassList = { ClassCode: "", ClassDescKeywordId: "", ClassDescriptionId: "", ClassSuffix: "", DirectOK: "", Keyword: "No Records Found - Please Try Again" }
                        //var start = performance.now();
                        restServiceConsumer.getData(url).then(function (data) {
                            /*If search delay elapsed the other class will be shown in autocomplete. */

                            // if keyword search performed first time
                            if (scope.$parent.keywordsearchcounter == 0) {
                                scope.$parent.keywordsearchcounter = 1;
                                // If data received from API have records.
                                if (data.length > 0) {
                                    // Set timeout for 5 seconds and after that add other class item in the list.
                                    $timeout(function () {
                                        data.push(otherClassList)
                                        response($.map(data, function (item) {
                                            return {
                                                label: item.Keyword,
                                                value: item.ClassDescKeywordId + ":" + item.ClassDescriptionId + ":" + item.ClassCode + ":" + (item.DirectOK || '')
                                            };
                                        }))

                                    }, 5000);
                                }
                            }
                            else {
                                // If second time search and data received from API then add other class description.
                                if (data.length > 0) {
                                    data.push(otherClassList);
                                }
                            }

                            // In case no data received from API then add no record found and other class description.
                            if (data.length == 0) {
                                data.push(noDataFoundClassList);
                                data.push(otherClassList);
                            }

                            scope.businessnames = data;
                            if (data.length < 1)
                                scope.$parent.dataNotFound = true;
                            else
                                scope.$parent.dataNotFound = false;

                            scope.$parent.msmcData = null;

                            response($.map(data, function (item) {
                                return {
                                    label: item.Keyword,
                                    value: item.ClassDescKeywordId + ":" + item.ClassDescriptionId + ":" + item.ClassCode + ":" + (item.DirectOK || '')
                                };

                            }));
                            element.removeClass('ui-autocomplete-loading');
                        });
                    },
                    select: function (event, ui) {
                        $timeout(function () {
                            element.val(ui.item.label);
                            if (ui.item.value.split(":")[1] != "") {
                                scope.$parent.quoteViewModel.ClassDescriptionKeywordId = ui.item.value.split(":")[0];
                                scope.$parent.quoteViewModel.ClassDescriptionId = ui.item.value.split(":")[1];
                                scope.$parent.quoteViewModel.ClassCode = ui.item.value.split(":")[2];
                                scope.$parent.quoteViewModel.BusinessClassDirectSales = ui.item.value.split(":")[3];
                                scope.$parent.directSales = scope.$parent.quoteViewModel.BusinessClassDirectSales;
                                scope.$parent.quoteViewModel.BusinessName = ui.item.label;
                                ngModel.$setViewValue(ui.item.label);
                                ngModel.$commitViewValue();
                                if (scope.$parent.quoteViewModel.ClassDescriptionId > 0) {
                                    scope.$parent.setCompanionClassData(scope.$parent.quoteViewModel.ClassDescriptionId, 'business', scope.$parent.directSales);
                                }
                            }
                            else {
                                element.val("");
                            }
                        });
                        return false;
                    },
                    focus: function (event, ui) {
                        event.preventDefault();
                    },
                    change: function (event, ui) {
                        //if (ui.item === null) {
                        //    ui.item.value = null;
                        //}
                    },
                    //GUIN-465 To Enable Item selection on first tap in iphone safari
                    open: function (event, ui) {
                        $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
                    }

                }).off('blur').blur(function (event, ui) {
                    $timeout(function () {
                        var selval = element.val();
                        if (scope.businessnames) {
                            for (var i = 0; i < scope.businessnames.length; i++) {
                                if (scope.businessnames[i].Keyword.toUpperCase().trim() == selval.toUpperCase().trim()) {
                                    element.autocomplete("close");
                                    scope.$parent.quoteViewModel.ClassDescriptionKeywordId = scope.businessnames[i].ClassDescKeywordId;
                                    scope.$parent.quoteViewModel.ClassDescriptionId = scope.businessnames[i].ClassDescriptionId;
                                    scope.$parent.quoteViewModel.ClassCode = scope.businessnames[i].ClassCode;
                                    scope.$parent.quoteViewModel.BusinessClassDirectSales = scope.businessnames[i].DirectOK;
                                    scope.$parent.directSales = scope.$parent.quoteViewModel.BusinessClassDirectSales;
                                    scope.$parent.quoteViewModel.BusinessName = scope.businessnames[i].Keyword;
                                    scope.$parent.quoteViewModel.searchOptionNotSelected = false;
                                    if (scope.$parent.quoteViewModel.CompClassData && scope.$parent.quoteViewModel.CompClassData.length > 0) {
                                        scope.$parent.quoteViewModel.CompClassData = scope.$parent.quoteViewModel.CompClassData;
                                    }
                                    else {
                                        scope.$parent.setCompanionClassData(scope.$parent.quoteViewModel.ClassDescriptionId, 'business', scope.$parent.directSales);
                                    }
                                }
                            }
                        }
                        if (!scope.$parent.quoteViewModel.ClassDescriptionKeywordId || scope.$parent.quoteViewModel.ClassDescriptionKeywordId == '') {
                            ngModel.$setValidity('itemNotSelected', false);
                        }
                        else {
                            ngModel.$setValidity('itemNotSelected', true);
                        }
                    });

                }).keydown(function (e) {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        //return false;
                    }
                });
            }
        }
    }]);
})();


