$(function () {
    var activeNavItem = $('.nav-item');

    activeNavItem.click(function () {
        activeNavItem.removeClass('active');
        $(this).addClass('active');
    });
})
function getDate() {
    var today = new Date();
    $('.date').val(today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2));
}

