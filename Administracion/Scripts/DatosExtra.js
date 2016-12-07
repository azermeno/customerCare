$(document).ready(function () {
    $(".rtbUL").children().slice(1,3).hide();
    $(".rtbUL>:eq(0)").click(function () {
        $(".rtbUL").children().slice(1,3).fadeIn();
    });
});

function guardar() {
    $(".rtbUL>:eq(2)").click();
}