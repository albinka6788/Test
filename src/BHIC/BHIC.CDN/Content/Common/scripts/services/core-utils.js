(
    function () {
        'use sctrict';
        /* This service is used for core utilities across the application
           different inputs provided to the method.
           Inject _.js before using this service
        */
        angular
            .module('BHIC.Services')
            .factory('coreUtils', ['$window',coreUtilsFn]);

        function coreUtilsFn(window) {
            //Comment : Here valid email-id reg-ex
            var _emailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
            var _isServiceExecuting = false;
            var _controlLoadersCount = 0;
            /*Method to check if a string is 
              empty ,
              null,
              undefined
              or contains only blank spaces*/
            function _isEmptyString(str) {
                return (!str || 0 === str.length || /^\s*$/.test(str) || !(str.toString().trim()))
            }

            function _isEmptyObject(obj) {
                return _.isUndefined(obj) || _.isNull(obj);
            }

            function _formatDate(dateVal) {
                var value = new Date(dateVal);
                return value.getMonth() + 1 + "/" + value.getDate() + "/" + value.getFullYear();
            }

            function _getQuoteIdFromUrl() {
                var paramString = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                return paramString[0].split("=")[1];
            }

            function _getvalidAmount(str) {
                if (!_isEmptyString(str))
                    if (isNaN(str))
                        return parseFloat(str.replace(/\D/g, ""));
                    else
                        return parseFloat(str);
            }

            //Comment : Here utility method will strip non-numeric characters and return only number value
            function _toNumeric(str) {
                //first check for Undefined and empty object second check for empty string else do as usual
                return !_isEmptyString(str) ? (isNaN(str) ? str.replace(/\D/g, "") : str) : '';

                //return !_isEmptyObject ? (!_isEmptyString(str) ? (isNaN(str) ? str.replace(/\D/g, "") : str) : '') : '';  //generating error
            }

            //Comment : Here trim white spaces from given object
            function _trim(str) {
                return !_isEmptyString(str) ? (isNaN(str) ? str.replace(/[\s]/g, '') : str) : '';
            }

            //Comment : Here to check pattern of email-id added RegEx
            function _validEmailRegEx() {
                return _emailRegEx;
            }

            //Comment : Here method to show and hide Control level loader in application
            function _controlLevelLoader(elem, show) {
                if (show != null) {
                    if (show == true) {
                        _controlLoadersCount = _isServiceExecuting + 1;
                        _isServiceExecuting = true;
                        elem.addClass("btn-loading");
                    }
                    else {
                        _controlLoadersCount = _isServiceExecuting - 1;
                        elem.removeClass("btn-loading");
                        _isServiceExecuting = false;
                    }
                }
            }

            //Comment : Here method to show and hide Control level loader in application
            function _remodelPopUp(elem, state) {
                if (state != null) {
                    var modelBox = elem.remodal();

                    //if object not undefined then only
                    if (modelBox) {
                        //Comment : Here "OPEN" Remodel pop-up
                        if (state == true) {
                            modelBox.open();
                            $('.remodal-overlay').removeClass("overlay-light");
                        }
                            //Comment : Here "CLOSE" Remodel pop-up
                        else {
                            modelBox.close();
                        }
                    }
                }
            }

            function _backgroundProcessExecuting() {
                if (_isServiceExecuting && _controlLoadersCount > 0) {
                    return true;
                }
                else
                    return false;
            }

            //Comment : Here method to page scroll to top 
            function _scrollToTop() {
                $("html,body").animate({ scrollTop: 0 }, "slow");
            }

            //Comment : Here method to set "Navigation Bar" in PurchasePath
            function _setNavigation(path,status)
            {
                window.navigation =
                {
                    path: path || '',
                    status: status
                };
            }

            //Comment : Here method to Re-set "Navigation Bar" in PurchasePath
            function _resetNavigation(path, status)
            {
                window.navigation = {};
            }

            return {
                IsEmptyString: _isEmptyString,
                IsEmptyObject: _isEmptyObject,
                FormatDate: _formatDate,
                GetQuoteIdFromUrl: _getQuoteIdFromUrl,
                GetValidAmount: _getvalidAmount,
                ConvertToNumeric: _toNumeric,
                Trim: _trim,
                ValidEmailRegEx: _validEmailRegEx,
                controlLevelLoader: _controlLevelLoader,
                BackgroundProcessExecuting: _backgroundProcessExecuting,
                RemodelPopUp: _remodelPopUp,
                ScrollToTop: _scrollToTop,
                setNavigation: _setNavigation,
                resetNavigation: _resetNavigation
            }
        }
    }
)();
