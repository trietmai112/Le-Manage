
function formatDate(jsonDate) {
    var ms = jsonDate.substring(6, jsonDate.length - 2);
    var date = new Date(parseInt(ms));
    var hour = date.getHours();
    var mins = date.getMinutes() + '';
    var time = "AM";

    // find time 
    if (hour >= 12) {
        time = "PM";
    }
    // fix hours format
    if (hour > 12) {
        hour -= 12;
    }
    else if (hour == 0) {
        hour = 12;
    }
    // fix minutes format
    if (mins.length == 1) {
        mins = "0" + mins;
    }
    // return formatted date time string
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + " " + hour + ":" + mins + " " + time;
}

var mySwal = function (title, content, type, timer) {
    if (timer < 1) {
        swal({
            title: title,
            type: type,
            text: content,
            showCloseButton: false,
            showCancelButton: false,
            showConfirmButton: false,
            allowOutsideClick: false,
            onOpen: function () {
                swal.showLoading()
            }
        }).then(
            function () { },
            // handling the promise rejection
            function (dismiss) { }
            );
    } else {
        swal({
            title: title,
            text: content,
            type: type,
            showCancelButton: false,
            showConfirmButton: false,
            timer: timer,
            allowOutsideClick: false,
            onOpen: function () {
                swal.showLoading()
            }
        }).then(
            function () { },

            function (dismiss) {
            }
            );
    }
}

var showAjaxError = function (jqXHR) {
    var msg = '';
    if (jqXHR.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (jqXHR.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (jqXHR.status == 500) {
        msg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + jqXHR.responseText;
    }
    swal("Xẩy ra lỗi", msg, "error");
}

$("body").on("click", ".date-time-picker-clear", function (e) {
    e.preventDefault();
    $(this).parent().find("input").val("")
});

