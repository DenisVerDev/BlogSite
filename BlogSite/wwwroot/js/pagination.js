$('document').ready(function () {

    if (!pagin_has_prev) {
        var li = $('#pagin-prev').parent();
        li.addClass('disabled');
    }

    if (!pagin_has_next) {
        var li = $('#pagin-next').parent();
        li.addClass('disabled');
    }

    $('#pagin-curpage').keydown(function (event) {
        if (event.which == 13) {
            var entered_page = $('#pagin-curpage').val();

            if (entered_page < pagin_min || entered_page > pagin_max) return false;

            $('#filter-page-num').val(entered_page);
            $('#filter-form').submit();
        }

        return true;
    });

    $('#pagin-prev').click(function () {
        var prev_page = pagin_cur - 1;

        $('#filter-page-num').val(prev_page);
        $('#filter-form').submit();
    });

    $('#pagin-next').click(function () {
        var next_page = pagin_cur + 1;

        $('#filter-page-num').val(next_page);
        $('#filter-form').submit();
    });

});