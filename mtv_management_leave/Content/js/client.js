
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

function dateConvert(jsonDate) {
    var ms = jsonDate.substring(6, jsonDate.length - 2);
    var date = new Date(parseInt(ms));
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
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
    } else if (jqXHR.status == 400) {       
        msg = jqXHR.responseJSON.message;
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


var notify = function(message, type) {
    $.growl({
        icon: "",
        title: '',
        message: message,
        url: ''
    }, {
        element: 'body',
        type: type,
        allow_dismiss: true,
        placement: {
            from: "top",
            align: "right"
        },
        offset: {
            x: 20,
            y: 45
        },
        spacing: 10,
        z_index: 1031,
        delay: 1500,
        timer: 1000,
        url_target: '_blank',
        mouse_over: false,        
        icon_type: 'class',
        template: '<div data-growl="container" class="alert" role="alert">' +
                        '<button type="button" class="close" data-growl="dismiss">' +
                            '<span aria-hidden="true">&times;</span>' +
                            '<span class="sr-only">Close</span>' +
                        '</button>' +
                        '<span data-growl="icon"></span>' +
                        '<span data-growl="title"></span>' +
                        '<span data-growl="message"></span>' +
                        '<a href="#" data-growl="url"></a>' +
                    '</div>'
    });
};

$("body").on("click", ".date-time-picker-clear", function (e) {
    e.preventDefault();
    $(this).parent().find("input").val("")
});

