app1.controller("AC_ProductCatalogCtrl", function ($scope, $http, $window, $compile)
{
    //$('#advancedUsageTable').dataTable().fnFilter('');

    //setTimeout(function () {
    //    dynamicDataTable('#advancedUsageTable', '#tableTools1', '#colVis1');

    //}, 5);

    GetPrivilages();
    //GetProductList();
    GetProductCatalog();
    GetUOM();
    $scope.ProductListCollection = [];
    $scope.ProductSubGroupCollection = [];
    $scope.ProductSelectorId = [];
    
    // Ger privilages
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Admin_SystemSetUp/GetPrivilages");
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
    //Get the product catalog list
    function GetProductCatalog() {
        var GetProductCatalogList = $http.get("/ET_Admin_ProductCatalog/GetProductCatalogList",
            {
                params: { deleted: false }
            });
        GetProductCatalogList.then(function (result)
        {
            $scope.ProductCatalogLists = result.data;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the product deleted 
    function GetProductCatalogrestoreList() {
        var restoreList = $http.get("/ET_Admin_ProductCatalog/GetProductCatalogRestoreList",
            {
                params: { deleted: false }
            });
        restoreList.then(function (result) {
            debugger;
            $scope.ProductCatalogRestoreLists = result.data;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the product list
    function GetProductList()
    {
        var getProductllist = $http.get("/ET_Admin_ProductCatalog/GetProductList");
        getProductllist.then(function (product)
        {
            
            $scope.products = product.data;
        }, function () {
            alert('Data not found');
        });
    }
    //Get the UOM list
    function GetUOM() {
        var getUOMllist = $http.get("/ET_Admin_ProductCatalog/GetUOMList");
        getUOMllist.then(function (UOM) {
            $scope.UOMS = UOM.data;
        }, function () {
            alert('Data not found');
        });
    }
    //Get the product details
    $scope.GetProduct_Details = function () {
        a = $scope.SelectedProduct;
        var get = $http({
            method: "POST",
            url: "/ET_Admin_ProductCatalog/GetProductDetails",
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
                $scope.txtProductCode = res.P_ArticleNo;
                $scope.txtProductSpecification = res.P_Description;
                $scope.SelectedUOM = res.P_UOM;


                $("#drpUOM").val(res.P_UOM).trigger("chosen:updated");
                $("#div_Edit").css("display", "block");
                //  $scope.submittext = "Update";
                $scope.Edit = "Edit";
                $("#span_tabtext").html("Edit");
            }
        });

        get.error(function (data, status) {
            $window.alert(data.Message);
        });
    };

    //Watch for all data binding
    $scope.$watch("products", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpProduct").val($scope.SelectedProduct).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("UOMS", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpUOM").val($scope.SelectedUOM).trigger("chosen:updated"); }, 100);
        }
    });
    //View the records
    $scope.showRecords = function ()
    {
        $("#advancedusage").dataTable().fnDestroy();
        $("#advancedUsageTable").dataTable().fnDestroy();
        //GetProductCatalog();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    //View the deleted records
    $scope.restoreRecords = function () {
        GetProductCatalogrestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    };
    // Create function
    $scope.createnew = function () {
        if ($scope.Iscreate)
        {
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#formSubmit").attr('disabled', "disabled");
            $("#htndiscountpersentage").addClass("hidden");
            $("#htndiscountamount").addClass("hidden");
            $("#htnadvancetype").addClass("hidden");

            $scope.PriceBookId = 0;
            $scope.PC_MasterId = 0;
            $scope.SelectedProduct = "";
            $("#drpProduct").val("").trigger("chosen:updated");
            $scope.txtProductCode = "";
            $scope.txtPriceBookCode = "";
            $scope.txtProductSpecification = "";
            $scope.SelectedUOM = "";
            //$scope.PL_Catalog_Type = "0";
            $("#drpCatalogType").val("").trigger("chosen:updated");
            $("#drpUOM").val("").trigger("chosen:updated");
            $scope.txtUnitPrice1 = "";
            $scope.txtUnitPrice2 = "";
            $scope.txtUnitPrice3 = "";
            $scope.txtUnitPrice4 = "";
            $("#txtEnqDate").val("");
            $("#drpCatalogType").val("0").trigger("chosen.updated");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.catalog_name = "";
            $scope.catalog_description = "";
            productIds = [];
            length = 0;
            $scope.PL_Catalog_Type = "1";
            //Add new row element.
            angular.element(document.getElementById('catalogBody')).html("");
            var Rowhtml = "<tr><td width='10%'><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td width='10%'><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductGroupListShort" + length + "' onchange='PGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductGroupListShort' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+st.PG_NAME }}</option></select></td><td width='10%'><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductSubGroupShort" + length + "' onchange='SubPGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductSubGroupCollection[" + length + "]' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+ st.PG_NAME }}</option></select></td><td><p style='display:none;'></p><select class='form-control chosen-select' data-live-search='true' data-show-subtext='true' id='drpProductshortname" + 1 + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListCollection[" + length + "]' value='{{ st.P_ID }}'>{{ st.P_ArticleNo+'-'+st.P_ShortName }}</option></select></td><td style='display:none;'><input id='txtProductCode' class='form-control' /></td><td><input id='txtUOM' class='form-control' /></td><td><input id='txtDescription' class='form-control' /></td><td><input type='test' id='txtSummerPrice_" + 1 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtWinterPrice_" + 1 + "' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            $('.chosen-select').chosen();
            $("#catalogBody").append(temp);
            //ProductDetails();
            GetProductGroupDetails(function ()
            {
                //var dynaTable = $('#advancedUsageTable').DataTable({
                //    "paging": false,
                //    "ordering": false,
                //    "info": false,
                //    "stateSave": true,
                //    "orderCellsTop": true,
                //    "fixedHeader": true,
                //    "searching": true,
                //});
                //$('#advancedUsageTable thead tr').clone(true).appendTo('#advancedUsageTable thead');
                //$('#advancedUsageTable thead tr:eq(1) th').each(function (i) {
                //    var title = $(this).text();
                //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');

                //    $('input', this).on('keyup change', function () {
                //        if (dynaTable.column(i).search() !== this.value) {
                //            dynaTable
                //                .column(i)
                //                .search(this.value)
                //                .draw();
                //        }
                //    });
                //});
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    // Insert and Update function
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var date = $("#txtEnqDate").val();
        var txt = validate();
        if (txt != "") {
            var post = $http(
                {
                    method: "POST",
                    url: "/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Add",
                    dataType: 'json',
                    data:
                    {
                        ProductMasterId: $scope.PC_MasterId,
                        PriceBookId: $scope.PriceBookId,
                        PriceBookCode: $scope.txtPriceBookCode,
                        ProductCatalogCode: $scope.txtPriceBookCode,
                        ProductCatalogName: $scope.catalog_name,
                        ProductCatalogDes: $scope.catalog_description,
                        ValidityDate: date,
                        catalogStringType: $scope.PL_Catalog_Type,
                        articleDetails: txt
                    },
                    headers: { "Content-Type": "application/json" }
                });

            post.success(function (data, status)
            {
                $("#div_loadImage").css("display", "none");
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
                else if (data.indexOf("Validation") > -1) {
                    toastr["error"](data, "Notification");
                }
                else if (data.indexOf("Success") > -1)
                {
                    var res = data.split(':');
                    if ($scope.PriceBookId == 0)
                    {
                        message = 'Record Inserted Successfully With Code : ' + res[1].toString();
                        toastr["success"](message, "Notification");
                        //$scope.createnew();
                        setTimeout(() => {
                            location.reload();
                            $scope.showRecords();
                        }, 5000); 
                    }
                    else {
                        message = 'Record Updated Successfully With Code : ' + res[1].toString();
                        toastr["success"](message, "Notification");
                        $scope.createnew();
                        setTimeout(() => {
                            location.reload();
                            $scope.showRecords();
                        }, 5000); 

                        //setTimeout(function ()
                        //{
                        //    $scope.showRecords();
                        //}, 2000);
                    }
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
    };

    //Validate the data
    function validate()
    {
        //if ($scope.txtPriceBookCode == "") {
        //    toastr["error"]("Select Price Book Code", "Notification");
        //    return "";
        //}
        if ($scope.catalog_name == "") {
            toastr["error"]("Catalog Name is empty", "Notification");
            return "";
        }
        if ($scope.catalog_description == "") {
            toastr["error"]("Catalog Description is Empty", "Notification");
            return "";
        }
        if ($scope.PL_Type == "") {
            toastr["error"]("Select Product Category", "Notification");
            return "";
        }
      //  var check = $('#advancedUsageTable').find('input[type=checkbox]:checked').length;
       // if (check <= 0)
       // {
         //   toastr["error"]("Select Atleast one Product Article", "Notification");
           // return "";
            //toastr["info"]("Selected Product Articles will be Submitted for Catalog", "Notification");
        //}
        var tr = $("#catalogBody").find("tr");
        var txt = "";

        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
            //var articleSelected = $(td[0]).find('input[type="checkbox"]').is(':checked');
            //if (articleSelected)
            //{
                var Shortname = $(td[3]).find("p").text();
                var summerPrice = $(td[7]).find("input").val();
                var winterPrice = $(td[8]).find("input").val();
                if (Shortname == "0") {
                    message = "Choose Shortname at row : " + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else if (summerPrice == "") {
                    message = "Summer Price cannot be empty at row : " + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else if (parseFloat(summerPrice.split('.').join("").replace(",", ".")) == "") {
                    message = "Summer Price cannot be 0 at row : " + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else if (winterPrice == "") {
                    message = "Winter Price cannot be 0 at row : " + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else if (parseFloat(winterPrice.split('.').join("").replace(",", ".")) == "") {
                    message = "Winter Price cannot be 0 at row : " + (i + 1);
                    toastr["error"](message, "Notification");
                    return "";
                }
                else {
                    //debugger;
                    txt = txt + $(td[1]).find("select").val() + "}";
                    txt = txt + $(td[2]).find("select").val() + "}";
                    txt = txt + $(td[3]).find("select").val() + "}";
                    txt = txt + $(td[4]).find("input").val() + "}";
                    txt = txt + $(td[5]).find("input").val() + "}";
                    txt = txt + parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                    txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + "}";
                    txt = txt + "0|";
                }
            //}
        }
        return txt;
    };

    // Edit function
    $scope.editRecords = function (a)
    {
        //Initialize the Product List collection and Sub Group Collection.
        $scope.ProductListCollection = [];
        $scope.ProductSubGroupCollection = [];
        $scope.ProductSelectorId = [];
        a = parseInt(a);
        if ($scope.Isedit) {
            var get = $http({
                method: "POST",
                url: "/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Update_GetbyID",
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
                    debugger;

                    $scope.PC_MasterId = res.PC_MasterID;
                    $scope.PriceBookId = res.PC_MasterID;
                    $scope.txtPriceBookCode = res.PC_CatalogueCode;
                    $scope.catalog_name = res.PC_Name;
                    $scope.catalog_description = res.PC_Description;
                    $scope.PL_Type = res.PC_ProductCategory;
                    // $("#txtEnqDate").val(res.LAST_PRICE_REWISE_DATE);
                    var validityDate = "";
                    if (res.VALIDITY_DATE == null || res.VALIDITY_DATE == "")
                        validityDate = new Date().toDateString();
                    else
                        validityDate = res.VALIDITY_DATE;

                    var Orderdate = new Date(parseInt(validityDate.substr(6)));
                    var Orderdt = ("0" + Orderdate.getDate()).slice(-2) + "-" + ("0" + (Orderdate.getMonth() + 1)).slice(-2) + "-" + Orderdate.getFullYear();
                    $("#txtEnqDate").val(Orderdt);
                    var productType = res.PC_ProductCategory;
                    if (res.PC_CatalogType == 1)
                        $scope.PL_Catalog_Type = "1";
                    else
                        $scope.PL_Catalog_Type = "2";
                    $("#drpCatalogType").val(res.PC_CatalogType).trigger("chosen:updated");
                    //check the product type
                    var productCategory = "1";
                    //if (productType == 1)
                    //{
                    //    productCategory = "1";
                    //    $scope.PL_Type = "1";
                    //}
                    //else if (productType == 4) {
                    //    productCategory = "2";
                    //    $scope.PL_Type = "2";
                    //}
                    //else
                    //{
                    //    productCategory = "3";
                    //    $scope.PL_Type = "3";
                    //}

                    //$("#drpPackType").val($scope.PL_Type).trigger("chosen:updated");
                    //$("#drpUOM").val(res.UOM).trigger("chosen:updated");
                    //$("#drpProduct").val(res.PRODUCT_ID).trigger("chosen:updated");
                    //$scope.SelectedProduct = res.PRODUCT_ID;
                    //$scope.txtUnitPrice1 = parseFloat(res.UNIT_PRICE1).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //$scope.txtUnitPrice2 = parseFloat(res.UNIT_PRICE2).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //$scope.txtUnitPrice3 = parseFloat(res.UNIT_PRICE3).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //$scope.txtUnitPrice4 = parseFloat(res.UNIT_PRICE4).toLocaleString("es-ES", { minimumFractionDigits: 2 });
                    //GetCurrentProductDetails();
                    //GetProductGroupDetails();
                    $scope.GetProductCatalogDetails(res.PC_CatalogueCode);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                }
            });

            get.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    $scope.GetProductCatalogDetails = function (e) {
        // Binding the contact details in dynamic table 
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Details_UpdateChildtable',
            data: {
                id: id
            }
        }).success(function (data)
        {
            var contactdata = JSON.parse(data);
            var i = contactdata.length;
            var j = 0;
            length = 0;
            angular.element(document.getElementById('catalogBody')).html("");
            while (i != 0)
            {
                //var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtSerial' value='{{ item.SO_Serial }}' class='form-control' /><p style='display:none;'></p></td><td><P style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListshortname' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input readonly='readonly' type='text' id='txtUOM' class='form-control' /><p style='display:none;'>test</p></td><td><input type='text' id='txtPackingUOM' class='form-control' readonly/></td><td><input type='test' id='txtprice_" + j + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtqty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td ng-if='istrading == true || isonetomany == true'><input type='text' id='txtDiscount' style='text-align:right;' onchange='DiscountValidation(this);' maxlength='21' class='form-control' /></td><td><input type='text' id='txtdescription' class='form-control'/></td><td><input type='text' id='txtRemarks' class='form-control'/></td><td ng-if='istrading == true'><input type='text' id='txtDesign' class='form-control'/></td><td ng-if='isaction == true'><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td><td ng-if='isagency == true'><input  type='checkbox' id='chkoffer_" + j + "' value='0' onchange='checkoffer(this);' /></td><td ng-if='isonetomany == true'><a style='padding: 5px;' ng-click='GetInfo($event)'><i class='fa fa-info'></i></a></td></tr>";
                //<td><p style='display:none;'></p><input readonly='readonly' type='text' id='txtLastUpdated' class='form-control' /></td>;
                //<td><input type='checkbox' name='select_all' value=' " + j + "' id='product-select-all'></td>
                var Rowhtml = "<tr><td width='10%'><input readonly='readonly' type='number' value = '" + (length + 1) + "' id='txtSerial' class='form-control' /></td><td width='10%'><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductGroupListShort" + length + "' onchange='PGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductGroupListShort' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+st.PG_NAME }}</option></select></td><td width='10%'><p style='display:none;'></p><select  class='form-control chosen-select' id='drpProductSubGroupShort" + length + "' onchange='SubPGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductSubGroupCollection[" + length + "]' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+ st.PG_NAME }}</option></select></td><td><p style='display:none;'></p><select class='form-control chosen-select' data-live-search='true' data-show-subtext='true' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListCollection[" + length + "]' value='{{ st.P_ID }}'>{{ st.P_ArticleNo+'-'+st.P_ShortName}}</option></select></td><td style='display:none;'><input id='txtProductCode' class='form-control' /></td><td><input id='txtUOM' class='form-control' /></td><td><input id='txtDescription' class='form-control' /></td><td><input type='test' id='txtSummerPrice_" + i + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtWinterPrice_" + i + "' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                $('.chosen-select').chosen();
                length++;
                $("#catalogBody").append(temp);
                //GetCurrentProductDetails();
                i--;
                j++;
            }
            //$('#advancedUsageTable').DataTable().ajax.reload();

            //First Add the Product Group(PG) to the Article List.
            GetProductGroupDetails(function ()
            {
                $scope.AssignCatalogDataScope(contactdata);
            });
        });
    };

    $scope.PGSubGroupModified = function () {
    };

    $scope.AssignCatalogDataScope = function (contactdata)
    {
        //Update the Catalog Details, so that the Watch operation will be initiated.
        $scope.CatalogDetails = contactdata;
    };

    //Add the new productssss
    $scope.addnewrow = function (a)
    {
        var e = a.target;
        var tr = $("#catalogBody").find("tr");
        var txt = "";
        productIds = [];
        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
            //var articleSelected = $(td[0]).find('input[type="checkbox"]').is(':checked');
            //if (articleSelected) {
            var productGroupSelected = $(td[1]).find("select").val();
            var subGroupSelected = $(td[2]).find("select").val();
            var productSelected = $(td[3]).find("select").val();
            productIds[i] = productSelected;
            var Shortname = $(td[4]).find("input").val();
            var summerPrice = $(td[7]).find("input").val();
            var winterPrice = $(td[8]).find("input").val();
            if (productGroupSelected == 0)
            {
                message = "Choose Product Category at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            if (productSelected == 0)
            {
                message = "Choose Product at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            if (Shortname == "0")
            {
                message = "Choose Shortname at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            else if (summerPrice == "") {
                message = "Summer Price cannot be empty at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            else if (parseFloat(summerPrice.split('.').join("").replace(",", ".")) == "") {
                message = "Summer Price cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            else if (winterPrice == "") {
                message = "Winter Price cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            else if (parseFloat(winterPrice.split('.').join("").replace(",", ".")) == "") {
                message = "Winter Price cannot be 0 at row : " + (i + 1);
                toastr["error"](message, "Notification");
                txt = message;
                return "";
            }
            //}
        }

        if (txt == "")
        {
            //length = (tr.length + 1);
            //length = (tr.length + 1);
            length = (tr.length + 1);
            var Rowhtml = "<tr><td width='10%'><input readonly='readonly' type='number'value='" + length + "' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductGroupListShort" + length + "' onchange='PGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductGroupListShort' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+st.PG_NAME }}</option></select></td><td width='5%'><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductSubGroupShort" + length + "' onchange='SubPGChangeDetails(this," + length + ");'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductSubGroupCollection[" + length + "]' value='{{ st.PG_ID }}'>{{ st.PG_CODE +'-'+ st.PG_NAME }}</option></select></td><td><p style='display:none;'></p><select class='form-control chosen-select' data-live-search='true' data-show-subtext='true' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListCollection[" + length +"]' value='{{ st.P_ID }}'>{{ st.P_ArticleNo+'-'+st.P_ShortName }}</option></select></td><td style='display:none;'><input id='txtProductCode' class='form-control' /></td><td><input id='txtUOM' class='form-control' /></td><td><input id='txtDescription' class='form-control' /></td><td><input type='test' id='txtSummerPrice_" + i + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtWinterPrice_" + i + "' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('catalogBody')).append(temp);
            //$('.chosen-select').chosen();
            //ProductDetails();
            //GetCurrentProductDetails();
            //GetProductGroupDetails();
        }
       
    };

    //Delete the product
    $scope.deleterow = function (a)
    {
        var e = a.target;
        var len = $("#catalogBody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () {
                $(this).remove();
                length = length - 1;
            });
            var tr = $("#catalogBody").find("tr");
            for (var i = 0; i < tr.length; i++) {
                var td = tr[i].cells;
                $(td[0]).find("input").val(i + 1);
            }
        }
        else {
            message = "Atleast one contact required";
            toastr["error"](message, "Notification");
        }
    };

    //Restore the deleted records
   
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
    //            url: "/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Restore_delete",
    //            dataType: 'json',
    //            data: {
    //                id: a,
    //                type: true                      
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
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_Restore_delete",
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
                if (data == "Success")
                {
                    $($event.target.parentElement.parentElement.parentElement).fadeOut(800, function () { $(this).remove(); });
                    if (b)
                    {
                        message = "Record Deleted Successfully.";
                    }
                    else
                    {
                        message = "Record Restored Successfully.";
                    }

                    toastr["success"](message, "Notification");
                    setTimeout(() => {
                        location.reload();
                    }, 1000); 
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
    $scope.Restoredeleterecords = function (a, $event, b)
    {
        if (b)
        {
            alertmessageModified($event, a.toString(), b, 'ET_Admin_ProductCatalog', 'ET_Admin_ProductCatalog_Restore_delete');
        }
        else {
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    };
    $scope.PLCatalogChange = function () {
        //alert($scope.PL_Catalog_Type);
    }
    //$scope.PLTypeChange = function ()
    //{
    //    //$("#drpPackType").attr('disabled', false);
    //    var productCategory = "";
    //    if ($scope.PL_Type == "1")
    //        productCategory = "Yarn";
    //    else if ($scope.PL_Type == "2")
    //        productCategory = "Fabric";
    //    else
    //        productCategory = "Finished Goods";

    //    //Add new row element.
    //    angular.element(document.getElementById('catalogBody')).html("");
    //    var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><p style='display:none;'></p><select class='form-control chosen-select' id='drpProductshortname" + 1 + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListCollection[" + 1 +"]' value='{{ st.P_ID }}'>{{ st.P_ArticleNo +'-'+st.P_ShortName }}</option></select></td><td><input id='txtProductCode' class='form-control' /></td><td><input id='txtUOM' class='form-control' /></td><td><input id='txtDescription' class='form-control' /></td><td><input type='test' id='txtSummerPrice_" + 1 + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtWinterPrice_" + 1 + "' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
    //    var temp = $compile(Rowhtml)($scope);
    //    $('.chosen-select').chosen();
    //    $("#catalogBody").append(temp);
    //    ProductDetails();
    //};

    $scope.PGCatalogChange = function (recordId, rowIndex)
    {
        $scope.SubPL_Type = recordId;
        //Call the Product retreival
        //GetCurrentProductDetails(rowIndex);
        GetCurrentProductDetails(rowIndex, function () {});
    };

    $scope.SubGroupChange = function (recordId, rowIndex)
    {
        //alert($scope.ProductSubGroupCollection.length);
        $scope.PL_Type = recordId;
        GetProductSubGroupDetails(rowIndex, function ()
        {
            //$('#drpProductSubGroupShort0 option').each(function () {
            //    //arr.push($(this).val());
            //    alert($(this).val());
            //});
        }
        );
    };

    //Get Product Sub Group Details.
    function GetProductSubGroupDetails(rowIndex, _callback)
    {
        var productSGIndex = true;
        $.ajax({
            type: "GET",
            url: "/ET_Admin_ProductCatalog/GetProductSubGroupDetails",
            async: false,
            data: {
                parentGroup: $scope.PL_Type 
            },
            success: function (result)
            {
                var res = JSON.parse(result);
                $scope.ProductSubGroupCollection[rowIndex] = res;
                $scope.ProductSubGroupListShort = res;
                //SleepPGUpdates(80000);
                $("#drpProductSubGroupShort" + rowIndex).trigger("chosen:updated");
            }
        });
        //var deferred = $q.defer();
        //if (productSGIndex)
        //{
        //    var getproductlist = $http.get("/ET_Admin_ProductCatalog/GetProductSubGroupDetails",
        //        {
        //            params: { parentGroup: $scope.PL_Type }
        //        });
        //    getproductlist.then(function (result)
        //    {
               
        //    }, function ()
        //        {
        //        alert('Data not found');
        //    });
        //}
        //alert("Return back from subgroup collection");
        _callback();
    }

    //Get the product details
    function GetCurrentProductDetails(rowIndex,_callback)
    {
        //if ($scope.PL_Type == null || $scope.PL_Type == "")
        //    $scope.PL_Type = "1";
        $.ajax({
            type: "GET",
            url: "/ET_Admin_ProductCatalog/GetProductsCollection",
            async: false,
            data: {
                parentCategory: $scope.PL_Type, subCategory: $scope.SubPL_Type
            },
            success: function (result)
            {
                var res = JSON.parse(result);
                $scope.ProductListCollection[rowIndex] = res;
                $scope.ProductListshortname = res;
            }
        });
        //var getproductlist = $http.get("/ET_Admin_ProductCatalog/GetProductsCollection",
        //    {
        //        params: { parentCategory: $scope.PL_Type, subCategory: $scope.SubPL_Type }
        //    });
        //getproductlist.then(function (productlist)
        //{
            
        //   // alert(rowIndex);
        //    //ApplyProductCollection(rowIndex, res, function ()
        //    //{
        //    //    alert("Applied Products");
        //    //    //SleepPGUpdates(5000);
        //    //    _callback();
        //    //});
        //    alert(productlist.data);
        //    alert("Apply Scoping");
        //    $scope.ProductListCollection[rowIndex] = res;
        //    $scope.ProductListshortname = res;
        //    //alert("ProductList Short collection update");
        //    //$scope.$apply();
        //    //$("#drpProductshortname" + rowIndex).trigger("chosen:updated");
        //    //setTimeout(function () { $scope.$apply(); }, 1000);
        //}, function ()
        //    {
        //    alert('Data not found');
        //    });
        //SleepPGUpdates(5000);
        _callback();
    }

    function ApplyProductCollection(rowIndex,res,_callback)
    {
        $scope.ProductListCollection[rowIndex] = res;
        $scope.ProductListshortname = res;
        $scope.$apply();
        _callback();
    }

    function GetProductGroupDetails(_callback)
    {
        $.ajax({
            type: "GET",
            url: "/ET_Admin_ProductCatalog/GetProductGroupList",
            async: false,
            data: {
            },
            success: function (result)
            {
                var res = JSON.parse(result);
                $scope.ProductGroupListShort = res;
            }
        });
        //var getproductlist = $http.get("/ET_Admin_ProductCatalog/GetProductGroupList",
        //    {
        //        params: {}
        //    });
        //getproductlist.then(function (result)
        //{
        //    var res = JSON.parse(result.data);
        //    $scope.ProductGroupListShort = res;
        //    alert("Getting the Product Group as initial list");
        //}, function ()
        //{
        //    alert('Data not found');
        //    });
        _callback();
    }


    //Get the product details
    function ProductDetails()
    {
        if ($scope.PL_Type == null || $scope.PL_Type == "")
            $scope.PL_Type = "1";
        var getproductlist = $http.get("/ET_Admin_ProductCatalog/GetProducts",
            {
                params: { id: 0, productCategory: $scope.PL_Type }
            });
        getproductlist.then(function (productlist)
        {
            var res = JSON.parse(productlist.data);
            var i = res.length;
            var j = 1;
            $("#catalogBody").html("");
            //while (i != 0)
            //{
            //<td><input type='checkbox' name='select_all' value=' " + j + "' id='product-select-all'></td>
            var Rowhtml = "<tr><td width='10%'><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td width='10%'><p style='display:none;'></p><select class='form-control chosen-select' data-live-search='true' data-show-subtext='true' id='drpProductshortname" + length + "' onchange='ChangeDetails(this);'><option value='0'>-Select-</option><option data-ng-repeat='st in ProductListCollection[" + length +"]' value='{{ st.P_ID }}'>{{ st.P_ArticleNo+'-'+st.P_ShortName }}</option></select></td><td style='display:none'><input id='txtProductCode' class='form-control' /></td><td><input id='txtUOM' class='form-control' /></td><td><input id='txtDescription' class='form-control' /></td><td><input type='test' id='txtSummerPrice_" + i + "' class='form-control' onchange='MoneyValidation(this);' style='text-align:right;' /></td><td><input type='text' id='txtWinterPrice_" + i + "' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='21' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            //alert("catalog body");
            $("#catalogBody").append(temp);
            $('.chosen-select').chosen();
                //i--;
            //}
            //GetCurrentProductDetails();
            GetCurrentProductDetails(0, function () { });
            $scope.CatalogDetails = res;
        },
        function ()
        {
            alert('Data not found');
        }
        );
    };

    $scope.$watch("ProductListshortname", function (value) {
        var val = value || null;
        if (val)
        {
            //debugger;
            //Re-Assign the Product id again
            setTimeout(function () { $("#drpProductshortname" + i).trigger("chosen:updated"); }, 1000);
            //var tr = $("#catalogBody").find("tr");
            //for (i = 0; i < tr.length; i++)
            //for (i = 0; i < tr.length; i++)
            //{
            //    var td = tr[i].cells;
            //    alert($scope.ProductSelectorId[i]);
            //    $(td[3]).find("select").val($scope.ProductSelectorId[i]).trigger("chosen:updated");
            //    $(td[3]).find("p").text($scope.ProductSelectorId[i]);
            //    $("#drpProductshortname" + i).val($scope.ProductSelectorId[i]).trigger("chosen:updated");
            //    //alert("ProductListShortname changes for:" + i);
            //    //setTimeout(function () { $("#drpProductshortname" + i).trigger("chosen:updated"); }, 5);
            //    //    alert("Inside tr:" + i);
            //    //    var td = tr[i].cells;
            //    //    $(td[3]).find("select").val(1).trigger("chosen:updated");
            //    //    $(td[3]).find("p").text(1);
            //    ////    alert(productIds[i]);
            //    ////    $(td[1]).find("select").val(productIds[i]);
            //    ////    alert($(td[1]).find("select").val());
            //}
        }
    });

    $scope.$watch("ProductSubGroupListShort", function (value) {
        var val = value || null;
        if (val)
        {
            //debugger;
            setTimeout(function () { $("#drpProductSubGroupShort" + length).trigger("chosen:updated"); }, 5);
            //Re-Assign the Product id again
            //var tr = $("#catalogBody").find("tr");
            ////alert(tr.length);
            //for (i = 0; i < tr.length; i++)
            //{
            //    alert("Inside pggroup list short");
            //    var td = tr[i].cells;
            //    $(td[2]).find("select").val(12).trigger("chosen:updated");
            //    $(td[2]).find("p").text(12);
            ////    alert(productIds[i]);
            ////    $(td[1]).find("select").val(productIds[i]);
            ////    alert($(td[1]).find("select").val());
            //}
        }
    });

     $scope.$watch("ProductGroupListShort", function (value)
    {
        var val = value || null;
        if (val) {
            //debugger;
            //setTimeout(function () { $("#drpProductGroupListShort" + length).trigger("chosen:updated"); }, 100);
            //Re-Assign the Product id again
            //var tr = $("#catalogBody").find("tr");
            //alert(tr.length);
            //for (i = 0; i < tr.length; i++)
            //{
            //    var td = tr[i].cells;
            //    alert(productIds[i]);
            //    $(td[1]).find("select").val(productIds[i]);
            //    alert($(td[1]).find("select").val());
            //}
        }
    });

    function SleepPGUpdates(milliseconds)
    {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++)
        {
            if ((new Date().getTime() - start) > milliseconds)
            {
                break;
            }
        }
    }

    $scope.$watch("CatalogDetails", function (value)
    {
        var val = value || null;
        if (val) {
            var contactdata = $scope.CatalogDetails;
            setTimeout(function ()
            {
                var tr = $("#catalogBody").find("tr");
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(contactdata[i].SO_Serial);
                    $(td[1]).find("select").val(contactdata[i].PCG_ID).trigger("chosen:updated");
                    $(td[1]).find("p").text(contactdata[i].PCG_ID);
                    $scope.PL_Type = contactdata[i].PCG_ID;
                    $scope.SubPL_Type = contactdata[i].PCSG_ID;
                    GetProductSubGroupDetails(i,function ()
                    {
                        //SleepPGUpdates(20000);
                        //$('.chosen-select').chosen();
                        //var arr = new Array();
                        //$('#drpProductSubGroupShort0 option').each(function ()
                        //{
                        //    //arr.push($(this).val());
                        //});
                        $scope.$apply();
                        $("#drpProductSubGroupShort" + i).val(contactdata[i].PCSG_ID).trigger("chosen:updated");
                        $(td[2]).find("p").text(contactdata[i].PCSG_ID);
                        $(td[2]).find("select").val(contactdata[i].PCSG_ID).trigger("chosen:updated");
                        $(td[2]).find("p").text(contactdata[i].PCSG_ID);


                        $(td[4]).find("input").val(contactdata[i].P_ShortName);
                        $(td[5]).find("input").val(contactdata[i].LU_Description);
                        $(td[6]).find("input").val(contactdata[i].P_Description);
                        //$(td[2]).find("input").val(contactdata[i].P_ArticleNo);
                        $(td[7]).find("input").val(contactdata[i].P_SummerPrice.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $(td[8]).find("input").val(contactdata[i].P_WinterPrice.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                        $scope.ProductSelectorId[i] = contactdata[i].P_ID;
                        
                        GetCurrentProductDetails(i, function ()
                        {
                            $scope.$apply();
                            $(td[3]).find("select").val($scope.ProductSelectorId[i]).trigger("chosen:updated");
                            $(td[3]).find("p").text($scope.ProductSelectorId[i]);
                            $("#drpProductshortname" + i).val($scope.ProductSelectorId[i]).trigger("chosen:updated");
                            //Selection of the Product Subgroup Short.
                            //$scope.$apply();
                            //setTimeout(function () {
                            //    alert("waiting for timeout");
                            //    $scope.$apply(function () {
                            //    }
                            //    );
                            //}, 5);
                            //$timeout(function ()
                            //{
                            //}, 0, false);
                        });
                        //$("#drpProductSubGroupShort" + i).trigger("chosen:updated");
                        
                        //GetCurrentProductDetails(i);
                        //alert("Updating the Table Cells for row:" + i);
                        //$(td[2]).find("select").val(contactdata[i].PCSG_ID).trigger("chosen:updated");
                        //$(td[2]).find("p").text(contactdata[i].PCSG_ID);
                        ////alert(contactdata[i].PCSG_ID);
                    }); 
                    //alert("after call back");
                    //var lastUpdatedDate = new Date(parseInt(contactdata[i].P_LAST_PRICE_REWISE_DATE.substr(6)));
                    //var updatedDate = ("0" + lastUpdatedDate.getDate()).slice(-2) + "-" + ("0" + (lastUpdatedDate.getMonth() + 1)).slice(-2) + "-" + lastUpdatedDate.getFullYear();
                    //$(td[7]).find("input").val(updatedDate);
                }
            }, 1000);
        }
    });


    $scope.Restoredeleterecords = function (a, $event, b) {
        //debugger;
        if (b) {
            $scope.Isdelete = true;
            $scope.Isrestore = false;
            alertmessage(a.toString(), $event, $scope.PO_Type, 'ET_Admin_ProductCatalog', 'ET_Admin_ProductCatalog_Restore_delete');
        }
        else {
            $scope.Isdelete = false;
            $scope.Isrestore = true;
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    };

    //View: Popup view
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_View",
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
    };

    $scope.$watch("ProductCatalogLists", function (value)
    {
        var val = value || null;
        if (val)
        {
            setTimeout(function ()
            {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
            
        }
        //setTimeout(function () {
        //        dynamicDataTable('#advancedUsageTable', '#tableTools', '#colVis');
        //    }, 5);
        //$("#advancedUsageTable").dataTable();
        //$('#advancedUsageTable').DataTable({
        //    "paging": false,
        //    "ordering": false,
        //    "info": false
        //});
    });
    
});