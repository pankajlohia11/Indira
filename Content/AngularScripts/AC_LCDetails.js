app1.controller("AC_LcdetailsCtrl", function ($scope, $http, $window) {
    GetAC_LcdetailsList();
    GetAC_LcdetailsRestoreList();
    GetPrivilages();
    //Customer();
    Bind_Currency();
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Admin_LC_Details/GetPrivilages");
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

    // List page in LC Details
    function GetAC_LcdetailsList() {
        $scope.SelectedCustomer = "0";
        var getac_lcdetailslist = $http.get("/ET_Admin_LC_Details/GetAC_LcdetailsList",
            {
                params: { deleted: false }
            });
        getac_lcdetailslist.then(function (ac_lcdetails) {
            var res = JSON.parse(ac_lcdetails.data);

            $scope.lcdetailslist = res;
        }, function () {
            alert("No Data Found");
        });
    }

    // Restore page in LC Details
    function GetAC_LcdetailsRestoreList() {
        var getac_lcdetailstorelist = $http.get("/ET_Admin_LC_Details/GetAC_LcdetailsList",
            {
                params: { deleted: true }
            });
        getac_lcdetailstorelist.then(function (ac_lcdetails) {
            var res = JSON.parse(ac_lcdetails.data);
            $scope.lcdetailrestorelist = res;
        }, function () {
            alert("No Data Found");
        });
    }

    // Binding the customer only 
    $scope.Customer = function () {
        var id = "1";
        $http({
            method: 'GET',
            url: '/ET_Admin_LC_Details/Bind_dropdown_Banknameandcussup',
            params: {
                id: id,
            },
        }).
            success(function (data) {
                var customer = JSON.parse(data)
                $scope.CustomerList = customer;

            });
    }

    // Binding the supplier only
    $scope.Supplier = function () {
        var id1 = 0;
        $http({
            method: 'POST',
            url: '/ET_Admin_LC_Details/Bind_dropdown_Banknameandcussup',
            data: {
                id: id1,
            },
        }).
            success(function (data) {
                var customer = JSON.parse(data)
                $scope.CustomerList = customer;
            });
    }

    // Binding expiry date
    $scope.dateexpiry = function () {
        var day = $scope.txtLCPeriod;
        // var date = $scope.LCcalendar;
        var date = $("#LCcalendar").val();
        if (date) {
            $http({
                method: 'GET',
                url: '/ET_Admin_LC_Details/Add_Days',
                params: {
                    EndDate: date,
                    day: day
                },
            }).success(function (result) {
                $scope.Expirydate = result;
            });
        }
        else {
            $scope.txtLCPeriod = "";
            message = "Select LC Date.";
            toastr["error"](message, "Notification");
        }
    }

    // Binding currency 
    function Bind_Currency() {
        $http({
            method: 'GET',
            url: '/ET_Admin_LC_Details/Bind_Currency',

        }).success(function (result) {
            var res = JSON.parse(result)
            $scope.Currencylist = res;
            $scope.SelectedCurrency = "0";
        });
    }

    //function balamnt() {
    //    var balamt = $scope.txtamount;
    //    $scope.txtBalAmount = balamt;
    //};
    
    $scope.showRecords = function () {
        GetAC_LcdetailsList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    $scope.restoreRecords = function () {
        GetAC_LcdetailsRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    }

    // Create new record 
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#drpCurrency").val("0").trigger("chosen:updated");
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.Supplier();
            $("#chkEuroTexttiles").attr("checked", "checked");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";

            $scope.txtLCCode = "";
            $scope.txtLCID = "0";
            $scope.txtamount = "";
            $scope.drpCurrency = "";
            $scope.txtLCPeriod = "";
            $scope.Expirydate = "";
            $scope.drpBankName = "";
            $scope.drpCustomerName = "";
            $scope.LCcalendar = "";
            $scope.txtBalAmount = "";
            $scope.txtLCNo = "";
            $("#LCcalendar").val("");
            //  $scope.txtLCNo = "";
            // $scope.chkEuroTexttiles = false;
            // $scope.chkCustomerLC = false;
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }

    // Insert and update function
    $scope.SubmitClick = function () {

        if ($scope.SelectedCustomer != "0") {

            if ($("#chkCustomerLC").is(':checked')) {
                Cussup = 0;
            }
            else {
                Cussup = 1;
            }
            var post = $http({
                method: "POST",
                url: "/ET_Admin_LC_Details/ET_Admin_LC_Details_Add",
                dataType: 'json',
                data: {
                    txtLCCode: $scope.txtLCCode,
                    txtLCID: $scope.txtLCID,
                    Cussup: Cussup,
                    txtLCNo: $scope.txtLCNo,
                    LCcalendar: $("#LCcalendar").val(),
                    Expirydate: $scope.Expirydate,
                    drpCustomer: $scope.SelectedCustomer,
                    drpCurrency: $scope.SelectedCurrency,
                    txtamount: $scope.txtamount,
                    BalAmount: $scope.txtBalAmount
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
                else if (data.indexOf("Validation") > -1) {
                    toastr["error"](data, "Notification");
                }
                else if (data == "Success") {
                    if ($scope.txtLCID == 0) {
                        message = 'Record Inserted Successfully.';
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully.';
                        toastr["success"](message, "Notification");
                    }
                    GetAC_LcdetailsList();
                    $scope.createnew();
                }
                else {
                    message = "Failed to do this operation.";
                    toastr["error"](message, "Notification");
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else {
            message = "Select customer / supplier name.";
            toastr["error"](message, "Notification");
        }
    }

    // Edit function in LC details
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var get = $http({
                method: "POST",
                url: "/ET_Admin_LC_Details/ET_Admin_LC_Details_Update_GetbyID",
                dataType: 'json',
                data: {
                    id: a
                },
                headers: { "Content-Type": "application/json" }
            });
            get.success(function (data, status) {
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
                    var res = JSON.parse(data);
                    if (res.CUST_SUPP == "0") {
                        $("#chkCustomerLC").prop("checked", true);
                        $scope.SelectedCustomer = res.CUST_ID;                        
                        $scope.Customer();
                        $("#drpCustomer").val(res.CUST_ID).trigger("chosen:updated");
                    }
                    else {
                        $("#chkEuroTexttiles").prop("checked", true);
                        $scope.SelectedCustomer = res.CUST_ID;
                        $scope.Supplier();
                        $("#drpCustomer").val(res.CUST_ID).trigger("chosen:updated");
                    }
                    $scope.SelectedCurrency = res.CURRENCY_ID;
                    $("#drpCurrency").val(res.CURRENCY_ID).trigger("chosen:updated");
                    $scope.txtLCCode=res.LC_CODE;
                    $scope.txtLCID = res.LC_ID;                    
                    $scope.txtLCNo = res.LC_NO;

                    var date = new Date(parseInt(res.LC_DATE.substr(6)));
                    var newDate = ("0" + (date.getMonth() + 1)).slice(-2) + "/" + ("0" + date.getDate()).slice(-2) + "/" + date.getFullYear();

                    var date1 = new Date(parseInt(res.LC_EXPIRYDATE.substr(6)));
                    var expDate = ("0" + (date1.getMonth() + 1)).slice(-2) + "/" + ("0" + date1.getDate()).slice(-2) + "/" + date1.getFullYear();

                    var differenceinDays = parseInt(moment.duration(moment($scope.Expirydate = res.LC_EXPIRYDATE).diff(moment($scope.LCcalendar = res.LC_DATE))).asDays());
                    $scope.txtLCPeriod = differenceinDays;                    
                  
                    $scope.LCcalendar = newDate;
                    $scope.Expirydate = expDate;
                    $scope.txtamount = res.LC_AMOUNT;
                    $scope.txtBalAmount = res.LC_BAL_AMT;
                    $scope.drpCurrency = res.CURRENCY_ID;

                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                }
            });

            get.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }

    // Delete and restore records in LC details
    $scope.PerformRestore = function (a, $event, b)
    {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_LC_Details/ET_Admin_LC_Details_Restore_Insert",
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
    $scope.Restoredeleterecords = function (a, $event, b) {
        if (b) {
            alertmessageModified($event, a.toString(), b, 'ET_Admin_LC_Details', 'ET_Admin_LC_Details_Restore_Insert');
        }
        else {
            $scope.PerformRestore(a, $event, b);
        }
    } 

    // View function in LC details
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Admin_LC_Details/ET_Admin_LC_Details_View",
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

    // Dropdown using these are methods in LC details
    $scope.$watch("CustomerList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated"); }, 100);
            setTimeout(function () { $("#drpBank").val($scope.SelectedCustomer).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("Currencylist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCurrency").val($scope.selectedstate).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.CustChange = function () {
        $("#drpBank").val($("#drpCustomer").val()).trigger("chosen:updated");
        $scope.Selectedbank = $("#drpCustomer").val();
        $scope.BankChange();
    }
    $scope.BankChange = function () {
    }
    $scope.CurrencyChange = function () {
    }

}); 


