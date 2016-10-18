(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('physicianPanelController', ['$scope', '$location', 'loadingService', 'physicianPanelData', physicianPanelServiceFn]);
        function physicianPanelServiceFn($scope, $location, loadingService, physicianPanelData) {
            // Controler funciton Start  write any methods/functions after this line.
            
            if (physicianPanelData.status) {
                $scope.physicianDocuments = physicianPanelData.physicianDocuments
                $scope.physicianStatus = ($scope.physicianDocuments.length > 0) ? true : false;                
            } else if (physicianPanelData.redirectStatus) {
                $location.path("/Error/");
            }           

            loadingService.hideLoader();

            $scope.DownloadDocument = function (docID) {
                window.location.href = OriginLocation() + baseUrl + "Document/DownloadApiDocument?docId=" + docID + "&CYBKey=" + window.name.split("CYB")[0];
            }
            // Controler funciton End Please dont write any methods/functions after this line.         
        }
    })();