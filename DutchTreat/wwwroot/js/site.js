// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    //var theForm = $("#theForm");
    //theForm.hide();

    //var button = $("#buyButton");
    //button.on("click", function () {
    //    console.log("Buy button clicked");
    //    theForm.show();
    //});

    var productInfo = $(".product-info li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    var loginToggle = $("#loginToggle");
    var popupForm = $(".popup-form");

    loginToggle.on("click", function () {
        popupForm.slideToggle(400);
    });
});

