
$(document).ready(function () {
    $(".book-card").on("click", function () {
        const bookImage = $(this).data("image");
        const bookTitle = $(this).data("title");
        const bookAuthor = $(this).data("author");
        const bookISBN = $(this).data("isbn");
        const bookCopyright = $(this).data("copyright");
        const bookCategory = $(this).data("category");
        const bookShelf = $(this).data("bookshelf");
        const bookStocks = $(this).data("stocks");
        const description = $(this).data("description");

        $("#modalBookImage").attr("src", bookImage || "default-book.jpg");
        $("#modalBookTitle").text(bookTitle);
        $("#modalBookAuthor").text(bookAuthor);
        $("#modalBookISBN").text(bookISBN);
        $("#modalBookCopyright").text(bookCopyright);
        $("#modalCategory").text(bookCategory);
        $("#modalBookShelf").text(bookShelf);
        $("#modalStocks").text(bookStocks);
        $("#modalBookDescription").text(description);

        // Handle stock status
        if (parseInt(bookStocks) <= 0) {
            $("#btnBorrow").prop("disabled", true).text("Out of Stock");
        } else {
            $("#btnBorrow").prop("disabled", false).text("Borrow");
        }
        // Show the modal
        const modal = new bootstrap.Modal(document.getElementById('bookModal'));
        modal.show();
    });
});


document.getElementById('borrowDate').addEventListener('change', function () {
    const borrowDate = new Date(this.value);

    if (!isNaN(borrowDate.getTime())) {
        // Add 3 days to the borrow date
        borrowDate.setDate(borrowDate.getDate() + 3);

        // Format the date back to yyyy-mm-dd
        const returnDate = borrowDate.toISOString().split('T')[0];

        // Set the return date value
        document.getElementById('returnDate').value = returnDate;
    } else {
        document.getElementById('returnDate').value = '';
    }
});

$(document).ready(function () {
    const today = new Date().toISOString().split('T')[0];
    $('#borrowDate').attr('min', today);
});

$(document).ready(function () {
    $("#btnBorrow").on("click", function () {
        let isbn = $("#modalBookISBN").text();
        let borrowDate = $("#borrowDate").val();
        let returnDate = $("#returnDate").val();
        let userId = '000000'; 

        if (!isbn || !borrowDate) {
            showCustomAlert('Please input Borrow Date!', 'danger');
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Book/SaveBorrowBooks",
            data: {
                UserID: userId,
                ISBN: isbn,
                BorrowDate: borrowDate,
                EstimatedReturnDate: returnDate
            },
            success: function (response) {
                if (response.success) {
                    showCustomAlert(response.message, "success");
                    const modalEl = document.getElementById('bookModal');
                    const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                    modalInstance.hide();
                    document.body.classList.remove('modal-open');
                    document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
                } else {
                    showCustomAlert(response.message, "danger");
                }
            },
            error: function () {
                showCustomAlert("Something went wrong. Please try again!", "warning");
            }
        });
    });
});

$(document).ready(function () {
    $("#btnReserve").on("click", function () {
        let isbn = $("#modalBookISBN").text();
        let userId = '000000';

        if (!isbn || !borrowDate) {
            showCustomAlert('Please input Borrow Date!', 'danger');
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Book/SaveReservationBooks",
            data: {
                UserID: userId,
                ISBN: isbn
            },
            success: function (response) {
                if (response.success) {
                    showCustomAlert(response.message, "success");
                    const modalEl = document.getElementById('bookModal');
                    const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                    modalInstance.hide();
                    document.body.classList.remove('modal-open');
                    document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
                } else {
                    showCustomAlert(response.message, "danger");
                }
            },
            error: function () {
                //alert("An error occurred while processing.");
                showCustomAlert("Something went wrong. Please try again!", "warning");
            }
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

$('#bookModal').on('hidden.bs.modal', function () {
    $(".modal-backdrop").remove();
    $("body").removeClass("modal-open").css("overflow", "");

    // Reset inputs if needed
    $("#borrowDate").val('');
    $("#returnDate").val('');
});

$(document).ready(function () {
    $('#btnAddToShelf').on('click', function () {
        let isbn = $("#modalBookISBN").text();

        $.ajax({
            type: 'POST',
            url: '/Book/AddToShelf',
            data: {
                isbn: isbn
            },
            success: function (response) {
                if (response.message == "Book added to MyShelf Successfuly!") {
                    showCustomAlert(response.message, 'success');
                }
                else {
                    showCustomAlert(response.message, 'danger');
                }
            },
            error: function (xhr, status, error) {
                showCustomAlert('An error occurred while adding to shelf.', 'danger');
            }
        });
    });
});
$(document).ready(function () {
    $('#searchBar').on('keyup', function () {
        var keyword = $(this).val().toLowerCase();

        $('.book-card').each(function () {
            var title = $(this).find('.card-title').text().toLowerCase();
            var author = $(this).find('.card-text').text().toLowerCase();
            var isbn = $(this).data('isbn').toString().toLowerCase();

            if (title.includes(keyword) || author.includes(keyword) || isbn.includes(keyword)) {
                $(this).closest('.col-md-3').show(); // show the card column
            } else {
                $(this).closest('.col-md-3').hide(); // hide the card column
            }
        });
    });
});