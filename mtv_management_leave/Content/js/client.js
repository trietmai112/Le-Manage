
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

$("body").on("click", ".date-time-picker-clear", function (e) {
    e.preventDefault();
    $(this).parent().find("input").val("")
});
