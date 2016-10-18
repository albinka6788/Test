(
function () {
    var dependencies =
    [
            "ngRoute",
            'ngSanitize',
            'ngMessages',
            'BHIC.WC.Controllers',
            'BHIC.WC.Directives',
            'BHIC.WC.Services',
            'BHIC.Directives',
            'BHIC.Controllers'
    ];

    angular.module("BHIC.WC", dependencies)
    .config(['$routeProvider', function (routeProvider) {
        routeProvider
            .when('/', {
                title: 'LOB Selection',
                redirectTo: '/GetBusinessInfo'
            })
             .when('/GetBusinessInfo', {
                 title: 'LOB Selection',
                 templateUrl: '/BusinessInfo/GetBusinessInfo',
                 controller: 'businessInfoController',
                 controllerAs: 'businessInfoCtrl'
             })
            .when('/GetBusinessInfo/:quoteId', {
                title: 'LOB Selection',
                templateUrl: function (params) {
                    return '/BusinessInfo/GetBusinessInfo?quoteId=' + params.quoteId || null;
                },
                controller: 'businessInfoController',
                controllerAs: 'businessInfoCtrl'
            })
            .when('/GetExposureDetails', {
                title: 'Exposure',
                templateUrl: '/Exposure/GetExposureDetails',
                controller: 'exposureController'
            })
            .when('/GetExposureDetails/:quoteId', {
                title: 'Exposure',
                //redirectTo: function (params) {
                //    return '/GetBusinessInfo/' + params.quoteId || null;
                //},
                templateUrl: function (params) {
                    return '/Exposure/GetExposureDetails?quoteId=' + params.quoteId || null;
                },
                controller: 'exposureController'
            })
            .when('/Home', {
                title: 'Home',
                templateUrl: '/Home/Index'
            })
            //.when('/CaptureQuote', {
            //    title: 'Exposure',
            //    templateUrl: '/Quote/CaptureQuote',
            //    controller: 'quoteController',
            //    resolve: {
            //        quotePageDefaults: ['quoteService', function (quoteService) {
            //            return quoteService.getQuotePageDefaults();
            //        }],
            //        quoteViewModel: ['quoteService', function (quoteService) {
            //            return quoteService.getQuoteViewModel();
            //        }]
            //    }
            //})
            .when('/GetQuestions', {
                title: 'Questions',
                templateUrl: '/Questions/GetQuestions',
                controller: 'questionsController',
                controllerAs: 'questionsCtrl'
            })
            .when('/GetQuestions/:quoteId', {
                title: 'Questions',
                templateUrl: function (params) { return '/Questions/GetQuestions?quoteId=' + params.quoteId || null; },
                controller: 'questionsController',
                controllerAs: 'questionsCtrl'
            })
            //.when('/ModifyQuote', {
            //    title: 'Exposure',
            //    templateUrl: '/Quote/CaptureQuote',
            //    controller: 'quoteController',
            //    resolve: {
            //        quotePageDefaults: ['quoteService', function quotePageDefaults(quoteService) {
            //            return quoteService.getQuotePageDefaults();
            //        }],
            //        quoteViewModel: ['quoteService', '$route', function quoteViewModel(quoteService, $route) {
            //            return quoteService.getQuoteViewModel($route.current.params.quoteId);
            //        }]
            //    }
            //})
            //.when('/ModifyQuote/:quoteId', {
            //    title: 'Exposure',
            //    templateUrl: function (params) {
            //        return '/Quote/CaptureQuote?quoteId=' + params.quoteId || null;
            //    },
            //    controller: 'quoteController',
            //    resolve: {
            //        quotePageDefaults: ['quoteService', function quotePageDefaults(quoteService) {
            //            return quoteService.getQuotePageDefaults();
            //        }],
            //        quoteViewModel: ['quoteService', '$route', function quoteViewModel(quoteService, $route) {
            //            return quoteService.getQuoteViewModel($route.current.params.quoteId);
            //        }]
            //    }
            //})
            .when('/QuoteSummary', {
                title: 'Quote Summary',
                templateUrl: '/QuoteSummary/GetQuoteSummary',
                controller: 'quoteSummaryController',
                controllerAs: 'quoteSummaryController'
            })
            .when('/QuoteSummary/:quoteId', {
                title: 'Quote Summary',
                templateUrl: function (params) { return '/QuoteSummary/GetQuoteSummary?quoteId=' + params.quoteId || null; },
                controller: 'quoteSummaryController',
                controllerAs: 'quoteSummaryController'
            })
            .when('/ReferralQuote', {
                title: 'Referral Quote',
                templateUrl: '/ReferralQuote/GetReferralQuote',
                controller: 'referralQuoteController',
                controllerAs: 'referralQuoteController'
            })
            .when('/DeclinedQuote', {
                title: 'Decline Quote',
                templateUrl: '/ReferralQuote/DeclinedQuote',
                controller: ''
            })
            .when('/PurchaseQuote', {
                title: 'Business Contact Information',
                templateUrl: '/PurchaseQuote/Quote',
                controller: 'purchaseQuote',
                resolve: {
                    purchaseQuotePageDefaults: ['quotePurchaseService', function (quotePurchaseService) {
                        return quotePurchaseService.getQuotePurchasePageDefaults();
                    }],
                    savedQuotePurchaseData: ['quotePurchaseService', function (quotePurchaseService) {
                        return quotePurchaseService.getSavedQuotePurchaseData();
                    }]
                }
            })
            .when('/ModifyPurchaseQuote/:quoteId',
            {
                title: 'Business Contact Information',
                templateUrl: function (params) {
                    return '/PurchaseQuote/Quote?quoteId=' + params.quoteId || null;
                },
                controller: 'purchaseQuote',
                resolve: {
                    purchaseQuotePageDefaults: ['quotePurchaseService', '$route', function (quotePurchaseService, $route) {
                        return quotePurchaseService.getQuotePurchasePageDefaults($route.current.params.quoteId);
                    }],
                    savedQuotePurchaseData: ['quotePurchaseService', '$route', function (quotePurchaseService, $route) {
                        return quotePurchaseService.getSavedQuotePurchaseData($route.current.params.quoteId);
                    }]
                }
            })
             .when('/BuyPolicy', {
                 title: 'Payment',
                 templateUrl: '/PolicyPurchase/BuyPolicy',
                 controller: 'buyPolicyController'
             })
             .when('/BuyPolicy/:quoteId?', {
                 title: 'Payment',
                 templateUrl: function (params) {
                     return '/PolicyPurchase/BuyPolicy?quoteId=' + params.quoteId || null;
                 },
                 controller: 'buyPolicyController'
             })
             .when('/OrderSummary/:transactionCode', {
                 title: 'Thank You',
                 templateUrl: function (params) {
                     return '/OrderSummary/ConfirmationContent?transactionCode=' + params.transactionCode || null;
                 },
                 controller: 'orderSummaryController',
             })
            .when('/AppError', {
                title: 'Application Error',
                templateUrl: '/Error/OnExceptionError/',
                controller: ''
            })
            //.when('/UnAuthorizePageRequest', {
            //    templateUrl: '/Error/UnAuthorizePageRequest/',
            //    controller: ''
            //})
            .when('/UnAuthorised', {
                title: 'UnAuthorised',
                templateUrl: '/Error/CookieExpiredPartial/',
                controller: ''
            })
            .when('/SessionExpired', {
                title: 'Session Expired',
                templateUrl: '/Error/SessionExpiredPartial/',
                //controller: 'sessionExpiredController',
                //controllerAs: 'sessionExpiredController',
            })
            .otherwise(
            {
                title: 'PageNotFound',
                templateUrl: '/Error/PageNotFoundPartial/',
            });


    }])
    .config(['$httpProvider', function ($httpProvider) {
        //Comment : Here Added interceptor over all http calls to filter call response 
        //If response having SessionExpired headers then don't allow user to take further action in application
        $httpProvider.interceptors.push('sessionRecovererService');
    }])
    .config(['$provide', function ($provide) {
        //Comment : Here Added interceptor over all http calls to filter call response 
        // Tack on any additional properties that you want to be part of an injectable config object.
        $provide.constant('appConfig', {
            appBaseDomain: appBaseDomain,
            appCdnDomain: appCdnDomain,
            loggedUserEmailId: loggedUserEmailId,
            phoneNumber: phoneNumber,
            hoursBusiness: hoursBusiness,
            quoteId: quoteId,
            formGotDirty: false,
            appBaseUrl: appBaseUrl,
            prospectInfoEmailId: prospectInfoEmailId
        });
    }])
    .run(['$rootScope', '$templateCache', '$http', 'loadingService', '$route', '$location', '$window', 'coreUtils', function ($rootScope, $templateCache, $http, loadingService, $route, $location, $window, coreUtilityMethod) {
        //append anti forgery token in header, on init.
        $http.defaults.headers.common['X-XSRF-Token'] = $('input[name="__RequestVerificationToken"]').val();

        //Comment : Here every template load clear cached template 
        $rootScope.$on('$viewContentLoaded', function () {
            $templateCache.removeAll();

            //Comment : Here added SaveForLater dic FEIXED position after html DOM has manipulated
            var sidebar = $('.sidebar'),
           hideText = $('.js-hos'),
           sidebarWidth = sidebar.width(),
           win = $(window);

            win.scroll(function () {
                var winTop = win.scrollTop(),
                  sidebarHeight = sidebar.height(),
                  limit = $('footer').offset().top - sidebarHeight - 60;
                if (winTop > 300) {
                    hideText.slideUp();
                    sidebar.addClass('sticky');
                    sidebar.css({
                        'max-width': sidebarWidth,
                        'top': 20
                    });
                } else {
                    hideText.slideDown();
                    sidebar.removeClass('sticky');
                    sidebar.css('max-width', 'initial');
                }
                if (limit < winTop) {
                    var diff = limit - winTop;
                    sidebar.css('top', diff);
                }
            });

            /*
            var sidebar = $('.sidebar'),
            sidebarWidth = sidebar.width(),
            hideText = $('.js-hos'),
            win = $(window);

            win.scroll(function () {
                var winPos = win.scrollTop();
                if (winPos > 300) {
                    hideText.slideUp();
                    sidebar.addClass("sticky");
                    sidebar.css('max-width', sidebarWidth);
                } else {
                    sidebar.removeClass("sticky");
                    sidebar.css('max-width', 'initial');
                    hideText.slideDown();
                }
            });*/


            //Add Google Anaytics tracker for each partial views
            if ($location.url().indexOf('OrderSummary') <= 0) {
                $window.ga('send', 'pageview', { page: $location.url() });
            }
        });

        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            //var excludedPathArrays = ["SessionExpired", "UnAuthorised", "AppError", "GetQuestions", "QuoteSummary", "PurchaseQuote", "BuyPolicy"];
            //var pathMatched = _.some(excludedPathArrays, function (item) {
            //    return next.originalPath.indexOf(item) > -1;
            //});

            //if (!pathMatched && current && current.scope && current.scope.quoteViewModel && !current.scope.responseRecieved) {
            //    loadingService.showLoader();
            //    event.preventDefault();
            //    if (current.scope.quoteViewModel.showCompanionClass) {
            //        current.scope.quoteViewModel.showCompanionClass = false;
            //        //loadingService.hideLoader();
            //    }
            //    else {
            //        window.location.href = '/PurchasePath/Home/Index';
            //    }
            //}
            //else

            loadingService.showLoader();
            var isDirty = false;

            if (current != undefined && current.scope != undefined) {

                //execute code only when user clicks on navigation menu
                if (navigation != undefined && navigation != null && navigation.status) {

                    if (current.scope.frmExposureInput != undefined) {
                        if (current.scope.frmExposureInput.$dirty == true || current.scope.frmExposureInput.$invalid == true) {
                            isDirty = true;
                        }
                    }
                    else if (current.scope.form_Questions != undefined) {
                        if (current.scope.form_Questions.$dirty == true) {
                            isDirty = true;
                        }
                    }
                    else if (current.scope.frmQuoteSummary != undefined) {
                        if (current.scope.frmQuoteSummary.$dirty == true) {
                            isDirty = true;
                        }
                    }
                    else if (current.scope.purchaseQuoteForm != undefined) {
                        if (current.scope.purchaseQuoteForm.$dirty == true) {
                            isDirty = true;
                        }
                    }
                    //else if (current.scope.paymentForm != undefined) {
                    //    if (current.scope.paymentForm.$dirty == true) {
                    //        alert("Please Click on continue to save your data");
                    //        event.preventDefault();
                    //        loadingService.hideLoader();
                    //    }
                    //}

                    if (isDirty) {
                        //Comment : Here in case of DIRTY state prevent user 
                        loadingService.hideLoader();
                        coreUtilityMethod.RemodelPopUp($('[data-remodal-id=navigationModel]'), true);
                        event.preventDefault();
                    }

                }
            }
        });

        $rootScope.$on('$routeChangeSuccess', function (event, next, current) {
            //Comment : Here after route change success reset global varibale used for "ProgressBar (navigation)"
            coreUtilityMethod.resetNavigation();
            //navigation = {};

            loadingService.hideLoader();
            $(':input:enabled:visible:first').focus();
        });
        $rootScope.$on("$routeChangeSuccess", function (currentRoute, previousRoute) {
            //Change page title, based on Route information
            if ($route.current.title != undefined && $route.current.title !== '') {
                document.title = $route.current.title + " - Cover Your Business";
            }
            else {
                document.title = "Cover Your Business";
            }
        });
    }]);
}
());