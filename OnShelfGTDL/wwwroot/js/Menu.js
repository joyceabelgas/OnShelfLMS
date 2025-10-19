function initializeMenuBehavior() {
    const links = document.querySelectorAll(".menu-link");
    const loadingOverlay = document.getElementById("loadingOverlay");

    if (loadingOverlay) loadingOverlay.style.display = "none";
    CheckHamburger();

    const userdetails = document.getElementById("user-details");

    if (window.innerWidth <= 768) {
        userdetails.classList.add("show-flex");
        userdetails.classList.remove("hide");
    } else {
        userdetails.classList.add("hide");
        userdetails.classList.remove("show-flex");
    }

    if (location.pathname !== "/") {
        updateActiveLinks(location.pathname);
    }

    // Submenu clicks
    document.querySelectorAll('.submenu-link').forEach(submenuLink => {
        submenuLink.addEventListener('click', function (event) {
            const submenu = submenuLink.closest('.submenu');
            const parentMenu = submenu?.previousElementSibling;

            if (parentMenu && parentMenu.classList.contains('menu-link')) {
                parentMenu.classList.add('active');
            }

            updateActiveLinks(location.pathname);
        });
    });

    // Main menu click logic
    links.forEach(link => {
        link.addEventListener("click", function (event) {
            const submenu = link.nextElementSibling;

            if (submenu && submenu.classList.contains("submenu")) {
                event.preventDefault();
                submenu.classList.toggle("open");
                loadingOverlay.style.display = "none";
                return;
            }

            loadingOverlay.style.display = "flex";
        });
    });
}
function updateActiveLinks(url) {
    // Reset all states
    document.querySelectorAll('.submenu-link').forEach(link => link.classList.remove('active'));
    document.querySelectorAll('.menu-link').forEach(link => link.classList.remove('active'));
    document.querySelectorAll('.submenu').forEach(sub => sub.classList.remove('open'));

    // Sort links so longer hrefs (more specific) are checked first
    const allLinks = Array.from(document.querySelectorAll('a'))
        .filter(link => link.getAttribute('href'))
        .sort((a, b) => b.getAttribute('href').length - a.getAttribute('href').length);

    // Find and activate the best matching link
    allLinks.forEach(link => {
        const href = link.getAttribute('href');

        // Match exact URL or when url starts with href (not just contains)
        if (url === href || url.startsWith(href + "/") || url.startsWith(href + "?")) {
            link.classList.add('active');

            const submenu = link.closest('.submenu');
            if (submenu) {
                submenu.classList.add('open');

                const parentMenu = submenu.previousElementSibling;
                if (parentMenu && parentMenu.classList.contains('menu-link')) {
                    parentMenu.classList.add('active');
                }
            }
        }
    });
}



document.addEventListener("DOMContentLoaded", initializeMenuBehavior);
function loadCategory(category) {
    document.getElementById("loadingOverlay").style.display = "flex";

    let prefix = category.includes('-') ? category.split('-')[0] + '-' : '';

    if (prefix == 'E-') {
        $.ajax({
            url: '/Book/GetEBookList',
            type: 'GET',
            data: { category: category },
            success: function (data) {
                $('body').html(data);

                // Push new state to history
                const newUrl = `/Book/GetEBookList?category=${encodeURIComponent(category)}`;
                history.pushState({ category: category }, '', newUrl);

                updateActiveLinks(newUrl);

            },
            error: function () {
                alert("Failed to load books for " + category);
            },
            complete: function () {
                document.getElementById("loadingOverlay").style.display = "none";
            }
        });
    }
    else {
        $.ajax({
            url: '/Book/GetBookList',
            type: 'GET',
            data: { category: category },
            success: function (data) {
                $('body').html(data);

                // Push new state to history
                const newUrl = `/Book/GetBookList?category=${encodeURIComponent(category)}`;
                history.pushState({ category: category }, '', newUrl);

                updateActiveLinks(newUrl);

            },
            error: function () {
                alert("Failed to load books for " + category);
            },
            complete: function () {
                document.getElementById("loadingOverlay").style.display = "none";
            }
        });
    }
    
}



//------for Dropdown Menu only------
function toggleDropdown(event, element) {
    const submenu = element.nextElementSibling;

    if (submenu && submenu.classList.contains("submenu")) {
        event.preventDefault(); // Prevent navigation
        submenu.classList.toggle("open");

        // Add/remove 'active' class on parent menu
        if (submenu.classList.contains("open")) {
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }

        // Optional: Close other open submenus (accordion effect)
        document.querySelectorAll(".submenu.open").forEach(otherSubmenu => {
            if (otherSubmenu !== submenu) {
                otherSubmenu.classList.remove("open");

                // Remove 'active' from other parent menu links
                const parentLink = otherSubmenu.previousElementSibling;
                if (parentLink && parentLink.classList.contains("menu-link")) {
                    parentLink.classList.remove("active");
                }
            }
        });

        // Don't show loading overlay for dropdowns
        document.getElementById("loadingOverlay").style.display = "none";
    }
}


function toggleSidebar() {
    const sidebar = document.getElementById("sidebar");

    // Check if the screen width is mobile (<= 768px)
    if (window.innerWidth <= 768) {
        sidebar.classList.toggle("open");
    }
    else {
        if (sidebar.classList.contains('open')) {
            sidebar.classList.remove('open');
            sidebar.classList.add('close');
            localStorage.setItem("sidebarState", "closed");
            return;
        }
        if (sidebar.classList.contains('close')) {
            sidebar.classList.remove('close');
            sidebar.classList.add('open');
            localStorage.setItem("sidebarState", "open");
            return;
        }
    }
    

}

function CheckHamburger() {
    const sidebar = document.getElementById("sidebar");
    const userdetails = document.getElementById("user-details");
    // Read the saved state from localStorage
    const savedState = localStorage.getItem("sidebarState");

    if (window.innerWidth <= 768) {
        // If the sidebar is saved as open, open it; otherwise, close it
        if (savedState === "open") {
            sidebar.classList.remove("open");
        }
    } else {
        // On desktop, sidebar should default to open state
        sidebar.classList.remove("open", "close"); // Reset both states
        //sidebar.classList.add("open");
        userdetails.classList.style = "none";
        if (savedState === "open") {
            sidebar.classList.add("open");
        } else if (savedState === "closed") {
            sidebar.classList.add("close");
        }
    }
}

document.addEventListener("click", function (event) {
    const sidebar = document.getElementById("sidebar");
    const hamburger = document.querySelector(".hamburger-menu");

    if (window.innerWidth <= 768) {
        if (!sidebar.contains(event.target) && !hamburger.contains(event.target)) {
            sidebar.classList.remove("open");
            localStorage.setItem("sidebarState", "closed");
        }
    }
});


//function CheckHamburger() {
//    const sidebar = document.getElementById("sidebar");
//    const hamburger = document.querySelector(".hamburger-menu");

//    if (window.innerWidth <= 768) {
//        sidebar.classList.remove("open");
//    }
//}

//// Optional: Close sidebar when clicking outside
//document.addEventListener("click", function (event) {
//    const sidebar = document.getElementById("sidebar");
//    const hamburger = document.querySelector(".hamburger-menu");

//    if (window.innerWidth <= 768) { // Only apply for mobile
//        if (!sidebar.contains(event.target) && !hamburger.contains(event.target)) {
//            sidebar.classList.remove("open");
//        }
//    }
//});

function toggleProfileDropdown() {
    const dropdown = document.getElementById("profileDropdown");
    dropdown.classList.toggle("show");
}

window.addEventListener("popstate", function () {
    document.getElementById("loadingOverlay").style.display = "none";
});

function showCustomAlert(message, type = "success", timeout = 3000) {
    const alertBox = document.getElementById("customAlert");

    if (alertBox) {
        // Set message and alert type (Bootstrap classes)
        document.getElementById("alertMessage").innerText = message;
        alertBox.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x shadow`;

        // Show the alert
        alertBox.classList.remove("d-none");

        // Auto-hide after timeout
        const timer = setTimeout(() => {
            hideCustomAlert();
        }, timeout);

        // Ensure timeout clears if manually closed
        alertBox.setAttribute("data-timer", timer);
    }
}

// Reset and hide the alert completely
function hideCustomAlert() {
    const alertBox = document.getElementById("customAlert");
    if (alertBox) {
        alertBox.classList.add("d-none");
        alertBox.classList.remove("alert-success", "alert-danger", "alert-warning", "fade", "show");

        // Clear any existing timeout to avoid double hiding
        const timer = alertBox.getAttribute("data-timer");
        if (timer) clearTimeout(timer);
    }
}

function checkForNotifications() {
    $.ajax({
        url: '/Home/CheckNotification',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.hasNotification) {
                showCustomAlert(data.message, "success");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error checking notifications:", error);
        }
    });
}

// Call on page load and poll every 5 seconds
$(document).ready(function () {
    checkForNotifications();

    setInterval(function () {
        checkForNotifications();
    }, 2000); // 2 seconds
});
