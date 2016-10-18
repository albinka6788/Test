(function () {
    var dependencies = [
            "ngRoute",
            'ngSanitize',
            'BHIC.LandingPage.Controllers',
            'BHIC.LandingPage.Directives',
            'BHIC.Directives',
            'BHIC.LandingPage.Services',
            'BHIC.Services'
    ];

    landing = angular.module("Landing", dependencies);

    landing.filter('offset', function () {
        return function (input, start) {
            start = parseInt(start, 10);
            return input.slice(start);
        };
    });

    landing.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        var baseUrl = "LandingPage/";

        $routeProvider.when('/', {
            redirectTo: baseUrl + 'Login/Login',
            controller: 'loginController'
        })
        .when('/Login', {
            templateUrl: baseUrl + 'Login/Login',
            controller: 'loginController'
        })
        .when('/LandingPages', {
            templateUrl: baseUrl + 'Login/LandingPages',
            controller: 'landingPageController',
            resolve: {
                resultSet: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetAllLandingPages" });
                }]
            }
        })
        .when('/AddLandingPage', {
            templateUrl: baseUrl + 'Login/InsertOrUpdateLandingPage',
            controller: 'insertOrUpdateLandingPageController',
            resolve: {
                resultSet: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetDefaultLists" });
                }]
            }
        })
        .when('/EditLandingPage/:TokenId', {
            templateUrl: function (params) { return baseUrl + 'Login/InsertOrUpdateLandingPage?TokenId=' + params.TokenId },
            controller: 'insertOrUpdateLandingPageController',
            resolve: {
                resultSet: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetDefaultLists" });
                }]
            }
        })
        .when('/TemplateBackground', {
            templateUrl: baseUrl + 'Login/AddTemplateBcakground',
            controller: 'templateBackgroundController',
            resolve: {
                resultSet: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetDefaultLists" });
                }]
            }
        })

        .when('/Ads/:TokenId', {
            templateUrl: function (params) { return baseUrl + 'Templates/Templates?TokenId=' + params.TokenId },
            controller: 'templateController',
            resolve: {
                resultSet: ['authorisedUserWrapperService', '$route', function (authorisedUserWrapperService, $route) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetTemplateByTokenId", "queryString": $route.current.params.TokenId });
                }]
            }
        })

        .when('/PreViewTemplate/:Template', {
            templateUrl: function (params) { return baseUrl + 'Templates/PreViewTemplate?template=' + params.Template },
            controller: 'previewLandingPageController'
        })

        .when('/Logout/', {
            templateUrl: baseUrl + 'Login/Logout',
            controller: 'previewLandingPageController'
        })

            .when('/GetLandingPageDetailsByTokenId/:TokenId', {
                templateUrl: baseUrl + 'Login/GetLandingPageDetailsByTokenId',
                controller: 'loginController',
                resolve: {
                    resultSet: ['authorisedUserWrapperService', '$route', function (authorisedUserWrapperService, $route) {
                        return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetTemplateByTokenId", "queryString": $route.current.params.TokenId });
                    }]
                }
            })

        .otherwise({ redirectTo: '/Login' });


    }])
  .run(['$rootScope', '$http', '$route', '$location', 'loadingService', '$window', '$templateCache', 'sharedUserService',
      function ($rootScope, $http, $route, $location, loadingService, $window, $templateCache, sharedUserService) {
          //append anti forgery token in header, on init.
          $http.defaults.headers.common['X-XSRF-Token'] = $('input[name="__RequestVerificationToken"]').val();

          //Comment : Here every template load clear cached template 
          $rootScope.$on('$viewContentLoaded', function () {
              $templateCache.removeAll();
              if (window.localStorage.getItem('isSessionCreated') == null && $location.url() != '/Login') {
                  $window.ga('send', 'pageview', { page: 'LandingPage/#' + $location.url() });
              }

          });

          $rootScope.$on('$routeChangeStart', function (event, current, previous) {
              loadingService.showLoader();
              var user = sharedUserService.getUser("user")
              if (!window.localStorage.getItem('isSessionCreated') && user == null) {
                  if (current.params.TokenId === undefined)
                      $location.path("/Login");
                  else
                      $location.path("/Ads/" + current.params.TokenId).replace();
              }
          });

      }]);
}());
