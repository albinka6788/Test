
(function () {
    'use strict';

    //directive definition
    angular.module('BHIC.WC.Directives').directive('saveForLaterDivControl', ['appConfig', 'coreUtils', saveForLaterDivControlFn]);
    angular.module('BHIC.WC.Directives').directive('saveForLaterFormModelControl', ['appConfig', 'coreUtils', saveForLaterFormModelControlFn]);
    angular.module('BHIC.WC.Directives').directive('thankYouModelControl', ['appConfig', thankYouModelControlFn]);

    //Comment : Here SaveForLater div control directive implementation
    function saveForLaterDivControlFn(appConfig, coreUtilityMethod) {
        return {
            restrict: 'A',
            scope: { controllercontext: '=', formvalidationstatus: '=', validateSaveForLater: '&' },
            replace: true,            
            link: linkSaveForLaterDivControlFn,
            template: ['<div class="lg-offset-1 lg-col-3">                                                                                                   ',
                       '    <div class="sidebar">                                                                                                            ',
                       '        <div class="widget">                                                                                                         ',
                       '            <div class="widget-heading">                                                                                             ',
                       '                <h6>Don\'t have time to finish right now?</h6>                                                                       ',
                       '            </div>                                                                                                                   ',
                       '            <div class="widget-content">                                                                               ',
                       '                <p>We\'ll save your information and email you a link to your submission so you can continue whenever you’re ready.</p>',
                       '                <p data-ng-show="loggedUserEmailId && loggedUserEmailId != \'\'">                                                  ',
                       //'                <p data-ng-show="false">                                                                                             ',
                       '                        You can also access your saved information in View Saved Quotes in Policy center.</p>',
                       '                <input id="btn_SaveForLater" type="button" value="Save For Later" class="btn btn-primary"                            ',
                       '                data-ng-disabled="!controllercontext.acceptFlag" data-ng-click=\'controllercontext.hasValidForm=formvalidationstatus;',
                       '                validateSaveForLater(controllercontext)\' />                                               ',
                       '            </div>                                                                                                                   ',
                       '        </div>                                                                                                                       ',
                       '        <div class="widget">                                                                                                         ',
                       '            <div class="widget-heading">                                                                                             ',
                       '                <h6>Have Questions?</h6>                                                                                             ',
                       '            </div>                                                                                                                   ',
                       '            <div class="widget-content">                                                                                             ',
                       '                <p class="mb0">Call us at <a href="" class="sysPhoneAnchor"></a>. If now is not a convenient time,                ',
                       '                <a href="javascript:void(0);" data-ng-click="openScheduleCallModel();">pick a time</a> and we\'ll call you back.</p> ',
                       '            </div>                                                                                                                   ',
                       '        </div>                                                                                                                       ',
                       '    </div>                                                                                                                           ',
                       '</div>'].join('')
        };
        
        function linkSaveForLaterDivControlFn(scope, elem, attrs) {
            //Comment : here get appConfig provider values
            scope.loggedUserEmailId = appConfig.loggedUserEmailId;
           
            $('.sysPhoneAnchor').attr('href', "tel:" + appConfig.phoneNumber + "");
            $('.sysPhoneAnchor').html(appConfig.phoneNumber);

            scope.openScheduleCallModel = function ()
            {
                //Comment : Here if form has been validated then show model POP-UP for further information
                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=scheduleCallModel]'), true);
                $('.remodal-overlay').addClass("overlay-light");

                //var modelBox = $('[data-remodal-id=scheduleCallModel]').remodal();

                ////if not undefined
                //if (modelBox) {
                //    modelBox.open();
                //    $('.remodal-overlay').addClass("overlay-light");
                //}
            };
        }
    }

    //Comment : Here SaveForLater Form Model control directive implementation
    function saveForLaterFormModelControlFn(appConfig, coreUtilityMethod) {
        return {
            restrict: 'E',
            scope: { controllercontext: '=', submitSaveForLater: '&', closeSaveForLaterModel: '&' },
            replace: true,
            link: linksaveForLaterFormModelControlFn,
            template: [
'<div class="remodal-bg">',
'	<div class="remodal saveForLater" data-remodal-id="ModelSaveForLater" data-remodal-options="closeOnOutsideClick: false">',
'		<button class="remodal-close" data-ng-click="controllercontext.closeSaveForLaterModel(\'ModelSaveForLater\')"></button>',
'		<form name="form_saveForLater" novalidate>',
'			<div class="remodal-heading">',
'				<h2>Get Your Quote via Email</h2>',
'			</div>',
'			<div class="remodal-content">',
'<p>Pick up right where you left off whenever you’re ready!  We’ll send a link for you to access your confidential submission to your e-mail.</p>',
'				<input type="email" name="contactEmailText" class="field field-lg" placeholder="Enter your email address" data-ng-model="controllercontext.userEmailId" data-ng-maxlength="128" data-ng-focus="controllercontext.formSubmittedSaveForLater=false;" data-ng-blur="controllercontext.formSubmittedSaveForLater=false;" data-ng-pattern="ValidEmailRegEx" ng-required="true" />',
'				<div data-ng-messages="form_saveForLater.contactEmailText.$error" data-ng-if="(form_saveForLater.contactEmailText.$touched && form_saveForLater.contactEmailText.$invalid) ||            ',
'					(form_saveForLater.contactEmailText.$invalid && controllercontext.formSubmittedSaveForLater)" data-ng-cloak="">',
'					<span data-ng-message="required" class="error">Please enter a email ID</span>',
'					<span data-ng-message="email" class="error">Please enter a valid email ID</span>',
'                   <span data-ng-message="pattern" class="error">Please enter valid email id</span>',
'				</div>',
'			</div>',
'			<div class="remodal-footer center">',
'				<button id="btn_SaveForLater" class="btn btn-primary" data-ng-class="{\'btn-loading\': (form_saveForLater.contactEmailText.$valid && controllercontext.formSubmittedSaveForLater)}" data-ng-disabled="form_saveForLater.contactEmailText.$valid && controllercontext.formSubmittedSaveForLater" data-ng-click="controllercontext.formSubmittedSaveForLater=true;',
'        controllercontext.submitSaveForLater(form_saveForLater.$valid,controllercontext)">{{(form_saveForLater.contactEmailText.$valid && controllercontext.formSubmittedSaveForLater)?\'Please wait ...\':\'Submit\'}}</button>',
'    </div>',
'</form>',
'</div>',
'</div>'
].join('')
        };

        function linksaveForLaterFormModelControlFn(scope, elem, attrs)
        {
            //Comment : here call service to get valid email id reg-ex
            //scope.ValidEmailRegEx = coreUtilityMethod.ValidEmailRegEx();
            scope.ValidEmailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

            //Comment : here get appConfig provider values
            scope.controllercontext.userEmailId = appConfig.loggedUserEmailId;

            var remodalElements = angular.element('.saveForLater,remodal-is-initialized');
            if (remodalElements.length > 0)
            {
                angular.forEach(remodalElements, function (value, key)
                {
                    //Comment : Here other than first item remove all other model box added automatically by Remodel plugin
                    if (key > 0) {
                        remodalElements[key].remove();
                    }
                });
            }
        }
    }

    //Comment : Here SaveForLater ThankYou Model control directive implementation
    function thankYouModelControlFn(appConfig) {
        return {
            restrict: 'E',
            scope: { controllercontext: '=', closeSaveForLaterModel: '&' },
            replace: true,
            template: [
                '<div class="remodal-bg">',
                '    <div class="remodal" data-remodal-id="ModelSaveForLaterThankYou">',
                '        <button class="remodal-close" data-ng-click="controllercontext.closeSaveForLaterModel(\'ModelSaveForLaterThankYou\')"></button>',
                '        <form action="#">',
                '            <div class="remodal-heading">',
                '                <h2>Thank you</h2>',
                '            </div>',
                '            <div class="remodal-content">',
                '                <p>Your link to your confidential submission is on the way. You should receive an email from us within the next few minutes. If you don’t, be sure to check your junk/spam email filter.</p>',
                '                <div class="alert alert-info"><span class="alert-text d-block left"><strong>Important! </strong> Our prices can change from time to time so we want you to be aware if you revisit this quote on a later date, the price may be different.</span></div>',
                '            </div>',
                '        </form>',
                '    </div>',
                '</div>'].join('')
        };
    }

})();