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
    // Search function
    $('#searchBar').on('keyup', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.borrowed-card').each(function () {
            var cardText = $(this).text().toLowerCase();
            $(this).toggle(cardText.includes(searchTerm));
        });
    });

    // Cancel reservation function
    $('.cancel-reservation-btn').on('click', function () {
        var button = $(this);
        var isbn = button.data('isbn');

        showConfirmModal("Are you sure you want to cancel this?", function () {
            $.ajax({
                url: '/Reserved/CancelReservation',
                method: 'POST',
                data: { isbn: isbn },
                success: function (response) {
                    if (response.success) {
                        button.closest('.borrowed-card').remove();
                        showCustomAlert('Reservation cancelled.', 'success');
                    } else {
                        showCustomAlert('Failed to cancel reservation.', 'danger');
                    }
                },
                error: function () {
                    showCustomAlert('An error occurred while cancelling the reservation.', 'danger');
                }
            });
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