"use strict";

var sideMenu = document.getElementById("sideMenu");

function showMenu() {

    sideMenu.style.right = "0";
}

function hideMenu() {
    sideMenu.style.right = "-55%";
}

document.addEventListener("DOMContentLoaded", function () {
    var sections = document.querySelectorAll('.fadeInSection');

    var observer = new IntersectionObserver(function (entries, observer) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    sections.forEach(function (section) {
        observer.observe(section);
    });
});

var form = document.querySelector("form");

if (form != null) {
    form.addEventListener("submit", function () {
        localStorage.setItem("scrollPosition", window.scrollY);
    });
}

window.addEventListener("load", function () {
    var scrollPosition = localStorage.getItem("scrollPosition");

    if (scrollPosition) {
        var position = parseInt(scrollPosition, 10);
        if (!isNaN(position)) {
            window.scrollTo({
                top: position,
                behavior: "auto"
            });
        }
        localStorage.removeItem("scrollPosition");
    }
});

