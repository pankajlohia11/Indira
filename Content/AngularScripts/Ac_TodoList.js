app1.controller("TodoWidgetCtrl", function ($scope, $http, $window, $compile) {
    GetShipmentList();
    debugger;
    var months = new Array(12);
    months[0] = "January";
    months[1] = "February";
    months[2] = "March";
    months[3] = "April";
    months[4] = "May";
    months[5] = "June";
    months[6] = "July";
    months[7] = "August";
    months[8] = "September";
    months[9] = "October";
    months[10] = "November";
    months[11] = "December";

    var current_date = new Date();
    
    month_value = current_date.getMonth();
    $("#Month").text(months[month_value]);
    $("#Day").text(current_date.getDate());
    //$(".CheckingTodoList:checkbox").click(function (event)
    $(document).on('click', '.CheckingTodoList', function() {
        event.preventDefault();
        debugger;
        var value = $(this).parent().parent().parent().attr("class");
        console.log(value);
        if (value == undefined || value == "") {
            $(this).parent().parent().parent().addClass("completed");
            this.setAttribute("checked", "checked");
            this.checked = true;
        }
        if (value == "completed") {
            $(this).parent().parent().parent().removeClass("completed");
        }
    })
    $(document).on('click', '.text-danger', function () {
        $(this).parent().parent().remove();
    })
    $scope.AddTodoList = function () {
        debugger;
        $(".todo-list").append('<li><div class="view"><label class="checkbox checkbox-custom m-0 text-muted inline" ><input type="checkbox" class="CheckingTodoList" ><i></i></label><span>' + $scope.todo + '</span><a role="button" tabindex="0" class="text-danger remove-todo pull-right"><i class="fa fa-times"></i> </a></div ></li>'); 

    }
           
    function GetShipmentList() {
        var getshipmentlist = $http.get("/ET_TodoList/GetShipmentListForNotifications",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (shipmentlist) {
            debugger;
            var res = JSON.parse(shipmentlist.data);
            if (res.length > 0) {
                $(".todo-list").append('<li><div class="view"><label class="checkbox checkbox-custom m-0 text-muted inline" ><input type="checkbox" class="CheckingTodoList" ><i></i></label><span id="ShipmentId">Your today shipments  </span><a role="button" tabindex="0" class="text-danger remove-todo pull-right"><i class="fa fa-times"></i> </a></div ></li>');
            }
            for (i = 0; i < res.length; i++)
            {
                if (i == 0) {
                    $("#ShipmentId").append(res[i].S_Code + "-" + res[i].COM_DISPLAYNAME);
                }
                else {
                    $("#ShipmentId").append(" , " + res[i].S_Code + "-" + res[i].COM_DISPLAYNAME);
                }
        }
            GetDocumentList();

        }, function () {
            alert("Quotation List Found");
        });
    }
    function GetDocumentList() {

        var getshipmentlist = $http.get("/ET_TodoList/GetShipmentDocumentListForNotifications",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {

            var res = JSON.parse(Quotationlist.data);
            if (res.length > 0) {
                $(".todo-list").append('<li><div class="view"><label class="checkbox checkbox-custom m-0 text-muted inline" ><input type="checkbox" class="CheckingTodoList" ><i></i></label><span id="DocumentId">Your today Document Receiving shipment  </span><a role="button" tabindex="0" class="text-danger remove-todo pull-right"><i class="fa fa-times"></i> </a></div ></li>');
            }
            for (i = 0; i < res.length; i++) {
                if (i == 0) {
                    $("#DocumentId").append(res[i].S_Code + "-" + res[i].COM_DISPLAYNAME);
                }
                else {
                    $("#DocumentId").append(" , " + res[i].S_Code + "-" + res[i].COM_DISPLAYNAME);
                }
            }
            GetTaskList();

        }, function () {

        });
    }
    function GetTaskList() {

        var getshipmentlist = $http.get("/ET_TodoList/GetTaskList",
            {
                params: { delete: false }
            });
        getshipmentlist.then(function (Quotationlist) {
            debugger;
            var res = Quotationlist;
            if (Quotationlist.data.length > 0) {
               
            }
            for (i = 0; i < Quotationlist.data.length; i++) {
                var Quotationdate = new Date(parseInt(Quotationlist.data[i].Expec_Date.substr(6)));
                var QDT = Quotationdate.getHours() + ":" + Quotationdate.getMinutes();
                $(".todo-list").append('<li><div class="view"><label class="checkbox checkbox-custom m-0 text-muted inline" ><input type="checkbox" class="CheckingTodoList" ><i></i></label><span>' + Quotationlist.data[i].TSK_Title + ' </span><a role="button" tabindex="0" class="text-danger remove-todo pull-right"><i class="fa fa-times"></i> </a></div ></li>');       
                $("#appointments-carousel").append('<div><p class="text-center text-strong"> <i class="fa fa-clock-o"></i> ' + QDT+'</p><p><h5 class="mt-10 mb-0 text-strong">' + Quotationlist.data[i].TSK_Title + '</h5><small class="text-thin text-transparent-white">' + Quotationlist.data[i].TSK_Desc+'</small></p></div >');       
            
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
});