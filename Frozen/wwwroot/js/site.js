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
            IncreaseProductQuantity(productId);
            CalculateTotalPrice();
        });
}
function ReduceFromCart(productId) {
    fetch("https://localhost:44362/cart/reducefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
            DecreaseProductQuantity(productId);
            CalculateTotalPrice();
        });
}
function RemoveFromCart(productId) {
    fetch("https://localhost:44362/cart/removefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
            HideRemovedProduct(productId);
            CalculateTotalPrice();
        });
}

function CountProductsInCart() {
    fetch("https://localhost:44362/cart/countproductsincart")
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}

function IncreaseProductQuantity(productId) {
    let productQuantityElement = document.getElementById('productinCartQuantity_' + productId);
    productQuantityElement.innerHTML = parseInt(productQuantityElement.innerHTML) + 1;
   
}

function DecreaseProductQuantity(productId) {
    let productQuantityElement = document.getElementById('productinCartQuantity_' + productId);
    let quantity = parseInt(productQuantityElement.innerHTML) - 1;
    productQuantityElement.innerHTML = quantity;

    HideProductLessThanOne(productId, quantity);
   
}

function HideProductLessThanOne(productId, quantity) {
    if (quantity < 1) {
        HideRemovedProduct(productId);
    }
    
}

function HideRemovedProduct(productId) {
    document.getElementById('productInCart_' + productId).style.display = 'none';
}

function CalculateTotalPrice() {
    fetch("https://localhost:44362/cart/calculatetotalprice/")
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("totalPrice").innerHTML = data;           
        });

}

CountProductsInCart();



// When the user scrolls the page, execute stickyFunction
window.onscroll = function () { stickyFunction() };

// Get the navbar
var navbar = document.getElementById("stickyNavbar");

// Get the offset position of the navbar
var sticky = navbar.offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
function stickyFunction() {
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
}


