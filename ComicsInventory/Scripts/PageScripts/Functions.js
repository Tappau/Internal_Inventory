function makeArrayFromObject(input, field) {
    var out = [];
    for (var i = 0; i < input.length; i++) {
        parseInt(out.push(input[i][field]));
    }
    return out;
}

//The following functions are inspired and courtesy of...
//http://www.codescratcher.com/javascript/print-image-using-javascript/#comment-762
//Accessed on 20/03/2017

function SourcePrint(source, id) {
    var box = id;
    return "<html><head><script>function step1(){\n" +
        "setTimeout('step2()', 10);}\n" +
        "function step2(){window.print();window.close()}\n" +
        "</scri" + "pt></head><body onload='step1()'>\n" +
        "<img src='" + source + "' />" + '<p style="padding-left: 100px; margin-top:-10px;">Box # ' + box + "</p>" +
        "</body>" +
        "</html>";
}

function QrPrint(source, boxNumber) {
    var badge = boxNumber.toString();
    var pagelink = "about:blank";
    var win = window.open(pagelink, "_new");
    win.document.open();
    win.document.write(SourcePrint(source, badge));
    win.document.close();

}