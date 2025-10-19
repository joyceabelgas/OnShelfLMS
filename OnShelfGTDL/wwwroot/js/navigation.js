//document.addEventListener("DOMContentLoaded", () => {
//    console.log("DOM loaded!");

//    // Toggle sidebar (if needed)
//    window.toggleSidebar = function () {
//        document.getElementById("sidebar").classList.toggle("open");
//    };

//    // Toggle submenu dropdowns
//    window.toggleDropdown = function (event, element) {
//        event.preventDefault();
//        const submenu = element.nextElementSibling;

//        if (submenu && submenu.classList.contains("submenu")) {
//            submenu.classList.toggle("open");
//            $(".submenu").not(submenu).removeClass("open");
//        }
//    };

//    // AJAX content loading for menu links
//    $(".menu-link").on("click", function (e) {
//        const url = $(this).attr("href");

//        if (url && url !== "#") {
//            e.preventDefault();

//            $("#content").fadeOut(200, function () {
//                $.get(url, function (data) {
//                    $("#content").html(data).fadeIn(200);
//                }).fail(function () {
//                    alert("Failed to load content.");
//                });
//            });
//        }
//    });
//});
