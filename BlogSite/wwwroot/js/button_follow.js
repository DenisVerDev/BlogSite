$(document).ready(function () {

    function followAuthor() {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: 'Author/Index?handler=Follow',
            data: { author_id: authorId, new_follow_status: true },
            dataType: 'html',
            success: function (result) {
                $('#btn-follow-container').empty();
                $('#btn-follow-container').append(result);
                $('#btn-auth-unfollow').bind('click', unfollowAuthor); //binding
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    function unfollowAuthor() {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: 'Author/Index?handler=Follow',
            data: { author_id: authorId, new_follow_status: false },
            dataType: 'html',
            success: function (result) {
                $('#btn-follow-container').empty();
                $('#btn-follow-container').append(result);
                $('#btn-auth-follow').bind('click', followAuthor); //binding
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    $('#btn-auth-follow').click(followAuthor);
    $('#btn-auth-unfollow').click(unfollowAuthor);
});