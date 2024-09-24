const sideMenu = document.getElementById("sideMenu")

function showMenu() {

    sideMenu.style.right = "0";
}

function hideMenu() {
    sideMenu.style.right = "-55%";
}

function deleteQuestion(contentId) {
    fetch('/umbraco/api/ItemController/deletequestion', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ contentId: contentId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Question deleted successfully!');
            } else {
                alert('Error: ' + data.message);
            }
        })
        .catch(error => console.error('Error:', error));
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
