app1.controller("Ac_NotificationCtrl", function ($scope, $http, $window, $compile) {
   
   
    GetShipmentList();
    GetShipmentListForStore();
    GetSchduleList();
    GetEnquiryList();
    GetQuotationList();
    GetDocumentList();
    function GetQuotationList() {

        var getshipmentlist = $http.get("/ET_Sales_Quotation/GetQuotationNotinOrderList",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {
            
            var res = JSON.parse(Quotationlist.data);
            $scope.Quotationlist = res;
            var quotationCount = res.length;
            $("#quotationCount").text(quotationCount);

        }, function () {

        });    }
    function GetEnquiryList() {
        
        var getshipmentlist = $http.get("/ET_Sales_Quotation/GetEnquiryNotinQuotationList",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Enquirylist) {
            
            var res = JSON.parse(Enquirylist.data);
            $scope.Enquirylist = res;
            var EnquiryCount = res.length;
            $("#EnquiryCount").text(EnquiryCount);

        }, function () {
            alert("Quotation List Found");
        });

    }
    function GetShipmentList() {
        var getshipmentlist = $http.get("/ET_Sales_Shipment/GetShipmentListForNotifications",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (shipmentlist) {
            
            var res = JSON.parse(shipmentlist.data);
            $scope.shipmentlist = res;
            var ShipmentCount = res.length;
            $("#ShipmentCount").text(ShipmentCount);

        }, function () {
            alert("Quotation List Found");
        });
    }
    function GetShipmentListForStore() {
        var getshipmentlist = $http.get("/ET_Sales_Despatch/GetDespatchListForNotifications",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (shipmentlist) {
            
            var res = JSON.parse(shipmentlist.data);
            $scope.shipmentlist = res;
            var ShipmentCount = res.length;
            $("#ShipmentCountStore").text(ShipmentCount);

        }, function () {
            alert("Quotation List Found");
        });
    }
    function GetDocumentList() {

        var getshipmentlist = $http.get("/ET_Sales_Shipment/GetShipmentDocumentListForNotifications",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {
            
            var res = JSON.parse(Quotationlist.data);
            $scope.Quotationlist = res;
            var quotationCount = res.length;
            $("#DocumentCount").text(quotationCount);

        }, function () {

        });
    }
    function GetSchduleList() {
        var getshipmentlist = $http.get("/ET_Sales_Shedule/GetSchduleListForNotification",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (shipmentlist) {
            
            var res = JSON.parse(shipmentlist.data);
            $scope.shipmentlist = res;
            var ShipmentCount = res.length;
            $("#SchduleCount").text(ShipmentCount);

        }, function () {
            alert("Quotation List Found");
        });
    }
   
  
});