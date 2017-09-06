$("#searchBtn").click(function(e) {
    //Search the GCD via AJAX using the 2 search terms
    e.preventDefault();
    var seriesBar = $("#search").val();
    var issueNum = $("#issueNumber").val();

    //if search term equals all numbers then search via barcode
    if (seriesBar.match(/^[0-9]+$/)) {
        if (issueNum.trim()) {
            $("#issueNumber").val("");
        }
        $.ajax({
            url: "/SearchComics/SearchBarcode",
            type: "GET",
            contentType: "application/html; charsetutf-8",
            data: { barcodeOrIsbn: seriesBar },
            datatype: "html",
            success: function(data) {
                $("#displaySearchResult").html(data);
                $("#issueNumber").val("");
            },
            error: function() {
                $("#searchError").show();
            }
        });
    } else {
        //else send seriesname and issue number to server for lookup
        $.ajax({
            url: "/SearchComics/SearchNameAndNumber",
            type: "GET",
            contentType: "application/html; charsetutf-8",
            data: { seriesName: seriesBar, issueNumber: issueNum },
            datatype: "html",
            success: function(data) {
                $("#displaySearchResult").html(data);
                $("#issueNumber").val("");
            },
            error: function() {
                $("#searchError").show();
            }
        });
    }
});


//On add button press, present the modal and define details within
$("#modalBtn").click(function() {
    $("#pubName").text($("#PublishersName").html());
    $("#serie").text($("#SeriesNames").html());
    $("#num").text($("#IssuesNumber").html());
    $("#pubOn").text($("#PublishingDate").html());
    $("#bar").text($("#BarcodeNumber").html());
});

//inside Modal button to add to stock
$("#btnAddData").click(function(e) {
    e.preventDefault();
    var pubId = $("#PublisherId").val();
    var sid = $("#SeriesId").val();
    var issId = $("#IssueId").val();
    var qty = $("#quantityInput").val();
    var condition = $("#gradeSelector").val();

    $.ajax({
        url: "/SearchComics/Add",
        type: "POST",
        data: { seriesId: sid, publisherId: pubId, issueId: issId, quantity: qty, gradeId: condition },
        success: function(data) {
            if (data.success) {
                //show success alert here
                $("#successInsert").show();
                $("#successInsert").delay(2000).fadeOut(500);
            } else {
                $("#errorInsert").show();
            }
        },
        error: function() {
            //Server error code 40x 0r 50x then run this error
            $("#errorInsert").show();
        }
    });
});