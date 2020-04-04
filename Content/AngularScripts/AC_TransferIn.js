app1.controller("AC_ITCtrl", function ($scope, $http, $window, $compile) {
    GetPrivilages();
    GetITList(); 
    GetStore();
    GetGIno();
    //GetProductList();
      //Get the Privillage access for this document
    function GetPrivilages() {
        var getprivilages = $http.get("/ET_Purchase_Transfers/GetPrivilages");
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
    //Get the Transfer List
    function GetITList() {
        var getstorelist = $http.get("/ET_Purchase_Transfers/GetITList",
            {
                params: { delete: false }
            });
        getstorelist.then(function (itlist) {
            var res = JSON.parse(itlist.data);
            $scope.itlist = res;
        }, function () {
            alert("No Data Found");
        });
    }
     //Bind from Stores
     function GetStore() {

         debugger;
         var getstoreuserlist = $http.get("/ET_Purchase_Transfers/GetStore");
        getstoreuserlist.then(function (storelist) {

            var res = JSON.parse(storelist.data);
            $scope.storelist = res;
            $scope.SM_Store_User = "";
        }, function () {
            alert("No Users Found");
        });
    }
    //Get the GI NO
    function GetGIno() {
        debugger;
        var getgilist = $http.get("/ET_Purchase_Transfers/GetGIno");
        getgilist.then(function (gilist) {

            var res = JSON.parse(gilist.data);
            $scope.gilist = res;
            $scope.IT_GINo = "";
        }, function () {
            alert("No Users Found");
        });
    }
     // Bind to Stores
    $scope.UserChange = function () {
        debugger;
        if ($scope.IT_TransferStore != "0") {
            GetProductList($scope.IT_TransferStore);
            var gettostorelist = $http.get("/ET_Purchase_Transfers/Bindtostores",
                {
                    params: { id: $scope.IT_TransferStore }
                });
            gettostorelist.then(function (tostorelist) {
                debugger;
                var res = JSON.parse(tostorelist.data);
                $scope.tostorelist = res;
                //$scope.IT_TransferToStore = "";
            }, function () {
                alert("No Data Found");
            });
        }
        else {
            $scope.IT_TransferToStore = "";
            $("#drptostore").val("").trigger("chosen:updated");}
    }

    $scope.tostorechange = function () {

    }
    //Get the Transfer List which are deleted
    function GetStoreRestoreList() {
        var getstorerestorelist = $http.get("/ET_Purchase_Transfers/GetITList",
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
    //Get the Products in from store
    function GetProductList(id) {
        var getproductlist = $http.get("/ET_Purchase_Transfers/GetProducts",
            {
                params:
                    {
                        FromStoreId:id
                    }
            });
        getproductlist.then(function (productlist) {
            debugger;
            var res = JSON.parse(productlist.data);
            $scope.products = res;
        }, function () {
            alert('Data not found');
        });
    } 
    //Add the new product
    $scope.addnewrow = function (a) {
        debugger;
        var e = a.target;
        var tr = $("#storedetailsbody").find("tr");
        var txt = "";
        for (var i = 0; i < tr.length; i++)
        {
            var td = tr[i].cells;
            var Product = $(td[1]).find("select").val();
            var Qty = $(td[4]).find("input").val();
            var availableQty = $(td[5]).find("input").val();
            
            if (Product == "")
            {
                message = "Select Product at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
            else if (Qty == "")
            {
                message = "Enter the Transfer Qty at row " + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }

            var qty1 = parseFloat(Qty);
            var qty2 = parseFloat(availableQty);
            if (qty1 > qty2)
            {
                message = "Transfer Qty cannot be greater than Available Qty" + (i + 1);
                toastr["error"](message, "Notification");
                txt = "asd";
            }
        }
        if (txt == "")
        {
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (tr.length + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.SD_Itemcode }}'>{{ product.SD_ItemDescription }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' style='text-align:right;' onchange='MoneyValidation(this);' id='txtPackage' class='form-control'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody')).append(temp);
        }
    }
    //delete the product
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
   //Show the transfer list
    $scope.showRecords = function () {
        $("#advancedusage").dataTable().fnDestroy();
        GetITList();
        $("#div_View").css("display", "block");
        $("#div_Restore").css("display", "none");
        $("#div_Edit").css("display", "none");
    }
    //Show the deleted records
    $scope.restoreRecords = function () {
        $("#advancedusageRestore").dataTable().fnDestroy();
        GetStoreRestoreList();
        $("#div_View").css("display", "none");
        $("#div_Restore").css("display", "block");
        $("#div_Edit").css("display", "none");
    }
    //Create new transfer
    $scope.createnew = function () {
        if ($scope.Iscreate) {
            debugger;
            $("#formSubmit").attr('disabled', "disabled");
            $("#div_View").css("display", "none");
            $("#div_Edit").css("display", "block");
            $scope.submittext = "Submit";
            $scope.createedit = "Create";
            $scope.IT_ID = "0";
            $scope.IT_Code = "";
            $scope.IT_TransferType = "";
            $scope.IT_GINo = "";
            $scope.IT_TransferStore = "";
            $scope.IT_TransferToStore = "";
            $scope.IT_TransferReceivedBy = "";
            $scope.IT_TransferNote = ""; 
            $("#txtTransferdate").val("");
            $("#drpGINo").val("").trigger("chosen:updated");
            $("#drptype").val("").trigger("chosen:updated");
            $("#drpStoreName").val("").trigger("chosen:updated");
            $("#drptostore").val("").trigger("chosen:updated");
            var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '1' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.SD_Itemcode }}'>{{ product.SD_ItemDescription }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
            var temp = $compile(Rowhtml)($scope);
            angular.element(document.getElementById('storedetailsbody')).html("");
            angular.element(document.getElementById('storedetailsbody')).append(temp);
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
            var product = $(td[1]).find("select").val();
            var qty = $(td[4]).find("input").val();
            var rate = $(td[5]).find("input").val();
            var qty1 = parseFloat(qty);
            var qty2 = parseFloat(rate);
            if (product == "") {
                message = "Enter Product Name at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            } else if (qty == "") {
                message = "Enter Transfer Qty at row:" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else if (qty1 > qty2)
            {
                message = "Transfer Qty cannot be greater than Available Qty" + (i + 1);
                toastr["error"](message, "Notification");
                return "";
            }
            else {
                txt = txt + $(td[1]).find("select").val() + ",";
                txt = txt + $(td[1]).find("select option:selected").text() + ",";
                txt = txt + $(td[2]).find("input").val() + ",";
                txt = txt + $(td[3]).find("input").val() + ",";
                txt = txt + parseFloat($(td[4]).find("input").val().split('.').join("").replace(",", ".")) + ",";
                txt = txt + parseFloat($(td[5]).find("input").val().split('.').join("").replace(",", ".")) + "|";
            }
        }
        return txt;
    }
    //Saving the Data
    $scope.SubmitClick = function () {
        debugger;
        $("#div_loadImage").css("display", "block");
        var txt = validate();
        var date = $("#txtTransferdate").val();
        debugger;
        if (txt != "") {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_Transfers/ET_Master_IT_Add",
                dataType: 'json',
                data: {
                    IT_ID: $scope.IT_ID,
                    IT_Code: $scope.IT_Code,
                    IT_TransferDate: $("#txtTransferdate").val(),
                    IT_TransferType: $scope.IT_TransferType,
                    IT_GINo: $scope.IT_GINo,
                    IT_TransferStore: $scope.IT_TransferStore,
                    IT_TransferToStore: $scope.IT_TransferToStore,
                    IT_TransferReceivedBy: $scope.IT_TransferReceivedBy,
                    IT_TransferNote: $scope.IT_TransferNote,
                    ITDetails: txt
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
                    if ($scope.IT_ID == 0) {
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
    //Edit the transfer Details
    $scope.editRecords = function (a) {
        a = parseInt(a);
        if ($scope.Isedit) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_Transfers/ET_Master_IT_Update_GetbyID",
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
                    var TransferID = JSON.parse(data);
                    $("#formSubmit").removeAttr('disabled');
                    $("#div_View").css("display", "none");
                    $("#div_Edit").css("display", "block");
                    $scope.submittext = "Update";
                    $scope.createedit = "Edit";
                    $("#span_tabtext").html("Edit");
                    debugger;
                    $scope.IT_ID = TransferID.IT_ID;
                    $scope.IT_Code = TransferID.IT_Code; 
                    $scope.IT_TransferType = TransferID.IT_TransferType;
                    $scope.IT_GINo = TransferID.IT_GINo;
                    $scope.IT_TransferStore = TransferID.IT_TransferFromStore;
                    $scope.IT_TransferToStore = TransferID.IT_TransferToStore;
                    $scope.IT_TransferReceivedBy = TransferID.IT_TransferReceivedBy;
                    $scope.IT_TransferNote = TransferID.IT_TransferNote; 
                    //var Quotationdate = new Date(parseInt(TransferID.IT_TransferDate.substr(6)));
                    //var QDT = ("0" + (Quotationdate.getMonth() + 1)).slice(-2) + "-" + ("0" + Quotationdate.getDate()).slice(-2) + "-" + Quotationdate.getFullYear();
                    //$("#txtTransferdate").val(QDT); 
                    var Quotationdate = new Date(parseInt(TransferID.IT_TransferDate.substr(6)));
                    var QDT = ("0" + Quotationdate.getDate()).slice(-2) + "-" + ("0" + (Quotationdate.getMonth() + 1)).slice(-2) + "-" + Quotationdate.getFullYear();
                    $("#txtTransferdate").val(QDT);
                    $("#drptype").val(TransferID.IT_TransferType).trigger("chosen:updated");
                    $("#drpGINo").val(TransferID.IT_GINo).trigger("chosen:updated");
                    $("#drpStoreName").val(TransferID.IT_TransferFromStore).trigger("chosen:updated");
                    $scope.UserChange();
                    $("#drptostore").val(TransferID.IT_TransferToStore).trigger("chosen:updated"); 
                    ITDetails(TransferID.IT_ID);
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
    //Get the transfer product details
    function ITDetails(e) {
        var id = e;
        $http({
            method: 'POST',
            url: '/ET_Purchase_Transfers/ET_Master_IT_Details_Load',
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
                var Rowhtml = "<tr><td><input readonly='readonly' type='number' value = '" + (j + 1) + "' id='txtSerial' class='form-control' /></td><td><select class='form-control' id='drpProduct' onchange='ProductChange(this);'><option value=''>-Select-</option><option ng-repeat='product in products' value='{{ product.SD_Itemcode }}'>{{ product.SD_ItemDescription }}</option ></select></td><td><input type='text' id='txtProductName' class='form-control' readonly='readonly' /></td><td><input type='text' id='txtUOM' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtPackage' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control'/></td><td><input type='text' id='txtQuantity' style='text-align:right;' onchange='MoneyValidation(this);' class='form-control' readonly='readonly' /></td><td><a style='padding: 5px;' ng-click='addnewrow($event)'><i class='fa fa-plus'></i></a><a style='padding: 5px;color:red' ng-click='deleterow($event)'><i class='fa fa-trash'></i></a></td></tr>";
                var temp = $compile(Rowhtml)($scope);
                angular.element(document.getElementById('storedetailsbody')).append(temp);
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
            url: "/ET_Purchase_Transfers/ET_Master_IT_RestoreDelete",
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
    //            url: "/ET_Purchase_Transfers/ET_Master_IT_RestoreDelete",
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
            alertmessageModified($event, a.toString(),b,'ET_Purchase_Transfers', 'ET_Master_IT_RestoreDelete');
        }
        else
        {
            $scope.PerformRestore(a, $event, b);
           // message = "You don't access to do this operation, please contact admin.";
            //toastr["error"](message, "Notification");
        }
    } 


    //View the transfer Details
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        debugger;
        if ($scope.Isview) {
            var post = $http({
                method: "POST",
                url: "/ET_Purchase_Transfers/ET_Purchase_IT_View",
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
                alert(data);
                $window.alert(data.Message);
            });
        }
        else {
            message = "You don't access to do this operation, please contact admin.";
            toastr["error"](message, "Notification");
        }
    }
    //Watch for all data binding
    $scope.$watch("storelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpStoreName").val($scope.IT_TransferStore).trigger("chosen:updated"); }, 100);
        }
    }); 
    $scope.$watch("tostorelist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drptostore").val($scope.IT_TransferToStore).trigger("chosen:updated"); }, 100);
        }
    }); 
    $scope.$watch("gilist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpGINo").val($scope.IT_GINo).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("itlist", function (value) {
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
                for (var i = 0; i < tr.length; i++) {
                    var td = tr[i].cells;
                    $(td[1]).find("select").val(enquirydata[i].productid);
                    $(td[2]).find("input").val(enquirydata[i].name);
                    $(td[3]).find("input").val(enquirydata[i].uom);
                    $(td[4]).find("input").val(parseFloat(enquirydata[i].transQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                    $(td[5]).find("input").val(parseFloat(enquirydata[i].balqty).toLocaleString("es-ES", { minimumFractionDigits: 2 }));
                }
            }, 1000);
        }
    });

});