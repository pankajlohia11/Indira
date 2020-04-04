app1.controller("AC_DashboardCtrl", function ($scope, $http, $window, $rootScope) {
    var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var color = Chart.helpers.color;
    debugger;
   
    //alert("Dashboard");
    var current_date = new Date();

    month_value = current_date.getMonth();
    $("#Month").text(MONTHS[month_value]);
    $("#Day").text(current_date.getDate());
    CheckIfAdminUser();
    GetSalesPersonList();
    GetSalesPersonListForTrading();
    GetSalesPersonListForStore();
    GetMonthwiseSalesForAgency();
    SalespersonPieChart();
    GetTaskList();
    function GetTaskList() {

        var getshipmentlist = $http.get("/ET_TodoList/GetTaskList",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {
            debugger;
            var res = Quotationlist;
            if (Quotationlist.data.length > 0) {
                $("#TotalTask").text(Quotationlist.data.length);
            }
            
            for (i = 0; i < Quotationlist.data.length; i++) {
                var Quotationdate = new Date(parseInt(Quotationlist.data[i].Expec_Date.substr(6)));
                var QDT = Quotationdate.getHours() + ":" + Quotationdate.getMinutes();
                $(".todo-list").append('<li><div class="view"><label class="checkbox checkbox-custom m-0 text-muted inline" ><input type="checkbox" class="CheckingTodoList" ><i></i></label><span>' + Quotationlist.data[i].TSK_Title + ' </span><a role="button" tabindex="0" class="text-danger remove-todo pull-right"><i class="fa fa-times"></i> </a></div ></li>');
                $("#appointments-carousel").append('<div><p class="text-center text-strong"> <i class="fa fa-clock-o"></i> ' + QDT + '</p><p><h5 class="mt-10 mb-0 text-strong">' + Quotationlist.data[i].TSK_Title + '</h5><small class="text-thin text-transparent-white">' + Quotationlist.data[i].TSK_Desc + '</small></p></div >');

            }
            $('#appointments-carousel').owlCarousel({
                autoPlay: 5000,
                stopOnHover: true,
                slideSpeed: 300,
                paginationSpeed: 400,
                navigation: true,
                navigationText: ['<i class=\'fa fa-chevron-left\'></i>', '<i class=\'fa fa-chevron-right\'></i>'],
                singleItem: true
            });

        }, function () {

        });
    }

    function CheckIfAdminUser()
    {
        var adminPrivilege = $http.get("/ET_TaskStatus/CheckIfAdminUser");
        adminPrivilege.then(function (privilage1) {
            var privilage = JSON.parse(privilage1.data);
            //$scope.Iscreate = privilage[0].IS_ADD;
            //$scope.Isedit = privilage[0].IS_EDIT;
            //$scope.Isdelete = privilage[0].IS_DELETE;
            //$scope.Isrestore = privilage[0].IS_FULLCONTROL;
            //$scope.Isview = privilage[0].IS_VIEW;
            if (privilage)
            {
                //$(document).find("#adminStatus").prop('style', 'display:block');
                $('#navigation > li[id="adminStatus"]').prop('style', 'display:block');
                //alert("true value");
            }
            else
            {
                //alert("fasle");
                //$(document).find("#adminStatus").prop('style', 'display:none');
                $('#navigation > li[id="adminStatus"]').prop('style', 'display:none');
            }

        }, function () {
            //alert('Privilages Not Found');
        }
        );
    }

    //Scope function to Draw the Bar Chart for the Agency Sales(Previous & Current Year)
    $scope.SalesPersonChange = function () {
        $("#container").empty();
        $("#container").append('<canvas id="canvas"></canvas>');
        var currentYearData = "";
        var previousYearData = "";
        var currentYear = new Date().getFullYear();
        var previousYear = currentYear - 1;
        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetAgencySales',
            data: { "id": $scope.S_SalesPerson, "startYear": previousYear, "endYear": previousYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result)
            {
                previousYearData = result;
            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });

        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetAgencySales',
            data: { "id": $scope.S_SalesPerson, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                currentYearData = result;
                var dataForBar = [{
                    label: $("#drpSalesPerson option:selected").text().trim() + ":" + previousYear.toString(),
                    backgroundColor: "#66ccff",
                    borderColor: "#66ccff",
                    borderWidth: 1,
                    data: JSON.parse(previousYearData),
                },
                {
                    label: $("#drpSalesPerson option:selected").text().trim() + ":" + currentYear.toString(),
                    backgroundColor: "red",
                    borderColor: "red",
                    borderWidth: 1,
                    data: JSON.parse(currentYearData),
                }
                ];

                var barChartData = {
                    labels: MONTHS,
                    datasets: dataForBar
                };
                var ctx = document.getElementById('canvas').getContext('2d');
                window.myBar = new Chart(ctx, {
                    type: 'bar',
                    data: barChartData,
                    options: {
                        responsive: true,
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Agency(Salesperson wise)'
                        }
                    }
                });

            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
    };

    //Scope function to Draw the Bar Chart for the Trading Sales(Previous & Current Year)
    $scope.SalesPersonChangeForTrading = function () {
        debugger;
        $("#containerForTrading").empty();
        $("#containerForTrading").append('<canvas id="canvasForTrading"></canvas>');
        var currentYearData = "";
        var previousYearData = "";
        var currentYear = new Date().getFullYear();
        var previousYear = currentYear - 1;
        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetTradingSalesForBarchart',
            data: { "id": $scope.S_SalesPersonForTrading, "startYear": previousYear, "endYear": previousYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                previousYearData = result;
            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetTradingSalesForBarchart',
            data: { "id": $scope.S_SalesPersonForTrading, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                currentYearData = result;
                var dataForBar = [{
                    label: $("#drpSalesPersonForTrading option:selected").text().trim() + ":" + previousYear.toString(),
                    backgroundColor: "#66ccff",
                    borderColor: "#66ccff",
                    borderWidth: 1,
                    data: JSON.parse(previousYearData),
                },
                {
                    label: $("#drpSalesPersonForTrading option:selected").text().trim() + ":" + currentYear.toString(),
                    backgroundColor: "red",
                    borderColor: "red",
                    borderWidth: 1,
                    data: JSON.parse(currentYearData),
                }
                ];

                var barChartData = {
                    labels: MONTHS,
                    datasets: dataForBar
                };
                var ctx = document.getElementById('canvasForTrading').getContext('2d');
                window.myBar = new Chart(ctx, {
                    type: 'bar',
                    data: barChartData,
                    options: {
                        responsive: true,
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Trading(Salesperson wise)'
                        }
                    }
                });

            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
    };

    //Scope function to Draw the Bar Chart for the Store Sales(Previous & Current Year)
    //These Functions can be Generalized in the Future.
    $scope.SalesPersonChangeForStore = function () {
        $("#containerForStore").empty();
        $("#containerForStore").append('<canvas id="canvasForStore"></canvas>');
        var currentYearData = "";
        var previousYearData = "";
        var currentYear = new Date().getFullYear();
        var previousYear = currentYear - 1;
        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetStoreSalesForBarchart',
            data: { "id": $scope.S_SalesPersonForStore, "startYear": previousYear, "endYear": previousYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                previousYearData = result;
            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
        $.ajax({
            type: "GET",
            url: '/ET_Admin_Dashboard/GetStoreSalesForBarchart',
            data: { "id": $scope.S_SalesPersonForStore, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                currentYearData = result;
                var dataForBar = [{
                    label: $("#drpSalesPersonForStore option:selected").text().trim() + ":" + previousYear.toString(),
                    backgroundColor: "#66ccff",
                    borderColor: "#66ccff",
                    borderWidth: 1,
                    data: JSON.parse(previousYearData),
                },
                {
                    label: $("#drpSalesPersonForStore option:selected").text().trim() + ":" + currentYear.toString(),
                    backgroundColor: "red",
                    borderColor: "red",
                    borderWidth: 1,
                    data: JSON.parse(currentYearData),
                }
                ];

                var barChartData = {
                    labels: MONTHS,
                    datasets: dataForBar
                };
                var ctx = document.getElementById('canvasForStore').getContext('2d');
                window.myBar = new Chart(ctx, {
                    type: 'bar',
                    data: barChartData,
                    options: {
                        responsive: true,
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Store(Salesperson wise)'
                        }
                    }
                });

            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
    };

    function GetSalesPersonList() {
        var getsalespersonlist = $http.get("/ET_Admin_Dashboard/GetSalesPerson",
            {
                params: {
                    orgtype: 1
                }
            });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersons = res;
            $scope.S_SalesPerson = 0;
            $scope.SalesPersonChange();
        }, function () {
            alert('Data not found');
        });
    }
    function GetSalesPersonListForTrading() {
        var getsalespersonlist = $http.get("/ET_Admin_Dashboard/GetSalesPerson",
            {
                params: {
                    orgtype: 2
                }
            });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersonsForTrading = res;
            $scope.S_SalesPersonForTrading = 0;
            $scope.SalesPersonChangeForTrading();
        }, function () {
            alert('Data not found');
        });
    }
    function GetSalesPersonListForStore() {
        var getsalespersonlist = $http.get("/ET_Admin_Dashboard/GetSalesPerson",
            {
                params: {
                    orgtype: 3
                }
            });
        getsalespersonlist.then(function (salespersonlist) {
            var res = JSON.parse(salespersonlist.data);
            $scope.salespersonsForStore = res;
            $scope.S_SalesPersonForStore = 0;
            $scope.SalesPersonChangeForStore();
        }, function () {
            alert('Data not found');
        });
    }
    $scope.$watch("salespersons", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPerson").val($scope.S_SalesPerson).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersonsForTrading", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPersonForTrading").val($scope.S_SalesPersonForTrading).trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("salespersonsForStore", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSalesPersonForStore").val($scope.S_SalesPersonForStore).trigger("chosen:updated"); }, 100);
        }
    });
    function SalespersonPieChart() {
        var getAgencyShipmentDetails = $http.get("/ET_Admin_Dashboard/GetSalespersonPieChart");
        getAgencyShipmentDetails.then(function (res) {
            var data2 = JSON.parse(res.data.s5);
            var data3 = JSON.parse(res.data.s6);
            //$('#AgencyPie').sparkline(data2, {
            //    type: 'pie',
            //    width: 'auto',
            //    height: '250px',
            //    tooltipFormat: '{{offset:offset}} ({{percent.1}}%)',
            //    tooltipValueLookups: {
            //        'offset': data3
            //    },
            //    sliceColors: ['#22beef', '#a2d200', '#ffc100', '#ff4a43','#22beef', '#a2d200', '#ffc100', '#ff4a43']

            //});
            var bgColorsForCanvas = [];
            for (i = 0; i < data2.length; i++) {
                r = Math.floor(Math.random() * 100);
                g = Math.floor(Math.random() * 100);
                b = Math.floor(Math.random() * 100);
                v = Math.floor(Math.random() * 500);
                c = 'rgb(' + r + ', ' + g + ', ' + b + ')';
                h = 'rgb(' + (r + 20) + ', ' + (g + 20) + ', ' + (b + 20) + ')';
                bgColorsForCanvas.push(c);
            }

            var dataForPie = [{
                label: "Agency Sales",
                borderWidth: 1,
                data: data2,
                backgroundColor: bgColorsForCanvas
            }];
            var pieChartData =
            {
                labels: data3,
                datasets: dataForPie
            };
            var ctx = document.getElementById('AgencyPie').getContext('2d');
            window.myBar = new Chart(ctx, {
                type: 'pie',
                data: pieChartData,
                options:
                {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var allData = data.datasets[tooltipItem.datasetIndex].data;
                                var tooltipLabel = data.labels[tooltipItem.index];
                                var tooltipData = allData[tooltipItem.index];
                                var total = 0;
                                for (var i in allData) {
                                    total += parseFloat(allData[i]);
                                }
                                var tooltipPercentage = Math.round((tooltipData / total) * 100);
                                return tooltipLabel + ': ' + tooltipData + ' (' + tooltipPercentage + '%)';
                            }
                        }
                    }
                }
            });
            TradingSalespersonPieChart();
        });
    }
    function TradingSalespersonPieChart() {
        var getAgencyShipmentDetails = $http.get("/ET_Admin_Dashboard/GettradingSalespersonPiechart");
        getAgencyShipmentDetails.then(function (res) {
            var data2 = JSON.parse(res.data.s5);
            var data3 = JSON.parse(res.data.s6);
            //$('#TradingPie').sparkline(data2, {
            //    type: 'pie',
            //    width: 'auto',
            //    height: '250px',
            //    tooltipFormat: '{{offset:offset}} ({{percent.1}}%)',
            //    tooltipValueLookups: {
            //        'offset': data3
            //    },
            //    sliceColors: ['#22beef', '#a2d200', '#ffc100', '#ff4a43', '#22beef', '#a2d200', '#ffc100', '#ff4a43']

            //});
            var bgColorsForCanvas = [];
            for (i = 0; i < data2.length; i++)
            {
                r = Math.floor(Math.random() * 100);
                g = Math.floor(Math.random() * 100);
                b = Math.floor(Math.random() * 100);
                v = Math.floor(Math.random() * 500);
                c = 'rgb(' + r + ', ' + g + ', ' + b + ')';
                h = 'rgb(' + (r + 20) + ', ' + (g + 20) + ', ' + (b + 20) + ')';
                //alert(data2[i] + data3[i] + c + h);
                //dataForPie.push({
                //    data: data2[i],
                //    label: data3[i],
                //    color: c,
                //    highlight: h
                //});
                bgColorsForCanvas.push(c);
            }

            var dataForPie = [{
                label: "Trading Sales",
                borderWidth: 1,
                data: data2,
                backgroundColor: bgColorsForCanvas
            }];
            var pieChartData =
            {
                labels: data3,
                datasets: dataForPie
            };
            var ctx = document.getElementById('TradingPie').getContext('2d');
            window.myBar = new Chart(ctx, {
                type: 'pie',
                data: pieChartData,
                options:
                {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var allData = data.datasets[tooltipItem.datasetIndex].data;
                                var tooltipLabel = data.labels[tooltipItem.index];
                                var tooltipData = allData[tooltipItem.index];
                                var total = 0;
                                for (var i in allData) {
                                    total += parseFloat(allData[i]);
                                }
                                var tooltipPercentage = Math.round((tooltipData / total) * 100);
                                return tooltipLabel + ': ' + tooltipData + ' (' + tooltipPercentage + '%)';
                            }
                        }
                    }
                },
            });
            StoreSalespersonPieChart();
        });
    }
    function StoreSalespersonPieChart() {
        var getAgencyShipmentDetails = $http.get("/ET_Admin_Dashboard/GetStoreSalespersonPiechart");
        getAgencyShipmentDetails.then(function (res) {
            var data2 = JSON.parse(res.data.s5);
            var data3 = JSON.parse(res.data.s6);
            //$('#StorePie').sparkline(data2, {
            //    type: 'pie',
            //    width: 'auto',
            //    height: '250px',
            //    tooltipFormat: '{{offset:offset}} ({{percent.1}}%)',
            //    tooltipValueLookups: {
            //        'offset': data3
            //    },
            //    sliceColors: ['#22beef', '#a2d200', '#ffc100', '#ff4a43', '#22beef', '#a2d200', '#ffc100', '#ff4a43']

            //});
            var bgColorsForCanvas = [];
            for (i = 0; i < data2.length; i++) {
                r = Math.floor(Math.random() * 100);
                g = Math.floor(Math.random() * 100);
                b = Math.floor(Math.random() * 100);
                v = Math.floor(Math.random() * 500);
                c = 'rgb(' + r + ', ' + g + ', ' + b + ')';
                h = 'rgb(' + (r + 20) + ', ' + (g + 20) + ', ' + (b + 20) + ')';
                bgColorsForCanvas.push(c);
            }

            var dataForPie = [{
                label: "Store Sales",
                borderWidth: 1,
                data: data2,
                backgroundColor: bgColorsForCanvas
            }];
            var pieChartData =
            {
                labels: data3,
                datasets: dataForPie
            };
            var ctx = document.getElementById('StorePie').getContext('2d');
            window.myBar = new Chart(ctx, {
                type: 'pie',
                data: pieChartData,
                options:
                {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    tooltips: {
                        callbacks:
                        {
                            label: function (tooltipItem, data)
                            {
                                var allData = data.datasets[tooltipItem.datasetIndex].data;
                                var tooltipLabel = data.labels[tooltipItem.index];
                                var tooltipData = allData[tooltipItem.index];
                                var total = 0;
                                for (var i in allData) {
                                    total += parseFloat(allData[i]);
                                }
                                var tooltipPercentage = Math.round((tooltipData / total) * 100);
                                return tooltipLabel + ': ' + tooltipData + ' (' + tooltipPercentage + '%)';
                            }
                        }
                    }
                }
            });
        });
    }
    function GetMonthwiseSalesForAgency() {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_Dashboard/GetMonthwiseSalesForAgency",
            dataType: 'json',
            data: { id: 0 },
            headers: { "Content-Type": "application/json" }
        });
        post.success(function (data, status) {
            debugger;
            if (data.TradingsalesMonthWise.length>0) {
                $("#LblTradingMonthSale").text(data.TradingsalesMonthWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.TradingsalesYearWise.length > 0) {
                $("#LblTradingYearSale").text(data.TradingsalesYearWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.StoresalesYearWise.length > 0) {
                $("#LblStoresYearSale").text(data.StoresalesYearWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.StoresalesMonthWise.length > 0) {
                $("#LblStoresMonthSale").text(data.StoresalesMonthWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.TradingsalesMonthWise.length > 0) {
                $("#LblTradingMonthSale").text(data.TradingsalesMonthWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.AgencysalesMonthWise.length > 0) {
                $("#LblagencyMonthSale").text(data.AgencysalesMonthWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
            if (data.AgencysalesYearWise.length > 0) {
                $("#LblagencyYearSale").text(data.AgencysalesYearWise[0].CommissionAmt.toLocaleString("es-ES", { minimumFractionDigits: 2 }));
            }
                       
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }
   

}); 