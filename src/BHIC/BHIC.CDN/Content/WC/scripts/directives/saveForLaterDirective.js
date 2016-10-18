
(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('saveForLaterDivControl', ['appConfig', 'coreUtils', 'restServiceConsumer', 'loadingService', '$timeout', saveForLaterDivControlFn]);

    //Comment : Here SaveForLater div control directive implementation
    function saveForLaterDivControlFn(appConfig, coreUtilityMethod, restServiceConsumer, loadingService, $timeout) {
        //Comment : Here Define directive related all action implementation in DirectiveController iteself
        var scheduleCallController = ['$scope', function (scope) {
            //debugger;
            //Comment : Here Varible declaration & inititlization
            scope.userEmailId = '';
            scope.quoteId = '';
            scope.formSubmittedSaveForLater = false;

            //Comment : here call service to get valid email id reg-ex
            scope.ValidEmailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

            //Comment : here get appConfig provider values
            scope.loggedUserEmailId = appConfig.loggedUserEmailId;

            var prospectInfoEmailId = appConfig.prospectInfoEmailId;
            scope.userEmailId = (typeof prospectInfoEmailId != 'undefined' & prospectInfoEmailId != '') ? prospectInfoEmailId : scope.loggedUserEmailId;

            $timeout(function () {
                scope.quoteId = scope.currentquoteid;
            })

            //Comment : Here method exposed over scope of directive
            //Comment : Here method will be used to OPEN save-for-model MODEL box
            scope.openSaveForLaterLocal = function () {
                //alert('Save for later called !!');
                //console.log('Directive Controller function called !!!');
                //$scope.saveForLater();
                //scope.formSubmittedSaveForLater = false;
                //coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);

                ////Add Google Anaytics tracker for SaveForLater 
                //window.ga('send', 'pageview', { page: '/SaveForLater Clicked' });

                var pageName = scope.retrieveQuotePageName;

                if (pageName != null && pageName != '') {
                    //Comment : Here Check is EmailId entered then post this data and send user link ro revoke this quote
                    if (pageName == 'BuyPolicy') {
                        //no-validation is required for payment page,because user will always get pre-filled drop-down
                        scope.formValidationStatus = 'false';
                    }

                    scope.formSubmittedSaveForLater = false;
                    coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLater]'), true);

                    //Add Google Anaytics tracker for SaveForLater 
                    window.ga('send', 'pageview', { page: '/SaveForLater Clicked' });
                }
            }

            //Comment : Here method will be used to CLOSE save-for-model MODEL box
            scope.closeSaveForLaterModelLocal = function (modelName) {
                //console.log('Directive Controller model close function called !!!');
                //$scope.saveForLater();
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=' + modelName + ']'), false);
            }

            //Comment : Here method will be used to SUBMIT save-for-model MODEL box form data and will send email link to user
            scope.submitSaveForLaterLocal = function () {

                //console.log('Controller model data submit function called !!!');
                var pageName = scope.retrieveQuotePageName;

                //Comment : Here Check is EmailId entered then post this data and send user link ro revoke this quote
                if (pageName != null && pageName != '' && scope.userEmailId != null && scope.userEmailId != '') {

                    //Comment : Here according to new requirement if user has filled "all Mandatory fields" and submitted then save inoformation for this page also
                    if (scope.formValidationStatus.toUpperCase() == 'TRUE') {
                        scope.submitValidatedForm({ argument1: scope.userEmailId });
                    }
                    else {
                        //Comment : Here get client side supplied page-name then set here name required to be send in email-link
                        var promise = restServiceConsumer.sendSaveForLaterLink(pageName, scope.userEmailId);

                        promise
                                .then(function (data) {
                                    if (data.resultStatus == 'OK') {
                                        //Add Google Anaytics tracker for SaveForLater Submitted
                                        window.ga('send', 'pageview', { page: '/SaveForLater Submitted' });
                                        //console.log(data.resultStatus);                                                                  
                                    }
                                    else if (data.resultStatus == 'NOK') {
                                        //Add Google Anaytics tracker for SaveForLater Failed
                                        window.ga('send', 'pageview', { page: '/SaveForLater Failed' });
                                        //console.log('Could not able to submit !');
                                    }
                                    else if (data.resultStatus == undefined) {
                                        scope.closeSaveForLaterModelLocal('ModelSaveForLater');
                                    }

                                    //Comment : Here do not wait for data submission because it will create a long delay in displaying thank you message
                                    setTimeout(function () {
                                        coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ModelSaveForLaterThankYou]'), true);
                                    }, 1500);
                                })
                                .catch(function (exception) {
                                    //Add Google Anaytics tracker for SaveForLater Passed
                                    window.ga('send', 'pageview', { page: '/SaveForLater Failed' });

                                    //console.log(exception.toUpperCase());
                                })
                            .finally(function () {
                                loadingService.hideLoader();
                            });
                    }
                }
            }

            //Comment : Here method will be used to OPEN schedule-a-call MODEL box
            scope.openScheduleCallModel = function () {
                //Comment : Here if form has been validated then show model POP-UP for further information
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=scheduleCallModel]'), true);
                $('.remodal-overlay').addClass("overlay-light");
            };

            //Comment : Here becuase remodel plugin adding remodel DIV mutiple times thats creating in model close call problem (Solution)
            var remodalElements = angular.element('.saveForLater,remodal-is-initialized');
            if (remodalElements.length > 0) {

                $.each(remodalElements, function (i, remodalElement) {
                    if (i > 0 && typeof remodalElements[i] != 'undefined') {
                        $(this).remove();
                    }
                });

                //Comment : Here Angular method not working only in IE
                //angular.forEach(remodalElements, function (value, key)
                //{
                //    //Comment : Here other than first item remove all other model box added automatically by Remodel plugin
                //    if (key > 0 && typeof remodalElements[key] != 'undefined') {
                //        remodalElements[key].remove();
                //    }
                //});
            }

            //Comment : Here to show confirguration based object value(like phone number) in dreicvive template html
            $('.sysPhoneAnchor').attr('href', "tel:" + appConfig.phoneNumber + "");
            $('.sysPhoneAnchor').html(appConfig.phoneNumber);

        }];

        return {
            restrict: 'A',
            scope: { retrieveQuotePageName: '@pagename', formValidationStatus: '@isvalidform', submitValidatedForm: '&', currentquoteid: "=" },
            controller: scheduleCallController,
            template: [

//Comment : Here ######################## SECTION-1. SaveForLater Div HTML Content ######################## 
'<div class="lg-offset-1 lg-col-3">                                                                                                  ',
'    <div class="sidebar">                                                                                                           ',
'        <div class="widget">                                                                                  ',
'            <div class="widget-heading radius">                                                                                     ',
'                 <h6>Quote #: {{quoteId}}</h6>                                                                                            ',
'           </div>                                                                                                                   ',
'        </div>                                                                                                                      ',
'        <div class="widget">                                                                                                         ',
'            <div class="widget-heading">                                                                                             ',
'                <h6>Don\'t have time to finish right now?</h6>                                                                       ',
'            </div>                                                                                                                   ',
'            <div class="widget-content">                                                                                             ',
'                <div class="js-hos">                                                                                                 ',
'                  <p>We\'ll save your information and email you a link to your submission so you can continue whenever you’re ready.</p>',
'                  <p data-ng-show="loggedUserEmailId && loggedUserEmailId != \'\'">                                                  ',
'                        You can also access your saved information in Saved/New Quotes in Policy center.</p>                        ',
'                 </div>                                                                                                              ',
'                <input id="btn_SaveForLater" type="button" value="Save For Later" class="btn btn-primary"                            ',
'                data-ng-click=\'openSaveForLaterLocal()\' />                                                                         ',
'            </div>                                                                                                                   ',
'        </div>                                                                                                                       ',
'        <div class="widget">                                                                                                         ',
'            <div class="widget-heading">                                                                                             ',
'                <h6>Have Questions?</h6>                                                                                             ',
'            </div>                                                                                                                   ',
'            <div class="widget-content">                                                                                             ',
'                <p class="mb0">Call us at <a href="" class="sysPhoneAnchor"></a></p> ',
//below part has been removed as per new requirement given by Guard on 16.06.2016.
// If now is not a convenient time,                   ',
//'                <a href="javascript:void(0);" data-ng-click="openScheduleCallModel();">pick a time</a> and we\'ll call you back.
'            </div>                                                                                                                   ',
'        </div>                                                                                                                       ',
'    </div>                                                                                                                           ',
'</div>',




//Comment : Here ######################## SECTION-2. SaveForLater ReModal Box HTML Content ######################## 
'<div class="remodal-bg">                                                                                                             ',
'	<div class="remodal saveForLater" data-remodal-id="ModelSaveForLater" data-remodal-options="closeOnOutsideClick: false">          ',
'		<button class="remodal-close" data-ng-click="closeSaveForLaterModelLocal(\'ModelSaveForLater\')"></button>                    ',
'		<form name="form_saveForLater" novalidate>                                                                                    ',
'			<div class="remodal-heading">                                                                                             ',
'				<h2>Get Your Quote via Email</h2>                                                                                     ',
'			</div>                                                                                                                    ',
'			<div class="remodal-content">                                                                                             ',
'<p>Pick up right where you left off whenever you’re ready!  We’ll send a link for you to access your confidential submission to your e-mail.</p>',
'				<input type="email" name="contactEmailText" class="field field-lg" placeholder="Enter your email address" data-ng-model="userEmailId" data-ng-maxlength="128" data-ng-focus="formSubmittedSaveForLater=false;" data-ng-blur="formSubmittedSaveForLater=false;" data-ng-pattern="ValidEmailRegEx" ng-required="true" />',
'				<div data-ng-messages="form_saveForLater.contactEmailText.$error" data-ng-if="(form_saveForLater.contactEmailText.$touched && form_saveForLater.contactEmailText.$invalid) ||                                                                                     ',
'					(form_saveForLater.contactEmailText.$invalid && formSubmittedSaveForLater)" data-ng-cloak="">                     ',
'					<span data-ng-message="required" class="error">Please provide your email address</span>                                     ',
'					<span data-ng-message="email" class="error">Please enter a valid email ID</span>                                  ',
'                   <span data-ng-message="pattern" class="error">Please enter a valid email ID</span>                                ',
'				</div>                                                                                                                ',
'			</div>                                                                                                                    ',
'			<div class="remodal-footer center">                                                                                       ',
'				<button id="btn_SaveForLater" class="btn btn-primary" data-ng-class="{\'btn-loading\': (form_saveForLater.contactEmailText.$valid && formSubmittedSaveForLater)}" data-ng-disabled="form_saveForLater.contactEmailText.$valid && formSubmittedSaveForLater" data-ng-click="formSubmittedSaveForLater=true;',
'        (form_saveForLater.$valid) ? submitSaveForLaterLocal(userEmailId): \'\'">{{(form_saveForLater.contactEmailText.$valid && formSubmittedSaveForLater)?\'Please wait ...\':\'Submit\'}}</button>                                                                                                               ',
'    </div>                                                                                                                           ',
'</form>                                                                                                                              ',
'</div>                                                                                                                               ',
'</div>                                                                                                                               ',

//Comment : Here ######################## SECTION-3. ThankYouModel Div HTML Content (Once user submitted details to server) ##################### 
'<div class="remodal-bg">                                                                                                             ',
'    <div class="remodal" data-remodal-id="ModelSaveForLaterThankYou">                                                                ',
'        <button class="remodal-close" data-ng-click="closeSaveForLaterModelLocal(\'ModelSaveForLaterThankYou\')"></button>',
'        <form action="#">                                                                                                            ',
'            <div class="remodal-heading">                                                                                            ',
'                <h2>Thank you</h2>                                                                                                   ',
'            </div>                                                                                                                   ',
'            <div class="remodal-content">                                                                                            ',
'                <p>Your link to your confidential submission is on the way. You should receive an email from us within the next few minutes. If you don’t, be sure to check your junk/spam email filter.</p>                                                                                              ',
'                <div class="alert alert-info"><span class="alert-text d-block left"><strong>Important! </strong> Our prices can change from time to time so we want you to be aware if you revisit this quote on a later date, the price may be different.</span></div>                                       ',
'            </div>                                                                                                                   ',
'        </form>                                                                                                                      ',
'    </div>                                                                                                                           ',
'</div>                                                                                                                               '

            ].join('')
        };

    }

})();