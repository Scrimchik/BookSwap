/*Логин*/
$('#login_open').click(function(){
	$('.login').fadeToggle(300);
});
/*Регистрация*/
$('#registration_open').click(function () {
    if ($('#registration').hasClass('registration noshow')) {
        $('#registration').fadeIn(200);
        $('#registration').attr('class', 'registration show')
        $($('.container').children()).not('.hidden-content', $('.hidden-content').children()).css('opacity', '0.5');
    }
});
$(document).mouseup(function (e) {
    if (!$('#registration').is(e.target) && $('#registration').has(e.target).length == 0 && $('#registration').hasClass('registration show')) {
        $('#registration').fadeOut(200);
        $('#registration').attr('class', 'registration noshow');
        $($('.container').children()).css('opacity', '1');
    }
}); 
$("#input-img").change(function () {
    readUrl(this);
});
$('#change-image').change(function () {
    readUrl(this);
});
/*Вставки картинок с инпутов*/
function readUrl(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
/*Уведомление незарегестрированным пользователям*/
$('.non-auth').click(function () {
    $('.non-auth-alert').css('display', 'inline-block')
});