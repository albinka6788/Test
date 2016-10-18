
(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('zipValidationControl', ['coreUtils', 'restServiceConsumer', 'loadingService', '$timeout', zipValidationControlFn]);

    //Comment : Here SaveForLater div control directive implementation
    function zipValidationControlFn(coreUtilityMethod, restServiceProvider, loadingService, timeout)
    {
        //Comment : Here Define directive related all action implementation in DirectiveController iteself
        //var linkFunction = ;

        return {
            restrict: 'A',
            require: '?ngModel',
            scope: { businessInfoVm: '=vm' },
            link: function (scope, elem, attr, ctrl) {
                
                //Comment : Here check account existance on Element blur
                elem.on('keydup paste input', function (event)
                {
                    //RESET to default
                    //elem.removeClass("btn-loading");
                    coreUtilityMethod.controlLevelLoader(elem, false);

                    scope.businessInfoVm.lobId = '';
                    scope.businessInfoVm.lobStatus = '';
                    scope.businessInfoVm.invalidLobId = 0;
                    scope.businessInfoVm.lobList = [];
                    //scope.businessInfoVm.cityList = [];
                    scope.businessInfoVm.stateCode = '';

                    //default validaty is TRUE
                    ctrl.$setValidity('invalidZipCode', true);

                    //Comment : Here in bigning must check only when Digit is pressed
                    //var keyCode = $(event).attr('keyCode');
                    //if (!(typeof keyCode == 'undefined' || (keyCode >= 48 & keyCode <= 57)))
                    //{
                    //    return;
                    //}

                    //Comment : Here get control value
                    var zipCode = elem.val();
                    //zipCode = coreUtilityMethod.ConvertToNumeric(coreUtilityMethod.Trim(zipCode));

                    var transformedInput = zipCode.replace(/[^0-9]/g, '');
                    if (transformedInput != zipCode)
                    {
                        ctrl.$setViewValue(transformedInput);
                        ctrl.$render();
                    }

                    //Comment : Here if empty email then break execution
                    if (zipCode.length <= 4)
                    {
                        return;
                    };

                    //Comment : Here if have length == 5 then only 
                    if(typeof zipCode != 'undefined' & zipCode.length == 5)
                    {                        
                        //Comment : Here show control level loader/progress
                        //elem.addClass("btn-loading");
                        coreUtilityMethod.controlLevelLoader(elem, true);
                        //scope.businessInfoVm.showCity = false;

                        //ctrl.$asyncValidators.validateZip = function (modelValue, viewValue)
                        //{
                            var promise = restServiceProvider.validateZipCodeLobs(zipCode);

                            promise
                                .then(function (data) {
                                    timeout(function () {
                                        if (data != null) {

                                            //Comment : Here if taxid-number valid then 
                                            if (data.resultStatus == 'OK')
                                            {
                                                //var result = JSON.parse(data);  // no need to parse Already Json format

                                                var businessInfo = data.busInfoVM;
                                                //console.log(JSON.parse(businessInfo.LobList));
                                                //console.log(JSON.parse(businessInfo.CityList));

                                                //get/set LOB lists
                                                var lobList = businessInfo.LobList;
                                                scope.businessInfoVm.lobList = (lobList != null & lobList != 'undefined' & lobList != '') ? JSON.parse(lobList) : [];
                                                
                                                //get/set LOB lists
                                                //var cityList = businessInfo.CityList;
                                                //scope.businessInfoVm.cityList = (cityList != null & cityList != 'undefined' & cityList != '') ?
                                                //    JSON.parse(cityList) : [];

                                                //set state code into scope
                                                scope.businessInfoVm.stateCode = businessInfo.StateCode;

                                                //Set show-city flag in VM
                                                //scope.businessInfoVm.showCity = (typeof cityList != 'undefined' & JSON.parse(cityList).length > 1) ? true : false;
                                                //if (scope.businessInfoVm.cityList.length == 1)
                                                //{
                                                //    scope.businessInfoVm.selectedCity =  scope.businessInfoVm.cityList[0];  //Select first item
                                                //}

                                                //it means valid zip code found
                                                ctrl.$setValidity('invalidZipCode', true);
                                            }
                                            else if (data.resultStatus == 'NOK') {
                                                ctrl.$setValidity('invalidZipCode', false);
                                            }
                                        }
                                    });
                                })
                                .catch(function (exception) {
                                    //console.log(exception.toUpperCase());
                                })
                                .finally(function () {
                                    //Comment : Here hide control level loader/progress
                                    //elem.removeClass("btn-loading");
                                    coreUtilityMethod.controlLevelLoader(elem, false);
                                });
                        //};
                    }

                });
            }
        };

    }

})();