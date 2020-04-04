app1.controller("AC_ShipmentSchedule", function ($scope, $http, $window, $rootScope) {
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            debugger;
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.COM_ID = "0";
            $scope.COM_CODE = "";
            $scope.COM_NAME = "";
            $scope.COM_DISPLAYNAME = "";
            $scope.COM_USTID = "";
            $scope.Tax_No = "";
            $scope.COM_STREET = "";
            $scope.COM_ZIP = "";
            $scope.COM_EMAIL = "";
            $scope.COM_FAX = "";
            $scope.COM_PHONE = "";
            $scope.COM_MOBILE = "";
            $scope.COM_REMARKS = "";
            $("#drpCountry").val("0").trigger("chosen:updated");
            $("#drpBank").val("0").trigger("chosen:updated");
            $("#companytype").val("0").trigger("chosen:updated");
            $scope.selectedcompanytype = "0";
            $scope.selectedbank = "0";
            $scope.selectedcountry = "0";
            $scope.getstates = {};
            $scope.selectedstate = "0";
            $scope.getcities = {};
            $scope.selectedcity = "0";
            $("#contactbody").html("");
            $("#contactbody").append("<tr><td><select class='form-control' id='drptitle'><option value='1' selected>Mr.</option><option value='2'>Mrs.</option><option value='3'>Dr.</option><option value='4'>Drs.</option></select></td><td><input type='text' id='txtfirstname' class='form-control' /></td><td><input type='text' id='txtlastname' class='form-control' /></td><td><select class='form-control' id='drpjobtitle'><option value='MD'>MD</option><option value='Business Manager'>Business Manager</option><option value='Marketing Manager'>Marketing Manager</option><option value='Managing director'>Managing director</option></select></td><td><input type='number' id='txtphoneno' class='form-control' /></td><td><input type='number' id='txtfaxno' class='form-control' /></td><td><input type='number' id='txtmobileno' class='form-control' /></td><td><input type='text' id='txtemailc' class='form-control' /></td><td><input type='text' id='txtremarksc' class='form-control' /></td><td><a style='padding: 5px;' onclick='addnewrow(this)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' onclick='deleterow(this)'><i class='fa fa-trash'></i></a></td></tr>");

        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }


});