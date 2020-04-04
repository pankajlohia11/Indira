app1.controller("AC_EnquiryCtrl", function ($scope, $http, $window, $compile) {
    function GetTypeFromURL() {
        debugger;
        var type = window.location.href.toString().split("type=");
        $scope.Type = type[1];
        if ($scope.Type == "Agency") {
            $scope.E_Type = "1";
        }
        else if ($scope.Type == "Trading") {
            $scope.E_Type = "2";
        }
        else if ($scope.Type == "Store") {
            $scope.E_Type = "3";
        }
        $("#drpType").val($scope.E_Type).trigger("chosen:updated");
    }
    GetTypeFromURL();
    GetPrivilages();
    GetEnquiryList();
    //Get the Privillage access for this document
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Sales_Enquiry/GetPrivilages");
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
            $('.chosen-select').chosen();
        }, function () {
            alert('Privilages Not Found');
        }
        );
    }
    $scope.RedirectEnqtoCusOffer = function (a) {
        window.location = '../ET_Sales_Quotation/ET_Sales_EnquirytoQuotation?type=' + $scope.Type + '&EnqId=' + a;
    }
    //Get the enquiry list
    function GetEnquiryList() {
        var getenquirylist = $http.get("/ET_Sales_Enquiry/GetEnquiryList",
            {
                params: { delete: false, type: $scope.E_Type }
            });
        getenquirylist.then(function (enquirylist) {
            var res = JSON.parse(enquirylist.data);
            $scope.enquirylist = res;
        }, function () {
            alert("Enquiry List Found");
        });
    }
    //Get the delete enquiry list 
    function GetEnquiryRestoreList() {
        var getenquiryrestorelist = $http.get("/ET_Sales_Enquiry/GetEnquiryList",
            {
                params: { delete: true, type: $scope.E_Type }
            });
        getenquiryrestorelist.then(function (enquiryrestorelist) {
            var res = JSON.parse(enquiryrestorelist.data);
            $scope.enquiryrestorelist = res;
        }, function () {
            alert("Enquiry Restore List Found");
        });
    }
    //Get the customer list
    function GetCustomerList() {
        var getcustomerlist = $http.get("/ET_Sales_Enquiry/GetCustomers",
            {
                params: { id: $scope.E_ID }
            }
        );
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
        }, function () {
            alert("Customer Not Found");
        });
    }
    //Get the contact list
    function GetContactList() {
        if ($scope.E_CustomerID != "") {
            var getcontactlist = $http.get("/ET_Sales_Enquiry/GetContacts",
                {
                    params: { id: $scope.E_CustomerID }
                });
            getcontactlist.then(function (contactlist) {

                var res = JSON.parse(contactlist.data);
                $scope.contacts = res;
            }, function () {
                alert("Contact Not Found");
            });
        }
        else {
            $scope.contacts = {};
            $scope.E_ContactID = "";
        }
    }
    //Get the salesperson list
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_Enquiry/GetSalesPerson", {
            params: { id: $scope.E_ID }
        });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;


        }, function () {
            alert('Data not found');
        });
    }
    //Get the product list
    function GetProductList() {
        var getproductlist = $http.get("/ET_Sales_Enquiry/GetProducts",
            {
                params: { id: $scope.E_ID }
            });
        getproductlist.then(function (productlist) {
            debugger;
            var res = JSON.parse(productlist.data);
            $scope.products = res;
        }, function () {
            alert('Data not found');
        });
    }
    function GetProductListCallback(lengthval, enquirydata) {
        var getproductlist = $http.get("/ET_Sales_Enquiry/GetProducts",
            {
                params: { id: $scope.E_ID }
            });
        getproductlist.then(function (productlist) {
            debugger;
            var res = JSON.parse(productlist.data);
            $scope.products1 = res;
            if (length <= Productlength) {
                lengthval--;
                GetProductListCallback(lengthval, enquirydata);
            }
            else {
                $scope.enquirydata1 = enquirydata;
            }
            
        }, function () {
            alert('Data not found');
        });
    }
    //Get the productCategory list
    function GetProductCategList() {
        var getproductlist = $http.get("/ET_Sales_Enquiry/GetProductsCategory",
            {
                params: { id: $scope.E_ID }
            });
        getproductlist.then(function (productlist) {
            var res = JSON.parse(productlist.data);
            $scope.productscateg = res;
        }, function () {
            alert('Data not found');
        });
    }
    $scope.CustomerChange = function () {
        $scope.E_ContactID = "";
        GetContactList();
    }
    $scope.ContactChange = function () {

    }
    $scope.TypeChange = function () {
    }
    $scope.SalesPersonChange = function () {
    }
    $scope.ProductChange = function () {

        //alert($scope.selectedproduct);
    }
    var length = 1;
    var Productlength = 1;
    
    //Add new product
    $scope.addnewrow = function (a) {
        debugger;
        var e = a.target;
        var tr = $("#enquirydetailsbody").find("tr");
        var txt = "";
        for (var i = 0; i < tr.length; i++) {
            var td = tr[i].cells;
            var Product = $(td[1]).find("p").text();
            var Product1 = $(td[1]).find("select").attr("id");
            var valu = $("#" + Product1).text();
            var Qty = $(td[5]).find("input").val();
            if (Product == "") {
                message = "Select Product at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Qty == "") {
                message = "Enter the Quantity at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
        }
        if (txt == "")
        {
            var rowlength = tr.length + 1;
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control'/></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + rowlength + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control'/></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('enquirydetailsbody')).append(temp);
            $(".chosen-select").chosen({ width: "95%" });
            GetProductList();
        }
    };

    //Delete the product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#enquirydetailsbody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
            message = "Atleast one Product required";
            toastr["error"](message, "Notification");
        }
    }
    //View the enquiry list
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetEnquiryList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    //Restore list view
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetEnquiryRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    }
    //REQ viewPOPup
    $scope.ViewRfqDetails= function (a) {
        if ($scope.Isedit) {
            Supplier();
            var getshipmentlist = $http.get("/ET_Sales_Enquiry/GetRfqDetails",
                {
                    params: { id: a }
                });
            getshipmentlist.then(function (shipmentlist) {
                debugger;
                var res = JSON.parse(shipmentlist.data);
                angular.element(document.getElementById('popupFOBTable')).html("");
                for (var i = 0; i < res.length; i++) {
                    var Rowhtml = "<tr><td>" + res[i].ED_ProductName + "</td><td>" + res[i].ED_UOM + "</td><td style='text-align:right'>" + parseFloat(res[i].ED_Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    $("#TxtCustomerName").val(res[i].E_CustomerName);
                    $("#ENQType").val(res[i].E_Type);
                    angular.element(document.getElementById('popupFOBTable')).append(temp);
                }
                $("#popchkFob").prop('checked', false);
                $("#div_foblist").css("display", "block");
               
                $("#popID").val(a);
                GetRfqDetails1(a);
                
                $("#btnviewdebitnotepopup").trigger("click");
            }, function () {
                alert("Enquiry Details Not Found");
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    function GetRfqDetails1(a) {
        var MailText = "";
        var getshipmentlist = $http.get("/ET_Sales_Enquiry/GetRfqDetails1",
            {
                params: { id: a }
            });
        getshipmentlist.then(function (shipmentlist) {
            debugger;
            $("#popupRFQTable").empty();
            $("#RfqTableId").css("display", "block");
            var res = JSON.parse(shipmentlist.data);
            for (var i = 0; i < res.length; i++) {
                if (res[i].DELETED == 1) {
                    MailText = "Mail Sent";
                }
                else {
                    MailText = "";
                }
                $("#popupRFQTable").append("<tr><td>" + res[i].scheduleCode + "</td><td>" + res[i].SH_Code + "</td><td><a href='../Sales/PDFList/" + res[i].SD_ScheduleID + res[i].SH_Code+".pdf' target='_blank' class='btn btn-xs btn-default'><i class='fa fa-search'></i> View</a><a style='margin-left:10px;' onclick='SendEnquiryPrint(" + res[i].SD_ScheduleID + ',' + $("#popID").val() + ',' + res[i].S_OrderID + "); ' class='btn btn-xs btn-default'><i class='fa fa-envelope'></i> Send Mail </a><span style='color:red;margin-left:16px;' id='MailMessageId" + res[i].SD_ScheduleID + "'></span></td></tr>")
                if (res[i].LAST_UPDATED_DATE != null) {
                    var Orderdate = new Date(parseInt(res[i].LAST_UPDATED_DATE.substr(6)));
                    $("#MailMessageId" + res[i].SD_ScheduleID).text(MailText +" - "+ ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear());
                }
                
            }
            if (res.length == 0) {
                $("#RfqTableId").css("display", "none");
            }

        }, function () {
            alert("Enquiry Details Not Found");
        });
    }
    // Binding the supplier only
   
    $scope.Supplierchange = function () {

    }
    $scope.ProductChange = function () {

    }
    //Create the new enquiry
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.E_ID = "0";
            $scope.E_Code = "";
            $scope.E_CustomerID = "";
            $scope.E_ContactID = "";
            $scope.E_SalesPerson = "";
            
            var Orderdate = new Date();
            var Orderdt = ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear();
            $("#txtEnqDate").val(Orderdt);
            GetCustomerList();
            $("#drpContact").val("").trigger("chosen:updated");
            GetSalesPersonList();
            $scope.contacts = {};
            //GetProductCategList();
            debugger;

            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p id='productId' style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control' /></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p id='productId' style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control' /></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><select class='form-control' id='drpProductCateg' onchange='ProductCategChange(this);'><option value=''>-Select-</option><option ng-repeat='product in productscateg' value='{{ product.PG_ID }}'>{{ product.PG_NAME }}</option ></select></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control' /></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('enquirydetailsbody')).html("");
            angular.element(document.getElementById('enquirydetailsbody')).append(temp);
           
            $(".chosen-select").chosen({ width: "95%" });
            GetProductList();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Validate the data
    function validate() {
        var tr = $("#enquirydetailsbody").find("tr");
        var txt = "";
        if ($scope.E_Type == "") {
            message = "Select Type";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.E_CustomerID == "") {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.E_ContactID == "") {
            message = "Select Contact Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.E_SalesPerson == "") {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtEnqDate").val() == "") {
            message = "Select Enquiry Date";
            toastr["error"](message, "Notification");
            return "";
        }
        for (i = 0; i < tr.length; i++) {
            var td = $(tr[i]).find("td");
            //var product = $(td[0]).find("select").val();
            var product = $(td[1]).find("p").text();
            var qty = $(td[5]).find("input").val();
            if (product == "") {
                message = "Enter Product Name at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            } else if (qty == "") {
                message = "Enter Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                txt = txt + $(td[1]).find("p").text() + "}";
                txt = txt + $(td[4]).find("input").val() + "}";
                txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + "|";
            }
        }
        return txt;
    }
    //Save the enquiry
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display","block");
        var txt = validate();
        var date = $("#txtEnqDate").val();
        debugger;
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Enquiry/ET_Master_Enquiry_Add",
                dataType: 'json',
                data: {
                    E_ID: $scope.E_ID,
                    E_Code: $scope.E_Code,
                    E_Type: $scope.E_Type,
                    E_Date: date,
                    E_CustomerID: $scope.E_CustomerID,
                    E_ContactID: $scope.E_ContactID,
                    E_SalesPerson: $scope.E_SalesPerson,
                    EnquiryDetails: txt
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
                    var res = data.split(':');
                    if ($scope.E_ID == 0) {
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
                $window.alert(data.Message);
                $("#div_loadImage").css("display", "none");
            });
        }
        else
        {
            $("#div_loadImage").css("display", "none");
        }

    }
    //Edit the records
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Enquiry/ET_Master_Enquiry_Update_GetbyID",
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
                    var EnquiryByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    $scope.E_ID = EnquiryByID.E_ID;
                    $scope.E_Code = EnquiryByID.E_Code;
                    $scope.E_Type = EnquiryByID.E_Type;
                    $scope.E_CustomerID = EnquiryByID.E_CustomerID;
                    $scope.E_ContactID = EnquiryByID.E_ContactID;
                    $scope.E_SalesPerson = EnquiryByID.E_SalesPerson;
                    var Orderdate = new Date(parseInt(EnquiryByID.E_Date.substr(6)));
                    var Orderdt = ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear();
                    $("#txtEnqDate").val(Orderdt);
                    //GetProductList();
                    EnquiryDetails($scope.E_ID);
                    GetCustomerList();
                    GetSalesPersonList();
                    GetContactList();
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
    //Get the enquiry details
    function EnquiryDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Sales_Enquiry/ET_Master_Enquiry_ContactsLoad',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var enquirydata = JSON.parse(data);
            var i = enquirydata.length;
            Productlength = enquirydata.length;
            var j = 0;
            length = 1;
            angular.element(document.getElementById('enquirydetailsbody')).html("");
            while (i != 0)
            {
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value ='" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products1' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control' /></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' value ='" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProducts" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products1' value='{{ product.P_ID }}'>{{ product.P_ArticleNo+'-'+product.P_Name }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtProductName' class='form-control' /></td><td><input type='text' id='txtQuantity' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('enquirydetailsbody')).append(temp);
                $(".chosen-select").chosen();
                i--;
                length++;
                
                j++;
            }
            length = 1;
            GetProductListCallback(enquirydata.length, enquirydata);
           
           
        });
    }
    //Restore the deleted records
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Enquiry/ET_Master_Enquiry_RestoreDelete",
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

    $scope.Restoredeleterecords = function (a, $event, b) {
        //alert(a.toString());
        if (b)
        {
            alertmessageModified($event, a.toString(), b,'ET_Sales_Enquiry','ET_Master_Enquiry_RestoreDelete');
        }
        else
        {
           $scope.PerformRestore(a, $event, b);
        // message = "You don't access to do this operation, please contact admin.";
        //toastr["error"](message, "Notification");
        }
    } 
    //View: Popup view
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Enquiry/ET_Master_Enquiry_View",
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
    function Supplier() {
       
            var id1 = "0";
        
        $http({
            method: 'POST',
            url: '/ET_Sales_Enquiry/GetCustomerSupplier',
            data: {
                id: id1,
                custsup: 0
            },
        }).
            success(function (data) {
                debugger;
                var Supplier = JSON.parse(data)
                $scope.SupplierList = Supplier;
            });
    }
    $scope.ClientSch = function (schids) {
        debugger;
        $scope.S_ScheduleID = schids;
        var schids = "";
        try {
            schids = $scope.S_ScheduleID.join();
        }
        catch (ex) {
            schids = $scope.S_ScheduleID;
        }
        ShipmentDetails(schids);
    }

    //Watch for all data binding
    $scope.$watch("SupplierList", function (value) {
        debugger;
        setTimeout(function () { $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated"); }, 5);
    });

    $scope.$watch("customers", function (value)
    {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.E_CustomerID).trigger("chosen:updated"); }, 100);
        }
    });

    $scope.$watch("products", function (value)
    {
        var val = value || null;
        if (val)
        {
            var tr = $("#enquirydetailsbody").find("tr");
            var trLength = tr.length;
            debugger;
            setTimeout(function () { $("#drpProducts" + trLength).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("products1", function (value) {
        var val = value || null;
        if (val) {
            debugger;
            setTimeout(function () { $("#drpProducts" + length).trigger("chosen:updated"); length++; }, 100);
        }
    });
    $scope.$watch("contacts", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpContact").val($scope.E_ContactID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.E_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("enquirylist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("enquiryrestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
    $scope.$watch("enquirydata1", function (value) {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.enquirydata1;
            setTimeout(function () {
                debugger;
                var tr = $("#enquirydetailsbody").find("tr");
                for (var i = 0; i < tr.length; i++) {
                    var td = tr[i].cells;
                    $(td[1]).find("select").val(enquirydata[i].productid).trigger("chosen:updated");
                    $(td[1]).find("p").text(enquirydata[i].productid);
                    $(td[2]).find("input").val(enquirydata[i].uom);
                    $(td[3]).find("input").val(enquirydata[i].packing);
                    $(td[4]).find("input").val(enquirydata[i].description);
                    $(td[5]).find("input").val(parseFloat(enquirydata[i].quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                }
            }, 100);
        }
    });

});