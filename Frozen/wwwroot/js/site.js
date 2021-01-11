// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddToCart(productId) {
    fetch("https://localhost:44362/cart/addtocart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}