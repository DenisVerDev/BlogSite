$(document).ready(function () {
    $('#btn-delete-post').click(function () {
        $('#deletePostModal').modal('show');
    });

    $('#btn-deletePostModal-dismiss').click(function () {
        $('#deletePostModal').modal('hide');
    });

    $('#btn-deletePostModal-action').click(function () {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: '/Post/Index?handler=Delete',
            data: { post_id: postId },
            dataType: 'json',
            complete: function () {
                window.location.href = '/Author/Index';
            }
        });
    });
});