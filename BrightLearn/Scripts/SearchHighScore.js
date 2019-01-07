function SearchGameTable() {
    // Variabelen aanmaken
    var input, filter, table, tr, td, i, contains;
    input = document.getElementById("SearchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("GameTable");
    tr = table.getElementsByTagName("tr");

    // Door alllen tabel rijen lopen en verglijken of een waarden aan de zoekdracht voldoet
    for (i = 0; i < tr.length; i++) {
            tdGame = tr[i].getElementsByTagName("td")[0];

            if (tdGame) {
                if (tdGame.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
    }
}
