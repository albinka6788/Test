(function() {

/*
# =============================================================================
#   Equalize Column
# =============================================================================
*/  

/**
 * equalize.js
 * Author & copyright (c) 2012: Tim Svensen
 * Dual MIT & GPL license
 *
 * Page: http://tsvensen.github.com/equalize.js
 * Repo: https://github.com/tsvensen/equalize.js/
 */
(function(b){b.fn.equalize=function(a){var d=!1,g=!1,c,e;b.isPlainObject(a)?(c=a.equalize||"height",d=a.children||!1,g=a.reset||!1):c=a||"height";if(!b.isFunction(b.fn[c]))return!1;e=0<c.indexOf("eight")?"height":"width";return this.each(function(){var a=d?b(this).find(d):b(this).children(),f=0;a.each(function(){var a=b(this);g&&a.css(e,"");a=a[c]();a>f&&(f=a)});a.css(e,f+"px")})}})(jQuery);

  $(function() {
    $('.equalize').equalize({children:'.content-box'});
    $(window).resize(function(){  
      $('.equalize').equalize({reset:true, children:'.content-box'});
    });
  });



/*
# =============================================================================
#   Hero Unit Full Height
# =============================================================================
*/

  var H = $(window).height() -200;
  $('.hero-unit').height(H);



/*
# =============================================================================
#   Text Slider
# =============================================================================
*/ 
  $(".tagline").owlCarousel({
      slideSpeed : 300,
      paginationSpeed : 200,
      singleItem:true,
      autoPlay:true,
      transitionStyle : "backSlide"
  });

/*
# =============================================================================
#   Number Increment Decrement
# =============================================================================
*/ 
  	window.inputNumber = function(el) {

    var min = el.attr('min') || false;
    var max = el.attr('max') || false;

    var els = {};

    els.dec = el.prev();
    els.inc = el.next();

    el.each(function() {
      init($(this));
    });

    function init(el) {

      els.dec.on('click', decrement);
      els.inc.on('click', increment);

      function decrement() {
        var value = el[0].value;
        value--;
        if(!min || value >= min) {
          el[0].value = value;
        }
      }

      function increment() {
        var value = el[0].value;
        value++;
        if(!max || value <= max) {
          el[0].value = value++;
        }
      }
    }
  }
  inputNumber($('.input-number'));

/*
# =============================================================================
#   Tooltip
# =============================================================================
*/
    var targets = $( '.tooltip' ),
      target  = false,
      tooltip = false,
      title = false;

    targets.bind( 'mouseenter', function()
    {
      target  = $( this );
      tip   = target.attr( 'title' );
      tooltip = $( '<div class="tooltip-content"></div>' );

      if( !tip || tip == '' )
        return false;

      target.removeAttr( 'title' );
      tooltip.css( 'opacity', 0 )
           .html( tip )
           .appendTo( 'body' );

      var init_tooltip = function()
      {
        if( $( window ).width() < tooltip.outerWidth() * 1.5 )
          tooltip.css( 'max-width', $( window ).width() / 1.5 );
        else
          tooltip.css( 'max-width', 340 );

        var pos_left = target.offset().left + ( target.outerWidth() / 2 ) - ( tooltip.outerWidth() / 2 ),
          pos_top  = target.offset().top - tooltip.outerHeight() - 25;

        if( pos_left < 0 )
        {
          pos_left = target.offset().left + target.outerWidth() / 2 - 20;
          tooltip.addClass( 'left' );
        }
        else
          tooltip.removeClass( 'left' );

        if( pos_left + tooltip.outerWidth() > $( window ).width() )
        {
          pos_left = target.offset().left - tooltip.outerWidth() + target.outerWidth() / 2 + 20;
          tooltip.addClass( 'right' );
        }
        else
          tooltip.removeClass( 'right' );

        if( pos_top < 0 )
        {
          var pos_top  = target.offset().top + target.outerHeight() + 10;
          tooltip.addClass( 'top' );
          console.log(pos_top);
        }
        else
          tooltip.removeClass( 'top' );

        tooltip.css( { left: pos_left, top: pos_top } )
             .animate( { top: '+=10', opacity: 1 }, 50 );
      };

      init_tooltip();
      $( window ).resize( init_tooltip );

      var remove_tooltip = function()
      {
        tooltip.animate( { top: '-=10', opacity: 0 }, 50, function()
        {
          $( this ).remove();
        });

        target.attr( 'title', tip );
      };

      target.bind( 'mouseleave', remove_tooltip );
      tooltip.bind( 'click', remove_tooltip );
    });

/*
# =============================================================================
#   Templates
# =============================================================================
*/
  $.ajax({url: "templates/save-for-later.html", success: function(result){
      $(".sidebar").html(result);
  }});
  $.ajax({url: "templates/header.html", success: function(result){
      $(".with-nav").html(result);
  }});
  $.ajax({url: "templates/footer.html", success: function(result){
      $("footer").html(result);
  }});
  $.ajax({url: "templates/header-no-nav.html", success: function(result){
      $(".no-nav").html(result);
  }});
     

/*
# =============================================================================
#   Responsive Menu
# =============================================================================
*/

  $('.menu-open').on('click', function () {
      $('.navbar').addClass('reveal');
      $('.overlay').addClass('on');
  });
  $('.menu-close').on('click', function () {
      $('.navbar').removeClass('reveal');
      $('.overlay').removeClass('on');
  });

  $(window).resize(function () {
      $('.overlay').removeClass('on');
  });

})();

