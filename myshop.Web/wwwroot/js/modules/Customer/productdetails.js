$(function () {
    updateNavBadge();
});

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
function removeReview() {
    confirmationMessage({
        title: "Delete review?",
        message: "This action cannot be undone.",
        confirmButtonText: "Delete review",
        cancelButtonText: "Keep review",
        confirmationCallBack: () => {
            $("#js-deleteReviewForm").trigger("submit");
        }
    });
}
