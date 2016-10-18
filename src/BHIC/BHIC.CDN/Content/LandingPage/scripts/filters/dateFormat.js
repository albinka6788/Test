angular.module('BHIC.LandingPage.Filters').
    filter('myDateFormat', function myDateFormat($filter) {
        // return function (text) {
        //var tempdate = new Date(text.replace(/-/g, "/"));
        //return $filter('date')(tempdate, "MMM-dd-yyyy");
        //  }

        return function (input, format) {
            return (input) ? $filter('date')(parseInt(input.substr(6)), format) : '';
        };
    });