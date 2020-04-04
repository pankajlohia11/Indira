app1.controller("AC_OneToManyInvoiceCtrl", function ($scope, $http, $window, $compile) {
    debugger;
    var type1 = window.location.href.toString().split("&");
    var type = type1[1].split("DespatchId=");
    var SchduleId = type[1];
    GetPrivilages();
    GetCustomerList();
    GetSalesPersonList();
    //Get the Privillage access for this document
    function GetPrivilages() {
        debugger;
        
        $("#div_Edit").css("display", "block");
        $scope.submittext = "Submit";
        $scope.createedit = "Create";
        $scope.OTMI_ID = "0";
        $scope.OTMI_Code = "";
        $scope.OTMI_FreightCost = "";
        var getprivilages = $http.get("/ET_Sales_OneToManyInvoice/GetPrivilages");
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

    $scope.OTMI_DespatchIDs = SchduleId;
    //Get the Customers
    function GetCustomerList() {
        var getcustomerlist = $http.get("/ET_Sales_OneToManyInvoice/GetCustomers");
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
            $scope.OTMI_CustomerID = "";
            
            GetDespatchDetails($scope.OTMI_DespatchIDs);
        }, function () {
            alert("Customer Not Found");
        });
    }
    //Get the Sales Person
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_OneToManyInvoice/GetSalesPerson");
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
            $scope.OTMI_SalesPerson = 0;
        }, function () {
            alert('Data not found');
        });
    }
    //
    function GetDespatchDetails(a) {
        debugger;
        var getcustomerlist = $http.get("/ET_Sales_OneToManyInvoice/GetDespatchDetails",
            {
                params: {
                    id: a,
                }
            });
        getcustomerlist.then(function (customerlist) {
            debugger;
            var data1 = JSON.parse(customerlist.data)
            $scope.OTMI_CustomerID = data1[0].D_CustomerID;
            $("#drpCustomer").val($scope.OTMI_CustomerID).trigger("chosen:updated");
            $scope.OTMI_SalesPerson = data1[0].D_SalesPerson;
            $("#drpSalesPerson").val($scope.OTMI_SalesPerson).trigger("chosen:updated");
            getTax();
            $scope.CustomerChange();
        }, function () {
            alert("Customer Data Not Found");
        });
        //var post = $http({
        //    method: "POST",
        //    url: "/ET_Sales_Despatch/GetDespatchDetails",
        //    dataType: 'html',
        //    data: {
        //        id: a,
        //    },
        //    headers: { "Content-Type": "application/json" }
        //});
        //post.success(function (data, status) {
        //  
        //});

        //post.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
    }
    // Get the sales person  For Customer
    function GetSalesPersonForCustomer() {
        if ($scope.OTMI_CustomerID != "") {
            var SalesPerson = $http.get("/ET_Sales_OneToManyInvoice/GetSalesPersonForCustomer",
                {
                    params: { custid: $scope.OTMI_CustomerID }
                });
            SalesPerson.then(function (sPerson) {
                var data = sPerson.data;
                $scope.OTMI_SalesPerson = data[0];
                if ($scope.OTMI_SalesPerson == 0) {
                    message = "Sales Person not assigned in company master";
                    toastr["error"](message, "Notification");
                }
                else {
                    $("#drpSalesPerson").val($scope.OTMI_SalesPerson).trigger("chosen:updated");
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
    //Customer Change
    $scope.CustomerChange = function () {
       // $scope.OTMI_DespatchIDs = "";
        GetDespatches();
        GetSalesPersonForCustomer();
        
    }
    //Get the Despatches
    function GetDespatches() {
        debugger;
        if ($scope.OTMI_CustomerID != "") {
            var getdespatchlist = $http.get("/ET_Sales_OneToManyInvoice/GetDespatches",
                {
                    params: {
                        custid: $scope.OTMI_CustomerID,
                        type: $scope.submittext,
                        invid: $scope.OTMI_ID
                    }
                });
            getdespatchlist.then(function (despatchlist) {

                var res = JSON.parse(despatchlist.data);
                $scope.despatchlist = res;
                $scope.DespatchChange();
                
            }, function () {
                alert("Despatches Not Found");
            });
        }
        else {
            $scope.despatchlist = {};
            $scope.OTMI_DespatchIDs = "";
        }
    }
    //Despatch change
    $scope.DespatchChange = function () {

        GetInvoiceDetails();
    }
    //Get tax
    function getTax() {
        debugger;
        if ($scope.OTMI_CustomerID != "") {
            var getTaxPer = $http.get("/ET_Sales_OneToManyInvoice/GetTax", {
                params: { id: $scope.OTMI_CustomerID }
            });
            getTaxPer.then(function (getTaxPercent) {
                $scope.OTMI_TaxPer = parseFloat(getTaxPercent.data).toFixed(0);
            }, function () {
                alert('Data not found');
            });
        }
    }
    //Get invoice details
    function GetInvoiceDetails() {
        debugger;
        var id = "";
        try {
            id = $scope.OTMI_DespatchIDs.join();
        }
        catch
        {
            id = $scope.OTMI_DespatchIDs;
        }
        $http({
            method: 'POST',
            url: '/ET_Sales_OneToManyInvoice/GetInvoiceDetails',
            dataType: 'html',
            data: {
                ids: id,
            },
        }).success(function (data) {
            debugger;
            angular.element(document.getElementById('invoicedetailsbody')).html("");
            angular.element(document.getElementById('invoicedetailsbody')).html(data);
        }).error(function (result) {
            debugger;
            alert("Invoice Details Not Found");
        });
        $http({
            method: 'POST',
            url: '/ET_Sales_OneToManyInvoice/GetInvoiceTotal',
            dataType: 'json',
            data: {
                ids: id,
            },
        }).success(function (data) {
            debugger;
            $scope.OTMI_OrderAmount = parseFloat(data.data2).toLocaleString("es-ES", { minimumFractionDigits: 2 });
            var taxper = parseFloat($scope.OTMI_TaxPer);
            var taxamount = (parseFloat(data.data2) * taxper / 100);
            var invamt = parseFloat(data.data2) + taxamount;
            var DiscountAmt = invamt * parseFloat(data.data1[0].Discount) / 100;
            $scope.OTMI_TaxAmount = taxamount.toLocaleString("es-ES", { minimumFractionDigits: 2 });
            $scope.OTMI_DiscountAmt = DiscountAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 });
            $scope.OTMI_InvoiceAmount = (invamt - DiscountAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 });
            $scope.OTMI_PaymentTerms = data.data1[0].PaymentTerms;
        });
    }
    $scope.SalesPersonChange = function () {
    }
    //Tax changes
    $scope.taxchange = function () {
        debugger;
        if ($scope.OTMI_OrderAmount == "") {
            $scope.OTMI_TaxPer = "";
            message = "Please Select Despatch No, Order Amount cannot be empty.";
            toastr["error"](message, "Notification");
        }
        else {
            $scope.OTMI_TaxAmount = (parseFloat($scope.OTMI_OrderAmount) * (parseFloat($scope.OTMI_TaxPer)) / 100).toFixed(3);
            $scope.OTMI_InvoiceAmount = (parseFloat($scope.OTMI_OrderAmount) + (parseFloat($scope.OTMI_TaxAmount))).toFixed(3)
        }
    }
    //Show the invoice
    $scope.showRecords = function () {
        window.location = '../ET_Sales_Despatch/ET_Sales_Despatch';
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
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#div_Print").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";

            $scope.OTMI_ID = "0";
            $scope.OTMI_Code = "";
            $scope.OTMI_CustomerID = "";
            $scope.OTMI_SalesPerson = 0;
            $scope.OTMI_CustomerID = "";
            $scope.OTMI_DespatchIDs = "";
            $scope.OTMI_InvoiceDate = "";
            $scope.OTMI_OrderAmount = "";
            $scope.OTMI_TaxPer = "";
            $scope.OTMI_TaxAmount = "";
            $scope.OTMI_InvoiceAmount = "";

            $scope.orderlist = {};
            $scope.despatchlist = {};
            $("#txtINVDate").val("");
            $("#drpCustomer").val("").trigger("chosen:updated");
            $("#drpDespatchIDs").val("").trigger("chosen:updated");
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
        if ($scope.OTMI_CustomerID == "") {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTMI_DespatchIDs == "") {
            message = "Select Atleast One Despatch";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.OTMI_SalesPerson == 0) {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtOrderAmount").val() == "") {
            message = "Total Order Amount Cannot be empty. Select Despatch No";
            toastr["error"](message, "Notification");
            return "";
        }
        return "something";
    }
    //Save the invoice
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var date = $("#txtINVDate").val();
        var txt = validate();
        var desids = "";
        try {
            desids = $scope.OTMI_DespatchIDs.join();
        }
        catch (ex) {
            desids = $scope.OTMI_DespatchIDs;
        }
        if ($scope.OTMI_FreightCost == "") {
            $scope.OTMI_FreightCost = 0;
        }
        debugger;
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToManyInvoice/ET_Sales_Invoice_Add",
                dataType: 'json',
                data: {
                    OTMI_ID: $scope.OTMI_ID,
                    OTMI_Code: $scope.OTMI_Code,
                    OTMI_CustomerID: $scope.OTMI_CustomerID,
                    OTMI_SalesPerson: $scope.OTMI_SalesPerson,
                    OTMI_CustomerID: $scope.OTMI_CustomerID,
                    OTMI_DespatchIDs: desids,
                    OTMI_InvoiceDate: date,
                    OTMI_OrderAmount: parseFloat($scope.OTMI_OrderAmount.split('.').join("").replace(",", ".")),
                    OTMI_DiscountAmt: parseFloat($scope.OTMI_DiscountAmt.split('.').join("").replace(",", ".")),
                    OTMI_TaxPer: $scope.OTMI_TaxPer,
                    OTMI_PaymentTerms: $scope.OTMI_PaymentTerms,
                    OTMI_FreightCost: $scope.OTMI_FreightCost,
                    OTMI_TaxAmount: parseFloat($scope.OTMI_TaxAmount.split('.').join("").replace(",", ".")),
                    OTMI_InvoiceAmount: parseFloat($scope.OTMI_InvoiceAmount.split('.').join("").replace(",", ".")),
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
                    if ($scope.OTMI_ID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    setTimeout(function () {
                        window.location = '../ET_Sales_Despatch/ET_Sales_Despatch';
                    }, 2000);
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
   
    //Watch for All Binding
    $scope.$watch("customers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.OTMI_CustomerID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("despatchlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpDespatchIDs").val($scope.OTMI_DespatchIDs).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.OTMI_SalesPerson).trigger("chosen:updated"); }, 100);
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