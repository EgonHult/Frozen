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
function ReduceFromCart(productId) {
    fetch("https://localhost:44362/cart/reducefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.Ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}
function RemoveFromCart(productId) {
    fetch("https://localhost:44362/cart/removefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.Ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}
function CountProductsInCart() {
    fetch("https://localhost:44362/cart/countproductsincart/")
        .then(cartResponse => {
            if (cartResponse.Ok) {
                return cartResponse.text();
            }
        }).then(data => {
            console.log(data);
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}
CountProductsInCart();