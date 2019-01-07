function SearchGamesTable() {
    // Variabelen aanmaken
    var input, filter, table, tr, td, i, contains;
    input = document.getElementById("SearchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("GamesTable");
    tr = table.getElementsByTagName("tr");

    // Door alllen tabel rijen lopen en verglijken of een waarden aan de zoekdracht voldoet
    for (i = 0; i < tr.length; i++) {
        if (!tr[i].classList.contains("Header")) {

            contains = false;
            tdName = tr[i].getElementsByTagName("td")[0];
            tdLifes = tr[i].getElementsByTagName("td")[1];
            tdBonusTime = tr[i].getElementsByTagName("td")[2];

            if (tdName) {
                if (tdName.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (tdLifes) {
                if (tdLifes.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (tdBonusTime) {
                if (tdBonusTime.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (contains) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
