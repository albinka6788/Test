(function () {
    'use strict';

    //valid number directive definition
    angular.module('bhic-app.wc').directive('passwordStrength', [passwordStrengthControlFn]);

    function passwordStrengthControlFn() {
        return {
            restrict: 'A',
            scope: true,
            require: '?ngModel',
            link: function (scope, elem, attrs, control) {
                var strength = {
                    colors: ['#F00', '#F90', '#FF0', '#9F0', '#0F0'],
                    mesureStrength: function (p) {

                        var _force = 0;


                        return _force;

                    },
                    getColor: function (s) {

                        var idx = 0;
                        if (s <= 10) { idx = 0; }
                        else if (s <= 20) { idx = 1; }
                        else if (s <= 30) { idx = 2; }
                        else if (s <= 40) { idx = 3; }
                        else { idx = 4; }

                        return { idx: idx + 1, col: this.colors[idx] };

                    }
                };
            }
        };
    }
})();