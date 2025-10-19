
//function showLoading() {
//    $("#loadingOverlay").fadeIn();
//}

//function hideLoading() {
//    $("#loadingOverlay").fadeOut();
//}

//$(document).ready(function () {
//    $(".menu-link, .submenu a").on("click", function (e) {
//        e.preventDefault(); // Prevent default navigation

//        var url = $(this).attr("href");

//        if (url && url !== "javascript:void(0);") {
//            showLoading(); // Show loading animation

//            $.ajax({
//                url: url,
//                type: "GET",
//                success: function (data) {
//                    $("body").html(data); // Replace content
//                },
//                error: function () {
//                    alert("Failed to load content.");
//                },
//                complete: function () {
//                    hideLoading(); // Hide loading animation after request completes
//                }
//            });
//        } else {
//            toggleDropdown(e, this); // If no valid URL, toggle submenu
//        }
//    });
//});

//function toggleDropdown(event, element) {
//    const submenu = element.nextElementSibling;

//    if (submenu && submenu.classList.contains("submenu")) {
//        event.preventDefault();

//        // Toggle submenu visibility
//        submenu.classList.toggle("open");

//        // Optional: Close other open submenus for accordion effect
//        document.querySelectorAll(".submenu.open").forEach(otherSubmenu => {
//            if (otherSubmenu !== submenu) {
//                otherSubmenu.classList.remove("open");
//            }
//        });
//    }
//}
