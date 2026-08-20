
let beforeLoginCartId = $("#js-beforeLoginCartId").val();

$(function () {
    filterProductsByCategory(0);

    if (beforeLoginCartId && beforeLoginCartId > 0) {
        confirmationMessage(
            {
                title: "Old cart from your last login!",
                message: "Would you like to restore it & continue shopping?",
                confirmButtonText: "Restore my cart",
                cancelButtonText: "Keep current cart",
                confirmationCallBack: () => {
                    $.post(`${appBasePath}/Customer/Cart/ClearCart/${beforeLoginCartId}`)
                        .done((success) => {
                            updateNavBadge();
                        });

                }
            });
    }

    updateNavBadge();
});

function filterProductsByCategory(categoryId) {

    $.get(`${appBasePath}/Customer/Home/GetProducts/${categoryId}`)
        .done((view) => {
            $("#js-productsSection").html(view);
            $(".category-btn").removeClass("active");
            $(`#js-category-${categoryId}`).addClass("active");
        }).fail((error) => {
            errorMessage(error);
            console.error(error);
        });
}

function addToCart(productId) {
    let url = `${appBasePath}/Customer/Cart/AddCartItem/${productId}`;

    $.post(url)
        .done((success) => {
            successMessage(success);
            updateNavBadge();
        }).fail((error) => {
            // console.log(error);
            errorMessage(error);
        });

}



const categoryIcons = {
    'all categories': '🗂️',
    'electronics': '🔌',
    "computers & laptops": '💻',
    'mobile phones': '📱',
    'home appliances': '🧺',
    'kitchen & dining': '🍽️',
    'furniture': '🛋️',
    'office supplies': '📎',
    'books': '📚',
    "men's clothing": '👔',
    "women's clothing": '👗',
    'kids & baby': '🧸',
    'shoes': '👟',
    'beauty & personal care': '💄',
    'health & wellness': '💊',
    'sports & fitness': '🏋️',
    'outdoor & camping': '⛺',
    'automotive': '🚗',
    'tools & hardware': '🔧',
    'pet supplies': '🐾',
    'toys & games': '🎲',
    'gaming': '🎮',
    'cameras & photography': '📷',
    'audio & headphones': '🎧',
    'smart home': '🏠',
    'garden & outdoor': '🌿',
    'jewelry & watches': '💍',
    'groceries': '🛒',
    'cleaning supplies': '🧽',
    'fashion': '👕',
    'gift cards & vouchers': '🎁',
};

$('.category-icon').each(function () {
    const el = $(this);
    const key = el.data('icon-for');
    el.text(categoryIcons[key] || '🏷️')
});

