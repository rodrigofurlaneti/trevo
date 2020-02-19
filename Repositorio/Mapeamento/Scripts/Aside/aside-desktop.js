    $(document).ready(function () {
        var hidWidth;
        var scrollBarWidths = 40;

        var widthOfList = function () {
            var itemsWidth = 0;
            $('.navbar-desktop > ul> li').each(function () {
                var itemWidth = $(this).outerWidth();
                itemsWidth += itemWidth;
            });
            return itemsWidth;
        };

        var widthOfHidden = function () {
            return (($('.navbar-desktop').outerWidth()) - widthOfList() - getLeftPosi()) - scrollBarWidths;
        };

        var getLeftPosi = function () {
            return $('.navbar-desktop > ul> ').position().left;
        };

        var reAdjust = function () {
            if (($('.navbar-desktop').outerWidth()) < widthOfList()) {
        $('.scroller-right').removeClass('scroller-transparent');
    }
            else {
        $('.scroller-right').addClass('scroller-transparent');
    }

            if (getLeftPosi() < 0) {
        $('.scroller-left').removeClass('scroller-transparent');
    }
            else {
        $('.item').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow');
    $('.scroller-left').addClass('scroller-transparent');
            }
        }

        reAdjust();

        $(window).on('resize', function (e) {
        reAdjust();
    });

        $('.scroller-right').click(function () {
        $('.scroller-left').removeClass('scroller-transparent');
    $('.scroller-right').addClass('scroller-transparent');
            $('.navbar-desktop > ul> ').animate({left: "+=" + widthOfHidden() + "px" }, 'slow', function () {});
        });

        $('.scroller-left').click(function () {
        $('.scroller-right').removeClass('scroller-transparent');
    $('.scroller-left').addClass('scroller-transparent');
            $('.navbar-desktop > ul> ').animate({left: "-=" + getLeftPosi() + "px" }, 'slow', function () {});
        });
    });

    $(function () {
        $(".navbar-desktop > ul > li").hover(function () {
            var $container = $(this);
            var $list = $container.find("ul");
            var $anchor = $container.find("a");

            var height = ($list[0].parentElement.scrollHeight) * 1.1;
            var multiplier = height / 400;

            height += $list[0].parentElement.firstElementChild.offsetHeight;

            $container.data("origHeight", $container.height());
            $anchor.addClass("hover");
            $list.show().css({ paddingTop: $container.data("origHeight") });
            $('.navbar-desktop').css('height', height);
        }, function () {
            $(this).height($(this).data("origHeight")).find("ul").css({ top: 0 }).hide().end().find("a").removeClass("hover");
            $('.navbar-desktop').css('height', '100%')
        });
    });