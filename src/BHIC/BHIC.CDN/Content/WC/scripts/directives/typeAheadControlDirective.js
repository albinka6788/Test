(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('autoComplete', ['restServiceConsumer', '$timeout', function (restServiceConsumer, $timeout) {
        return function (scope, element, attrs) {
            scope.myModel = null;
            scope.myModelId = [];
            scope.isDisabled = true;
            scope.businessnames = {};
            scope.validateModel = function () {
                scope.isDisabled = ($scope.myModelId === null);
            };
            element.on('input', function () {
                scope.$parent.quoteViewModel.selectedClassDescKeyId = '';
                scope.$parent.quoteViewModel.selectedClassDescId = '';
                scope.$parent.quoteViewModel.selectedClassDirectSales = '';
            });
            element.autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var queryString = "?searchString=" + request.term;
                    if (!isNaN(request.term)) {
                        queryString = "?classDescriptionId=" + request.term;
                    }

                    var url = "/Quote/SearchBusiness" + queryString;
                    //var start = performance.now();
                    restServiceConsumer.getData(url).then(function (data) {
                        //var apirespRec = performance.now();
                        //var diff = apirespRec - start;
                        //console.log("API response recieved in :" + diff);
                        scope.businessnames = data;
                        if (data.length < 1)
                            scope.$parent.quoteViewModel.dataNotFound = true;
                        else
                            scope.$parent.quoteViewModel.dataNotFound = false;

                        scope.$parent.msmcData = null;

                        response($.map(data, function (item) {
                            return {
                                label: item.Keyword,
                                value: item.ClassDescKeywordId + "-" + item.ClassDescriptionId + "-" + item.ClassCode + "-" + (item.DirectOK || '')
                            };
                        }));
                        var end = performance.now();
                        //var diff = end - apirespRec;
                        //console.log("data processed in client side recieved in :" + diff + " ms");
                    });


                },
                focus: function (event, ui) {
                    $timeout(function () {
                        element.val(ui.item.label);
                        scope.myModelId.selected = ui.item.value;
                        scope.$parent.quoteViewModel.selectedClassDescKeyId = scope.myModelId.selected.split("-")[0];
                        scope.$parent.quoteViewModel.selectedClassDescId = scope.myModelId.selected.split("-")[1];
                        scope.$parent.quoteViewModel.selectedClassCode = scope.myModelId.selected.split("-")[2];
                        scope.$parent.quoteViewModel.selectedClassDirectSales = scope.myModelId.selected.split("-")[3];
                        scope.$parent.quoteViewModel.businessName = ui.item.label;
                    });
                    return false;
                },
                select: function (event, ui) {
                    $timeout(function () {
                        element.val(ui.item.label);
                        scope.myModelId.selected = ui.item.value;
                        scope.$parent.quoteViewModel.selectedClassDescKeyId = scope.myModelId.selected.split("-")[0];
                        scope.$parent.quoteViewModel.selectedClassDescId = scope.myModelId.selected.split("-")[1];
                        scope.$parent.quoteViewModel.selectedClassCode = scope.myModelId.selected.split("-")[2];
                        scope.$parent.quoteViewModel.selectedClassDirectSales = scope.myModelId.selected.split("-")[3];
                        scope.$parent.quoteViewModel.businessName = ui.item.label;
                    });
                    return false;
                },
                change: function (event, ui) {
                    if (ui.item === null) {
                        scope.myModelId.selected = null;
                    }
                },
                //GUIN-465 To Enable Item selection on first tap in iphone safari
                open: function (event, ui) {
                    $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
                }

            }).off('blur').blur(function (event, ui) {
                $timeout(function () {
                    var selval = element.val();
                    for (var i = 0; i < scope.businessnames.length; i++) {
                        if (scope.businessnames[i].Keyword.toUpperCase().trim() == selval.toUpperCase().trim()) {
                            element.autocomplete("close");
                            scope.$parent.quoteViewModel.selectedClassDescKeyId = scope.businessnames[i].ClassDescKeywordId;
                            scope.$parent.quoteViewModel.selectedClassDescId = scope.businessnames[i].ClassDescriptionId;
                            scope.$parent.quoteViewModel.selectedClassCode = scope.businessnames[i].ClassCode;
                            scope.$parent.quoteViewModel.selectedClassDirectSales = scope.businessnames[i].DirectOK;
                            scope.$parent.quoteViewModel.businessName = scope.businessnames[i].Keyword;
                            scope.$parent.quoteViewModel.searchOptionNotSelected = false;
                        }
                    }

                });

            });
        }
    }]);
})();


