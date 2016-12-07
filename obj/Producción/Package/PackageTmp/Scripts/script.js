
function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}

function tr(cols,par,n) {
    if (par[0]==n)
        return true;
    var next=[par[0],par[1]+1];
    if (next[1]>n)
        next=[par[0]+1,par[0]+2];
    for (col in cols) {
        for (p in col) {
            if (p[0]=par[0] or p[1]=par[0]){
                for (q in col) {
                    if (
