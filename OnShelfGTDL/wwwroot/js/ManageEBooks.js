function ClearData() {
    // Reset all input fields
    $("#ebookId").val("");
    $("#ebookTitle").val("");
    $("#ebookCategory").val("");
    $("#ebookAuthors").val("");
    $("#ebookDescription").val("");

    // Reset file inputs
    $("#ebookFile").val("");
    $("#ebookCoverImage").val("");

    // Clear stored paths
    $("#ebookFilePath").val("");
    $("#ebookCoverPath").val("");

    // Reset current file display
    $("#currentEbookFile").html('<span class="text-muted">No file uploaded</span>');

    // Reset cover preview image
    $("#ebookCover, #ebookCoverPreview").attr("src", "/images/default-book.png");

    // Reset modal header & button text
    $("#ebookModalLabel").text("Add E-Book");
    $("#btnSaveEbook").text("Save");

    //// Ensure save button is enabled (in case of prior disable)
    //$("#btnSaveEbook").prop('disabled', false);
}


$(document).ready(function () {
    loadEBooks();
    bindSearchFilter();
});

function bindSearchFilter() {
    const searchInput = document.getElementById('searchBar');
    if (!searchInput) return;
    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase();
        $('#bookTableBody tr').each(function () {
            const rowText = $(this).text().toLowerCase();
            $(this).toggle(rowText.includes(searchValue));
        });
    });
}

function loadEBooks() {
    $.ajax({
        url: '/Book/LoadEBooks',
        type: 'GET',
        success: function (data) {
            let tableBody = $('#bookTableBody');
            tableBody.empty();

            if (!data || data.length === 0) {
                tableBody.append('<tr><td colspan="8" class="text-center">No books found.</td></tr>');
                return;
            }

            $.each(data, function (index, book) {
                let id = book.ebookID || book.EbookId || "";
                let title = book.title || "";
                let category = book.category || "";
                let authors = book.authors || "";
                let description = book.description || "";
                let uploadedBy = book.uploadedBy || "";
                let dateUploaded = (book.dateUploaded || "").split('T')[0];
                let filePath = book.ebookFilePath || "";
                //let coverImage = book.CoverImage || "/images/default-book.png";

                let coverImage = book.coverImage
                    ? (book.coverImage.startsWith('data:image')
                        ? book.coverImage
                        : `/uploads/${book.coverImage.replace(/^\/+/, '')}`)
                    : "/images/default-book.png";

                let row = `
                    <tr data-ebookid="${id}"
                        data-title="${title}"
                        data-category="${category}"
                        data-authors="${authors}"
                        data-description="${description}"
                        data-ebookfilepath="${filePath}"
                        data-coverimage="${coverImage}"
                        data-uploadedby="${uploadedBy}"
                        data-dateuploaded="${dateUploaded}">
                        data-coverimage="${coverImage}"
                        <td>${id}</td>
                        <td>${title}</td>
                        <td>${category}</td>
                        <td>${authors}</td>
                        <td>${description}</td>
                        <td>${uploadedBy}</td>
                        <td>${dateUploaded}</td>
                        <td>
                            <button class="btn btn-primary btn-sm edit-book">
                                <img src="/images/edit.png" alt="Edit" width="16" height="16" />
                            </button>
                            <button class="btn btn-danger btn-sm delete-book">
                                <img src="/images/delete.png" alt="Delete" width="16" height="16" />
                            </button>
                        </td>
                    </tr>`;
                tableBody.append(row);
            });
        },
        error: function (err) {
            console.error("Failed to load e-books", err);
        }
    });
}

$('#btnUploadEbook').on('click', function () {
    ClearData();
    const modal = new bootstrap.Modal(document.getElementById('ebookModal'));
    modal.show();
});

$('#ebookCoverImage').on('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (evt) {
            $("#ebookCover").attr("src", evt.target.result);
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
    const ebookId = $('#ebookId').val();
    const pdfFile = $('#ebookFile')[0].files[0] || null;
    const coverFile = $('#ebookCoverImage')[0].files[0] || null;

    const formData = new FormData();
    formData.append('ebookId', ebookId);
    formData.append('title', $('#ebookTitle').val());
    formData.append('category', $('#ebookCategory').val());
    formData.append('authors', $('#ebookAuthors').val());
    formData.append('description', $('#ebookDescription').val());

    if (pdfFile) {
        formData.append('ebookFile', pdfFile);
    } else {
        formData.append('existingFilePath', $('#ebookFilePath').val());
    }

    if (coverFile) {
        formData.append('ebookCoverImage', coverFile);
    } else {
        formData.append('existingCoverPath', $('#ebookCoverPath').val());
    }

    $.ajax({
        url: ebookId ? '/Book/UpdateEbook' : '/Book/UploadEbook',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            $('#btnSaveEbook').prop('disabled', true).text('Saving...');
        },
        success: function (response) {
            showCustomAlert(response, 'success');
            $('#ebookModal').modal('hide');
            loadEBooks();
        },
        error: function (xhr) {
            showCustomAlert('❌ ' + xhr.responseText, 'danger');
        },
        complete: function () {
            $('#btnSaveEbook').prop('disabled', false).text('Save');
        }
    });
});

$(document).on('click', '.edit-book', function () {
    const row = $(this).closest('tr');

    // Retrieve all data attributes safely
    const ebookId = row.data('ebookid') || '';
    const title = row.data('title') || '';
    const category = row.data('category') || '';
    const authors = row.data('authors') || '';
    const description = row.data('description') || '';
    const ebookFilePath = row.data('ebookfilepath') || '';
    const coverImagePath = row.data('coverimage') || '/images/default-book.png';
    const uploadedBy = row.data('uploadedby') || '';

    // Populate modal fields
    $('#ebookModalLabel').text('Edit E-Book');
    $('#ebookId').val(ebookId);
    $('#ebookTitle').val(title);
    $('#ebookCategory').val(category);
    $('#ebookAuthors').val(authors);
    $('#ebookDescription').val(description);
    $('#ebookFilePath').val(ebookFilePath);
    $('#ebookCoverPath').val(coverImagePath);

    // Show the current ebook file link
    if (ebookFilePath) {
        const fileName = ebookFilePath.split('/').pop();
        $('#currentEbookFile').html(`<a href="${ebookFilePath}" target="_blank">${fileName}</a>`);
    } else {
        $('#currentEbookFile').empty();
    }

    $('#ebookCover').attr('src', coverImagePath || '/images/default-book.png');

    // Open the modal
    $('#ebookModal').modal('show');
});


$(document).on('click', '.delete-book', function () {
    const id = $(this).closest('tr').data('ebookid');
    showConfirmModal("Are you sure you want to delete this e-book?", function () {
        $.post('/Book/DeleteEBook', { id: id }, function (response) {
            showCustomAlert(response.message, response.success ? 'success' : 'danger');
            loadEBooks();
        }).fail(() => showCustomAlert('Error deleting the book.', 'danger'));
    });
});

$('#ebookCoverImage').on('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            $('#ebookCover').attr('src', e.target.result);
        };
        reader.readAsDataURL(file);
    }
});

function showConfirmModal(message, yesCallback) {
    document.getElementById('confirmModalMessage').textContent = message;
    confirmCallback = yesCallback;
    var myModal = new bootstrap.Modal(document.getElementById('confirmModal'));
    myModal.show();
}