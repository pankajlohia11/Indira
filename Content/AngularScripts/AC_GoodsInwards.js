app1.controller("AC_GICtrl", function ($scope, $http, $window, $compile) {
    GetPrivilages();
    GetGIList();  
    
    GetProducts();
    //Get the Privillage access for this document
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Purchase_GI/GetPrivilages");
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
     //Get The PO Details
    $scope.PoChange = function () {
        if ($scope.GI_POCode != 0) {
            GIDetails($scope.GI_POCode);
            var id = $scope.GI_POCode;
            $http({
                method: 'POST',
                url: '/ET_Purchase_GI/SalesOrgBind',
                dataType: 'json',
                data: {
                    id: id,
                },
            }).success(function (data) {
                debugger;
                var x = JSON.parse(data)
                $scope.GI_SupplierCode = x[0].suppliername;
                // $scope.GI_PODate = x[0].podate, "dd/M/Myyyy";
                var Quotationdate = new Date(parseInt(x[0].podate.substr(6)));
                var QDT = ("0" + Quotationdate.getDate()).slice(-2) + "-" + ("0" + (Quotationdate.getMonth() + 1)).slice(-2) + "-" + Quotationdate.getFullYear();

                $("#txtPODate").val(QDT);
                if ($scope.GI_ID == 0) {
                    GetProductDetailsFromPO($scope.GI_POCode, $scope.GI_StoreCode)
                }
            });
        }
        //else
        //{
        //}
    };

    //Get the ProductDetails From PO
    function GetProductDetailsFromPO(PO_ID, Store_ID)
    {
        $http({
            method: 'POST',
            url: '/ET_Purchase_GI/GetProductDetailsFromPO',
            dataType: 'json',
            data: {
                PO_ID: PO_ID,
                Store_ID: Store_ID,
            },
        }).success(function (data) {
           
            if (data.indexOf("Store Master") > -1) {
                message = data;
                toastr["error"](message, "Notification");
                $scope.products = "";
                $scope.ProductData = null;

                var temp = $compile($scope.singlerowdata)($scope);
                angular.element(document.getElementById('divTarget')).html("");
                angular.element(document.getElementById('divTarget')).append(temp);
            }
            else if (data.indexOf("PO already Exist") > -1)
            {
                message = data;
                toastr["error"](message, "Notification");

            }
            else {
                var enquirydata = JSON.parse(data);
                var i = enquirydata.length;
                var j = 0;
                debugger;
                $scope.GI_StoreCode = enquirydata[0].StoreId;
                $("#drpStoreCode").val($scope.GI_StoreCode).trigger("chosen:updated");
                $scope.Storechange();
                angular.element(document.getElementById('storedetailsbody')).html("");
                while (i != 0)
                {
                    var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' disabled=true' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtunitprice' class='form-control' style='text-align:right;' onchange='MoneyValidation(this);' readonly='readonly'/></td><td><input type='number' id='txtlotno' class='form-control'/></td><td><input type='text' id='txtPoQty' style='text-align:right;' onchange='MoneyValidation(this);' maxlength='14' class='form-control' /></td><td><input type='text' id='txtchallanQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtgiQty' maxlength='14' class='form-control' style='text-align:right;' onchange='calculateamount(this);'/></td><td><input type='text' id='txtbalanceQty' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly'/></td></tr>";
                    temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('storedetailsbody')).append(temp);
                    i--;
                    j++;
                }
                $scope.ProductData = enquirydata;
            }
        });
    };
     //Change The Store
    $scope.Storechange = function () {
        var id = $scope.GI_StoreCode;
        $http({
            method: 'POST',
            url: '/ET_Purchase_GI/storechange',
            dataType: 'json',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var x = JSON.parse(data)
            $scope.GI_StoreAddress = x[0].address;
        });

    };

    //Get PO list For GI
    function GetPOList(GI_Id)
    {
        debugger;
        var getsalespersonlist = $http.get("/ET_Purchase_GI/GetPOList",
            {
                params: {
                    GI_Id: GI_Id
                }
            });
        getsalespersonlist.then(function (ponolist) {
            var res = JSON.parse(ponolist.data);
            $scope.ponolist = res;
           // $scope.GI_POCode = "";
        }, function () {
            alert('Data not found');
        });
    };

    //Get the store List For storing the Product
    function GetStoreList(id)
    {
        debugger;
        var getstorelist = $http.get("/ET_Purchase_GI/GetStoreList",
            {
                params: {
                    id:id,
                }
            });
        getstorelist.then(function (storelist) {
            var res = JSON.parse(storelist.data);
            $scope.storelist = res;
        }, function () {
            alert("Supplier Not Found");
        });
    };
    //Get GI List
    function GetGIList() {
        var getstorelist = $http.get("/ET_Purchase_GI/GetGIList",
            {
                params: { delete: false }
            });
        getstorelist.then(function (gilist) {
            var res = JSON.parse(gilist.data);
            $scope.gilist = res;
        }, function () {
            alert("No Data Found");
        });
    }
    //Get GI List which are deleted
    function GetPORestoreList() {
        var getstorerestorelist = $http.get("/ET_Purchase_GI/GetGIList",
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
     //Get the All Products
    function GetProducts() {
        var getproductlist = $http.get("/ET_Purchase_GI/GetProducts");
        getproductlist.then(function (productlist) {
            var res = JSON.parse(productlist.data);
            $scope.products = res;
        }, function () {
            alert('Data not found');
        });
    }
   //Add the New Product
    $scope.addnewrow = function (a) {
        debugger;
        var e = a.target;
        var tr = $("#storedetailsbody").find("tr");
        var txt = "";
        for (var i = 0; i < tr.length; i++) {
            var td = tr[i].cells;
            var Product = $(td[1]).find("select").val();
            var po = $(td[6]).find("input").val();
            var gi = $(td[8]).find("input").val();
            if (Product == "") {
                message = "Select Product at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (po == "") {
                message = "Enter the PO Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (gi == "") {
                message = "Enter the GI Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
        }
        if (txt == "")
        {
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' disabled=true' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtunitprice' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtlotno' class='form-control'/></td><td><input type='text' style='text-align:right;' id='txtPoQty' maxlength='14' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtchallanQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtgiQty' class='form-control' maxlength='14' style='text-align:right;' onchange='calculateamount(this);'/></td><td><input type='text' id='txtbalanceQty' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly'/></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody')).append(temp);
        }
    }
    //Delete The Product
    $scope.deleterow = function (a) {
        var e = a.target;
        var len = $("#storedetailsbodystoredetailsbody").find("tr").length;
        if (len != 1) {
            $(e).parent().parent().parent().fadeOut(300, function () { $(this).remove(); });
        }
        else {
            message = "Atleast one Product required";
            toastr["error"](message, "Notification");
        }
    }
    //Show the GI List
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetGIList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    //Restore the GI
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetPORestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    }
    //Create the new GI
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            debugger;
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.GI_ID = "0";
            $scope.GI_Code = "";
            GetStoreList($scope.GI_ID);
            $scope.GI_StoreCode = "0";
            $scope.GI_StoreAddress = "";
            $scope.GI_POCode = "";
            
            $scope.GI_SupplierCode = "";
            $scope.GI_ChallanNo = "";
            $scope.GI_ReceivedBy = ""; 
            $("#txtPODate").val("");
            $("#txtGIDate").val("");
            $("#drpStoreCode").val("").trigger("chosen:updated");
            $("#drpPOCode").val("").trigger("chosen:updated");
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' disabled='true' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtunitprice' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly'/></td><td><input type='number' id='txtlotno' class='form-control'/></td><td><input type='text' id='txtPoQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtchallanQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtgiQty' class='form-control' maxlength='14' style='text-align:right;' onchange='calculateamount(this);'/></td><td><input type='text' id='txtbalanceQty' class='form-control' style='text-align:right;' onchange='MoneyValidation(this);' readonly='readonly'/></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody')).html("");
            angular.element(document.getElementById('storedetailsbody')).append(temp);
            GetPOList($scope.GI_ID);
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Validate the data before saving
    function validate() {
        
        var tr = $("#storedetailsbody").find("tr");
        var txt = "";
        for (i = 0; i < tr.length; i++) {
            var td = $(tr[i]).find("td"); 
            var po = $(td[6]).find("input").val();
            var gi = $(td[8]).find("input").val(); 
            if (po == "") {
                message = "Enter the PO Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (gi == "") {
                message = "Enter the GI Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
           
            else {
                debugger;
                var challanqty = "";
                console.log($(td[7]).find("input").val())
                if ($(td[7]).find("input").val() == "") {
                    challanqty = "";
                }
                else {
                    challanqty = parseFloat($(td[7]).find("input").val().split('.').join("").replace(",", "."));
                }
                
                txt = txt + $(td[1]).find("select").val() + ",";
                txt = txt + $(td[1]).find("select option:selected").text() + ",";
                txt = txt + $(td[2]).find("input").val() + ",";
                txt = txt + $(td[3]).find("input").val() + ",";
                txt = txt + parseFloat($(td[4]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + $(td[5]).find("input").val() + ",";
                txt = txt + parseFloat($(td[6]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + challanqty + ",";
                txt = txt + parseFloat($(td[8]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + parseFloat($(td[9]).find("input").val().split('.').join("").replace(",", ".")) + "|"; 
            }
        }
        return txt;
    }
    //Saving The GI
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var txt = validate();
        var gidate = $("#txtGIDate").val();
        var podate = $("#txtPODate").val();
        debugger;
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_GI/ET_Master_GI_Add",
                dataType: 'json',
                data: {
                    GI_ID: $scope.GI_ID,
                    GI_Code: $scope.GI_Code,
                    GI_Date: $("#txtGIDate").val(),
                    GI_StoreCode: $scope.GI_StoreCode,
                    GI_StoreAddress: $scope.GI_StoreAddress,
                    GI_POCode: $scope.GI_POCode,
                    GI_PODate: $("#txtPODate").val(),
                    GI_SupplierCode: $scope.GI_SupplierCode,
                    GI_ChallanNo: $scope.GI_ChallanNo,
                    GI_ReceivedBy: $scope.GI_ReceivedBy,
                    GIDetails: txt
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
                else if (data == "Success") {
                    if ($scope.GI_ID == 0) {
                        message = 'Record Inserted Successfully.';
                        toastr["success"](message, "Notification");
                    }
                    else {
                        message = 'Record Updated Successfully.';
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
        else {
            $("#div_loadImage").css("display", "none");
        }

    }
    //Edit the GI
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_GI/ET_Master_GI_Update_GetbyID",
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
                    var GIByID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    
                    $scope.GI_ID = GIByID.GI_ID;
                    GetPOList($scope.GI_ID);
                    GetStoreList($scope.GI_ID);
                    $scope.GI_Code = GIByID.GI_Code;
                   
                    $scope.GI_StoreCode = GIByID.GI_StoreCode;
                    $scope.GI_StoreAddress = GIByID.GI_StoreAddress;
                    $scope.GI_POCode = GIByID.GI_POCode;
                  
                    $scope.GI_SupplierCode = GIByID.GI_SupplierCode;
                    $scope.GI_ChallanNo = GIByID.GI_ChallanNo;
                    $scope.GI_ReceivedBy = GIByID.GI_ReceivedBy; 
                    var gidate = new Date(parseInt(GIByID.GI_Date.substr(6)));
                    
                    var QDT = ("0" + gidate.getDate()).slice(-2) + "-" + ("0" + (gidate.getMonth()+1)).slice(-2) + "-" + gidate.getFullYear();
                    $("#txtGIDate").val(QDT);
                    var podate = new Date(parseInt(GIByID.GI_PODate.substr(6)));
                    var QDTp = ("0" + podate.getDate()).slice(-2) + "-" + ("0" + (podate.getMonth()+1)).slice(-2) + "-" + podate.getFullYear();
                    $("#txtPODate").val(QDTp);
                    $("#drpStoreCode").val(GIByID.GI_StoreCode).trigger("chosen:updated");
                    $("#drpPOCode").val(GIByID.GI_POCode).trigger("chosen:updated"); 
                    GIDetails(GIByID.GI_ID);
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
    //Get the GI Product Details
    function GIDetails(e) {
        debugger;
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Purchase_GI/ET_Master_GI_Details_Load',
            data: {
                id: id,
            },
        }).success(function (data) {
            debugger;
            var enquirydata = JSON.parse(data);
            var i = enquirydata.length;
            var j = 0;
            angular.element(document.getElementById('storedetailsbody')).html("");
            while (i != 0)
            {
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' disabled=true' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.P_ID }}'>{{ product.P_ArticleNo }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtunitprice' class='form-control' style='text-align:right;' onchange='MoneyValidation(this);' readonly='readonly'/></td><td><input type='number' id='txtlotno' class='form-control'/></td><td><input type='text' id='txtPoQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtchallanQty' maxlength='14' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' /></td><td><input type='text' id='txtgiQty' maxlength='14' class='form-control' style='text-align:right;' onchange='calculateamount(this);'/></td><td><input type='text' id='txtbalanceQty' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly'/></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody')).append(temp);
                i--;
                j++;
            }
            $scope.enquirydata1 = enquirydata;
        });
    }
    //Restore the Deleted Records
    $scope.PerformRestore = function (a, $event, b) {
        var post = $http({
            method: "POST",
            url: "/ET_Purchase_GI/ET_Master_GI_RestoreDelete",
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
    //            url: "/ET_Purchase_GI/ET_Master_GI_RestoreDelete",
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
        debugger;
        //alert("delete message");
        //alert(a.toString());
        if (b) {
            alertmessageModified($event, a.toString(),b,'ET_Purchase_GI', 'ET_Master_GI_RestoreDelete');
        }
        else
        {
            $scope.PerformRestore(a, $event, b);
            //message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    } 
    //View The GI
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_GI/ET_Master_GI_View",
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
                alert(data)
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    
   
   //Watch for All Data Binding
    $scope.$watch("storelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpStoreCode").val($scope.GI_StoreCode).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("ponolist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpPOCode").val($scope.GI_POCode).trigger("chosen:updated"); }, 100);
        }
    }); 
    $scope.$watch("gilist", function (value) {
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
    $scope.$watch("enquirydata1", function (value) {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.enquirydata1;
            setTimeout(function () {
                debugger;
                var tr = $("#storedetailsbody").find("tr");
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(enquirydata[i].SO_Serial);
                    $(td[1]).find("select").val(enquirydata[i].productid);
                    $(td[2]).find("input").val(enquirydata[i].name);
                    $(td[3]).find("input").val(enquirydata[i].uom);
                    $(td[4]).find("input").val(parseFloat(enquirydata[i].Unitprice).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[5]).find("input").val(enquirydata[i].lotno); 
                    $(td[6]).find("input").val(parseFloat(enquirydata[i].poQuantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[7]).find("input").val(parseFloat(enquirydata[i].ChallanQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[8]).find("input").val(parseFloat(enquirydata[i].GIQuantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[9]).find("input").val(parseFloat(enquirydata[i].BalanceQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                }
            }, 3000);
        }
    });
    $scope.$watch("ProductData", function (value) {
        var val = value || null;
        if (val) {
            var enquirydata = $scope.ProductData;
            setTimeout(function () {
                debugger;
                var tr = $("#storedetailsbody").find("tr");
                for (var i = 0; i < tr.length; i++) {
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(enquirydata[i].SO_Serial);
                    $(td[1]).find("select").val(enquirydata[i].PD_ProductID);
                    $(td[2]).find("input").val(enquirydata[i].name);
                    $(td[3]).find("input").val(enquirydata[i].uom);
                    $(td[4]).find("input").val(parseFloat(enquirydata[i].Unitprice).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[6]).find("input").val(parseFloat(enquirydata[i].poQuantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                }
            }, 3000);
        }
    });
});