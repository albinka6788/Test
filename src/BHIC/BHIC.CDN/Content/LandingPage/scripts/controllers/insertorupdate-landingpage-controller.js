(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('insertOrUpdateLandingPageController', ['$scope', '$sce', '$http', '$filter', '$routeParams', '$location', 'loadingService', 'resultSet', 'authorisedUserWrapperService', insertOrUpdateLandingPageControllerFn])
    function insertOrUpdateLandingPageControllerFn($scope, $sce, $http, $filter, $routeParams, $location, loadingService, resultSet, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.

        var _self = this;
        $("body").removeAttr("style");
        //$('[name=bootstrap1]').attr('href', "Content/bootstrap.css");
        //$('[name=dyanamicCSS]').attr('href', "Content/stylesheet.css");
        $scope.Id = 0;
        $scope.enable = false;
        $scope.PageType = null;
        $scope.submitted = false;
        $scope.drpTypeList = resultSet.listOfLOB;
        $scope.drpStateList = resultSet.listOfStates;
        $scope.drpTemplateList = resultSet.listOfTemplates;
        $scope.drpImageList = resultSet.listOfBackgroundImages;
        $scope.defaultLOB = -1;
        $scope.defaultState = -1;
        var StateInd = 0;


        $scope.GetLandingPageDetails = function (response) {
            var objEdit = response.landingPageDetails;
            $scope.Id = objEdit.Id;
            $scope.txtPageName = objEdit.PageName;
            $scope.txtHeading = objEdit.Heading;
            $scope.txtSubHeading = objEdit.SubHeading;
            $scope.productName = objEdit.ProductName;
            $scope.productHighlight = objEdit.ProductHighlight;
            $scope.BTNText = objEdit.BTNText;
            $scope.calloutText = objEdit.CalloutText;

            $scope.IsDeployed = objEdit.IsDeployed;

            angular.forEach(objEdit.CTAMsgList, function (obj, ind) {
                if (ind == 0)
                    AddCATTemplate('edit', obj.Message, obj.Id);
                else
                    AddCATTemplate('edit', obj.Message, obj.Id);
            });

            $('option:selected').attr('selected', '');

            $scope.setDropDownDefaultValue(objEdit);

            loadingService.hideLoader();
        }

        $scope.setDropDownDefaultValue = function (objEdit) {

            var flag = false;
            angular.forEach($scope.drpTypeList, function (obj, ind) {
                flag = false;
                if (!flag && obj.Text == objEdit.lob) {
                    $scope.defaultLOB = (parseInt(ind)); flag = true;
                }
            });

            angular.forEach($scope.drpStateList, function (obj, ind) {
                flag = false;
                if (!flag && obj.Text == objEdit.State) {
                    $scope.defaultState = (parseInt(ind)); flag = true;
                }
            });

            angular.forEach($scope.drpTemplateList, function (obj, ind) {
                flag = false;
                if (!flag && obj.Value == objEdit.TemplateId) {
                    $scope.defaultTemplate = (parseInt(ind)); flag = true;
                }
            });

            var img = objEdit.MainImage.split("/");
            angular.forEach($scope.drpImageList, function (obj, ind) {
                flag = false;
                if (!flag && obj.Text == img[img.length - 1]) {
                    $scope.defaultImage = (parseInt(ind)); flag = true;
                }
            });

            $scope.lineTypeModel = $scope.drpTypeList[$scope.defaultLOB];
            $scope.stateModel = $scope.drpStateList[$scope.defaultState];
            $scope.templatesModel = $scope.drpTemplateList[$scope.defaultTemplate];
            $scope.mainImageModel = $scope.drpImageList[$scope.defaultImage];
        }



        $scope.init = function () {
            if ($routeParams.TokenId == undefined) {
                $scope.btnText = "Insert";
                $scope.HeaderText = "Add New Landing Page";
                AddCATTemplate();
            }
            else {
                loadingService.showLoader();
                $scope.btnText = "Update";
                $scope.HeaderText = "Update Your Landing Page";
                var wrapper = { "method": "GetLandingPageDetailsByTokenId", "queryString": $routeParams.TokenId };
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        $scope.GetLandingPageDetails(response);
                    } else
                        console.log("Record Not Exists for Update for the Token Id : " + $routeParams.TokenId);
                    // window.location.href = response.RedirectUrl;
                });
            }
        }

        $scope.InsertOrUpdateLandingPage = function () {
            try {
                //&& confirm("Have reviewed your landing page..?")
                if ($scope.CheckValidation()) {

                    var objLandingPage = {};
                    objLandingPage.Id = $scope.Id;
                    objLandingPage.lob = $scope.lineTypeModel.Text;
                    objLandingPage.state = $scope.stateModel.Text;
                    objLandingPage.TemplateId = $scope.templatesModel.Value;
                    objLandingPage.MainImage = $scope.mainImageModel.Text;
                    
                    objLandingPage.PageName = $scope.txtPageName;
                    objLandingPage.Heading = $scope.txtHeading;
                    objLandingPage.SubHeading = $scope.txtSubHeading;
                    objLandingPage.ProductName = $scope.productName;
                    objLandingPage.ProductHighlight = $scope.productHighlight;
                    objLandingPage.BTNText = $scope.BTNText;
                    objLandingPage.CalloutText = $scope.calloutText;
                    objLandingPage.IsDeployed = $scope.IsDeployed;

                    if ($scope.Id != 0) {
                        objLandingPage.TokenId = $routeParams.TokenId;
                    }

                    var lstCTAMsg = [];

                    $("#staticTable tr [name=CTAMsg]").each(function (ind, obj) {
                        var CTAMessage = { Id: $(obj).attr("ctaid"), TokenId: objLandingPage.TokenId, Message: $(obj).val() };
                        lstCTAMsg.push(CTAMessage);
                    });

                    objLandingPage.CTAMsgList = lstCTAMsg;
                    var wrapper = { "method": "InsertOrUpdateLandingPage", "postData": objLandingPage };
                    authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                        if (response.success) {
                            ($scope.Id == 0)
                                     ? alert("New Landing Page Created Successfully.")
                                     : alert("Landing Page Configuration updated Successfully.");
                            $location.path("/LandingPages");
                        } else {
                            console.log(response);
                        }
                    });
                }
            } catch (e) {
                console.log(e);
            }
        }

        $scope.GoToLandingPage = function () {
            $location.path("/LandingPages");
        }

        $scope.GoToLandingPagePreview = function () {
            if ($scope.CheckValidation()) {
                var objpreView = {};
                objpreView.lob = $scope.lineTypeModel.Text;
                objpreView.state = $scope.stateModel.Text;
                objpreView.TemplateId = $scope.templatesModel.Text;
                objpreView.TemplateCss = "Content/" + $scope.templatesModel.Text.split(".")[0] + ".css";
                objpreView.MainImage = window.location.origin + "/LandingPage/Images/MainImage/" + $scope.mainImageModel.Text;

                objpreView.Heading = ($scope.txtHeading == undefined || $scope.txtHeading.length == 0) ? null : $scope.txtHeading;
                objpreView.SubHeading = ($scope.txtSubHeading == undefined || $scope.txtSubHeading.length == 0) ? null : $scope.txtSubHeading;
                objpreView.ProductName = ($scope.productName == undefined || $scope.productName.length == 0) ? null : $scope.productName;
                objpreView.ProductHighlight = ($scope.productHighlight == undefined || $scope.productHighlight.length == 0) ? null : $scope.productHighlight;
                objpreView.BTNText = ($scope.BTNText == undefined || $scope.BTNText.length == 0) ? null : $scope.BTNText;
                objpreView.CalloutText = ($scope.calloutText == undefined || $scope.calloutText.length == 0) ? null : $scope.calloutText;
                //objpreView.CTATitle = ($scope.CTATitle == undefined || $scope.CTATitle.length == 0) ? null : $scope.CTATitle;

                if ($routeParams.Id != undefined) {
                    objpreView.TokenId = $routeParams.Id;
                }

                var lstCTAMsg = [];

                $("#staticTable tr [name=CTAMsg]").each(function (ind, obj) {
                    if ($(obj).val() != "") {
                        var CTAMessage = { Id: $(obj).attr("ctaid"), TokenId: 0, Message: $(obj).val() };
                        lstCTAMsg.push(CTAMessage);
                    }
                });

                objpreView.CTAMsgList = (lstCTAMsg);
                window.localStorage.setItem("preview", JSON.stringify(objpreView));
               // $location.path("PreViewTemplate/" + $scope.templatesModel.Text);
                window.open(window.location.origin + "/LandingPage/#/PreViewTemplate/" + $scope.templatesModel.Text);
                return false;
            }
        }

        $scope.CheckValidation = function () {
            $('.dyimg').remove();
            var flag = true;
            var errorMsg = ["Please select line of business", "Please select state", "Please select template", "Please select background image"
                // If its needed validation for all text boxes please enabble below error message
                /* "Please enter title..!", "Please enter description..!", "Please enter CTA Box title..!", "Please enter Buttont Text..!" */
            ];
            var keys = [{ "Value": $('[name=lineTypeMod]').find("option:selected").val() }, { "Value": $('[name=stateMod]').find("option:selected").val() },
                { "Value": $('[name=templatesMod]').find("option:selected").val() }, { "Value": $('[name=mainImageMod]').find("option:selected").val() }

                // If its needed validation for all text boxes please add textbox model for dynamic validitons.
                /*
                { "Value": $('[name=titleModel]').val() },
                { "Value": $('[name=DesModel]').val() },
                { "Value": $('[name=ctaBoxTitleModel]').val() },
                { "Value": $('[name=BTNTextModel]').val() }*/
            ];

            $.each(keys, function (ind, obj) {
                if ((obj.Value == "" || obj.Value == "0")) {
                    var img = "<img class='dyimg' src='Images/infor-icon.png' style='width:25px; height:25px' title='" + errorMsg[ind] + "'/>";
                    $($('[name=dynamicError]')[ind]).append(img);
                    flag = false;
                }
            });


            // If its needed validation for multiple CTA Messages.
            //if (flag) {
            //    $("#staticTable tr [name=CTAMsg]").each(function (ind, obj) {
            //        if ($(obj).val() == "" && flag) {
            //            var msg = "Please enter message for the " + parseInt(ind + 1) + " block";
            //            var img = "<img class='dyimg' src='Images/infor-icon.png' style='width:25px; height:25px' title='" + msg + "'/>";
            //            $($('[name=errorBlock]')[ind]).append(img);
            //            flag = false;
            //        }
            //    });
            //}

            return flag;
        }

        $scope.drpChange = function (sender, imgIndex) {
            if (sender || sender.Value != -1)
                $($(imgIndex).find("img")).remove();
        }

        loadingService.hideLoader();
        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
