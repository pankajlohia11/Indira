app1.controller("AC_AllschduleDetailsCtrl", function ($scope, $http, $window, $rootScope) {
    $scope.ifcustomdate = "none";
    $scope.SelectedDuration = "0";
   
    $scope.SelectedOredertype = "1";
    $("#div_treereport").html("");
    var content = "<tr><td colspan='12' style='text-align:center;color:red;'>Apply Filters</td></tr>";
    $("#div_treereport").html(content);
    calltree();
   
    
    //Get report Data
    $scope.GetAgencyShipmentDetails = function () {

        if ($scope.SelectedDuration == "7" && ($("#FromDate").val() == "" || $("#ToDate").val() == "")) {
            debugger;
            message = "Select From and To date";
            toastr["error"](message, "Notification");
        }
        else {
            debugger;
            var getAgencyShipmentDetails = $http.get("/AllSchduleDetails/GetAgencyShipmentDetails",
                {
                    params:
                        {
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
                           
                            content = content + "<tr>";
                            content = content + "<td>" + result[i].SchduleNo + "</td>";
                            content = content + "<td>" + result[i].ScNo + "</td>";
                            content = content + "<td>" + result[i].ScDate + "</td>";
                            content = content + "<td>" + result[i].ETD + "</td>";
                            
                            content = content + "<td>" + result[i].Comm + "</td>";
                            //-----
                            content = content + "<td>" + result[i].ProductName + "</td>";
                            content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                            content = content + "<td style='text-align:right;'>" + parseFloat(result[i].BalanceQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                            content = content + "</tr>";
                           

                        }
                    
                            $("#example-advanced").show();
                            $("#example-advanced1").hide();
                            $("#div_treereport").html(content);
                            calltree();
                        

                    } else {
                        var content = "<tr><td colspan='12' style='text-align:center;color:red;'>No Data Found</td></tr>";
                        $("#div_treereport").html(content);
                        $("#div_treereport1").html(content);
                    }

                    //Print
                    
                   
                        $("#div_treereportprint").html("");
                        $("#Print_2").hide();
                        $("#Print_1").show();
                        if (result.length > 0) {
                            var content = "";
                            for (var i = 0; i < result.length; i++) {
                                content = content + "<tr >";
                                content = content + "<td>" + result[i].SchduleNo + "</td>";
                                content = content + "<td>" + result[i].ScNo + "</td>";
                                content = content + "<td>" + result[i].ScDate + "</td>";
                                content = content + "<td>" + result[i].ETD + "</td>";
                                
                                content = content + "<td>" + result[i].Comm + "</td>";
                                //-----
                                content = content + "<td>" + result[i].ProductName + "</td>";
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].Quantity).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "<td style='text-align:right;'>" + parseFloat(result[i].BalanceQty).toLocaleString("es-ES", { minimumFractionDigits: 2 }) + "</td>";
                                content = content + "</tr>";
                            }
                            $("#div_treereportprint").html(content);
                        } else {
                            var content = "<tr><td colspan='12' style='text-align:center;color:red;'>No Data Found</td></tr>";
                            $("#div_treereportprint").html(content);

                        
                    }
                   
                    $scope.SelectedType = $("#drpOrdertype option:selected").text();
                    $scope.SelectedDurationtext = $("#drpDuration option:selected").text();
                    $scope.SelectedDurationVal = $("#drpDuration").val();
                    $scope.FromDateVal = $("#FromDate").val();
                    $scope.ToDateVal = $("#ToDate").val();
                }
            });
        }
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