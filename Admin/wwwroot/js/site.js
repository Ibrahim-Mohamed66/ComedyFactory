$(() => {
  //SVG converter
  const svgConverter = function () {
    $("img.svg").each(function () {
      var $img = jQuery(this);
      var imgID = $img.attr("id");
      var imgClass = $img.attr("class");
      var imgURL = $img.attr("src");
      jQuery.get(
        imgURL,
        function (data) {
          var $svg = jQuery(data).find("svg");
          if (typeof imgID !== "undefined") {
            $svg = $svg.attr("id", imgID);
          }
          if (typeof imgClass !== "undefined") {
            $svg = $svg
              .attr("class", imgClass + " replaced-svg")
              .css("display", "inline-block");
          }
          $svg = $svg.removeAttr("xmlns:a");
          $img.replaceWith($svg);
        },
        "xml"
      );
    });
  };
  svgConverter();

  $("img").on("error", function () {
    var resize = $(this).attr("src").split("?")[1];
    $(this).attr(
      "src",
      "/images/placeholder.jpg" +
        (resize != null ? "?" + resize : "?w=500&h=500&mode=crop&scale=both")
    );
    $(this).addClass("img-fluid");
  });

  // start opinion-slider
  const opinionSlider = $(".opinion-slider");
  if (opinionSlider.length > 0) {
    opinionSlider.slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      centerMode: true,
      centerPadding: "500px",
      responsive: [
        {
          breakpoint: 1800,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            dots: true,
            centerMode: true,
            centerPadding: "300px",
          },
        },
        {
          breakpoint: 1300,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 780,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
      ],
    });
  }

  // end  opinion-slider
  // start opinion-slider-ar
  const opinionSliderAr = $(".opinion-slider-ar");
  if (opinionSliderAr.length > 0) {
    opinionSliderAr.slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      centerMode: true,
      rtl: true,
      centerPadding: "500px",
      responsive: [
        {
          breakpoint: 1800,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            dots: true,
            centerMode: true,
            centerPadding: "300px",
          },
        },
        {
          breakpoint: 1300,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 780,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
      ],
    });
  }

  // end  opinion-slider-ar

  // start mop-cover-slider
  const mopSlider = $(".mop-slider");
  if (mopSlider.length > 0) {
    mopSlider.slick({
      slidesToShow: 5,
      slidesToScroll: 1,
      dots: true,
      centerMode: true,
      centerPadding: "100px",
      responsive: [
        {
          breakpoint: 1500,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "100px",
          },
        },
        {
          breakpoint: 780,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
      ],
    });
  }
  const mopSliderAr = $(".mop-slider-ar");
  if (mopSliderAr.length > 0) {
    mopSliderAr.slick({
      slidesToShow: 5,
      slidesToScroll: 1,
      dots: true,
      centerMode: true,
      rtl: true,
      // centerPadding: "300px",
      responsive: [
        {
          breakpoint: 1500,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "100px",
          },
        },
        {
          breakpoint: 780,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            centerMode: true,
            centerPadding: "50px",
          },
        },
      ],
    });
  }
});

// end mop-cover-slider

// start burger menu

$(".icon-menu").click(function () {
  $(".icon-menu").toggleClass("bom", "ar");
  $(".show-menu").toggleClass("open");
  $(".arbic").toggleClass("put");
});

// end burger menu

$(".explore-inside li a").click(function (e) {
  e.preventDefault();

  $(this)
    .addClass("active")
    .parent()
    .siblings()
    .find("a")
    .removeClass("active");
});

$(".explore-inside li a").click(function () {
  $("html,body").animate(
    {
      scrollTop: $("#" + $(this).data("value")).offset().top + 1,
    },
    100
  );
});

$(document).ready(function () {
  $(window).scroll(function () {
    var scroll = $(window).scrollTop();
    if (scroll > 80) {
      $(".menu").addClass("nav-active");
    } else {
      $(".menu").removeClass("nav-active");
    }
  });
});
