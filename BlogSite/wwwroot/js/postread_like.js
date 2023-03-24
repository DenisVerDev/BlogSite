$(document).ready(function () {

    function changeLikesCounter(status) {
        var likes = parseInt($('#likes-counter').html());

        if (status) likes++;
        else likes--;

        $('#likes-counter').html(likes);
    }

    function likePost() {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: 'Post/Index?handler=Like',
            data: { post_id: postId },
            dataType: 'html',
            success: function (result) {
                $('.like-button').html(result);
                $('.like-button').unbind('click');
                $('.like-button').bind('click', undoLikePost);

                changeLikesCounter(true);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    function undoLikePost() {
        $.ajax({
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            url: 'Post/Index?handler=UndoLike',
            data: { post_id: postId },
            dataType: 'html',
            success: function (result) {
                $('.like-button').html(result);
                $('.like-button').unbind('click');
                $('.like-button').bind('click', likePost);

                changeLikesCounter(false);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    if (postlike_status) $('.like-button').bind('click', undoLikePost);
    else $('.like-button').bind('click', likePost);
});