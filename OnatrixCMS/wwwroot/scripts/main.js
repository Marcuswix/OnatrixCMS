const sideMenu = document.getElementById("sideMenu")

function showMenu() {

    sideMenu.style.right = "0";
}

function hideMenu() {
    sideMenu.style.right = "-55%";
}

document.addEventListener("DOMContentLoaded", function () {
    const sections = document.querySelectorAll('.fadeInSection');

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    sections.forEach(section => {
        observer.observe(section);
    });
});

var form = document.querySelector("form")

if (form != null) {
    form.addEventListener("submit", function () {
        localStorage.setItem("scrollPosition", window.scrollY);
    });
}

window.addEventListener("load", function () {
    const scrollPosition = localStorage.getItem("scrollPosition");

    if (scrollPosition) {
        const position = parseInt(scrollPosition, 10);
        if (!isNaN(position)) {
            window.scrollTo({
                top: position,
                behavior: "auto"
            });
        }
        localStorage.removeItem("scrollPosition");
    }
});

