
'use strict';

//Comment : Here create controller object taking action request and can communicate with application data-models
var questionController =
    function ($scope)
    {
        $scope.model =
                    {
                        MsgAngular: 'Hello by QuestionEngine conroller !',
                        otherAttribute: 111
                    };

        //Comment : Here we can define other properties/applicatio data variable in $scope context
        $scope.ModuleTitle = "QuestionEngine Page Title";
    };
                        