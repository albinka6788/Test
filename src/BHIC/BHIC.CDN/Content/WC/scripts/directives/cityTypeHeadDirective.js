(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('cityAutoComplete', ['restServiceConsumer','$timeout', function (restServiceConsumer,timeout) {

        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                element.autocomplete({
                    minLength: 1,
                    appendTo: element.closest('.form-group'),
                    source: function (request, response) {
                        var data = scope.purchaseQuoteViewModel.purchaseQuotePageDefaults.cityList;
                        if (data.length < 1)
                            scope.dataNotFound = true;
                        else
                            scope.dataNotFound = false;
                        response($.map(data, function (item) {
                            if (item.City.toLowerCase().indexOf(request.term.toLowerCase()) != -1)
                                return {
                                    label: item.City,
                                    value: item.City
                                };
                        }));
                    },
                    //focus: function (event, ui) {
                    //    element.val(ui.item.label);
                    //    return false;
                    //},
                    focus: function (event, ui) {
                        event.preventDefault();
                    },
                    select: function (event, ui) {
                        timeout(function () {
                            element.val(ui.item.label);
                            scope.purchaseQuoteViewModel.businessMailingAddress.city = ui.item.value;
                            ngModel.$setViewValue(ui.item.label);
                            ngModel.$commitViewValue();
                        });
                        return false;
                    },
                    change: function (event, ui) {
                        if (ui.item != null) {
                            scope.purchaseQuoteViewModel.businessMailingAddress.city = ui.item.value;
                        }
                    },
                    //GUIN-465 To Enable Item selection on first tap in iphone safari
                    open: function (event, ui) {
                        $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
                    }

                }).blur(function (event, ui)
                {
                    timeout(function () 
                    {
                        var selval = element.val();
                        var existInList = false;
                        for (var i = 0; i < scope.purchaseQuoteViewModel.purchaseQuotePageDefaults.cityList.length; i++) 
                        {
                            if (scope.purchaseQuoteViewModel.purchaseQuotePageDefaults.cityList[i].City.toUpperCase().trim() == selval.toUpperCase().trim()) 
                            {
                                element.autocomplete("close");
                                existInList = true;
                                scope.purchaseQuoteViewModel.businessMailingAddress.city = selval;
                                break;
                            }
                        }

                        scope.purchaseQuoteViewModel.invalidZipCityState = (!existInList);

                    });
                }).keydown(function (e) {
                    if (e.keyCode == 13) { //if this is enter key
                        timeout(function () {
                            var selval = element.val();
                            var existInList = false;
                            for (var i = 0; i < scope.purchaseQuoteViewModel.purchaseQuotePageDefaults.cityList.length; i++) {
                                if (scope.purchaseQuoteViewModel.purchaseQuotePageDefaults.cityList[i].City.toUpperCase().trim() == selval.toUpperCase().trim()) {
                                    element.autocomplete("close");
                                    existInList = true;
                                    scope.purchaseQuoteViewModel.businessMailingAddress.city = selval;
                                    break;
                                }
                            }
                            scope.purchaseQuoteViewModel.invalidZipCityState = (!existInList);
                        });
                    }
                });
            }
        }
    }]);
})();

