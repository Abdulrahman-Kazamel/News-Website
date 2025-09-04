
(function () {
    "use strict";

    /** Helpers */
    const body = document.body;
    const onScroll = (fn) => document.addEventListener("scroll", fn);
    const onLoad = (fn) => window.addEventListener("load", fn);

    /** Toggle .scrolled on body */
    const toggleScrolled = () => {
        const header = document.querySelector("#header");
        if (!header) return;
        if (!header.classList.contains("scroll-up-sticky") &&
            !header.classList.contains("sticky-top") &&
            !header.classList.contains("fixed-top")) return;

        body.classList.toggle("scrolled", window.scrollY > 100);
    };
    onScroll(toggleScrolled);
    onLoad(toggleScrolled);

    /** Mobile nav toggle */
    const mobileNavToggleBtn = document.querySelector(".mobile-nav-toggle");
    const mobileNavToggle = () => {
        body.classList.toggle("mobile-nav-active");
        mobileNavToggleBtn.classList.toggle("bi-list");
        mobileNavToggleBtn.classList.toggle("bi-x");
    };
    if (mobileNavToggleBtn) {
        mobileNavToggleBtn.addEventListener("click", mobileNavToggle);
    }

    /** Hide mobile nav on hash links */
    document.querySelectorAll("#navmenu a").forEach((link) =>
        link.addEventListener("click", () => {
            if (body.classList.contains("mobile-nav-active")) mobileNavToggle();
        })
    );

    /** Toggle mobile nav dropdowns */
    document.querySelectorAll(".navmenu .toggle-dropdown").forEach((toggle) =>
        toggle.addEventListener("click", (e) => {
            e.preventDefault();
            toggle.parentNode.classList.toggle("active");
            toggle.parentNode.nextElementSibling?.classList.toggle("dropdown-active");
            e.stopImmediatePropagation();
        })
    );

    /** Preloader */
    const preloader = document.querySelector("#preloader");
    if (preloader) onLoad(() => preloader.remove());

    /** Scroll top button */
    const scrollTop = document.querySelector(".scroll-top");
    const toggleScrollTop = () => {
        if (scrollTop) scrollTop.classList.toggle("active", window.scrollY > 100);
    };
    if (scrollTop) {
        scrollTop.addEventListener("click", (e) => {
            e.preventDefault();
            window.scrollTo({ top: 0, behavior: "smooth" });
        });
    }
    onScroll(toggleScrollTop);
    onLoad(toggleScrollTop);

    /** Init AOS */
    onLoad(() =>
        AOS.init({ duration: 600, easing: "ease-in-out", once: true, mirror: false })
    );

    /** Auto-generate carousel indicators */
    document.querySelectorAll(".carousel-indicators").forEach((indicators) => {
        const carousel = indicators.closest(".carousel");
        carousel.querySelectorAll(".carousel-item").forEach((item, i) => {
            indicators.innerHTML += `
        <li data-bs-target="#${carousel.id}" data-bs-slide-to="${i}"${i === 0 ? ' class="active"' : ""}></li>
      `;
        });
    });

    /** Init glightbox */
    GLightbox({ selector: ".glightbox" });

    /** Init PureCounter */
    new PureCounter();

    /** Init Swiper */
    const initSwiper = () => {
        document.querySelectorAll(".init-swiper").forEach((el) => {
            const config = JSON.parse(el.querySelector(".swiper-config").textContent.trim());
            el.classList.contains("swiper-tab")
                ? initSwiperWithCustomPagination(el, config)
                : new Swiper(el, config);
        });
    };
    onLoad(initSwiper);

    /** FAQ toggle */
    document.querySelectorAll(".faq-item h3, .faq-item .faq-toggle").forEach((el) =>
        el.addEventListener("click", () => {
            el.parentNode.classList.toggle("faq-active");
        })
    );

    /** Correct hash link scrolling on load */
    onLoad(() => {
        if (window.location.hash) {
            const section = document.querySelector(window.location.hash);
            if (section) {
                setTimeout(() => {
                    const marginTop = parseInt(getComputedStyle(section).scrollMarginTop);
                    window.scrollTo({ top: section.offsetTop - marginTop, behavior: "smooth" });
                }, 100);
            }
        }
    });

    /** Navmenu scrollspy */
    const navLinks = document.querySelectorAll(".navmenu a");
    const navmenuScrollspy = () => {
        const pos = window.scrollY + 200;
        navLinks.forEach((link) => {
            if (!link.hash) return;
            const section = document.querySelector(link.hash);
            if (!section) return;

            const active = pos >= section.offsetTop && pos <= section.offsetTop + section.offsetHeight;
            link.classList.toggle("active", active);
            if (active) {
                document.querySelectorAll(".navmenu a.active").forEach((l) => {
                    if (l !== link) l.classList.remove("active");
                });
            }
        });
    };
    onScroll(navmenuScrollspy);
    onLoad(navmenuScrollspy);
})();







//window.previewImage = function (event) {
//    const reader = new FileReader();
//    reader.onload = function () {
//        $("#imagePreview").attr("src", reader.result);
//    };
//    reader.readAsDataURL(event.target.files[0]);
//};

