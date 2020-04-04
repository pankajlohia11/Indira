app1.controller("AC_TradingPackinglistCtrl", function ($scope, $http, $window, $compile)
{
    var type1 = window.location.href.toString().split("&");
    var enqid = window.location.href.toString().split("ShipID=");
    $scope.SelectedFileForUpload = null; //initially make it null
    $scope.BindData = null;
    $scope.showLoader = false;
    $scope.IsVisible = false;
    $scope.OTOI_ShipmentID = enqid[1];
    $scope.OTOI_ID = 0;
    $scope.PL_Type = "1";
    $scope.SelectedFileForUpload = null; //initially make it null
    //$("#drpPackType").attr('disabled', 'disabled');
    if ($scope.OTOI_ShipmentID == "" || $scope.OTOI_ShipmentID == null)
    {
        $scope.submittext = "Submit";
        //$("#drpShipment").attr('disabled', "disabled");
    }
    else
    {
        $scope.submittext = "";
        /*toastr.options =
            {
            "closeButton": true,
            "debug": false,
            "allowHtml": true,
            "positionClass": "toast-top-center",
            "onclick": null,
            "showDuration": "5000",
            "hideDuration": "5000",
            "timeOut": "50000",
            "extendedTimeOut": "5000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "show",
            "hideMethod": "fadeOut"
        };*/
        toastr["info"]("Shipment:" + $scope.OTOI_ShipmentID.toString() + "Create Packing List", "Notification");
    }
    //$("#drpShipment").val($scope.OTOI_ShipmentID).trigger("chosen:updated");
    //alert($("#drpShipment option:selected").text());
    GetPrivilages();
    GetPackingList();
    //GetTheShipmentDetails();
    //GetShipmentList();
    GetProducts(); 
    //check the privillages
    function GetPrivilages()
    {
        var getprivilages = $http.get("/ET_Trading_PackingList/GetPrivilages");
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
    };

    //app1.service("Excelservice", function ($http)
    //{
    //    this.SendExcelData = function (data)
    //    {
    //        var request = $http({
    //            method: "post",
    //            withCredentials: true,
    //            url: '/ET_Trading_PackingList/ReadExcel',
    //            data: data,
    //            headers: {
    //                'Content-Type': undefined
    //            },
    //            transformRequest: angular.identity
    //        });
    //        return request;
    //    }
    //});

    //used to check the extension of file while uploading
    function checkfile(sender)
    {
        var validExts = new Array(".xlsx", ".xls");
        var fileExt = sender.value;
        fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
        if (validExts.indexOf(fileExt) < 0)
        {
            alert("Invalid file selected, valid files are of " +
                validExts.toString() + " types.");
            return false;
        }
        else return true;
    }

    $scope.ParseExcel = function ()
    {
        var formData = new FormData();
        var file = $scope.SelectedFileForUpload;
        formData.append('file', file);
        formData.append("packingType", $scope.PL_Type);
        $scope.showLoader = true;   //show spinner
        var post = $http({
            method: "POST",
            url: "/ET_Trading_PackingList/ReadExcel",
            data: formData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        });
        post.success(function (data, status)
        {
            if (data.indexOf("ERR") > -1) {
                //message = "Packing";
                toastr["error"](data, "Notification");
            }
            else
            {
                toastr["success"]("Valid Packing List.Importing the Items.", "Notification");
                var i = data.length;
                var j = 0;
                var Rowhtml = "";
                angular.element(document.getElementById('storedetailsbody1')).html("");
                angular.element(document.getElementById('storedetailsbody2')).html("");
                angular.element(document.getElementById('storedetailsbody3')).html("");

                while (i != 0)
                {
                    var temp = "";
                    if ($scope.PL_Type == "1") {
                        Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtNoof' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                        temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('storedetailsbody1')).append(temp);
                    }
                    else if ($scope.PL_Type == "2") {
                        Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtDesignNo' class='form-control'/></td><td><input type='text' id='txtNoofpeices' class='form-control' /></td><td><input type='text' id='txtTotalMeters' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                        temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('storedetailsbody2')).append(temp);
                    }
                    else {
                        Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtNoofpieces' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPackingunit' class='form-control' readonly='readonly' /></td></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtRemarks' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td>";
                        temp = $compile(Rowhtml)($scope);
                        angular.element(document.getElementById('storedetailsbody3')).append(temp);
                    }
                    i--;
                    j++;
                }
                $scope.importedData = data;
                //$scope.BindData = data;
                $scope.IsVisible = true;
                $scope.showLoader = false;
            }
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
        //var response = Excelservice.SendExcelData(formData);   //calling service
        //response.then(function (d)
        //{
        //    var res = d.data;
        //    $scope.BindData = res;
        //    $scope.IsVisible = true; //showing the table after databinding
        //    $scope.showLoader = false; //after success hide spinner
        //}, function (err) {
        //    console.log(err.data);
        //    console.log("error in parse excel");
        //});
    }

    //Upload the Excel Files data.
    $scope.UploadFiles = function (files) {
        $scope.$apply(function ()
        {
            $scope.Message = '';
            $scope.SelectedFileForUpload = files[0];
        });
    };  

    $scope.PLTypeChange = function ()
    {
        packchange();
    };

    //Get the Shipments
    function GetShipmentForPacking()
    {
        if ($scope.PL_CustomerID != "")
        {
            var getshipmentlist = $http.get("/ET_Trading_PackingList/GetShipmentForPacking",
                {
                    params: { customerId: $scope.PL_CustomerID, orderId: $scope.SM_Store_User }
                });
            getshipmentlist.then(function (shipmentInfo)
            {
                var res = JSON.parse(shipmentInfo.data);
                $scope.OTOI_ShipmentID = res;
                //$("#drpShipment").val($scope.OTOI_ShipmentID).trigger("chosen:updated");
                $("#drpShipment").val($scope.OTOI_ShipmentID);
                if ($scope.OTOI_ShipmentID == "" || $scope.OTOI_ShipmentID == null)
                {
                    message = "Shipment not available for the current order";
                    toastr["error"](message, "Notification");
                }
                else
                {

                }
            }, function () {
                alert("Shipments Not Found");
            });
        }
        else
        {
            $scope.shipmentlist = {};
            $scope.OTOI_ShipmentID = "";
        }
    }

    function GetShipmentList()
    {
        var getshipmentlist = $http.get("/ET_Sales_Shipment/GetShipmentList",
            {
                params: { delete: false, type: 2 }
            });
        getshipmentlist.then(function (shipmentlist) {
            var res = JSON.parse(shipmentlist.data);
            $scope.shipmentlist = res;
        }, function () {
            alert("Quotation List Found");
        });
    }

    function GetTheShipmentDetails() {
        debugger;
        var idval = parseFloat($scope.OTOI_ShipmentID);
        var getshipmentlist = $http.get("/ET_Sales_OneToOneInvoice/GetTheShipmentDetails",
            {
                params: { id: idval }
            });
        getshipmentlist.then(function (shipmentlist) {
            debugger;
            var res = JSON.parse(shipmentlist.data);
            $scope.PL_CustomerID = res[0].S_CustSup;
            $("#drpCustomer").val($scope.PL_CustomerID).trigger("chosen:updated");
            //$scope.CustomerChange();
        }, function ()
        {
            alert("Shipment Not Found");
        });

    }

    //Get the packing list
    function GetPackingList() {
        var getstorelist = $http.get("/ET_Trading_PackingList/GetPackingList",
            {
                params: { delete: false }
            });
        getstorelist.then(function (storelist) {
            var res = JSON.parse(storelist.data);
            $scope.storelist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the restore list
    function GetStoreRestoreList() {
        var getstorerestorelist = $http.get("/ET_Trading_PackingList/GetPackingList",
            {
                params: { delete: true }
            });
        getstorerestorelist.then(function (storerestorelist) {
            var res = JSON.parse(storerestorelist.data);
            $scope.storerestorelist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get the customer list
    function GetCustomerList() {
        var getcustomerlist = $http.get("/ET_Trading_PackingList/GetCustomers",
            {
                params: {
                    id: $scope.PL_ID,
                }
            });
        getcustomerlist.then(function (customerlist) {
            var res = JSON.parse(customerlist.data);
            $scope.customers = res;
        }, function () {
            alert("Customer Data Not Found");
        });
    }
    $scope.CustomerChange = function ()
    {
        $scope.PL_OrderNo = "";
        GetOrderList();
        //GetShipments();
    }
    //Get the order list
    function GetOrderList() {
        debugger;
        var getstoreuserlist = $http.get("/ET_Trading_PackingList/GetOrderList", {
            params : { custid : $scope.PL_CustomerID }
        });
        getstoreuserlist.then(function (storeuserlist) {

            var res = JSON.parse(storeuserlist.data);
            $scope.storeuserlist = res;
        }, function () {
            alert("No Users Found");
        });
    }
    function packchange() {
        var type;
        var Rowhtml = '';
        var temp = '';
        GetProducts();
        if ($scope.PL_Type == "1")
        {
            angular.element(document.getElementById('storedetailsbody1')).html("");
            angular.element(document.getElementById('product_yarn_table')).attr("style", "display:block");
            angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
            angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
            Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtpalletno' class='form-control'/></td><td><input type='text' id='txtNoof' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody1')).append(temp);
        }

        else if ($scope.PL_Type == "2")
        {
            angular.element(document.getElementById('storedetailsbody2')).html("");
            angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
            angular.element(document.getElementById('product_fabric_table')).attr("style", "display:block");
            angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
            Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtDesignNo' class='form-control'/></td><td><input type='text' id='txtNoofpeices' class='form-control' /></td><td><input type='text' id='txtTotalMeters' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody2')).append(temp);
        }


        else if ($scope.PL_Type == "3")
        {
            angular.element(document.getElementById('storedetailsbody3')).html("");
            angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
            angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
            angular.element(document.getElementById('product_FI_table')).attr("style", "display:block");
            Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtNoofpieces' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPackingunit' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtRemarks' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td>";
            temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody3')).append(temp);
        }
    }
    //Get the products
    function GetProducts()
    {
        var getproductlist = $http.get("/ET_Trading_PackingList/GetProducts",
            {
                params: { productCategory: $scope.PL_Type }
            });
        getproductlist.then(function (productlist)
        {
            var res = JSON.parse(productlist.data);
            $scope.products = res;
        }, function ()
        {
            alert('Data not found');
        });
    } 

    $scope.PackingImport = function () {
        alert("Packing Import");
    };

    $scope.UserChange = function ()
    {
        GetShipmentForPacking();
    }
    //Add the new product
    $scope.addnewrow = function (a) {
        debugger;
        var e = a.target;
        if ($scope.PL_Type == "1")
        {
            var tr = $("#storedetailsbody1").find("tr");
            var txt = "";
            for (var i = 0; i < tr.length; i++)
            {
                var td = tr[i].cells;
                var Product = $(td[1]).find("select").val();
                if (Product == "")
                {
                    message = "Select Product at row " + (i + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";
                }
            }
            if (txt == "")
            {
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option data-ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtpalletno' class='form-control'/></td><td><input type='text' id='txtCones' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody1')).append(temp);
            }
        }
        else if ($scope.PL_Type == "2") {
            tr = $("#storedetailsbody2").find("tr");
            txt = "";
            for (var j = 0; j < tr.length; j++) {
                td = tr[j].cells;
                Product = $(td[1]).find("select").val();
                if (Product == "") {
                    message = "Select Product at row " + (j + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";
                }
            }
            if (txt == "")
            {
                Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtDesignNo' class='form-control'/></td><td><input type='text' id='txtNoofpeices' class='form-control' /></td><td><input type='text' id='txtTotalMeters' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody2')).append(temp);
            }
        }
        else {
            tr = $("#storedetailsbody3").find("tr");
            txt = "";
            for (var k = 0; i < tr.length; k++) {
                td = tr[k].cells;
                Product = $(td[1]).find("select").val();
                if (Product == "") {
                    message = "Select Product at row " + (k + 1);
                    toastr["error"](message, "Notification");
                    txt = "asd";
                }
            }
            if (txt == "")
            {
                Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtNoofpieces' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPackingunit' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtRemarks' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td>";
                temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody3')).append(temp);
            }
        }
    }
    //Delete the product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#storedetailsbody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
            message = "Atleast one Product required";
            toastr["error"](message, "Notification");
        }
    }
    //View the records
    $scope.showRecords = function () {
        debugger;
        $("#advancedusage").dataTable().fnDestroy();
        GetPackingList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //Restore the records
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetStoreRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
        $("#div_Print").css("display", "none");
    }
    //Create the new packing list
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            debugger;
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $("#div_Print").css("display", "none");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.PL_ID = "0";
            $scope.PL_Code = ""; 
            $scope.PL_OrderNo = "";
            $scope.PL_Remarks = ""; 
            $scope.PL_CustomerID = ""; 
            $("#drpPackType").val("1").trigger("chosen:updated");
            $("#txtDate").val("");
            $("#drporderUsers").val("").trigger("chosen:updated"); 
            $("#drpCustomer").val("").trigger("chosen:updated"); 
            GetCustomerList();
            angular.element(document.getElementById('storedetailsbody1')).html("");
            angular.element(document.getElementById('storedetailsbody2')).html("");
            angular.element(document.getElementById('storedetailsbody3')).html("");
            //var Rowhtml = "<tr><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtDesignNo' class='form-control'/></td><td><input type='text' id='txtNoofpeices' class='form-control' /></td><td><input type='text' id='txtTotalMeters' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            //var temp = $compile(Rowhtml)($scope);
            if ($scope.PL_Type == "1")
            {   
                angular.element(document.getElementById('storedetailsbody1')).html("");
                //angular.element(document.getElementById('storedetailsbody')).append(temp);
                angular.element(document.getElementById('product_yarn_table')).attr("style", "display:block");
                angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
                angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
                Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option data-ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPallet' class='form-control'/></td><td><input type='text' id='txtCones' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                //Rowhtml = "<tr><td></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtNoof' class='form-control'/></td><td><input type='text' id='txtCones' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody1')).append(temp);
            }
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Validate the data
    function validate() {
        debugger;
        var txt = "";
        var tr = "";
        if ($scope.PL_Type == "1")
        {
            tr = $("#storedetailsbody1").find("tr");
        
        }
        else if ($scope.PL_Type == "2") {
            tr = $("#storedetailsbody2").find("tr");

        }
        else
        {
            tr = $("#storedetailsbody3").find("tr");

        }
        
        for (i = 0; i < tr.length; i++)
        {
            var td = $(tr[i]).find("td");
            var article = $(td[1]).find("select").val();
            var product = $(td[2]).find("input").val();
            var palletno = $(td[4]).find("input").val();
            var rate = $(td[5]).find("input").val();
            if (product == "" || article == "")
            {
                message = "Select Article & Product at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            } 
            else
            {
                    txt = txt + $(td[1]).find("select").val() + ",";
                    txt = txt + $(td[1]).find("select option:selected").text() + ",";
                    txt = txt + $(td[2]).find("input").val() + ",";
                    txt = txt + $(td[3]).find("input").val() + ",";
                    txt = txt + $(td[4]).find("input").val() + ",";
                    txt = txt + $(td[5]).find("input").val() + ",";
                    txt = txt + $(td[6]).find("input").val() + ",";
                if ($scope.PL_Type == "1")
                {
                    txt = txt + $(td[7]).find("input").val() + ",";
                    txt = txt + $(td[8]).find("input").val() + "|";
                }
                else if ($scope.PL_Type == "2")
                {
                    txt = txt + $(td[7]).find("input").val() + ",";
                    txt = txt + $(td[8]).find("input").val() + ",";
                    txt = txt + $(td[9]).find("input").val() + ",";
                    txt = txt + $(td[10]).find("input").val() + "|";
                }
                else
                {
                    txt = txt + $(td[7]).find("input").val() + "|";
                }
            }
        }
        return txt;
    }
    //Save the records
    $scope.SubmitClick = function () {
        debugger;
        var txt = validate();
        var date = $("#txtDate").val();
        debugger;
        var ords = "";
        try
        {
            ords = $scope.SM_Store_User.join();
        }
        catch (ex) {
            ords = $scope.SM_Store_User;
        }
        if (txt != "")
        {
            var shipmentId = String($scope.OTOI_ShipmentID);
            var post = $http({
                method: "POST",
                url: "/ET_Trading_PackingList/ET_Trading_PackingList_Add",
                dataType: 'json',
                data: {
                    PL_ID: $scope.PL_ID,
                    PL_Code: $scope.PL_Code,
                    PL_Date: date,
                    PL_CustomerID: $scope.PL_CustomerID,
                    PL_OrderNo: ords,
                    PL_Remarks: $scope.PL_Remarks,
                    packingType: $scope.PL_Type,
                    shipmentCode: shipmentId,
                    packingSummary: $scope.PL_Summary,
                    EnquiryDetails: txt
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
                else if (data.indexOf("Validation") > -1) {
                    toastr["error"](data, "Notification");
                }
                else if (data.indexOf("Success") > -1) {
                    var res = data.split(':');
                    if ($scope.PL_ID == 0) {
                        message = 'Record Inserted Successfully With Code : '+res[1];
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
            });
        }

    }
    //Edit the records
    $scope.editRecords = function (a) {
        debugger;
        a = parseInt(a);
        if ($scope.Isedit)
        {
            var post = $http({
                method: "POST",
                url: "/ET_Trading_PackingList/ET_Trading_PackingList_Update_GetbyID",
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
                    var StoreByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    $scope.PL_ID = StoreByID.PL_ID;
                    $scope.PL_Code = StoreByID.PL_Code; 
                    $scope.PL_CustomerID = StoreByID.PL_CustomerID;  
                    $scope.SM_Store_User = StoreByID.PL_OrderNo.split(',');
                    $scope.PL_Remarks = StoreByID.PL_Remarks;
                    $scope.OTOI_ShipmentID = StoreByID.PL_ShipmentCode;
                    $scope.PL_ShipmentCode = StoreByID.PL_ShipmentCode;
                    $scope.PL_Summary = StoreByID.PL_Summary;
                    //$("#drpShipment").val($scope.OTOI_ShipmentID).trigger("chosen:updated");

                    if (StoreByID.PL_Type == 1)
                        $scope.PL_Type = "1";
                    else if (StoreByID.PL_Type == 2)
                        $scope.PL_Type = "2";
                    else
                        $scope.PL_Type = "3";

                    $("#drpPackType").val($scope.PL_Type).trigger("chosen:updated");
                    $("#drpShipment").val($scope.PL_ShipmentCode);
                    $("#drpShipment").val($scope.PL_ShipmentCode);
                    $("#txtPackingSummary").val($scope.PL_Summary);

                    var Quotationdate = new Date(parseInt(StoreByID.PL_Date.substr(6)));
                    var QDT = ("0" + Quotationdate.getDate()).slice(-2) + "-" + ("0" + (Quotationdate.getMonth() + 1)).slice(-2) + "-" + Quotationdate.getFullYear();
                    $("#txtDate").val(QDT);
                    $("#drporderUsers").val($scope.SM_Store_User).trigger("chosen:updated");
                    GetCustomerList();
                    GetOrderList();
                    EnquiryDetails(StoreByID.PL_ID);
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
    //Gt the product details
    function EnquiryDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Trading_PackingList/ET_Trading_Packing_Details_Load',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var enquirydata = JSON.parse(data);
            var i = enquirydata.length;
            var j = 0;
            var Rowhtml = "";
            angular.element(document.getElementById('storedetailsbody1')).html("");
            angular.element(document.getElementById('storedetailsbody2')).html("");
            angular.element(document.getElementById('storedetailsbody3')).html("");
            GetProducts();
            while (i != 0)
            {
                var temp = "";
                if ($scope.PL_Type == "1")
                {
                    Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtNoof' class='form-control'/></td><td><input type='text' id='txtlotno' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                    temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('storedetailsbody1')).append(temp);
                }
                else if ($scope.PL_Type == "2")
                {
                    Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /><td><input type='text' id='txtpalletno' class='form-control' /></td><td><input type='text' id='txtPalletNo' class='form-control'/></td><td><input type='text' id='txtDesignNo' class='form-control'/></td><td><input type='text' id='txtNoofpeices' class='form-control' /></td><td><input type='text' id='txtTotalMeters' class='form-control' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtremarks' class='form-control' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                    temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('storedetailsbody2')).append(temp);
                }
                else
                {
                    Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control chosen-select' id='drpProduct' onchange='ProductChange(this);'><option value='0'>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtNoofpieces' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtPackingunit' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtGwtinKGS' class='form-control' /></td><td><input type='text' id='txtNwtinKGS' class='form-control' /></td><td><input type='text' id='txtRemarks' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td>";
                    temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('storedetailsbody3')).append(temp);
                }
                i--;
                j++;
            }
            $scope.enquirydata1 = enquirydata;
        });
    }
    //Restore the deleted records
    $scope.PerformRestore = function (a, $event, b) {

        var post = $http({
            method: "POST",
            url: "/ET_Trading_PackingList/ET_Trading_PackingList_RestoreDelete",
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
    //            url: "/ET_Sales_PackingList/ET_Master_Store_RestoreDelete",
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
            alertmessageModified($event, a.toString(),b,'ET_Trading_PackingList', 'ET_Trading_PackingList_RestoreDelete');
        }
        else 
        {
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    } 
    //View the records
    $scope.ViewRecords = function (a) {
        debugger;
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Trading_PackingList/ET_Trading_PackingList_View",
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
    //Watch for all data binding
    $scope.$watch("storeuserlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drporderUsers").val($scope.SM_Store_User).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("customers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.PL_CustomerID).trigger("chosen:updated"); }, 100);
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
    $scope.$watch("storerestorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusageRestore', '#tableToolsRestore', '#colVisRestore');
            }, 5);
        }
    });
    //Change the language for printing
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
    //Print the records
    $scope.PrintRecords = function (a, b) {

        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Trading_PackingList/ET_Master_PackingList_Print",
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

                    $('#div_PrintContent').html(data);
                    $("#div_View").css("display", "none");
                    $("#div_Restore").css("display", "none");
                    $("#div_Edit").css("display", "none");
                    $("#div_Print").css("display", "block");
                }
            });

            post.error(function (data, status) {
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    };

    $scope.$watch("enquirydata1", function (value)
    {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.enquirydata1;
            setTimeout(function ()
            {
                debugger;
                var tr = "";
                if ($scope.PL_Type == "1")
                {
                    tr = $("#storedetailsbody1").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:block");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
                }
                else if ($scope.PL_Type == "2") {
                    tr = $("#storedetailsbody2").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:block");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
                }
                else {
                    tr = $("#storedetailsbody3").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:block");
                }
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    if ($scope.PL_Type == "1")
                    {
                        $(td[1]).find("select").val(enquirydata[i].productid);
                        $(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].palletno);
                        $(td[4]).find("input").val(enquirydata[i].noofcones);
                        $(td[5]).find("input").val(enquirydata[i].lotno);
                        $(td[6]).find("input").val(enquirydata[i].gwtinkgs);
                        $(td[7]).find("input").val(enquirydata[i].nwtinkgs);
                        $(td[8]).find("input").val(enquirydata[i].remarks);
                    }
                    else if ($scope.PL_Type == "2")
                    {
                        $(td[1]).find("select").val(enquirydata[i].productid);
                        $(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].palletno);
                        $(td[4]).find("input").val(enquirydata[i].designno);
                        $(td[5]).find("input").val(enquirydata[i].noofpeices);
                        $(td[6]).find("input").val(enquirydata[i].individualpiecelemgth);
                        $(td[7]).find("input").val(enquirydata[i].totalmeters);
                        $(td[8]).find("input").val(enquirydata[i].gwtinkgs);
                        $(td[9]).find("input").val(enquirydata[i].nwtinkgs);
                        $(td[10]).find("input").val(enquirydata[i].remarks);
                    }
                    else {
                        $(td[1]).find("select").val(enquirydata[i].productid);
                        $(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].noofpeices);
                        $(td[4]).find("input").val(enquirydata[i].packingunits);
                        $(td[5]).find("input").val(enquirydata[i].gwtinkgs);
                        $(td[6]).find("input").val(enquirydata[i].nwtinkgs);
                        $(td[7]).find("input").val(enquirydata[i].remarks);
                    }
                }
            }, 1000);
        }
    });

    $scope.$watch("importedData", function (value) {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.importedData;
            setTimeout(function () {
                debugger;
                var tr = "";
                if ($scope.PL_Type == "1") {
                    tr = $("#storedetailsbody1").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:block");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
                }
                else if ($scope.PL_Type == "2") {
                    tr = $("#storedetailsbody2").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:block");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:none");
                }
                else {
                    tr = $("#storedetailsbody3").find("tr");
                    angular.element(document.getElementById('product_yarn_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_fabric_table')).attr("style", "display:none");
                    angular.element(document.getElementById('product_FI_table')).attr("style", "display:block");
                }
                var previousSerial = 0;
                for (var i = 0; i < tr.length; i++)
                {
                    
                    //var serialNr = parseInt(enquirydata[i].Serial);
                    //if (serialNr != 0)
                    //    previousSerial = serialNr;
                    var td = tr[i].cells;
                    //if (serialNr == 0)
                    //    $(td[0]).find("input").val(previousSerial);
                    //else
                    //    $(td[0]).find("input").val(serialNr);
                    $(td[0]).find("input").val(i+1);
                    if ($scope.PL_Type == "1")
                    {
                        //$(td[1]).find("select").val(enquirydata[i].productid);
                        //$(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].PallentNo);
                        $(td[4]).find("input").val(enquirydata[i].ConesCount);
                        $(td[5]).find("input").val(enquirydata[i].LotNr);
                        $(td[6]).find("input").val(enquirydata[i].GWTKg);
                        $(td[7]).find("input").val(enquirydata[i].NtWTKg);
                        $(td[8]).find("input").val(enquirydata[i].Remarks);
                    }
                    else if ($scope.PL_Type == "2")
                    {
                        //$(td[1]).find("select").val(enquirydata[i].productid);
                        //$(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].PalletNo);
                        $(td[4]).find("input").val(enquirydata[i].DesignNo);
                        $(td[5]).find("input").val(enquirydata[i].PiecesNo);
                        $(td[6]).find("input").val(enquirydata[i].IndvPieceLength);
                        $(td[7]).find("input").val(enquirydata[i].TotalMtrs);
                        $(td[8]).find("input").val(enquirydata[i].GWTKg);
                        $(td[9]).find("input").val(enquirydata[i].NtWTKg);
                        $(td[10]).find("input").val(enquirydata[i].Remarks);
                    }
                    else {
                        //$(td[1]).find("select").val(enquirydata[i].productid);
                        //$(td[2]).find("input").val(enquirydata[i].name);
                        $(td[3]).find("input").val(enquirydata[i].PiecesCount);
                        $(td[4]).find("input").val(enquirydata[i].PackingUnits);
                        $(td[5]).find("input").val(enquirydata[i].GWTKg);
                        $(td[6]).find("input").val(enquirydata[i].NtWTKg);
                        $(td[7]).find("input").val(enquirydata[i].Remarks);
                    }
                }
            }, 1000);
        }
    });

});