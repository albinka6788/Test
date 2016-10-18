(function () {
    'use strict';

    //currency control directive definition
    angular.module('bhic-app.wc').directive('autocomplete', ['bhic-app.services.restServiceConsumer', '$timeout', function (restServiceConsumer, $timeout) {
        return function (scope, element, attrs) {
            scope.myModel = null;
            scope.myModelId = [];
            scope.isDisabled = true;

            scope.validateModel = function () {
                scope.isDisabled = ($scope.myModelId === null);
            };
            element.autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var queryString = "?searchString=" + request.term;
                    if (!isNaN(request.term)) {
                        queryString = "?classDescriptionId=" + request.term;
                    }
                    var url = "/WcHome/SearchBusiness" + queryString;
                    restServiceConsumer.getData(url).then(function (data) {
                        response(data);
                        $('span[role="status"]').hide();
                    });


                },
                focus: function (event, ui) {
                    //Comment : Here reset every time
                    scope.$parent.selectedClassDescKeyId = null;

                    element.val(ui.item.Keyword);
                    scope.$parent.selectedClassDescKeyId = ui.item.ClassDescKeywordId;
                    scope.$apply;
                    return false;
                },
                select: function (event, ui) {
                    scope.myModelId.selected = ui.item.ClassDescKeywordId;
                    scope.$parent.selectedClassDescKeyId = ui.item.ClassDescKeywordId;
                    scope.$apply;
                    return false;
                },
                change: function (event, ui) {
                    if (ui.item === null) {
                        scope.myModelId.selected = null;
                    }
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.Keyword + "</a>")
                    .appendTo(ul);
            };
            element.bind('blur', function () {
                if (scope.$parent.selectedClassDescKeyId == null)
                    console.log('search id not found!');
                else
                    console.log(':) search id found');

                scope.$apply(attrs.blur);
            });
        }
    }]);
})();


