//Details Page JS

var boxId = $("#hiddenBoxId").val();
var detailsTbl = $("#boxDetailstbl").DataTable({
    "ajax": {
        "url": "/Box/GetBoxContents",
        "dataSrc": "",
        "dataType": "JSON",
        "data": { boxIdForContent: boxId }
    },
    "columns": [
        { "data": "IssueId" },
        { "data": "Publisher" },
        { "data": "SeriesName" },
        { "data": "IssueNumber" },
        { "data": "Qty" },
        { "data": "Condition" },
        { "data": "GradeId" }
    ],
    "columnDefs": [
        { "targets": 0, "visible": false, "searchable": false, "orderable": false },
        { "targets": 6, "visible": false, "searchable": false, "orderable": false }
    ],
    "select": { "style": "multi" },
    "order": [[0, "asc"]]
});


$("#btnRetireBox, #btnRetireBox1").click(function(e) {
    e.preventDefault();
    var boxId = $("#hiddenBoxId").val();
    $.ajax({
        url: "/Box/RetireBox",
        type: "GET",
        contentType: "application/html; charset-8",
        data: { boxId: boxId },
        success: function() {
            location.href = "/Box/Index/";
        },
        error: function() {
            alert("Error");
        }
    });
});

////CREATE PAGE JS
var tblCreateBox = $("#tbl_CreateAddIssues").DataTable({
    "deferRender": true,
    "lengthChange": false,
    "ajax": {
        "url": "/Box/GetNonBoxedIssues",
        "dataSrc": "",
        "dataType": "JSON",
        "type": "GET"
    },
    "columns": [
        { "data": null }, // 0 
        { "data": "IssueId" }, // 1
        { "data": "Publisher" }, // 2
        { "data": "SeriesName" }, // 3
        { "data": "IssueNumber" }, // 4
        { "data": "Barcode" } // 5
    ],
    "columnDefs": [
        { "targets": 0, "checkboxes": { "selectRow": true } },
        { "targets": 1, "orderable": false, "searchable": false, "visible": false },
        { "targets": 2, "width": "50px" },
        { "targets": 3, "width": "50px" },
        { "targets": 4, "searchable": false, "width": "5px" },
        { "targets": 5, "orderable": false, "searchable": true, "width": "5px" }
    ],
    "select": { "style": "os" },
    "order": [[1, "asc"]]
});

$("#btn_CreateBox").click(function(e) {
    e.preventDefault();
    var rowsSelected = [];
    rowsSelected = tblCreateBox.column(0).checkboxes.selected();

    var id = $("#hide_CreateBoxId").val();
    var boxname = $("#inBoxName").val();
    var details = $("#inBoxDetails").val();

    var arrayIssueId = makeArrayFromObject(rowsSelected, "IssueId");

    //WRITE AJAX to send to server the arrays then set those boxids to null
    $.ajax({
        type: "POST",
        url: "/Box/CreateAjax", //sends array of Issue IDs to server
        data: {
            issueIdArray: arrayIssueId,
            boxName: boxname,
            boxDetails: details,
            boxIdToCreate: id
        },
        traditional: true,
        success: function() {
            alert("Success!");
            //location.reload();//reload the page in order to clear the table cache of selected rows
        },
        error: function() {
            alert("Error");
            //$("#editAlertError").show();
            //$("#editAlertError").delay(8000).slideUp(500);
        }
    });
});

////////EDIT PAGE JS

//Manage Alerts
$("#editSuccessAlert").delay(2000).slideUp(500);

$("#tabAddComic").click(function(e) {
    e.preventDefault();
    var id = $("#editHiddenBoxId").val();

    $.ajax({
        url: "/Box/EditAddIssuesToBox",
        type: "GET",
        datatype: "html",
        data: { boxIdToAdd: id },
        success: function(result) {
            $("#_addComics").html(result);
        },
        error: function() {
            alert("Error, Partialview not loaded into second Div");
        }
    });

});

$("#tabDefault").click(function(e) {
    e.preventDefault();
    $("#_addComics").html("");
});
var boxId = $("#editHiddenBoxId").val();
var editTbl = $("#editBoxTbl").DataTable({
    "lengthChange": false,
    "deferRender": true,
    "ajax": {
        "url": "/Box/GetBoxContents", //
        "dataSrc": "",
        "dataType": "JSON",
        "data": { boxIdForContent: boxId }
    },
    "columns": [
        { "data": null }, // 0
        { "data": "IssueId" }, // 1
        { "data": "GradeId" }, // 2
        { "data": "Publisher" }, // 3
        { "data": "SeriesName" }, // 4
        { "data": "IssueNumber" }, // 5
        { "data": "Qty" }, // 6
        { "data": "Condition" } // 7
    ],
    "columnDefs": [
        { "targets": 0, "checkboxes": { "selectRow": true } }, //Checkboxes
        { "targets": 1, "visible": false, "searchable": false, "orderable": false }, //IssueID Column
        { "targets": 2, "visible": false, "searchable": false, "orderable": false }, //GradeId Column
        { "targets": 3, "width": "5px" },
        { "targets": 4, "width": "300px" },
        { "targets": 5, "width": "5px" },
        { "targets": 6, "width": "5px" },
        { "targets": 7, "width": "5px" }
    ],
    "select": { "style": "multi" },
    "order": [[1, "asc"]]
});

//Handle the button press to send to server the Id Arrays (IssueId & GradeID)
//Remove the issues from the box, all of one issue will be removed by design
//as all of one issue no matter condition will be stored in a single box

$("#btnRemoveIssue").click(function(ex) {
    ex.preventDefault();
    var rowsSelected = [];
    rowsSelected = editTbl.column(0).checkboxes.selected();

    var issueIdsArray = makeArrayFromObject(rowsSelected, "IssueId");

    //WRITE AJAX to send to server the arrays then set those boxids to null
    $.ajax({
        type: "POST",
        url: "/Box/RemoveIssue", //sends array of Issue IDs to server
        data: { issuesArray: issueIdsArray },
        traditional: true,
        success: function() {
            location.reload(); //reload the page in order to clear the table cache of selected rows
        },
        error: function() {
            $("#editAlertError").show();
            $("#editAlertError").delay(8000).slideUp(500);
        }
    });


});

$("#btnQuantityUpdate").click(function(p) {
    p.preventDefault();

    var qty = $("#qtyEditInput").val();
    var rowsSelected = editTbl.column(0).checkboxes.selected();

    var issueIdsArray = makeArrayFromObject(rowsSelected, "IssueId");
    var gradeIdsArray = makeArrayFromObject(rowsSelected, "GradeId");

    $.ajax({
        type: "POST",
        url: "/Box/UpdateQuantity",
        data: { issueIdArray: issueIdsArray, gradeintsArray: gradeIdsArray, qtyToSet: qty },
        traditional: true,
        success: function(data) {
            if (data.success === true) {
                location.reload(); //refreshes the page and clears the arrays
            } else {
                if (data.emptyQty === true) {
                    $("#editQuantityError").show();
                    $("#editQuantityError").delay(2000).slideUp(500);
                }

            }
        }

    });
});