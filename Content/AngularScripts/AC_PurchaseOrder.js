app1.controller("AC_POCtrl", function ($scope, $http, $window, $compile)
{
    GetTypeFromURL();
    GetPrivilages();
    GetPOList();
    GetCurrency();
    ORGBind();
    CheckOrderRedirect();

    //Check if it is an Redirect from Order creation.
    function CheckOrderRedirect()
    {
        var redirectRequired = getParameterByName("redirect", window.location.href.toString());
        if (redirectRequired != null)
        {
            //Check if the redirect is successfull
            if (redirectRequired == 'true')
            {
                var orderId = getParameterByName("orderID", window.location.href.toString());
                if (orderId != null)
                {
                    $scope.PO_OrderReference = orderId;
                    var redirectType = getParameterByName("redirectType", window.location.href.toString());
                    //redirect 1 for creation
                    //redirect 2 for editing
                    if ($scope.PO_OrderReference != null)
                    {
                        if (redirectType == 1)
                        {
                            $scope.Iscreate = true;
                            $scope.Isedit = false;
                            message = "Create Purchase Order for Order:" + $scope.PO_OrderReference;
                            toastr["info"](message, "Notification");
                            //angular.element(document.getElementById('createRecordLink')).scope().createnew();
                            //angular.element('#createRecordLink').trigger('click');
                            //var element = angular.element(document.querySelector('#createRecordLink'));
                            //alert(element);
                            //angular.element(document.getElementById('#createRecordLink'))[0].triggerHandler("click");
                            //$("#createRecordLink").click();
                            /*var elem = document.getElementById("createRecordLink");
                            if (typeof elem.onclick == "function") {
                                elem.onclick.apply(elem);
                            }*/
                            //angular.element(document.querySelector('#poController')).scope().createnew();
                            //$scope.createnew();
                            //$("#drpOrderReference").val($scope.PO_OrderReference).trigger("chosen:updated");
                            //angular.element(document.getElementById('#divView')).scope().createnew();
                        }
                        else if (redirectType == 2)
                        {
                            $scope.Isedit = true;
                            $scope.Iscreate = false;
                            $http(
                                {
                                method: 'POST',
                                url: '/ET_Purchase_PO/GetPurchaseOrder',
                                data: {
                                    orderReference: $scope.PO_OrderReference,
                                    type: $scope.PO_Type
                                }
                            }).success(function (data)
                            {
                                var result = JSON.parse(data);
                                if (result == null || result == '')
                                    $scope.PO_Code = 0;
                                else
                                {
                                    $scope.PO_Code = result[0].ID;
                                }
                                if ($scope.PO_Code == 0)
                                {
                                    message = "Purchase Order does not exist for Order:" + $scope.PO_OrderReference;
                                    toastr["info"](message, "Notification");
                                    //angular.element(document.getElementById('#createRecordLink'))[0].triggerHandler("click");
                                }
                                else
                                {
                                    message = "Edit the Purchase Order:" + result[0].Code + " " +  "for the Order:" + $scope.PO_OrderReference;
                                    toastr["info"](message, "Notification");
                                    //angular.element(document.querySelector('#editRecordLink')).triggerHandler('ng-click', $scope.PO_Code);
                                }
                                }
                             );
                            //angular.element(document.querySelector('#editRecordLink')).triggerHandler('ng-click', $scope.PO_Code);
                            //angular.element(document.getElementById('#editRecordLink'))[0].triggerHandler("click", $scope.PO_Code);
                            //editRecords($scope.PO_Code);
                        }
                    }
                }
            }
        }
    }

    function getParameterByName(name, url)
    {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    //Get the Privillage access for this document
    function GetPrivilages() {
        $scope.isschedule = false;
        var getprivilages = $http.get("/ET_Purchase_PO/GetPrivilages");
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
    //Checking The Type From URL
    function GetTypeFromURL()
    {
        $scope.Type = getParameterByName("type",window.location.href.toString());
        //var type = window.location.href.toString().split("type=");
        //var type = jsonOuptut[0];
        //$scope.Type = type[1];
        if ($scope.Type == "Agency") {
            $scope.PO_Type = 1;
        }
        else if ($scope.Type == "Trading") {
            $scope.PO_Type = 2;
        }
        else if ($scope.Type == "Store") {
            $scope.PO_Type = 3;
        }
        $("#drpType").val($scope.PO_Type).trigger("chosen:updated");
    }
    //Get Payment Terms
    function PaymentTerms()
    {
        $http({
            method: 'POST',
            url: '/ET_Purchase_PO/Payment_terms_dropdown',
            data: { id: $scope.PP_ID }
        }).success(function (data) {
            var payment = JSON.parse(data)
            $scope.paymenttermlist = payment;
        });
    }
     //Get All PO list Which are not deleted
    function GetPOList() {
        var getstorelist = $http.get("/ET_Purchase_PO/GetPOList",
            {
                params: { delete: false, type: $scope.PO_Type }
            });
        getstorelist.then(function (storelist) {
            var res = JSON.parse(storelist.data);
            $scope.storelist = res;
        }, function () {
            alert("No Data Found");
        });
    }
     //Get All PO list Which are  deleted
    function GetStoreRestoreList() {

        var getporestorelist = $http.get("/ET_Purchase_PO/GetPOList",
            {
                params: { delete: true, type: $scope.PO_Type  }
            });
        getporestorelist.then(function (porestorelist) {
            debugger;
            var res = JSON.parse(porestorelist.data);
            $scope.porestorelist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    $scope.CurrencyChange = function () {
    }
      //Get Supplier address When Supplier Change
    $scope.SupplierChange = function () {

        var id = $scope.Po_Supplierkey;
        $http({
            method: 'POST',
            url: '/ET_Purchase_PO/BindSupplierAddress',
            dataType: 'json',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            $scope.Po_SupplierAddress = data;
            SupplierBankChecking($scope.Po_Supplierkey);
            });
        //debugger;
        //if ($scope.PO_Type == 3)
        //{
        //    GetProducts();
        //}
    }
    function ORGBind()
    {
        debugger;
        var GetTargetList = $http.get("/ET_Purchase_PO/BindRow_Employees");
        GetTargetList.then(function (org) {
           
            if (org.data == "Session Expired") {
                $window.location.href = '/ET_Login/ET_SessionExpire';
            }
            else if (org.data.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(data);
                $("#spanErrMessage2").html(data);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
            else {
                $scope.singlerowdata = org.data;
                var temp = $compile($scope.singlerowdata)($scope);
                angular.element(document.getElementById('divTarget')).append(temp);
            }
        }, function () {
            alert('Data not foundss');
        });
    }
            //Get Currency List
    function GetCurrency() {

        var getcurrencylist = $http.get("/ET_Purchase_PO/GetCurrency");
        getcurrencylist.then(function (currencylist) {
            var res = JSON.parse(currencylist.data);
            $scope.currencylist = res;
           // $scope.Po_CurrencyKey = "";
        }, function () {
            alert("Currency Not Found");
        });
    }
    //Edit Time Called For PO Type
    function OrderTypeChanged() {
        if ($scope.PO_Type == 2) {
            debugger;
            $("#OrderReference").show();
            $("#ShippingOnetoMany").hide();
            $("#ShippingOnetoone").show();
            $scope.GI_StoreCode = "0";
            $scope.isDisabled = true;
            $("#drpsuppliername").attr('disabled', "disabled");
        }
        if ($scope.PO_Type == 3) {
            $("#OrderReference").hide();
            $("#ShippingOnetoMany").show();
            $scope.Po_ShippingAddress = "";
            $("#ShippingOnetoone").hide();
            $scope.isDisabled = false;
            $("#drpsuppliername").removeAttr("disabled");
        }
    }
    //PO Type Change on submit time
    $scope.OrderTypeChange = function () {
        if ($scope.PO_Type == 2) {
            debugger;
            $("#OrderReference").show();
            $("#ShippingOnetoMany").hide();
            $("#ShippingOnetoone").show();
            $scope.GI_StoreCode = "0";
            $scope.PO_OrderReference = "0";
            $("#drpOrderReference").val("0").trigger("chosen:updated");
            $scope.isDisabled = true;
            $("#drpsuppliername").attr('disabled', "disabled");
            $scope.Po_Supplierkey = "";
            $("#drpsuppliername").val("").trigger("chosen:updated");
            $scope.Po_TotalAmount = "0";
            $("#txtSupplierAddress").val("");
            $("#txttotalamount").val("");
            $scope.ProductData1 = null;
            var temp = $compile($scope.singlerowdata)($scope);
            angular.element(document.getElementById('divTarget')).html("");
            angular.element(document.getElementById('divTarget')).append(temp);
        }
        if ($scope.PO_Type == 3) {
            debugger;
            $("#OrderReference").hide();
            $("#ShippingOnetoMany").show();
            $("#ShippingOnetoone").hide();
            $scope.Po_ShippingAddress = "";
            $scope.PO_OrderReference = "0";
            $("#drpOrderReference").val("0").trigger("chosen:updated");
            $scope.isDisabled = false;
            $("#drpsuppliername").removeAttr("disabled");
            $scope.Po_Supplierkey = "";
            $("#drpsuppliername").val("").trigger("chosen:updated");
            $scope.Po_TotalAmount = "0";
            $("#txtSupplierAddress").val("");
            $("#txttotalamount").val("");
            $scope.products = "";
            $scope.ProductData1 = null;
            temp = $compile($scope.singlerowdata)($scope);
            angular.element(document.getElementById('divTarget')).html("");
            angular.element(document.getElementById('divTarget')).append(temp);
        }
    }
    //This is for one-one PO. Get The orderReference
    $scope.OrderReferenceChange = function () {
        if ($scope.PO_OrderReference != 0)
        {
            //Get the Product List and use it for the Order Reference.
            var id = $scope.PO_OrderReference;
            GetSuppliers(id);
            GetProducts();
            $http({
                method: 'POST',
                url: '/ET_Purchase_PO/GetSupplierPriceList',
                data: {
                    id: id, catalogSelected: $("#drpCatalogSection").val()
                },
            }).success(function (data) {
                if (data.indexOf("ERR") > -1) {
                    debugger;
                    var message = data;
                    toastr["error"](message, "Notification");
                    $scope.PO_OrderReference = "0";
                    $("#drpOrderReference").val("0").trigger("chosen:updated");

                    var temp = $compile($scope.singlerowdata)($scope);
                    $scope.products = "";
                    $scope.ProductData1 = null;
                    angular.element(document.getElementById('divTarget')).html("");
                    angular.element(document.getElementById('divTarget')).append(temp);
                }
                else if (data.indexOf("Validations") > -1) {
                    debugger;
                    message = data;
                    toastr["error"](message, "Notification");
                    $scope.PO_OrderReference = "0";
                    $("#drpOrderReference").val("0").trigger("chosen:updated");

                    //$scope.singlerowdata
                    temp = $compile(data)($scope);
                    $scope.products = "";
                    $scope.ProductData1 = null;
                    angular.element(document.getElementById('divTarget')).html("");
                    angular.element(document.getElementById('divTarget')).append(temp);
                } else {
                    debugger;
                    if (data.indexOf("Some of the products Supplier") > -1)
                    {
                        message = "For Some Products, Supplier Price is greater than Order Price. Please verify and proceed";
                        toastr["error"](message, "Notification");
                    }
                    angular.element(document.getElementById('divTarget')).html("");
                    temp = $compile(data)($scope);
                    angular.element(document.getElementById('divTarget')).append(temp);
                    $scope.ProductData1 = data;
                }
            });
        }
        else
        {
            $scope.products = "";
            $scope.ProductData1 = null;
            var temp = $compile($scope.singlerowdata)($scope);
            angular.element(document.getElementById('divTarget')).html("");
            angular.element(document.getElementById('divTarget')).append(temp);
        }

    };

    //watch For Order Product Data
    $scope.$watch("ProductData1", function (value) {
        var val = value || null;
        if (val)
        {
            var ProductData1 = $scope.ProductData1;
            setTimeout(function () {
                try {
                    $("#divTarget").find("script").remove();
                }
                catch (ex)
                {
                    message = "Error Retreiving Product Information";
                    toastr["error"](message, "Notification");
                    return "";
                }
                var tbl = $("#divTarget").children();
                var txt = "";
                var total = 0;
                for (var i = 0; i < tbl.length;)
                {
                    debugger;
                    var td1 = tbl[i].children;
                    td1[1].children[0].setAttribute("disabled", "disabled");
                    td1[4].children[0].setAttribute("disabled", "disabled");
                    var val = td1[4].children[0];
                    //alert($(td1[5].children[0]).val());
                    //alert($(td1[6].children[0]).val());
                    var poQtyNum = $(td1[6].children[0]).val();
                    var priceNum = $(td1[7].children[0]).val();
                    var article = $(td1[3].children[0]).val();
                     //alert($td[2].find("select").val(ProductData1[i].PD_ProductID));
                    //$td[2].find("select").val(ProductData1[i].PD_ProductID).trigger("chosen:updated");
                    //var qty = parseFloat(poQty.split('.').join("").replace(",", "."));
                    //var unitprice = parseFloat(price.split('.').join("").replace(",", "."));
                    //$(td1[5].children[0]).val(poQtyNum).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //$(td1[6].children[0]).val(priceNum).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //var qty = parseFloat($(td1[5].children[0]).val());
                    //var unitprice = parseFloat($(td1[6].children[0]).val());
                    unitprice = parseFloat(priceNum.split('.').join("").replace(",", "."));
                    quantity = parseFloat(poQtyNum.split('.').join("").replace(",", "."));
                    //$(td[7]).find("input").val((price * quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }))
                    if (unitprice == 0) {
                        message = "Price is not defined in the Catalog Selected for Article:" + article;
                        toastr["error"](message, "Notification");
                    }
                    else
                    {
                        //message = "Price is defined in the Catalog Selected for Article:" + article;
                        //toastr["success"](message, "Notification");
                    }
                    var amt = quantity * unitprice;
                    total = total + amt;
                    $(td1[8].children[0]).val(amt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td1[2].children[0]).val($(td1[2].children[0]).attr("ng-init"));
                    i = i + 2;
                }
                $("#txttotalamount").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }, 5000);
        }
    });
    //this is for one - one PO .Get the Supplier name from order
    function GetSuppliers(id)
    {
        $.ajax({
            type: "GET",
            url: "/ET_Purchase_PO/GetSuppliersByOrder",
            async: false,
            data: {
                id: id
            },
            success: function (result) {
                var res = JSON.parse(result);
                $scope.Po_Supplierkey = res[0];
                $("#drpsuppliername").val($scope.Po_Supplierkey).trigger("chosen:updated");
                $scope.SupplierChange($scope.Po_Supplierkey);
            }
        });
    };

    //Checking the Bank details for Supplier
    function SupplierBankChecking(SupplierId) {
        debugger;
        var getSuppbank = $http.get("/ET_Purchase_PO/GetBankDetailsForSupplier",
            {
                params: { id: SupplierId }
            });
        getSuppbank.then(function (bankdata) {

            debugger;
            var res = JSON.parse(bankdata.data);
           
            if (res == 0) {
                message = "Bank Details Not there in Company Master For this Supplier";
                toastr["error"](message, "Notification");
            }
            
            
        }, function () {
            alert('Data not found');
        });
    }
    //this is for one - Many PO .Get the Suppliers List
    function GetSupplier(id) {

        var getsupplierlist = $http.get("/ET_Purchase_PO/GetSupplier",
            {
                params:
                    {
                        id:id
                    }
            });
        getsupplierlist.then(function (supplierslist) {

            var res = JSON.parse(supplierslist.data);
            $scope.supplierslist = res;
           // $scope.Po_Supplierkey = "";
        }, function () {
            alert("Supplier Not Found");
        });
    }
    // This is For One-many PO Get the store list
    function GetStoreMaster(PO_Id) {
        debugger;
        var getstorelists = $http.get("/ET_Purchase_PO/GetStorelist",
            {
                params: { PO_Id: PO_Id }
            }
        );
        getstorelists.then(function (storelist) {
            debugger;
            var res = JSON.parse(storelist.data);
            $scope.storelists = res;
        }, function () {
            alert("Supplier Not Found");
        });
    }
    //Get Products For Particular Supplier
    function GetProducts()
    {
        $.ajax({
            type: "GET",
            url: '/ET_Admin_ProductCatalog/GetProductsForCatalog',
            data:
            {
                catalogCode: $scope.Q_CatalogSelection
            },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var res = JSON.parse(result);
                $scope.products = res;
                $scope.$apply();
            },
            error: function (response)
            {
                toastr["error"]("Product Data Not Found", "Notification");
            }
        });

        //$.ajax({
        //    type: "GET",
        //    url: "/ET_Purchase_PO/GetProducts",
        //    async: false,
        //    data: {
        //        id: $scope.Po_Supplierkey
        //    },
        //    success: function (result)
        //    {
        //        var res = JSON.parse(result);
        //        $scope.products = res;
        //        $scope.$apply();
        //    }
        //});
    };

    //Adding New Delivery Schedule
    $scope.addnewrowchild = function (a, b) {

        var e = a.target;
        var tr = $("#tblPOSchedule" + b)[0].children[1].children;
        var txt = "";
        for (var i = 0; i < tr.length; i++) {
            var td = tr[i].cells;
            var date = $(td[2]).find("input").val();
            var Qty = $(td[6]).find("input").val();

            if (date == "") {
                message = " Select Date at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Qty == "") {
                message = "Enter the Opening Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
        }
        if (txt == "") {
            //var Rowhtml = "<tr><td><input type='number' id='txtOrderDetailId' class='form-control' readonly='readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control' readonly='readonly' /></td><td><input type='number' id='txtQuantity' onchange='calculateamount(this)' class='form-control' /></td><td><input type='number' id='txtUnitPrice' onchange='calculateamount(this)' class='form-control' /></td><td ng-if='istrading == true'><input type='text' id='txtDesignDetail' class='form-control' readonly='readonly' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtRemarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,' id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td ></tr >";
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td style='display:none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo +'-'+product.P_Name }}</option ></select></td><td style='display:none'><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtDesid' name='txtDesid' class='form-control' /></td><td><input type='text' id='txtQuantity' style='text-align:right'  onchange='calculateamount(this)' class='form-control' style='text-align:right' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right'  onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><input type='text' id='txtSupDesc' name='txtSupDesc' class='form-control' /></td><td><a id='id_addnewrow' style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtPackage' class='form-control'/></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><input type='number' id='txttax' class='form-control' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
            //var Rowhtml = "<tr><td style='display: none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td ><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtPackage' class='form-control'/></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><input type='number' id='txttax' class='form-control' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
            //var Rowhtml = ("<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><div class='input-group datepicker w-360' style='width: 100%' data-format='DD-MM - YYYY'><input type='text' id='txtDSDate' name='txtDSDate'  class='form-control' onfocus='getdatepic();' /><span class='input-group-addon'><span class='fa fa-calendar' onclick='getdatepic()'></span></span></div></td><td><input type='text' id='txtQuantity' style='text-align:right;' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrowchild($event,\"" + b + "\")'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterowchild($event,\"" + b + "\")'><i class='fa fa-trash'></i></a></td></tr>").toString();
            //var Rowhtml = ("<tr><td><div class='input-group datepicker w-360' style='width: 100%' data-format='DD-MM - YYYY'><input type='text' id='txtDSDate' name='txtDSDate'  class='form-control' onfocus='getdatepic();' /><span class='input-group-addon'><span class='fa fa-calendar' onclick='getdatepic()'></span></span></div></td><td><input type='number' id='txtQuantity' class='form-control' /></td></tr>").toString();
            var temp = $compile(Rowhtml)($scope);
            $($("#tblPOSchedule" + b)[0].children[1]).append(temp);
        }
    };

    $scope.CatalogSelection = function ()
    {
        //Retreive the Product Details specific.
        GetProducts();
        $http({
            method: 'POST',
            url: '/ET_Admin_ProductCatalog/GetProductCatalogValidityDate',
            data: {
                Q_CatalogId: $scope.Q_CatalogSelection
            }
        }).success(function (result) {
            //var validityDate = JSON.parse(result.data);
            var savedValidation = new Date(parseInt(result.substr(6)));
            var currentDate = new Date();
            savedValidation.setHours(0, 0, 0, 0);
            currentDate.setHours(0, 0, 0, 0);
            if (savedValidation < currentDate)
            {
                var Orderdt = ("0" + savedValidation.getDate()).slice(-2) + "-" + ("0" + (savedValidation.getMonth() + 1)).slice(-2) + "-" + savedValidation.getFullYear();
                message = "Catalog Validity is Expired as on:" + Orderdt;
                toastr["error"](message, "Notification");
                return;
            }
        });

        $http({
            method: 'POST',
            url: '/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Details_UpdateChildtable',
            data: {
                id: $scope.Q_CatalogSelection
            }
        }).success(function (data) {
            var contactdata = JSON.parse(data);
            var i = contactdata.length;
            var j = 0;
            length = 1;
            //alert("length:" + i);
            //angular.element(document.getElementById('Contactbody')).html("");
            //while (i != 0)
            //{
            //    var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            //    var temp = $compile(Rowhtml)($scope);
            //    angular.element(document.getElementById('Contactbody')).html("");
            //    angular.element(document.getElementById('Contactbody')).append(temp);
            //    $('.chosen-select').chosen();
            //    ProductDetails();
            //    //$("#drpPackType").val($scope.PL_Type).trigger("chosen:updated");

            //    //var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtSerial' value='{{ item.SO_Serial }}' class='form-control' /><p style='display:none;'></p></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            //    //<td><p style='display:none;'></p><input readonly='readonly' type='text' id='txtLastUpdated' class='form-control' /></td>;
            //    //<td><input type='checkbox' name='select_all' value=' " + j + "' id='product-select-all'></td>
            //    //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px; ' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px; color: red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //    //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
            //    //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td></tr>";
            //    //var temp = $compile(Rowhtml)($scope);
            //    //angular.element(document.getElementById('quotationdetailsbody')).append(temp);
            //    //$('.chosen-select').chosen();
            //    length++;
            //    //ProductDetails();
            //    //GetProductList();
            //    i--;
            //    j++;
            //}
            if (contactdata.length > 0) {
                $scope.CatalogDetails = contactdata;
            }
        });
    };

    //watch for the updtes in the catalog list.
    $scope.$watch("catalogList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCatalogSection").val($scope.Q_CatalogSelection).trigger("chosen:updated"); }, 100);
        }
    });

    //Gets the catalog list from the database.
    function GetCatalogList(catalogCode) {
        var getCatalogList = $http.get("/ET_Purchase_PO/GetCatlogList",
            {
                params: { catalogId: catalogCode, catalogType:2 }
            });
        getCatalogList.then(function (catalogCollection) {
            var res = JSON.parse(catalogCollection.data);
            $scope.catalogList = res;
            //$("#drpCatalogSection").val(QuotationByID.Q_CatalogSelection).trigger("chosen:updated");
        }, function () {
            alert("Catalog List not found");
        });
    }

    //Adding a new product
    $scope.addnewrow = function (a)
    {
        if ($scope.isDisabled == false)
        {
            var e = a.target;
            var tr = $("#divTarget").find("tr");
            var txt = "";
            for (var i = 0; i < tr.length; i++)
            {
                var td = tr[i].cells;
                var Product = $(td[2]).find("select").val();
                var Qty = $(td[6]).find("input").val();
                var price = $(td[7]).find("input").val();
                if (Product == "") {
                    message = "Select Product at row " + (i + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";
                }
                else if (Qty == "") {
                    message = "Enter the Opening Qty at row " + (i + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";

                }
                else if (price == "") {
                    message = "Enter the Opening Rate at row " + (i + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";
                }
            }
            if (txt == "") {
                var rowlength = tr.length + 1;
                var i = tr.length;
                //id = 0;
                //while (document.getElementById("collapseExampleid_" + id))
                //{
                //    id++;
                //}
                //var sdf = "_" + id;
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control'/></td><td><input type='text' id='txtQuantity' style='text-align:right;'  class='form-control' /></td><td><input type='text' style='text-align:right;' id='txttax' class='form-control' /></td><td><input type='text' id='txttotalAmount' class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='PO_TYPE == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td>$scope.singlerowdata.replace(/\id_0/g, 'id_' + id)";
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txttax' style='text-align:right;' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right;'  class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //var Rowhtml = "<tr><td style='display: none'><input type = 'text' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td ><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txttax' style='text-align:right;' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right;'  class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><input type='number' id='txtOrderDetailId' class='form-control' readonly='readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control' readonly='readonly' /></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><input type='number' id='txtUnitPrice' class='form-control' /></td><td ng-if='istrading == true'><input type='text' id='txtDesignDetail' class='form-control' readonly='readonly' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtRemarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,' id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td ></tr >";
                //var Rowhtml = "<tr><td><input type='number' id='txtOrderDetailId' class='form-control' readonly='readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control' readonly='readonly' /></td><td><input type='number' id='txtQuantity' onchange='calculateamount(this)' class='form-control' /></td><td><input type='number' id='txtUnitPrice' onchange='calculateamount(this)' class='form-control' /></td><td ng-if='istrading == true'><input type='text' id='txtDesignDetail' class='form-control' readonly='readonly' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtRemarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,' id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td ></tr >";
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td style='display:none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo +'-'+product.P_Name }}</option ></select></td><td style='display:none'><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtDesid' name='txtDesid' class='form-control' /></td><td><input type='text' id='txtQuantity' style='text-align:right' onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right'  onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txttotalAmount' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><input type='text' id='txtSupDesc' name='txtSupDesc' class='form-control' /></td><td><a id='id_addnewrow' style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('divTarget')).append(temp);
            }
        }
        else {
            message = "You cannot add products in One-One PO";
            toastr["error"](message, "Notification");
        }
    }
    //Deleting  Delivery Schedule
    $scope.deleterowchild = function (a, b) {

        var e = a.target;
        var len = $("#tblPOSchedule" + b)[0].children[1].children.length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
            message = "Atleast one Schedule required";
            toastr["error"](message, "Notification");
        }
    }
    //Removing one Product
    $scope.deleterow = function (a, f) {
        //alert("Deleting");
        if ($scope.isDisabled == false)
        {
            var e = a.target;
            if ($('[id="id_addnewrow"]').length > 1)
            {
                //$("#collapseExample" + f).parent().parent().fadeOut(300, function () { $(this).remove(); });
                $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
            } else {
                message = "Atleast one Product required";
                toastr["error"](message, "Notification");
            }
            //var sdfs = $(e).parent().parent().parent();
            //var lj = $(e).parent().parent().parent()[0].children[6];
            //var lj = $(e).parent().parent().parent()[0].children[6].children[0];
            calculateamount($($(e).parent().parent().parent()[0].children[6].children[0]).val());
        }
        else {
            message = "You cannot delete products in One-One PO";
            toastr["error"](message, "Notification");
        }
    };

    //After Delete the row Calculating the Amt
    function calculateamount(e)
    {
        var tbl = $("#divTarget").children();
        var total = 0;
        for (var i = 0; i < tbl.length;)
        {
            var td1 = tbl[i].children;
            if ($(td1[8].children[0]).val() != '')
                total = total + parseFloat($(td1[8].children[0]).val());
            i = i + 1;
        }
        var vale = 0;
        if (e != "")
            vale = parseFloat(e);
        $("#txttotalamount").val((total - vale).toFixed(2));
    };

    //Clone the Existing Record
    $scope.CloneRecord = function (a) {
        var post = $http({
            method: "POST",
            url: "/ET_Purchase_PO/ET_Purchase_ClonePO",
            dataType: 'html',
            data: {
                PP_ID: a
            },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            if (data == "Session Expired")
            {
                $window.location.href = '/ET_Login/ET_SessionExpire';
            }
            else if (data.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(data);
                $("#spanErrMessage2").html(data);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
            else {
                //var orderCode = JSON.parse(data);
                var res = data.split(":");
                // $("#div_loadImage").css("display", "none");
                //$('#viewpopup').html(data);
                //$("#btnviewpopup").trigger("click");
                message = 'Duplicate Record Created With Code : ' + res[1];
                toastr["success"](message, "Notification");
                setTimeout(() => {
                    location.reload();
                }, 1000);  //1s
            }
        });

        post.error(function (data, status) {
            $("#div_loadImage").css("display", "none");
            $window.alert(data.Message);
        });
    };

    //Showing The PO list
    $scope.showRecords = function () {

        $("#advancedusage").dataTable().fnDestroy();
        GetPOList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Print").css("display", "none");
        $("#div_Edit").css("display", "none");
        $scope.PODetails1 = null;
    };
    //Restore the Delete records
    $scope.restoreRecords = function () {

        $("#advancedusageRestore").dataTable().fnDestroy();
        GetStoreRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    };

    //Create the New PO
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $('#viewpopup').html("");
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.OrderTypeChange();
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.PP_ID = "0";
            $scope.PO_Code = "";
            $scope.PO_OrderReference = "0";
            $("#drpOrderReference").val("0").trigger("chosen:updated");
            //$("#OrderReference").hide();

            $scope.Po_CurrencyKey = "";
            $scope.Po_PaymentTerms = "0";
            $scope.Po_Supplierkey = "";
            $scope.Po_SupplierAddress = "";
            $scope.GI_StoreCode = "0";
            $scope.Po_ShippingAddress = "";
            $scope.Po_TotalAmount = "0";
            $scope.Po_SpecialInstruction = "";
            $scope.Po_TermsandConditions = "";
            $scope.Po_DeliveryShedule = false;
            // $scope.txtDeliveryDate = "";
            $("#txtDeliveryDate").val("");
            $("#txtPODate").val("");
            $("#txtSCDate").val("");
            $scope.txtScNo = "";
            $("#txttotalamount").val("");
            $scope.Po_TotalAmount = "0";
            $scope.ProductData1 = null;
            PaymentTerms();
            getOrderReference($scope.PP_ID);
            GetSupplier($scope.PP_ID);
            GetStoreMaster($scope.PP_ID);
            $("#drpsuppliername").val("").trigger("chosen:updated");
            $("#drpcurrency").val("").trigger("chosen:updated");
            $("#chkDeliveryShedule").prop('checked', false);
            $scope.isschedule = false;
            GetCatalogList(0);
            var Rowhtml = '';
            if ($scope.PO_Type == 2)
            {
                //Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txttax' style='text-align:right;' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right;'  class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //Rowhtml = "<tr><td style='display: none'><input type = 'text' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txttax' style='text-align:right;' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right;'  class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //Rowhtml = "<tr><td><input type='number' id='txtOrderDetailId' class='form-control' readonly='readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control' readonly='readonly' /></td><td><input type='number' id='txtQuantity' onchange='calculateamount(this)' class='form-control' /></td><td><input type='number' id='txtUnitPrice' onchange='calculateamount(this)' class='form-control' /></td><td ng-if='istrading == true'><input type='text' id='txtDesignDetail' class='form-control' readonly='readonly' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtRemarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,' id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td ></tr >";
                Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='1' class='form-control' /></td><td style='display:none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo +'-'+product.P_Name }}</option ></select></td><td style='display:none'><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtDesid' name='txtDesid' class='form-control' /></td><td><input type='text' style='text-align:right' id='txtQuantity' class='form-control'  onchange='calculateamount(this)' /></td><td><input type='text' id='txtUnitPrice'  style='text-align:right'  onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txttotalAmount' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><input type='text' id='txtSupDesc' name='txtSupDesc' class='form-control' /></td><td><a id='id_addnewrow' style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@0')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@0' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@0' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
            }
            else
            {
                //Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly ='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right;' class='form-control' onchange='MoneyValidation(this);' /></td><td><input type='text' id='txttotalAmount' class='form-control' style='text-align:right;' onchange='MoneyValidation(this);' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //Rowhtml = "<tr><td style='display: none'><input type = 'text' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' class='form-control' readonly ='readonly'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right;' class='form-control' onchange='MoneyValidation(this);' /></td><td><input type='text' id='txttotalAmount' class='form-control' style='text-align:right;' onchange='MoneyValidation(this);' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
                //Rowhtml = "<tr><td><input type='number' id='txtOrderDetailId' class='form-control' readonly='readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control' readonly='readonly' /></td><td><input type='number' id='txtQuantity' onchange='calculateamount(this)' class='form-control' /></td><td><input type='number' id='txtUnitPrice' onchange='calculateamount(this)' class='form-control' /></td><td ng-if='istrading == true'><input type='text' id='txtDesignDetail' class='form-control' readonly='readonly' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtRemarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,' id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td ></tr >";
                Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td style='display:none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo +'-'+product.P_Name }}</option ></select></td><td style='display:none'><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtDesid' name='txtDesid' class='form-control' /></td><td><input type='text' id='txtQuantity' style='text-align:right' class='form-control'  onchange='calculateamount(this)' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right'  onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txttotalAmount' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><input type='text' id='txtSupDesc' name='txtSupDesc' class='form-control' /></td><td><a id='id_addnewrow' style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@0')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@0' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@0' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
            }
            //var temp = $compile($scope.singlerowdata)($scope);

            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('divTarget')).html("");
            angular.element(document.getElementById('divTarget')).append(temp);
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    //Get the Order For One to one PO
    function getOrderReference(PPID) {
        var getOrderReferences = $http.get("/ET_Purchase_PO/GetOrderReference",
            {
                params: {
                    PP_ID: PPID

                }
            });
        getOrderReferences.then(function (orderreferencelist) {
             
            var res = JSON.parse(orderreferencelist.data);
            $scope.orderreferencelist = res;
            //$scope.PO_OrderReference = "0";
        }, function () {
            alert("Supplier Not Found");
        });
    }
    //Checking Delivery Schedule want or not
    $scope.DeliveyScheduleChange = function () {

        if ($("#chkDeliveryShedule").prop('checked')) {
            $("#txtDeliveryDate").val("");
            $("#txtDeliveryDate").attr('disabled', "disabled");
            $("#txtDeliveryDate").removeAttr('required');
            var tr = $("#tblSalesTarget").find("tr");
            var txt = "";
            $scope.isschedule = true;
            //var Rowhtml = "<tr><td> <div class='input-group datepicker w-360' style='width: 100%'><input type = 'text' id = 'txtDSDate' name = 'txtDSDate' class='form-control'/><span class='input-group-addon'><span class='fa fa-calendar'></span></span></div ></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrowchild($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterowchild($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var temp = $compile(Rowhtml)($scope);
            //angular.element(document.getElementById('storedetailsbody')).html("");
            //angular.element(document.getElementById('storedetailsbody')).append(temp);

        }
        else {
            $("#txtDeliveryDate").removeAttr('disabled');
            $("#txtDeliveryDate").attr('required', true);
            var tr = $("#tblSalesTarget").find("tr");
            var txt = "";
            $scope.isschedule = false;
            $("#tblSalesTarget .collapse").attr('class', 'collapse');
        }
    };

    //Validate the data Before Saving
    function validate()
    {
        var tbl = $("#divTarget").children();
        var tr = $("#divTarget").find("tr");
        var txt = "";
        if ($("#txtPODate").val() == "") {
            message = "Enter PO Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.PO_Type == "") {
            message = "Select PO Type";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.PO_Type == 2) {
            if ($scope.PO_OrderReference == "") {
                message = "Select Order Reference";
                toastr["error"](message, "Notification");
                return "";
            }
            if ($scope.Po_ShippingAddress == "") {
                message = "Enter The Shipping Address";
                toastr["error"](message, "Notification");
                return "";
            }
        }
        if ($scope.PO_Type == 3) {
            if ($scope.GI_StoreCode == "0") {
                message = "Select The Store";
                toastr["error"](message, "Notification");
                return "";
            }
        }
        if ($scope.Po_CurrencyKey == "") {
            message = "Select Currency";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Po_Supplierkey == "") {
            message = "Select Supplier";
            toastr["error"](message, "Notification");
            return "";
        }
        
        if ($scope.Po_TotalAmount == "") {
            message = "Enter The Total Amount";
            toastr["error"](message, "Notification");
            return "";
        }
        
        if (!$("#chkDeliveryShedule").prop('checked')) {
            if ($("#txtDeliveryDate").val() == "") {
                message = "Enter Delivery Date";
                toastr["error"](message, "Notification");
                return "";
            }

        }
        try {
            $("#divTarget").find("script").remove();
        }
        catch (ex) {
            message = "Error Performing Validation of PO Details";
            toastr["error"](message, "Notification");
            return "";
        }
        debugger;
        for (var i = 0; i < tbl.length;)
        {
            var td1 = tbl[i].children;
            //alert(td1.children.length);
            if (td1 !== null && td1.length > 1)
            {
                var productId = $(td1[1].children[0]).val();
                if (productId != null || productId != "")
                {
                    //var product = $(td1[1].children[1]).val();
                    var product = $(td1[3]).find("input").val();
                    var qty = parseFloat($(td1[6].children[0]).val().split('.').join("").replace(",", "."));
                    var total = $(td1[8].children[0]).val();
                    if (product == "") {
                        message = "Select Product Name at row:" + ((i / 2) + 1);
                        toastr["error"](message, "Notification");
                        return "";
                    } else if (qty == "") {
                        message = "Enter PO Quantity at row:" + ((i / 2) + 1);
                        toastr["error"](message, "Notification");
                        return "";
                    }
                    else if (total == "") {
                        message = "Total cannot be empty at row:" + ((i / 2) + 1);
                        toastr["error"](message, "Notification");
                        return "";
                    }
                    else if (parseFloat(total.split('.').join("").replace(",", ".")) == 0) {
                        message = "Total cannot be 0 at row:" + ((i / 2) + 1) + ". Set Supplier Price.";
                        toastr["error"](message, "Notification");
                        return "";
                    }
                    else {
                        var schd = "";
                        if ($("#chkDeliveryShedule").prop('checked')) {
                            var tr1 = $(tbl[i + 1]).find("#storedetailsbody")[0].children;
                            var qty1 = 0;
                            for (var k = 0; k < tr1.length; k++) {

                                if ($(tr1[k]).find('#txtDSDate').val() == "") {
                                    message = "Select Schedule Date for product : " + ((i / 2) + 1) + " at row :" + (k + 1);
                                    toastr["error"](message, "Notification");
                                    return "";
                                }
                                if ($(tr1[k]).find('#txtQuantity').val() == "") {
                                    message = "Enter Schedule Quantity for product : " + ((i / 2) + 1) + " at row :" + (k + 1);
                                    toastr["error"](message, "Notification");
                                    return "";
                                }
                                schd = schd + $(tr1[k]).find('#txtDSDate').val() + "@";
                                schd = schd + parseFloat($(tr1[k]).find('#txtQuantity').val().split('.').join("").replace(",", ".")) + "$";
                                qty1 = qty1 + parseFloat($(tr1[k]).find('#txtQuantity').val().split('.').join("").replace(",", "."));
                            }
                            if (qty != qty1) {
                                message = "PO Quantity is not distributed in schedule at row :" + ((i / 2) + 1);
                                toastr["error"](message, "Notification");
                                return "";
                            }
                        }
                        txt = txt + $(td1[1].children[0]).val() + "}";
                        txt = txt + $(td1[2].children[0]).val() + "}";
                        txt = txt + $(td1[2].children[0]).find("option:selected").text() + "}";
                        txt = txt + $(td1[3].children[0]).val() + "}";
                        txt = txt + $(td1[4].children[0]).val() + "}";
                        txt = txt + $(td1[5].children[0]).val() + "}";
                        txt = txt + parseFloat($(td1[6].children[0]).val().split('.').join("").replace(",", ".")) + "}";
                        txt = txt + parseFloat($(td1[7].children[0]).val().split('.').join("").replace(",", ".")) + "}";
                        txt = txt + parseFloat($(td1[8].children[0]).val().split('.').join("").replace(",", ".")) + "}";
                        txt = txt + $(td1[9].children[0]).val() + "}";
                        txt = txt + $(td1[10].children[0]).val() + "}";
                        txt = txt + schd + "|";
                    }
                }
            }
            i = i + 1;
        }
        return txt;
    }
    //Submit the PODetails
    $scope.SubmitClick = function ()
    {
        //$("#div_loadImage").css("display", "block");
        //var date = $("#txtPODate").val();
        //console.log("datat");
        var txt = validate();
        if (txt != "")
        {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_PO/ET_Master_PO_Add",
                dataType: 'json',
                data: {
                    PP_ID: $scope.PP_ID,
                    PO_Code: $scope.PO_Code,
                    PO_Date: $("#txtPODate").val(),
                    PO_Type: $scope.PO_Type,
                    PO_OrderReference: $scope.PO_OrderReference,
                    Po_CurrencyKey: $scope.Po_CurrencyKey,
                    Po_Supplierkey: $scope.Po_Supplierkey,
                    Po_DeliveryShedule: $scope.Po_DeliveryShedule,
                    Po_PaymentTerms: $scope.Po_PaymentTerms,
                    Po_DeliveryDate: $("#txtDeliveryDate").val(),
                    Po_TotalAmount: parseFloat($("#txttotalamount").val().split('.').join("").replace(",", ".")),
                    Po_ShippingAddress: $scope.Po_ShippingAddress,
                    Po_StoreId: $scope.GI_StoreCode,
                    Po_SupplierAddress: $scope.Po_SupplierAddress,
                    PO_ScNo: $scope.txtScNo,
                    PO_ScDate: $("#txtSCDate").val(),
                    Po_SpecialInstruction: $scope.Po_SpecialInstruction,
                    Po_TermsandConditions: $scope.Po_TermsandConditions,
                    Q_CatalogId: $scope.Q_CatalogSelection,
                    PODetails: txt
                },
                headers: { "Content-Type": "application/json" }
            }
            );
            
            post.success(function (data, status)
            {
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
                else if (data.indexOf("Success") > -1)
                {
                    var res = data.split(':');
                    if ($scope.PP_ID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                    }
                    $scope.createnew();
                    setTimeout(() => {
                        $scope.showRecords();
                    }, 5000);  //5s
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
    //Edit The Records
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_PO/ET_Master_PO_Update_GetbyID",
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
                else {
                    debugger;
                    $('#viewpopup').html("");
                    var POByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    $scope.PO_Type = POByID.PO_Type;
                    $scope.PP_ID = POByID.PP_ID;
                    //Specifically Added for a Cloned Record having an Order
                    //Reference null(For One-One Trading PO)
                    if (POByID.PO_Type == "2") {
                        if (POByID.PO_OrderReference == "0") {
                            $scope.PP_ID = "0";
                        }
                    }
                    GetSupplier($scope.PP_ID);
                    getOrderReference($scope.PP_ID);
                    PaymentTerms($scope.PP_ID);

                    $scope.PO_Code = POByID.PO_Code;
                    $scope.Po_CurrencyKey = POByID.Po_CurrencyKey;
                    $scope.Po_PaymentTerms = POByID.Po_PaymentTerms;
                    $scope.Po_Supplierkey = POByID.Po_Supplierkey;
                    $scope.Po_SupplierAddress = POByID.Po_SupplierAddress;
                    $scope.txtScNo = POByID.PO_SCNo;
                    $scope.Po_ShippingAddress = POByID.Po_ShippingAddress;
                    $scope.GI_StoreCode = POByID.PO_SMId;
                    $scope.Po_TotalAmount = parseFloat(POByID.Po_TotalAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.Po_SpecialInstruction = POByID.Po_SpecialInstruction;
                    $scope.Po_TermsandConditions = POByID.Po_TermsandConditions;
                    $scope.Po_DeliveryShedule = POByID.Po_DeliveryShedule;
                    $scope.PO_OrderReference = POByID.PO_OrderReference;
                    OrderTypeChanged();
                    GetStoreMaster($scope.PP_ID);
                    GetCatalogList(0);
                    GetProducts();
                    $("#chkDeliveryShedule").prop('checked', POByID.Po_DeliveryShedule);
                    $("#drpcurrency").val(POByID.Po_CurrencyKey).trigger("chosen:updated");
                    $("#drpsuppliername").val(POByID.Po_Supplierkey).trigger("chosen:updated");
                    $("#drpOrderReference").val(POByID.PO_OrderReference).trigger("chosen:updated");
                    $("#drpType").val(POByID.PO_Type).trigger("chosen:updated");
                    if (POByID.Q_CatalogId == null)
                        POByID.Q_CatalogId = "";

                    $scope.Q_CatalogSelection = POByID.Q_CatalogId;
                    $("#drpCatalogSection").val(POByID.Q_CatalogId).trigger("chosen:updated");
                   
                    getproductBySupplierAndOrder(POByID.PP_ID, POByID.Po_Supplierkey);
                    var Quotationdate = new Date(parseInt(POByID.PO_Date.substr(6)));
                    var QDT = ("0" + Quotationdate.getDate()).slice(-2) + "-" + ("0" + (Quotationdate.getMonth()+1)).slice(-2) + "-" + Quotationdate.getFullYear();

                    $("#txtPODate").val(QDT);
                
                    if (POByID.PO_SCDate == null) {
                        $("#txtSCDate").val("");
                    }
                    else {
                        var SCdate = new Date(parseInt(POByID.PO_SCDate.substr(6)));
                        var SCDT = ("0" + SCdate.getDate()).slice(-2) + "-" + ("0" + (SCdate.getMonth()+1)).slice(-2) + "-" + SCdate.getFullYear();

                        $("#txtSCDate").val(SCDT);
                    }
                    if (POByID.PO_Date == null) {
                        $("#txtDeliveryDate").val("");
                    }
                    else {
                        var DeliveryDate = new Date(parseInt(POByID.PO_Date.substr(6)));
                        var DDate = ("0" + DeliveryDate.getDate()).slice(-2) + "-" + ("0" + (DeliveryDate.getMonth() + 1)).slice(-2) + "-" + DeliveryDate.getFullYear();
                        $("#txtDeliveryDate").val(DDate);
                    }
                    PODetails(POByID.PP_ID);
                   // $scope.DeliveyScheduleChange();

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
    //code for print Option
    $scope.ChangeLanguage = function (lang) {
        //debugger;
        var id = 0;
        //if (lang == "E") {
        //    id = parseInt($("#germanformat").val());
        //}
        //else {
        //    id = parseInt($("#englishformat").val());
        //}
        if ($scope.PO_Id != null || $scope.PO_Id != "")
            id = parseInt($scope.PO_Id);

        $scope.PrintRecords(id, lang);
    }
    $scope.PrintRecords = function (a, b) {
        a = parseInt(a);
        $scope.PO_Id = a;
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_PO/ET_Purchase_PO_Print",
                dataType: 'html',
                data: {
                    id: a,
                    lang: b,
                    orderType: $scope.PO_Type
                },
                headers: { "Content-Type": "application/json" }
            });
            post.success(function (data, status)
            {
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
                    //alert(data);
                    //var fso = CreateObject("Scripting.FileSystemObject");
                    //var s = fso.CreateTextFile("C://filename.txt", True);
                    //s.writeline(data);
                    $('#div_PrintContent').html(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");

                    var post1 = $http({
                        method: "POST",
                        url: "/ET_Purchase_PO/ET_Purchase_PO_DetailsPrint",
                        dataType: 'html',
                        data: {
                            id: a,
                            lang: b,
                            orderType: $scope.PO_Type
                        },
                        headers: { "Content-Type": "application/json" }
                    });
                    post1.success(function (data, status) {
                        //debugger;
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
                            $("#PAthid").attr("href", "/Purchase/PDFList/PO/" + data[0].PO_Code + "/" + data[0].PO_Code + ".pdf ");
                            $("#PAthid").text(data);
                            $("#div_View").css("display", "none");
                            $("#div_Restore").css("display", "none");
                            $("#div_Edit").css("display", "none");
                            $("#div_Print").css("display", "block");
                        }
                    });
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
    };

    $scope.Storechange = function () {
        //GetProducts();
    };

    function getproductBySupplierAndOrder(PP_ID, SupplierID) {
        
        var getProducts = $http.get("/ET_Purchase_PO/GetProductBySupplierAndOrder",
        {
            params: {
                PP_ID: PP_ID,
                SupplierKey: SupplierID,
            }
        });
        getProducts.then(function (productlist) {
             
            var res = JSON.parse(productlist.data);
            $scope.products = res;
        }, function () {
            alert("Supplier Not Found");
        });
           
    }
    //Get PO Product Details
    function PODetails(e)
    {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Purchase_PO/ET_Master_PO_Details_Load',
            type: 'html',
            data: {
                id: id
            },
        }).success(function (data)
        {
            //angular.element(document.getElementById('divTarget')).html("");
            //var trData = data.find("tr");
            var contactdata = JSON.parse(data);
            var i = contactdata.length;
            var j = 0;
            length = 1;
            angular.element(document.getElementById('divTarget')).html("");
           while (i != 0)
            {
               //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct_@" + length + "'  onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtPackage' class='form-control'/></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><input type='number' id='txttax' class='form-control' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
               // var Rowhtml = "<tr><td><select class='form-control' id='drpProduct_@" + length + "'  onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtPackage' class='form-control'/></td><td><input type='number' id='txtQuantity' class='form-control' /></td><td><input type='number' id='txttax' class='form-control' /></td><td><input type='number' id='txttotalAmount' class='form-control' readonly='readonly'/></td> <td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event,'id_@i')'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
               //var temp = $compile(Rowhtml)($scope);
               //var temp = $compile(data)($scope);
               //alert($scope.products);
               //alert(temp);
               var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' class='form-control' /></td><td style='display:none'><input type = 'number' id = 'txtOrderDetailId' class='form-control' readonly = 'readonly' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo +'-'+product.P_Name }}</option ></select></td><td style='display:none'><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtDesid' name='txtDesid' class='form-control' /></td><td><input type='text' id='txtQuantity' style='text-align:right' onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txtUnitPrice' style='text-align:right' onchange='calculateamount(this)' class='form-control' /></td><td><input type='text' id='txttotalAmount' style='text-align:right' class='form-control' readonly='readonly'/></td><td ng-if='PO_Type == 2'><input type='text' id='txtDesign' class='form-control'/></td><td><input type='text' id='txtSupDesc' name='txtSupDesc' class='form-control' /></td><td><a id='id_addnewrow' style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a><a ng-if='isschedule == true' style='padding: 5px;color:green' title='Delivery Schedule' data-toggle='collapse' href='#collapseExampleid_@i' class='hideandshow' aria-expanded='false' aria-controls='collapseExampleid_@i' id='btnProductTarget'><i class='fa fa-truck'></i></a></td></tr>";
               var temp = $compile(Rowhtml)($scope);
               //  alert(temp);
               angular.element(document.getElementById('divTarget')).append(temp);
               //GetProducts();
               $('.chosen-select').chosen();
               //alert('data chosen');
               length++;
               i--;
               j++;
            }
            GetProducts();
            //angular.element(document.getElementById('divTarget')).html(data);
            //$('.chosen-select').chosen();
            $scope.PODetails1 = contactdata;
            //$scope.PODetails1 = data;
        });
    }
    //Getting the Deleted Records 
    $scope.PerformRestore = function (a, $event, b) {

        var post = $http({
            method: "POST",
            url: "/ET_Purchase_PO/ET_Master_PO_RestoreDelete",
            dataType: 'json',
            data: {
                id: a,
                type: b,
                type1: $scope.PO_Type
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

   //function deleteModifiedRecordConfirm(e, f)
   // {
   //     alert('delete record confirm');
   //     var res = false;
   //     var record = parseInt(f); 
   //     res = $scope.Isdelete;
   //     if (res) {
   //         var post = $http({
   //             method: "POST",
   //             url: "/ET_Purchase_PO/ET_Master_PO_RestoreDelete",
   //             dataType: 'json',
   //             data: {
   //                 id: record,
   //                 type: true,
   //                 type1: $scope.PO_Type
   //             },
   //             headers: { "Content-Type": "application/json" }
   //         });
   //         post.success(function (data, status) {
   //             if (data == "Session Expired") {
   //                 $window.location.href = '/ET_Login/ET_SessionExpire';
   //             }
   //             else if (data.indexOf("ERR") > -1) {
   //                 $("#spanErrMessage1").html(data);
   //                 $("#spanErrMessage2").html(data);
   //                 if ($("#exceptionmessage").length)
   //                     $("#exceptionmessage").trigger("click");
   //             }
   //             else {
   //                 if (data == "Success") {
   //                     $($event.target.parentElement.parentElement.parentElement).fadeOut(800, function () { $(this).remove(); });
   //                     if (b) {
   //                         message = "Record Deleted Successfully.";
   //                     }
   //                     else {
   //                         message = "Record Restored Successfully.";
   //                     }

   //                     toastr["success"](message, "Notification");
   //                 }
   //                 else {
   //                     message = "Failed to do this operation.";
   //                     toastr["error"](message, "Notification");
   //                 }
   //             }
   //         });
   //         post.error(function (data, status) {
   //             $window.alert(data.Message);
   //         });
   //     }
   // };
    $scope.Restoredeleterecords = function (a, $event, b) {
        //debugger;
        if (b) {
            $scope.Isdelete = true;
            $scope.Isrestore = false;
            alertmessage(a.toString(), $event, $scope.PO_Type, 'ET_Purchase_PO', 'ET_Master_PO_RestoreDelete');
        }
        else {
            $scope.Isdelete = false;
            $scope.Isrestore = true;
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    }; 
    //function deletePORecordConfirm(e, f, type1, ctrlName, ctrlAction) {
    //    //debugger;
    //    var res = false;
    //    var deleteUrl = '/' + ctrlName + '/' + ctrlAction;
    //    var rowId = parseInt(f);
    //    res = true;
    //    if (res) {
    //        $.ajax({
    //            type: "POST",
    //            url: deleteUrl,
    //            data:
    //                JSON.stringify(
    //                    {
    //                        id: rowId,
    //                        type: true,
    //                        type1: type1
    //                    }),
    //            contentType: "application/json;charset=utf-8",
    //            dataType: "json",
    //            success: function (result) {
    //                if (result.indexOf("ERR") > -1) {
    //                    $("#spanErrMessage1").html(result);
    //                    $("#spanErrMessage2").html(result);
    //                    if ($("#exceptionmessage").length)
    //                        $("#exceptionmessage").trigger("click");
    //                }
    //                else {
    //                    if (result == "Success") {
    //                        message = 'Record Deleted Successfully. You can retrive it from Restore page';
    //                        toastr["success"](message, "Notification");
    //                        var ctrl = "#" + f;
    //                        $(ctrl).parent().parent().fadeOut(800, function () { $(this).remove(); })
    //                    }
    //                    else {
    //                        message = 'Failed To Delete';
    //                        toastr["error"](message, "Notification");
    //                    }
    //                }
    //            },
    //            error: function (response) {
    //                alert(response.responseText);
    //                if ($("#exceptionmessage").length)
    //                    $("#exceptionmessage").trigger("click");
    //            }
    //        });
    //    }
    //    setTimeout(() => {
    //        location.reload();
    //    }, 1000);  //1s
    //}

    //Viewing The PO List
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
         
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_PO/ET_Purchase_PO_View",
                dataType: 'html',
                data: {
                    id: a
                },
                headers: { "Content-Type": "application/json" }
            });
            post.success(function (data, status) {
                debugger;
                if (data == "Session Expired") {
                    $window.location.href = '/ET_Login/ET_SessionExpire';
                }
                else if (data.indexOf("ERR") > -1)
                {
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
    //Watch for All Data binding
    $scope.$watch("currencylist", function (value) {
        var val = value || null;
        if (val) {
             
            setTimeout(function () { $("#drpcurrency").val($scope.Po_CurrencyKey).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("orderreferencelist", function (value) {
         
        var val = value || null;
        if (val) {
            
            setTimeout(function () { $("#drpOrderReference").val($scope.PO_OrderReference).trigger("chosen:updated"); }, 100);
        }
    }); 
    $scope.$watch("supplierslist", function (value) {
         
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpsuppliername").val($scope.Po_Supplierkey).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("paymenttermlist", function (value) {

        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpPaymentTerms").val($scope.Po_PaymentTerms).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("storelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("GI_StoreCode", function (value) {
            setTimeout(function () { $("#drpStoreCode").val($scope.GI_StoreCode).trigger("chosen:updated"); }, 100);
    });
    $scope.$watch("storelists", function (value) {

        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpStoreCode").val($scope.GI_StoreCode).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("porestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
    $scope.$watch("PODetails1", function (value) {
        var val = value || null;
        if (val)
        {
            var PODetails1 = $scope.PODetails1;
            setTimeout(function () {
                try {
                    $("#divTarget").find("script").remove();
                }
                catch (ex) {
                    message = "Error Retreiving PO Details";
                    toastr["error"](message, "Notification");
                    return "";
                }
                var tbl = $("#divTarget").children();
                var txt = "";
                var total = 0;
                for (var i = 0; i < tbl.length; i++)
                {
                    var td1 = tbl[i].children;
                    $(td1[0]).find("input").val(PODetails1[i].SO_Serial);
                    $(td1[1]).find("input").val(PODetails1[i].PO_OrderDetailID);
                    $(td1[2].children[0]).val($(td1[2].children[0]).attr("ng-init"));
                    setTimeout(function () { $(td1[2]).find("select").val(PODetails1[i].PD_ProductID).trigger("chosen:updated"); }, 1000);
                    $(td1[2]).find("select").val(PODetails1[i].PD_ProductID).trigger("chosen:updated");
                    $(td1[2]).find("p").text(PODetails1[i].PD_ShortName);
                    $(td1[3]).find("input").val(PODetails1[i].PD_ShortName);
                    $(td1[4]).find("input").val(PODetails1[i].PD_UOM);
                    $(td1[5]).find("input").val(PODetails1[i].Description);
                    $(td1[6]).find("input").val(parseFloat(PODetails1[i].PD_Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td1[7]).find("input").val(parseFloat(PODetails1[i].PD_UnitPrice).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td1[8]).find("input").val(parseFloat(PODetails1[i].PD_TotalAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td1[9]).find("input").val(PODetails1[i].DesignDetail);
                    $(td1[10]).find("input").val(PODetails1[i].PD_SuppDef);
                    if ($scope.PO_Type == 2)
                    {
                        //alert("trading type");
                        //$(td1[1].find("select")).setAttribute("disabled", "disabled");
                        //$(td1[4].children[0]).setAttribute("disabled", "disabled");
                    }
                    var unitprice = parseFloat(PODetails1[i].PD_UnitPrice);
                    var quantity = parseFloat(PODetails1[i].PD_Quantity);
                    var amt = quantity * unitprice;
                    total = total + amt;
                    if (unitprice == 0)
                    {
                        message = "Price is not defined in the Catalog Selected for Article:" + PODetails1[i].PD_ShortName;
                        toastr["error"](message, "Notification");
                    }
                    //else
                    //{
                    //    message = "Price is defined in the Catalog Selected for Article:" + PODetails1[i].PD_ShortName;
                    //    toastr["success"](message, "Notification");
                    //}
                }
                $("#txttotalamount").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                //alert('completion target finished');
                //i = i + 2;
            }, 5000);
        }
    });

});