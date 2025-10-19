
document.addEventListener('DOMContentLoaded', function () {

    loadBorrowBookData();

    const filterDropdown = document.getElementById('cmbFilter');

    filterDropdown.addEventListener('change', () => {
        // Get the selected filter value
        const filterValue = filterDropdown.value.trim().toLowerCase();

        // Get all rows in the table
        const rows = document.querySelectorAll('#userTable tbody tr');

        rows.forEach(row => {
            // Get the status value from the 4th column (index 3)
            const status = row.cells[4].innerText.trim().toLowerCase();  // Assuming status is in the 4th column

            // Show or hide the row based on the filter value and status
            if (filterValue === 'filter' || filterValue === '') {
                row.style.display = ''; // Show all rows if no filter is selected
            } else if (status === filterValue) {
                row.style.display = ''; // Show row if status matches filter
            } else {
                row.style.display = 'none'; // Hide row if status doesn't match filter
            }
        });
    });

    // Delegate click event for 'Pay' button dynamically
    document.querySelector('#userTable').addEventListener('click', function (e) {
        if (e.target && e.target.classList.contains('btn-pay')) {
            const fineId = e.target.getAttribute('data-id');
            const userId = 'YourUserId';  // Replace with the actual UserID or get it dynamically
            updateFineStatus(fineId, userId);
        }
    });

});
$(document).ready(function () {
    $('#cmbFilter').on('change', function () {
        var selected = $(this).val();
        var placeholderText = 'Search';

        if (selected === 'Paid') {
            placeholderText = 'Search Paid';
        } else if (selected === 'Unpaid') {
            placeholderText = 'Search Unpaid';
        }
        $('#searchBar').attr('placeholder', placeholderText);
    });
});
$(document).ready(function () {
    $('#searchBar').on('keyup', function () {
        let value = $(this).val().toLowerCase();

        $('#userTable tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});
function loadBorrowBookData() {
    fetch('/FineManagement/GetBorrowBook')
        .then(response => response.json())
        .then(data => {

            const tableBody = document.querySelector('#userTable tbody');
            tableBody.innerHTML = '';

            data.forEach(record => {
                const row = `
                    <tr>
                        <td style="display:none;">${record.id}</td>
                        <td>${record.name || 'N/A'}</td>
                        <td>${record.isbn || 'N/A'}</td>
                        <td>${record.totalFines !== null ? record.totalFines.toFixed(2) : '0.00'}</td>
                        <td>${record.status || 'N/A'}</td>
                        <td>
                            <button class="btn btn-sm btn-primary btn-pay" data-id="${record.id}">Pay</button>
                        </td>
                    </tr>`;
                tableBody.innerHTML += row;
            });

        })
        .catch(error => console.error('Error loading data:', error));
}

// Function to update the fine status when 'Pay' is clicked
function updateFineStatus(fineId, userId) {
    const requestData = {
        id: fineId,
        userId: userId
    };

    fetch('/FineManagement/UpdateFineStatus', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showCustomAlert('Fine paid successfully!', 'success');
                loadBorrowBookData();  // Reload the data to reflect the updated status
            } else {
                showCustomAlert('Error updating fine status. Please try again.', 'danger');
            }
        })
        .catch(error => {
            showCustomAlert('An error occurred while updating the fine. Please try again later.', 'danger');
        });
}


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