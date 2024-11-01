/*-----------------------------------------------------------------

Template Name:  Sadaka - Non Profit Charity HTML Template
Author:  Gramentheme
Author URI: https://themeforest.net/user/gramentheme/portfolio
Developer: Kawser Ahmed Roni
Version: 1.0.0
Description: Sadaka - Non Profit Charity HTML Template

-------------------------------------------------------------------
CSS TABLE OF CONTENTS
-------------------------------------------------------------------

01. preloader
02. header
03. swiper slider
04. animated text with swiper slider
05. shop products count
06. image src change
07. hide & show a div
08. maenMenu 
09. add class & remove class
10. magnificPopup
11. back to top
12. data backgrund
13. coundown by click
14. remove products
15. wow animation

------------------------------------------------------------------*/

(function ($) {
	("use strict");

	// Preloader area start here ***
	var windowOn = $(window);
	windowOn.on("load", function () {
		$("#loading").fadeOut(500);
	});
	// Preloader area end here ***

	// Header area start here ***
	// Mobile menu
	$(".header-area nav").meanmenu();
	// Menu Fixed
	var fixed_top = $(".header-area");
	$(window).on("scroll", function () {
		if ($(this).scrollTop() > 300) {
			fixed_top.addClass("menu-fixed animated fadeInDown");
		} else {
			fixed_top.removeClass("menu-fixed fadeInDown");
		}
	});
	// Header area end here ***

	// Menu active one page js area start here ***
	$(window).scroll(function () {
		var scrollPos = $(document).scrollTop();

		$("section").each(function () {
			var offsetTop = $(this).offset().top;
			var height = $(this).height();
			var id = $(this).attr("id");

			if (scrollPos >= offsetTop && scrollPos < offsetTop + height) {
				$('nav a[href="#' + id + '"]').addClass("primary-color");
			} else {
				$('nav a[href="#' + id + '"]').removeClass("primary-color");
			}
		});
	});
	// Menu active one page js area end here ***

	// Button js area start here ***
	$(".btn-inner").mouseenter(function (e) {
		var parentOffset = $(this).offset();

		var relX = e.pageX - parentOffset.left;
		var relY = e.pageY - parentOffset.top;
		$(this).prev(".btn-circle").css({ left: relX, top: relY });
		$(this).prev(".btn-circle").removeClass("desplode-circle");
		$(this).prev(".btn-circle").addClass("explode-circle");
	});
	$(".btn-inner").mouseleave(function (e) {
		var parentOffset = $(this).offset();

		var relX = e.pageX - parentOffset.left;
		var relY = e.pageY - parentOffset.top;
		$(this).prev(".btn-circle").css({ left: relX, top: relY });
		$(this).prev(".btn-circle").removeClass("explode-circle");
		$(this).prev(".btn-circle").addClass("desplode-circle");
	});
	// Button js area end here ***

	// Banner Two slider area end here ***
	var sliderActive1 = ".banner-two__slider";
	var sliderInit1 = new Swiper(sliderActive1, {
		loop: true,
		slidesPerView: 1,
		effect: "fade",
		speed: 3000,
		autoplay: {
			delay: 7000,
			disableOnInteraction: false,
		},
		navigation: {
			nextEl: ".banner-two__arry-next",
			prevEl: ".banner-two__arry-prev",
		},
	});
	// Here this is use for animate ***
	function animated_swiper(selector, init) {
		var animated = function animated() {
			$(selector + " [data-animation]").each(function () {
				var anim = $(this).data("animation");
				var delay = $(this).data("delay");
				var duration = $(this).data("duration");
				$(this)
					.removeClass("anim" + anim)
					.addClass(anim + " animated")
					.css({
						webkitAnimationDelay: delay,
						animationDelay: delay,
						webkitAnimationDuration: duration,
						animationDuration: duration,
					})
					.one("animationend", function () {
						$(this).removeClass(anim + " animated");
					});
			});
		};
		animated();
		init.on("slideChange", function () {
			$(sliderActive1 + " [data-animation]").removeClass("animated");
		});
		init.on("slideChange", animated);
	}
	animated_swiper(sliderActive1, sliderInit1);
	// Banner Two slider area end here ***

	// Testimonial slider area start here ***
	var swiper = new Swiper(".testimonial__slider", {
		loop: "true",
		spaceBetween: 30,
		speed: 400,
		centeredSlides: true,
		autoplay: {
			delay: 4000,
			disableOnInteraction: false,
		},
		navigation: {
			nextEl: ".testimonial__arry-next",
			prevEl: ".testimonial__arry-prev",
		},
		breakpoints: {
			1299: {
				slidesPerView: 4,
			},
			991: {
				slidesPerView: 3,
			},
			575: {
				slidesPerView: 2,
			},
		},
	});
	// Testimonial slider area end here ***

	// Cause slider area start here ***
	var swiper = new Swiper(".cause__slider", {
		loop: "false",
		spaceBetween: 30,
		speed: 400,
		autoplay: {
			delay: 4000,
			disableOnInteraction: false,
		},
		navigation: {
			nextEl: ".cause__arry-next",
			prevEl: ".cause__arry-prev",
		},
		breakpoints: {
			1200: {
				slidesPerView: 4,
			},
			992: {
				slidesPerView: 3,
			},
			575: {
				slidesPerView: 2,
			},
		},
	});
	// Cause slider area end here ***

	// Brand slider area start here ***
	var swiper = new Swiper(".brand__slider", {
		loop: "true",
		spaceBetween: 30,
		speed: 300,
		autoplay: {
			delay: 3000,
			disableOnInteraction: false,
		},
		breakpoints: {
			1200: {
				slidesPerView: 5,
			},
			992: {
				slidesPerView: 4,
			},
			575: {
				slidesPerView: 3,
			},
			320: {
				slidesPerView: 2,
			},
		},
	});
	// Brand slider area end here ***

	// Hide & show by clicks js area start here ***
	$(document).on("click", "#openButton", function () {
		$("#targetElement").removeClass("sidebar__hide");
	});
	$(document).on("click", "#closeButton", function () {
		$("#targetElement").addClass("sidebar__hide");
	});
	// Hide & show by clicks js area end here ***

	// Hover over the elements with the class "service__item"
	$(".service__item").hover(
		// Function to run when the mouse enters the element
		function () {
			// Remove the "active" class from all elements
			$(".service__item").removeClass("active");
			// Add the "active" class to the currently hovered element
			$(this).addClass("active");
		}
	);
	// Hover add & remove js area end here ***

	// Background image date area start here ***
	$("[data-background").each(function () {
		$(this).css(
			"background-image",
			"url( " + $(this).attr("data-background") + "  )"
		);
	});
	// Background image date area end here ***

	// Video popup area start here ***
	$(".video-popup").magnificPopup({
		type: "iframe",
		iframe: {
			markup:
				'<div class="mfp-iframe-scaler">' +
				'<div class="mfp-close"></div>' +
				'<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
				"</div>",

			patterns: {
				youtube: {
					index: "youtube.com/",

					id: "v=",

					src: "https://www.youtube.com/embed/%id%?autoplay=1",
				},
				vimeo: {
					index: "vimeo.com/",
					id: "/",
					src: "//player.vimeo.com/video/%id%?autoplay=1",
				},
				gmaps: {
					index: "//maps.google.",
					src: "%id%&output=embed",
				},
			},

			srcAction: "iframe_src",
		},
	});
	// Video popup area end here ***

	// Map popup area start here ***
	$(".map-popup").magnificPopup({
		disableOn: 700,
		type: "iframe",
		mainClass: "mfp-fade",
		removalDelay: 160,
		preloader: false,
		fixedContentPos: false,
	});
	// Map popup area end here ***

	// Add payment amount area start here ***
	$(document).on("click", ".amount-btn", function () {
		// Remove the "active" class from all buttons
		$(".amount-btn").removeClass("active");

		// Add the "active" class to the clicked button
		$(this).addClass("active");

		// Get the text value of the clicked button
		var buttonValue = $(this).text();

		// Update the value attribute of the input element
		$(".addAmount-value").val(buttonValue);
	});
	// Add payment amount area end here ***

	// Payment button area start here ***
	$(document).on("click", ".payment-btn", function () {
		// Check if the clicked button already has the "active" class
		if ($(this).hasClass("active")) {
			// If it does, remove the "active" class
			$(this).removeClass("active");
		} else {
			// If it doesn't, remove the "active" class from all buttons
			$(".payment-btn").removeClass("active");
			// Add the "active" class to the clicked button
			$(this).addClass("active");
		}
	});
	// Payment button area end here ***

	// Counter up area start here ***
	$(".count").counterUp({
		delay: 20,
		time: 3000,
	});
	$(".progress-count").counterUp({
		delay: 20,
		time: 1000,
	});
	// Counter up area end here ***

	// Nice seclect area start here ***
	$(document).ready(function () {
		$("select").niceSelect();
	});
	// Nice seclect area end here ***

	// Footer image popup start here ***
	$(".footer-popup").magnificPopup({
		type: "image",
		gallery: {
			enabled: true,
		},
	});
	// Footer image popup end here ***

	// Back to top area start here ***
	var scrollPath = document.querySelector(".scroll-up path");
	var pathLength = scrollPath.getTotalLength();
	scrollPath.style.transition = scrollPath.style.WebkitTransition = "none";
	scrollPath.style.strokeDasharray = pathLength + " " + pathLength;
	scrollPath.style.strokeDashoffset = pathLength;
	scrollPath.getBoundingClientRect();
	scrollPath.style.transition = scrollPath.style.WebkitTransition =
		"stroke-dashoffset 10ms linear";
	var updatescroll = function () {
		var scroll = $(window).scrollTop();
		var height = $(document).height() - $(window).height();
		var scroll = pathLength - (scroll * pathLength) / height;
		scrollPath.style.strokeDashoffset = scroll;
	};
	updatescroll();
	$(window).scroll(updatescroll);
	var offset = 50;
	var duration = 950;
	jQuery(window).on("scroll", function () {
		if (jQuery(this).scrollTop() > offset) {
			jQuery(".scroll-up").addClass("active-scroll");
		} else {
			jQuery(".scroll-up").removeClass("active-scroll");
		}
	});
	jQuery(".scroll-up").on("click", function (event) {
		event.preventDefault();
		jQuery("html, body").animate(
			{
				scrollTop: 0,
			},
			duration
		);
		return false;
	});
	// Back to top area end here ***

	// WOW Animatin area start here ***
	new WOW().init();
	// WOW Animatin area start here ***
})(jQuery);
