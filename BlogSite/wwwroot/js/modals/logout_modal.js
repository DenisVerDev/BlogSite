$(document).ready(function () {
    $('#btn-logout').click(function () {
        $('#logoutModal').modal('show');
    });

    $('#btn-logoutModal-dismiss').click(function () {
        $('#logoutModal').modal('hide');
    });

    $('#btn-logoutModal-action').click(function () {
        window.location.href = '/Authentication/Logout';
    });
});