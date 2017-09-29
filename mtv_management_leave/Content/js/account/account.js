$(document).ready(function (e) {
    $("body").on("click", ".btn-login", function (e) {
        e.preventDefault();
        var form = $(this).parents("form")[0];
        var validator = $(form).validate();
        if (validator.form()) {
            $(form).submit();
        }
    });
    $("body").on("click", "a[data-ma-action]", function (e) {
        location.assign($(this).attr("href"));
    });
});