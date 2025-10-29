let _selectedID = null;
let _isbn = null;
let _status = null;
let _userID = null;

document.addEventListener('DOMContentLoaded', function () {

    loadBorrowBookData();

    const filterDropdown = document.getElementById('cmbFilter');
    const dateFilterSection = document.querySelector('.datefilter');
    const filterBtn = document.getElementById('filterBtn');

    dateFilterSection.style.display = 'none';

    filterDropdown.addEventListener('change', () => {
        const value = filterDropdown.value;

        dateFilterSection.style.display = value === "Borrow Date" ? 'block' : 'none';

        if (value !== "Filter" && value !== "Borrow Date") {
            filterByStatus(value);
        } else {
            showAllRows();
        }
    });

    filterBtn.addEventListener('click', () => {
        filterByDateRange();
    });
});

function showAllRows() {
    document.querySelectorAll('#userTable tbody tr').forEach(row => {
        row.style.display = '';
    });
}

function filterByStatus(status) {
    document.querySelectorAll('#userTable tbody tr').forEach(row => {
        const rowStatus = row.cells[7]?.innerText.trim();
        row.style.display = (rowStatus === status) ? '' : 'none';
    });
}

function filterByDateRange() {
    const fromInput = document.getElementById('dateFrom').value;
    const toInput = document.getElementById('dateTo').value;

    const fromDate = fromInput ? new Date(fromInput) : null;
    const toDate = toInput ? new Date(toInput) : null;

    const rows = document.querySelectorAll('#userTable tbody tr');
    console.log("Found rows:", rows.length);

    if (rows.length === 0) {
        console.warn("No rows found. The table may not be loaded yet.");
        return;
    }

    rows.forEach(row => {
        const text = row.cells[4]?.innerText.trim();
        if (!text) {
            row.style.display = 'none';
            return;
        }

        const borrowDate = new Date(text);
        if (isNaN(borrowDate)) {
            console.warn("Invalid date format:", text);
            row.style.display = 'none';
            return;
        }

        const inRange =
            (!fromDate || borrowDate >= fromDate) &&
            (!toDate || borrowDate <= toDate);

        row.style.display = inRange ? '' : 'none';
    });
}


$(document).ready(function () {
    $('#cmbFilter').on('change', function () {
        var selected = $(this).val();
        var placeholderText = 'Search';

        if (selected === 'Approved') {
            placeholderText = 'Search Approved';
        } else if (selected === 'Pending') {
            placeholderText = 'Search Pending';
        } else if (selected === 'Denied') {
            placeholderText = 'Search Denied';
        } 
        $('#searchBar').attr('placeholder', placeholderText);
    });
});
function loadBorrowBookData() {
    fetch('/Book/GetBorrowBook')
        .then(response => response.json())
        .then(data => {
            const tableBody = document.querySelector('#userTable tbody');
            tableBody.innerHTML = '';

            data.forEach(record => {
                const row = `
                    <tr data-id="${record.id}" data-isbn="${record.isbn}" data-status="${record.status}" data-userID="${record.userID}" data-id="${record.id}">
                        <td style="display:none;">${record.id}</td>
                        <td style="display:none;">${record.userID}</td>
                        <td>${record.name}</td>
                        <td>${record.isbn}</td>
                        <td>${formatDate(record.borrowDate)}</td>
                        <td>${formatDate(record.estimatedReturnDate)}</td>
                        <td>${record.actualReturnDate ? formatDate(record.actualReturnDate) : 'N/A'}</td>
                        <td>${record.status}</td>
                        <td>${record.approvedBy}</td>
                        <td>${record.approvedDate ? formatDate(record.approvedDate) : 'N/A'}</td>
                    </tr>`;
                tableBody.innerHTML += row;
            });

            filterByDateRange();
            bindRowClickEvent();
        })
        .catch(error => console.error('Error loading data:', error));
}


function bindRowClickEvent() {
    document.querySelectorAll('#userTable tbody tr').forEach(row => {
        row.addEventListener('click', function () {
            _selectedID = this.getAttribute('data-id');
            _isbn = this.getAttribute('data-isbn');
            _status = this.getAttribute('data-status');
            _userID = this.getAttribute('data-userID');
            _id = this.getAttribute('data-id');

            clearFines();

            if (_status === 'Approved' || _status === 'Returned' || _status === 'Returned w/ Fines') {
                LoadReturn(_id, _status);
                return;
            }
            if (_status === 'Denied') {
                showCustomAlert("Book request already Denied", "danger");
                return;
            }

            if (_isbn && _status === 'Pending') {
                fetchBookDetails(_isbn);
            } else {
                showCustomAlert("ISBN not found for this row.", "danger");
            }
        });
    });
}

function fetchBookDetails(isbn) {
    fetch(`/Book/GetBookByISBN?isbn=${isbn}`)
        .then(response => response.json())
        .then(book => {
            $("#modalBookImage").attr("src", book.imageUrl || "default-book.jpg");
            $("#modalBookTitle").text(book.title);
            $("#modalBookAuthor").text(book.author);
            $("#modalBookISBN").text(book.isbn);
            $("#modalBookCopyright").text(book.publicationYear);
            $("#modalCategory").text(book.category);
            $("#modalBookShelf").text(book.bookshelf);
            $("#modalStocks").text(book.stocks);
            $("#modalBookDescription").text(book.description);

            $("#bookModal").modal('show');
        })
        .catch(error => console.error('Error fetching book details:', error));
}


function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return isNaN(date.getTime()) ? 'N/A' : date.toLocaleDateString();
}


$(document).ready(function () {
    $("#btnApprove").on("click", function () {

        if (!_selectedID) {
            alert("Please reselect a row");
            return;
        }

        $.ajax({
            type: "POST",
            url: `/Book/UpdateBorrowBooks`,
            data: { id: _selectedID, status: true }, // Pass ID and status
            success: function (response) {
                if (response.success) {
                    $("#bookModal").modal('hide');
                    loadBorrowBookData();
                    ProcessEmailSending();
                    showCustomAlert(response.message, "success");
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


function ProcessEmailSending() {
    if (!_selectedID) {
        alert("User ID or ISBN is missing.");
        return;
    }

    $.ajax({
        url: "/Book/ApproveBorrow",
        type: "POST",
        data: { id: _selectedID },
        success: function (response) {
            loadBorrowBookData();
            showCustomAlert(response.message, "success");
        },
        error: function (xhr, status, error) {
            alert("Error: " + xhr.responseText);
        }
    });
}

$(document).ready(function () {
    $("#btnDeny").on("click", function () {

        if (!_selectedID) {
            alert("Please reselect a row");
            return;
        }

        $.ajax({
            type: "POST",
            url: `/Book/UpdateBorrowBooks`,
            data: { id: _selectedID, status: false }, // Pass ID and status
            success: function (response) {
                if (response.success) {
                    $("#bookModal").modal('hide');
                    loadBorrowBookData();
                    ProcessEmailSending();
                    showCustomAlert(response.message, "success");
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

// Search function for tbody rows
$(document).ready(function () {
    $('#searchBar').on('keyup', function () {
        let value = $(this).val().toLowerCase();

        $('#userTable tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});
function LoadReturn(id, status) {
    $.ajax({
        url: '/Borrowed/GetBorrowReturnDetails', 
        type: 'GET',
        data: { id: id },
        success: function (data) {
            // Fill modal content
            $('#modalBookId').val(data.id);
            $('#modalUserId').val(data.userID);
            $('#ISBN').val(data.isbn);
            $('#modalName').text(data.name);
            $('#modalISBN').text(data.isbn);
            $('#modalBorrowDate').text(formatDate(data.borrowDate));
            $('#modalReturnDate').text(formatDate(data.returnedDate));
            $('#modalOverdue').text(data.overDue);

            //if (data.overDue === 0) {
            //    $('#btnReturnWithFines').prop('disabled', true);
            //    $('#btnReturn').prop('disabled', true);
            //} else {
            //    $('#btnReturnWithFines').prop('disabled', false);
            //    $('#btnReturn').prop('disabled', false);
            //}
            if (status === "Returned" || status === "Returned w/ Fines") {
                $('#btnReturn').prop('disabled', true);
                $('#btnReturnWithFines').prop('disabled', true);
            } else {
                $('#btnReturn').prop('disabled', false);
                $('#btnReturnWithFines').prop('disabled', false);
            }
            // Show modal

            $('#returnModal').modal('show');
        },
        error: function () {
            alert('Error retrieving borrow data.');
        }
    });
}

// Helper to format date as MM/dd/yyyy
function formatDate(dateString) {
    const date = new Date(dateString);
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${month}/${day}/${date.getFullYear()}`;
}

$('#btnReturn').on('click', function () {
    var bookId = $('#modalBookId').val();
    $.ajax({
        url: '/Borrowed/ReturnBook',
        type: 'POST',
        data: { id: bookId, withFines: false },
        success: function (res) {
            if (res.success) {
                $('#returnModal').modal('hide');
                showCustomAlert('Book returned successfully.','success');
                loadBorrowBookData();
            } else {
                showCustomAlert('Failed: ' + res.message, 'danger');
            }
        },
        error: function () {
            showCustomAlert('Error calling return endpoint.','danger');
        }
    });
});

$('#btnReturnWithFines').click(function () {
    if ($(this).text() === 'Confirm Return with Fines') {
        // Gather data
        const userId = $('#modalUserId').val();
        const isbn = $('#ISBN').val();
        var bookId = $('#modalBookId').val();
        const checkFines = $('#SelectedFines').val();

        // Validate if there are any fines selected
        if (checkFines.length === 0) {
            showCustomAlert('Please add at least one fine.', 'danger');
            return;
        }

        const selectedFines = JSON.parse($('#SelectedFines').val()); 

        //if (selectedFines.length === 0) {
        //    showCustomAlert('Please add at least one fine.', 'danger');
        //    return;
        //}

        // Create an object to send to the server
        const returnData = {
            userID: userId,
            isbn: isbn,
            id: bookId,
            fines: selectedFines
        };

        // Send the data to the server via AJAX
        $.ajax({
            url: '/Book/SaveFines', // Correct URL
            type: 'POST',
            contentType: 'application/json',  // Set the content type to JSON
            data: JSON.stringify(returnData),  // Serialize the data into JSON
            success: function (response) {
                if (response.success) {
                    showCustomAlert('Return with fines saved successfully!', 'success');
                    $('#myModal').modal('hide'); // Hide the modal after successful save
                    // Optionally, reset the form and fine section
                        loadBorrowBookData();
                    clearFines();
                } else {
                    showCustomAlert('Failed to save return with fines. Please try again.', 'danger');
                }
            },
            error: function (xhr, status, error) {
                showCustomAlert('An error occurred while saving. Please try again later.', 'danger');
            }
        });
    } else {
        $('#fineSection').removeClass('d-none');
        $(this).text('Confirm Return with Fines');
        return;
    }
});


let selectedFines = [];

$('#btnAddFine').click(function () {
    const selectedOption = $('#cmbFine option:selected');
    const fineId = selectedOption.val();
    const fineName = selectedOption.text();
    const amount = parseFloat(selectedOption.data('amount'));

    if (!fineId) {
        showCustomAlert('Please select a fine.', 'danger');
        return;
    }

    // Check if already added
    if (selectedFines.some(f => f.id === fineId)) {
        showCustomAlert('Fine already added.', 'danger');
        return;
    }

    // Push the selected fine into the selectedFines array
    selectedFines.push({ id: fineId, fineType: fineName, amount: amount });

    // Update table with the new fine
    $('#tblFines tbody').append(`
        <tr data-id="${fineId}">
            <td>${fineName}</td>
            <td>${amount.toFixed(2)}</td>
            <td><button type="button" class="btn btn-danger btn-sm btnRemoveFine">Remove</button></td>
        </tr>
    `);

    // Clear the dropdown selection
    $('#cmbFine').val('');

    // Update the hidden field with the selected fines
    updateHiddenField();
});

// Remove Fine
$(document).on('click', '.btnRemoveFine', function () {
    const row = $(this).closest('tr');
    const fineId = row.data('id');

    // Remove the fine from the selectedFines array
    const index = selectedFines.findIndex(f => f.id === String(fineId));
    if (index > -1) {
        selectedFines.splice(index, 1);
    }

    // Remove the row from the table
    row.remove();

    // Update the hidden field with the updated selected fines
    updateHiddenField();
});

// Function to update hidden field with selected fines
function updateHiddenField() {
    $('#SelectedFines').val(JSON.stringify(selectedFines));
}
function clearFines() {

    selectedFines = [];

    $('#fineSection').addClass('d-none');
    $('#btnReturnWithFines').text('Return with Fines');
    $('#tblFines tbody').empty();

    $('#tblFines tbody').empty();
    $('#cmbFine').val('');
    $('#SelectedFines').val('');
}

