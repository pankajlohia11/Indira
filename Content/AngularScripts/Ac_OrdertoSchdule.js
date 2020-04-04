app1.controller("AC_OrdertoScheduleCtrl", function ($scope, $http, $window, $compile) {
    function GetTypeFromURL() {
        debugger;
        var type1 = window.location.href.toString().split("&");
        var type = type1[0].split("type=");
        var orderID = window.location.href.toString().split("orderID=");
        $scope.Type = type[1];
        $scope.orderId = orderID[1];
        if ($scope.Type == "Agency") {
            $scope.SH_Type = 1;
        }
        else if ($scope.Type == "Trading") {
            $scope.SH_Type = 2;
        }
        else if ($scope.Type == "Store") {
            $scope.SH_Type = 3;
        }
        $("#drpType").val($scope.SelectedOredertype).trigger("chosen:updated");
    }
    GetTypeFromURL();
    GetPrivilages();

    GetSalesPersonList();
    //Check the privilages
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Sales_Shedule/GetPrivilages");
        getprivilages.then(function (privilage1) {
            var privilage = JSON.parse(privilage1.data);
            if (privilage.length != 0) {
                $scope.Iscreate = privilage[0].IS_ADD;
                $scope.Isedit = privilage[0].IS_EDIT;
                $scope.Isdelete = privilage[0].IS_DELETE;
                $scope.Isrestore = privilage[0].IS_FULLCONTROL;
                $scope.Isview = privilage[0].IS_VIEW;
            }
            else {
                $scope.Iscreate = true;
                $scope.Isedit = true;
                $scope.Isdelete = true;
                $scope.Isrestore = true;
                $scope.Isview = true;
            }
            debugger;
            if ($scope.Iscreate) {
                $scope.submittext = "Submit";
                $scope.createedit = "Create";
                $scope.SH_ID = "0";
                $scope.SH_Code = "";
                $scope.SH_OrderID = "";
                $scope.SH_SalesPerson = "";
                $("#txtSchDate").val("");
                $("#txtSupSchDate").val("");
                $scope.SelectedSupplier = "0";
                $scope.SelectedCustomer = "";
                $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated");
                $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated");
                $("#drpOrder").val("").trigger("chosen:updated");
                $("#drpSalesPerson").val("").trigger("chosen:updated");
                $scope.orders = {};
                $("#scheduledetailsbody").html("");
                $scope.txtOrderdate = "";
                Customer();
                Supplier();
                GetOrderDetails();
            }
            else {
                message = "You don't access to do this operation, please contact admin.";
                toastr["error"](message, "Notification");
                window.location = '../ET_Sales_OrderDetails/ET_Sales_OrderDetails?type=' + $scope.Type;
            }
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    //Get order List
    function GetOrderDetails() {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shedule/GetOrderDetails",
            dataType: 'html',
            data: {
                id: $scope.orderId
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            debugger;
            var res = JSON.parse(data);
            $scope.SH_OrderID = res[0].SO_ID;
            $scope.SelectedCustomer = res[0].SO_CutomerID;
            $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated");
            $scope.Customerchange();
            $scope.OrderChange();
            $("#drpOrder").val($scope.SH_OrderID).trigger("chosen:updated");
            $scope.SH_SalesPerson = res[0].SO_SalesPersonID;
                $("#drpSalesPerson").val($scope.SH_SalesPerson).trigger("chosen:updated");
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }
    
    // Binding the customer only 
    function Customer() {
        debugger;
        if ($scope.SH_OrderID != "") {
            var id = $scope.SH_OrderID;
        }
        else {
            var id = "0";
        }
        $http({
            method: 'GET',
            url: '/ET_Sales_Shedule/GetCustomerSupplier',
            params: {
                id: id,
                custsup: 1
            },
        }).
            success(function (data) {
                var customer = JSON.parse(data)
                $scope.CustomerList = customer;

            });
    }
    // Binding the supplier only
    function Supplier() {
        if ($scope.SH_OrderID != "") {
            var id1 = $scope.SH_OrderID;
        }
        else {
            var id1 = "0";
        }

        $http({
            method: 'POST',
            url: '/ET_Sales_Shedule/GetCustomerSupplier',
            data: {
                id: id1,
                custsup: 0
            },
        }).
            success(function (data) {
                var Supplier = JSON.parse(data)
                $scope.SupplierList = Supplier;
            });
    }
    //Get the order list
    function GetOrders(CustomerId) {
        if ($scope.SH_Type != "") {
            var getorderlist = $http.get("/ET_Sales_Shedule/GetOrders",
                {
                    params: {
                        type: $scope.SH_Type,
                        CustomerId: CustomerId
                    }
                });
            getorderlist.then(function (orderlist) {
                var res = JSON.parse(orderlist.data);
                $scope.orders = res;
            }, function () {
                alert("Customer Not Found");
            });
        }
        else {
            $scope.orders = {};
        }
    }
    //Get the salesperson list
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_Shedule/GetSalesPerson");
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
            $scope.SH_SalesPerson = "";
        }, function () {
            alert('Data not found');
        });
    }
    $scope.OrderTypeChange = function () {

        $scope.SH_OrderID = "";

    }
    $scope.Customerchange = function () {
        GetOrders($scope.SelectedCustomer);
    }
    //Change order function
    $scope.OrderChange = function ()
    {
        var orderId = $scope.SH_OrderID;
        var supplierPost = $http({
            method: "POST",
            url: "/ET_Sales_Shedule/GetSupplierPO",
            dataType: 'html',
            data: {
                orderCode: orderId
            },
            headers: { "Content-Type": "application/json" }
        });
        supplierPost.success(function (data, status) {
            var res = JSON.parse(data);
            $scope.SupplierPO = res;
            $("#txtSupplierPO").val($scope.SupplierPO);
        });

        supplierPost.error(function (data, status) {
            $window.alert(data.Message);
        });

        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shedule/ET_Sales_ScheduleDetails",
            dataType: 'html',
            data: {
                id: $scope.SH_OrderID,
                type: $scope.submittext,
                code: $scope.SH_Code,
                scheduleType: $scope.SH_Type
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
                $('#scheduledetailsbody').html(data);
                getSupplierNameOrderDate();
                getSupplierScheduleDate();
            }
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }

    function getSupplierNameOrderDate() {

        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shedule/getSupplierNameOrderDate",
            dataType: 'html',
            data: {
                id: $scope.SH_OrderID,
                type: $scope.submittext,
                code: $scope.SH_Code
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            debugger;
            var res = JSON.parse(data);
            $scope.SelectedSupplier = res[0].SO_SupplierID;
            $scope.SelectedCustomer = res[0].SO_CutomerID;
            $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated");
            $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated");
            var SHDATE = new Date(parseInt(res[0].SO_OrderDate.substr(6)));
            $scope.txtOrderdate = ("0" + SHDATE.getDate()).slice(-2) + "-" + ("0" + (SHDATE.getMonth() + 1)).slice(-2) + "-" + SHDATE.getFullYear();
            $("#txtOrderdate").val($scope.txtOrderdate);
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }

    function getSupplierScheduleDate() {

        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shedule/getSupplierScheduleDate",
            dataType: 'html',
            data: {
                id: $scope.SH_OrderID,
                type: $scope.submittext,
                code: $scope.SH_Code
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            var res = JSON.parse(data);
            var SupDate = new Date(parseInt(res[0].Po_DeliveryDate.substr(6)));
            $scope.txtSupSchDate = ("0" + SupDate.getDate()).slice(-2) + "-" + ("0" + (SupDate.getMonth() + 1)).slice(-2) + "-" + SupDate.getFullYear();
            $("#txtSupSchDate").val($scope.txtSupSchDate);
        });

        post.error(function (data, status) {
            //$window.alert(data.Message);
            $scope.txtSupSchDate = $scope.txtOrderdate;
        });
    }

    $scope.SalesPersonChange = function () {
    }
    //View the schdule list
    $scope.showRecords = function () {
        window.location = '../ET_Sales_OrderDetails/ET_Sales_OrderDetails?type=' + $scope.Type;
    }
    
    //Create the new schdule
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.SH_ID = "0";
            $scope.SH_Code = "";
            $scope.SH_OrderID = "";
            $scope.SH_SalesPerson = "";
            $("#txtSchDate").val("");
            $("#txtSupSchDate").val("");
            $scope.SelectedSupplier = "0";
            $scope.SelectedCustomer = "";
            $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated");
            $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated");
            $("#drpOrder").val("").trigger("chosen:updated");
            $("#drpSalesPerson").val("").trigger("chosen:updated");
            $scope.orders = {};
            $("#scheduledetailsbody").html("");
            $scope.OrderTypeChange();
            $scope.txtOrderdate = "";
            Customer();
            Supplier();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Validate the data
    function validate() {
        var tr = $("#scheduledetailsbody").find("tr");
        var txt = "";
        if ($("#txtSchDate").val() == "") {
            message = "Select Schedule Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtSupSchDate").val() == "") {
            message = "Supplier Schedule Date is Empty";
            toastr["error"](message, "Notification");
            return "";
        }

        var orderSchDate = $("#txtSchDate").val();
        var supSchDate = $("#txtSupSchDate").val();
        var dateRegex = /^(\d{1,2})\-(\d{1,2})\-(\d{4})$/;
        var orderSchArrayDate = orderSchDate.match(dateRegex);
        var supSchArrayDate = supSchDate.match(dateRegex);
        var expectedOrderSchDate = new Date(orderSchArrayDate[3], orderSchArrayDate[2], orderSchArrayDate[1]);
        var expectedSupSchDate = new Date(supSchArrayDate[3], supSchArrayDate[2], supSchArrayDate[1]);

        //Add the Validation for the Customer Schedule Date and Supplier Schedule Date.
        if (expectedOrderSchDate <= expectedSupSchDate) {
            message = "Customer Order Schedule Date should be greater than Supplier Schedule Date";
            toastr["error"](message, "Notification");
            return "";
        }

        var total = 0;
        for (var i = 0; i < tr.length; i++) {
            var td = $(tr[i]).find("td");
            var availableqty = $(td[6]).find("input").val();
            var qty = $(td[7]).find("input").val();
            if (qty == "") {
                message = "Enter Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(qty.split('.').join("").replace(",", ".")) > parseFloat(availableqty.split('.').join("").replace(",", "."))) {
                message = "Quantity cannot be more than Available Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                txt = txt + $(td[1]).find("input").val() + ",";
                txt = txt + $(td[2]).find("input").val() + ",";
                txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                debugger;
                if ($scope.SH_Type == 2) {
                    txt = txt + parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                    txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                }
                else {
                    txt = txt + parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                }
                total = total + parseFloat(qty.split('.').join("").replace(",", "."));
            }
        }
        if (tr.length == 0)
        {
            message = "Atleast one product must be scheduled";
            toastr["error"](message, "Notification");
            return "";
        }
        return txt;
    }
    //Save the records
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var txt = validate();

        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shedule/ET_Sales_Schedule_Add",
                dataType: 'json',
                data: {
                    SH_ID: $scope.SH_ID,
                    SH_Code: $scope.SH_Code,
                    SH_Type: $scope.SH_Type,
                    SH_OrderID: $scope.SH_OrderID,
                    SH_DATE: $("#txtSchDate").val(),
                    ScheduleDetails: txt,
                    SH_SalesPerson: $scope.SH_SalesPerson,
                    SUP_SH_DATE: $("#txtSupSchDate").val()
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
                    var res = data.split(":");
                    if ($scope.SH_ID == 0) {
                        message = 'Record Inserted Successfully With Code :' + res[1];
                        toastr["success"](message, "Notification");
                        setTimeout(function () {
                            window.location = '../ET_Sales_OrderDetails/ET_Sales_OrderDetails?type=' + $scope.Type;
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
                $("#div_loadImage").css("display", "none");
            });
        }
        else {
            $("#div_loadImage").css("display", "none");
        }

    }
    
    function ScheduleDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Sales_Shedule/ET_Sales_ScheduleDetails',
            data: {
                id: id,
                type: $scope.submittext,
                code: $scope.SH_Code,
                scheduleType: $scope.SH_Type
            },
        }).success(function (data) {

            var enquirydata = JSON.parse(data)
            angular.element(document.getElementById('quotationdetailsbody')).html("");
        });
    }
    //Check the restore or delete
   
    //View: Popup view
    
    //Watch for all data binding
    $scope.$watch("CustomerList", function (value) {
        var val = value || null;
        if (val) {
            // alert(val);
            setTimeout(function () { $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated"); }, 5);
        }
    });

    $scope.$watch("SupplierList", function (value) {
        setTimeout(function () { $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated"); }, 5);
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.SH_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("orders", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpOrder").val($scope.SH_OrderID).trigger("chosen:updated"); }, 100);
        }
    });
   

});