function SearchQuestionsTable() {
    // Variabelen aanmaken
    var input, filter, table, tr, td, i, contains;
    input = document.getElementById("SearchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("QuestionsTable");
    tr = table.getElementsByTagName("tr");

    // Door alllen tabel rijen lopen en verglijken of een waarden aan de zoekdracht voldoet
    for (i = 0; i < tr.length; i++) {
        if (!tr[i].classList.contains("Header")) {

            contains = false;
            tdQuestion = tr[i].getElementsByTagName("td")[0];
            tdType = tr[i].getElementsByTagName("td")[1];

            if (tdQuestion) {
                if (tdQuestion.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (tdType) {
                if (tdType.innerHTML.toUpperCase().indexOf(filter) > -1) {
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
