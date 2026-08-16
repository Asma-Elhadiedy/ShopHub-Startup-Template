
(function () {
    // "use strict";

    const API_BASE = `${appBasePath}/Customer/Cart`;
    const ENDPOINTS = {
        getCart: () => `${API_BASE}/GetCart`,
        // addItem: (productId) => `${API_BASE}/AddCartItem`,
        updateQty: () => `${API_BASE}/UpdateQuantity`,
        removeItem: (itemId) => `${API_BASE}/RemoveCartItem/${itemId}`,
        clearCart: (cartId) => `${API_BASE}/ClearCart/${cartId}`,
        checkout: () => `${API_BASE}/Checkout`
    };

    const els = {
        loading: document.getElementById("cartLoading"),
        empty: document.getElementById("cartEmpty"),
        content: document.getElementById("cartContent"),
        error: document.getElementById("cartError"),
        itemsList: document.getElementById("cartItemsList"),
        template: document.getElementById("cartItemTemplate"),
        sumSubtotal: document.getElementById("sumSubtotal"),
        sumTotal: document.getElementById("sumTotal"),
        navCartCount: document.getElementById("navCartCount"),
        clearBtn: document.getElementById("clearCartBtn"),
        checkoutBtn: document.getElementById("checkoutBtn")
    };

    let cart = { id: null, items: [] };

    // ---------- helpers ----------
    const money = (n) =>
        (Number(n) || 0).toLocaleString(undefined, { style: "currency", currency: "USD" });

    function showState(state) {
        els.loading.classList.toggle("d-none", state !== "loading");
        els.empty.classList.toggle("d-none", state !== "empty");
        els.content.classList.toggle("d-none", state !== "content");
    }

    function showError(message) {
        els.error.textContent = message;
        els.error.classList.remove("d-none");
        setTimeout(() => els.error.classList.add("d-none"), 4000);
    }

    async function apiFetch(url, options = {}) {
        const res = await fetch(url, {
            headers: { "Content-Type": "application/json" },
            // credentials: "include",
            ...options
        });
        if (!res.ok) {
            let errorObject = await res.json().catch(() => null);
            if (errorObject.message)
                errorMessage(errorObject);

            const text = await res.text().catch(() => "");
            throw new Error(text || `Request failed (${res.status})`);
        }
        const contentType = res.headers.get("content-type") || "";
        return contentType.includes("application/json") ? res.json() : null;
    }

    // ---------- rendering ----------
    function render() {
        const items = cart.items || [];
        updateNavBadge(items);

        if (items.length === 0) {
            showState("empty");
            return;
        }

        els.itemsList.innerHTML = "";
        let subtotal = 0;

        items.forEach((item) => {
            const lineTotal = item.quantity * item.unitPrice;
            subtotal += lineTotal;

            const node = els.template.content.cloneNode(true);
            const row = node.querySelector(".cart-item");
            row.dataset.productId = item.productId;

            row.querySelector(".cart-item-name").textContent = item.productName;
            row.querySelector(".cart-item-price").textContent = `${money(item.unitPrice)} each`;
            row.querySelector(".cart-item-subtotal").textContent = money(lineTotal);

            const qtyInput = row.querySelector(".qty-input");
            qtyInput.value = item.quantity;

            row.querySelector(".qty-decrease").addEventListener("click", () =>
                changeQuantity(cart.id, item.id, item.productId, item.quantity - 1)
            );
            row.querySelector(".qty-increase").addEventListener("click", () =>
                changeQuantity(cart.id, item.id, item.productId, item.quantity + 1)
            );
            qtyInput.addEventListener("change", (e) => {
                const val = parseInt(e.target.value, 10);
                changeQuantity(cart.id, item.id, item.productId, isNaN(val) || val < 1 ? 1 : val);
            });

            row.querySelector(".remove-btn").addEventListener("click", () =>
                removeItem(item.id)
            );

            els.itemsList.appendChild(node);
        });

        els.sumSubtotal.textContent = money(subtotal);
        els.sumTotal.textContent = money(subtotal); // add shipping/tax logic here if needed

        showState("content");
    }

    function updateNavBadge(items) {
        const count = items.reduce((sum, i) => sum + i.quantity, 0);
        els.navCartCount.textContent = count;
    }

    // ---------- data loading ----------
    async function loadCart() {
        showState("loading");
        try {
            const data = await apiFetch(ENDPOINTS.getCart());
            cart = normalizeCart(data);
            render();
        } catch (err) {
            showState("content");
            showError("Couldn't load your cart. Please refresh the page.");
            console.error(err);
        }
    }

    // Normalize backend PascalCase (Id, Items, ProductId...) to camelCase
    function normalizeCart(data) {
        if (!data) return { id: null, items: [] };

        const items = (data.items || data.Items || []).map((i) => ({
            id: i.id ?? i.Id,
            productId: i.productId ?? i.ProductId,
            productName: i.productName ?? i.ProductName,
            quantity: i.quantity ?? i.Quantity,
            unitPrice: i.unitPrice ?? i.UnitPrice
        }));
        return { id: data.id ?? data.Id, items };
    }

    // ---------- actions ----------
    async function changeQuantity(cartId, itemId, productId, newQty) {
        if (newQty < 1) return;

        const item = cart.items.find((i) => i.productId === productId);
        if (!item) return;

        const previousQty = item.quantity;
        item.quantity = newQty; // optimistic update
        render();

        try {
            await apiFetch(ENDPOINTS.updateQty(), {
                method: "POST",
                body: JSON.stringify({
                    id: itemId,
                    quantity: newQty,
                    shoppingCartId: cartId,
                    productId: productId
                })
            });
        } catch (err) {
            item.quantity = previousQty; // rollback
            render();
            showError("Couldn't update quantity. Please try again.");
            console.error(err);
        }
    }

    async function removeItem(itemId) {
        const previousItems = cart.items;
        cart.items = cart.items.filter((i) => i.id !== itemId);
        render();

        try {
            await apiFetch(ENDPOINTS.removeItem(itemId), { method: "POST" });
        } catch (err) {
            cart.items = previousItems; // rollback
            render();
            showError("Couldn't remove item. Please try again.");
            console.error(err);
        }
    }

    async function clearCart() {
        if (!cart.items.length) return;

        confirmationMessage({
            message: "Remove all items from your cart?",
            confirmationCallBack: async () => {
                const previousItems = cart.items;
                cart.items = [];
                render();

                try {
                    await apiFetch(ENDPOINTS.clearCart(cart.id), { method: "POST" });
                } catch (err) {
                    cart.items = previousItems;
                    render();
                    showError("Couldn't clear the cart. Please try again.");
                    console.error(err);
                }
            }
        });
    }

    async function checkout() {
        els.checkoutBtn.disabled = true;
        els.checkoutBtn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Processing...`;

        try {
            const result = await apiFetch(ENDPOINTS.checkout(), { method: "GET" });
            window.location.href = (result && result.redirectUrl) || `${appBasePath}/customer/order/checkout`;
        } catch (err) {
            showError("Checkout failed. Please try again.");
            console.error(err);
            els.checkoutBtn.disabled = false;
            els.checkoutBtn.innerHTML = `Proceed to Checkout <i class="fas fa-arrow-right ms-1"></i>`;
        }
    }

    // ---------- init ----------
    els.clearBtn.addEventListener("click", clearCart);
    els.checkoutBtn.addEventListener("click", checkout);

    document.addEventListener("DOMContentLoaded", loadCart);
    if (document.readyState !== "loading") loadCart();
})();