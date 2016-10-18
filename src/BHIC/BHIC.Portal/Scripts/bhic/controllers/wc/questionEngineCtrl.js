(
    /**
        * @expression : IIFE
        * @expressiondesc : IIFE "immediately-invoked function expression" (Auto gloabl scope section)
    */
    function ()
    {
        'use strict';

        /**
           * @class : questionController
           * @classdesc : Main Question-Engine Controller for doing question rendering before allowing to generate quotes,
                        function with certain Models,Properties
           * @ngInject
        */

        function questionController($scope, configConstant,$http)
        {
            /*Private and public Properties and function */
            var pageTitle = 'Questions :: please give your answers and get quotes';

            //var appVersion = appVersionHistory;
            //$scope.appVersion = appVersion;
            $scope.hello('Prem');
            console.log('Your name : ' + configConstant.Name + ', Your Age :' + configConstant.Age);

            var model =
                {
                    questionId: 1,
                    questionText: 'Do you engage in the sale, delivery, service, or repair of large trucks (commercial), tractor trailers, mobile homes, or boats?',
                    questionType: 'text'
                };

            function privateQuestionSet()
            {
                return {
                    "d":
                    [
                      {
                          "SrNo": 201,
                          "Id": 1,
                          "Type": "date",
                          "Value": "",
                          "Text": "Provide you organization estblishment date?  ",
                          "Order": 1,
                          "WaterMarkText": "",
                          "Render": true
                      }
                   ]
                };
            }

            var questions = 
                {
                    "d":
                    [
                      {
                          "SrNo": 101,
                          "Id": 1,
                          "Type": "text",
                          "Value": "Enter Value",
                          "Text": "Do you engage in the sale, delivery, service, or repair of large trucks (commercial), tractor trailers, mobile homes, or boats?  ",
                          "Order": 1,
                          "WaterMarkText": "First Name",
                          "Render": true
                      },
                      {
                          "SrNo": 102,
                          "Id": 2,
                          "Type": "number",
                          "Value": null,
                          "Text": "What is the delivery/service radius (in miles)? ",
                          "Order": 2,
                          "Render": true
                      },
                      {
                          "SrNo": 103,
                          "Id": 3,
                          "Type": "radio",
                          "Value": "",
                          "Text": "Do you provide towing for AAA or for the police?  ",
                          "Order": 3,
                          "RadioValue": false,
                          "RadioLable": "Yes",
                          "Render": true
                      }
                      ,
                      {
                          "SrNo": 104,
                          "Id": 4,
                          "Type": "select",
                          "Value": "Omganesh",
                          "Text": "Do you provide BBB or other expenses for employee?  ",
                          "Order": 4,
                          "Render": true
                      },
                      {
                          "SrNo": 105,
                          "Id": 5,
                          "Type": "text",
                          "Value": "",
                          "Text": "Do you engage in the sale, delivery, service, or repair of large trucks (commercial), tractor trailers, mobile homes, or boats?  ",
                          "Order": 5,
                          "WaterMarkText": "",
                          "Render": true
                      },
                      {
                          "SrNo": 106,
                          "Id": 6,
                          "Type": "email",
                          "Value": "",
                          "Text": "Do you engage in the sale, delivery, service, or repair of large trucks (commercial), tractor trailers, mobile homes, or boats?  ",
                          "Order": 6,
                          "WaterMarkText": "",
                          "Render": true
                      }
                    ]
                };


            /**
               * @function : getIndustries
               * @functiondesc : interact with client API and get data for poppulate form controlls
               * @parameters : dst, destination object in which
                             src, source object model & properties will be copied
            */
            $scope.getIndustries = function getDataFromAPI()
            {
                $http.get('https://ydsml.guard.com/inssvc/api/Industries').success(function (data)
                {
                    $scope.industries = data.Industries;
                });
            }

            
            $scope.getIndustries();

            /**
               * @function : angular.extend()
               * @functiondesc : To minimize redundancy (This will copy all src[local controller objects] into dst[$scope])
                            function with certain Models,Properties
               * @parameters : dst, destination object in which
                             src, source object model & properties will be copied
            */
            angular
                .extend($scope,
                {
                    pageTitle: pageTitle,
                    model: model,
                    questions: questions,
                    publicQuestionSet: privateQuestionSet
                });

        };


        /**
           * @module : bhic-app.wc
           * @moduledesc : Main worker compensation (wc) module which will register many controllers to take different action
                        here register this controller[questionController] into module
           * @ngInject : inject $scope module into controller for certain Moel-View communication
        */
        angular
            .module('bhic-app.wc')
            .controller('questionEngineController', ['$scope','configConstant','$http',  questionController]);

    }
)();
