app1.controller("AC_OrderdetailsCtrl", function ($scope, $http, $window, $compile) {

    function GetTypeFromURL()
    {
        var type = window.location.href.toString().split("type=");
        $scope.Type = type[1];
        if ($scope.Type == "Agency") {
            $scope.SelectedOredertype = "1";
        }
        else if ($scope.Type == "Trading") {
            $scope.SelectedOredertype = "2";
        }
        else if ($scope.Type == "Store") {
            $scope.SelectedOredertype = "3";
        }
        $("#drpOrdertype").val($scope.SelectedOredertype).trigger("chosen:updated");
    }
    var length = 1;
    GetTypeFromURL();
    //ProductDetails(function () { });
    if ($scope.SelectedOredertype == "3") {
        AC_GetSalesOrder_List1();
    }
    else {
        AC_GetSalesOrder_List();
    }
    GetPrivilages();
    Bind_Currency();
    Bind_Packing();
    debugger;
  
    //View the orders
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
        if ($scope.SelectedOredertype == "3") {
            AC_GetSalesOrder_List1();
        }
        else {
            AC_GetSalesOrder_List();
        }
    }

    // Restore div in order
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
        AC_GetOrderRestore_List();
    }
    //Check privilages
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Sales_OrderDetails/GetPrivilages");
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
    //Get the order list
    function AC_GetSalesOrder_List() {
        var getac_salesorderlist = $http.get("/ET_Sales_OrderDetails/Tbl_OrderList",
            {
                params: { deleted: false, type: $scope.SelectedOredertype }
            });
        getac_salesorderlist.then(function (result) {
            var res = JSON.parse(result.data);
            $scope.orderlist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the order list
    function AC_GetSalesOrder_List1() {
        var getac_salesorderlist = $http.get("/ET_Sales_OrderDetails/Tbl_OrderList1",
            {
                params: { deleted: false, type: $scope.SelectedOredertype }
            });
        getac_salesorderlist.then(function (result) {
            var res = JSON.parse(result.data);
            $scope.orderlist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the all quation list
    function BindQuotation() {
        
        if ($scope.SelectedCustomer != "") {
            var getquotationlist = $http.get("/ET_Sales_OrderDetails/GetQuotations",
                {
                    params: { id: $scope.SelectedCustomer, orderid: $scope.OrderID, type: $scope.SelectedOredertype }
                });
            getquotationlist.then(function (result) {
                var res = JSON.parse(result.data);
                $scope.QuotationList = res;
            }, function () {
                alert("No Data Found");
            });
        }
        else
        {
            $scope.QuotationList = {};
            $scope.SelectedQuotationNo = 0;
        }
    }
    //Get restore list
    function AC_GetOrderRestore_List() {
        var getpaymentrestorelist = $http.get("/ET_Sales_OrderDetails/Tbl_OrderList",
            {
                params: { deleted: true, type: $scope.SelectedOredertype }
            });
        getpaymentrestorelist.then(function (result) {
            var res = JSON.parse(result.data);
            $scope.orderRestorelist = res;
        }, function () {
            alert("No Data Found");
        });
    }

    // Create new record in General Offer
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            //$("#hdncustomer").addClass("hidden");
            // $("#drpCurrency").val("0").trigger("chosen:updated");
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#formSubmit").attr('disabled', "disabled");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";

            $scope.ORGNAMEText = "";
            $scope.ORGIDValue = "";
            $scope.OrderCode = "";
            $scope.OrderID = "0";
            $scope.SelectedSalesperson = "";
            $("#drpSalesperson").val("").trigger("chosen:updated");
            $scope.ORGIDValue = "0";
            $("#txtorderdate").val("");
            $scope.SO_Discount = "";
            $scope.SelectedQuotationNo = "0";
            $("#drpQuotationNo").val("0").trigger("chosen:updated");

            $scope.SelectedCustomer = "";
            $scope.SelectedSupplier = "0";
            $scope.txtcusponumber = "";
            $("#cuspodate").val("");

            $scope.txtsubscnumber = "";
            $("#subscdate").val("");

            $scope.SelectedCusCurrency = "";
            $("#drpCusCurrency").val($scope.SelectedCusCurrency).trigger("chosen:updated");

            $scope.txtcommisionagy = "";
            $scope.txtCusdeliveryterms = "";
            $scope.remarks = "";
            $scope.SO_PriceType = "1";
            $scope.PL_Type = "1";
            //$("#drpPackType").attr('disabled', false);
            $("#drpPriceType").val("1").trigger("chosen:updated");
            $scope.SelectedCuspaymenttype = "";
            $("#txttotalamount").val("0");
            $scope.Q_CatalogSelection = "0";
            Customer();
            Supplier();
            CusPaymentTerms();
            Salesperson();
            $scope.changeOrdertyper();
            GetCatalogList(0);
            ProductDetails(function () { });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    //Get the product details
    function ProductDetails(_callback)
    {
        if ($scope.PL_Type == null || $scope.PL_Type == "")
            $scope.PL_Type = "1";
        if ($scope.Q_CatalogSelection == "" || $scope.Q_CatalogSelection == null)
            $scope.Q_CatalogSelection = $("#drpCatalogSection").val();

        $scope.ProductListshortname = "";
        //var jsdata = '{' + "'catalogCode:'" + $scope.Q_CatalogSelection + '}';

        $.ajax({
            type: "GET",
            url: '/ET_Admin_ProductCatalog/GetProductsForCatalog',
            dataType: "json",
            data: { catalogCode: $scope.Q_CatalogSelection },
            async:false,
            contentType: "application/json;charset=utf-8",
            success: function (result)
            {
                var res = JSON.parse(result);
                $scope.ProductListshortname = res;
                $scope.$apply();
            },
            error: function (response)
            {
                toastr["error"]("Product Data Not Found", "Notification");
            }
        });
        _callback();
    }

    //Get the UOM
    function Bind_Packing() {
       // var id = $scope.SelectedCategory;
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/Bind_Packing'            
        }).success(function (data) {
            var packing = JSON.parse(data);
            $scope.Packinglist = packing;
            $scope.PackingChange = 0;
        });
    }
    //Get the stock
    $scope.GetInfo = function (a) {
        //debugger;
        var e = a.target.parentElement.parentElement.parentElement;
        var dummy = $(e.children[2].children[0]).val();
        var Product = $(e.children[2].children[0]).val();
        if (Product != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_Stock_View",
                dataType: 'html',
                data: {
                    pid: Product,
                    oid: $scope.OrderID
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
        else
        {
            message = "Select Product";
            toastr["error"](message, "Notification");
        }
    }
    //Add the new product
    $scope.addnewrow = function (a) {
        var e = a.target;
        var tr = $("#Contactbody").find("tr");
        var txt = "";
        for (var i = 0; i < tr.length; i++) {
            var td = tr[i].cells;
            //var Product = $(td[1]).find("input").val();
            var Product = $(td[1]).find("p").text();
            var Currency = $(td[4]).find("select").val();
            var Packing = $(td[6]).find("select").val();
            var Qty = $(td[5]).find("input").val();
            var Price = $(td[4]).find("input").val();
            if (Product == "0") {
                message = "Select Product at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            } else if (Currency == "0") {
                message = "Select Currency at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Packing == "") {
                message = "Enter the Packing at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Qty == "") {
                message = "Enter the Quantity at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Price == "") {
                message = "Enter the Price at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
        }
        if (txt == "") {
            //length = (tr.length + 1);
            //length = (tr.length + 1);
            length = (tr.length + 1);
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'></p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly /></td><td><input style='text-align:right;' type='test' id='txtprice_" + tr.length + "' class='form-control' onchange='MoneyValidation(this);'/></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + tr.length + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('Contactbody')).append(temp);
            $('.chosen-select').chosen();
            ProductDetails(function () { });
        }
    };


    // Binding the contact details in dynamic table 
    function OrderDetails(e)
    {
        var orderId = parseInt(e);
        //var jsdata = '{id:' + orderId + '}';
        $.ajax({
            type: "GET",
            url: '/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Update_Childtable',
            data: { id: orderId },
            headers: { "Content-Type": "application/json" },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result)
            {
                if (result == "Session Expired") {
                    $window.location.href = '/ET_Login/ET_SessionExpire';
                }
                else if (result.indexOf("ERR") > -1) {
                    $("#spanErrMessage1").html(data);
                    $("#spanErrMessage2").html(data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else {
                    var contactdata = JSON.parse(result);
                    var i = contactdata.length;
                    var j = 0;
                    length = 0;
                    angular.element(document.getElementById('Contactbody')).html("");
                    while (i != 0)
                    {
                        //var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtSerial' value='{{ item.SO_Serial }}' class='form-control' /><p style='display:none;'></p></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                        var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' class='form-control' /></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'></p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                        var temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('Contactbody')).append(temp);
                        $('.chosen-select').chosen();
                        length++;
                        i--;
                        j++;
                    }
                    //First Add the Product Group(PG) to the Article List.
                    ProductDetails(function ()
                    {
                        //alert("Scope Order Data");
                        $scope.orderdata1 = contactdata;
                    });
                }
            },
            error: function (response) {
                $window.alert(response);
            }
        });
        //$http({
        //    method: 'POST',
        //    url: '/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Update_Childtable',
        //    data: {
        //        id: id
        //    }
        //}).success(function (data)
        //{
        //    var contactdata = JSON.parse(data);
        //    var i = contactdata.length;
        //    var j = 0;
        //    length = 0;
        //    angular.element(document.getElementById('Contactbody')).html("");
        //    while (i != 0)
        //    {
        //        //var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtSerial' value='{{ item.SO_Serial }}' class='form-control' /><p style='display:none;'></p></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
        //        var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' class='form-control' /></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'></p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
        //        var temp = $compile(Rowhtml)($scope);
        //        angular.element(document.getElementById('Contactbody')).append(temp);
        //        $('.chosen-select').chosen();
        //        length++;
        //        ProductDetails(function () { });
        //        i--;
        //        j++;
        //    }
        //    $scope.orderdata1 = contactdata;
        //});
    }
    //Delete the product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#Contactbody").find("tr").length;
        if (len != 1)
        {
            $(e).parent().parent().parent().fadeOut(300, function () {
                $(this).remove();
                length = length - 1;
            });
            var tr = $("#Contactbody").find("tr");
            for (var i = 0; i < tr.length; i++)
            {
                var td = tr[i].cells;
                $(td[0]).find("input").val(i + 1);
            }
        }
        else
        {
            message = "Atleast one contact required";
            toastr["error"](message, "Notification");
        }
    }

    //get the sales person
    function Salesperson() {
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/GetSalesPerson',
            data: { id: $scope.OrderID, type: $scope.SelectedOredertype }
        }).success(function (data) {
            
            var person = JSON.parse(data)
            $scope.Salesperson = person;
        });
    }
    //Get the sales group
    $scope.Salesgroup = function () { 
       
        var id = $scope.SelectedSalesperson;
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/SalesOrgBind',
            dataType: 'json',
            data: {
                id: id,
                orgtype: $scope.SelectedOredertype
            },
        }).success(function (data) {
            
            var x = data.split(',');
            $scope.ORGNAMEText = x[0];
            $scope.ORGIDValue = x[1];
        });
    }
    //Order type
    $scope.changeOrdertyper = function ()
    {
        // setTimeout(function () { $("#drpOrdertype").val($scope.SelectedOredertype).trigger("chosen:updated"); }, 100);
        //debugger;
        $scope.isquotation = false;
        if ($scope.SelectedOredertype == "1") {
            $("#commision").removeClass("hidden");
            $("#Sub_name").removeClass("hidden");
            $("#supdetails").removeClass("hidden");
            
           // $("#Quote_No").addClass("hidden");
            $("#Sup_posc").addClass("hidden");
            $scope.isagency = true;
            $scope.isaction = true;
            $scope.istrading = false;
            $scope.isonetomany = false;
            BindQuotation();
        }
        else if ($scope.SelectedOredertype == "2") {
            $("#supdetails").removeClass("hidden");
            $("#Sub_name").addClass("hidden");
            $("#Quote_No").removeClass("hidden");
            $("#Sup_posc").removeClass("hidden");

            $("#commision").addClass("hidden");
            $scope.isagency = false;
            $scope.istrading = true;
            $scope.isaction = true;
            $scope.isonetomany = false;
            BindQuotation();
        }
        else if ($scope.SelectedOredertype == "3") {
            $("#Quote_No").removeClass("hidden");

            $("#supdetails").addClass("hidden");
            $("#Sub_name").addClass("hidden");
            $("#commision").addClass("hidden");
            $scope.isagency = false;
            $scope.istrading = false;
            $scope.isaction = true;
            $scope.isonetomany = true;
            BindQuotation();
        }
        else
        {
            $("#Sub_name").addClass("hidden");
            $("#supdetails").addClass("hidden");
            $("#commision").addClass("hidden");
            $("#Quote_No").addClass("hidden");
            $scope.isagency = false;
            $scope.isaction = true;
            $scope.isonetomany = false;
        }
        //<td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td>
        var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'></p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
        var temp = $compile(Rowhtml)($scope);
        angular.element(document.getElementById('Contactbody')).html("");
        angular.element(document.getElementById('Contactbody')).append(temp);
        $('.chosen-select').chosen();
    }
    //Get the currency
    function Bind_Currency() {
        $http({
            method: 'GET',
            url: '/ET_Admin_GeneralOffer/Bind_Currency',

        }).success(function (result) {
            var res = JSON.parse(result)
            $scope.Currencylist = res;
            $scope.SelectedCusCurrency = "";
        });
    }
    //Set the advance type
    $scope.CustomerAdvancetype = function () {

        setTimeout(function () { $("#drpSusAdvancetype").val($scope.SelectedCusAdvancetype).trigger("chosen:updated"); }, 100);

        if ($scope.SelectedAdvancetype != "1") {
            $("#htnadvancepercentage").addClass("hidden");
            $("#htnadvanceamount").removeClass("hidden");
        }
        else {
            $("#htnadvancepercentage").removeClass("hidden");
            $("#htnadvanceamount").addClass("hidden");
        }
    }

    //$scope.Discounttype = function () {

    //    setTimeout(function () { $("#drpdiscounttype").val($scope.Selecteddiscounttype).trigger("chosen:updated"); }, 100);

    //    if ($scope.Selecteddiscounttype != "1") {
    //        $("#htndiscountpercentage").addClass("hidden");
    //        $("#htndiscountamount").removeClass("hidden");
    //    }
    //    else {
    //        $("#htndiscountpercentage").removeClass("hidden");
    //        $("#htndiscountamount").addClass("hidden");
    //    }
    //}

    $scope.Supadvancetype = function () {

        setTimeout(function () { $("#drpsupAdvancetype").val($scope.SelectedsupAdvancetype).trigger("chosen:updated"); }, 100);

        if ($scope.SelectedsupAdvancetype != "1") {
            $("#htnsupradvancepercentage").addClass("hidden");
            $("#htnsupradvanceamount").removeClass("hidden");
        }
        else {
            $("#htnsupradvancepercentage").removeClass("hidden");
            $("#htnsupradvanceamount").addClass("hidden");
        }
    }

    $scope.Supdiscounttype = function () {

        setTimeout(function () { $("#drpsupdiscounttype").val($scope.Selectedsupdiscounttype).trigger("chosen:updated"); }, 100);

        if ($scope.Selectedsupdiscounttype != "1") {
            $("#htnsupdiscountpercentage").addClass("hidden");
            $("#htnsupdiscountamount").removeClass("hidden");
        }
        else {
            $("#htnsupdiscountpercentage").removeClass("hidden");
            $("#htnsupdiscountamount").addClass("hidden");
        }
    }
    //Get the payment terms
    function CusPaymentTerms() {
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/Payment_terms_dropdown',
            data: { id: $scope.OrderID}
        }).success(function (data) {
            var payment = JSON.parse(data);
            $scope.Cuspayment = payment;
        });
    }
    // Binding the customer only 
    function Customer() {
        var id = $scope.OrderID;
        $http({
            method: 'GET',
            url: '/ET_Sales_OrderDetails/GetCustomerSupplier',
            params: {
                id: id,
                custsup:1
            },
        }).
            success(function (data) {
                var customer = JSON.parse(data)
                $scope.CustomerList = customer;

            });
    }

    // Binding the supplier only
    function Supplier() {
        var id1 = $scope.OrderID;
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/GetCustomerSupplier',
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

    //Gets the catalog list from the database.
    function GetCatalogList(catalogCode) {
        var getCatalogList = $http.get("/ET_Sales_OrderDetails/GetCatlogList",
            {
                params: { catalogId: catalogCode , catalogType:1}
            });
        getCatalogList.then(function (catalogCollection) {
            var res = JSON.parse(catalogCollection.data);
            $scope.catalogList = res;
            //$("#drpCatalogSection").val(QuotationByID.Q_CatalogSelection).trigger("chosen:updated");
        }, function () {
            alert("Catalog List not found");
        });
    }


    //Validate the data
    function validate()
    {
        if ($scope.SelectedSalesperson == "")
        {
            toastr["error"]("Select Sales Person", "Notification");
            return "";
        }
        if ($("#txtorderdate").val() == "") {
            toastr["error"]("Select Order Date", "Notification");
            return "";
        }
        if ($scope.SelectedOredertype == "") {
            toastr["error"]("Select Order Type", "Notification");
            return "";
        }
        if ($scope.SelectedCustomer == "") {
            toastr["error"]("Select Customer", "Notification");
            return "";
        }
        if ($scope.txtcusponumber == "") {
            toastr["error"]("Enter Customer PO", "Notification");
            return "";
        }
        if ($("#cuspodate").val() == "") {
            toastr["error"]("Select Customer PO Date", "Notification");
            return "";
        }
        if ($scope.Q_CatalogSelection == "") {
            message = "Select Product Catalog";
            toastr["error"](message, "Notification");
            return "";
        }

        if ($scope.SelectedOredertype == '1')
        {
            if ($scope.SelectedSupplier == "0")
            {
                toastr["error"]("Select Supplier", "Notification");
                return "";
            }
            if ($scope.txtsubscnumber == "0") {
                toastr["error"]("Select Supplier SC Number", "Notification");
                return "";
            }
            if ($("#subscdate").val() == "") {
                toastr["error"]("Select Supplier SC Date", "Notification");
                return "";
            }
            if ($scope.txtcommisionagy == "") {
                toastr["error"]("Enter Commission", "Notification");
                return "";
            }
            if (parseFloat($scope.txtcommisionagy) == 0) {
                toastr["error"]("Enter Commission", "Notification");
                return "";
            }
        }
        if ($scope.SelectedOredertype == '2') {
            if ($scope.SelectedSupplier == "0") {
                toastr["error"]("Select Supplier", "Notification");
                return "";
            }
        }
        if ($scope.SelectedCusCurrency == "") {
            toastr["error"]("Select Currency", "Notification");
            return "";
        }
        if ($scope.txtCusdeliveryterms == "") {
            toastr["error"]("Enter Delivery Terms", "Notification");
            return "";
        }
        if ($scope.SelectedCuspaymenttype == "") {
            toastr["error"]("Select Payment Terms", "Notification");
            return "";
        }
        if ($scope.SO_Discount == "") {
            $scope.SO_Discount = 0;
            //toastr["error"]("Enter Discount", "Notification");
            //return "";
        }
        var tr = $("#Contactbody").find("tr");
        var txt = "";
        
        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
            //var Shortname = $(td[0]).find("select").val();
            var Shortname = $(td[1]).find("p").text();
            var Price = $(td[5]).find("input").val();
            var Quantity= $(td[6]).find("input").val();
            //var Discount = $(td[7]).find("input").val();
            if (Shortname == "0")
            {
                message = "Choose Shortname at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }   
            else if (Price == "") {
                message = "Price cannot be empty at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(Price.split('.').join("").replace(",", ".")) == "")
            {
                message = "Price cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (Quantity == "") {
                message = "Quantity cannot be empty at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat($scope.OrderID)== 0 && parseFloat(Quantity.split('.').join("").replace(",", ".")) == "") {
                message = "Quantity cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else
            {
                //debugger;
                if ($("#drpOrdertype").val() != "1")
                {
                    if ($(td[6]).find("input").val() == "")
                    {
                        $(td[6]).find("input").val(0);
                    }
                        if ($(td[7]).find("input").val() == "") {
                        $(td[7]).find("input").val(0);
                    }
                }
                else
                {
                    if ($(td[5]).find("input").val() == "")
                    {
                        $(td[5]).find("input").val(0);
                    }
                    if ($(td[6]).find("input").val() == "")
                    {
                        $(td[6]).find("input").val(0);
                    }
                    if ($(td[7]).find("input").val() == "")
                    {
                        $(td[7]).find("input").val(0);
                    }
                }
                txt = txt + $(td[2]).find("input").val() + "}";
                txt = txt + $(td[1]).find("p").text() + "}";
                txt = txt + $(td[3]).find("input").val() + "}";
                txt = txt + $(td[3]).find("p").text() + "}";
                txt = txt + $(td[4]).find("input").val() + "}";
                txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                txt = txt + parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                if ($("#drpOrdertype").val() != "1")
                {
                    txt = txt + parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                }
                else
                {
                    txt = txt + $(td[7]).find("input").val() + "}";
                }
                
                txt = txt + $(td[8]).find("input").val() + "}";
                if($("#drpOrdertype").val() != "1")
                {
                    var remarks = "";
                    var design = "";

                    if ($(td[9]).find("input").val() != null && $(td[9]).find("input").val() != "undefined")
                        remarks = $(td[9]).find("input").val();
                    if ($(td[10]).find("input").val() != null && $(td[10]).find("input").val() != "undefined")
                        design = $(td[10]).find("input").val();
                    
                    txt = txt + remarks + "}";
                    txt = txt + design + "}";
                }
                if ($("#drpOrdertype").val() == "1" && $scope.SelectedQuotationNo == 0)
                {
                    txt = txt + $($(td[10]).find("checkbox").context.lastChild).val() + "|";
                }
                else
                {
                    txt = txt + "0|";
                }
            }
        }
        return txt;
    };
    //Save the order
    $scope.SubmitClick = function () {
        // var Commision = ;
        $("#div_loadImage").css("display", "block");
        if ($scope.txtcommisionagy == "") {
            $scope.txtcommisionagy = 0;
        }
        var txt = validate();
        if (txt != "")
        {
            if ($scope.txtTaxApplicable == null || $scope.txtTaxApplicable == "")
                $scope.txtTaxApplicable = false;
            if ($scope.txtTaxRemarks == null)
                $scope.txtTaxRemarks = '';

            if ($scope.PL_Type == "1")
            {
                productType = 1;
            }
            else if ($scope.PL_Type == "2")
            {
                productType = 4;
            }
            else {
                productType = 0;
            }
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Add",
                dataType: 'json',
                data: {
                    OrderCode: $scope.OrderCode,
                    OrderID: $scope.OrderID,
                    SO_PriceType: $scope.SO_PriceType,
                    drpSalesperson: $scope.SelectedSalesperson,
                    ORGIDValue: $scope.ORGIDValue,
                    drpOrdertype: $scope.SelectedOredertype,
                    txtorderdate: $("#txtorderdate").val(),
                    drpCustomer: $scope.SelectedCustomer,
                    drpSupplier: $scope.SelectedSupplier,
                    txtcusponumber: $scope.txtcusponumber,
                    cuspodate: $("#cuspodate").val(),
                    txtsubscnumber: $scope.txtsubscnumber,
                    subscdate: $("#subscdate").val(),
                    drpCusCurrency: $scope.SelectedCusCurrency,
                    txtcommisionagy: $scope.txtcommisionagy,
                    txtCusdeliveryterms: $scope.txtCusdeliveryterms,
                    drpCusPaymenttype: $scope.SelectedCuspaymenttype,
                    remarks: $scope.remarks,
                    quotation: $scope.SelectedQuotationNo,
                    SO_Discount: $scope.SO_Discount,
                    taxApplicable: $scope.txtTaxApplicable,
                    taxRemarks: $scope.txtTaxRemarks,
                    productType: productType,
                    Q_CatalogId: $scope.Q_CatalogSelection,
                    Orderdetails: txt
                },
                headers: { "Content-Type": "application/json" }
            });

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
                    var res = data.split(":");
                    if ($scope.OrderID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        //toastr["success"](message, "Notification");
                    }
                    else
                    {
                        message = 'Record Updated Successfully With Code : ' + res[1];
                        //toastr["success"](message, "Notification");
                    }
                    //Added the Order Id and Operation Type required 
                    //for Redirection.
                    var resultOrderCode = res[1];
                    var operationType = 1;
                    if ($scope.submittext == "Submit")
                        operationType = 1;
                    else
                        operationType = 2;

                    alertPORedirection($scope.Type,resultOrderCode, operationType);
                    //$scope.RedirectPurchaseOrderPage(res[1],1);
                    //$scope.createnew();
                }
                else {
                    message = "Failed to do this operation.";
                    toastr["error"](message, "Notification");
                }
            });

            post.error(function (data, status)
            {
                //alert("data error");
                $window.alert(data.Message);
                $("#div_loadImage").css("display", "none");
            });

        }
        else
        {
            $("#div_loadImage").css("display", "none");
        }
    }

    function alertPORedirection(scopeType, orderId, recordOperationType) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "allowHtml": true,
            "positionClass": "toast-top-center",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "50000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "show",
            "hideMethod": "fadeOut"
        };
        //toastr['success']("Are you sure you want to delete this record?<br><button style='height:30px;text-align:center;font-size:small' type='button' onClick='RedirectPurchaseOrderPage(\"" + orderId + "\",\"" + recordOperationType + "\");' class='btn dark'>YES</button><button type='button' style='height:30px;text-align:center;font-size:small' class='btn default pull-right'>NO</button>", "Alert");
        toastr['success'](orderId.toString() + " Successfully Created/Updated.Are you sure you want to Redirect to Purchase Order ? ", "<br>Order Creation/Updation</br> <br><button class='btn btn-greensea btn-sm mb-10' style='padding:5px;text-align:center;font-size:small' type='button' id='redirectConfirmYes' class='btn dark' onClick='RedirectPurchaseOrderPage(\"" + scopeType + "\",\"" + orderId + "\",\"" + recordOperationType + "\");' value='yes'>YES</button> &nbsp <button type = 'button' style = 'padding:5px;text-align:center;font-size:small' onclick='RedirectPurchaseOrderCancelled();' class= 'btn btn-red btn-sm mb-10' value='no'>NO</button>", "Alert");
    }

    //$scope.RedirectPurchaseOrderPage = function (orderId, recordOperationType)
    //{
    //    alert('redirect in scope');
    //    message = 'Customer Order Created.Redirecting to Purchase Order creation';
    //    toastr["success"](message, "Notification");
    //    setTimeout(() => {
    //        window.location = '../ET_Purchase_PO/ET_Purchase_PO?type=' + $scope.Type + '&orderID=' + orderId + '&redirect=true' + '&redirectType=' + recordOperationType;
    //    }, 5000);  //1s
    //}

    //Edit the order
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var get = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Update_GetbyID",
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
                    //debugger;
                    $scope.SO_Serial = res[0].SO_Serial;
                    $scope.OrderCode = res[0].SO_Code;
                    $scope.OrderID = res[0].SO_ID;
                    $scope.SelectedSalesperson = res[0].SO_SalesPersonID;
                    $("#drpSalesperson").val($scope.SelectedSalesperson).trigger("chosen:updated");
                    $scope.Salesgroup($scope.SelectedSalesperson);
                    $scope.ORGIDValue = res[0].SO_ORGID;
                    $scope.SelectedOredertype = res[0].SO_OrderType;
                    $scope.SelectedCustomer = res[0].SO_CutomerID;
                    $scope.SelectedSupplier = res[0].SO_SupplierID;
                    $scope.changeOrdertyper($scope.SelectedOredertype);
                    $scope.SO_PriceType = res[0].SO_PriceType;
                    if (res[0].Q_CatalogId == null)
                        res[0].Q_CatalogId = "";

                    $scope.Q_CatalogSelection = res[0].Q_CatalogId;
                    $("#drpCatalogSection").val(res[0].Q_CatalogId).trigger("chosen:updated");
                    $("#drpPriceType").val(res[0].SO_PriceType).trigger("chosen:updated");
                    var Orderdate = new Date(parseInt(res[0].SO_OrderDate.substr(6)));
                    var Orderdt = ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear();
                    $("#txtorderdate").val(Orderdt);

                    $scope.txtcusponumber = res[0].SO_CusPONO;
                    var cuspodate = new Date(parseInt(res[0].SO_CusPODate.substr(6)));
                    var Cuspodt = ("0" + cuspodate.getDate()).slice(-2) + "-" + ("0" + (cuspodate.getMonth() + 1)).slice(-2) + "-" + cuspodate.getFullYear();
                    $("#cuspodate").val(Cuspodt);

                    $scope.txtsubscnumber = res[0].SO_SupSCNO;
                    if (res[0].SO_SupSCDate != null) {
                        var subscdate = new Date(parseInt(res[0].SO_SupSCDate.substr(6)));
                        var Supscdt = ("0" + subscdate.getDate()).slice(-2) + "-" + ("0" + (subscdate.getMonth() + 1)).slice(-2) + "-" + subscdate.getFullYear();
                        $("#subscdate").val(Supscdt);
                    }
                    else {
                        $("#subscdate").val("");
                    }

                    $scope.SelectedCusCurrency = res[0].SO_CusCurrency;
                    $("#drpCusCurrency").val($scope.SelectedCusCurrency).trigger("chosen:updated");

                    $scope.txtcommisionagy = res[0].SO_Commision;
                    $scope.txtCusdeliveryterms = res[0].SO_CusDeliveryTerms;
                    $scope.remarks = res[0].SO_Remarks;

                    $scope.SelectedQuotationNo = res[0].SO_QuotationID;
                    $scope.Q_CatalogSelection = res[0].Q_CatalogId; 
                    var productType = res[0].SO_ProductType;
                    //check the product type
                    var productCategory = "1";

                    if (productType == 1) {
                        productCategory = "1";
                        $scope.PL_Type = "1";
                    }
                    else if (productType == 4) {
                        productCategory = "2";
                        $scope.PL_Type = "2";
                    }
                    else {
                        productCategory = "3";
                        $scope.PL_Type = "3";
                    }

                    $("#drpPackType").val($scope.PL_Type).trigger("chosen:updated");
                    //$("#drpPackType").attr('disabled', "disabled");
                    $scope.SelectedCuspaymenttype = res[0].SO_CusPaymentTermID;

                    $scope.SO_Discount = res[0].SO_Discount.toFixed(0);
                    if (res[0].SO_TaxApplicable == null || res[0].SO_TaxApplicable == "")
                        res[0].SO_TaxApplicable = false;
                    $scope.txtTaxApplicable = res[0].SO_TaxApplicable;
                    $scope.txtTaxRemarks = res[0].SO_TaxRemarks;
                    CusPaymentTerms();
                    Customer();
                    Supplier();
                    Salesperson();
                    GetCatalogList(0);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    if ($scope.SelectedOredertype == '2' || $scope.SelectedOredertype == '3' || $scope.SelectedOredertype == '1') {
                        if ($scope.SelectedQuotationNo != 0) {
                            $scope.QuotationChange();
                        }
                        else {
                            OrderDetails($scope.OrderID);
                        }
                    }
                    if ($scope.SelectedOredertype == "1" && $scope.SelectedQuotationNo == 0) {
                        OrderDetails($scope.OrderID);
                    }
                }
            });

            get.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else
        {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    $scope.CatalogSelection = function ()
    {
        //Retreive the Product Details specific.
        ProductDetails(function () { });

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
            //    ProductDetails(function () { });
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
            //    //ProductDetails(function () { });
            //    //GetProductList();
            //    i--;
            //    j++;
            //}
            $scope.CatalogDetails = contactdata;
        });
    };

    //get the price type
    $scope.PriceTypeChange = function () {
    }

    $scope.PLTypeChange = function ()
    {
        //If the Scope is Create then allow user to edit the Product Articles.
        if ($scope.Iscreate)
        {
            //$("#drpPackType").attr('disabled', false);
            var productCategory = "";
            if ($scope.PL_Type == "1")
                productCategory = "Yarn";
            else if ($scope.PL_Type == "2")
                productCategory = "Fabric";
            else
                productCategory = "Finished Goods";
            message = productCategory + ":" + "Product Articles Available for Order Creation";
            toastr["info"](message, "Notification");
            ProductDetails(function () { });
        }
        else
        {
            //$("#drpPackType").attr('disabled', "disabled");
            //$("#drpPackType").attr('disabled', true);
            var message = "Product category Already saved in Previous Save.Cannot modify the Product Type";
            toastr["info"](message, "Notification");
        }
    }

    // Delete and restore function
    $scope.PerformRestore = function (a, $event, b)
    {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Restore_Delete",
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

                    AC_GetPayment_List();
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

//    $scope.deleteRecordConfirm = function (e, f) {
//        var res = false;
//        if (b)
//            res = $scope.Isdelete;
//        else
//            res = $scope.Isrestore;
//    res = $scope.Isdelete;

//        if (res) {
//            var post = $http({
//                method: "POST",
//                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Restore_Delete",
//                dataType: 'json',
//                data: {
//                    id: a,
//                    type: b
//                },
//                headers: { "Content-Type": "application/json" }
//            });
//            post.success(function (data, status) {
//                if (data == "Session Expired") {
//                    $window.location.href = '/ET_Login/ET_SessionExpire';
//                }
//                else if (data.indexOf("ERR") > -1) {
//                    $("#spanErrMessage1").html(data);
//                    $("#spanErrMessage2").html(data);
//                    if ($("#exceptionmessage").length)
//                        $("#exceptionmessage").trigger("click");
//                }
//                else {
//                    if (data == "Success") {
//                        $($event.target.parentElement.parentElement.parentElement).fadeOut(800, function () { $(this).remove(); });
//                        if (b) {
//                            message = "Record Deleted Successfully.";
//                        }
//                        else {
//                            message = "Record Restored Successfully.";
//                        }

//                        toastr["success"](message, "Notification");

//                        AC_GetPayment_List();
//                    }
//                    else {
//                        message = "Failed to do this operation.";
//                        toastr["error"](message, "Notification");
//                    }
//                }
//            });
//            post.error(function (data, status) {
//                $window.alert(data.Message);
//            });
//        }
//}
    $scope.Restoredeleterecords = function (a, $event, b) {
        if (b)
        {
            alertmessageModified($event, a.toString(), b, 'ET_Sales_OrderDetails', 'ET_Sales_OrderDetails_Restore_Delete');
        }
        else {
            $scope.PerformRestore(a, $event, b);
        }
        //else {
        //message = "You don't access to do this operation, please contact admin.";
        // toastr["error"](message, "Notification");
        //}
    };
    //Redirect To schdule Page
    $scope.RedirectSchdulePage = function (a) {
        window.location = '../ET_Sales_Shedule/ET_Sales_OrdertoSchdule?type=' + $scope.Type + '&orderID=' + a;
    };
    //$scope.RedirectPurchaseOrderPage = function (a,type)
    //{
    //    message = 'Customer Order Created.Redirecting to Purchase Order creation';
    //    toastr["success"](message, "Notification");
    //    setTimeout(() => {
    //        window.location = '../ET_Purchase_PO/ET_Purchase_PO?type=' + $scope.Type + '&orderID=' + a + '&redirect=true' + '&redirectType=' + type;
    //    }, 5000);  //1s
    //};
    // View function in Order details
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_View",
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
    };
    //Print the orders
    $scope.ChangeLanguage = function (lang)
    {
        //debugger;
        var id = 0;
        /*if (lang == "E")
        {
            id = parseInt($("#germanformat").val());
        }
        else
        {
            id = parseInt($("#englishformat").val());
        }*/

        if ($scope.OrderID != null || $scope.OrderID != "")
            id = parseInt($scope.OrderID);
        $scope.PrintRecords(id, lang);
    };

    $scope.PrintRecords = function (a, b)
    {
        a = parseInt(a);
        $scope.OrderID = a;
        var jsdata = '{id:' + a + ',' + 'lang:' + b +'}';

        if ($scope.Isview)
        {
            $.ajax({
                type: "POST",
                url: '/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Print',
                dataType: 'json',
                data: jsdata,
                headers: { "Content-Type": "application/json" },
                contentType: "application/json;charset=utf-8",
                success: function (result) {
                    if (result == "Session Expired") {
                        $window.location.href = '/ET_Login/ET_SessionExpire';
                    }
                    else if (result.indexOf("ERR") > -1) {
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
                },
                error: function (response) {
                    $window.alert(data.Message);
                }
            });

            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Print",
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
                else if (data.indexOf("ERR") > -1)
                {
                    $("#spanErrMessage1").html(data);
                    $("#spanErrMessage2").html(data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else
                {
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
                        url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetailsPrint",
                        dataType: 'html',
                        data: {
                            enqId: a,
                            Lang: b
                        },
                        headers: { "Content-Type": "application/json" }
                    });
                    post1.success(function (data, status) {
                        //debugger;
                        if (data == "Session Expired")
                        {
                            $window.location.href = '/ET_Login/ET_SessionExpire';
                        }
                        else if (data.indexOf("ERR") > -1)
                        {
                            $("#spanErrMessage1").html(data);
                            $("#spanErrMessage2").html(data);
                            if ($("#exceptionmessage").length)
                                $("#exceptionmessage").trigger("click");
                        }
                        else
                        {
                            $("#PAthid").attr("href", "/Sales/PDFList/Order/" + data[0].SO_Code + "/" + data[0].SO_Code + ".pdf ");
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

    $scope.$watch("CatalogDetails", function (value) {
        var val = value || null;
        if (val) {
            var contactdata = $scope.CatalogDetails;
            setTimeout(function () {
                var tr = $("#Contactbody").find("tr");
                var quantity = 0.0;
                var rowTotal = 0.0;
                var total = 0.0;
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    $(td[3]).find("input").val(contactdata[i].LU_Description);
                    $(td[1]).find("select").val(id).trigger("chosen:updated");
                    $(td[2]).find("input").val(id);
                    $(td[1]).find("p").text(contactdata[i].P_ShortName);
                    $(td[4]).find("input").val(contactdata[i].LU_PackingUOM);
                    $(td[9]).find("input").val(contactdata[i].CustomerDef);
                    $(td[8]).find("input").val(contactdata[i].P_Description);
                    $(td[5]).find("input").val(contactdata[i].P_SummerPrice.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                }
            }, 5000);
        }
    });

    //watch for the updtes in the catalog list.
    $scope.$watch("catalogList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCatalogSection").val($scope.Q_CatalogSelection).trigger("chosen:updated"); }, 100);
        }
    });

    //Watch for all binding data
    $scope.$watch("QuotationList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpQuotationNo").val($scope.SelectedQuotationNo).trigger("chosen:updated"); }, 5);
        }
    });
    $scope.$watch("Currencylist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCusCurrency").val($scope.SelectedCusCurrency).trigger("chosen:updated"); }, 5);
        }
    });
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

    $scope.$watch("Salesperson", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesperson").val($scope.SelectedSalesperson).trigger("chosen:updated"); }, 5);
        }
    });

    $scope.$watch("Cuspayment", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCusPaymenttype").val($scope.SelectedCuspaymenttype).trigger("chosen:updated"); }, 5);  
        }
    });
    $scope.$watch("SelectedCuspaymenttype", function (value) {
            setTimeout(function () { $("#drpCusPaymenttype").val($scope.SelectedCuspaymenttype).trigger("chosen:updated"); }, 5);
    });

    $scope.$watch("Packinglist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drppacking").val($scope.SelectedPacking).trigger("chosen:updated"); }, 5);
        }
    });

    $scope.$watch("orderdata1", function (value)
    {
        var val = value || null;
        if (val)
        {
            var contactdata = $scope.orderdata1;
            setTimeout(function ()
            {
                //debugger;
                var tr = $("#Contactbody").find("tr");
                var finalTotalAmount = 0;
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(contactdata[i].SO_Serial);
                    $(td[1]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                    $(td[2]).find("input").val(contactdata[i].PRODUCT_ID);
                    //$("#drpProductshortname").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                    $(td[1]).find("p").text(contactdata[i].SHORT_NAME);
                    $(td[3]).find("input").val(contactdata[i].UOM_NAME);
                    $(td[3]).find("p").text(contactdata[i].ORDER_ID);
                    $(td[4]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                    $(td[5]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[6]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[7]).find("input").val(contactdata[i].DiscountPer.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    if ($scope.SelectedOredertype == 1)
                    {
                        $(td[7]).find("input").val(contactdata[i].OrderDescription);
                    }
                    else {
                        $(td[8]).find("input").val(contactdata[i].OrderDescription);
                    }

                    if ($scope.SelectedOredertype == 1)
                    {
                        $(td[8]).find("input").val(contactdata[i].CustomerDesc);
                    }
                    else
                    {
                        $(td[9]).find("input").val(contactdata[i].CustomerDesc);
                    }

                    if ($scope.SelectedOredertype == 2)
                    {
                        $(td[10]).find("input").val(contactdata[i].DesignDetail);
                    }
                    if (contactdata[i].SUPPLIEROFFER_ID !== 0)
                    {
                        $(td[11]).find("input").prop('checked', true);
                        $(td[11]).find("input").val(contactdata[i].SUPPLIEROFFER_ID);
                    }
                    var unitprice = parseFloat(contactdata[i].PRICE);
                    var quantity = parseFloat(contactdata[i].QUANTITY);
                    var rowtotal = quantity * unitprice;
                    var discountAmt = ((contactdata[i].DiscountPer) / 100) * rowtotal;
                    var netRowTotal = rowtotal - discountAmt;
                    finalTotalAmount = finalTotalAmount + netRowTotal;
                }
                $("#txttotalamount").val(finalTotalAmount.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }, 1000);
        }
    });

    $scope.$watch("orderdata2", function (value)
    {

        var val = value || null;
        if (val) {
            var contactdata = $scope.orderdata2;
            setTimeout(function () {
                var tr = $("#Contactbody").find("tr");
                for (var i = 0; i < tr.length; i++) {
                    
                    var td = tr[i].cells;
                    if ($scope.OrderID !== "0")
                    {
                        $(td[0]).find("input").val(contactdata[i].SO_Serial);
                        $(td[1]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                        $(td[2]).find("input").val(contactdata[i].PRODUCT_ID);
                        $(td[1]).find("p").text(contactdata[i].SHORT_NAME);
                        $(td[3]).find("input").val(contactdata[i].UOM_NAME);
                        $(td[3]).find("p").text(contactdata[i].ORDER_ID);
                        $(td[4]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                        $(td[5]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[6]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[7]).find("input").val(contactdata[i].DiscountPer);
                        if ($scope.SelectedOredertype == 1) {
                            $(td[7]).find("input").val(contactdata[i].OrderDescription);
                        }
                        else {
                            $(td[8]).find("input").val(contactdata[i].OrderDescription);
                        }
                        if ($scope.SelectedOredertype == 1) {
                            $(td[8]).find("input").val(contactdata[i].CustomerDesc);
                        }
                        else {
                            $(td[9]).find("input").val(contactdata[i].CustomerDesc);
                        }
                        if ($scope.SelectedOredertype == 2) {
                            $(td[10]).find("input").val(contactdata[i].DesignDetail);
                        }
                        if (contactdata[i].SUPPLIEROFFER_ID != 0) {
                            $(td[11]).find("input").prop('checked', true);
                            $(td[11]).find("input").val(contactdata[i].SUPPLIEROFFER_ID);
                        }
                    }
                    else {
                        //debugger;
                        $(td[0]).find("input").val(contactdata[i].SO_Serial);
                        $(td[1]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                        $(td[2]).find("input").val(contactdata[i].PRODUCT_ID);
                        $(td[1]).find("p").text(contactdata[i].SHORT_NAME);
                        $(td[3]).find("input").val(contactdata[i].UOM_NAME);
                        $(td[3]).find("p").text(contactdata[i].ORDER_ID);
                        $(td[4]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                        $(td[5]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[6]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[7]).find("input").val(contactdata[i].DiscountPer);
                        if ($scope.SelectedOredertype == 1) {
                            $(td[7]).find("input").val(contactdata[i].OrderDescription);
                        }
                        else {
                            $(td[8]).find("input").val(contactdata[i].OrderDescription);
                        }
                        if ($scope.SelectedOredertype == 1) {
                            $(td[8]).find("input").val(contactdata[i].CustomerDesc);
                        }
                        else {
                            $(td[9]).find("input").val(contactdata[i].CustomerDesc);
                        }
                        if ($scope.SelectedOredertype == 2) {
                            $(td[10]).find("input").val(contactdata[i].DesignDetail);
                        }
                        if (contactdata[i].SUPPLIEROFFER_ID != 0) {
                            $(td[11]).find("input").prop('checked', true);
                            $(td[11]).find("input").val(contactdata[i].SUPPLIEROFFER_ID);
                        }
                        var unitprice = parseFloat(contactdata[i].PRICE);
                        var quantity = parseFloat(contactdata[i].QUANTITY);
                        var amt = quantity * unitprice;
                        var discount = (discountPer / 100) * rowtotal;
                        var netRowTotal = amt - discount;
                        finalTotalAmount = finalTotalAmount + netRowTotal;
                    }
                }
                $("#txttotalamount").val(finalTotalAmount.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }, 1000);
        }
    });

    $scope.$watch("SelectedOredertype", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpOrdertype").val($scope.SelectedOredertype).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.QuotationChange = function () {
        //debugger;
        //if ($scope.SelectedQuotationNo != 0) {
            $scope.isaction = true;
            $scope.isagency = false;
            if ($scope.SelectedOredertype == 2) {
                $scope.istrading = true;
            }
            else {
                $scope.istrading = false;
            }
            $scope.isquotation = true;
        var id = $scope.SelectedQuotationNo;
            $http({
                method: 'POST',
                url: '/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Update_ChildtableQuotation',
                data: {
                    id: id,
                    orderid: $scope.OrderID
                },
            }).success(function (data) {
                //debugger;
                var contactdata = JSON.parse(data);
                $scope.txtCusdeliveryterms = contactdata.QuotationInfo[0].Q_DeliveryTerms;
                $scope.SelectedCuspaymenttype = contactdata.QuotationInfo[0].Q_PaymentTerms;
                $scope.CusPymtchange();
                var i = contactdata.QuotationDetails.length;
                var j = 0;
                angular.element(document.getElementById('Contactbody')).html("");
                while (i != 0)
                {
                    var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'></p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('Contactbody')).append(temp);
                    //$('.chosen-select').chosen();
                    $(".chosen-select").chosen({ width: "95%" });
                    length++;
                    i--;
                    j++;
                }
                ProductDetails(function () { });
                $scope.orderdata2 = contactdata.QuotationDetails;
                });
        //}
        //else
        //{
        //    $scope.isaction = true;
        //    $scope.isagency = true;
        //    if ($scope.SelectedOredertype == 2) {
        //        $scope.istrading = true;
        //    }
        //    else {
        //        $scope.istrading = false;
        //    }
        //    $scope.isquotation = false;
        //    $scope.txtCusdeliveryterms = "";
        //    $scope.SelectedCuspaymenttype = "";
        //    length = length + 1;
        //    var Rowhtml = "<tr><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
        //    var temp = $compile(Rowhtml)($scope);
        //    angular.element(document.getElementById('Contactbody')).html("");
        //    angular.element(document.getElementById('Contactbody')).append(temp);
        //    $('.chosen-select').chosen();
        //    ProductDetails(function () { });
        //}
    }
    $scope.Customerchange = function () {
        
        $scope.SO_QuotationID = "0";
        if ( $scope.SelectedOredertype == '1' || $scope.SelectedOredertype == '2' || $scope.SelectedOredertype == '3') {
            BindQuotation();
        }
       
    }
    $scope.Supplierchange = function () {
    }
    $scope.CurrencyChange = function () {
    }
    $scope.CusCurrencyChange = function () {
    }
    $scope.SupCurrencyChange = function () {
    }
    $scope.PackingChange = function () {
    }
    $scope.SupPymtchange = function () {
    }
    $scope.CusPymtchange = function () {
        //if ($scope.SelectedCuspaymenttype != "") {
        //    var getdiscount = $http.get("/ET_Sales_OrderDetails/GetDiscount",
        //        {
        //            params: { id: $scope.SelectedCuspaymenttype }
        //        });
        //    getdiscount.then(function (result) {
        //        var res = JSON.parse(result.data);
        //        if (res[0].PT_DiscountType == "1") {
        //            $scope.DiscountType = "Percentage";
        //            $scope.SO_Discount = res[0].PT_DiscountPer;
        //        }
        //        else
        //        {
        //            $scope.DiscountType = "Amount";
        //            $scope.SO_Discount = res[0].PT_DiscountAmount.toLocaleString("es-ES", { minimumFractionDigits: 3 });
        //        }
        //    }, function () {
        //        alert("No Data Found");
        //    });
        //}
        //else
        //{
        //    $scope.SO_Discount = "";
        //}
    }
    
    $scope.RedirectOrdertoShipment = function (a) {
        window.location = '../ET_Sales_Despatch/ET_Sales_OrderToDespatch?type=1&OrderId=' + a;
    };

    //Clone the Existing Record
    $scope.CloneRecord = function (a) {
        var post = $http({
            method: "POST",
            url: "/ET_Sales_OrderDetails/ET_Sales_CloneOrder",
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
    $scope.MailSentForOrder = function (a) 
    {
        //debugger;
        $("#div_loadImage").css("display", "block");
        /*var id1 = 0;
        $.ajax({
            type: "GET",
            url: '/ET_Sales_OrderDetails/ET_Sales_Mail_Compose_Reply_Forward',
            data: {
                id: a
            },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result)
            {
                //alert("Compose Mail Function success" + result);
                //debugger;
                if (result == true) {
                    $("#div_loadImage").css("display", "none");
                    toastr["success"]("Maill Sent Successfully", "Notification");
                  
                }
                else if (res.indexOf("ERR") > -1) {
                    $("#div_loadImage").css("display", "none");
                    toastr["error"]("Mail not sent.Some Error Occured:" + res, "Notification");
                }
            },
            error: function (response) {
                $("#div_loadImage").css("display", "none");
                toastr["error"]("Product Data Not Found", "Notification");
            }
        });*/
        var post = $http({
            method: "POST",
            url: "/ET_Sales_OrderDetails/ET_Mail_View",
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
                $("#div_loadImage").css("display", "none");
                $('#viewpopup').html(data);
                $("#btnviewpopup").trigger("click");
            }
        });

        post.error(function (data, status) {
            $("#div_loadImage").css("display", "none");
            $window.alert(data.Message);
        });
    };

    $scope.$watch("ProductListshortname", function (value) {
        var val = value || null;
        if (val) {
            //debugger;
            setTimeout(function () { $("#drpProductshortname" + length).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("orderlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
               dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });
    $scope.$watch("orderRestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
   
});