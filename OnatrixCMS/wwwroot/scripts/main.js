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
                observer.unobserve(entry.target); // Ta bort observer när sektionen har visats
            }
        });
    }, { threshold: 0.1 }); // Tröskelvärdet, 0.1 betyder att sektionen måste vara 10% synlig

    sections.forEach(section => {
        observer.observe(section);
    });
});

document.querySelector("form").addEventListener("submit", function () {
    window.addEventListener("beforeunload", function () {
        localStorage.setItem("scrollPosition", window.scrollY);
    });
});

window.addEventListener("load", function () {
    const scrollPosition = localStorage.getItem("scrollPosition");
    if (scrollPosition) {
        window.scrollTo({
            top: scrollPosition,
            behavior: "auto"
        });
    }
    localStorage.removeItem("scrollPosition");
});


