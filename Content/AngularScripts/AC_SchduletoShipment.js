app1.controller("AC_SchduletoShipmentCtrl", function ($scope, $http, $window, $compile)
{
    function GetTypeFromURL()
    {
        var type1 = window.location.href.toString().split("&");
        var type = type1[0].split("type=");
        var SchduleId = window.location.href.toString().split("SchduleID=");
        $scope.Type = type[1];
        $scope.SchduleId = SchduleId[1];
        if ($scope.Type == "Agency") {
            $scope.S_Type = 1;
        }
        else if ($scope.Type == "Trading") {
            $scope.S_Type = 2;
        }
        else if ($scope.Type == "Store") {
            $scope.S_Type = 3;
        }
        $("#drpType").val($scope.SelectedOredertype).trigger("chosen:updated");
    }

    GetTypeFromURL();
    GetPrivilages();
    GetSalesPersonList();

    function GetPrivilages()
    {
        $scope.CompanyType = "Customer";
        var getprivilages = $http.get("/ET_Sales_Shipment/GetPrivilages");
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
            if ($scope.Iscreate) {
                $scope.submittext = "Submit";
                $scope.createedit = "Create";
                $scope.S_ID = "0";
                $scope.S_Code = "";
                $scope.S_CustSup = "";
                $scope.S_SupplierID = "";
                $scope.S_ScheduleID = "";
                $scope.S_SalesPerson = "";

                $("#txtETD").val("");
                $("#txtInvoiceAmount").val("");
                $("#txtETA").val("");
                $scope.S_DeparturePort = "";
                $scope.S_ArrivalPort = "";
                $scope.S_BL_NO = "";
                $("#txtBLDate").val("");
                $scope.S_INV_NO = "";
                $("#txtINVDate").val("");
                $scope.S_INV_AMT = "";
                $scope.C_INV_AMT = "";
                $scope.custsups = {};
                $scope.supplierlist = {};
                $scope.S_GrossWeight = "";
                $scope.S_ContainerNo = "";
                $scope.S_MotherVessel = "";
                $scope.S_FleederVessel = "";
                $("#drpCustSup").val("").trigger("chosen:updated");
                $("#drpScheduleID").val("").trigger("chosen:updated");
                $("#drpSalesPerson").val("").trigger("chosen:updated");
                $scope.schedules = {};
                $("#shipmentdetailsbody").html("");
                $scope.OrderTypeChange();
                GetSchduleDetails($scope.SchduleId);

            }
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    function GetSchduleDetails(a) {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shipment/GetSchduleDetails",
            dataType: 'html',
            data: {
                id: a,
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            debugger;
            var res = JSON.parse(data);
            $scope.S_SupplierID = res[0].SO_SupplierID;
            $scope.S_CustSup = res[0].SO_CutomerID;
            $("#drpCustSup").val($scope.S_CustSup).trigger("chosen:updated");
            $("#drpSupplierID").val($scope.S_SupplierID).trigger("chosen:updated");
            $scope.S_SalesPerson = res[0].SH_SalesPerson;
            $("#drpSalesPerson").val($scope.S_SalesPerson).trigger("chosen:updated");
            $scope.SupplierChange();
            $scope.ClientSch(a);
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_Shipment/GetSalesPerson",
            {
                params: {
                    orgtype: $scope.S_Type
                }
            });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
            $scope.S_SalesPerson = "";
        }, function () {
            alert('Data not found');
        });
    }

    $scope.OrderTypeChange = function () {
        $scope.S_SupplierID = "";
        $scope.S_CustSup = "";
        $scope.S_ScheduleID = "";
        $scope.schedules = {};
        if ($scope.S_Type == 1) {
            $scope.CompanyType = "Supplier";
        }
        else {
            $scope.CompanyType = "Customer";
        }
        GetCustomers();
        GetSuppliers();
    }
    $scope.CustSupChange = function () {
        $scope.schedules = {};
        $scope.S_ScheduleID = "";
        GetSchedules();
    }
    $scope.SupplierChange = function () {
        $scope.schedules = {};
        $scope.S_ScheduleID = "";
        GetSchedules();
    }
    function GetCustomers() {
        if ($scope.S_Type != "") {
            var getsalespersonlist = $http.get("/ET_Sales_Shipment/GetCustSup", {
                params: {
                    type: 1
                }
            });
            getsalespersonlist.then(function (salespersonlist) {
                var res = JSON.parse(salespersonlist.data);
                $scope.custsups = res;
            }, function () {
                alert('Data not found');
            });
        }
        else {
            $scope.schedules = {};
        }
    }
    function GetSuppliers() {
        if ($scope.S_Type != "") {
            var getsalespersonlist = $http.get("/ET_Sales_Shipment/GetCustSup", {
                params: {
                    type: 0
                }
            });
            getsalespersonlist.then(function (salespersonlist) {
                var res = JSON.parse(salespersonlist.data);
                $scope.supplierlist = res;
            }, function () {
                alert('Data not found');
            });
        }
        else {
            $scope.schedules = {};
        }
    }
    function GetSchedules() {
        if ($scope.S_CustSup != "" && $scope.S_SupplierID != "") {
            var getsalespersonlist = $http.get("/ET_Sales_Shipment/GetSchedules", {
                params: {
                    id: $scope.S_ID,
                    custsupid: $scope.S_CustSup,
                    supplierid: $scope.S_SupplierID,
                    ordertype: $scope.S_Type,
                    type: $scope.submittext
                }
            });
            getsalespersonlist.then(function (salespersonlist) {
                var res = JSON.parse(salespersonlist.data);
                $scope.schedules = res;
            }, function () {
                alert('Data not found');
            });
        }
        else {
            $scope.schedules = {};
        }
    }

    $scope.SalesPersonChange = function () { };

    $scope.ScheduleChange = function () {
    };

    $scope.ClientSch = function (schids) {
        debugger;
        $scope.S_ScheduleID = schids;
        //var schids = "";
        try {
            schids = $scope.S_ScheduleID.join();
        }
        catch (ex) {
            schids = $scope.S_ScheduleID;
        }
        ShipmentDetails(schids);
    };

    $scope.showRecords = function () {
        window.location = '../ET_Sales_Shedule/ET_Sales_Shedule?type=' + $scope.Type;
    }
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetShipmentRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.S_ID = "0";
            $scope.S_Code = "";
            $scope.S_CustSup = "";
            $scope.S_SupplierID = "";
            $scope.S_ScheduleID = "";
            $scope.S_SalesPerson = "";

            $("#txtETD").val("");
            $("#txtInvoiceAmount").val("");
            $("#txtCusInvoiceAmount").val("");
            $("#txtETA").val("");
            $scope.S_DeparturePort = "";
            $scope.S_ArrivalPort = "";
            $scope.S_BL_NO = "";
            $("#txtBLDate").val("");
            $scope.S_INV_NO = "";
            $("#txtINVDate").val("");
            $scope.S_INV_AMT = "";
            $scope.custsups = {};
            $scope.supplierlist = {};
            $scope.S_GrossWeight = "";
            $scope.S_ContainerNo = "";
            $scope.S_MotherVessel = "";
            $scope.S_FleederVessel = "";
            $("#drpCustSup").val("").trigger("chosen:updated");
            $("#drpScheduleID").val("").trigger("chosen:updated");
            $("#drpSalesPerson").val("").trigger("chosen:updated");
            $scope.schedules = {};
            $("#shipmentdetailsbody").html("");
            $scope.OrderTypeChange();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    function validate()
    {
        debugger;
        var tr = $("#shipmentdetailsbody").find("tr");
        var txt = "";
        if ($scope.S_SalesPerson == "") {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_Type == "") {
            message = "Select Order Type";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_CustSup == "") {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_SupplierID == "") {
            message = "Select Supplier";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_ScheduleID == "") {
            message = "Select Schedule Codes";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtETD").val() == "") {
            message = "Select ETD Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtETA").val() == "") {
            message = "Select ETA Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_DeparturePort == "") {
            message = "Enter Departure Port";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_ArrivalPort == "") {
            message = "Enter Arrival Port";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_BL_NO == "") {
            message = "Enter BL No";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtBLDate").val() == "") {
            message = "Select BL Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_INV_NO == "") {
            message = "Enter Invoice No";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtINVDate").val() == "") {
            message = "Select Invoice Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.S_GrossWeight == "") {
            message = "Enter Gross Weight";
            toastr["error"](message, "Notification");
            return "";
        }
        if (parseFloat($scope.S_GrossWeight.split('.').join("").replace(",", ".")) == 0) {
            message = "Gross Weight Cannot be empty";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtInvoiceAmount").val() == "") {
            message = "Invoice Amount Cannot be empty";
            toastr["error"](message, "Notification");
            return "";
        }
        if (parseFloat($("#txtInvoiceAmount").val().split('.').join("").replace(",", ".")) == 0) {
            message = "Invoice Amount Cannot be empty";
            toastr["error"](message, "Notification");
            return "";
        }
        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
            //var scheduledate = $(td[4]).find("input").val();
            var availableqty = 0;
            var qty = 0;
            if ($scope.S_Type == 2)
            {
                availableqty=$(td[8]).find("input").val();
                qty= $(td[9]).find("input").val();
            }
            else {
                availableqty = $(td[8]).find("input").val();
                qty = $(td[9]).find("input").val();
            }
            //if (scheduledate == "") {
            //    message = "Enter Schedule Date at row:" + (i + 1);
            //    toastr["error"](message, "Notification");
            //    return "";
            //}
            if (qty == "") {
                message = "Enter Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(qty.split('.').join("").replace(",", ".")) > parseFloat(availableqty.split('.').join("").replace(",", "."))) {
                if ($scope.S_Type == "3")
                {
                    message = "Quantity cannot be more than Available Quantity at row:" + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else {
                    txt = txt + $(td[1]).find("input").val() + ",";
                    txt = txt + $(td[2]).find("input").val() + ",";
                    txt = txt + $(td[3]).find("input").val() + ",";
                    txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                    if ($scope.S_Type == 2)
                    {
                        //txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                        txt = txt + parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                    }
                    else {
                        txt = txt + parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                    }
                    // }

                }
            }
            else {
                txt = txt + $(td[1]).find("input").val() + ",";
                txt = txt + $(td[2]).find("input").val() + ",";
                txt = txt + $(td[3]).find("input").val() + ",";
                if ($scope.S_Type == 2)
                {
                    txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                    txt = txt + parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                }
                else
                {
                    txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                    txt = txt + parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", ".")) + "|";
                }
            }
        }
        return txt;
    };

    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var txt = validate();
        var schids = "";
        try
        {
            schids = $scope.S_ScheduleID.join();
        }
        catch (ex)
        {
            schids = $scope.S_ScheduleID;
        }
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shipment/ET_Sales_Shipment_Add",
                dataType: 'json',
                data: {
                    S_ID: $scope.S_ID,
                    S_Code: $scope.S_Code,
                    S_Type: $scope.S_Type,
                    S_CustSup: $scope.S_CustSup,
                    S_SupplierID: $scope.S_SupplierID,
                    S_SalesPerson: $scope.S_SalesPerson,
                    S_ScheduleID: schids,
                    S_ETD: $("#txtETD").val(),
                    S_ETA: $("#txtETA").val(),
                    S_DeparturePort: $scope.S_DeparturePort,
                    S_ArrivalPort: $scope.S_ArrivalPort,
                    S_BL_NO: $scope.S_BL_NO,
                    S_BL_DATE: $("#txtBLDate").val(),
                    S_INV_NO: $scope.S_INV_NO,
                    S_INV_DATE: $("#txtINVDate").val(),
                    S_INV_AMT: parseFloat($("#txtInvoiceAmount").val().split('.').join("").replace(",", ".")),
                    C_INV_AMT: parseFloat($("#txtCusInvoiceAmount").val().split('.').join("").replace(",", ".")),
                    S_GrossWeight: parseFloat($scope.S_GrossWeight.split('.').join("").replace(",", ".")),
                    S_ContainerNo: $scope.S_ContainerNo,
                    S_MotherVessel: $scope.S_MotherVessel,
                    S_FleederVessel: $scope.S_FleederVessel,
                    shipmentdetails: txt,
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
                    if ($scope.S_ID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                        setTimeout(function () {
                            window.location = '../ET_Sales_Shedule/ET_Sales_Shedule?type=' + $scope.Type;
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

    };
   
    function ShipmentDetails(a) 
    {
        debugger;
        $http({
            method: 'POST',
            url: '/ET_Sales_Shipment/ET_Sales_ShipmentDetails',
            dataType: 'html',
            data: {
                ids: a,
                scheduleType: $scope.S_Type,
                redirect:true
            },
        }).success(function (data) {
            debugger;
            angular.element(document.getElementById('shipmentdetailsbody')).html(data);
            var tr = angular.element(document.getElementById('shipmentdetailsbody')).find("tr");
            var total = 0.0;
            var cusTotal = 0.0;
            for (var i = 0; i < tr.length; i++)
            {
                var td = $(tr[i]).find("td");
                if (td != null)
                {
                    var price = parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", "."));
                    var supplierPrice = parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", "."));
                    if ($scope.S_Type == 1)
                        supplierPrice = price;
                    var shipmentQty = parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", "."));
                    var rowtotal = price * shipmentQty;
                    var rowSupplierTotal = supplierPrice * shipmentQty;

                    //total = total + rowtotal;
                    total = total + rowSupplierTotal;
                    $(td[10]).find("input").val(rowtotal.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    //call the invoice amount calculation.
                    //calinvamount(price);
                    cusTotal = cusTotal + rowtotal;
                }
            }
            $("#txtInvoiceAmount").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            $("#txtCusInvoiceAmount").val(cusTotal.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            $scope.CUS_INV_AMT = cusTotal;
            //GetSchduleDetails(a);
        });
    };
   
    $scope.$watch("salespersons", function (value)
    {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.S_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("custsups", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustSup").val($scope.S_CustSup).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("supplierlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSupplierID").val($scope.S_SupplierID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("schedules", function (value) {
        debugger;
        var val = value || null;
        if (val) {
            setTimeout(function () {
                debugger;
                $("#drpScheduleID").val($scope.S_ScheduleID).trigger("chosen:updated");
            }, 100);
        }
    });
    $scope.$watch("S_ScheduleID", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpScheduleID").val($scope.S_ScheduleID).trigger("chosen:updated"); }, 100);
        }
    });

});