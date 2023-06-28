function ShowDetails() {
    document.getElementById("panel").style.visibility = "visible";

    var button = document.getElementById("addWatchlist");
    if (button) {
        button.innerHTML = '<i class="fas fa-plus"></i> Add to Watchlist';
    }
}
