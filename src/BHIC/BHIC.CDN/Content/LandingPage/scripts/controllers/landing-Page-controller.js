(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('landingPageController', ['$scope', '$filter', '$location', 'loadingService', 'resultSet', 'sharedUserService', 'authorisedUserWrapperService', '$route', landingPageControllerFn])
    function landingPageControllerFn($scope, $filter, $location, loadingService, resultSet, sharedUserService, authorisedUserWrapperService, $route) {

        // Controler funciton Start  write any methods/functions after this line.

        var _self = this;
        $scope.btnDelete = true;
        $scope.loggedInUser = JSON.parse(window.localStorage.getItem("user")).username;
        $("body").removeAttr("style");
        $('[name=site]').attr('href', "Content/Site.css");
        //$('[name=bootstrap1]').attr('href', "Content/bootstrap.css");
        //$('[name=dyanamicCSS]').attr('href', "Content/stylesheet.css");
        for (var i = 1; i <= 5; i++) {
            $('[name=dyanamicCSS' + i + ']').attr('href', "Content/Template" + i + ".css");
        }
        $(document).ready(function () {
            document.title = "Landing Page";
        });
        $scope.btnEdit = true;
        $scope.flagDate = false;
        loadingService.hideLoader();
        $scope.landingPages = resultSet.listOfLandingPages;
        $scope.None = (resultSet.listOfLandingPages.length == 0) ? true : false;

        $scope.drpsearchList = [{ Value: "lob", Text: "LOB" }, { Value: "State", Text: "State" }];

        $scope.convertToDate = function (inputString) {
            var convertedDate = new Date(parseInt(inputString.match(/\d/g).join("")));
            return $filter('date')(convertedDate, 'MM/dd/yyyy');
        }

        $scope.GotoInsertOrUpdate = function (actionType) {
            var url = null;;
            switch (actionType) {
                case "Add": url = "/AddLandingPage"; break;
                case "Edit": url = "/EditLandingPage/" + $("input[name='radioValue']:checked").val(); break;
                case "View": url = "/Ads/" + $("input[name='radioValue']:checked").val();
                    var lob = $("input[name='radioValue']:checked").attr("lobType");
                    $(document).ready(function () {
                        document.title = (lob == "WC") ? "Workers' Compensation – Cover Your Business" : (lob == "BOP") ? "Business Owner's Policy – Cover Your Business" : "Commercial Auto – Cover Your Business";
                    });

                    break;
                case "Temp": url = "/TemplateBackground/"; break;
            }
            $location.path(url);
        }

        $scope.SetEnable = function (btnId) {
            $scope.btnEdit = false;
            $('.dynamicbtnClass').attr('disabled', true);
            $('#btn' + btnId).removeAttr('disabled');
        }

        $scope.RowCheck = function () {
            //if ($('input.rowChk:checked').length == $scope.landingPages.length)
            //    $(".headChk").prop('checked', true);
            //else
            //    $(".headChk").prop('checked', false);

            if ($('input.rowChk:checked').length > 0)
                $scope.btnDelete = false;
            else
                $scope.btnDelete = true;
        }

        $scope.SelectAll = function (isselected) {
            if (isselected) {
                $(".headChk").prop('checked', true);
                $('input.rowChk').prop('checked', true);
            }
            else {
                $(".headChk").prop('checked', false);
                $('input.rowChk').prop('checked', false);
            }

            if ($('input.rowChk:checked').length > 0)
                $scope.btnDelete = false;
            else
                $scope.btnDelete = true;
        }

        $scope.DeletLandingPages = function () {
            var selectedIds = null;
            $.each($('input.rowChk:checked'), function (ind, obj) {
                selectedIds = (selectedIds == null) ? $(obj).val() : selectedIds + "," + $(obj).val();
            });
            
            if (confirm("Are you sure you want to delete this landing Page/Pages ?")) {
                var wrapper = { "method": "DeleteLandingPages", "postData": selectedIds };
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        alert("Landing Page/Pages deleted successfully.");
                        //$location.reload();
                        $route.reload();
                    }
                });
            }
        }

        $scope.SignOut = function () {
            var wrapper = { "method": "Logout" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    window.localStorage.clear();
                    sharedUserService.setUser(null);
                    $location.path("/Login");
                }
            });
        }
        loadingService.hideLoader();

        /*********************************** Pagination *******************************************/

        $scope.predicate = 'name';
        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };

        $scope.itemsPerPage = 5;
        $scope.currentPage = 0;
        $scope.items = $scope.landingPages;
       
        $scope.totalPageNumbers = Math.ceil($scope.items.length / 5);

        $scope.prevPage = function () {
            if ($scope.currentPage > 0) {
                $scope.currentPage--;
            }
        };

        $scope.prevPageDisabled = function () {
            return $scope.currentPage === 0 ? "disabled" : "";
        };

        $scope.pageCount = function () {
            return Math.ceil($scope.items.length / $scope.itemsPerPage) - 1;
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.pageCount()) {
                $scope.currentPage++;
            }
        };

        $scope.nextPageDisabled = function () {
            return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
        };

        $scope.setPage = function (n) {
            if ($scope.currentPage == 0 && n == 1)
                $scope.currentPage = 0;
            else
                $scope.currentPage = n;
        };


        $scope.sort = { column: 'Id', descending: true };

        $scope.selectedCls = function (column) {
            return column == $scope.sort.column + ":" + $scope.sort.descending;
        };

        $scope.changeSorting = function (column) {
            var sort = $scope.sort;
            if (sort.column == column) {
                sort.descending = !sort.descending;
            } else {
                sort.column = column;
                sort.descending = false;
            }
        };

        /******************************************************************************/

        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
