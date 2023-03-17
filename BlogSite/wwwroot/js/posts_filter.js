$(document).ready(function () {

    function submitFilter() {
        $('#filter-page-num').val(1);
        $('#filter-form').submit();
    }

    $('.filter-input').change(function () {
        submitFilter();
    });
});