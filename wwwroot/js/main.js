$( document ).ready(function() {
    // menu

    $('.nav-button').click(function() {
        $('.navigation').toggleClass('active');
        $('.nav-bg').toggleClass('active');
        $('.nav-button').toggleClass('active');
    });

    // slider

    $('.three-cards').slick({
        dots: true,
        infinite: true,
        speed: 300,
        slidesToShow: 1,
        arrows: true,
        nextArrow: '<div class="arrow-btn arrow-next"><i class="fa fa-angle-right"></i></div>',
        prevArrow: '<div class="arrow-btn arrow-prev"><i class="fa fa-angle-left"></i></div>',
    });

    // // works
    // $('.grid').imagesLoaded( function() {
    //     var iso = new Isotope( '.grid', {
    //         itemSelector: '.element-item',
    //         layoutMode: 'fitRows'
    //     });

    //     var filterFns = {
    //         numberGreaterThan50: function( itemElem ) {
    //             var number = itemElem.querySelector('.number').textContent;
    //             return parseInt( number, 10 ) > 50;
    //         },
    //     ium: function( itemElem ) {
    //             var name = itemElem.querySelector('.name').textContent;
    //             return name.match( /ium$/ );
    //         }
    //     };

    //     var filtersElem = document.querySelector('.filters-button-group');
    //     filtersElem.addEventListener( 'click', function( event ) {
    //     if ( !matchesSelector( event.target, 'button' ) ) {
    //         return;
    //     }
    //     var filterValue = event.target.getAttribute('data-filter');
    //         filterValue = filterFns[ filterValue ] || filterValue;
    //         iso.arrange({ filter: filterValue });
    //     });

    //     var buttonGroups = document.querySelectorAll('.button-group');
    //     for ( var i=0, len = buttonGroups.length; i < len; i++ ) {
    //         var buttonGroup = buttonGroups[i];
    //         radioButtonGroup( buttonGroup );
    //     }

    //     function radioButtonGroup( buttonGroup ) {
    //         buttonGroup.addEventListener( 'click', function( event ) {
    //             if ( !matchesSelector( event.target, 'button' ) ) {
    //                 return;
    //             }
    //             buttonGroup.querySelector('.active').classList.remove('active');
    //             event.target.classList.add('active');
    //         });
    //     }
    //     $('.grid').isotope({
    //         itemSelector: '.element-item',
    //         percentPosition: true,
    //     })

    // });

    // counter

    let time = 2, cc = 1;
    $(window).scroll(function() {
        let cPos = $('.six-reviews').offset().top,
            topWindow = $(window).scrollTop();
        if(cPos < topWindow + 100) {
            if(cc < 2) {
                $('.count').each(function () {
                    $(this).prop('Counter', 0).animate ( {
                        Counter:$(this).text()
                    }, {
                        duration: 4000,
                        easing: 'swing',
                        step: function(now) {
                            $(this).text(Math.ceil(now));
                            cc = cc + 2;
                        }
                    });
                });
            }
        }
    });

    // right nav

    var lastId,
    topMenu = $(".nav-block"),
    topMenuHeight = topMenu.outerHeight(),
    menuItems = topMenu.find("a"),
    scrollItems = menuItems.map(function(){
    var item = $($(this).attr("href"));
        if (item.length) { return item; }
    });
    menuItems.click(function(e){
        var href = $(this).attr("href"),
            offsetTop = href === "#" ? 0 : $(href).offset().top-50;
        $('html, body').stop().animate({
            scrollTop: offsetTop
        }, 500);
        e.preventDefault();
    });
    $(window).scroll(function(){
        if($(window).scrollTop() == $(document).height() - $(window).height()) {
        var fromTop = $('#contacts').scrollTop()+50000;
    } else {
        var fromTop = $(this).scrollTop()+200;
    }
    var cur = scrollItems.map(function(){
        if ($(this).offset().top < fromTop)
        return this;
    });
    cur = cur[cur.length-1];
    var id = cur && cur.length ? cur[0].id : "";
    if (lastId !== id) {
        lastId = id;
        menuItems
        .parent().removeClass("active")
        .end().filter("[href='#"+id+"']").parent().addClass("active");
    }
    });

    // preloader

    $(window).on('load',function() {
        setTimeout(function() {
            $('.preloader').addClass('complete');
        }, 1700);
    });

    // first effect

    $(window).on('load', function() {
        setTimeout(function() {
            $('.page-wrapper').addClass('transform');
        }, 2000);
    });

    // first bg

    $(".first-bg").vegas({
        overlay: true,
        transition: 'fade',
        transitionDuration: 4000,
        delay: 10000,
        color: 'red',
        animation: 'random',
        animationDuration: 20000,
        slides: [
            { src: 'img/bg/bg_img1.jpg' },
            { src: 'img/bg/bg_img2.jpg' },
            { src: 'img/bg/bg_img3.jpg' },
            { src: 'img/bg/bg_img4.jpg' },
            { src: 'img/bg/bg_img5.jpg' },
        ]
    });

    // menu scroll effect

    var CurrentScroll = 0;
    $(window).scroll(function(event){
        var NextScroll = $(this).scrollTop();
        if(!$('.navigation').hasClass('active')) {
            if (NextScroll > CurrentScroll){
                if($(window).scrollTop() > $('.first').height()/2) {
                    $('.nav-button').addClass('close');
                    $('.btn-top').addClass('active');
                }
            }
            else {
                $('.nav-button').removeClass('close');
                $('.btn-top').removeClass('active');
            }
        }
        CurrentScroll = NextScroll;
    });

    $('.btn-top').click(function() {
        $('html, body').animate({
            scrollTop: 0
        }, 500);
    });
});