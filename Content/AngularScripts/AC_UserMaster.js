
app1.controller("AC_UserMaster", function ($scope, $http, $window, $rootScope) {
    GetUserList();
    GetPrivilages();
    GetCountry();
    //Get privilage
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Admin_User_Master/GetPrivilages");
        getprivilages.then(function (privilage1) {
            var privilage = JSON.parse(privilage1.data);
            $scope.Iscreate = privilage[0].IS_ADD;
            $scope.Isedit = privilage[0].IS_EDIT;
            $scope.Isdelete = privilage[0].IS_DELETE;
            $scope.Isrestore = privilage[0].IS_FULLCONTROL;
            $scope.Isview = privilage[0].IS_VIEW;
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    //Get User List
    function GetUserList() {
        var getuserllist = $http.get("/ET_Admin_User_Master/GetUserList",
            {
                params: { deleted: false }
            });
        getuserllist.then(function (user) {
            var res = JSON.parse(user.data);
            $scope.userlist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get User Restore List
    function GetUserRestoreList() {
        var getrestoreuserllist = $http.get("/ET_Admin_User_Master/GetUserList",
            {
                params: { deleted: true }
            });
        getrestoreuserllist.then(function (user) {
            var res = JSON.parse(user.data);
            $scope.userrestorelist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Bind Country List to dropdown
    function GetCountry() {
        var getcountryllist = $http.get("/ET_Admin_User_Master/GetCountryList");
        getcountryllist.then(function (country) {
            $scope.countrys = country.data;
        }, function () {
            alert('Data not found');
        });
    }
    //Bind Roles List to dropdown
    function GetRoles() {
        
        var getrolellist = $http.get("/ET_Admin_User_Master/BindRoles",
            {
                params: { id : $scope.USER_ID }
            });
        getrolellist.then(function (role) {
            $scope.roles = role.data;
        }, function () {
            alert('Data not found');
        });
    }
    //Not in usage
    $scope.GetState = function () {
        BindState();
        $scope.getcities = {};
    }
    //Not in usage
    function BindState()
    {
        if ($scope.selectedcountry != "") {
            var getstatelist = $http.get("/ET_Admin_User_Master/BindStates",
                {
                    params: { id: $scope.selectedcountry }
                });
            getstatelist.then(function (state) {
                $scope.getstates = state.data;
            }, function () {
                alert("No Data Found");
            });
        }
        else {
            $scope.getstates = {};
            $scope.getcities = {};
        }
    }
    // Not in Usage
    $scope.GetCity = function () {
        if ($scope.selectedstate != "") {
            var getcitylist = $http.get("/ET_Admin_User_Master/BindCities",
                {
                    params: { id: $scope.selectedstate }
                });
            getcitylist.then(function (city) {
                $scope.getcities = city.data;
            }, function () {
                alert("No Data Found");
            });
        }
        else { $scope.getcities = {}; }
    }
    //Not in Usage
    $scope.CityChange = function () {
    }
    //On Role Change
    $scope.RoleChange = function () {
    }
    //Show list of records
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetUserList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    //Show list of restore records
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetUserRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    }
    //Initialize input fields on create click
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.USER_ID = "0";
            $scope.USER_CODE = "";
            $scope.USER_NAME = "";
            $scope.DISPLAY_NAME = "";
            $scope.USER_PASSWORD = "";
            $scope.USER_PASSWORDConfirm = "";
            $scope.USER_STREET = "";
            $scope.USER_ZIP = "";
            $scope.USER_EMAIL = "";
            $scope.USER_FAX = "";
            $scope.USER_PHONE = "";
            $scope.USER_MOBILE = "";
            $scope.USER_REMARKS = "";
            $("#drpCountry").val("").trigger("chosen:updated");
            $scope.selectedrole = "";
            $scope.selectedcountry = "";
            $scope.getstates = {};
            $scope.txtState = "";
            $scope.getcities = {};
            $scope.txtCity = "";
            $scope.selectedrole = "";
            GetRoles();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Add or Update a record
    $scope.SubmitClick = function () {
        $("#div_loadImage").css("display", "block");
        var post = $http({
            method: "POST",
            url: "/ET_Admin_User_Master/ET_Admin_UserMaster_Add",
            dataType: 'json',
            data: {
                UserID: $scope.USER_ID,
                txtUsercode: $scope.USER_CODE,
                txtUserName: $scope.USER_NAME,
                txtDisplayName: $scope.DISPLAY_NAME,
                txtPassword: $scope.USER_PASSWORD,
                drpRole: $scope.selectedrole,
                txtStreet: $scope.USER_STREET,
                drpCountry: $scope.selectedcountry,
                drpState: $scope.txtState,
                drpCity: $scope.txtCity,
                txtZipCode: $scope.USER_ZIP,
                txtEmail: $scope.USER_EMAIL,
                txtFax: $scope.USER_FAX,
                txtPhone: $scope.USER_PHONE,
                txtMobile: $scope.USER_MOBILE,
                txtRemarks: $scope.USER_REMARKS
            },
            headers: { "Content-Type": "application/json" }
        });

        post.success(function (data, status) {
            $("#div_loadImage").css("display", "none");
            if (data == "Session Expired") {
                $window.location.href = '/ET_Login/ET_SessionExpire';
            }
            else if (data.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(data);
                $("#spanErrMessage2").html(data);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
            else if (data.indexOf("Validation") > -1) {
                toastr["error"](data, "Notification");
            }
            else if (data.indexOf("Success") > -1) {
                var data1 = data.split('-');
                if ($scope.USER_ID == 0) {
                    message = 'Record Inserted Successfully With Code :' + data1[1];
                    toastr["success"](message, "Notification");
                }
                else {
                    message = 'Record Updated Successfully With Code :' + data1[1];
                    toastr["success"](message, "Notification");
                }
                $scope.createnew();
            }
            else {
                message = "Failed to do this operation.";
                toastr["error"](message, "Notification");
            }
        });

        post.error(function (data, status) {
            $("#div_loadImage").css("display", "none");
            $window.alert(data.Message);
        });

    }
    //Edit a record
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Admin_User_Master/ET_Admin_User_Update_Get",
                dataType: 'json',
                data: {
                    id: a
                },
                headers: { "Content-Type": "application/json" }
            });
            post.success(function (data, status) {
                if (data == "Session Expired") {
                    $window.location.href = '/ET_Login/ET_SessionExpire';
                }
                else if (data.indexOf("ERR") > -1) {
                    $("#spanErrMessage1").html(data);
                    $("#spanErrMessage2").html(data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else {
                    var UserByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    $scope.USER_ID = UserByID.USER_ID;
                    $scope.USER_CODE = UserByID.USER_CODE;
                    $scope.USER_NAME = UserByID.USER_NAME;
                    $scope.DISPLAY_NAME = UserByID.DISPLAY_NAME;
                    $scope.USER_PASSWORD = UserByID.USER_PASSWORD;
                    $scope.USER_PASSWORDConfirm = UserByID.USER_PASSWORD;
                    $scope.USER_STREET = UserByID.USER_STREET;
                    $scope.USER_ZIP = UserByID.USER_ZIP;
                    $scope.USER_EMAIL = UserByID.USER_EMAIL;
                    $scope.USER_FAX = UserByID.USER_FAX;
                    $scope.USER_PHONE = UserByID.USER_PHONE;
                    $scope.USER_MOBILE = UserByID.USER_MOBILE;
                    $scope.USER_REMARKS = UserByID.USER_REMARKS;
                    $("#drpCountry").val(UserByID.USER_COUNTRY).trigger("chosen:updated");
                    $scope.selectedrole = UserByID.ROLE_ID;
                    $scope.txtState = UserByID.USER_STATE;
                    $scope.txtCity = UserByID.USER_CITY;
                    $scope.selectedcountry = UserByID.USER_COUNTRY;
                    //$scope.selectedstate = UserByID.USER_STATE;
                    //BindState();
                    //$scope.selectedcity = UserByID.USER_CITY;
                    //$scope.GetCity();
                    $scope.selectedrole = UserByID.ROLE_ID;
                    GetRoles();
                    
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }

    
    ///* This function is used to confirm the deletion*/
    //$scope.deleteRecordConfirm = function (e, f) {
    //    debugger;
    //    var res = false;
    //    //if (b)
    //    //  res = $scope.Isdelete;
    //    //else
    //    //  res = $scope.Isrestore
    //    res = $scope.Isdelete;
    //    if (res) {
    //        var post = $http({
    //            method: "POST",
    //            url: "/ET_Admin_User_Master/ET_Admin_User_Master_RestoreDelete",
    //            dataType: 'json',
    //            data: {
    //                id: a,
    //                type: true
    //            },
    //            headers: { "Content-Type": "application/json" }
    //        });
    //        post.success(function (data, status) {
    //            if (data == "Session Expired") {
    //                $window.location.href = '/ET_Login/ET_SessionExpire';
    //            }
    //            else if (data.indexOf("ERR") > -1) {
    //                $("#spanErrMessage1").html(data);
    //                $("#spanErrMessage2").html(data);
    //                if ($("#exceptionmessage").length)
    //                    $("#exceptionmessage").trigger("click");
    //            }
    //            else {
    //                if (data == "Success") {
    //                    $($event.target.parentElement.parentElement.parentElement).fadeOut(800, function () { $(this).remove(); });
    //                    if (b) {
    //                        message = "Record Deleted Successfully.";
    //                    }
    //                    else {
    //                        message = "Record Restored Successfully.";
    //                    }

    //                    toastr["success"](message, "Notification");
    //                }
    //                else {
    //                    message = "Failed to do this operation.";
    //                    toastr["error"](message, "Notification");
    //                }
    //            }
    //        });
    //        post.error(function (data, status) {
    //            $window.alert(data.Message);
    //        });
    //    }
    //}
    //Restore or Delete a record
    $scope.Restoredeleterecords = function (a, $event, b) {
        if (b) {
            alertmessageModified($event, a.toString(), b, 'ET_Admin_User_Master', 'ET_Admin_User_Master_RestoreDelete');
        }
        else
        {
            $scope.PerformRestore(a, $event, b);
            // message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    };
    //Restore the deleted records
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_User_Master/ET_Admin_User_Master_RestoreDelete",
            dataType: 'json',
            data: {
                id: a,
                type: b
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            if (data == "Session Expired") {
                $window.location.href = '/ET_Login/ET_SessionExpire';
            }
            else if (data.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(data);
                $("#spanErrMessage2").html(data);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
            else {
                if (data == "Success") {
                    $($event.target.parentElement.parentElement.parentElement).fadeOut(800, function () { $(this).remove(); });
                    if (b) {
                        message = "Record Deleted Successfully.";
                    }
                    else {
                        message = "Record Restored Successfully.";
                    }

                    toastr["success"](message, "Notification");
                }
                else {
                    message = "Failed to do this operation.";
                    toastr["error"](message, "Notification");
                }
            }
        });
        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }

    //View a record
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Admin_User_Master/ET_Admin_User_View",
                dataType: 'html',
                data: {
                    id: a
                },
                headers: { "Content-Type": "application/json" }
            });
            post.success(function (data, status) {
                if (data == "Session Expired") {
                    $window.location.href = '/ET_Login/ET_SessionExpire';
                }
                else if (data.indexOf("ERR") > -1) {
                    $("#spanErrMessage1").html(data);
                    $("#spanErrMessage2").html(data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else {
                    $('#viewpopup').html(data);
                    $("#btnviewpopup").trigger("click");
                    //var privilage = JSON.parse(data);
                    //$scope.txtMaterialCodeView = privilage[0].MATERIAL_CODE;
                    //$scope.txtMaterialnameView = privilage[0].MATERIAL_NAME;
                    //$scope.txtDescriptionView = privilage[0].MATERIAL_DESCRIPTION;
                    //if (privilage[0].COTTON_PER == 1)
                    //    $scope.chkCottonView = true;
                    //else
                    //    $scope.chkCottonView = false;
                    //$rootScope.$emit("CallParentMethod", { a: $scope.txtMaterialCodeView, b: $scope.txtMaterialnameView, c: $scope.txtDescriptionView, d: $scope.chkCottonView });

                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    $scope.$watch("getstates", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpstate").val($scope.selectedstate).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("getcities", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpcity").val($scope.selectedcity).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("roles", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpRole").val($scope.selectedrole).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("userlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("userrestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
    
});