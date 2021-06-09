// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


if (localStorage.getItem("filmsOnPage") == null) {
    localStorage.setItem("filmsOnPage", "5");
}

function changeNumFilmsOnPage(num) {
    localStorage.setItem("filmsOnPage", num);
    location.reload();
}

$(document).ready(function () {
    $('#uniqueId').buzinaPagination({
        itemsOnPage: Number(localStorage.getItem("filmsOnPage"))
    });
});
