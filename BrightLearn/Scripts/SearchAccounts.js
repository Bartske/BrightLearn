function SearchAccountsTable() {
  // Variabelen aanmaken
  var input, filter, table, tr, td, i, contains;
  input = document.getElementById("SearchInput");
  filter = input.value.toUpperCase();
  table = document.getElementById("AccountsTable");
  tr = table.getElementsByTagName("tr");

  // Door alllen tabel rijen lopen en verglijken of een waarden aan de zoekdracht voldoet
    for (i = 0; i < tr.length; i++) {
        if (!tr[i].classList.contains("Header")) {

            contains = false;
            tdFirstName = tr[i].getElementsByTagName("td")[0];
            tdMiddleName = tr[i].getElementsByTagName("td")[1];
            tdLastName = tr[i].getElementsByTagName("td")[2];

            if (tdFirstName) {
                if (tdFirstName.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (tdMiddleName) {
                if (tdMiddleName.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    contains = true;
                }
            }
            if (tdLastName) {
                if (tdLastName.innerHTML.toUpperCase().indexOf(filter) > -1) {
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
