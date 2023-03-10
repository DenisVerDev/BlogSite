$(document).ready(function () {

    $('#filter-themes').change(function () {
        $('#filter-page-num').val(1);
        $('#filter-form').submit();
    });

    $('#filter-periods').change(function () {
        $('#filter-page-num').val(1);
        $('#filter-form').submit();
    });

    $('#filter-most-popular').change(function () {
        $('#filter-page-num').val(1);
        $('#filter-form').submit();
    });

    $('#filter-only-favorites').change(function () {
        $('#filter-page-num').val(1);
        $('#filter-form').submit();
    });
});