app1.controller("AC_OneTOOneInvoiceCtrl", function ($scope, $http, $window, $compile)
{
    GetPrivilages();
    GetInvoiceList();
    GetCustomerList();
    GetSalesPersonList();
    //Get the Privillage access for this document
    function GetPrivilages() {
        debugger;
        var getprivilages = $http.get("/ET_Sales_OneToOneInvoice/GetPrivilages");
        getprivilages.then(function (privilage1) {
            var privilage = JSON.parse(privilage1.data);
            if (privilage.length != 0)
            {
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
    //Get the invoice list
    function GetInvoiceList() {
        var getinvoicelist = $http.get("/ET_Sales_OneToOneInvoice/GetInvoiceList",
            {
                params: { delete: false }
            });
        getinvoicelist.then(function (invoicelist) {
            var res = JSON.parse(invoicelist.data);
            $scope.invoicelist = res;
        }, function () {
            alert("Invoice List Found");
        });
    }
    //Get the invoice list which are deleted
    function GetInvoiceRestoreList() {
        var getinvoicerestorelist = $http.get("/ET_Sales_OneToOneInvoice/GetInvoiceList",
            {
                params: { delete: true }
            });
        getinvoicerestorelist.then(function (invoicerestorelist) {
            var res = JSON.parse(invoicerestorelist.data);
            $scope.invoicerestorelist = res;
        }, function () {
            alert("Invoice Restore List Found");
        });
    }
    //Get the Customers
    function GetCustomerList() {
        var getcustomerlist = $http.get("/ET_Sales_OneToOneInvoice/GetCustomers");
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
            $scope.OTOI_CustomerID = "";
        }, function () {
            alert("Customer Not Found");
        });
    }
    //Get the Shipments
    function GetShipments()
    {
        if ($scope.OTOI_CustomerID != "")
        {
            var getshipmentlist = $http.get("/ET_Sales_OneToOneInvoice/GetShipments",
                {
                    params: { id: $scope.OTOI_CustomerID, type: $scope.submittext, invid: $scope.OTOI_ID }
                });
            getshipmentlist.then(function (shipmentlist) {

                var res = JSON.parse(shipmentlist.data);
                $scope.shipmentlist = res;
            }, function () {
                alert("Shipment Not Found");
            });
        }
        else {
            $scope.shipmentlist = {};
            $scope.OTOI_ShipmentID = "";
        }
    }
     //Get the Sales Person
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_OneToOneInvoice/GetSalesPerson");
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
            $scope.OTOI_SalesPerson = 0;
        }, function () {
            alert('Data not found');
        });
    }
    // Get the sales person  For Customer
    function GetSalesPersonForCustomer() {
        debugger;
        if ($scope.OTOI_CustomerID != "") {
            var SalesPerson = $http.get("/ET_Sales_OneToOneInvoice/GetSalesPersonForCustomer",
                {
                    params: { custid: $scope.OTOI_CustomerID }
                });
            SalesPerson.then(function (sPerson) {
                var data = sPerson.data;
                $scope.OTOI_SalesPerson = data[0];
                if ($scope.OTOI_SalesPerson == 0) {
                    message = "Sales Person not assigned in company master";
                    toastr["error"](message, "Notification");
                }
                else {
                    $("#drpSalesPerson").val($scope.OTOI_SalesPerson).trigger("chosen:updated");
                }
                
            }, function () {
                alert("Enquires Not Found");
            });
        }
        else {
            message = "Select Type";
            toastr["error"](message, "Notification");
            $scope.Q_CustomerID == "";
            $("#drpCustomer").val("").trigger("chosen:updated");
        }
    }
    //Change the language in print
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
    //Customer Change
    $scope.CustomerChange = function () {
        debugger;
        $scope.OTOI_ShipmentID = "";
        GetShipments();
        getTax();
        GetSalesPersonForCustomer();
    }
    //Shipment Change
    $scope.ShipmentChange = function ()
    {
        ShipmentDetails();
        getTax();
    }
    //Get the tax
    $scope.SalesPersonChange = function () {
    }
    function getTax()
    {
        if ($scope.OTOI_CustomerID != "") {
            var getTaxPer = $http.get("/ET_Sales_OneToOneInvoice/GetTax",
            {
                params: { id: $scope.OTOI_CustomerID, shipmentId: $scope.OTOI_ShipmentID }
            });
            getTaxPer.then(function (getTaxPercent) {
                $scope.OTOI_TaxPer = parseFloat(getTaxPercent.data).toFixed(0);
            }, function () {
                alert('Data not found');
            });
        }
    }
    //Show the invoice
    $scope.showRecords = function ()
    {
        $("#advancedusage").dataTable().fnDestroy();
        GetInvoiceList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //Show the deleted invoice
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetInvoiceRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //create the new invoice
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#div_Print").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.OTOI_ID = "0";
            $scope.OTOI_Code = "";
            $scope.OTOI_CustomerID = "";
            $scope.OTOI_SalesPerson = 0;
            $scope.OTOI_ShipmentID = "";
            $scope.OTOI_InvoiceDate = "";
            $scope.OTOI_ShipmentAmount = "";
            $scope.OTOI_TaxPer = "";
            $scope.OTOI_TaxAmount = "";
            $scope.OTOI_InvoiceAmount = "";
            $scope.shipmentlist = {};
            $("#txtINVDate").val("");
            $("#drpCustomer").val("").trigger("chosen:updated");
            $("#drpShipment").val("").trigger("chosen:updated");
            $("#drpSalesPerson").val("").trigger("chosen:updated");
            angular.element(document.getElementById('invoicedetailsbody')).html("");
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Validate the data before saving
    function validate() {
        var txt = "";
        if ($("#txtINVDate").val() == "") {
            message = "Select Invoice Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTOI_CustomerID == "") {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTOI_ShipmentID == "") {
            message = "Select Shipment";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTOI_SalesPerson == 0) {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtShipmentAmount").val() == "") {
            message = "Total Shipment Amount Cannot be empty. Select Shipment No";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTOI_TaxPer == "") {
            message = "Enter Tax %";
            toastr["error"](message, "Notification");
            return "";
        }
    }
    //Save the invoice
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var date = $("#txtINVDate").val();
        var txt = validate();
        debugger;
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_Add",
                dataType: 'json',
                data: {
                    OTOI_ID: $scope.OTOI_ID,
                    OTOI_Code: $scope.OTOI_Code,
                    OTOI_CustomerID: $scope.OTOI_CustomerID,
                    OTOI_SalesPerson: $scope.OTOI_SalesPerson,
                    OTOI_ShipmentID: $scope.OTOI_ShipmentID,
                    OTOI_InvoiceDate: date,
                    OTOI_ShipmentAmount: parseFloat($scope.OTOI_ShipmentAmount.split('.').join("").replace(",", ".")),
                    OTOI_TaxPer: $scope.OTOI_TaxPer,
                    OTOI_TaxAmount: parseFloat($scope.OTOI_TaxAmount.split('.').join("").replace(",", ".")),
                    OTOI_InvoiceAmount: parseFloat($scope.OTOI_InvoiceAmount.split('.').join("").replace(",", ".")),
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
                    if ($scope.OTOI_ID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully With Code : ' + res[1];
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
        else {
            $("#div_loadImage").css("display", "none");
        }

    }
    //Edit the invoice details
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit)
        {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_Update_GetbyID",
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
                    var InvoiceByID = JSON.parse(data);
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                     $scope.OTOI_ID = InvoiceByID.data.OTOI_Serial;
                    $scope.OTOI_ID = InvoiceByID.data.OTOI_ID;
                    $scope.OTOI_Code = InvoiceByID.data.OTOI_Code;
                    $scope.OTOI_CustomerID = InvoiceByID.data.OTOI_CustomerID;
                    $scope.OTOI_SalesPerson = InvoiceByID.data.OTOI_SalesPerson;
                    $scope.OTOI_ShipmentID = InvoiceByID.data.OTOI_ShipmentID;
                    $scope.OTOI_InvoiceDate = InvoiceByID.data.OTOI_InvoiceDate;
                    $scope.OTOI_ShipmentAmount = parseFloat(InvoiceByID.data.OTOI_ShipmentAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.OTOI_TaxPer = InvoiceByID.data.OTOI_TaxPer;
                    $scope.OTOI_TaxAmount = parseFloat(InvoiceByID.data.OTOI_TaxAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.OTOI_InvoiceAmount = parseFloat(InvoiceByID.data.OTOI_InvoiceAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.OTOI_ContainerNo = InvoiceByID.data1.containerNo;
                    $scope.OTOI_Blno = InvoiceByID.data1.Bl_no;
                    var Invoicedate = new Date(parseInt(InvoiceByID.data.OTOI_InvoiceDate.substr(6)));
                    var INVDT = ("0" + Invoicedate.getDate()).slice(-2) + "-" + ("0" + (Invoicedate.getMonth() + 1)).slice(-2) + "-" +  Invoicedate.getFullYear();
                    $("#txtINVDate").val(INVDT);

                    $("#drpCustomer").val($scope.OTOI_CustomerID).trigger("chosen:updated");
                    $("#drpSalesPerson").val($scope.OTOI_SalesPerson).trigger("chosen:updated");
                    getTax();
                    GetShipments();
                    ShipmentDetails();
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
    //Get the Shipment Details
    function ShipmentDetails() {
        var id = $scope.OTOI_ShipmentID;
        $http({
            method: 'POST',
            url: '/ET_Sales_OneToOneInvoice/GetShipmentDetails',
            dataType: 'html',
            data: {
                id: id,
            },
        }).success(function (data) {
            angular.element(document.getElementById('invoicedetailsbody')).html("");
            angular.element(document.getElementById('invoicedetailsbody')).html(data);
            });
        $http({
            method: 'POST',
            url: '/ET_Sales_OneToOneInvoice/GetShipmentTotal',
            dataType: 'json',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var total = JSON.parse(data);
            $scope.OTOI_ShipmentAmount = parseFloat(total[0].OTOI_InvoiceAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 });
            var taxper = parseFloat($scope.OTOI_TaxPer);
            var taxamount = (parseFloat(total[0].OTOI_InvoiceAmount) * taxper / 100);
            var invamt = parseFloat(total[0].OTOI_InvoiceAmount) + taxamount;
            $scope.OTOI_TaxAmount = taxamount.toLocaleString("es-ES", { minimumFractionDigits: 2 });
            $scope.OTOI_InvoiceAmount = invamt.toLocaleString("es-ES", { minimumFractionDigits: 2 });
            $("#txtTaxAmt").val(taxamount.toFixed(2));
            $("#txtInvAmt").val(invamt.toFixed(2));
            $scope.OTOI_ContainerNo = total[0].containerNo;
            $scope.OTOI_Blno = total[0].Bl_no;
        });
    }
    //Restore the deleted records
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_RestoreDelete",
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
    };

    //$scope.Restoredeleterecords = function (a, $event, b) {

    //    var res = false;
    //    //if (b)
    //    //    res = $scope.Isdelete;
    //    //else
    //    //    res = $scope.Isrestore
    //    res = $scope.Isdelete;
    //    if (res) {
    //        var post = $http({
    //            method: "POST",
    //            url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_RestoreDelete",
    //            dataType: 'json',
    //            data: {
    //                id: a,
    //                type: b
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
    $scope.Restoredeleterecords = function (a, $event, b) {
        if (b)
        {
            alertmessageModified($event, a.toString(), b, 'ET_Sales_OneToOneInvoice', 'ET_Sales_Invoice_RestoreDelete');
        }
        else
        {
            $scope.PerformRestore(a, $event, b);
            setTimeout(() => {
                location.reload();
            }, 1000);  //1s
            //message = "You don't access to do this operation, please contact admin.";
           // toastr["error"](message, "Notification");
        }
    } 
    //View the invoice
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_View",
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
                else
                {
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
                    //getTax();
                    //GetShipments();
                    //ShipmentDetails();
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
    //Print the invoice
    $scope.PrintRecords = function (a,b) {
        debugger;
        a = parseInt(a); 
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_Print",
                dataType: 'html',
                data: {
                    id: a,
                    lang:b
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
                    $('#div_PrintContent').html(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
            var post1 = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_InvoicePrint",
                dataType: 'html',
                data: {
                    id: a,
                    lang: b
                },
                headers: { "Content-Type": "application/json" }
            });
            post1.success(function (data, status) {
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
                    $("#PAthid").attr("href", "/Sales/PDFList/OnetoOneInvoice/" + data[0].OTOI_Code + "/" + data[0].OTOI_Code + ".pdf ");
                    $("#PAthid").text(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");
                }
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Watch for All data Binding
    $scope.$watch("customers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.E_CustomerID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("shipmentlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpShipment").val($scope.OTOI_ShipmentID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.E_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("invoicelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("invoicerestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
});