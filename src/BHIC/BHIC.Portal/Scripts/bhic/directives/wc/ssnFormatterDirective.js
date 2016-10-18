angular.module('bhic-app.wc.purchaseGet').directive('ssnFormat', ['$timeout', function ($timeout) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {

            ngModel.$parsers.unshift(function (viewValue) {
                $timeout(function () {
                    scope.purchaseModel.businessInfo.taxIdOrSSN = viewValue;
                })
                if (viewValue.split('_').length > 1) {
                    ngModel.$setValidity('minlength', false);
                }

                else
                    ngModel.$setValidity('minlength', true);
            });
        }
    }
}]);

