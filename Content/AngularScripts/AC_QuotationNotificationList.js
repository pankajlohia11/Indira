app1.controller("AC_QuotationNotificationListCtrl", function ($scope, $http, $window, $compile) {
    GetPrivilages();
    GetEnquiryList();

    function GetPrivilages() {
        $scope.D_OrderID = "";
        var getprivilages = $http.get("/ET_Sales_Despatch/GetPrivilages");
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
    function GetEnquiryList() {
        var getshipmentlist = $http.get("/ET_Sales_Quotation/GetQuotationNotinOrderList",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {
            debugger;
            var res = JSON.parse(Quotationlist.data);
            $scope.quotationlist = res;
            

        }, function () {

        });  
    }
    $scope.ViewRecords = function (a) {
        debugger;
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
    $scope.$watch("quotationlist", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            }, 5);
        }
    });

});