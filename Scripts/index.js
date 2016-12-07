if (window != top) {
    try {
        parent.location = "index.aspx?cad=si&url=" + encodeURIComponent(parent.location.pathname + parent.location.search);
    }
    catch (er) {
    }
}