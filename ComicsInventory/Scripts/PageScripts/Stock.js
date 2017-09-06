$(document).ready(function() {

    //Datatables SetUp and Population using Serverside AJAX
    var oTable = $("#stockTable").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Stock/GetData",
            "type": "POST",
            "datatype": "JSON",
            "data": function(d) {
                if ($("#ddlPublisherSelect").val()) {
                    d.publisherId = $("#ddlPublisherSelect").val();
                } else {
                    d.publisherId = null;
                }
                if ($("#ddlSeriesSelect").val()) {
                    d.seriesId = $("#ddlSeriesSelect").val();
                } else {
                    d.seriesId = null;
                }

            }
        },

        "columns": [
            { "data": "PublisherId" },
            { "data": "Publisher" },
            { "data": "SeriesId" },
            { "data": "SeriesName", "autoWidth": true },
            { "data": "IssueId" },
            { "data": "IssueNumber", "width": "20px;" },
            { "data": "PublishDate", "autoWidth": true },
            { "data": "BoxId", "autoWidth": true },
            { "data": "Barcode", "autoWidth": true }
        ],
        "columnDefs": [
            { "visible": false, "targets": 0, "searchable": false }, //hiding PublisherId
            { "targets": 1, "searchable": true, "visible": false }, //Publisher column
            { "visible": false, "targets": 2, "searchable": false }, //hiding Series ID
            { "targets": 3, "searchable": true }, //Series name column
            { "visible": false, "targets": 4, "searchable": false }, //hiding IssueId
            { "targets": 5, "sortable": false, "searchable": false, "orderable": false }, //issueNumber column
            { "targets": 6, "sortable": false, "searchable": false, "orderable": false }, //Publish Date column
            { "targets": 7, "sortable": false, "searchable": false, "orderable": false }, //Boxid column
            { "targets": 8, "sortable": false, "searchable": true, "orderable": false, "visible": false }, //Barcode/UPC column searchable not visible
            {
                "targets": 9,
                "data": null,
                "autowidth": true,
                "searchable": false,
                "orderable": false,
                "defaultContent": '<button class="btn btn-xs btn-info" type="button" id="tblBtnDetails"><i class="fa fa-fw fa-info" style="margin-right: 2.5px;"></i>Details</button>'
            }
        ]

    });
    //stockTable row click details button
    $(function() {
        $("#stockTable tbody").on("click", "#tblBtnDetails", function(e) {
            e.preventDefault();
            var data = oTable.row($(this).parents("tr")).data();
            var pubId = data["PublisherId"];
            var seriesId = data["SeriesId"];
            var issueId = data["IssueId"];

            $.ajax({
                url: "/Stock/Details",
                type: "GET",
                data: { publisherId: pubId, seriesId: seriesId, issueId: issueId },
                success: function(data) {
                    $("#detailsReturn").html(data);
                    //display returned partial view in a div here
                },
                error: function() {
                    console.log("Error");
                }
            });
        });
    });

    //ddl filters triggering the redraw of the datable utilizing the oTable
    $(function() {
        $("#ddlPublisherSelect").change(function() {
            var val = $(this).val();
            $("#ddlSeriesSelect").html('<option selected="selected" value="">-- Select --</option>');
            var seriesItems = "";
            $.get("/Stock/GetSeriesItems", { publisherId: val }, function(data) {
                $.each(data, function(index, item) {
                    seriesItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $("#ddlSeriesSelect").append(seriesItems);
            });
            oTable.column(0).search(val).draw();
        });
    });

    $(function() {
        $("#ddlSeriesSelect").change(function() {
            var qry = $(this).val();
            oTable.columns(2).search(qry).draw();
        });
    });


    //Editpage Delete button is pressed
    $("#btnIssueDelete").click(function(e) {
        e.preventDefault();
        var issueToDelete = $("#editIssueId").val();
        $.ajax({
            type: "POST",
            url: "/Stock/Delete",
            data: { issueId: issueToDelete },
            success: function() {
                $("#deleteSuccess").show(0, function() {
                    setTimeout(function() { window.location.replace("/Stock/Index"); }, 1000);
                });
            },
            error: function() {
                $("#deleteError").show();
            }
        });
    });

    $("#btnClearDetailsPane").click(function(e) {
        e.preventDefault();
        $("#detailsReturn").html("<p>Choose an issue to view more details...</p>");
    });


});