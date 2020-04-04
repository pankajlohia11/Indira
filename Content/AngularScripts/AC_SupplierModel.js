app1.controller("AC_SupplierModel", function ($scope, $http, $window, $compile) {
    var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var ProductsLabel = ['Yarn', 'Fabric', 'Finished Goods'];
    //Make sure to use color codes, instead of color name.
    $scope.colorsPie = ['#90EE90', '#FF6600', '#8080FF'];

    GetSupplierList();
    function GetSupplierList() {
        var post = $http({
            method: "POST",
            url: "/ET_Admin_SupplierModule/GetSupplierList",
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
                
                var supplierList = JSON.parse(data);
                $scope.suppliers = supplierList;
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
            url: "/ET_Admin_SupplierModule/GetPendingShipmentDetails",
            data: { "supplier": $scope.E_SupplierID },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result, status) {
                var res = JSON.parse(result);
                //var rowlength = 1;
                //var i = res.length;
                //angular.element(document.getElementById('pendingshipmentlist')).html("");
                //while (i != 0) {
                //    //var tr = $("#pendingshipmentlist").find("tr");
                //    //var rowlength = tr.length + 1;
                //    var Rowhtml = "<tr><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtshipmentcode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentType' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtshipmentduedate' class='form-control' readonly='readonly'/></td></tr>";
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
    };
    $scope.GetPendingInvoicelist = function () {
        debugger;

        //Get Pending Shipment Details.
        $.ajax({
            type: "GET",
            url: "/ET_Admin_SupplierModule/GetPendingInvoicelist",
            data: { "supplier": $scope.E_SupplierID },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result, status) {
                var res = JSON.parse(result);
                //var rowlength = 1;
                //var i = res.length;
                //angular.element(document.getElementById('PendingInvoicelist')).html("");
                //while (i != 0) {
                //    //var tr = $("#pendingshipmentlist").find("tr");
                //    //var rowlength = tr.length + 1;
                //    var Rowhtml = "<tr><td width='5%'><input readonly='readonly' type='text' id='txtSerial' value='" + rowlength + "' class='form-control' /></td><td width='20%'><input type='text' id='txtinvoicecode' class='form-control' readonly='readonly'/></td><td width='20%'><input type='text' id='txtShipmentCode' class='form-control' readonly='readonly'/></td><td><input type='text' id='txtinvoiceduedate' class='form-control' readonly='readonly'/></td></tr>";
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
    };

    $scope.GetProductWiseSales = function ()
    {
        $("#containerForProductSales").empty();
        $("#containerForProductSales").append('<canvas id="canvasForProductSales"></canvas>');
        var currentYearData = "";
        var currentYear = new Date().getFullYear();

        $.ajax({
            type: "GET",
            url: "/ET_Admin_SupplierModule/GetProductWiseSalesForBarchart",
            data: { "id": $scope.E_SupplierID, "startYear": currentYear, "endYear": currentYear },
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
            url: "/ET_Admin_SupplierModule/GetSupplierSalesForBarchart",
            data: { "id": $scope.E_SupplierID, "startYear": previousYear, "endYear": previousYear },
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
            url: '/ET_Admin_SupplierModule/GetSupplierSalesForBarchart',
            data: { "id": $scope.E_SupplierID, "startYear": currentYear, "endYear": currentYear },
            async: false,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result)
            {
                currentYearData = result;
                var dataForBar = [{
                    label: $("#drpSupplier option:selected").text().trim() + ":" + previousYear.toString(),
                    backgroundColor: "#66ccff",
                    borderColor: "#66ccff",
                    borderWidth: 1,
                    data: JSON.parse(previousYearData),
                },
                {
                    label: $("#drpSupplier option:selected").text().trim() + ":" + currentYear.toString(),
                    backgroundColor: "red",
                    borderColor: "red",
                    borderWidth: 1,
                    data: JSON.parse(currentYearData),
                }
                ];

                var barChartData =
                {
                    labels: MONTHS,
                    datasets: dataForBar
                };

                if (currentYearData.length <= 0)
                {
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
                                text: 'Supplier Sales'
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
    };

    $scope.GetCompanyCommunication = function () {
        var getcustomerinfo = $http.get("/ET_Admin_CustomerModule/GetCompanyCommList",
            {
                params: { id: $scope.E_SupplierID }
            });

        getcustomerinfo.then(function (CompCommList) {
            var commList = JSON.parse(CompCommList.data);
            $("#tblSuppModule_Communication").dataTable().fnDestroy();
            $scope.CompanyCommList = commList;
        }, function () {
            alert("No Data Found");
        });
    }

    $scope.SupplierChange = function () {

        var getsupplierinfo = $http.get("/ET_Admin_SupplierModule/GetSupplierInfo",
            {
                params: { id: $scope.E_SupplierID }
            });
        getsupplierinfo.then(function (SupplierMaster) {
            $("#tblSuppModule_Contact").dataTable().fnDestroy();
            $("#tblSuppModule_Communication").dataTable().fnDestroy();
            $("#tblSuppModule_OrderDetails").dataTable().fnDestroy();
            $("#tblSuppModule_Commission").dataTable().fnDestroy();
            $("#ShipmentOne_to_one").dataTable().fnDestroy();
            $("#tblSuppModule_Schedules").dataTable().fnDestroy();
            $("#Shipment_Agency_List").dataTable().fnDestroy();
            $("#pending_shipment").dataTable().fnDestroy();
            $("#pending_invoice").dataTable().fnDestroy();
            var custMaster = JSON.parse(SupplierMaster.data.Result);
            var CompContacts = JSON.parse(SupplierMaster.data.Contact);
            var OrderLi = JSON.parse(SupplierMaster.data.Order);
            var PaymentLi = JSON.parse(SupplierMaster.data.Payment);
            var ScheduleLi = JSON.parse(SupplierMaster.data.Schedule);
            var Shipment1to1Li = JSON.parse(SupplierMaster.data.Shipment1to1);
            var ShipmentAgencyLi = JSON.parse(SupplierMaster.data.ShipmentAgency);
            

            $scope.SupplierMasterList = custMaster;
            $scope.CompanyContactsList = CompContacts;
            $scope.OrderList = OrderLi;
            $scope.PaymentList = PaymentLi;
            $scope.ScheduleList = ScheduleLi;
            $scope.Shipment1to1List = Shipment1to1Li;
            $scope.ShipmentAgencyList = ShipmentAgencyLi;
            $scope.GetCompanyCommunication();
            $scope.GetPendingShipmentDetails();
            $scope.GetPendingInvoicelist();
            $scope.GetProductWiseSales();
            $scope.GetCustomerSales();
        }, function () {
            alert("No Data Found");
        });

    }
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
    $scope.$watch("suppliers", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () { $("#drpSupplier").val("").trigger("chosen:updated"); }, 100);
        }
    });
    $scope.$watch("CompanyContactsList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#tblSuppModule_Contact');
            }, 5);
        }
    });
    $scope.$watch("OrderList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#tblSuppModule_OrderDetails');
            }, 5);
        }
    });
    $scope.$watch("PaymentList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#tblSuppModule_Commission');
            }, 5);
        }
    });
    $scope.$watch("Shipment1to1List", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#ShipmentOne_to_one');
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
    $scope.$watch("ScheduleList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#tblSuppModule_Schedules');
            }, 5);
        }
    });
    $scope.$watch("CompanyCommList", function (value) {
        var val = value || null;
        if (val) {
            setTimeout(function () {
                dynamicDataTable1('#tblSuppModule_Communication');
            }, 5);
        }
    });
    $scope.$watch("ShipmentDetailsInfo", function (value) {
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
});