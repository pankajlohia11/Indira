app1.controller("AC_BookingDetailsCtrl", function ($scope, $http, $window, $rootScope) {
    debugger;
    $scope.ifcustomdate = "none";
    $scope.SelectedDuration = "0";
    $scope.SelectedSalesperson = "0";
    $scope.SelectedCustomer = "0";
    $scope.SelectedSupplier = "0";
    $scope.SelectedOredertype = "1";
    $("#div_treereport").html("");
    var content = "<tr><td colspan='20' style='text-align:center;color:red;'>Apply Filters</td></tr>";
    $("#div_treereport").html(content);
    calltree();
    debugger;
    GetCustomerSupplierSalesperson();
    // Binding the Customer , Supplier , Sales Person Dropdowns
    function GetCustomerSupplierSalesperson() {
        $http({
            method: 'GET',
            url: '/BookingDetails/GetCustomerSupplierSalesperson',
        }).success(function (data) {
            debugger;
            $scope.SupplierList = data.supplier;
            $scope.SalespersonList = data.salesperson;
            $scope.CustomerList = data.customer;
        });
    }
    //Get report Data
    $scope.GetAgencyShipmentDetails = function () {
        debugger;
        if ($scope.SelectedDuration == "7" && ($("#FromDate").val() == "" || $("#ToDate").val() == "")) {
            message = "Select From and To date";
            toastr["error"](message, "Notification");
        }
        else {
            var getAgencyShipmentDetails = $http.get("/BookingDetails/GetAgencyShipmentDetails",
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
                    $("#div_treereport").html("");
                    if (result.length > 0) {
                        var content = "";
                        for (var i = 0; i < result.length; i++) {
                            debugger;
                            if ($scope.SelectedOredertype == 3) {
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].salespersonName + "</td>";
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].PoNo + "</td>";
                                content = content + "<td>" + result[i].PoDate + "</td>";
                                content = content + "<td>" + result[i].Product + "</td>";
                                content = content + "<td>" + result[i].Uom + "</td>";
                                content = content + "<td>" + result[i].Currency + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }
                                
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Qty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Amount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }
                                
                                content = content + "<td>" + result[i].Comm + "</td>";
                                content = content + "</tr>";
                            }
                            else {
                                debugger;
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].salespersonName + "</td>";
                                //suguna
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].supplierName + "</td>";
                                content = content + "<td>" + result[i].ScNo + "</td>";
                                content = content + "<td>" + result[i].Sc_Date + "</td>";
                                content = content + "<td>" + result[i].Po_No + "</td>";
                                content = content + "<td>" + result[i].PoDate + "</td>";
                                //content = content + "<td>" + result[i].DealDate + "</td>";
                                //content = content + "<td>" + result[i].Deal_No + "</td>";
                                content = content + "<td>" + result[i].Product + "</td>";
                                content = content + "<td>" + result[i].Uom + "</td>";
                                content = content + "<td>" + result[i].Currency + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Qty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Amount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "<td>" + result[i].Comm + "</td>";
                                //content = content + "<td>" + result[i].Comm_Amt + "</td>";
                                //content = content + "<td>" + result[i].Comm_Euro + "</td>";
                                //-------
                                content = content + "</tr>";
                            }

                        }
                        if ($scope.SelectedOredertype == 3) {
                            $("#example-advanced1").show();
                            $("#example-advanced").hide();
                            $("#div_treereport1").html(content);
                            calltree1();
                        }
                        else {
                            $("#example-advanced").show();
                            $("#example-advanced1").hide();
                            $("#div_treereport").html(content);
                            calltree();
                        }

                    } else {
                        var content = "<tr><td colspan='20' style='text-align:center;color:red;'>No Data Found</td></tr>";
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
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].salespersonName + "</td>";
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].PoNo + "</td>";
                                content = content + "<td>" + result[i].PoDate + "</td>";
                                content = content + "<td>" + result[i].Product + "</td>";
                                content = content + "<td>" + result[i].Uom + "</td>";
                                content = content + "<td>" + result[i].Currency + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }

                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Qty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Amount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }

                                content = content + "<td>" + result[i].Comm + "</td>";
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint1").html(content);
                        } else {
                            var content = "<tr><td colspan='20' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint1").html(content);

                        }
                    }
                    else {
                        $("#div_treereportprint").html("");
                        $("#Print_2").hide();
                        $("#Print_1").show();
                        if (result.length > 0) {
                            var content = "";
                            for (var i = 0; i < result.length; i++) {
                                content = content + "<tr data-tt-id='" + result[i].ID + "' data-tt-parent-id=" + result[i].PID + ">";
                                content = content + "<td>" + result[i].salespersonName + "</td>";
                                //suguna
                                content = content + "<td>" + result[i].customerName + "</td>";
                                content = content + "<td>" + result[i].supplierName + "</td>";
                                content = content + "<td>" + result[i].ScNo + "</td>";
                                content = content + "<td>" + result[i].Sc_Date + "</td>";
                                content = content + "<td>" + result[i].Po_No + "</td>";
                                content = content + "<td>" + result[i].PoDate + "</td>";
                                //content = content + "<td>" + result[i].DealDate + "</td>";
                                //content = content + "<td>" + result[i].Deal_No + "</td>";
                                content = content + "<td>" + result[i].Product + "</td>";
                                content = content + "<td>" + result[i].Uom + "</td>";
                                content = content + "<td>" + result[i].Currency + "</td>";
                                if (result[i].Price == "") {
                                    content = content + "<td style='text-align:right;'></td>";
                                }
                                else {
                                    content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Price).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                }
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Qty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Amount).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "<td>" + result[i].Comm + "</td>";
                                //content = content + "<td>" + result[i].Comm_Amt + "</td>";
                                //content = content + "<td>" + result[i].Comm_Euro + "</td>";
                                //-------
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint").html(content);
                        } else {
                            var content = "<tr><td colspan='8' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint").html(content);

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