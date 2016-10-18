(function () {
    var dependencies = [
            "ngRoute",
            'BHIC.Dashboard.Controllers',
            'BHIC.Dashboard.Directives',
            'BHIC.Directives',
            'BHIC.Dashboard.Services',
            'BHIC.Services'
    ];

    login = angular.module("Login", dependencies);
    login
        .config(['$routeProvider', '$locationProvider', '$provide', function ($routeProvider, $locationProvider, $provide) {

            $routeProvider.when('/', {
                title: "Your Account",
                redirectTo: baseUrl + 'Login/Login',
                controller: 'loginController'
            })
            .when('/Login', {
                title: "Your Account",
                templateUrl: baseUrl + 'Login/Login',
                controller: 'loginController'
            })
            .when('/Login/:Key', {
                templateUrl: function (params) { return baseUrl + 'Login/GetEmailVerified?key=' + params.Key },
                controller: 'loginController'
            })
            .when('/Registration', {
                title: "Account Registration",
                templateUrl: baseUrl + 'Registration/Registration',
                controller: 'accountRegistrationController'
            })
            .when('/ForgotPassword', {
                title: "Forgot Password",
                templateUrl: baseUrl + 'ForgotPassword/ForgotPassword',
                controller: 'forgotPasswordController'
            })
            .when('/ResetPassword', {
                title: "Reset Password",
                templateUrl: baseUrl + 'ResetPassword/ResetPassword',
                controller: 'resetPasswordController',
                resolve: {
                    emailId: ['unauthorisedUserWrapperService', '$location', function (unauthorisedUserWrapperService, $location) {
                        var wrapper = { "method": "GetEmail", "queryString": $location.search().queryKey };
                        return unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper);
                    }]
                }
            })
            .when('/ReportClaimDashboard', {
                title: "Report a Claim",
                templateUrl: baseUrl + 'ReportClaim/ReportClaimDashboard',
                controller: 'reportClaimDashboardController'
            })
            .when('/ReportClaimFromHome', {
                title: "Report a Claim WC",
                templateUrl: baseUrl + 'ReportClaim/ReportClaimFromHome',
                controller: 'reportClaimFromHomeController'
            })
            .when('/BopReportClaimFromHome', {
                title: "Report a Claim BOP",
                templateUrl: baseUrl + 'ReportClaim/ReportClaimFromHome',
                controller: 'reportClaimFromHomeController'
            })
            .when('/Error', {
                title: "Error",
                templateUrl: baseUrl + 'Error/OnExceptionError',
                controller: 'errorController'
            })
            .when('/IsPCSessionAlive', {
                templateUrl: baseUrl + 'Login/IsPCSessionAlive',
                controller: ''
            })


            //.when('/GlobalSignOut', {            
            //    templateUrl: baseUrl + 'Login/GlobalSignOutPartial',
            //    controller: 'headerController'
            //})
            .otherwise({
                    title: "Your Account",
                     redirectTo: '/Login'
                });

            //$locationProvider.html5Mode(false).hashPrefix('');

        }])

        .config(['$provide', function ($provide) {
            //Comment : Here Added interceptor over all http calls to filter call response 
            // Tack on any additional properties that you want to be part of an injectable config object.
            $provide.constant('appConfig', {
                appCdnDomain: appCdnDomain,
                appBaseDomain: appBaseDomain
            });
        }])

        .run(['$rootScope', '$templateCache', '$http', '$route', '$location', 'loadingService', function ($rootScope, $templateCache, $http, $route, $location, loadingService) {
            //append anti forgery token in header, on init.
            $http.defaults.headers.common['X-XSRF-Token'] = $('input[name="__RequestVerificationToken"]').val();

            //Comment : Here every template load clear cached template 
            $rootScope.$on('$viewContentLoaded', function () {
                $templateCache.removeAll();
            });

            //Comment : Here it will set the Active css on link based on routing
            $rootScope.$on('$routeChangeStart', function (event, current, previous) {
                var routeText = $location.$$path.replace(/\//g, '');
                if (routeText.indexOf("ReportClaim") < 0) {
                    topmenustate("YourAccount");
                }
                else {
                    topmenustate("ReportClaim");
                }
            });

            $rootScope.$on("$routeChangeSuccess", function (event, current, previous) {
                $('#ui-datepicker-div').css('display', 'none');
                $('html, body').animate({ scrollTop: 0 }, 800);

                //Change page title, based on Route information
                if ($route.current.title != undefined && $route.current.title !== '') {
                    document.title = $route.current.title + " - Cover Your Business";
                }
                else {
                    document.title = "Cover Your Business";
                }
            });

            if ($location.search().queryKey != null)
                $location.path("/ResetPassword").replace();

        }]);
}());
