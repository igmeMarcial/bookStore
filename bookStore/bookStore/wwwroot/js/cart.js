function addToCartAndRedirect(bookId) {
  $.post("/Cart/AddToCart", { bookId: bookId }, function (response) {
    window.location.href = "/Cart/ShoppingCart";
  });
}

document.addEventListener("DOMContentLoaded", function () {
  updateCartCount();
});
function showCartToast() {
  const toastContainer = document.querySelector(".toast-container");
  const toast = document.getElementById("cartToast");
  toastContainer.classList.add("show");
  toast.classList.add("showing");
  setTimeout(() => {
    toast.classList.remove("showing");
    toastContainer.classList.remove("show");
  }, 3000);
}

function addToCart(bookId) {
  $.post("/Cart/AddToCart", { bookId: bookId }, function (response) {
    showCartToast();
    updateCartCount();
  });
}

function updateCartCount() {
  $.get("/Cart/GetCartCount", function (response) {
    const cartCount = document.getElementById("cartCount");
    if (cartCount) {
      cartCount.textContent = response;
      if (response > 0) {
        cartCount.style.display = "flex";
      } else {
        cartCount.style.display = "none";
      }
    }
  });
}

// Función para abrir el carrito en un modal lateral
function openCartSidebar() {
  $.get("/Cart/Cart", function (response) {
    $("#cartSidebar").html(response);
    new bootstrap.Offcanvas(document.getElementById("cartOffcanvas")).show();
  });
}
