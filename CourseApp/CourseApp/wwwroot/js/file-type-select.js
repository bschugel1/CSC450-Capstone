$("input:radio").change(function () {
    $(".type").hide();
    $(this).next("input").show();
});
