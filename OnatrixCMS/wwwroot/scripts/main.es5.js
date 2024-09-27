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
                observer.unobserve(entry.target); // Ta bort observer n�r sektionen har visats
            }
        });
    }, { threshold: 0.1 }); // Tr�skelv�rdet, 0.1 betyder att sektionen m�ste vara 10% synlig

    sections.forEach(function (section) {
        observer.observe(section);
    });
});

document.querySelector("form").addEventListener("submit", function () {
    window.addEventListener("beforeunload", function () {
        localStorage.setItem("scrollPosition", window.scrollY);
    });
});

window.addEventListener("load", function () {
    var scrollPosition = localStorage.getItem("scrollPosition");
    if (scrollPosition) {
        window.scrollTo({
            top: scrollPosition,
            behavior: "auto"
        });
    }
    localStorage.removeItem("scrollPosition");
});

