(function () {

    var dependencies = ['ngRoute', 'BHIC.Controllers', 'BHIC.Services', 'BHIC.Directives'];

    angular.module('BHIC', dependencies)
    .run(['$rootScope', '$http', function ($rootScope, $http) {

        //append anti forgery token in header, on init.
        $http.defaults.headers.common['X-XSRF-Token'] = $('input[name="__RequestVerificationToken"]').val();

    }])
    .config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
        //Comment : Here Added interceptor over all http calls to filter call response 
        // Tack on any additional properties that you want to be part of an injectable config object.
        $provide.constant('appConfig', {
            appBaseDomain: appBaseDomain,
            appCdnDomain: appCdnDomain,
            loggedUserEmailId: loggedUserEmailId,
            hoursBusiness: hoursBusiness,
            quoteId: '',
            appBaseUrl: appBaseUrl,
            prospectInfoEmailId: prospectInfoEmailId
        });
        $routeProvider
            .when('/products', {
                title: 'Exposure',
                templateUrl: '/PurchasePath/Home/SiteContents',
            });

        //Comment : Here Rouring related blocks
        /*
        $routeProvider
        .when('/',
        {
            redirectTo: '/Index'
        })
        .when('/Home',
        {
            templateUrl: '/Index'
        })
        .when('/Home/:quoteId',
        {
            templateUrl: function (params) { debugger; return '/Home/Index?quoteId=' + params.quoteId || null; },
            controller: 'landingPageController',
            controllerAs: ''
        })*/

    }]);

})();