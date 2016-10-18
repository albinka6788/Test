
//Comment : Here STEP-1 create instance of root application object/app-module 
//which will map different controller, routing rules, factory method for application functionality
var questionEngineModule = angular.module('questionEngineApp', ['ngRoute'])


//Comment : Here STEP-2 attach controller in App-Module for taking differentt actions

//Comment : Here { Way - 1 }
//questionEngineModule.controller('ControllerName', ['param1',$scope, function (param1,$scope) { }]);


//Comment : Here { Way - 2 }
//questionEngineModule.controller('QuestionController', ['$scope', function ($scope)
//{
//    $scope.model =
//                {
//                    MsgAngular: 'QuestionEngine controller (Angular) !',
//                    otherAttribute: 111
//                };

//    //Comment : Here we can define other properties/applicatio data variable in $scope context
//    $scope.ModuleTitle = "QuestionEngine Page Title";

//}]);


//Comment : Here { Way - 3 }
questionEngineModule.controller('questionCtrl', questionController);


//Comment : Here add somr routing rule in this module 
var configFunction = function ($routeProvider)
{
    //Comment : Here define diffrent route path for redirection
    $routeProvider
        .when('/QuestionEngine/GetQuestions',
        {
            templateUrl: 'GetQuestions'
            //templateUrl: '/QuestionEngine/Questions/GetQuestions'     //Will work (Not full path required)
            //templateUrl: 'QuestionEngine/Questions/GetQuestions'     //Will not work (Path convention error - 404)
        })
        .when('/QuestionEngine/GetQuestions/:id',
        {
            templateUrl: function (paramUrl)
                        {
                            return 'GetQuestions?id='+ paramUrl.id;
                        }
        })
        .when('/QuestionEngine/GetQuestionsByAngular2',
        {
            templateUrl: 'GetQuestionsByAngular2'
        })
        .when('/routeFour/:id',
        {
            templateUrl: function (param)
                        {
                            return 'Route/ActionFour?id=' + param.id
                        }
        })
}

configFunction.$inject = ['$routeProvider'];

//Comment : Here set this routeFunction object into app/module 
questionEngineModule.config(configFunction);
