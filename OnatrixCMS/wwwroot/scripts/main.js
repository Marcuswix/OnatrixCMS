const sideMenu = document.getElementById("sideMenu")

function showMenu() {

    sideMenu.style.right = "0";
}

function hideMenu() {
    sideMenu.style.right = "-55%";
}

document.addEventListener('DOMContentLoaded', function () {
    var myCarousel = new bootstrap.Carousel('#carouselExampleControls');
});
