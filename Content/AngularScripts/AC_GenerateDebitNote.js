app1.controller("AC_GenerateDebiNoteCtrl", function ($scope, $http, $window, $compile) {
    function GetTypeFromURL() {
        var type = window.location.href.toString().split("type=");
        $scope.Type = type[1];
        if ($scope.Type == "Agency") {
            $scope.S_Type = "1";
        }
        else if ($scope.Type == "Trading") {
            $scope.S_Type = "2";
        }
        else if ($scope.Type == "Store") {
            $scope.S_Type = "3";
        }
        $("#drpType").val($scope.SelectedOredertype).trigger("chosen:updated");
    }
    GetTypeFromURL();
    GetPrivilages();
    GetSalesPersonList();
    GetDebitNoteList();

    function GetPrivilages() {
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
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    //Get the Shipments
    function GetShipments() {
        if ($scope.S_CustSup != "") {
            var getshipmentlist = $http.get("/ET_Sales_GenerateDebitNote/GetShipments",
                {
                    params: { id: $scope.S_CustSup, type: $scope.submittext, invid: $scope.DN_Id }
                });
            getshipmentlist.then(function (shipmentlists) {
                debugger;
                var res = JSON.parse(shipmentlists.data);
                $scope.shipmentlists = res;
            }, function () {
                alert("Shipment Not Found");
            });
        }
        else {
            $scope.shipmentlists = {};
            $scope.DN_Id = "";
        }
    }
    $scope.ShipmentChange = function () {
        $http({
            method: 'POST',
            url: '/ET_Sales_GenerateDebitNote/GetDebitNoteDetails',
            dataType: 'json',
            data: {
                id: $scope.SelectShipmentId,
            },
        }).success(function (shipmentlist) {
            debugger;
            var res = JSON.parse(shipmentlist);
            angular.element(document.getElementById('popupFOBTable')).html("");
            for (var i = 0; i < res.data1.length; i++) {
                var Rowhtml = "<tr><td>" + res.data1[i].ordNo + "</td><td style='text-align:right'>" + parseFloat(res.data1[i].ordAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td><td><input style='text-align:right' type='text' onchange='fobAmountChange(this);' class='form-control' /></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('popupFOBTable')).append(temp);
            }
            for (var j = 0; j < res.data2.length; j++) {
                $("#popchkFob").prop('checked', false);
                $("#div_foblist").css("display", "none");
                $("#txtInvoiceAmount").val(parseFloat(res.data2[j].S_INV_AMT).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                $scope.S_SalesPerson = res.data2[j].S_SalesPerson;
                $("#drpSalesPerson").val($scope.S_SalesPerson).trigger("chosen:updated");
                $scope.txtSupplierName = res.data2[j].COM_NAME;
                $scope.S_INV_NO = res.data2[j].S_INV_NO;
                var INVDate1 = new Date(parseInt(res.data2[j].S_INV_DATE.substr(6)));
                var INVDate = ("0" + INVDate1.getDate()).slice(-2) + "-" + ("0" + (INVDate1.getMonth() + 1)).slice(-2) + "-" + INVDate1.getFullYear();
                $("#txtINVDate").val(INVDate);

            }
        });
    }
    function GetDebitNoteList() {
        var getDebitNotelist = $http.get("/ET_Sales_GenerateDebitNote/GetDebitNoteList",
            {
                params: { delete: false, type: $scope.S_Type }
            });
        getDebitNotelist.then(function (DebitNotelist) {
            var res = JSON.parse(DebitNotelist.data);
            $scope.shipmentlist = res;
        }, function () {
            alert("Quotation List Found");
        });
    }
    function GetShipmentRestoreList() {
        var getshipmentrestorelist = $http.get("/ET_Sales_Shipment/GetDebitNoteList",
            {
                params: { delete: true, type: $scope.S_Type }
            });
        getshipmentrestorelist.then(function (shipmentrestorelist) {
            var res = JSON.parse(shipmentrestorelist.data);
            $scope.shipmentrestorelist = res;
        }, function () {
            alert("Quotation Restore List Found");
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
       
        GetShipments();
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
    
    $scope.SalesPersonChange = function () { }
    $scope.ScheduleChange = function () {
    }
   

    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetDebitNoteList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
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
            $("#invoiceCol").css("display", "block");
            $("#div_foblist").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.DN_Id = "0";
            $scope.DN_Code = "";
            $scope.S_CustSup = "";
            $scope.S_SupplierID = "";
            $scope.S_ScheduleID = "";
            $scope.S_SalesPerson = "";
            $scope.txtSupplierName = "";
            $scope.SelectShipmentId = "";

            $("#txtInvoiceAmount").val("");
            $scope.S_INV_NO = "";
            $("#txtINVDate").val("");
            $scope.S_INV_AMT = "";
            $scope.custsups = {};
            $("#drpCustSup").val("").trigger("chosen:updated");
            $("#drpSalesPerson").val("").trigger("chosen:updated");
            $("#DrpShipmentId").val("").trigger("chosen:updated");
            $scope.OrderTypeChange();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
  
    $scope.editRecords = function (a,b) {
        debugger;
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_GenerateDebitNote/GetDebitNoteDetailsForEdit",
                dataType: 'json',
                data: {
                    id: a,
                    Status: b
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
                    var ShipmentByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    GetCustomers();
                    $scope.DN_Id = ShipmentByID.data2[0].DN_ID;
                    $scope.DN_Code = ShipmentByID.data2[0].DN_Code;
                    $scope.S_Type = ShipmentByID.data2[0].S_Type;
                    $scope.S_CustSup = ShipmentByID.data2[0].S_CustSup;
                    $scope.S_SalesPerson = ShipmentByID.data2[0].S_SalesPerson;
                    $scope.S_INV_NO = ShipmentByID.data2[0].S_INV_NO;
                    $scope.SelectShipmentId = ShipmentByID.data2[0].S_ID;
                    $scope.txtSupplierName = ShipmentByID.data2[0].COM_NAME;
                    GetShipments();
                    var INVDate1 = new Date(parseInt(ShipmentByID.data2[0].S_INV_DATE.substr(6)));
                    var INVDate = ("0" + INVDate1.getDate()).slice(-2) + "-" + ("0" + (INVDate1.getMonth() + 1)).slice(-2) + "-" + INVDate1.getFullYear();
                    $("#txtINVDate").val(INVDate);


                    $scope.S_INV_AMT = ShipmentByID.data2[0].S_INV_AMT.toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $("#drpSalesPerson").val($scope.S_SalesPerson).trigger("chosen:updated");
                    $("#DrpShipmentId").val($scope.SelectShipmentId).trigger("chosen:updated");
                    angular.element(document.getElementById('popupFOBTable')).html("");
                    
                    if (ShipmentByID.data2[0].DN__FOBStatus == 0) {
                        for (var i = 0; i < ShipmentByID.data1.length; i++) {
                            var Rowhtml = "<tr><td>" + ShipmentByID.data1[i].ordNo + "</td><td style='text-align:right'>" + parseFloat(ShipmentByID.data1[i].ordAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td><td><input style='text-align:right' type='text'  onchange='fobAmountChange(this);' class='form-control' /></td></tr>";
                            var temp = $compile(Rowhtml)($scope);
                            angular.element(document.getElementById('popupFOBTable')).append(temp);
                        }
                        $("#popchkFob").prop('checked', false);
                        $("#div_foblist").css("display", "none");
                        $("#invoiceCol").css("display", "block");
                    }
                    else {
                        for (var i = 0; i < ShipmentByID.data1.length; i++) {
                            var Rowhtml = "<tr><td>" + ShipmentByID.data1[i].ordNo + "</td><td style='text-align:right'>" + parseFloat(ShipmentByID.data1[i].ordAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td><td><input style='text-align:right' type='text' value=" + parseFloat(ShipmentByID.FabAmounts[i]).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " onchange='fobAmountChange(this);' class='form-control' /></td></tr>";
                            var temp = $compile(Rowhtml)($scope);
                            angular.element(document.getElementById('popupFOBTable')).append(temp);
                        }
                        $("#popchkFob").prop('checked', true);
                        $("#div_foblist").css("display", "block");
                        $("#invoiceCol").css("display", "none");
                    }
                   
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
    $scope.ViewRecords = function (a) {
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shipment/ET_Sales_Shipment_View",
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
    
    
    $scope.PerformRestore = function (a, $event, b)
    {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Shipment/ET_Sales_Shipment_RestoreDelete",
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
        debugger;
        //alert("delete message");
        //alert(a.toString());
        if (b) {
            alertmessageModified($event, a.toString(), b, 'ET_Sales_Shipment', 'ET_Sales_Shipment_RestoreDelete');
        }
        else {
            $scope.PerformRestore(a, $event, b);
            // message = "You don't access to do this operation, please contact admin.";
            // toastr["error"](message, "Notification");
        }
    } 

    $scope.generateDebitNote = function (a, b) {
        if ($scope.Isedit) {

            var getshipmentlist = $http.get("/ET_Sales_Shipment/GetDebitNoteDetails",
                {
                    params: { id: a }
                });
            getshipmentlist.then(function (shipmentlist) {
                debugger;
                var res = JSON.parse(shipmentlist.data);
                angular.element(document.getElementById('popupFOBTable')).html("");
                for (var i = 0; i < res.length; i++) {
                    var Rowhtml = "<tr><td>" + res[i].ordNo + "</td><td style='text-align:right'>" + parseFloat(res[i].ordAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td><td><input style='text-align:right' type='text' onchange='fobAmountChange(this);' class='form-control' /></td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('popupFOBTable')).append(temp);
                }
                $("#popchkFob").prop('checked', false);
                $("#div_foblist").css("display", "none");
                $("#popInvoice").val(parseFloat(b).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                $("#popID").val(a);
                $("#btnviewdebitnotepopup").trigger("click");
            }, function () {
                alert("DebitNote Details Not Found");
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    $scope.CommissionRecieve = function (a) {
        if ($scope.Isedit) {
            debugger;
            $("#popupCommissionRecieved").val("");
            $("#txtCommissionDate").val("");
            $("#popupID").val(a);
            var getshipmentlist = $http.get("/ET_Sales_Shipment/GetCommissionDetails",
                {
                    params: { id: a }
                });
            getshipmentlist.then(function (shipmentlist) {
                debugger;
                var res = JSON.parse(shipmentlist.data);
                if (res.isfab == 0) {
                    $("#span_commissiontitle").html("Order Amount");
                    $("#popupInvoiceAmt").val(parseFloat(res.popupInvoiceAmt[0]).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $("#popupCommissionAmt").val(parseFloat(res.popupCommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    angular.element(document.getElementById('popupCommisionTable')).html("");

                    for (var i = 0; i < res.data.length; i++) {
                        var Rowhtml = "<tr><td>" + res.data[i].ordNo + "</td><td>" + parseFloat(res.data[i].commission).toFixed(0) + " %</td><td style='text-align:right'>" + parseFloat(res.data[i].ordAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td><td style='text-align:right'>" + parseFloat(res.data[i].commission * res.data[i].ordAmt / 100).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td></tr>";
                        var temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('popupCommisionTable')).append(temp);
                    }
                    var Rowhtml = "<tr><td colspan='2' style='text-align:center;font-weight:bold;'>Total</td><td style='text-align:right;font-weight:bold;'>" + parseFloat(res.popupInvoiceAmt[0]).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €<input style='display:none;' type='text' id='totalInvoiceamount' value='" + parseFloat(res.popupInvoiceAmt[0]).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "' /></td><td style='text-align:right;font-weight:bold;'>" + parseFloat(res.popupCommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('popupCommisionTable')).append(temp);

                    $("#btncommissionrecievepopup").trigger("click");
                }
                else {
                    $("#span_commissiontitle").html("FAB Amount");
                    $("#popupInvoiceAmt").val(parseFloat(res.popupInvoiceAmt[0]).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $("#popupCommissionAmt").val(parseFloat(res.popupCommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    angular.element(document.getElementById('popupCommisionTable')).html("");
                    var totfab = 0;
                    for (var i = 0; i < res.data.length; i++) {
                        var Rowhtml = "<tr><td>" + res.data[i].ordNo + "</td><td>" + parseFloat(res.data[i].commission).toFixed(0) + " %</td><td style='text-align:right'>" + parseFloat(res.FabAmounts[i]).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td><td style='text-align:right'>" + parseFloat(res.data[i].commission * res.FabAmounts[i] / 100).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td></tr>";
                        var temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('popupCommisionTable')).append(temp);
                        totfab = totfab + (parseFloat(res.FabAmounts[i]));
                    }
                    var Rowhtml = "<tr><td colspan='2' style='text-align:center;font-weight:bold;'>Total</td><td style='text-align:right;font-weight:bold;'>" + totfab.toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €<input style='display:none;' type='text' id='totalInvoiceamount' value='" + totfab.toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "' /></td><td style='text-align:right;font-weight:bold;'>" + parseFloat(res.popupCommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + " €</td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('popupCommisionTable')).append(temp);

                    $("#btncommissionrecievepopup").trigger("click");
                }
            }, function () {
                alert("Commission Details Not Found");
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    $scope.ChangeLanguage = function (lang) {
        debugger;
        var id = 0;
        if (lang == "E") {
            id = parseInt($("#germanformat").val());
        }
        else {
            id = parseInt($("#englishformat").val());
        }
        $scope.PrintRecords(id, lang);
    }
    $scope.PrintRecords = function (a, b) {
        debugger;
        if ($scope.S_ComType == null)
            $scope.S_ComType = 1;

        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shipment/DebitNote_Print",
                dataType: 'html',
                data: {
                    id: a,
                    lang: b,
                    companyIndex: $scope.S_ComType
                },
                headers: { "Content-Type": "application/json" }
            });
            post.success(function (data, status) {
                debugger;
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
                    $('#div_PrintContent').html(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");
                }
            });

        }
    }
    $scope.$watch("salespersons", function (value) {
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
   
    $scope.$watch("shipmentlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("shipmentlists", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#DrpShipmentId").val($scope.SelectShipmentId).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("shipmentrestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }

    });
});