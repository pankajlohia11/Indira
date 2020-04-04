app1.controller("AC_StockSummaryCtrl", function ($scope, $http, $window, $rootScope) {
    $scope.ifcustomdate = "none";
    $scope.SelectedProduct = "0";
    $scope.SelectedOredertype = "3";
    $("#div_treereport").html("");
    var content = "<tr><td colspan='12' style='text-align:center;color:red;'>Apply Filters</td></tr>";
    $("#div_treereport").html(content);
    GetProduct();
    // Binding the Customer , Supplier , Sales Person Dropdowns
    function GetProduct() {
        $http({
            method: 'GET',
            url: '/StockSummary/GetProductData',
        }).success(function (data) {
            debugger;
            $scope.ProductList = data.Products;
            $scope.StoreList = data.Stores;
            $scope.SelectedStoretype = "1";
        });
    }
    //Get report Data
    $scope.GetStockSummaryDetails = function () {

       
            debugger;
        var getStockSummaryDetails = $http.get("/StockSummary/GetStockSummaryDetails",
                {
                    params:
                        {
                            product: $scope.SelectedProduct,
                            Store: $scope.SelectedStoretype,
                            Type: $scope.SelectedOredertype,
                        }
                });
            getStockSummaryDetails.then(function (res) {
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
                                content = content + "<td>" + result[i].ProductCode + "</td>";
                                content = content + "<td>" + result[i].ProductDescription + "</td>";
                                content = content + "<td>" + result[i].UOM + "</td>";
                                content = content + "<td>" + result[i].OpnStockQty + "</td>";
                                content = content + "<td>" + result[i].ReceiptQty + "</td>";
                                content = content + "<td>" + result[i].SoldQty + "</td>";
                                content = content + "<td>" + result[i].TrnsfrQty + "</td>";
                                content = content + "<td>" + result[i].ClosingStockQty + "</td>";
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
                                content = content + "<td>" + result[i].ProductCode + "</td>";
                                content = content + "<td>" + result[i].ProductDescription + "</td>";
                                content = content + "<td>" + result[i].UOM + "</td>";
                                content = content + "<td>" + result[i].OpnStockQty + "</td>";
                                content = content + "<td>" + result[i].ReceiptQty + "</td>";
                                content = content + "<td>" + result[i].SoldQty + "</td>";
                                content = content + "<td>" + result[i].TrnsfrQty + "</td>";
                                content = content + "<td>" + result[i].ClosingStockQty + "</td>";
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint1").html(content);
                        } else {
                            var content = "<tr><td colspan='12' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint1").html(content);

                        }
                    }
                    $scope.SelectedProductName = $("#ssProduct option:selected").text();
                    $scope.SelectedType = $("#ssOrdertype option:selected").text();
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
    $scope.$watch("StoreList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpStoretype").val($scope.SelectedStoretype).trigger("chosen:updated"); }, 100);
        }
    });

    function calltree() {
        debugger;
        $("#example-advanced").removeData("treetable").removeClass("treetable");
        $("#example-advanced").treetable({ expandable: true });
        $("#example-advanced").treetable({ expandable: true });

        // Highlight selected row
        $("#example-advanced tbody").on("mousedown", "tr", function () {
            $(".selected").not(this).removeClass("selected");
            $(this).toggleClass("selected");
        });

        // Drag & Drop Example Code
        $("#example-advanced .file, #example-advanced .folder").draggable({
            helper: "clone",
            opacity: .75,
            refreshPositions: true, // Performance?
            revert: "invalid",
            revertDuration: 300,
            scroll: true
        });

        $("#example-advanced .folder").each(function () {
            $(this).parents("#example-advanced tr").droppable({
                accept: ".file, .folder",
                drop: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    $("#example-advanced").treetable("move", droppedEl.data("ttId"), $(this).data("ttId"));
                },
                hoverClass: "accept",
                over: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    if (this != droppedEl[0] && !$(this).is(".expanded")) {
                        $("#example-advanced").treetable("expandNode", $(this).data("ttId"));
                    }
                }
            });
        });

        $("form#reveal").submit(function () {
            var nodeId = $("#revealNodeId").val()

            try {
                $("#example-advanced").treetable("reveal", nodeId);
            }
            catch (error) {
                alert(error.message);
            }

            return false;
        });
    }
    function calltree1() {
        debugger;
        $("#example-advanced1").removeData("treetable").removeClass("treetable");
        $("#example-advanced1").treetable({ expandable: true });
        $("#example-advanced1").treetable({ expandable: true });

        // Highlight selected row
        $("#example-advanced1 tbody").on("mousedown", "tr", function () {
            $(".selected").not(this).removeClass("selected");
            $(this).toggleClass("selected");
        });

        // Drag & Drop Example Code
        $("#example-advanced1 .file, #example-advanced1 .folder").draggable({
            helper: "clone",
            opacity: .75,
            refreshPositions: true, // Performance?
            revert: "invalid",
            revertDuration: 300,
            scroll: true
        });

        $("#example-advanced1 .folder").each(function () {
            $(this).parents("#example-advanced1 tr").droppable({
                accept: ".file, .folder",
                drop: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    $("#example-advanced1").treetable("move", droppedEl.data("ttId"), $(this).data("ttId"));
                },
                hoverClass: "accept",
                over: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    if (this != droppedEl[0] && !$(this).is(".expanded")) {
                        $("#example-advanced1").treetable("expandNode", $(this).data("ttId"));
                    }
                }
            });
        });

        $("form#reveal").submit(function () {
            var nodeId = $("#revealNodeId").val()

            try {
                $("#example-advanced1").treetable("reveal", nodeId);
            }
            catch (error) {
                alert(error.message);
            }

            return false;
        });
    }
});