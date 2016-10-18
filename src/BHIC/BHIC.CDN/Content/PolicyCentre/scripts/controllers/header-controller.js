(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('headerController', ['$scope', '$http', '$rootScope', '$location', '$window', 'loadingService', 'authorisedUserWrapperService', 'sharedUserService', headerControllerFn]);
    function headerControllerFn($scope, $http, $rootScope, $location, $window, loadingService, authorisedUserWrapperService, sharedUserService) {

        // Controler funciton Start  write any methods/functions after this line.
        // Define $scope methods variables etc.
        var _self = this;
        loadingService.hideLoader();

        $scope.logout = function (ppLogoutUrl)
        {
            //debugger;
            loadingService.showLoader();
            var wrapper = { "method": "SignOut" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (signOut)
            {
                if (signOut)
                {                   
                    sharedUserService.setUser(null);
                    window.CacheControl = "no-cache";                    
                    $window.localStorage.clear();                    

                    //Comment : Here request for PP sing-out 
                    $http.get(ppLogoutUrl,
                    {
                        headers:
                            {
                                'access-control-allow-origin': '*',
                                'access-control-allow-methods': "GET, POST, PUT, DELETE, OPTIONS",
                                'access-control-allow-headers': "content-type, accept",
                                'access-control-max-age': 60, // Seconds.
                                'content-length': 0,
                                'content-type': 'application/json'
                            }
                    })
                    .then(function (res)
                    {                        
                        //Comment : Here user has sign-out successfully from PP also then redirect to PC login page
                        window.location.replace(OriginLocation() + baseUrl);

                    }, function (res)
                    {                        
                        //console.log('Error response came from PP !!');                        
                        //console.log(res);
                    });
                } else {
                    loadingService.hideLoader();
                    //alert('TODO : Signout failed due to server exception');
                }

            });
        }

        $scope.globalogout = function () {
            
            loadingService.showLoader();
            var wrapper = { "method": "GlobalSignOut" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (signOut) {
                if (signOut) {
                    //loadingService.hideLoader();
                    sharedUserService.setUser(null);
                    window.CacheControl = "no-cache";                    
                    $window.localStorage.clear();
                    window.location.replace(OriginLocation() + baseUrl);
                } else {
                    //loadingService.hideLoader();
                    //alert('TODO : Signout failed due to server exception');
                }

            });
        }

        // Controler funciton End Please dont write any methods/functions after this line.

    }
})();
