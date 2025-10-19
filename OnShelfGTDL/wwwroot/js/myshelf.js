$(document).ready(function () {
    $('#searchBar').on('keyup', function () {
        var searchTerm = $(this).val().toLowerCase();

        $('.borrowed-card').each(function () {
            var cardText = $(this).text().toLowerCase();

            if (cardText.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});
$(document).ready(function () {
    $('.remove-myshelf-btn').on('click', function () {
        var button = $(this);
        var isbn = button.closest('.borrowed-card').find("p:contains('ISBN')").text().replace("ISBN:", "").trim();

        showConfirmModal("Are you sure you want to remove this?", function () {
            $.ajax({
                url: '/MyShelf/RemoveFromShelf',
                method: 'POST',
                data: { isbn: isbn },
                success: function (response) {
                    if (response.success) {
                        button.closest('.borrowed-card').remove();
                        showCustomAlert('Book removed from your shelf.', 'success');
                    } else {
                        showCustomAlert('Failed to remove book.', 'danger');
                    }
                },
                error: function () {
                    showCustomAlert('An error occurred.', 'danger');
                }
            });
        });

    });

    // Optional: Search feature
    $('#searchBar').on('keyup', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.borrowed-card').each(function () {
            var cardText = $(this).text().toLowerCase();
            $(this).toggle(cardText.includes(searchTerm));
        });
    });
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

let confirmCallback = null;

function showConfirmModal(message, yesCallback) {
    document.getElementById('confirmModalMessage').textContent = message;
    confirmCallback = yesCallback;
    var myModal = new bootstrap.Modal(document.getElementById('confirmModal'));
    myModal.show();
}

document.getElementById('confirmYesBtn').addEventListener('click', function () {
    if (typeof confirmCallback === 'function') {
        confirmCallback();
    }
    const modalEl = document.getElementById('confirmModal');
    bootstrap.Modal.getInstance(modalEl).hide();
    confirmCallback = null;
});