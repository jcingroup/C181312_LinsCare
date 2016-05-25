    $(document).ready(function () {

        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                $('.goTop').fadeIn();
            } else {
                $('.goTop').fadeOut();
            }
        });

        /*$('a').click(function () {
            $('html, body').animate({
                scrollTop: $($.attr(this, 'href')).offset().top
            }, 1000);
            return false;
        });*/

    });