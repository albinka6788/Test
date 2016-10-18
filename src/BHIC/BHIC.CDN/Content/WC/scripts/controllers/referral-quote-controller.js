(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module("BHIC.WC.Controllers").controller('referralQuoteController', ['$scope', 'referralQuoteService', '$location', 'loadingService', 'coreUtils', referralQuoteControllerFn]);

        //controller function
        function referralQuoteControllerFn(scope, referralQuoteService, location, loadingService, coreUtilityMethod)
        {
            var self = this;
            
            var _referralQuoteVm =
            {
                contactName: '',
                businessName: '',
                contactPhone: '',
                contactEmail: '',
                policyInformation: {}
            };

            var _formSubmitted = false;
            var _isMultiState = false;
            var _listOfErrors = [];
            var _emailRegEx = /^[_a-zA-Z0-9]+(\.[_a-zA-Z0-9]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

            var _currentPolicyFilesFormData, _emptyString = '', _uploadedFileException = '';
            var _currentPolicySelectedFiles, _filesSize = 0, _valid = true;
            var _currentPolicySelectedFilesNew = [];
            var _notAllowedExtensions = ['', 'exe', 'msi', 'dll', 'ocx', 'com', 'jar', 'bat']; //must check file type is not empty

            function _init(businessInfoModel)
            {
                loadingService.hideLoader();
                _setReferralHeaderMsg();

                //Comment : here call service to get valid email id reg-ex
                //_emailRegEx = coreUtilityMethod.ValidEmailRegEx();

                //Clearing the file upload 
                $("#currentPolicyFiles").click(function () {
                    angular.element(document.getElementById('currentPolicyFiles')).val('');
                });

                _setReferralModel(businessInfoModel);
            };

            function _setReferralModel(model) {
                self.referralQuoteVm.contactName = model.ContactName;
                self.referralQuoteVm.businessName = model.CompanyName;
                self.referralQuoteVm.contactPhone = model.PhoneNumber;
                self.referralQuoteVm.contactEmail = model.Email;
            }

            var _setReferralHeaderMsg = function ()
            {
                //Comment : Here reset 
                self.isMultiState = false;
            };

            var _submitReferralQuoteData = function (form_referralQuote, referralQuoteVm)
            {
                //debugger;

                //reset error list
                self.listOfErrors = [];

                //Comment : Here check for all validation are successfully processed then proceed further
                var hasValidForm = form_referralQuote.$valid;
                if (hasValidForm) {
                    loadingService.showLoader();

                    //Comment : Here map feild data
                    var referralQuoteMappedVm = {
                        ContactName: referralQuoteVm.contactName,
                        BusinessName: referralQuoteVm.businessName,                        
                        PhoneNumber: referralQuoteVm.contactPhone,
                        //PhoneNumber: _setPhoneNumberFormat(referralQuoteVm.contactPhone),
                        Email: referralQuoteVm.contactEmail
                    };

                    //Comment : Here collect uploded files (Current Policy) FormData
                    var uploadedFilesFormData = _currentPolicyFilesFormData;

                    //Comment : Here Way to get files/object from FormData javascript object (Important)
                    //console.log(uploadedFilesFormData.get('1_1_1_Json.txt'));
                    //console.log(uploadedFilesFormData.get('DataModel'));

                    //Comment : here call service to submit questions response                    
                    var promise = referralQuoteService.submitReferralQuoteData(referralQuoteMappedVm, _currentPolicySelectedFilesNew);

                    promise
                        .then(function (data) {
                            if (data.resultStatus == 'OK') {
                                //Comment : Here open model to show message and then on close redirect user to Home page
                                coreUtilityMethod.RemodelPopUp($('[data-remodal-id=ReferralQuote]'), true);
                            }
                            else if (data.resultStatus == 'NOK') {
                                //Comment : Here must check for any user system errors if any
                                var errorMessages = data.resultMessages;

                                if (errorMessages != undefined && errorMessages != null && errorMessages.length == 0) {
                                    location.path("/AppError/");
                                }
                                else {
                                    //Comment : Here iterate errors list and display it to screen
                                    self.listOfErrors = errorMessages;
                                    coreUtilityMethod.ScrollToTop();
                                }
                            }

                            //Comment : Here reset form model to clear and then set from state as PRISTINE to hide all validation messages
                            self.referralQuoteVm = {};
                            form_referralQuote.$setPristine();
                            self.formSubmitted = false;
                        })
                        .catch(function (exception) {
                            //console.log(exception.toUpperCase());
                            location.path("/AppError/");
                        })
                        .finally(function () {
                            //self.referralQuoteVm = {};
                            loadingService.hideLoader();

                            //Comment : Here clear all uploded file seleted bu user
                            angular.element(document.getElementById('currentPolicyFiles')).val('');
                            //console.log("finally finished quote payment plan submission.");
                        });
                }
                //Comment : Not displaying additional Error message(else part commented)
                //else {
                //    //Comment : Here iterate errors list and display it to screen
                //    self.listOfErrors = ['Please fill the all required fields.'];
                //    coreUtilityMethod.ScrollToTop();
                //    //alert('Please fill the all required fields.');   
                //}
            };

            //validating and appending the files to to FormData
            var _getCurrentPolicyUploadedFiles = function ($files)
            {
                //debugger;

                _filesSize = 0;
                _currentPolicyFilesFormData = _emptyString;
                _currentPolicySelectedFilesNew = [];

                //Comment : Here First of all check for no. of files uploaded
                if ($files.length > 3)
                {
                    if (navigator.userAgent.indexOf('MSIE') > 0)
                    {
                        $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                    }
                    else {
                        angular.element(document.getElementById('currentPolicyFiles')).val('');
                    }

                    scope.$apply(function ()
                    {
                        self.uploadedFileException = "Only 3 files can be uploaded";
                    });

                    return;
                }

                //Comment : Here itereate each file to check FileType, Size & etc.
                angular.forEach($files, function (file, index)
                {
                    //Comment : Here try to get current file type during iteration
                    var filetype = file.name.split('.').pop();
                    _filesSize += file.size;                    

                    //Comment : Here must check each file extention
                    if (_notAllowedExtensions.indexOf(filetype) > -1)
                    {
                        if (navigator.userAgent.indexOf('MSIE') > 0)
                        {
                            $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                        }
                        else {
                            angular.element(document.getElementById('currentPolicyFiles')).val('');
                        }

                        scope.$apply(function ()
                        {
                            self.uploadedFileException = "The file format is not supported";
                        });

                        //break or return execution
                        return;
                    }
                    else if (_filesSize >= 10485760)
                    {
                        if (navigator.userAgent.indexOf('MSIE') > 0) {
                            $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                        }
                        else {
                            angular.element(document.getElementById('currentPolicyFiles')).val('');
                        }

                        _filesSize = 0;

                        scope.$apply(function () {
                            self.uploadedFileException = "The file size should not be greater then 10 MB.";
                        });
                        return;
                    }
                    else
                    {
                        if (_valid == true || _currentPolicyFilesFormData == "") {
                            _currentPolicyFilesFormData = new FormData();
                            _valid = false;
                        }

                        //Comment : Here add each file during iteration into "FormData" object
                        _currentPolicyFilesFormData.append(file.name, file);

                        //Comment : Here add each file during iteration into local "Array" object
                        _currentPolicySelectedFilesNew.push(file);

                        //if ((index + 1) == $files.length)
                        //{
                        //    _currentPolicyFilesFormData.append("DataModel", JSON.stringify(self.referralQuoteVm));
                        //}
                        //console.log(_currentPolicyFilesFormData.get(file.name));

                        scope.$apply(function () {
                            self.uploadedFileException = "";
                        });
                    }
                });
            };

            function _setPhoneNumberFormat(phoneNumber)
            {
                if (phoneNumber.indexOf('x') == -1)
                {
                    phoneNumber = phoneNumber = phoneNumber.slice(0, 3) + '-' + phoneNumber.slice(3, 6) + '-' + phoneNumber.slice(6, 10) + 'x' + (phoneNumber.length > 10 ? phoneNumber.slice(10, phoneNumber.length) : '____');
                }

                return phoneNumber;
            }

            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(self, {
                init: _init,
                referralQuoteVm: _referralQuoteVm,
                isMultiState: _isMultiState,
                formSubmitted: _formSubmitted,
                submitReferralQuoteData: _submitReferralQuoteData,
                listOfErrors: _listOfErrors,
                getCurrentPolicyUploadedFiles: _getCurrentPolicyUploadedFiles,
                uploadedFileException: _uploadedFileException,
                currentPolicySelectedFiles: _currentPolicySelectedFiles,
                ValidEmailRegEx: _emailRegEx
            });

        };
    }
)();