app1.controller("AC_StatusCtrl", function ($scope, $http, $window, $compile)
{
    var dynamicTableSet = false;
    //initialize the dynamic data table.
    $("#advancedusage").dataTable().fnDestroy();
    //setTimeout(function ()
    //{
    //    dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
    //    //$('#advancedusage').DataTable();
    //}, 5000);

    //$('#advancedusage').DataTable();

    $("#txtFromDate").val("");
    $("#txtToDate").val("");
    //default load operations
    var today = new Date();
    var taskstartDate = ("0" + today.getDate()).slice(-2) + "-" + ("0" + (today.getMonth() + 1)).slice(-2) + "-" + 2017;
    var taskCurrentDate = ("0" + today.getDate()).slice(-2) + "-" + ("0" + (today.getMonth() + 1)).slice(-2) + "-" + today.getFullYear();
    $("#txtFromDate").val(taskstartDate).trigger("chosen:updated");
    $("#txtToDate").val(taskCurrentDate).trigger("chosen:updated");
    //$("#drpStatus").val("0");
    $("#drpStatus").val("0").trigger("chosen:updated");
    $scope.T_StatusID = "0";
    var statusValue = $("#drpStatus").val();
    GetTaskStatus(statusValue, taskstartDate, taskCurrentDate);

    function GetTaskStatus(stat, fromDate, toDate)
    {
        //debugger;
        ////a = parseInt(a);
        ////alert("GetTaskStatus" + stat + fromDate + toDate);
        //var get = $http(
        //    {
        //    method: "POST",
        //        url: '/ET_TaskStatus/GetTaskStatus/',
        //    data: { status: stat, Fromdate: fromDate, Todate: toDate },
        //    dataType: "json",
        //    headers: { "Content-Type": "application/json" }
        //});
        //get.success(function (data, status)
        //{
        //    if (data == "Session Expired")
        //    {
        //        $window.location.href = '/ET_Login/ET_SessionExpire';
        //    }
        //    else if (data.indexOf("ERR") > -1)
        //    {
        //        $("#spanErrMessage1").html(data);
        //        $("#spanErrMessage2").html(data);
        //        if ($("#exceptionmessage").length)
        //            $("#exceptionmessage").trigger("click");
        //    }
        //    else
        //    {
        //        alert(res);
        //        $scope.TaskStatus = res;
        //        var res = JSON.parse(data);
        //        //var contactdata = JSON.parse(data);
        //        var i = res.length;
        //        length = 1;
        //        angular.element(document.getElementById('taskbody')).html("");
        //       // alert(res);
        //        while (i != 0)
        //        {
        //            var Rowhtml = "<tr><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td></tr>";
        //            var temp = $compile(Rowhtml)($scope);
        //            angular.element(document.getElementById('taskbody')).append(temp);
        //            /*$scope.TSK_Title = res[0].TSK_Title;
        //            $scope.TSK_Desc = res[0].TSK_Desc;
        //            $scope.TSK_Priority = res[0].TSK_Priority;
        //            $scope.TSK_Status = res[0].TSK_Status;
        //            $scope.Expec_Date = res[0].Expec_Date;
        //            $scope.TSK_Compl_Date = res[0].TSK_Compl_Date;
        //            $scope.TSK_AssignTo = res[0].TSK_AssignTo;*/
        //            // $("#drpStatus").val(res[0].TSK_Status).trigger("chosen:updated");
        //            i = i - 1;
        //        }
        //        $scope.taskdata = res;
        //    }
        //});
        //get.error(function (data, status) {
        //    $window.alert(data.Message);
        //});
        //$("#advancedusage").dataTable().fnDestroy();
        //if (!dynamicTableSet) {
        //    $("#advancedusage").DataTable({
        //        //"language":
        //        //{
        //        //    "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        //        //},
        //        "processing": true, // for show progress bar  
        //        "serverSide": true, // for process server side  
        //        "filter": true, // this is for disable filter (search box)  
        //        "orderMulti": false, // for disable multiple column at once  
        //        "searching": true,
        //        "ajax":
        //        {
        //            "url": "/ET_TaskStatus/GetTaskStatus",
        //            "type": "GET",
        //            "datatype": "json",
        //            "data": { status: stat, Fromdate: fromDate, Todate: toDate },
        //            "sPaginationType": "full_numbers",
        //            "success": function (result) {
        //                if (result.indexOf("ERR") > -1) {
        //                    $("#spanErrMessage1").html(result);
        //                    $("#spanErrMessage2").html(result);
        //                    if ($("#exceptionmessage").length)
        //                        $("#exceptionmessage").trigger("click");
        //                }
        //                else {
        //                    var res = JSON.parse(result);
        //                    //$scope.TaskStatus = result;
        //                    //var contactdata = JSON.parse(data);
        //                    var i = res.length;
        //                    length = 1;
        //                    angular.element(document.getElementById('taskbody')).html("");
        //                    // alert(res);
        //                    while (i != 0) {
        //                        var Rowhtml = "<tr><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td></tr>";
        //                        var temp = $compile(Rowhtml)($scope);
        //                        angular.element(document.getElementById('taskbody')).append(temp);
        //                        i = i - 1;
        //                    }
        //                    $scope.taskdata = res;
        //                }
        //            },
        //            error: function (response) {
        //                if ($("#exceptionmessage").length)
        //                    $("#exceptionmessage").trigger("click");
        //            }
        //        }

        //    });
        //    dynamicTableSet = true;
        //}
        //else
        //{
           // var jsonData;
        $.ajax(
            {
            type: "GET",
            //url: '@Url.Action("GetTaskStatus", "ET_TaskStatus")',
            url: '/ET_TaskStatus/GetTaskStatus',
            data: { status: stat, Fromdate: fromDate, Todate: toDate },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
                success: function (result) {
                    if (result.indexOf("ERR") > -1) {
                        $("#spanErrMessage1").html(result);
                        $("#spanErrMessage2").html(result);
                        if ($("#exceptionmessage").length)
                            $("#exceptionmessage").trigger("click");
                    }
                    else {
                        var res = JSON.parse(result);
                        //var contactdata = JSON.parse(data);
                        var i = res.length;
                        length = 1;
                        angular.element(document.getElementById('taskbody')).html("");
                        // alert(res);
                        while (i != 0)
                        {
                            //alert("i:" + i);
                            var Rowhtml = "<tr><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td></tr>";
                            var temp = $compile(Rowhtml)($scope);
                            angular.element(document.getElementById('taskbody')).append(temp);
                            //(Better way of doing this is using the scope below..But for now this is temp.)
                            /*$scope.TSK_Title = res[0].TSK_Title;
                            $scope.TSK_Desc = res[0].TSK_Desc;
                            $scope.TSK_Priority = res[0].TSK_Priority;
                            $scope.TSK_Status = res[0].TSK_Status;
                            $scope.Expec_Date = res[0].Expec_Date;
                            $scope.TSK_Compl_Date = res[0].TSK_Compl_Date;
                            $scope.TSK_AssignTo = res[0].TSK_AssignTo;*/
                            // $("#drpStatus").val(res[0].TSK_Status).trigger("chosen:updated");
                            i = i - 1;
                        }
                        $scope.taskdata = res;
                        $scope.TaskStatus = result;
                    }
                },
             error: function (response)
                {
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
                }
            }
        );
            //$('#advancedusage').DataTable().ajax.reload(jsonData);
        //}
        //$.ajax({
        //    type: "GET",
        //    //url: '@Url.Action("GetTaskStatus", "ET_TaskStatus")',
        //    url: '/ET_TaskStatus/GetTaskStatus',
        //    data: { status: stat, Fromdate: fromDate, Todate: toDate },
        //    contentType: "application/json;charset=utf-8",
        //    dataType: "json",
        //    success: function (result) {
        //        if (result.indexOf("ERR") > -1)
        //        {
        //            $("#spanErrMessage1").html(result);
        //            $("#spanErrMessage2").html(result);
        //            if ($("#exceptionmessage").length)
        //                $("#exceptionmessage").trigger("click");
        //        }
        //        else
        //        {
        //            var res = JSON.parse(result);
        //            $scope.TaskStatus = result;
        //            //var contactdata = JSON.parse(data);
        //            var i = res.length;
        //            length = 1;
        //            angular.element(document.getElementById('taskbody')).html("");
        //            // alert(res);
        //            while (i != 0)
        //            {
        //                var Rowhtml = "<tr><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td><td><p></p></td></tr>";
        //                var temp = $compile(Rowhtml)($scope);
        //                angular.element(document.getElementById('taskbody')).append(temp);
        //                //(Better way of doing this is using the scope below..But for now this is temp.)
        //                /*$scope.TSK_Title = res[0].TSK_Title;
        //                $scope.TSK_Desc = res[0].TSK_Desc;
        //                $scope.TSK_Priority = res[0].TSK_Priority;
        //                $scope.TSK_Status = res[0].TSK_Status;
        //                $scope.Expec_Date = res[0].Expec_Date;
        //                $scope.TSK_Compl_Date = res[0].TSK_Compl_Date;
        //                $scope.TSK_AssignTo = res[0].TSK_AssignTo;*/
        //                // $("#drpStatus").val(res[0].TSK_Status).trigger("chosen:updated");
        //                i = i - 1;
        //            }
        //            $scope.taskdata = res;
        //        }
        //    },
        //    error: function (response) {
        //        if ($("#exceptionmessage").length)
        //            $("#exceptionmessage").trigger("click");
        //    }
        //});
    }

    //Submit the Task Status Request.
    $scope.ET_submit = function ()
    {
        var stat = $scope.T_StatusID;
        var fromDate = $("#txtFromDate").val();
        var toDate = $("#txtToDate").val();

        if (fromDate == "") {
            toastr["error"]("Select Starting Task Date", "Notification");
            return "";
        }

        if (toDate == "")
        {
            toastr["error"]("Select Ending Task Date", "Notification");
            return "";
        }
        GetTaskStatus(stat, fromDate, toDate);
       // alert("whether function is hit or not");
        //a = parseInt(a);
        //alert("GetTaskStatus" + stat + fromDate + toDate);
    };


    // GetPrivilages();
    //alert("task status");
 


function shipping(a) {
    debugger;
    a = parseInt(a);
    $.ajax({
        type: "GET",
        url: '@Url.Action("ET_TaskStatus_View", "ET_TaskStatus")',
        data: { id: a },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.indexOf("ERR") > -1) {
                $("#spanErrMessage1").html(result);
                $("#spanErrMessage2").html(result);
                if ($("#exceptionmessage").length)
                    $("#exceptionmessage").trigger("click");
            }
            else {
                var assign;
                var res = JSON.parse(result);
                for (var i = 0; i < res.length;) {
                    var Rowhtml = "<tr><td><input readonly='readonly' type='text' id='txtTitle' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtdesc' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtPriority' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtStatus' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtExcepdate' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtAcual' value='" + length + "' class='form-control' /></td></tr><tr><td><input readonly='readonly' type='text' id='txtAssignto' value='" + length + "' class='form-control' /></td></tr>";
                    var temp = $compile(Rowhtml)($scope);
                    angular.element(document.getElementById('taskBody')).append(temp);
                    var tr = $("#Contactbody").find("tr");
                    var td = tr[i].cells;
                    $(td[0]).find("input").val(res[i]["TSK_Title"]);
                    $(td[1]).find("input").val(res[i]["TSK_Desc"]);
                    $(td[2]).find("input").val(res[i]["TSK_Priority"]);
                    $(td[3]).find("input").val(res[i]["TSK_Status"]);

                    var Expecdate = new Date(parseInt(res[i]["Excep_Date"].substr(6)));
                    var ampm = Expecdate.getHours() >= 12 ? 'PM' : 'AM';
                    var DT = '';
                    if (Expecdate.getMinutes() <= 9) {
                        if (Expecdate.getHours() <= 9) {
                            DT = ("0" + Expecdate.getDate()).slice(-2) + "\/" + ("0" + (Expecdate.getMonth() + 1)).slice(-2) + "\/" + Expecdate.getFullYear() + " " + "0" + Expecdate.getHours().toLocaleString().replace(/:\d{2}\s/, ' ') + ":0" + Expecdate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }
                        else {
                            DT = ("0" + Expecdate.getDate()).slice(-2) + "\/" + ("0" + (Expecdate.getMonth() + 1)).slice(-2) + "\/" + Expecdate.getFullYear() + " " + Expecdate.getHours() + ":0" + Expecdate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }

                        $(td[4]).find("input").val(DT);
                    }
                    else {
                        
                        if (Expecdate.getHours() <= 9) {

                            DT = ("0" + Expecdate.getDate()).slice(-2) + "\/" + ("0" + (Expecdate.getMonth() + 1)).slice(-2) + "\/" + Expecdate.getFullYear() + " " + "0" + Expecdate.getHours().toLocaleString().replace(/:\d{2}\s/, ' ') + ":" + Expecdate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }
                        else {
                            DT = ("0" + Expecdate.getDate()).slice(-2) + "\/" + ("0" + (Expecdate.getMonth() + 1)).slice(-2) + "\/" + Expecdate.getFullYear() + " " + Expecdate.getHours() + ":" + Expecdate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }

                        $(td[4]).find("input").val(DT);
                    }

                    var CompletionDate = new Date(parseInt(res[i]["TSK_Compl_date"].substr(6)));
                    ampm = CompletionDate.getHours() >= 12 ? 'PM' : 'AM';
                    var DT1 = '';
                    if (CompletionDate.getMinutes() <= 9) {
                        if (CompletionDate.getHours() <= 9)
                        {
                            DT1 = ("0" + CompletionDate.getDate()).slice(-2) + "\/" + ("0" + (CompletionDate.getMonth() + 1)).slice(-2) + "\/" + CompletionDate.getFullYear() + " " + "0" + CompletionDate.getHours().toLocaleString().replace(/:\d{2}\s/, ' ') + ":0" + CompletionDate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }
                        else {
                            DT1 = ("0" + CompletionDate.getDate()).slice(-2) + "\/" + ("0" + (CompletionDate.getMonth() + 1)).slice(-2) + "\/" + CompletionDate.getFullYear() + " " + CompletionDate.getHours() + ":0" + CompletionDate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }

                        $(td[5]).find("input").val(DT1);
                    }
                    else {
                        if (CompletionDate.getHours() <= 9) {

                            DT1 = ("0" + CompletionDate.getDate()).slice(-2) + "\/" + ("0" + (CompletionDate.getMonth() + 1)).slice(-2) + "\/" + CompletionDate.getFullYear() + " " + "0" + CompletionDate.getHours().toLocaleString().replace(/:\d{2}\s/, ' ') + ":" + CompletionDate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }
                        else {
                            DT1 = ("0" + CompletionDate.getDate()).slice(-2) + "\/" + ("0" + (CompletionDate.getMonth() + 1)).slice(-2) + "\/" + CompletionDate.getFullYear() + " " + CompletionDate.getHours() + ":" + CompletionDate.getMinutes().toLocaleString().replace(/:\d{2}\s/, ' ') + ' ' + ampm;
                        }

                        $(td[5]).find("input").val(DT1);
                    }

                    if (res[i]["TSK_Type"] == true)
                    {
                        type = 1;
                        $('#drpEmpList').css('display', 'block');
                        assign = JSON.parse("[" + res[i]['TSK_AssignTo'].replace(/['']+/g, '') + "]");
                        var assignLength = assign.length;
                        var assignedMembers = '';
                        for (var assignId = 0; assignId < assignLength; assignId++) {
                            var assignedMember = Int32.parse(assignId[i]);
                            var get = $http(
                                {
                                    method: "GET",
                                    url: '/ET_TaskStatus/GetTaskAssingedUserName/',
                                    data: { currentemployee: assignedMember },
                                    dataType: "json",
                                    headers: { "Content-Type": "application/json" }
                                });
                            get.success(function (data, status)
                            {
                                if (data == "Session Expired") {
                                    $window.location.href = '/ET_Login/ET_SessionExpire';
                                }
                                else if (data.indexOf("ERR") > -1) {
                                    $("#spanErrMessage1").html(data);
                                    $("#spanErrMessage2").html(data);
                                    if ($("#exceptionmessage").length)
                                        $("#exceptionmessage").trigger("click");
                                }
                                else
                                {
                                    var displayname = JSON.parse(data);
                                    assignedMembers += displayname;
                                }
                            });
                            get.error(function (data, status)
                            {
                                $window.alert(data.Message);
                            });
                        }

                        //Fill up in the specific colummn where the assinged members 
                        $(td[7]).find("input").val(assignedMembers);

                    } else { type = 0; }

                    i++;
                }


                //$("td[0]").val(res[i]["TSK_Title"]);
                //$("#title-desc").val(res[0]["TSK_Desc"]);
                //$("td[1]").val(res[i]["TSK_Priority"]);
                //$("#TSK_ID").val(res[0]["TSK_ID"]);
                //var type = 0;

            }
        },
        error: function (response) {
            if ($("#exceptionmessage").length)
                $("#exceptionmessage").trigger("click");
        }
    });
    }

    $scope.Statuschange = function () {
        //if ($scope.T_StatusID == "0")
        //    productCategory = "Yarn";
        //else if ($scope.T_StatusID == "1")

        //else if ($scope.T_StatusID == "1")

    };

    $scope.$watch("TaskStatus", function (value)
    {
        var val = value || null;
        if (val)
        {
            if (!dynamicTableSet) {
                setTimeout(function () {
                    dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
                    dynamicTableSet = true;
                }, 5000);
            }
            else {
                //var table = $("#advancedusage").DataTable();
                //table.clear();
                //table.rows.add(val).draw();
                $('#advancedusage').DataTable().ajax.reload();
            }

            //alert("TaskStatus updated");
            //if (!dynamicTableSet)
            //{
            //    setTimeout(function () {
            //        dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            //        dynamicTableSet = true;
            //    }, 5000);
            //}
            //else
            //{
            //    $('#advancedusage').DataTable().ajax.reload();
            //}
            //$('#advancedusage').DataTable().ajax.reload();
            //$("#advancedusage").dataTable().fnDestroy();
            //setTimeout(function ()
            //{
            //    dynamicDataTable('#advancedusage', '#tableTools', '#colVis');
            //    //$('#advancedusage').DataTable();
            //}, 0);
        }
    });

    $scope.$watch("taskdata", function (value)
    {
        var val = true;
        if (val)
        {
            var res = value;
            setTimeout(function ()
            {
                //debugger;
                var tr = $("#taskbody").find("tr");
                for (var i = 0; i < tr.length; i++)
                {
                    var td = tr[i].cells;
                    $(td[0]).find("p").text(res[i].SO_Serial);
                    $(td[1]).find("p").text(res[i].TSK_Title);
                    $(td[2]).find("p").text(res[i].TSK_Description);
                    $(td[3]).find("p").text(res[i].TSK_Priority);
                    $(td[4]).find("p").text(res[i].TSK_Status);
                    var Expecdt = "";
                    if (res[i].TSK_Expected_Date != null)
                    {
                        var Expecdate = new Date(parseInt(res[i].TSK_Expected_Date.substr(6)));
                        Expecdt = ("0" + Expecdate.getDate()).slice(-2) + "-" + ("0" + (Expecdate.getMonth() + 1)).slice(-2) + "-" + Expecdate.getFullYear();
                    }
                    //$("#txtFromDate").val(Expecdt);
                    if (res[i].TSK_Compl_Date != null)
                    {
                        var Compldate = new Date(parseInt(res[i].TSK_Compl_Date.substr(6)));
                        var Compldt = ("0" + Compldate.getDate()).slice(-2) + "-" + ("0" + (Compldate.getMonth() + 1)).slice(-2) + "-" + Compldate.getFullYear();
                        //$("#txtToDate").val(Compldt);
                    }

                    $(td[5]).find("p").text(Expecdt);
                    $(td[6]).find("p").text(Compldt);

                    var assignedString = res[i].TSK_Assignto.replace(/'/g, '');
                    if (assignedString !== null && assignedString !== "")
                    {
                        var assign = assignedString.split(',');
                        var assignLength = assign.length;
                        var assignedMembers = '';
                        for (var assignId = 0; assignId < assignLength; assignId++)
                        {
                            var assignedMember = parseInt(assign[assignId], 10);
                            $.ajax({
                                type: "GET",
                                url: '/ET_TaskStatus/GetTaskAssingedUserName',
                                data:
                                {
                                    currentemployee: assignedMember
                                },
                                async: false,
                                contentType: "application/json;charset=utf-8",
                                dataType: "json",
                                success: function (result)
                                {
                                    //if (result == "Session Expired")
                                    //{
                                    //    $window.location.href = '/ET_Login/ET_SessionExpire';
                                    //}
                                    //else if (result.indexOf("ERR") > -1)
                                    //{
                                    //    $("#spanErrMessage1").html(result);
                                    //    $("#spanErrMessage2").html(result);
                                    //    if ($("#exceptionmessage").length)
                                    //        $("#exceptionmessage").trigger("click");
                                    //}
                                    //else
                                    //{
                                    var displayname = JSON.parse(result);
                                    if (assignId == 0)
                                        assignedMembers = assignedMembers + displayname;
                                    else
                                        assignedMembers = assignedMembers + displayname + ",";
                                    //}
                                },
                                    error: function (response)
                                    {
                                        //$window.alert(response.Message);
                                }
                            });
                        }
                        //alert(assignedMembers);
                        $(td[7]).find("p").text(assignedMembers);
                    }
                    //$scope.TSK_AssignTo = assignedMembers;
                }
            }, 0);
        }
    });

});

