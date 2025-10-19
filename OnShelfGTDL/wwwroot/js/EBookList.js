$(document).ready(function () {

    // When book card is clicked
    $('.book-card').on('click', function () {
        const bookTitle = $(this).data('title');
        const bookAuthor = $(this).data('copyright');
        const bookDesc = $(this).data('category');
        const bookImage = $(this).data('image');
        const bookId = $(this).data('isbn');

        // Populate modal fields
        $('#modalBookTitle').text(bookTitle || 'Untitled');
        $('#modalBookAuthor').text(`By ${bookAuthor || 'Unknown Author'}`);
        $('#modalBookDescription').text(bookDesc || 'No description available.');
        $('#modalBookImage').attr('src', bookImage || '/images/no-cover.png');

        // Attach ebook id to read/download buttons
        $('#readOnline').data('id', bookId);
        $('#downloadBook').data('id', bookId);

        // Show modal (optional if not using data-bs-toggle)
        $('#bookModal').modal('show');
    });

    $('#readOnline').on('click', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) {
            window.open(`/Book/Read/${id}`, '_blank'); // open PDF reader in new tab
        }
    });


});
