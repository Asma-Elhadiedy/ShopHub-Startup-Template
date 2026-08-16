/**
 * Checkout page logic.
 * Reuses the same cart data shape as cart.js (ShoppingCartVM: Id, Items[]
 * with ProductId, ProductName, Quantity, UnitPrice).
 *
 * Adjust API_BASE / endpoint paths below to match your actual routes.
 */
(function () {
    "use strict";

    const API_BASE = `${appBasePath}/Customer`;
    const ENDPOINTS = {
        getCart: () => `${API_BASE}/Cart/GetCart`,
        submitOrder: () => `${API_BASE}/Order/Checkout`
    };

    const els = {
        loading: document.getElementById("checkoutLoading"),
        empty: document.getElementById("checkoutEmpty"),
        content: document.getElementById("checkoutContent"),
        error: document.getElementById("checkoutError"),
        itemsList: document.getElementById("checkoutItemsList"),
        template: document.getElementById("checkoutItemTemplate"),
        sumSubtotal: document.getElementById("sumSubtotal"),
        sumTotal: document.getElementById("sumTotal"),
        form: document.getElementById("deliveryForm"),
        city: document.getElementById("city"),
        phone: document.getElementById("phone"),
        address: document.getElementById("address"),
        name: document.getElementById("name"),
        notes: document.getElementById("notes"),
        paymentMethodGroup: document.getElementById("paymentMethodGroup"),
        paymentMethodError: document.getElementById("paymentMethodError"),
        payBtn: document.getElementById("payBtn")
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
            credentials: "include",
            ...options
        });
        // console.log(res);
        // console.log(await res.text());
        if (!res.ok) {
            const text = await res.text().catch(() => "");
            throw new Error(text || `Request failed (${res.status})`);
        }
        const contentType = res.headers.get("content-type") || "";
        return contentType.includes("application/json") ? res.json() : null;
    }

    function normalizeCart(data) {
        if (!data) return { id: null, items: [] };
        const items = (data.items || data.Items || []).map((i) => ({
            productId: i.productId ?? i.ProductId,
            productName: i.productName ?? i.ProductName,
            quantity: i.quantity ?? i.Quantity,
            unitPrice: i.unitPrice ?? i.UnitPrice
        }));
        return { id: data.id ?? data.Id, items };
    }

    // ---------- rendering ----------
    function render() {
        const items = cart.items || [];

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
            const row = node.querySelector(".checkout-item");
            row.dataset.productId = item.productId;

            row.querySelector(".cart-item-name").textContent = item.productName;
            row.querySelector(".cart-item-price").textContent = `${money(item.unitPrice)} each`;
            row.querySelector(".qty-value").textContent = item.quantity;
            row.querySelector(".cart-item-subtotal").textContent = money(lineTotal);

            els.itemsList.appendChild(node);
        });

        els.sumSubtotal.textContent = money(subtotal);
        els.sumTotal.textContent = money(subtotal); // add shipping/tax logic here if needed

        showState("content");
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
            showError("Couldn't load your order. Please refresh the page.");
            console.error(err);
        }
    }

    // ---------- validation ----------
    function validateForm() {
        let isValid = true;

        [els.name, els.city, els.phone, els.address].forEach((field) => {
            const fieldValid = field.checkValidity();
            field.classList.toggle("is-invalid", !fieldValid);
            if (!fieldValid) isValid = false;
        });

        const paymentSelected = !!getSelectedPaymentMethod();
        els.paymentMethodError.classList.toggle("d-none", paymentSelected);
        if (!paymentSelected) isValid = false;

        return isValid;
    }

    function getSelectedPaymentMethod() {
        const checked = els.paymentMethodGroup.querySelector('input[name="paymentMethod"]:checked');
        return checked ? Number(checked.value) : null;
    }

    // clear the red state as the user fixes each field
    [els.name, els.city, els.phone, els.address].forEach((field) => {
        field.addEventListener("input", () => {
            if (field.checkValidity()) field.classList.remove("is-invalid");
        });
    });

    // ---------- submit ----------
    async function submitOrder() {
        if (!validateForm()) {
            showError("Please fill in all required delivery fields.");
            return;
        }

        if (!cart.items.length) {
            showError("Your cart is empty.");
            return;
        }

        const payload = {
            cartId: cart.id,
            paymentMethod: getSelectedPaymentMethod(),
            city: els.city.value.trim(),
            name: els.name.value.trim(),
            phoneNumber: els.phone.value.trim(),
            address: els.address.value.trim(),
            notes: els.notes.value.trim()
        };

        els.payBtn.disabled = true;
        els.payBtn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Processing...`;

        try {
            const result = await apiFetch(ENDPOINTS.submitOrder(), {
                method: "POST",
                body: JSON.stringify(payload)
            });

            window.location.href = (result && result.redirectUrl) || `${API_BASE}/order`;
        } catch (err) {
            showError("Couldn't place your order. Please try again.");
            console.error(err);
            els.payBtn.disabled = false;
            els.payBtn.innerHTML = `Proceed to Payment <i class="fas fa-arrow-right ms-1"></i>`;
        }
    }

    // ---------- init ----------
    els.payBtn.addEventListener("click", submitOrder);

    document.addEventListener("DOMContentLoaded", loadCart);
    if (document.readyState !== "loading") loadCart();
})();