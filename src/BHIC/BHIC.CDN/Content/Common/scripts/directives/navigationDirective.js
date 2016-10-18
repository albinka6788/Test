(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives')
        .directive('navigationDivControl', ['coreUtils', '$location', navigationDivControlFn]);

    function navigationDivControlFn(coreUtilityMethod, location)
    {        
        //Comment : Here Define directive related all action implementation in DirectiveController iteself
        var navigationController = ['$scope', function (scope)
        {            
            scope.navigationMessageOK = function ()
            {
                //Navigate to target location                
                location.path(navigation.path);
                coreUtilityMethod.resetNavigation();
                //navigation = {};
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id= navigationModel]'), false);
            }

            scope.navigationMessageCancel = function ()
            {
                coreUtilityMethod.resetNavigation();
                //navigation = {};
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id= navigationModel]'), false);
            }
        }];

        return {
            restrict: 'E',
            controller: navigationController,
            template: [
        '<div class="remodal-heading bg-warning">                                                                      ',
        '        <h2>Alert</h2>                                                                             ',
        '    </div>                                                                                         ',
        '    <div class="remodal-content">                                                                  ',
        '        <p class="mb0">Changes made on this page would not be saved unless you fill in the complete information and press continue button on the bottom of the page. Do you want to discard your changes and move to requested screen?</p>     ',
        '    </div>                                                                                         ',
        '    <div class="remodal-footer center">                                                            ',
        '    <input id="btn_ConfirmOk" type="button" value="Yes" class="btn"                     ',
        '                data-ng-click=\'navigationMessageOK()\' />                                         ',
        '    <input id="btn_ConfirmCancel" type="button" value="No" class="btn btn-primary"             ',
        '                data-ng-click=\'navigationMessageCancel()\' />                                     ',
        '</div>                                                                                             ',

            ].join('')
        };
    }

})();