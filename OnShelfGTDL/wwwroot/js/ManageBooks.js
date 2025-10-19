
function ClearData() {
    document.getElementById('isnb').value = null;
    document.getElementById('bookShelf').value = null;
    document.getElementById('bookName').value = null;
    document.getElementById('copyright').value = null;
    document.getElementById('stockQuantity').value = null;
    document.getElementById('category').value = null;
    document.getElementById('authorsName').value = null;
    document.getElementById('publicationName').value = null;
    document.getElementById('description').value = null;

    document.getElementById("btnSaveUser").textContent = "Save";
}

function ClearValues() {
    document.getElementById('btnNew').textContent = 'NEW';
    //document.getElementById('btnEdit').textContent = 'EDIT';
}

const newButton = document.getElementById('btnNew');
const saveButton = document.getElementById('btnSaveUser'); 
//const editButton = document.getElementById('btnEdit');
const userForm = document.getElementById('userForm');


const isbn = document.getElementById('isnb');
const bookShelf = document.getElementById('bookShelf');
const bookName = document.getElementById('bookName');
const copyright = document.getElementById('copyright');
const stockQuantity = document.getElementById('stockQuantity');
const category = document.getElementById('category');
const authorsName = document.getElementById('authorsName');
const publicationName = document.getElementById('publicationName');
const description = document.getElementById('description');
const stats = document.getElementById('status');

const searchInput = document.getElementById('searchBar');
const tableRows = document.querySelectorAll('tbody tr');

if (searchInput) { // Check if element exists
    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase();

        tableRows.forEach(row => {
            const rowText = row.textContent.toLowerCase();
            row.style.display = rowText.includes(searchValue) ? '' : 'none';
        });
    });
} else {
    //console.error('Search input not found!');
}

//-----------NEW/SAVE--------------- 
newButton.addEventListener('click', async function () {
    ClearData();
    new bootstrap.Modal(document.getElementById('userModal')).show();
});

document.getElementById('btnUploadEbook').addEventListener('click', function () {
    const modal = new bootstrap.Modal(document.getElementById('ebookModal'));
    modal.show();
});

document.getElementById('ebookCoverImage').addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (evt) {
            document.getElementById('ebookCoverPreview').src = evt.target.result;
        };
        reader.readAsDataURL(file);
    }
});

saveButton.addEventListener('click', async function () {

    if (isbn.value == '') {
        showCustomAlert('Please input ISBN', "danger");
        return;
    }
    if (bookShelf.value == '') {
        showCustomAlert('Please select Book     Shelf', "danger");
        return;
    }
    if (bookName.value == '') {
        showCustomAlert('Please input Book Name', "danger");
        return;
    }
    if (copyright.value == '') {
        showCustomAlert('Please input Copyright', "danger");
        return;
    }
    if (category.value == '') {
        showCustomAlert('Please input Category', "danger");
        return;
    }
    if (authorsName.value == '') {
        showCustomAlert('Please input Authors Name', "danger");
        return;
    }
    if (publicationName.value == '') {
        showCustomAlert('Please input Publication Name', "danger");
        return;
    }
    if (description.value == '') {
        showCustomAlert('Please input description', "danger");
        return;
    }

    if (saveButton.textContent === "Save") {
        const formData = new FormData();

        // Gather form inputs
        formData.append('ISNB', document.getElementById('isnb').value);
        formData.append('bookShelf', document.getElementById('bookShelf').value);
        formData.append('bookName', document.getElementById('bookName').value);
        formData.append('copyright', document.getElementById('copyright').value);
        formData.append('stockQuantity', document.getElementById('stockQuantity').value);
        formData.append('category', document.getElementById('category').value);
        formData.append('authorsName', document.getElementById('authorsName').value);
        formData.append('publicationName', document.getElementById('publicationName').value);
        formData.append('description', document.getElementById('description').value);

            //Handle image upload
        const bookImage = document.getElementById('bookImage').files[0];
        if (bookImage) {
            formData.append('bookImage', bookImage);
        }

        try {
            const response = await fetch('/Book/SaveBookInformation', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();
            if (result.success) {
                ClearData();
                showCustomAlert('Books information saved successfully!','success')
                const modalEl = document.getElementById('userModal');
                const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modalInstance.hide();
                loadBooks();
            } else {
                showCustomAlert('Failed to save book information: ' + result.message, 'dander')
            }
        } catch (error) {
            showCustomAlert('An error occurred while saving book.', 'dander')
        }
    }
    if (saveButton.textContent === "Update") {
        const formData = new FormData();

        // Gather form inputs
        formData.append('ISNB', document.getElementById('isnb').value);
        formData.append('bookShelf', document.getElementById('bookShelf').value);
        formData.append('bookName', document.getElementById('bookName').value);
        formData.append('copyright', document.getElementById('copyright').value);
        formData.append('stockQuantity', document.getElementById('stockQuantity').value);
        formData.append('category', document.getElementById('category').value);
        formData.append('authorsName', document.getElementById('authorsName').value);
        formData.append('publicationName', document.getElementById('publicationName').value);
        formData.append('description', document.getElementById('description').value);

        //Handle image upload
        const bookImage = document.getElementById('bookImage').files[0];
        if (bookImage) {
            formData.append('bookImage', bookImage);
        }

        try {
            const response = await fetch('/Book/UpdateBookInformation', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();
            if (result.success) {
                ClearData();
                showCustomAlert('Books information update successfully!', 'success')
                const modalEl = document.getElementById('userModal');
                const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modalInstance.hide();
                loadBooks();
            } else {
                showCustomAlert('Failed to update book information: ' + result.message, 'dander')
            }
        } catch (error) {
            showCustomAlert('An error occurred while updating book.', 'dander')
        }
    }
});

document.querySelectorAll("#userTable tbody tr").forEach(row => {
    row.addEventListener("dblclick", () => {

        const _isbn = document.getElementById('isnb');
        // Fetch row data
        const isbn = row.getAttribute("data-isbn");
        const bookname = row.getAttribute("data-bookname");
        const category = row.getAttribute("data-category");
        const authorsname = row.getAttribute("data-authorsname") || "";
        const bookshelf = row.getAttribute("data-bookshelf");
        const copyright = row.getAttribute("data-copyright") || "";
        const stockqty = row.getAttribute("data-stockqty");
        const publicationname = row.getAttribute("data-publicationname") || "";
        const description = row.getAttribute("data-description") || "";
        const bookimage = row.getAttribute("data-bookimage") || "";

        // Populate form fields
        document.getElementById("isnb").value = isbn || "";
        document.getElementById("bookShelf").value = bookshelf || "";
        document.getElementById("bookName").value = bookname || "";
        document.getElementById("copyright").value = copyright || "";
        document.getElementById("stockQuantity").value = stockqty || "";
        document.getElementById("category").value = category || "";
        document.getElementById("authorsName").value = authorsname || "";
        document.getElementById("publicationName").value = publicationname || "";
        document.getElementById("description").value = description || "";
        //document.getElementById("bookImage").value = bookimage || "";

        const imagePreview = document.getElementById("booksPicture");

        if (bookimage) {
            // Ensure the image is displayed using Base64 data (if available)
            imagePreview.src = `${bookimage}`;
        } else {
            imagePreview.src = "/images/default-book.png";
        }

        _isbn.disabled = true;
        // Show the form if hidden
        userForm.classList.add("show");
        userForm.style.display = "block";
        cancelButton.style.display = "inline-block";
        buttonLine.classList.add("show");
    });
}); 

$(document).ready(function () {
    loadBooks();   
});
function loadBooks() {
    $.ajax({
        url: '/Book/LoadBooks',
        type: 'GET',
        success: function (data) {
            let tableBody = $('#bookTableBody');
            tableBody.empty();

            if (data.length === 0) {
                tableBody.append('<tr><td colspan="12" class="text-center">No books found.</td></tr>');
                return;
            }

            $.each(data, function (index, book) {
                // Prepare the base64 image but do NOT show it in the table
                let bookImage = book.bookImage ? `data:image/jpeg;base64,${book.bookImage}` : '';

                let row = `
                    <tr 
                        data-isbn="${book.isnb}"
                        data-bookname="${book.bookName}"
                        data-category="${book.category}"
                        data-authorsname="${book.authorsName || ''}"
                        data-bookshelf="${book.bookShelf}"
                        data-copyright="${book.copyright || ''}"
                        data-stockqty="${book.stockQuantity}"
                        data-publicationname="${book.publicationName || ''}"
                        data-description="${book.description || ''}"
                        data-bookimage="${bookImage}"
                    >
                        <td>${book.isnb}</td>
                        <td>${book.bookName}</td>
                        <td>${book.category}</td>
                        <td>${book.authorsName}</td>
                        <td>${book.bookShelf}</td>
                        <td>${book.copyright}</td>
                        <td>${book.stockQuantity}</td>
                        <td>${book.publicationName}</td>
                        <td>${book.description}</td>
                        <td>${book.createdBy}</td>
                        <td>${book.createdDate.split('T')[0]}</td>
                        <td>
                            <button class="btn btn-primary btn-sm edit-book">
                                <img src="/images/edit.png" alt="Edit" style="width: 16px; height: 16px;" />
                            </button>
                            <button class="btn btn-danger btn-sm delete-book">
                                <img src="/images/delete.png" alt="Delete" style="width: 16px; height: 16px;" />
                            </button>
                        </td>
                    </tr>`;
                tableBody.append(row);
            });
        },
        error: function (err) {
            console.error("Failed to load books", err);
        }
    });
}

$('#bookTableBody').on('click', '.edit-book', function () {
    const row = $(this).closest('tr');

    const isbn = row.data("isbn");
    const bookname = row.data("bookname");
    const category = row.data("category");
    const authorsname = row.data("authorsname") || "";
    const bookshelf = row.data("bookshelf");
    const copyright = row.data("copyright") || "";
    const stockqty = row.data("stockqty");
    const publicationname = row.data("publicationname") || "";
    const description = row.data("description") || "";
    const bookimage = row.data("bookimage") || "";

    // Populate modal fields
    $("#isnb").val(isbn);
    $("#bookShelf").val(bookshelf);
    $("#bookName").val(bookname);
    $("#copyright").val(copyright);
    $("#stockQuantity").val(stockqty);
    $("#category").val(category);
    $("#authorsName").val(authorsname);
    $("#publicationName").val(publicationname);
    $("#description").val(description);

    const imagePreview = document.getElementById("booksPicture");

    if (bookimage) {
        // Ensure the image is displayed using Base64 data (if available)
        imagePreview.src = `${bookimage}`;
    } else {
        imagePreview.src = "/images/default-book.png";
    }
    // Show modal
    const userModal = new bootstrap.Modal(document.getElementById("userModal"));
    $("#btnSaveUser").text("Update");
    userModal.show();
});

document.getElementById("bookImage").addEventListener("change", function (event) {
    const file = event.target.files[0];
    const preview = document.getElementById("booksPicture");

    if (file) {
        const reader = new FileReader();

        // When file is read successfully
        reader.onload = function (e) {
            preview.src = e.target.result; // Set the preview image source
        };

        reader.readAsDataURL(file); // Read file as a data URL
    } else {
        preview.src = "/images/default-book.png"; // Reset to default if no file selected
    }
});

$(document).on('click', '.delete-book', function () {
    const row = $(this).closest('tr');

    const isbn = row.data("isbn");
    showConfirmModal("Are you sure you want to delete this book?", function () {
        $.ajax({
            url: `/Book/DeleteBook`, 
            type: 'POST',
            data: { isbn: isbn },
            success: function (response) {
                if (response.success) {
                    showCustomAlert(response.message, 'success')
                    loadBooks();
                } else {
                    showCustomAlert(response.message, 'danger');
                }
            },
            error: function () {
                showCustomAlert('Error deleting the book.', 'danger')
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

// Preview cover image
$('#ebookCoverImage').on('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            $('#ebookCoverPreview').attr('src', e.target.result).show();
        };
        reader.readAsDataURL(file);
    }
});

$('#ebookFile').on('change', function () {
    const file = this.files[0];
    if (file && file.size > 200 * 1024 * 1024) {
        alert('The selected PDF exceeds the 200 MB limit.');
        $(this).val('');
    }
});

$('#btnSaveEbook').on('click', function () {
    const pdfFile = $('#ebookFile')[0].files[0];
    const coverFile = $('#ebookCoverImage')[0].files[0];

    const maxSize = 200 * 1024 * 1024;
    if (pdfFile && pdfFile.size > maxSize) {
        alert('The selected PDF file exceeds the 200 MB size limit.');
        return;
    }
    if (coverFile && coverFile.size > maxSize) {
        alert('The cover image exceeds the 200 MB size limit.');
        return;
    }

    const formData = new FormData();
    formData.append("title", $('#ebookTitle').val());
    formData.append("category", $('#ebookCategory').val());
    formData.append("authors", $('#ebookAuthors').val());
    formData.append("description", $('#ebookDescription').val());
    if (pdfFile) formData.append("ebookFile", pdfFile);
    if (coverFile) formData.append("ebookCoverImage", coverFile);

    $.ajax({
        url: '/Book/UploadEbook',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            $('#btnSaveEbook').prop('disabled', true).text('Saving...');
        },
        success: function (response) {
            alert('E-Book uploaded successfully!');
            $('#ebookModal').modal('hide');
            //$('#frmEbookUpload')[0].reset();
            $('#ebookCoverPreview').attr('src', '').hide();
        },
        error: function (xhr) {
            console.error(xhr);
            //alert('❌ Error uploading e-book:\n' + xhr.responseText);
            alert('Error saving pdf');
        },
        complete: function () {
            $('#btnSaveEbook').prop('disabled', false).text('Save');
        }
    });
});
