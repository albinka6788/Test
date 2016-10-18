$(document).ready(function () {

    var sysVariables = [];

    //Fetch all system variables on load
    $.ajax("/PurchasePath/Home/GetAllSystemVariables", {
        success: function (result) {

            sysVariables = result.data;

            $.each(sysVariables, function (k, v) {

                //define classes for PhoneNumber_CSR system variable
                if (window.phoneNumber_CSR != undefined && window.phoneNumber_CSR != "") {
                    if (v.Key == phoneNumber_CSR) {
                        $('.sysPhone').html(v.Value);
                        $('.sysPhoneHref').attr('href', "tel:" + v.Value + "");
                        $('.sysPhoneAnchor').attr('href', "tel:" + v.Value + "");
                        $('.sysPhoneAnchor').html(v.Value);
                    }
                }

                //define classes for Company_Domain system variable
                if (window.companyDomain != undefined && window.companyDomain != "") {
                    if (v.Key == companyDomain) {
                        $('.companyDomain').html(v.Value);
                    }
                }

                //define classes for CompanyName system variable
                if (window.companyName != undefined && window.companyName != "") {
                    if (v.Key == companyName) {
                        $('.sysCompany').html(v.Value);
                    }
                }

                //define classes for CompanyName_NENATI20 system variable
                if (window.companyName_NENATI20 != undefined && window.companyName_NENATI20 != "") {
                    if (v.Key == companyName_NENATI20) {
                        $('.sysCompanyNENATI20').html(v.Value);
                    }
                }

                //define classes for Physical_Address1 system variable
                if (window.physical_Address1 != undefined && window.physical_Address1 != "") {
                    if (v.Key == physical_Address1) {
                        $('.physicalAddress1').html(v.Value);
                    }
                }

                //define classes for Physical_Address2 system variable
                if (window.physical_Address2 != undefined && window.physical_Address2 != "") {
                    if (v.Key == physical_Address2) {
                        $('.physicalAddress2').html(v.Value);
                    }
                }

                //define classes for Physical_AddressCSZ system variable
                if (window.physical_AddressCSZ != undefined && window.physical_AddressCSZ != "") {
                    if (v.Key == physical_AddressCSZ) {
                        $('.physical_AddressCSZ').html(v.Value);
                    }
                }

                //define classes for MailingClaims_NewClaimPhone system variable
                if (window.mailingClaims_NewClaimPhone != undefined && window.mailingClaims_NewClaimPhone != "") {
                    if (v.Key == mailingClaims_NewClaimPhone) {
                        $('.mailingClaims_NewClaimPhone').html(v.Value);
                        $('.mailingClaims_NewClaimPhone').attr('href', "tel:" + v.Value + "");
                    }
                }

                //define classes for FaxNumber_Claims system variable
                if (window.faxNumber_Claims != undefined && window.faxNumber_Claims != "") {
                    if (v.Key == faxNumber_Claims) {
                        $('.faxNumber_Claims').html(v.Value);
                        $('.faxNumber_ClaimsHref').html(v.Value);
                        $('.faxNumber_ClaimsHref').attr('href', "tel:" + v.Value + "");
                    }
                }

                //define classes for Company_WebsiteShortURL system variable
                if (window.company_WebsiteURL != undefined && window.company_WebsiteURL != "") {
                    if (v.Key == company_WebsiteURL) {
                        $('.company_WebsiteURL').attr('href', v.Value);
                        $('.company_WebsiteURL').html(v.Value);
                    }
                }

                //define classes for BusinessHours system variable
                if (window.businessHours != undefined && window.businessHours != "") {
                    if (v.Key == businessHours) {
                        $('.businessHours').html(v.Value);
                    }
                }

            });

            return false;
        },
        async: false,
        error: function () {
        }
    });

    return false;
});





