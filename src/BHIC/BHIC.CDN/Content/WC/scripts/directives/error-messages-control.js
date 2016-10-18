(function ()
{
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('errorMessagesControl', [errorMessagesControlFn]);

    //Comment : Here SaveForLater div control directive implementation
    function errorMessagesControlFn() {
        return {
            restrict: 'A',
            scope: { errormessagelist: '=' },
            replace: true,
            template: ['<div class="alert alert-danger" data-ng-show="errormessagelist.length >0">                                     ',
                      '      <p class="alert-text wbr" data-ng-repeat="errorMessage in errormessagelist track by $index" data-ng-bind="errorMessage"></p> ',
                      '  </div>'].join('')
        };
    }

})();