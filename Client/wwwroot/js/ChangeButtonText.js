function ChangeButtonText() {
    var button = document.getElementById("addWatchlist");
    if (button) {
        button.innerHTML = '<i class="fas fa-check"></i> Added!';
    }
}
