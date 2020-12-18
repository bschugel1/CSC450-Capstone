//Filter/Search List Reference: https://www.w3schools.com/howto/howto_js_filter_lists.asp provided by W3 schools
function searchList() {
    // Declare variables
    var input, filter, ul, li, a, i, txtValue;
    input = document.getElementById('searchField');
    filter = input.value.toUpperCase();
    ul = document.getElementById("list");
    li = ul.getElementsByTagName('li');

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}