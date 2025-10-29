$(document).ready(function () {
    $('.borrowed-card').each(function () {
        const statusText = $(this).find('.badge').text().trim().toLowerCase();
        const cancelButton = $(this).find('.btn-cancel');

        if (statusText === 'pending') {
            cancelButton.show();
        } else {
            cancelButton.hide();
        }
    });
});

$(document).ready(function () {
    // Show or hide cancel buttons based on status
    $('.borrowed-card').each(function () {
        const statusText = $(this).find('.badge').text().trim().toLowerCase();
        const cancelButton = $(this).find('.btn-cancel');

        if (statusText === 'pending') {
            cancelButton.show();
        } else {
            cancelButton.hide();
        }
    });

    // Cancel button click handler
    $(document).on('click', '.btn-cancel', function () {
        const borrowId = $(this).data('id');
        const isbn = $(this).data('isbn');

        if (!borrowId || !isbn) {
            showCustomAlert("Invalid book information.", "danger");
            return;
        }

        if (confirm("Are you sure you want to cancel this borrowed book?")) {
            $.ajax({
                type: "POST",
                url: "/Borrowed/CancelBorrowedBook",
                data: {
                    borrowId: borrowId,
                    isbn: isbn
                },
                success: function (response) {
                    if (response.success) {
                        showCustomAlert(response.message, "success");

                        // Optional: fade out the canceled card
                        setTimeout(() => {
                            $(`.btn-cancel[data-id='${borrowId}']`).closest('.borrowed-card').fadeOut(500);
                        }, 800);
                    } else {
                        showCustomAlert(response.message, "danger");
                    }
                },
                error: function () {
                    showCustomAlert("Something went wrong. Please try again.", "warning");
                }
            });
        }
    });
});
