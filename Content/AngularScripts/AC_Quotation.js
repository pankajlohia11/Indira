app1.controller("AC_QuotationCtrl", function ($scope, $http, $window, $compile) {
    function GetTypeFromURL() {
        var type = window.location.href.toString().split("type=");
        $scope.Type = type[1];
        if ($scope.Type == "Agency") {
            $scope.Q_Type = "1";
        }
        else if ($scope.Type == "Trading") {
            $scope.Q_Type = "2";
        }
        else if ($scope.Type == "Store") {
            $scope.Q_Type = "3";
        }
        $("#drpType").val($scope.Q_Type).trigger("chosen:updated");
    }
    var length = 1;
    GetTypeFromURL();
    GetPrivilages();
    GetQuotationList();
    GetCurrency();
    //Check the privilages
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Sales_Quotation/GetPrivilages");
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
    //Get the quoation list
    function GetQuotationList() {
        var getquotationlist = $http.get("/ET_Sales_Quotation/GetQuotationList",
            {
                params: { delete: false, type: $scope.Q_Type}
            });
        getquotationlist.then(function (quotationlist) {
            var res = JSON.parse(quotationlist.data);
            $scope.quotationlist = res;
        }, function () {
            alert("Quotation List Found");
        });
    }
    //Get the deleted quation
    function GetQuotationRestoreList() {
        var getquotationrestorelist = $http.get("/ET_Sales_Quotation/GetQuotationList",
            {
                params: { delete: true, type: $scope.Q_Type }
            });
        getquotationrestorelist.then(function (quotationrestorelist) {
            var res = JSON.parse(quotationrestorelist.data);
            $scope.quotationrestorelist = res;
        }, function () {
            alert("Quotation Restore List Found");
        });
    }
    //Get the customer list
    function GetCustomerList() {
        var getcustomerlist = $http.get("/ET_Sales_Quotation/GetCustomers",
            {
                params: { id: $scope.Q_ID }
            });
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
        }, function () {
            alert("Customer Not Found");
        });
    }

    //Gets the catalog list from the database.
    function GetCatalogList(catalogCode)
    {
        var getCatalogList = $http.get("/ET_Sales_Quotation/GetCatlogList",
            {
                params: { catalogId: catalogCode}
            });
        getCatalogList.then(function (catalogCollection) {
            var res = JSON.parse(catalogCollection.data);
            $scope.catalogList = res;
            //$("#drpCatalogSection").val(QuotationByID.Q_CatalogSelection).trigger("chosen:updated");
        }, function () {
            alert("Catalog List not found");
        });
    }

  //Get the currency list
    function GetCurrency() {
        var getcurrencylist = $http.get("/ET_Sales_Quotation/GetCurrency");
        getcurrencylist.then(function (currencylist) {
            var res = JSON.parse(currencylist.data);
            $scope.currencylist = res;
            $scope.Q_CurrencyCode = "";
        }, function () {
            alert("Currency Not Found");
        });
    }
    //Get the salesperson list
    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Sales_Quotation/GetSalesPerson",
            {
                params: { id: $scope.Q_ID, type: $scope.Q_Type }
            });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
        }, function () {
            alert('Data not found');
        });
    }
    //Get the payment terms
    function PaymentTerms() {
        $http({
            method: 'POST',
            url: '/ET_Sales_Quotation/Payment_terms_dropdown',
            data: { id: $scope.Q_ID }
        }).success(function (data) {
            var payment = JSON.parse(data)
            $scope.paymenttermlist = payment;
        });
    }
    //GEt the product
    function GetProductList()
    {
        //$.ajax({
        //    type: "GET",
        //    url: "/ET_Sales_Quotation/GetProducts",
        //    async: false,
        //    data: {
        //        id: $scope.Q_ID
        //    },
        //    success: function (result) {
        //        var res = JSON.parse(result);
        //        $scope.products = res;
        //    }
        //});
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
            error: function (response) {
                toastr["error"]("Product Data Not Found", "Notification");
            }
        });
    }

    //Customer change
    $scope.CustomerChange = function () {
        debugger;
        $scope.Q_EnquiryID = "0";
        $("#drpEnquiry").val("0").trigger("chosen:updated");
        GetSalesPersonForCustomer();
       
        GetEnquiries();
    }
    //Get the enqiry list
    function GetEnquiries() {
        debugger;
        if ($scope.Q_CustomerID != "" && $scope.Q_Type != "") {
            var getenquirylist = $http.get("/ET_Sales_Quotation/GetEnquiries",
                {
                    params: { id: $scope.Q_ID, custid: $scope.Q_CustomerID, type: $scope.Q_Type }
                });
            getenquirylist.then(function (enquirylist) {
                var res = JSON.parse(enquirylist.data);
                $scope.enquiries = res;
                if ($scope.Q_ID != 0) {
                    GetSalesPersonForCustomer();
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
    //Get the sales person for customer
    function GetSalesPersonForCustomer() {
        if ($scope.Q_CustomerID != "" && $scope.Q_Type != "") {
            var SalesPerson = $http.get("/ET_Sales_Quotation/GetSalesPersonForCustomer",
                {
                    params: { custid: $scope.Q_CustomerID }
                });
            SalesPerson.then(function (sPerson) {
                var data = sPerson.data;
                $scope.Q_Sales_Person = data[0];
                if ($scope.Q_Sales_Person == 0) {
                    message = "Sales Person not assigned in company master";
                    toastr["error"](message, "Notification"); 
                }
                else {
                    $("#drpSalesPerson").val($scope.Q_Sales_Person).trigger("chosen:updated");
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
    //Enquiry change
    $scope.EnquiryChange = function () {
        $("#txtTotalValue").val("0");
        if ($scope.Q_EnquiryID != "0") {
            EnquiryDetails($scope.Q_EnquiryID);
        }
        else {
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(1)'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px; ' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px; color: red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(1)'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('quotationdetailsbody')).html("");
            angular.element(document.getElementById('quotationdetailsbody')).append(temp);
            $('.chosen-select').chosen({ width: "95%" });
            GetProductList();
        }
    };

    $scope.CatalogSelection = function ()
    {
        GetProductList();

        $http({
            method: 'POST',
            url: '/ET_Admin_ProductCatalog/GetProductCatalogValidityDate',
            data: {
                Q_CatalogId: $scope.Q_CatalogSelection
            }
        }).success(function (result)
        {
            var savedValidation = new Date(parseInt(result.substr(6)));
            var currentDate = new Date();
            if (savedValidation < currentDate)
            {
                var Orderdt = ("0" + savedValidation.getDate()).slice(-2) + "-" + ("0" + (savedValidation.getMonth() + 1)).slice(-2) + "-" + savedValidation.getFullYear();
                message = "Catalog Validity is Expired as on:" + Orderdt ;
                toastr["error"](message, "Notification");
                return;
            }
            });

        //if ($scope.Q_EnquiryID != "0")
        //{
        //    EnquiryDetails($scope.Q_EnquiryID);
        //}
        //else
        //{
        //    //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
        //    var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form - control chosen - select' ng-change='PriceTypeChange()'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
        //    var temp = $compile(Rowhtml)($scope);
        //    angular.element(document.getElementById('quotationdetailsbody')).html("");
        //    angular.element(document.getElementById('quotationdetailsbody')).append(temp);
        //    $('.chosen-select').chosen({ width: "95%" });
        //    GetProductList();
        //}

        $http({
            method: 'POST',
            url: '/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Details_UpdateChildtable',
            data: {
                id: $scope.Q_CatalogSelection
            }
        }).success(function (data)
        {
            var contactdata = JSON.parse(data);
            var i = contactdata.length;
            var j = 0;
            length = 1;
            //alert("length:" + i);
            //angular.element(document.getElementById('quotationdetailsbody')).html("");
            //while (i != 0)
            //{
            //    //var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtSerial' value='{{ item.SO_Serial }}' class='form-control' /><p style='display:none;'></p></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            //    //<td><p style='display:none;'></p><input readonly='readonly' type='text' id='txtLastUpdated' class='form-control' /></td>;
            //        //<td><input type='checkbox' name='select_all' value=' " + j + "' id='product-select-all'></td>
            //    //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px; ' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px; color: red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //    var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange("+length+")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
            //    //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td></tr>";
            //    var temp = $compile(Rowhtml)($scope);
            //    angular.element(document.getElementById('quotationdetailsbody')).append(temp);
            //    $('.chosen-select').chosen();
            //    length++;
            //    //ProductDetails();
            //    GetProductList();
            //    i--;
            //    j++;
            //}
            $scope.CatalogDetails = contactdata;
        });
    };


    $scope.TypeChange = function () {
        if ($scope.Q_CustomerID != "" && $scope.Q_Type != "") {
            GetEnquiries();
        }
    };

    //Price Type Change(Summer/Winter Price)
    $scope.PriceTypeChange = function (e)
    {
        var index = parseInt(e);
        var tr = $("#quotationdetailsbody").find("tr");
        var td = tr[index - 1].cells;
        var total = 0.0;
        //var td = $(e).parent().parent().find("td");
        var catalogData = $scope.CatalogDetails;
        if ($scope.Q_PriceType == 1)
        {
            //Update the total Price for the 
            $(td[4]).find("input").val(catalogData[index - 1].P_SummerPrice.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        }
        else
        {
            $(td[4]).find("input").val(catalogData[index - 1].P_WinterPrice.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        }

        var currentRowPrice = parseFloat($(td[4]).find("input").val().split('.').join("").replace(",", "."));
        var currentRowQty = parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", "."));
        //Update the total row price.
        $(td[6]).find("input").val((currentRowPrice * currentRowQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        for (var i = 0; i < tr.length; i++)
        {
            //var td1 = tr[i].children;
            var amt = parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", "."));
            total = total + amt;
        }
        $("#txtTotalValue").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        //alert($scope.Q_PriceType);
        // $scope.EnquiryChange();
    };

    $scope.CurrencyChange = function () {}
    $scope.SalesPersonChange = function () {
    }
    //Add the new product
    $scope.addnewrow = function (a) {
        var e = a.target;
        var tr = $("#quotationdetailsbody").find("tr");
        var txt = "";
        for (var i = 0; i < tr.length; i++)
        {
            var td = tr[i].cells;
           // var Product = $(td[0]).find("select").val();
            var Product = $(td[1]).find("p").text();
            var Price = $(td[3]).find("input").val();
            var Qty = $(td[4]).find("input").val();
            //var Tax = $(td[7]).find("input").val();
            if (Product == "") {
                message = "Select Product at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Price == "") {
                message = "Enter the Price at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Qty == "") {
                message = "Enter the Quantity at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            //else if (Tax == "") {
            //    message = "Enter the Tax at row " + (i + 1);
            //    toastr["error"](message, "Notification");
            //    txt = "asd";
            //}
        }
        if (txt == "")
        {
            //var rowlength = tr.length + 1;
            length = (length + 1);
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange("+length+")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('quotationdetailsbody')).append(temp);
            $('.chosen-select').chosen({ width: "95%" });
            GetProductList();
        }
    }
    //Delete the product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#quotationdetailsbody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
            message = "Atleast one Product required";
            toastr["error"](message, "Notification");
        }
    }
    //View the list
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetQuotationList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //View the deleted list
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetQuotationRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //Create the new quotation
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#div_Print").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.Q_ID = "0";
            $scope.Q_Code = "";
            $scope.Q_CustomerID = "";
            $scope.Q_EnquiryID = "0";
            $scope.Q_CatalogSelection = "0";
            $scope.Q_CurrencyCode = "";
            $scope.Q_PaymentTerms = "";
            $scope.Q_DeliveryTerms = "";
            $scope.Q_ValidityDays = "";
            $scope.Q_Freight_Cost = "";
            $scope.Q_Enclosures = "";
            $scope.Q_TotalValue = "";
            $scope.Q_Sales_Person = 0;
            $scope.Q_PaymentTerms = "";
            $scope.Q_Quotedescription = "";
            $scope.Q_specialdescription = "";
            $scope.Q_TotalValue = "";
            $scope.Q_PriceType = "1";
            $scope.Q_CatalogSelection = "";
            $("#drpPriceType").val("1").trigger("chosen:updated");
            $("#txtTotalValue").val("");
            var Orderdate = new Date();
            var Orderdt = ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear();
            $("#txtQDate").val(Orderdt);
            $("#drpEnquiry").val("").trigger("chosen:updated");
            $("#drpCurrency").val("").trigger("chosen:updated");
            $("#drpCatalogSection").val("").trigger("chosen:updated");
            $("#chkWarrenty").prop('checked', false);

            GetSalesPersonList();
            GetCustomerList();
            GetCatalogList(0);
            PaymentTerms();
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='1' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='1' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange("+length+")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice'  style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('quotationdetailsbody')).html("");
            angular.element(document.getElementById('quotationdetailsbody')).append(temp);
            $('.chosen-select').chosen({ width: "95%" });
            GetProductList();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };
    //Validate the data
    function validate() {
        var tr = $("#quotationdetailsbody").find("tr");
        var txt = "";
        if ($scope.Q_Type == "") {
            message = "Select Type";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtQDate").val() == "") {
            message = "Select Quotation Date";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Q_CustomerID == "")
        {
            message = "Select Customer";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Q_CatalogSelection == "") {
            message = "Select Product Catalog";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Q_Sales_Person == 0) {
            message = "Select Sales Person";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Q_CurrencyCode == "") {
            message = "Select Currency";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($("#txtTotalValue").val() == "") {
            message = "Total Value Cannot be empty. Select Products";
            toastr["error"](message, "Notification");
            return "";
        }
        if ($scope.Q_ValidityDays == "") {
            message = "Enter Validity Days";
            toastr["error"](message, "Notification");
            return "";
        }
        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
           // var product = $(td[0]).find("select").val();
            var product = $(td[1]).find("p").text();
            var price = $(td[4]).find("input").val();
            var qty = $(td[5]).find("input").val();
            if (product == "") {
                message = "Select Product Name at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            } else if (price == "") {
                message = "There is no Price for this Product in Price Catalog at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(price.split('.').join("").replace(",", ".")) == 0) {
                message = "There is no Price for this Product in Price Catalog at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (qty == "") {
                message = "Enter Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(qty.split('.').join("").replace(",", ".")) == 0) {
                message = "Enter Quantity at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", ".")) == 0) {
                message = "Amount cannot be 0 at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                txt = txt + product + "}";
                txt = txt + $(td[2]).find("input").val() + "}";
                //txt = txt + $(td[3]).find("select").val() + "}";
                txt = txt + parseFloat($(td[3]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                txt = txt + parseFloat($(td[4]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + "}"; 
                txt = txt + $(td[6]).find("input").val() + "|";
            }
        }
        return txt;
    }
    //Save the records
    $scope.SubmitClick = function () {
        $("#div_loadImage").css("display", "block");
        var txt = validate();
        var date = $("#txtQDate").val();
        if ($scope.Q_Freight_Cost == "")
            $scope.Q_Freight_Cost = "0";
        if (txt != "") {
            debugger;
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Quotation/ET_Master_Quotation_Add",
                dataType: 'json',
                data: {
                    Q_ID: $scope.Q_ID,
                    Q_Code: $scope.Q_Code,
                    Q_Type: $scope.Q_Type,
                    Q_PriceType: $scope.Q_PriceType,
                    Q_Date: date,
                    Q_CustomerID: $scope.Q_CustomerID,
                    Q_EnquiryID: $scope.Q_EnquiryID,
                    Q_CurrencyCode: $scope.Q_CurrencyCode,
                    Q_PaymentTerms: $scope.Q_PaymentTerms,
                    Q_DeliveryTerms: $scope.Q_DeliveryTerms,
                    Q_ValidityDays: $scope.Q_ValidityDays,
                    Q_Freight_Cost: parseFloat($scope.Q_Freight_Cost.split('.').join("").replace(",", ".")),
                    Q_Enclosures: $scope.Q_Enclosures,
                    Q_TotalValue: parseFloat($("#txtTotalValue").val().split('.').join("").replace(",", ".")),
                    Q_Sales_Person: $scope.Q_Sales_Person,
                    Q_Quotedescription: $scope.Q_Quotedescription,
                    Q_specialdescription: $scope.Q_specialdescription,
                    Q_CatalogId: $scope.Q_CatalogSelection,
                    QuotationDetails: txt
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
                    if ($scope.Q_ID == 0) {
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
        else
        {
            $("#div_loadImage").css("display", "none");
        }

    }
    //Edit the details
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Quotation/ET_Master_Quotation_Update_GetbyID",
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
                    var QuotationByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $("#div_Print").css("display", "none");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    
                    $scope.Q_ID = QuotationByID.Q_ID;
                    $scope.Q_Code = QuotationByID.Q_Code;
                    $scope.Q_Type = QuotationByID.Q_Type;
                    $scope.Q_CustomerID = QuotationByID.Q_CustomerID;
                    $scope.Q_EnquiryID = QuotationByID.Q_EnquiryID;
                    $scope.Q_CurrencyCode = QuotationByID.Q_CurrencyCode;
                    $scope.Q_PaymentTerms = QuotationByID.Q_PaymentTerms;
                    $scope.Q_DeliveryTerms = QuotationByID.Q_DeliveryTerms;
                    $scope.Q_ValidityDays = QuotationByID.Q_ValidityDays;
                    $scope.Q_Freight_Cost = parseFloat(QuotationByID.Q_Freight_Cost).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.Q_Enclosures = QuotationByID.Q_Enclosures;
                    $scope.Q_TotalValue = parseFloat(QuotationByID.Q_TotalValue).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    $scope.Q_Sales_Person = QuotationByID.Q_Sales_Person;
                    $scope.Q_Quotedescription = QuotationByID.Q_Quotedescription;
                    $scope.Q_specialdescription = QuotationByID.Q_specialdescription;
                    $scope.Q_PriceType = QuotationByID.Q_PriceType;
                    $scope.Q_CatalogSelection = QuotationByID.Q_CatalogId;
                    $("#drpPriceType").val(QuotationByID.Q_PriceType).trigger("chosen:updated");
                    $("#drpCatalogSection").val(QuotationByID.Q_CatalogId).trigger("chosen:updated");
                    $scope.CatalogSelection();
                    var Quotationdate = new Date(parseInt(QuotationByID.Q_Date.substr(6)));
                    var QDT = ("0" + Quotationdate.getDate()).slice(-2) + "-" + ("0" + (Quotationdate.getMonth() + 1)).slice(-2)+ "-" + Quotationdate.getFullYear();
                    $("#txtQDate").val(QDT);
                    GetSalesPersonList();
                    GetCustomerList();
                    GetCatalogList(0);
                    PaymentTerms();
                    GetEnquiries();
                    $("#drpCurrency").val($scope.Q_CurrencyCode).trigger("chosen:updated");
                    QuotationDetails(QuotationByID.Q_ID);
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
   // var length = 1;
    //Get the quotation product details
    function QuotationDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Sales_Quotation/ET_Master_Quatation_Details',
            data: {
                id: id,
            },
        }).success(function (data)
        {
            debugger;
            var enquirydata = JSON.parse(data);
            var i = enquirydata.length;
            var j = 0;
            length = 1;
            angular.element(document.getElementById('quotationdetailsbody')).html("");
            while (i != 0)
            {
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('quotationdetailsbody')).append(temp);
                $(".chosen-select").chosen();
                i--;
                length++;
                GetProductList();
                j++;
            }
            $scope.enquirydata1 = enquirydata;
        });
    }

    function SleepPGUpdates(milliseconds) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if ((new Date().getTime() - start) > milliseconds) {
                break;
            }
        }
    };

    //Get the enquiry product details
    function EnquiryDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Sales_Quotation/ET_Master_Enquiry_Details',
            data: {
                id: id, priceType: $scope.Q_PriceType,
            },
        }).success(function (data) {
            
            var enquirydata = JSON.parse(data);
            var i = enquirydata.length;
            var j = 0;
            length = 1;
            angular.element(document.getElementById('quotationdetailsbody')).html("");
            while (i != 0)
            {
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right;' class='form-control' onchange='calculateamount(this);'/></td><td><input type='text' id='txtQuantity' style='text-align: right;' onchange='calculateamount(this);' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right;' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><select data-ng-model='Q_PriceType' id='drpPriceType' data-parsley-trigger='change' class='form-control chosen-select' ng-change='PriceTypeChange(" + length + ")'>< option value = '' > --Select--</option ><option value='1'>Summer Price</option><option value='2'>Winter Price</option></select ></td><td><input type='text' id='txtPrice' style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px; ' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px; color: red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProduct" + length + "' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo + '-' +product.P_ShortName  }}</option ></select></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPrice' style='text-align: right; ' class='form-control' onchange='calculateamount(this); '/></td><td><input type='text' id='txtQuantity' style='text-align: right; ' onchange='calculateamount(this); ' class='form-control' /></td><td><input type='text' id='txtAmount' style='text-align: right; ' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtDescription' class='form-control'  /></td><td><a style='padding: 5px; ' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px; color: red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('quotationdetailsbody')).append(temp);
                i--;
                $(".chosen-select").chosen();
                length++;
                GetProductList();
                j++;
            }
            $scope.enquirydata1 = enquirydata;
        });
    };
    
    $scope.RedirectQuotationToOrder = function (a) {

        window.location = '../ET_Sales_OrderDetails/ET_Sales_quotationtoOrderDetails?type=' + $scope.Type + '&QuoId=' + a;
    };

    //Get the restore product details
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Quotation/ET_Master_Quotation_RestoreDelete",
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
    //$scope.deleteRecordConfirm = function (e, f) {

    //    var res = false;
    //    //if (b)
    //    //    res = $scope.Isdelete;
    //    //else
    //    //    res = $scope.Isrestore
    //    res = $scope.Isdelete;
    //    if (res) {
    //        var post = $http({
    //            method: "POST",
    //            url: "/ET_Sales_Quotation/ET_Master_Quotation_RestoreDelete",
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
        if (b) {
            alertmessageModified($event, a.toString(),b,'ET_Sales_Quotation', 'ET_Master_Quotation_RestoreDelete');
        }
        else
        {
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    }     //View the records
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Quotation/ET_Master_Quotation_View",
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
    //Change language for print
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
    //Print the quoation
    $scope.PrintRecords = function (a, b) {

        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Quotation/ET_Master_Quotation_Print",
                dataType: 'html',
                data: {
                    id: a,
                    lang: b
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
                    debugger;
                    $scope.printid = a;
                    $('#div_PrintContent').html(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");
                }
            });

            post.error(function (data, status) {
            });
            var post1 = $http({
                method: "POST",
                url: "/ET_Sales_Quotation/ET_Master_QuotationPrint",
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
                    $("#PAthid").attr("href", "/Sales/PDFList/Quotation/" + data[0].Q_Code + "/" + data[0].Q_Code + ".pdf ");
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
    };

    $scope.$watch("CatalogDetails", function (value)
    {
        //var val = value || null;
        //if (val) {
        //    var contactdata = $scope.CatalogDetails;
        //    //alert("Inside catalog details");
        //    setTimeout(function () {
        //        var tr = $("#quotationdetailsbody").find("tr");
        //        var quantity = 0.0;
        //        var rowTotal = 0.0;
        //        var total = 0.0;
        //        for (var i = 0; i < tr.length; i++)
        //        {
        //            var td = tr[i].cells;
        //            //alert(contactdata[i].SO_Serial);
        //            $(td[1]).find("select").val(contactdata[i].P_ID).trigger("chosen:updated");
        //            $(td[1]).find("p").text(contactdata[i].P_ShortName);
        //            $(td[2]).find("input").val(contactdata[i].LU_Description);
        //            if (contactdata[i].P_SummerPrice == "")
        //                contactdata[i].P_SummerPrice = "0";
        //            $(td[4]).find("input").val(parseFloat(contactdata[i].P_SummerPrice).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        //            $(td[5]).find("input").val(parseFloat(quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        //            rowTotal = contactdata[i].P_SummerPrice * quantity;

        //            $(td[6]).find("input").val(rowTotal.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        //            $(td[7]).find("input").val(contactdata[i].P_Description);
        //            total = total + rowTotal;
        //            //var lastUpdatedDate = new Date(parseInt(contactdata[i].P_LAST_PRICE_REWISE_DATE.substr(6)));
        //            //var updatedDate = ("0" + lastUpdatedDate.getDate()).slice(-2) + "-" + ("0" + (lastUpdatedDate.getMonth() + 1)).slice(-2) + "-" + lastUpdatedDate.getFullYear();
        //            //$(td[7]).find("input").val(updatedDate);
        //        }
        //        $("#txtTotalValue").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
        //    }, 5000);
        //}
    });

    //Watch for all data binding
    $scope.$watch("customers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.Q_CustomerID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("catalogList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCatalogSection").val($scope.Q_CatalogSelection).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("products", function (value) {
        var val = value || null;
        if (val)
        {
            debugger;
            setTimeout(function () { $("#drpProduct" + length).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("enquiries", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpEnquiry").val($scope.Q_EnquiryID).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.Q_Sales_Person).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("currencylist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCurrency").val($scope.Q_CurrencyCode).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("paymenttermlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpPaymentTerms").val($scope.Q_PaymentTerms).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("quotationlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("quotationrestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
    $scope.$watch("enquirydata1", function (value)
    {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.enquirydata1;
            setTimeout(function () {
                debugger;
                var tr = $("#quotationdetailsbody").find("tr");
                var total = 0;
                for (var i = 0; i < tr.length; i++) {
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(enquirydata[i].SO_Serial);
                    $(td[1]).find("select").val(enquirydata[i].QD_ProductID).trigger("chosen:updated");
                    $(td[1]).find("p").text(enquirydata[i].QD_ProductID);
                    $(td[2]).find("input").val(enquirydata[i].QD_UOM);
                    if (enquirydata[i].QD_Unit_Price == "")
                        enquirydata[i].QD_Unit_Price = "0";
                    //$(td[3]).find("select").val(enquirydata[i].QD_CatalogPriceType).trigger("chosen:updated");
                    $(td[3]).find("input").val(parseFloat(enquirydata[i].QD_Unit_Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[4]).find("input").val(parseFloat(enquirydata[i].QD_Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[5]).find("input").val(parseFloat(enquirydata[i].QD_Unit_Price * enquirydata[i].QD_Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[6]).find("input").val(enquirydata[i].QD_Description);
                    total = total + parseFloat(enquirydata[i].QD_Unit_Price * enquirydata[i].QD_Quantity);
                }
                $("#txtTotalValue").val(total.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }, 100);
        }
    });
    //Send the mail
    $scope.sendMail = function () {
        debugger;
        var data1 = $("#tblprintpdf").html();
        data1 = data1.replace(/class="generaltable"/g, "style='font-size: 10px;line-height: 8px;'");
        data1 = data1.replace(/class="mediumtable"/g, "style='font-size: 8px;line-height: 8px;'");
        data1 = data1.replace(/class="footertable"/g, "style='font-size: 6px;line-height: 8px;'");
        data1 = data1.replace(/<hr style="font-weight:bold;border-top:1px solid black;">/g, "_____________________________________________________________________________________");
        var post = $http({
            method: "POST",
            url: "/ET_Sales_Quotation/SendMail",
            dataType: 'json',
            data: {
                formData: data1,
                id: $scope.printid
            },
            headers: { "Content-Type": "application/json" }
        });

        post.success(function (data, status) {
            if (data == "Session Expired") {
                $window.location.href = '/ET_Login/ET_SessionExpire';
            }
            try {
                if (data.indexOf("ERR") > -1) {
                    $("#spanErrMessage1").html(data);
                    $("#spanErrMessage2").html(data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else if (data.indexOf("Validation") > -1) {
                    toastr["error"](data, "Notification");
                }
                else if (data == "Success") {
                    toastr["success"]("Email has Sent Successfully", "Notification");
                }
                else {
                    toastr["error"]("Failed To Send Mail", "Notification");
                }
            }
            catch (ex)
            {
                $window.alert("Failed Quotation Details");
            }
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }


});