(function () {
    'use strict';
    angular.module('BHIC.LandingPage.Directives').directive('dcar', ['$compile','$timeout', dateControlFn]);
    function dateControlFn($compile, $timeout) {
        var inItCarouselItems = function (scope, element, attrs) {
            var content = "";
            $.each(scope.content, function (ind, obj) {
                content += '<div><h1 class=mb0 >' + obj + ' </h1></div>';
            });
            element.append(content);
        };

        var inItCarouselLoad = function (scope, element, attrs) {
            $timeout(function () {
                $(element).owlCarousel({
                    autoPlay: 7000,
                    singleItem: true,
                    autoHeight: true,
                    pagination: false,
                    transitionStyle: "backSlide"
                });
            }, 70);
        };

        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            compile: function compile(tElement, tAttrs, transclude) {
                return {
                    pre: inItCarouselItems,
                    post: inItCarouselLoad
                }
            }
        };
    };
})();
