$(document).ready(function () {
    $('#btn-delete-acc').click(function () {
        $('#deleteAccModal').modal('show');
    });

    $('#btn-deleteAccModal-dismiss').click(function () {
        $('#deleteAccModal').modal('hide');
    });

    $('#btn-deleteAccModal-action').click(function () {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: '/Author/Settings?handler=Delete',
            dataType: 'json',
            complete: function () {
                window.location.href = '/Authentication/SignIn';
            }
        });
    });
});