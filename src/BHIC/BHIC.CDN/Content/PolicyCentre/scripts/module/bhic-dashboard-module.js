(function () {
    var dependencies = [
            "ngRoute",
            'BHIC.Dashboard.Controllers',
            'BHIC.Dashboard.Directives',
            'BHIC.Directives',
            'BHIC.Dashboard.Services',
            'BHIC.Services'
    ];


    dash = angular.module("BHIC.Dashboard", dependencies);
    dash.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/', {
            title: "Your Policies",
            redirectTo: baseUrl + 'YourPolicies/YourPolicies',
            controller: ''
        })
        // Dashboard Headers
         .when('/YourPolicies', {
             title: "Your Policies",
             templateUrl: baseUrl + 'YourPolicies/YourPolicies',
             controller: 'yourPoliciesController',
             resolve: {
                 yourPoliciesData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetYourPolicies" });
                 }]
             }
         })
         .when('/ContactInfo', {
             title: "Contact Information",
             templateUrl: baseUrl + 'ContactInfo/ContactInfo',
             controller: 'contactInfoController'
         })
         .when('/SavedQuotes', {
             title: "Saved/New Quotes",
             templateUrl: baseUrl + 'SavedQuotes/SavedQuotes',
             controller: 'savedQuotesController',
             resolve: {
                 savedQuotesData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetQuotes" });
                 }]
             }
         })
        .when('/Resources', {
            title: "Resources",
            templateUrl: baseUrl + 'HelpfulResources/Resource',
            controller: 'helpfulResourcesController'
        })
        .when('/EmployerNotices', {
            title: "Employer Posting Notices",
            templateUrl: baseUrl + 'HelpfulResources/EmployerNotices',
            controller: 'helpfulResourcesController'
        })
        .when('/Connecticut', {
            title: "Connecticut MCP",
            templateUrl: baseUrl + 'HelpfulResources/Connecticut',
            controller: 'helpfulResourcesController'
        })
        .when('/TexasMPN', {
            title: "Texas MPN",
            templateUrl: baseUrl + 'HelpfulResources/TexasMPN',
            controller: 'helpfulResourcesController'
        })
        .when('/CAClaims', {
            title: "CA Claims Information",
            templateUrl: baseUrl + 'HelpfulResources/CAClaims',
            controller: 'helpfulResourcesController'
        })
         // Side menu Headers
         .when('/PolicyInformation', {
             title: "Policy Information",
             templateUrl: baseUrl + 'PolicyInformation/PolicyInformation',
             controller: 'policyInformationController',
             resolve: {
                 policyInformationData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetPolicyDetailByPolicyNumber" });
                 }]
             }
         })
         .when('/PolicyInformation/:transactionCode', {
             title: "Policy Information",
             templateUrl: function (params) { return baseUrl + 'PolicyInformation/PolicyInformation?transactionCode=' + params.transactionCode },
             controller: 'policyInformationController',
             resolve: {
                 policyInformationData: ['authorisedUserWrapperService', '$route', function (authorisedUserWrapperService, $route) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetPolicyWithTransactionCode", "queryString": $route.current.params.transactionCode });
                 }]
             }
         })
         .when('/MakePayment', {
             title: "Make Payment",
             templateUrl: baseUrl + 'MakePayment/MakePayment',
             controller: 'policyPaymentController',
             resolve: {
                 policyPremiumData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetBillingDetails" });
                 }]
             }
         })
         .when('/MakePayment/:CYBKey', {
             title: "Make Payment",
             templateUrl: function (params) { return baseUrl + 'MakePayment/MakePayment?CYBKey=' + params.CYBKey },
             controller: 'policyPaymentController',
             resolve: {
                 policyPremiumData: ['authorisedUserWrapperService', '$route', function (authorisedUserWrapperService, $route) {
                     return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetBillingDetails", "queryString": $route.current.params.CYBKey });
                 }]
             }
         })
        .when('/PhysicianPanel', {
            title: "Physician Panel",
            templateUrl: baseUrl + 'PC/PhysicianPanel/PhysicianPanel',
            controller: 'physicianPanelController',
            resolve: {
                physicianPanelData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetPhysicianDocument" });
                }]
            }
        })
        .when('/CancelPolicy', {
            title: "Cancel Policy",
            templateUrl: baseUrl + 'CancelPolicy/CancelPolicy',
            controller: 'cancelPolicyController',
            resolve: {
                contactData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetPolicySpecificUserDetail" });
                }]
            }
        })
        .when('/PolicyDocument', {
            title: "Policy Documents",
            templateUrl: baseUrl + 'PolicyDocument/PolicyDocument',
            controller: 'policyDocumentController',
            resolve: {
                policyDocumentData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetPolicyDocument" });
                }]
            }
        })
        .when('/ReportClaim', {
            title: "Report a Claim",
            templateUrl: baseUrl + 'ReportClaim/ReportClaim',
            controller: 'reportClaimController'
        })
        .when('/ReportClaimFromHeader', {
            title: "Report a Claim",
            templateUrl: baseUrl + 'ReportClaim/ReportClaimFromHeader',
            controller: 'reportClaimController'
        })
        .when('/RequestCertificate', {
            title: "Request a Certificate",
            templateUrl: baseUrl + 'RequestCertificate/RequestCertificate',
            controller: 'requestCertificateController',
            resolve: {
                certificatesData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetCertificateofInsurance" })
                }]
            }
        })
        .when('/RequestPolicyChange', {
            title: "Policy Change",
            templateUrl: baseUrl + 'RequestPolicyChange/RequestPolicyChange',
            controller: 'requestPolicyChangeController',
            resolve: {
                policyChangeOptionsData: ['authorisedUserWrapperService', function (authorisedUserWrapperService) {
                    return authorisedUserWrapperService.AuthorisedUserApiCall({ "method": "GetLobPolicyChangeOptions" })
                }]
            }
        })
        .when('/ChangePassword', {
            title: "Change Password",
            templateUrl: baseUrl + 'ChangePassword/ChangePassword',
            controller: 'changePasswordController'
        })
        .when('/EditContactInfo', {
            title: "Edit Contact Information",
            templateUrl: baseUrl + 'EditContactInfo/EditContactInfo',
            controller: 'editContactInfoController'
        })
        .when('/EditContactAddress', {
            title: "Edit Contact Address",
            templateUrl: baseUrl + 'EditContactAddress/EditContactAddress',
            controller: 'editContactAddressController'
        })
         .when('/SessionExpired', {
             title: "Session Expired",
             templateUrl: baseUrl + 'SessionExpired/SessionExpired',
             controller: 'sessionController'
         })
        .when('/GlobalSignOut', {
            title: "Sign Out",
            templateUrl: baseUrl + 'Login/GlobalSignOutPartial',
            controller: 'headerController'
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
        .when('/RequestLossRun', {
            title: "Loss Runs",
            templateUrl: baseUrl + 'RequestLossRun/RequestLossRun',
            controller: 'requestLossRunController'
        })
        .when('/RestrictedAccess', {
            title: "Restricted Access",
            templateUrl: baseUrl + 'Error/OnRestrictedAccess',
            controller: 'errorController'
        })
         .when('/UploadDocuments', {
             title: "Upload Documents",
             templateUrl: baseUrl + 'UploadDocuments/UploadDocuments',
             controller: 'uploadDocumentsController'
         })
        .otherwise({
            title: "Your Policies",
            redirectTo: '/YourPolicies'
        });

        $locationProvider.html5Mode(false).hashPrefix('');

    }])
      .run(['$rootScope', '$templateCache', '$http', 'loadingService', '$route', '$location', '$window', '$timeout', 'loadingService', 'sharedUserService', function ($rootScope, $templateCache, $http, loadingService, $route, $location, $window, $timeout, loadingService, sharedUserService) {
          //append anti forgery token in header, on init.
          $http.defaults.headers.common['X-XSRF-Token'] = $('input[name="__RequestVerificationToken"]').val();
          //Comment : Here every template load clear cached template 
          $rootScope.$on('$viewContentLoaded', function () {
              $templateCache.removeAll();
          });

          var isSessionTimeout = false;
          var lastDigestRun = new Date().getTime();

          $rootScope.$watch(function detectIdle() {
              var now = new Date();
              if (now - lastDigestRun > sessionTimeOut * 60 * 1000) {
                  isSessionTimeout = true;
                  nosidemenu(true);
                  $('.rootText').text("Your timeout information...");
                  $location.path("/SessionExpired");
              }
          });
          $rootScope.$on('$routeChangeStart', function (event, current, previous) {
              loadingService.showLoader();

              var user = sharedUserService.getUser("user");
              if (user != null) {
                  var clientUsr = JSON.parse(user);
                  var serverUsr = JSON.parse($window.localStorage.getItem("user"));
                  if (clientUsr.Email != serverUsr.Email) {
                      window.location.href = baseUrl;
                  }
              }

              if ($window.localStorage.getItem('isSessionCreated') && !isSessionTimeout) {
                  var routeText = $location.$$absUrl.split('#')[1];
                  if (routeText != undefined) {
                      routeText = (routeText.split("/").length == 4) ? "YourPolicies" : routeText;
                      currenturl = routeText;
                      if (routeText.indexOf('/') == 0) {
                          routeText = routeText.split('/')[1];
                      }

                      if (routeText.indexOf('/') > 0) {
                          routeText = routeText.split('/')[0];
                      }

                      //routeText = routeText.replace(/\//g, '');

                      if (routeText != 'EditContactAddress' && routeText != 'EditContactInfo') {
                          $window.localStorage.removeItem('editcontactInfo');
                      }

                      if (routeText == 'EditContactAddress' && $window.localStorage.getItem('editcontactInfo') == null) {
                          routeText = 'EditContactInfo'
                      }

                      var menuName = window.name.split("CYB")[2];
                      var isValidRequest = true;
                      if (window.name != '') {
                          if (menuName == 'Active Soon') {
                              if (routeText == "RequestCertificate" || routeText == "ReportClaim" || routeText == "CancelPolicy") {
                                  isValidRequest = false;
                              }
                          } else if (menuName.indexOf('Expired') > -1) {
                              if (routeText == "MakePayment" || routeText == "RequestCertificate" || routeText == "RequestPolicyChange" || routeText == "EditContactAddress" || routeText == "CancelPolicy") {
                                  isValidRequest = false;
                              }
                          } else if (menuName.indexOf('Pending Cancellation') > -1 || menuName.indexOf('Cancelled') > -1) {
                              if (routeText == "RequestCertificate" || routeText == "CancelPolicy") {
                                  isValidRequest = false;
                              }
                          } else if (menuName.indexOf('No Coverage') > -1) {
                              if (routeText == "MakePayment" || routeText == "PolicyDocument" || routeText == "RequestCertificate" || routeText == "RequestPolicyChange" || routeText == "EditContactAddress" || routeText == "ReportClaim" || routeText == "ReportClaimFromHeader" || routeText == "PhysicianPanel" || routeText == "CancelPolicy") {
                                  isValidRequest = false;
                              }

                          }

                      }

                      if (isValidRequest) {

                          if (routeText == 'SavedQuotes') {
                              $("div#headergeico").parent().remove();
                          }

                          $('.rootText').text(PageDictionory[routeText]);
                          if (user != "undefined" && user != null && routeText != "") {
                              if ("YourPolicies" == routeText || "ContactInfo" == routeText || "SavedQuotes" == routeText || "ChangePassword" == routeText || 'ReportClaimFromHeader' == routeText || 'Resources' == routeText || 'EmployerNotices' == routeText || 'Connecticut' == routeText || 'TexasMPN' == routeText || 'CAClaims' == routeText) {
                                  nosidemenu(true);
                              }
                              else {
                                  if (window.name == "") {
                                      if ("YourPolicies" == routeText || "ContactInfo" == routeText || "SavedQuotes" == routeText || "ChangePassword" == routeText || 'ReportClaimFromHeader' == routeText || 'EditContactInfo' == routeText || 'EditContactAddress' == routeText || 'GlobalSignOut' == routeText || 'IsPCSessionAlive' == routeText || 'Resources' == routeText || 'EmployerNotices' == routeText || 'Connecticut' == routeText || 'TexasMPN' == routeText || 'CAClaims' == routeText) {
                                          nosidemenu(true);
                                          $location.path("/" + routeText);
                                      }
                                      else
                                          $location.path("/YourPolicies");
                                  }
                                  else {
                                      if (sideMenuReload) {
                                          SetNavigation(routeText);
                                      }
                                      nosidemenu(false);
                                      //$(".sidebar-menu .policyNumber").html("(" + window.name.split("CYB")[1] + ")");
                                  }
                              }
                              if (routeText == "YourPolicies" || routeText == "ContactInfo" || routeText == "SavedQuotes" || routeText == "ReportClaimFromHeader" || routeText == "Login" || routeText == "Resources" || routeText == "EmployerNotices" || routeText == "Connecticut" || routeText == "TexasMPN" || routeText == "CAClaims") {
                                  topmenustate(routeText);
                              }
                              else {
                                  sidemenustate(routeText);
                              }
                          }
                      } else {
                          loadingService.hideLoader();
                          $location.path("/YourPolicies");
                      }
                  }
              }
              else {
                  if (!$window.localStorage.getItem('isSessionCreated') && user == null)
                      window.location.href = baseUrl;
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

          $rootScope.$on("$routeChangeError", function (event, current, previous) {
              window.location.href = baseUrl;
          });

      }]);
}());
