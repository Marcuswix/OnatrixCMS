"use strict";

var sideMenu = document.getElementById("sideMenu");

function showMenu() {

    sideMenu.style.right = "0";
}

function hideMenu() {
    sideMenu.style.right = "-55%";
}

window.scrollTo({
    top: document.getElementsByName("myForm").offsetTop,
    behavior: "smooth"
});

document.addEventListener("DOMContentLoaded", function () {
    document.body.classList.add('fade-in');
    setTimeout(function () {
        document.body.classList.add('show');
    }, 50); // F�rdr�jning f�r att s�kerst�lla att klassen l�ggs till efter att sidan har laddats
});

