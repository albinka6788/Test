
(
    function () {
        'use strict';
        /* This service will be used to display loader while processing http request
        */
        angular
            .module('BHIC.Services')
            .factory('loadingService', [loadingServiceFn]);

        function loadingServiceFn() {

            function _showLoader() {
                $('#divLoader').show();
                $('html').addClass("is-locked");
            }

            function _hideLoader() {
                $('#divLoader').hide();
                $('html').removeClass("is-locked");
            }

            return {
                showLoader: _showLoader,
                hideLoader: _hideLoader
            };
        }
    }
)();