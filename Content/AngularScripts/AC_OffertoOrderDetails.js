app1.controller("AC_OffertoOrderdetailsCtrl", function ($scope, $http, $window, $compile)
{
    function GetTypeFromURL() {
        debugger;
        var type1 = window.location.href.toString().split("&");
        var offerId = window.location.href.toString().split("offerId=");
        $scope.OfferID = offerId[1];
        var type = type1[0].split("type=");
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
    var productlength = 1;
    var productData = {};
    GetTypeFromURL();
    ProductDetails();
    GetPrivilages();
    Bind_Currency();
    Bind_Packing();
    
    //Get Offer Details
    function GetOfferDetails(id)
    {
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/ET_Sales_OfferDetails',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var contactdata = JSON.parse(data)
            $scope.SelectedSupplier = contactdata.OfferInfo[0].COM_ID;
            $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated");
            var i = contactdata.OfferDetails.length;
            var j = 0;
            angular.element(document.getElementById('Contactbody')).html("");
            while (i != 0) {
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + (j + 1) + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('Contactbody')).append(temp);
                i--;
                j++;
            }
            $scope.offerData = contactdata.OfferDetails;
        });
    }
    //View the orders
    $scope.showRecords = function () {
        
        window.location = '../ET_Admin_GeneralOffer/ET_Admin_GeneralOffer?type=' + $scope.Type;
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
            if ($scope.Iscreate) {
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
                $("#drpPriceType").val("1").trigger("chosen:updated");
                $scope.SelectedCuspaymenttype = "";
                Customer();
                Supplier();
                CusPaymentTerms();
                Salesperson();
                $scope.changeOrdertyper();
                GetOfferDetails($scope.OfferID);  
                //var Rowhtml = "<tr><td><select class='form-control chosen-select' id='drpProductshortname' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                //var temp = $compile(Rowhtml)($scope);
                //angular.element(document.getElementById('Contactbody')).html("");
                //angular.element(document.getElementById('Contactbody')).append(temp);
            }
        }, function () {
            alert('Privilages Not Found');
        }
        );
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
        else {
            $scope.QuotationList = {};
            $scope.SelectedQuotationNo = 0;
        }
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
            $("#drpPriceType").val("1").trigger("chosen:updated");
            $scope.SelectedCuspaymenttype = "";
            Customer();
            Supplier();
            CusPaymentTerms();
            Salesperson();
            $scope.changeOrdertyper();
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='1' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('Contactbody')).html("");
            angular.element(document.getElementById('Contactbody')).append(temp);
            $('.chosen-select').chosen();
            ProductDetails();
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Get the product details
    function ProductDetails() {
        var getproductlist = $http.get("/ET_Sales_OrderDetails/GetProducts",
            {
                params: { id: 0 }
            });
        getproductlist.then(function (productlist) {
            var res = JSON.parse(productlist.data);
            $scope.ProductListshortname = res;
             
        }, function () {
            alert('Data not found');
        });
    }
    //Get the UOM
    function Bind_Packing() {
        // var id = $scope.SelectedCategory;
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/Bind_Packing',
        }).success(function (data) {
            var packing = JSON.parse(data)
            $scope.Packinglist = packing;
            $scope.PackingChange = 0;
        });
    }
    //Get the stock
    $scope.GetInfo = function (a) {
        debugger;
        var e = a.target.parentElement.parentElement.parentElement;
        var Product = $(e.children[0].children[0]).val();
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
        else {
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
            // var Product = $(td[0]).find("select").val();
            var Product = $(td[1]).find("p").text();
            var Currency = $(td[5]).find("select").val();
            var Packing = $(td[6]).find("select").val();
            var Qty = $(td[4]).find("input").val();
            var Price = $(td[3]).find("input").val();
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
            length = (length + 1);
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly /></td><td><input style='text-align:right;' type='test' id='txtprice_" + tr.length + "' class='form-control' onchange='MoneyValidation(this);'/></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + tr.length + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('Contactbody')).append(temp);
            $('.chosen-select').chosen();
            ProductDetails();
        }
    }


    // Binding the contact details in dynamic table 
    function OrderDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Update_Childtable',
            data: {
                id: id,
            },
        }).success(function (data) {
            var contactdata = JSON.parse(data);
            var i = contactdata.length;
            var j = 0;
            length = 1;
            angular.element(document.getElementById('Contactbody')).html("");
            while (i != 0) {
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('Contactbody')).append(temp);
                $('.chosen-select').chosen();
                length++;
                ProductDetails();
                i--;
                j++;
            }
            $scope.orderdata1 = contactdata;
        });
    }
    //Delete the product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#Contactbody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
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
    $scope.changeOrdertyper = function () {
        // setTimeout(function () { $("#drpOrdertype").val($scope.SelectedOredertype).trigger("chosen:updated"); }, 100);
        debugger;
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
        else {
            $("#Sub_name").addClass("hidden");
            $("#supdetails").addClass("hidden");
            $("#commision").addClass("hidden");
            $("#Quote_No").addClass("hidden");
            $scope.isagency = false;
            $scope.isaction = true;
            $scope.isonetomany = false;
        }
        var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='1' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
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
            data: { id: $scope.OrderID }
        }).success(function (data) {
            var payment = JSON.parse(data)
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
    //Validate the data
    function validate() {

        if ($scope.SelectedSalesperson == "") {
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
        if ($scope.SelectedOredertype == '1') {
            if ($scope.SelectedSupplier == "0") {
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
        for (i = 0; i < tr.length; i++) {
            var td = $(tr[i]).find("td");
            //var Shortname = $(td[0]).find("select").val();
            var Shortname = $(td[0]).find("p").text();
            var Price = $(td[3]).find("input").val();
            var Quantity = $(td[4]).find("input").val();
            var Discount = $(td[5]).find("input").val();
            if (Shortname == "0") {
                message = "Choose Shortname at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (Price == "") {
                message = "Price cannot be empty at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat(Price.split('.').join("").replace(",", ".")) == "") {
                message = "Price cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (Quantity == "") {
                message = "Quantity cannot be empty at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (parseFloat($scope.OrderID) == 0 && parseFloat(Quantity.split('.').join("").replace(",", ".")) == "") {
                message = "Quantity cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                debugger;
                if ($(td[5]).find("input").val() == "") {
                    $(td[5]).find("input").val(0);
                }
                txt = txt + $(td[0]).find("p").text() + "}";
                txt = txt + $(td[1]).find("input").val() + "}";
                txt = txt + $(td[1]).find("p").text() + "}";
                txt = txt + $(td[2]).find("input").val() + "}";
                txt = txt + parseFloat($(td[3]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                txt = txt + parseFloat($(td[4]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                txt = txt + $(td[5]).find("input").val() + "}";
                txt = txt + $(td[6]).find("input").val() + "}";
                txt = txt + $(td[7]).find("input").val() + "}";
                txt = txt + $(td[8]).find("input").val() + "}";
                if ($("#drpOrdertype").val() == "1" && $scope.SelectedQuotationNo == 0) {
                    txt = txt + $($(td[8]).find("checkbox").context.lastChild).val() + "|";
                }
                else {
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
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_Add",
                dataType: 'json',
                data: {
                    OrderCode: $scope.OrderCode,
                    OrderID: $scope.OrderID,
                    drpSalesperson: $scope.SelectedSalesperson,
                    ORGIDValue: $scope.ORGIDValue,
                    drpOrdertype: $scope.SelectedOredertype,
                    txtorderdate: $("#txtorderdate").val(),
                    drpCustomer: $scope.SelectedCustomer,
                    drpSupplier: $scope.SelectedSupplier,
                    SO_PriceType: $scope.SO_PriceType,
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
                    Orderdetails: txt
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
                    if ($scope.OrderID == 0) {
                        message = 'Record Inserted Successfully With Code : ' + res[1];
                        toastr["success"](message, "Notification");
                        setTimeout(function () {
                            window.location = '../ET_Admin_GeneralOffer/ET_Admin_GeneralOffer?type=' + $scope.Type;
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

    //get the price type
    $scope.PriceTypeChange = function () {
    }

    //Watch for all binding data
    $scope.$watch("ProductListshortname", function (value) {
        var val = value || null;
        if (val) {
            debugger;
            setTimeout(function () { $("#drpProductshortname" + length).trigger("chosen:updated"); }, 100);
        }
    });
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

    $scope.$watch("orderdata1", function (value) {
        var val = value || null;
        if (val) {
            var contactdata = $scope.orderdata1;
            setTimeout(function () {
                debugger;
                var tr = $("#Contactbody").find("tr");
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    //$(td[0]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                    //$(td[0]).find("p").text(contactdata[i].PRODUCT_ID);
                    //$(td[1]).find("input").val(contactdata[i].UOM_NAME);
                    //$(td[1]).find("p").text(contactdata[i].ORDER_ID);
                    //$(td[2]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                    //$(td[3]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    //$(td[4]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    //$(td[5]).find("input").val(contactdata[i].DiscountPer);
                    $(td[0]).find("input").val(i + 1);
                    $(td[1]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                    $(td[2]).find("input").val(contactdata[i].PRODUCT_ID);
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
            }, 5);
        }
    });
    
    $scope.$watch("offerData", function (value) {
        var val = value || null;
        if (val) {
            var contactdata = $scope.offerData;
            setTimeout(function () {
                var tr = $("#Contactbody").find("tr");
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    if ($scope.OrderID != "0")
                    {
                        $(td[0]).find("input").val(i + 1);
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
                        $(td[0]).find("input").val(i + 1);
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
                        if (contactdata[i].SUPPLIEROFFER_ID != 0)
                        {
                            $(td[11]).find("input").prop('checked', true);
                            $(td[11]).find("input").val(contactdata[i].SUPPLIEROFFER_ID);
                        }
                    }
                }
            }, 5);
        }
    });
    $scope.$watch("orderdata2", function (value) {
        var val = value || null;
        if (val) {
            var contactdata = $scope.orderdata2;
            setTimeout(function () {
                var tr = $("#Contactbody").find("tr");
                for (var i = 0; i < tr.length; i++) {
                    var td = tr[i].cells;
                    if ($scope.OrderID != "0") {
                        $(td[0]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                        $(td[1]).find("input").val(contactdata[i].UOM_NAME);
                        $(td[2]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                        $(td[3]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[4]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[5]).find("input").val(contactdata[i].OrderDescription);
                    }
                    else {
                        $(td[0]).find("select").val(contactdata[i].PRODUCT_ID).trigger("chosen:updated");
                        $(td[1]).find("input").val(contactdata[i].UOM_NAME);
                        $(td[2]).find("input").val(contactdata[i].P_PackingQuantityUOM);
                        $(td[3]).find("input").val(contactdata[i].PRICE.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[4]).find("input").val(contactdata[i].QUANTITY.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[5]).find("input").val(contactdata[i].OrderDescription);
                    }
                }
            }, 5);
        }
    });

    $scope.$watch("SelectedOredertype", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpOrdertype").val($scope.SelectedOredertype).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.QuotationChange = function () {
        debugger;
        if ($scope.SelectedQuotationNo != 0) {
            $scope.isaction = false;
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
                debugger;
                var contactdata = JSON.parse(data);
                $scope.txtCusdeliveryterms = contactdata.QuotationInfo[0].Q_DeliveryTerms;
                $scope.SelectedCuspaymenttype = contactdata.QuotationInfo[0].Q_PaymentTerms;
                $scope.CusPymtchange();
                var i = contactdata.QuotationDetails.length;
                var j = 0;
                angular.element(document.getElementById('Contactbody')).html("");
                while (i != 0) {
                    var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + (j + 1) + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);' disabled><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName}}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control'/></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('Contactbody')).append(temp);
                    $('.chosen-select').chosen();
                    length++;
                    ProductDetails();
                    i--;
                    j++;
                }
                $scope.orderdata2 = contactdata.QuotationDetails;
            });
        }
        else {
            $scope.isaction = true;
            $scope.isagency = true;
            if ($scope.SelectedOredertype == 2) {
                $scope.istrading = true;
            }
            else {
                $scope.istrading = false;
            }
            $scope.isquotation = false;
            $scope.txtCusdeliveryterms = "";
            $scope.SelectedCuspaymenttype = "";
            length = length + 1;
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + length + "' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input type='text' id='txtPID' class='form-control' style='display:none' /></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + 0 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + 0 + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('Contactbody')).html("");
            angular.element(document.getElementById('Contactbody')).append(temp);
            $('.chosen-select').chosen();
            ProductDetails();
        }
    }
    $scope.Customerchange = function () {

        $scope.SO_QuotationID = "0";
        if ($scope.SelectedOredertype == '1' || $scope.SelectedOredertype == '2' || $scope.SelectedOredertype == '3') {
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
  

});