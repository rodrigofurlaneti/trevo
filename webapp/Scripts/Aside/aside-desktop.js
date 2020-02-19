$(document).ready(function () {
    let larguraDeCadaMenu;
    let larguraVisivel;
    let larguraMaxParaPercorrer;
    let larguraPercorrida;

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

    function resetarMenu() {
        $('.scroller-left').addClass('scroller-transparent');
        $('.scroller-right').removeClass('scroller-transparent');

        larguraDeCadaMenu = buscarLarguraDeCadaMenu();
        larguraVisivel = parseFloat(window.getComputedStyle($('.navbar-desktop')[0]).width.replace("px", ""));
        menusVisiveis = parseInt(larguraVisivel / larguraDeCadaMenu);
        larguraMaxParaPercorrer = (($('.navbar-desktop > ul > li').length - menusVisiveis) * larguraDeCadaMenu);
        larguraPercorrida = 0;

        $('.navbar-desktop > ul').animate({ left: "0px" }, 'fast', function () { });

        if (!temProximo())
            $('.scroller-right').addClass('scroller-transparent');
    }

    function temProximo() {
        return parseFloat((larguraPercorrida + larguraDeCadaMenu).toFixed(1)) <= parseFloat((larguraMaxParaPercorrer.toFixed(1)));
    }

    function temAnterior() {
        return larguraPercorrida - larguraDeCadaMenu >= 0;
    }

    function buscarLarguraDeCadaMenu() {
        let width = parseFloat(window.getComputedStyle($('.navbar-desktop ul li:first')[0]).width.replace("px", ""));
        let marginLeft = parseFloat(window.getComputedStyle($('.navbar-desktop ul li:first')[0]).marginLeft.replace("px", ""));
        let marginRight = parseFloat(window.getComputedStyle($('.navbar-desktop ul li:first')[0]).marginRight.replace("px", ""));

        return width + marginLeft + marginRight;
    }

    resetarMenu();
    $(window).on('resize', function (e) {
        resetarMenu();
    });

    $('.scroller-right').click(function () {
        if (temProximo()) {
            $('.scroller-left').removeClass('scroller-transparent');
            larguraPercorrida += larguraDeCadaMenu;
            $('.navbar-desktop > ul').animate({ left: `-${larguraPercorrida}px` }, 'fast', function () { });

            if (!temProximo()) {
                $('.scroller-right').addClass('scroller-transparent');
            }
        }
    });

    $('.scroller-left').click(function () {
        if (temAnterior()) {
            $('.scroller-right').removeClass('scroller-transparent');
            larguraPercorrida -= larguraDeCadaMenu;
            $('.navbar-desktop > ul').animate({ left: `-${larguraPercorrida}px` }, 'fast', function () { });

            if (!temAnterior())
                $('.scroller-left').addClass('scroller-transparent');
        }
    });
});

$(function () {
    $(".navbar-desktop > ul > li").hover(function () {
        $(this).find("a").addClass("hover");
        $(".aside-desktop nav").removeClass("hidden");
    }, function () {
        $(this).find("a").removeClass("hover");
        $(".aside-desktop nav").addClass("hidden");
    });
});