$(document).ready(function () {
    $(".NumericOnly").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
            //$(".txt-customDanger").html("Digits Only").show().fadeOut("slow");
        }
    });

    // ----------------------------------------
    // slider initialization
    // ----------------------------------------

    $(".ui-slider").each(function () {
        var index = $(this).attr('id').replace('slider-step', '');
        var input = $("#slider-step-value" + index);
        var min = parseInt($(input).attr("min")) || 0;
        var max = parseInt($(input).attr("max")) || 100;
        var val = parseInt($(input).val().replace('%', '')) || 0;

        $(this).empty().slider(
        {
            value: val,
            min: min,
            max: max,
            range: "min",
            step: 10,
            slide: function (event, ui) {
                $(input).val(ui.value);
            },
            stop: function (event, ui) { $(input).change(); }
        });
        $(input).val(val);
    });
});