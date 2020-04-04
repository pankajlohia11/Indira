app1.controller("AC_CheckStockQtyofanItemCtrl", function ($scope, $http, $window, $rootScope) {
    $scope.SelectedProduct = "0";
    $("#div_treereport").html("");
    GetProduct();
    // Binding the Customer , Supplier , Sales Person Dropdowns
    function GetProduct() {
        debugger;
        $http({
            method: 'GET',
            url: '/CheckStockQtyofanItem/GetProductData',
        }).success(function (data) {
            debugger;
            $scope.ProductList = data.Products;
        });
    }
    //Get report Data
    $scope.GetStockQtyofanItemDetails = function () {
            debugger;
        var getPoStatusDetails = $http.get("/CheckStockQtyofanItem/CheckStockQtyofanItem",
                {
                    params:
                        {
                            product: $scope.SelectedProduct,
                        }
                });
        GetStockQtyofanItemDetails.then(function (res) {
                debugger;
                if (res.data.indexOf("Validation") > -1) {
                    toastr["error"](res.data, "Notification");
                }
                else if (res.data.indexOf("ERR") > -1) {
                    $("#spanErrMessage1").html(res.data);
                    $("#spanErrMessage2").html(res.data);
                    if ($("#exceptionmessage").length)
                        $("#exceptionmessage").trigger("click");
                }
                else {
                    var result = JSON.parse(res.data);
                    $("#div_treereport").html("");
                    if (result.length > 0) {
                        var content = "";
                        for (var i = 0; i < result.length; i++) {
                            if ($scope.SelectedOredertype == 3) {
                                content = content + "<tr>";
                                content = content + "<td>" + result[i].Date + "</td>";
                                content = content + "<td>" + result[i].TrnType + "</td>";
                                content = content + "<td>" + result[i].PartyName + "</td>";
                                content = content + "<td>" + result[i].Quantity + "</td>";
                                content = content + "<td>" + result[i].UOM + "</td>";
                                content = content + "<td>" + result[i].OpeningQty + "</td>";
                                content = content + "<td>" + result[i].ClosingQty + "</td>";
                                content = content + "</tr>";
                            }
                        }
                        if ($scope.SelectedOredertype == 3) {
                            $("#example-advanced1").show();
                            $("#example-advanced").hide();
                            $("#div_treereport1").html(content);
                        }

                    } else {
                        var content = "<tr><td colspan='12' style='text-align:center;color:red;'>No Data Found</td></tr>";
                        $("#div_treereport").html(content);
                        $("#div_treereport1").html(content);
                    }

                    //Print
                    if ($scope.SelectedOredertype == 3) {
                        $("#div_treereportprint1").html("");
                        $("#Print_1").hide();
                        $("#Print_2").show();
                        if (result.length > 0) {
                            var content = "";
                            for (var i = 0; i < result.length; i++) {
                                content = content + "<tr>";
                                content = content + "<td>" + result[i].Date + "</td>";
                                content = content + "<td>" + result[i].TrnType + "</td>";
                                content = content + "<td>" + result[i].PartyName + "</td>";
                                content = content + "<td>" + result[i].Quantity + "</td>";
                                content = content + "<td>" + result[i].UOM + "</td>";
                                content = content + "<td>" + result[i].OpeningQty + "</td>";
                                content = content + "<td>" + result[i].ClosingQty + "</td>";
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint1").html(content);
                        } else {
                            var content = "<tr><td colspan='12' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint1").html(content);

                        }
                    }
                    $scope.SelectedProductName = $("#ssProduct option:selected").text();
                }
            });
        
    }

    $scope.Productchange = function () {
    }
    $scope.$watch("ProductList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#ssProduct").val($scope.SelectedProduct).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SelectedProduct", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#ssProduct").val($scope.SelectedProduct).trigger("chosen:updated"); }, 100);
        }
    });
    
});