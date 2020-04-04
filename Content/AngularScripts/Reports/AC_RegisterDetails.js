app1.controller("AC_RegisterDetailsCtrl", function ($scope, $http, $window, $rootScope) {
    $scope.ifcustomdate = "none";
    $scope.SelectedDuration = "0";
    $scope.SelectedSalesperson = "0";
    $scope.SelectedCustomer = "0";
    $scope.SelectedSupplier = "0";
    $scope.SelectedOredertype = "1";
    $scope.SelectedRegtype = "1";
    $("#div_treereportSale").html("");
    var content = "<tr><td colspan='7' style='text-align:center;color:red;'>Apply Filters</td></tr>";
    $("#div_treereportSale").html(content);
    calltreeSale();
    GetCustomerSupplierSalesperson();
    // Binding the Customer , Supplier , Sales Person Dropdowns
    function GetCustomerSupplierSalesperson() {
        $http({
            method: 'GET',
            url: '/RegisterDetails/GetCustomerSupplierSalesperson',
        }).success(function (data) {
            debugger;
            $scope.SupplierList = data.supplier;
            $scope.SalespersonList = data.salesperson;
            $scope.CustomerList = data.customer;
        });
    }
    //Get report Data
    $scope.GetAgencyShipmentDetails = function () {

        if ($scope.SelectedDuration == "7" && ($("#FromDate").val() == "" || $("#ToDate").val() == "")) {
            message = "Select From and To date";
            toastr["error"](message, "Notification");
        }
        else {
            var getAgencyShipmentDetails = $http.get("/RegisterDetails/GetAgencyShipmentDetails",
                {
                    params:
                        {
                            salesperson: $scope.SelectedSalesperson,
                            customer: $scope.SelectedCustomer,
                            supplier: $scope.SelectedSupplier,
                            duration: $scope.SelectedDuration,
                            fromdate: $("#FromDate").val(),
                            todate: $("#ToDate").val(),
                            Type: $scope.SelectedOredertype,
                            RegType: $scope.SelectedRegtype,
                        }
                });
            getAgencyShipmentDetails.then(function (res) {
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
                    $("#div_treereportSale").html("");
                    $("#div_treereportPurchase").html("");
                    if (result.length > 0) {
                        var content = "";
                        for (var i = 0; i < result.length; i++) {
                            if ($scope.SelectedOredertype == 3) {
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].InvNo + "</td>";
                                content = content + "<td>" + result[i].InvDate + "</td>";
                                //content = content + "<td>" + result[i].Product + "</td>";
                                //if (result[i].Quantity == "") {
                                //    content = content + "<td></td>";
                                //}
                                //else {
                                //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                //}
                                //if (result[i].Price == "") {
                                //    content = content + "<td></td>";
                                //}
                                //else {
                                //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                //}
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].InvAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "</tr>";
                            }
                            else {
                                if ($scope.SelectedRegtype == 1) {
                                    content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                    content = content + "<td>" + result[i].customerName + "</td>";
                                    content = content + "<td>" + result[i].InvNo + "</td>";
                                    content = content + "<td>" + result[i].InvDate + "</td>";
                                    //suguna
                                    //content = content + "<td>" + result[i].Product + "</td>";
                                    //if (result[i].Quantity == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //if (result[i].Price == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //-----
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].CommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    content = content + "</tr>";
                                }
                                if ($scope.SelectedRegtype == 2) {
                                    content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                    content = content + "<td>" + result[i].supplierName + "</td>";
                                    content = content + "<td>" + result[i].InvNo + "</td>";
                                    content = content + "<td>" + result[i].InvDate + "</td>";
                                    //suguna
                                    //content = content + "<td>" + result[i].Product + "</td>";
                                    //if (result[i].Quantity == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //if (result[i].Price == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //-----
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].CommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    content = content + "</tr>";
                                }
                               
                            }

                        }
                        if ($scope.SelectedOredertype == 3) {
                            $("#example-advanced1").show();
                            $("#example-advancedPurchase").hide();
                            $("#example-advancedSales").hide();
                            $("#div_treereport1").html(content);
                            calltree1();
                        }
                        else {
                            if ($scope.SelectedRegtype == 1) {
                                $("#example-advancedSales").show();
                                $("#example-advancedPurchase").hide();
                                $("#example-advanced1").hide();
                                $("#div_treereportSale").html(content);
                                calltreeSale();
                            }

                            if ($scope.SelectedRegtype == 2) {
                                $("#example-advancedSales").hide();
                                $("#example-advancedPurchase").show();
                                $("#example-advanced1").hide();
                                $("#div_treereportPurchase").html(content);
                                calltreePurchase();
                            }
                        }

                    } else {
                        var content = "<tr><td colspan='7' style='text-align:center;color:red;'>No Data Found</td></tr>";
                        $("#div_treereportPurchase").html(content);
                        $("#div_treereportSale").html(content);
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
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].InvNo + "</td>";
                                content = content + "<td>" + result[i].InvDate + "</td>";
                                //content = content + "<td>" + result[i].Product + "</td>";
                                //if (result[i].Quantity == "") {
                                //    content = content + "<td></td>";
                                //}
                                //else {
                                //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                //}
                                //if (result[i].Price == "") {
                                //    content = content + "<td></td>";
                                //}
                                //else {
                                //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                //}
                                
                                
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].InvAmount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint1").html(content);
                        } else {
                            var content = "<tr><td colspan='7' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint1").html(content);

                        }
                    }
                    else {
                        if ($scope.SelectedRegtype == 1) {
                            $("#div_treereportprintSale").html("");
                            $("#Print_Purchase").hide();
                            $("#Print_Sales").show();
                            if (result.length > 0) {
                                var content = "";
                                for (var i = 0; i < result.length; i++) {
                                    content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                    content = content + "<td>" + result[i].customerName + "</td>";
                                    content = content + "<td>" + result[i].InvNo + "</td>";
                                    content = content + "<td>" + result[i].InvDate + "</td>";
                                    //suguna
                                    //content = content + "<td>" + result[i].Product + "</td>";
                                    //if (result[i].Quantity == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //if (result[i].Price == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                  
                                    //-----
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].CommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    content = content + "</tr>";
                                }
                                $("#div_treereportprintSale").html(content);
                            } else {
                                var content = "<tr><td colspan='7' style='text-align:center;color:red;'>No Data Found</td></tr>";
                                $("#div_treereportprintSale").html(content);

                            }
                        }
                        if ($scope.SelectedRegtype == 2) {
                            $("#div_treereportprintPurchase").html("");
                            $("#Print_Sales").hide();
                            $("#Print_Purchase").show();
                            if (result.length > 0) {
                                var content = "";
                                for (var i = 0; i < result.length; i++) {
                                    content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                    content = content + "<td>" + result[i].supplierName + "</td>";
                                    content = content + "<td>" + result[i].InvNo + "</td>";
                                    content = content + "<td>" + result[i].InvDate + "</td>";
                                    //suguna
                                    //content = content + "<td>" + result[i].Product + "</td>";
                                    //if (result[i].Quantity == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //if (result[i].Price == "") {
                                    //    content = content + "<td></td>";
                                    //}
                                    //else {
                                    //    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    //}
                                    //-----
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].CommissionAmt).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                    content = content + "</tr>";
                                }
                                $("#div_treereportprintPurchase").html(content);
                            } else {
                                var content = "<tr><td colspan='7' style='text-align:center;color:red;'>No Data Found</td></tr>";
                                $("#div_treereportprintPurchase").html(content);

                            }
                        }
                       
                    }
                    $scope.SelectedCustomerName = $("#drpCustomer option:selected").text();
                    $scope.SelectedSupplierName = $("#drpSupplier option:selected").text();
                    $scope.SelectedSalespersonName = $("#drpSalesperson option:selected").text();
                    $scope.SelectedType = $("#drpOrdertype option:selected").text();
                    $scope.SelectedDurationtext = $("#drpDuration option:selected").text();
                    $scope.SelectedDurationVal = $("#drpDuration").val();
                    $scope.FromDateVal = $("#FromDate").val();
                    $scope.ToDateVal = $("#ToDate").val();

                }
            });
        }
    }
    $scope.SalespersonChange = function () {
    }
    $scope.Customerchange = function () {
      
    }
    $scope.Supplierchange = function () {
    }
    $scope.changeRegtyper = function () {
        //if ($scope.SelectedRegtype == 1) {

        //    $("#example-advancedSales").show();
        //    $("#example-advancedPurchase").hide();
        //}
        //debugger;
        //if ($scope.SelectedRegtype == 2) {
        //    $("#example-advancedSales").hide();
        //    $("#example-advancedPurchase").show();
        //}
    }
    $scope.Durationchange = function () {
        if ($scope.SelectedDuration != "7") {
            $scope.ifcustomdate = "none";
        }
        else {
            $scope.ifcustomdate = "block";
        }
        $("#FromDate").val("");
        $("#ToDate").val("");
    }
    $scope.$watch("SalespersonList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesperson").val($scope.SelectedSalesperson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("CustomerList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SupplierList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SelectedSalesperson", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesperson").val($scope.SelectedSalesperson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SelectedCustomer", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpCustomer").val($scope.SelectedCustomer).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SelectedSupplier", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSupplier").val($scope.SelectedSupplier).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("SelectedDuration", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpDuration").val($scope.SelectedDuration).trigger("chosen:updated"); }, 100);
        }
    });
    function calltreeSale() {
        debugger;
        $("#example-advancedSales").removeData("treetable").removeClass("treetable");
        $("#example-advancedSales").treetable({ expandable: true });
        $("#example-advancedSales").treetable({ expandable: true });

        // Highlight selected row
        $("#example-advancedSales tbody").on("mousedown", "tr", function () {
            $(".selected").not(this).removeClass("selected");
            $(this).toggleClass("selected");
        });

        // Drag & Drop Example Code
        $("#example-advancedSales .file, #example-advancedSales .folder").draggable({
            helper: "clone",
            opacity: .75,
            refreshPositions: true, // Performance?
            revert: "invalid",
            revertDuration: 300,
            scroll: true
        });

        $("#example-advancedSales .folder").each(function () {
            $(this).parents("#example-advancedSales tr").droppable({
                accept: ".file, .folder",
                drop: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    $("#example-advancedSales").treetable("move", droppedEl.data("ttId"), $(this).data("ttId"));
                },
                hoverClass: "accept",
                over: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    if (this != droppedEl[0] && !$(this).is(".expanded")) {
                        $("#example-advancedSales").treetable("expandNode", $(this).data("ttId"));
                    }
                }
            });
        });

        $("form#reveal").submit(function () {
            var nodeId = $("#revealNodeId").val()

            try {
                $("#example-advancedSales").treetable("reveal", nodeId);
             
            }
            catch (error) {
                alert(error.message);
            }

            return false;
        });
    }
    function calltreePurchase() {
        debugger;
        $("#example-advancedPurchase").removeData("treetable").removeClass("treetable");
        $("#example-advancedPurchase").treetable({ expandable: true });
        $("#example-advancedPurchase").treetable({ expandable: true });

        // Highlight selected row
        $("#example-advancedPurchase tbody").on("mousedown", "tr", function () {
            $(".selected").not(this).removeClass("selected");
            $(this).toggleClass("selected");
        });

        // Drag & Drop Example Code
        $("#example-advancedPurchase .file, #example-advancedPurchase .folder").draggable({
            helper: "clone",
            opacity: .75,
            refreshPositions: true, // Performance?
            revert: "invalid",
            revertDuration: 300,
            scroll: true
        });

        $("#example-advancedPurchase .folder").each(function () {
            $(this).parents("#example-advancedPurchase tr").droppable({
                accept: ".file, .folder",
                drop: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    $("#example-advancedPurchase").treetable("move", droppedEl.data("ttId"), $(this).data("ttId"));
                },
                hoverClass: "accept",
                over: function (e, ui) {
                    var droppedEl = ui.draggable.parents("tr");
                    if (this != droppedEl[0] && !$(this).is(".expanded")) {
                        $("#example-advancedPurchase").treetable("expandNode", $(this).data("ttId"));
                    }
                }
            });
        });

        $("form#reveal").submit(function () {
            var nodeId = $("#revealNodeId").val()

            try {
                $("#example-advancedPurchase").treetable("reveal", nodeId);
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