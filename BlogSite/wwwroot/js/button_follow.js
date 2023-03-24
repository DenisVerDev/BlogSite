$(document).ready(function () {

    function followAuthor() {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: 'Author/Index?handler=Follow',
            data: { author_id: authorId},
            dataType: 'html',
            success: function (result) {
                $('#btn-follow-container').html(result);
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
            url: 'Author/Index?handler=Unfollow',
            data: { author_id: authorId},
            dataType: 'html',
            success: function (result) {
                $('#btn-follow-container').html(result);
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