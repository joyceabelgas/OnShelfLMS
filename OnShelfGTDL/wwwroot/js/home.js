$(document).ready(function () {
    var booksDataUrl = '/Home/LoadBooksData';  // Build the URL manually

    $.ajax({
        url: booksDataUrl,  // Use the URL here
        method: 'GET',
        success: function (data) {
            if (data.success !== false) {
                // Loop through and display Most Borrowed Books in One Row
                data.mostBorrowed.forEach(function (book) {
                    $('#mostBorrowedBooks').append(`
                        <div class="col-md-3">
                            <div class="card shadow-sm mx-auto p-2 text-center book-card fs-7" style="flex: 0 0 auto; width: 18rem;">
                                <img src="${book.bookImage}" class="card-img book-img" alt="${book.bookName}" />
                                <div class="card-body fs-7">
                                    <h5 class="card-title">${book.bookName}</h5>
                                </div>
                            </div>
                        </div>
                    `);
                });

                // Loop through and display Today's Pick Books in One Row
                data.todaysPick.forEach(function (book) {
                    $('#todaysPickBooks').append(`
                        <div class="col-md-3">
                            <div class="card shadow-sm mx-auto p-2 text-center book-card fs-7" style="flex: 0 0 auto; width: 18rem;">
                                <img src="${book.bookImage}" class="card-img-top" alt="${book.bookName}" />
                                <div class="card-body">
                                    <h5 class="card-title">${book.bookName}</h5>
                                    <p class="card-text">ISBN: ${book.isbn}</p>
                                </div>
                            </div>
                        </div>
                    `);
                });
            } else {
                alert('Error: ' + data.message); // Display message from server
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error - Status: " + textStatus + ", Error: " + errorThrown);
            alert('Error loading books. Status: ' + textStatus + ', Error: ' + errorThrown);
        }
    });
});
