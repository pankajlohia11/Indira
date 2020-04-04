app1.controller("AC_CustomerModel", function ($scope, $http, $window, $compile) {
    var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var ProductsLabel = ['Yarn', 'Fabric', 'Finished Goods'];
    //Make sure to use color codes, instead of color name.
    $scope.colorsPie = ['#90EE90', '#FF6600', '#8080FF'];

    GetCustomerList();
    //Get customer list
    function GetCustomerList() {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_CustomerModule/GetCustomerList",
            dataType: 'json',
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
                var customerList = JSON.parse(data);
                $scope.customers = customerList;
            }
        });

        post.error(function (data, status) {
            $window.alert(data.Message);
        });
    }
    $scope.GetPendingShipmentDetails = function () {
        debugger;
        //Get Pending Shipment Details.
        $.ajax({
            type: "GET",
            url: "/ET_Admin_CustomerModule/GetPendingShipmentDetails",
            data: { "customer": $scope.E_CustomerID },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result,status) {
                var res = JSON.parse(result);
                //var rowlength = 1;
                //var i = res.length;
                //angular.element(document.getElementById('pendingshipmentlist')).html("");
                //while (i != 0)
                //{
                //    //var tr = $("#pendingshipmentlist").find("tr");
                //    //var rowlength = tr.length + 1;
                //    var Rowhtml = "<tr class='table-info'><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtshipmentcode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentType' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentduedate' class='form-control' readonly='readonly'/></td></tr>";
                //    var temp = $compile(Rowhtml)($scope);
                //    angular.element(document.getElementById('pendingshipmentlist')).append(temp);
                //    $(".chosen-select").chosen({ width: "95%" });
                //    i--;
                //    rowlength++;
                //}
                //$scope.ShipmentDetailsInfo = res
                $scope.ShipmentDetailsInfo = res;
            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
        //var post = $http({
        //    method: "POST",
        //    url: "/ET_Admin_CustomerModule/GetPendingShipmentDetails",
        //    dataType: 'json',
        //    data: { customer: $scope.E_CustomerID },
        //    headers: { "Content-Type": "application/json" }
        //});
        //post.success(function (result, status) {
        //    var res = JSON.parse(result);
        //    var rowlength = 1;
        //    var i = res.length;
        //    angular.element(document.getElementById('pendingshipmentlist')).html("");
        //    while (i != 0) {
        //        //var tr = $("#pendingshipmentlist").find("tr");
        //        //var rowlength = tr.length + 1;
        //        var Rowhtml = "<tr><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtshipmentcode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentType' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentduedate' class='form-control' readonly='readonly'/></td></tr>";
        //        var temp = $compile(Rowhtml)($scope);
        //        angular.element(document.getElementById('pendingshipmentlist')).append(temp);
        //        $(".chosen-select").chosen({ width: "95%" });
        //        i--;
        //        rowlength++;
        //    }
        //    //$scope.ShipmentDetailsInfo = res
        //    $scope.ShipmentDetailsInfo = res;
        //});
        //post.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
    };
    $scope.GetPendingInvoicelist = function ()
    {
        debugger;

        //Get Pending Shipment Details.
        $.ajax({
            type: "GET",
            url: "/ET_Admin_CustomerModule/GetPendingInvoicelist",
            data: { "customer": $scope.E_CustomerID },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result, status)
            {
                var res = JSON.parse(result);
                //var rowlength = 1;
                //var i = res.length;
                //angular.element(document.getElementById('PendingInvoicelist')).html("");
                //while (i != 0)
                //{
                //    //var tr = $("#pendingshipmentlist").find("tr");
                //    //var rowlength = tr.length + 1;
                //    var Rowhtml = "<tr class='table-info'><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtinvoicecode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtShipmentCode' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtinvoiceduedate' class='form-control' readonly='readonly'/></td></tr>";
                //    var temp = $compile(Rowhtml)($scope);
                //    angular.element(document.getElementById('PendingInvoicelist')).append(temp);
                //    $(".chosen-select").chosen({ width: "95%" });
                //    i--;
                //    rowlength++;
                //}
                //$scope.ShipmentDetailsInfo = res
                $scope.InvoiceDetailsInfo = res;
            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });
        //var post = $http({
        //    method: "POST",
        //    url: "/ET_Admin_CustomerModule/GetPendingInvoicelist",
        //    dataType: 'json',
        //    data: { customer: $scope.E_CustomerID },
        //    headers: { "Content-Type": "application/json" }
        //});
        //post.success(function (result, status) {
        //    //$scope.pendingInvoiceList = data;
        //    //alert($scope.pendingInvoiceList);
        //    var res = JSON.parse(result);
        //    var rowlength = 1;
        //    var i = res.length;
        //    angular.element(document.getElementById('PendingInvoicelist')).html("");
        //    while (i != 0) {
        //        //var tr = $("#pendingshipmentlist").find("tr");
        //        //var rowlength = tr.length + 1;
        //        var Rowhtml = "<tr><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtinvoicecode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtShipmentCode' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtinvoiceduedate' class='form-control' readonly='readonly'/></td></tr>";
        //        var temp = $compile(Rowhtml)($scope);
        //        angular.element(document.getElementById('PendingInvoicelist')).append(temp);
        //        $(".chosen-select").chosen({ width: "95%" });
        //        i--;
        //        rowlength++;
        //    }
        //    //$scope.ShipmentDetailsInfo = res
        //    $scope.InvoiceDetailsInfo = res;
        //});


        ////debugger;
        ////var tr = $("#pendingshipmentlist").find("tr");
        ////var rowlength = tr.length + 1;
        ////var Rowhtml = "<tr><td><input readonly='readonly' type='number' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td><input type='text' id='txtshipmentcode' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtshipmentduedate' class='form-control' readonly='readonly'/></td></tr>";
        ////var temp = $compile(Rowhtml)($scope);
        ////angular.element(document.getElementById('enquirydetailsbody')).append(temp);
        ////$(".chosen-select").chosen({ width: "95%" });

        //post.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
    };

    //Get the communication list
    $scope.GetCompanyCommunication = function () {
        var getcustomerinfo = $http.get("/ET_Admin_CustomerModule/GetCompanyCommList",
            {
                params: { id: $scope.E_CustomerID }
            });

        getcustomerinfo.then(function (CompCommList) {
            var commList = JSON.parse(CompCommList.data);
            $(" #Company_Comm_List").dataTable().fnDestroy();
            $scope.CompanyCommList = commList;
        }, function () {
            alert("No Data Found");
        });
    }
    //customer change call
    $scope.CustomerChange = function () {
        var getcustomerinfo = $http.get("/ET_Admin_CustomerModule/GetCustomerInfo",
            {
                params: { id: $scope.E_CustomerID }
            });
        getcustomerinfo.then(function (CustomerMaster) {
            debugger;
            $(" #cust_contact").dataTable().fnDestroy();
            $(" #Order_List").dataTable().fnDestroy();
            $(" #One_to_one").dataTable().fnDestroy();
            $(" #One_ToMany_List").dataTable().fnDestroy();
            $(" #Payment_List").dataTable().fnDestroy();
            $(" #ShipmentOne_ToMany_List").dataTable().fnDestroy();
            $(" #ShipmentOne_to_one").dataTable().fnDestroy();
            $(" #Schedule_List").dataTable().fnDestroy();
            $(" #Company_Comm_List").dataTable().fnDestroy();
            $(" #Shipment_Agency_List").dataTable().fnDestroy();
            $(" #pending_shipment").dataTable().fnDestroy();
            $(" #pending_invoice").dataTable().fnDestroy();
            //$("#advancedusage").dataTable().fnDestroy();
            //$("#PendingInvoicelist").dataTable.fnDestroy();
            var custMaster = JSON.parse(CustomerMaster.data.Result);
            var CompContacts = JSON.parse(CustomerMaster.data.Contact);
            var OrderLi = JSON.parse(CustomerMaster.data.Order);
            var OneToOneLi = JSON.parse(CustomerMaster.data.OneToOne);
            var OneToManyLi = JSON.parse(CustomerMaster.data.OneToMany);
            var PaymentLi = JSON.parse(CustomerMaster.data.Payment);
            var ScheduleLi = JSON.parse(CustomerMaster.data.Schedule);
            var ShipmentLi = JSON.parse(CustomerMaster.data.Shipment);
            var DespatchLi = JSON.parse(CustomerMaster.data.Despatch);
            var ShipmentAgencyLi = JSON.parse(CustomerMaster.data.ShipmentAgency);
            $scope.CustomerMasterList = custMaster;
            $scope.CompanyContactsList = CompContacts;
            $scope.OrderList = OrderLi;
            $scope.OneToOneList = OneToOneLi;
            $scope.OneToManyList = OneToManyLi;
            $scope.PaymentList = PaymentLi;
            $scope.ScheduleList = ScheduleLi;
            $scope.ShipmentList = ShipmentLi;
            $scope.DespatchList = DespatchLi;
            $scope.ShipmentAgencyList = ShipmentAgencyLi;
            $scope.GetCompanyCommunication();
            $scope.GetPendingShipmentDetails();
            $scope.GetPendingInvoicelist();
            $scope.GetCustomerSales();
            $scope.GetProductWiseSales();
            if (data.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(data);
                $("#spanErrMessage2").html(data);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
        }, function () {
            alert("No Data Found");
        });
    };

    $scope.GetProductWiseSales = function ()
    {
        $("#containerForProductSales").empty();
        $("#containerForProductSales").append('<canvas id="canvasForProductSales"></canvas>');
        var currentYearData = "";
        var currentYear = new Date().getFullYear();

        $.ajax({
            type: "GET",
            url: "/ET_Admin_CustomerModule/GetProductWiseSalesForBarchart",
            data: { "id": $scope.E_CustomerID, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                currentYearData = result;
                var ctx = document.getElementById('canvasForProductSales').getContext('2d');
                var dataForBar = [{
                    data: JSON.parse(currentYearData),
                    backgroundColor: ['#90EE90', '#FF6600', '#8080FF']
                }
                ];
                $scope.PieDataSetOverride = [{ yAxisID: 'y-axis-1' }];
                var barChartData =
                {
                    labels: ProductsLabel,
                    datasets: dataForBar
                };
                if (currentYearData.length <= 0) {
                    barChartData =
                        {
                            labels: ["No data"],
                            datasets: [{
                                labels: 'No data',
                                backgroundColor: ['#D3D3D3'],
                                data: [100]
                            }]
                        };
                }
                window.myBar = new Chart(ctx,
                    {
                        type: 'pie',
                        data: barChartData,
                        options: {
                            responsive: true,
                            legend: {
                                position: 'top',
                            },
                            title: {
                                display: true,
                                text: 'Product-Wise Sales'
                            },
                            tooltips:
                            {
                                mode: 'index',
                                intersect: false
                            },
                            scales:
                            {
                                //xAxes: [{
                                //    stacked: false,
                                //}],
                                yAxes: [{
                                    id: 'y-axis-1',
                                    type: 'linear',
                                    display: true,
                                    position: 'left'
                                }]
                            }
                        }
                    });
            //var productCurtain = document.getElementById("productCurtain");
            //productCurtain.parentElement.removeChild(productCurtain);

            },
            error: function (response) {
                $window.alert(response.Message);
            }
        });

        //var post = $http(
        //    {
        //        method: "POST",
        //        url: "/ET_Admin_CustomerModule/GetProductWiseSalesForBarchart",
        //        dataType: 'json',
        //        data: { id: $scope.E_CustomerID, startYear: currentYear, endYear: currentYear },
        //        headers: { "Content-Type": "application/json" }
        //    });
        //post.success(function (data, status) {
        //    currentYearData = data;
        //    var ctx = document.getElementById('canvasForProductSales').getContext('2d');
        //    var dataForBar = [{
        //        data: JSON.parse(currentYearData),
        //        backgroundColor: ['#90EE90', '#FF6600', '#8080FF']
        //    }
        //    ];
        //    $scope.PieDataSetOverride = [{ yAxisID: 'y-axis-1' }];
        //    var barChartData =
        //    {
        //        labels: ProductsLabel,
        //        datasets: dataForBar
        //    };
        //    window.myBar = new Chart(ctx,
        //        {
        //            type: 'pie',
        //            data: barChartData,
        //            options: {
        //                responsive: true,
        //                legend: {
        //                    position: 'top',
        //                },
        //                title: {
        //                    display: true,
        //                    text: 'Product-Wise Sales'
        //                },
        //                tooltips:
        //                {
        //                    mode: 'index',
        //                    intersect: false
        //                },
        //                scales:
        //                {
        //                    //xAxes: [{
        //                    //    stacked: false,
        //                    //}],
        //                    yAxes: [{
        //                        id: 'y-axis-1',
        //                        type: 'linear',
        //                        display: true,
        //                        position: 'left'
        //                    }]
        //                }
        //            }
        //        });
        //    //var productCurtain = document.getElementById("productCurtain");
        //    //productCurtain.parentElement.removeChild(productCurtain);

        //});
        //post.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
    };

    $scope.GetCustomerSales = function ()
    {
        $("#containerForTrading").empty();
        $("#containerForTrading").append('<canvas id="canvasForTrading"></canvas>');
        var currentYearData = "";
        var previousYearData = "";
        var currentYear = new Date().getFullYear();
        var previousYear = currentYear - 1;

        $.ajax({
            type: "GET",
            url: "/ET_Admin_CustomerModule/GetCustomerSalesForBarchart",
            data: { "id": $scope.E_CustomerID, "startYear": previousYear, "endYear": previousYear },
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
            url: '/ET_Admin_CustomerModule/GetCustomerSalesForBarchart',
            data: { "id": $scope.E_CustomerID, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result)
            {
                currentYearData = result;
                var dataForBar = [{
                    label: $("#drpCustomer option:selected").text().trim() + ":" + previousYear.toString(),
                    backgroundColor: "#66ccff",
                    borderColor: "#66ccff",
                    borderWidth: 1,
                    data: JSON.parse(previousYearData),
                },
                {
                    label: $("#drpCustomer option:selected").text().trim() + ":" + currentYear.toString(),
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
                if (currentYearData.length <= 0) {
                    barChartData =
                        {
                            labels: ["No data"],
                            datasets: [{
                                labels: 'No data',
                                backgroundColor: ['#D3D3D3'],
                                data: [100]
                            }]
                        };
                }

                var ctx = document.getElementById('canvasForTrading').getContext('2d');
                window.myBar = new Chart(ctx,
                    {
                        type: 'bar',
                        data: barChartData,
                        options: {
                            responsive: true,
                            legend: {
                                position: 'top',
                            },
                            title: {
                                display: true,
                                text: 'Customer Sales'
                            },
                            tooltips:
                            {
                                mode: 'index',
                                intersect: false
                            },
                            scales:
                            {
                                xAxes: [{
                                    stacked: false,
                                }],
                                yAxes: [{
                                    stacked: false
                                }]
                            }
                        }
                    });

                //var customerCurtain = document.getElementById("customerCurtain");
                //customerCurtain.parentElement.removeChild(customerCurtain);

            },
            error: function (response) {
                $window.alert(response.Message);
            }
            });

        //var post1 = $http({
        //    method: "POST",
        //    url: "/ET_Admin_CustomerModule/GetCustomerSalesForBarchart",
        //    dataType: 'json',
        //    data: { id: $scope.E_CustomerID, startYear: currentYear, endYear:currentYear},
        //    headers: { "Content-Type": "application/json" }
        //});
        //post1.success(function (data, status) {
        //    currentYearData = data;
        //    var dataForBar = [{
        //        label: $("#drpCustomer option:selected").text().trim() + ":" + previousYear.toString(),
        //        backgroundColor: "#66ccff",
        //        borderColor: "#66ccff",
        //        borderWidth: 1,
        //        data: JSON.parse(previousYearData),
        //    },
        //    {
        //        label: $("#drpCustomer option:selected").text().trim() + ":" + currentYear.toString(),
        //        backgroundColor: "red",
        //        borderColor: "red",
        //        borderWidth: 1,
        //        data: JSON.parse(currentYearData),
        //    }
        //    ];

        //    var barChartData = {
        //        labels: MONTHS,
        //        datasets: dataForBar
        //    };

        //    var ctx = document.getElementById('canvasForTrading').getContext('2d');
        //    window.myBar = new Chart(ctx,
        //        {
        //            type: 'bar',
        //            data: barChartData,
        //            options: {
        //                responsive: true,
        //                legend: {
        //                    position: 'top',
        //                },
        //                title: {
        //                    display: true,
        //                    text: 'Customer Sales'
        //                },
        //                tooltips:
        //                {
        //                    mode: 'index',
        //                    intersect: false
        //                },
        //                scales:
        //                {
        //                    xAxes: [{
        //                        stacked: false,
        //                    }],
        //                    yAxes: [{
        //                        stacked: false
        //                    }]
        //                }
        //            }
        //        });

        //    var customerCurtain = document.getElementById("customerCurtain");
        //    customerCurtain.parentElement.removeChild(customerCurtain);
        //    //if (ctx.data.datasets[0].data.length === 0) {
        //    //    // No data is present
        //    //    var ctx1 = ctx.chart.ctx;
        //    //    var width = ctx.chart.width;
        //    //    var height = ctx.chart.height;
        //    //    ctx.clear();

        //    //    ctx1.save();
        //    //    ctx1.textAlign = 'center';
        //    //    ctx1.textBaseline = 'middle';
        //    //    ctx1.font = "16px normal 'Helvetica Nueue'";
        //    //    // chart.options.title.text <=== gets title from chart 
        //    //    // width / 2 <=== centers title on canvas 
        //    //    // 18 <=== aligns text 18 pixels from top, just like Chart.js 
        //    //    ctx1.fillText('My Chart Title', width / 2, 18); // <====   ADDS TITLE
        //    //    ctx1.fillText('No data to display for selected time period', width / 2, height / 2);
        //    //    ctx1.restore();
        //    //}

        //    //var ctx1 = document.getElementById('containerForProductSales').getContext('2d');
        //    //window.myBar = new Chart(ctx1,
        //    //    {
        //    //        type: 'bar',
        //    //        data: barChartData,
        //    //        options: {
        //    //            responsive: true,
        //    //            legend: {
        //    //                position: 'top',
        //    //            },
        //    //            title: {
        //    //                display: true,
        //    //                text: 'Product Wise Sales'
        //    //            }
        //    //        }
        //    //    });
        //});
        //post1.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
    };

    //View for one to many
    $scope.ViewOneToMany = function (a) {
        a = parseInt(a);
        {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToManyInvoice/ET_Sales_Invoice_View",
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
        
    }
    //View one to one
    $scope.ViewOneToOne = function (a) {
        a = parseInt(a);
         {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OneToOneInvoice/ET_Sales_Invoice_View",
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
        
    }
    //View schdule
    $scope.ViewSchedule = function (a) {
        
        {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shedule/ET_Sales_Schedule_View",
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
        
    }
    //View shipment
    $scope.ViewShipment = function (a) {
        a = parseInt(a);
         {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shipment/ET_Sales_Shipment_View",
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
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
       
    }
    //View orders
    $scope.ViewOrders = function (a) {
        a = parseInt(a);
       {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_OrderDetails/ET_Sales_OrderDetails_View",
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
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }
    }
    //View  shipment
    $scope.ViewDespatch = function (a) {
        a = parseInt(a);
        {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Despatch/ET_Sales_Despatch_View",
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
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }

    }
    $scope.ViewRecords = function (a) {
        a = parseInt(a);
        {
            var post = $http({
                method: "POST",
                url: "/ET_Sales_Shipment/ET_Sales_Shipment_View",
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
                }
            });

            post.error(function (data, status) {
                $window.alert(data.Message);
            });
        }

    };

    //Watch for all data binding
    $scope.$watch("customers", function (value)
    {
        var val = value || null;
        if (val)
        {
            setTimeout(function () { $("#drpCustomer").val("").trigger("chosen:updated"); }, 100);
            //dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            //dynamicDataTable('#PendingInvoicelist', '#tableTools', '#colVis');
            //setTimeout(function ()
            //{
                
            //    //$('#advancedusage').dataTable({
            //    //    "paging": false,
            //    //    "ordering": false,
            //    //    "info": false,
            //    //    "searching": false
            //    //});
            //}, 5);
            //setTimeout(function ()
            //{
            //    //$('#PendingInvoicelist').dataTable({
            //    //    "paging": false,
            //    //    "ordering": false,
            //    //    "info": false,
            //    //    "searching":false
            //    //});
               
            //}, 5);
        }
    });

    $scope.$watch("ShipmentDetailsInfo", function (value)
    {
        var val = value || null;
        if (val)
        {
            setTimeout(function () {
                dynamicDataTable1('#pending_shipment');
            }, 5);
        }
    }
    );

    $scope.$watch("InvoiceDetailsInfo", function (value) {
        var val = value || null;
        if (val)
        {
            setTimeout(function () {
                dynamicDataTable1('#pending_invoice');
            }, 5);
        }
    });

    function formatDate(timestamp)
    {
        var x = new Date(timestamp);
        var dd = x.getDate();
        var mm = x.getMonth() + 1;
        var yy = x.getFullYear();
        return dd + "/" + mm + "/" + yy;
    }

    //$scope.$watch("customers", function (value) {
    //    debugger;
    //    var val = value || null;
    //    if (val) {
    //        setTimeout(function () { $("#drpCustomer").val("").trigger("chosen:updated"); }, 100);
    //        setTimeout(function () {
    //            dynamicDataTable('#pendingshipmentlist', '#tableTools', '#colVis');
    //        }, 5);
    //    }
    //});
    $scope.$watch("CompanyContactsList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#cust_contact');
            }, 5);
        }
    }); 
    $scope.$watch("OrderList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#Order_List');
            }, 5);
        }
    });
    $scope.$watch("OneToOneList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#One_to_one');
            }, 5);
        }
    });
    $scope.$watch("OneToManyList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#One_ToMany_List');
            }, 5);
        }
    });
    $scope.$watch("ShipmentAgencyList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#Shipment_Agency_List');
            }, 5);
        }
    });
    $scope.$watch("PaymentList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#Payment_List');
            }, 5);
        }
    });
    $scope.$watch("ShipmentList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#ShipmentOne_to_one');
            }, 5);
        }
    });
    $scope.$watch("DespatchList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#ShipmentOne_ToMany_List');
            }, 5);
        }
    });
    $scope.$watch("ScheduleList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#Schedule_List');
            }, 5);
        }
    });
    $scope.$watch("CompanyCommList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#Company_Comm_List');
            }, 5);
        }
    });
});