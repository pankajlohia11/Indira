app1.controller("AC_OrdertoDespatchCtrl", function ($scope, $http, $window, $compile) {
    GetPrivilages();
    debugger;
    var type1 = window.location.href.toString().split("&");
    var type = type1[1].split("OrderId=");
    var redirectval = type1[0].split("type");
    $scope.D_OrderID = type[1];
    function GetPrivilages() {
        $scope.D_ID = "0";
        $("#formSubmit").attr('disabled', "enabled");
        $("#div_View").css("display", "none");
        $("#div_Edit").css("display", "block");
        $("#div_Print").css("display", "none");
        $scope.submittext = "Submit";
        $scope.createedit = "Create";
        $scope.D_ID = "0";
        $scope.D_Code = "";
        $scope.D_DespatchDate = "";
        $scope.D_ModeOfTransport = "";
        $scope.D_VechileNo = "";
        $scope.D_TransporterName = "";
        $scope.D_DeliveryFrom = "";
        $scope.D_DeliveryTo = "";
        $scope.D_Remarks = "";
        var getprivilages = $http.get("/ET_Sales_Despatch/GetPrivilages");
        getprivilages.then(function (privilage1) {
            var privilage = JSON.parse(privilage1.data);
            if (privilage.length != 0) {
                $scope.Iscreate = privilage[0].IS_ADD;
                $scope.Isedit = privilage[0].IS_EDIT;
                $scope.Isdelete = privilage[0].IS_DELETE;
                $scope.Isrestore = privilage[0].IS_FULLCONTROL;
                $scope.Isview = privilage[0].IS_VIEW;
                GetCustomerList();
                GetSalesPersonList();
                GetStores();
            }
            else {
                $scope.Iscreate = true;
                $scope.Isedit = true;
                $scope.Isdelete = true;
                $scope.Isrestore = true;
                $scope.Isview = true;
            }
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    function GetCustomerList() {
    
        var getcustomerlist = $http.get("/ET_Sales_Despatch/GetCustomers",
            {
                params: {
                    id: $scope.D_ID,
                }
            });
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
            GetOrderDetails($scope.D_OrderID);
        }, function () {
            alert("Customer Data Not Found");
        });
    }
    function GetStores() {
        var getstorelist = $http.get("/ET_Sales_Despatch/GetStores",
            {
                params: {
                    id: $scope.D_ID,
                }
            });
        getstorelist.then(function (storelist) {
            var res = JSON.parse(storelist.data);
            $scope.storelist = res;
        }, function () {
            alert("Store Data Not Found");
        });
    }

    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_Despatch/GetSalesPerson");
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
          //  $scope.D_SalesPerson = "";
        }, function () {
            alert('Data not found');
        });
    }
    $scope.CustomerChange = function () {
       // $scope.D_OrderID = "";
        angular.element(document.getElementById('despatchdetailsbody')).html("");
        GetOrderList();
    }
    function GetOrderList() {
        debugger;
        if ($scope.D_CustomerID != "") {
            var getorderlist = $http.get("/ET_Sales_Despatch/Orders",
                {
                    params:
                        {
                            customerid: $scope.D_CustomerID,
                            despatchid: $scope.D_ID
                        }
                });
            getorderlist.then(function (orderlist) {

                var res = JSON.parse(orderlist.data);
                $scope.orderlist = res;
               // 
            }, function () {
                $scope.orders = {};
                $scope.D_OrderID = "";
            });
        }
        else {
            $scope.orders = {};
            $scope.D_OrderID = "";
        }
    }
    function GetOrderDetails(a) {
        debugger;
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Despatch/GetOrderDetails",
            dataType: 'html',
            data: {
                id: a,
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            debugger;
            var res = JSON.parse(data);
            $scope.D_CustomerID = res[0].SO_CutomerID;
            $("#drpCustomer").val($scope.D_CustomerID).trigger("chosen:updated");
            $scope.D_SalesPerson = res[0].SO_SalesPersonID;
            $("#drpSalesPerson").val($scope.D_SalesPerson).trigger("chosen:updated");
            $scope.CustomerChange();
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }
    $scope.OrderChanged = function () {
    }
    $scope.ClientOrderChanged = function (a) {
        debugger;
       $scope.D_OrderID = a;
        OrderChange();
    }
    function OrderChange() {
        if ($scope.D_OrderID != "" && $scope.D_OrderID != null) {
            if ($scope.D_StoreID != "") {
                var ords = "";
                try {
                    ords = $scope.D_OrderID.join();
                }
                catch (ex) {
                    ords = $scope.D_OrderID;
                }
                var post = $http({
                    method: "POST",
                    url: "/ET_Sales_Despatch/GetDespatchDetails",
                    dataType: 'html',
                    data:
                        {
                            ids: ords,
                            despatchid: $scope.D_ID,
                            storeid: $scope.D_StoreID
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
                        $('#despatchdetailsbody').html(data);
                    }
                });

                post.error(function (data, status) {
                    angular.element(document.getElementById('despatchdetailsbody')).html("");
                });
            }
            else {
                message = "Select Store";
                toastr["error"](message, "Notification");
                $scope.D_OrderID = "";
                $("#drpOrder").val("").trigger("chosen:updated");
                angular.element(document.getElementById('despatchdetailsbody')).html("");
            }
        }
        else {
            angular.element(document.getElementById('despatchdetailsbody')).html("");
        }
    }
    $scope.StoreChange = function () {
        $scope.ClientOrderChanged($scope.D_OrderID);
        angular.element(document.getElementById('despatchdetailsbody')).html("");
    }
    $scope.TransportChange = function () { }
    $scope.SalesPersonChange = function () {
    }
 
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#div_Print").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.D_ID = "0";
            $scope.D_Code = "";
            $scope.D_OrderID = "";
            $scope.D_CustomerID = "";
           // $scope.D_SalesPerson = "";
            $scope.D_StoreID = "";
            $scope.D_DespatchDate = "";
            $scope.D_ModeOfTransport = "";
            $scope.D_VechileNo = "";
            $scope.D_TransporterName = "";
            $scope.D_DeliveryFrom = "";
            $scope.D_DeliveryTo = "";
            $scope.D_Remarks = "";
            GetCustomerList();
            $("#txtDDate").val("");
            $("#drpTransport").val("").trigger("chosen:updated");
            $("#drpOrder").val("").trigger("chosen:updated");
           // $("#drpSalesPerson").val("").trigger("chosen:updated");
            angular.element(document.getElementById('despatchdetailsbody')).html("");
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    function validate() {
        var tr = $("#despatchdetailsbody").find("tr");
        var txt = "";
        if ($("#txtDDate").val() == "") {
            message = "Select Despatch Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.D_CustomerID == "") {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.D_StoreID == "") {
            ssss
            message = "Select Store";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.D_OrderID == "") {
            message = "Select Order";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.D_SalesPerson == "") {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.D_ModeOfTransport == "") {
            message = "Select Mode Of Transport";
            toastr["error"](message, "Notification");
            return "";
        }
        var total = 0;
        for (i = 0; i < tr.length; i++) {
            var td = $(tr[i]).find("td");
            var ordqty = $(td[7]).find("input").val();
            var avlqty = $(td[9]).find("input").val();
            var stkqty = $(td[8]).find("input").val();
            var desqty = $(td[10]).find("input").val();
            if (desqty == "") {
                message = "Enter Despatch Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(desqty.split('.').join("").replace(",", ".")) > parseFloat(avlqty.split('.').join("").replace(",", "."))) {
                message = "Despatch Quantity cannot be more than Pending quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(desqty.split('.').join("").replace(",", ".")) > parseFloat(stkqty.split('.').join("").replace(",", "."))) {
                message = "Despatch Quantity cannot be more than Available stock at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                txt = txt + $(td[1]).find("input").val() + ",";
                txt = txt + $(td[2]).find("input").val() + ",";
                txt = txt + $(td[3]).find("input").val() + ",";
                txt = txt + $(td[6]).find("input").val() + ",";
                txt = txt + parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + parseFloat($(td[10]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + $(td[11]).find("input").val() + "|";
                total = total + parseFloat($(td[10]).find("input").val().split('.').join("").replace(",", "."));
            }
        }
        if (total == 0) {
            message = "Atleast one Product has to be Despatched";
            toastr["error"](message, "Notification");
            return "";
        }
        return txt;
    }
    $scope.showRecords = function () {
        if (redirectval[1] == "=2") {
            window.location = '../ET_Sales_Shedule/ET_Sales_Shedule?type=Store';
        }
        else {
            window.location = '../ET_Sales_OrderDetails/ET_Sales_OrderDetails?type=Store';
            
        }
        
    }
    $scope.SubmitClick = function () {
        debugger;
        var txt = validate();
        var date = $("#txtDDate").val();
        debugger;
        var ords = "";
        try {
            ords = $scope.D_OrderID.join();
        }
        catch (ex) {
            ords = $scope.D_OrderID;
        }
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Despatch/ET_Sales_Despatch_Add",
                dataType: 'json',
                data: {
                    D_ID: $scope.D_ID,
                    D_Code: $scope.D_Code,
                    D_OrderID: ords,
                    D_CustomerID: $scope.D_CustomerID,
                    D_SalesPerson: $scope.D_SalesPerson,
                    D_StoreID: $scope.D_StoreID,
                    D_DespatchDate: date,
                    D_ModeOfTransport: $scope.D_ModeOfTransport,
                    D_VechileNo: $scope.D_VechileNo,
                    D_TransporterName: $scope.D_TransporterName,
                    D_DeliveryFrom: $scope.D_DeliveryFrom,
                    D_DeliveryTo: $scope.D_DeliveryTo,
                    D_Remarks: $scope.D_Remarks,
                    DespatchDetails: txt
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
                else if (data.indexOf("Success") > -1) {
                    var res = data.split(":");
                    if ($scope.D_ID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    if (redirectval[1] == "=2") {
                        setTimeout(function () {
                            window.location = '../ET_Sales_Shedule/ET_Sales_Shedule?type=Store';
                        }, 2000);
                    }
                    else {
                        setTimeout(function () {
                            window.location = '../ET_Sales_OrderDetails/ET_Sales_OrderDetails?type=Store';
                        }, 2000);
                    }
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

    }
   
    $scope.$watch("customers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.D_CustomerID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("orderlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpOrder").val($scope.D_OrderID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("D_OrderID", function (value) {
        setTimeout(function () { $("#drpOrder").val($scope.D_OrderID).trigger("chosen:updated"); }, 100);
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.D_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("storelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpStore").val($scope.D_StoreID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("despatchlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("despatchrestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
});