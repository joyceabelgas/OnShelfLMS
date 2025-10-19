function ClearData() {
   document.getElementById('userId').value = null;
    document.getElementById('memberType').value = null;
    document.getElementById('firstName').value = null;
    document.getElementById('middleName').value = null;
    document.getElementById('lastName').value = null;
    document.getElementById('suffix').value = null;
    document.getElementById('address').value = null;
    document.getElementById('emailAddress').value = null;
    document.getElementById('mobileNumber').value = null;
    document.getElementById('status').value = null;

    document.getElementById("btnSaveUser").textContent = "Save";
}

function ClearValues() {
    document.getElementById('btnNew').textContent = 'NEW';
    document.getElementById('btnEdit').textContent = 'EDIT';
}

const newButton = document.getElementById('btnNew');
const saveButton = document.getElementById('btnSaveUser'); 
const editButton = document.getElementById('btnEdit');

const userID = document.getElementById('userId');
const memberType = document.getElementById('memberType');
const firstName = document.getElementById('firstName');
const middleName = document.getElementById('middleName');
const lastName = document.getElementById('lastName');
const address = document.getElementById('address');
const mobileNumber = document.getElementById('mobileNumber');
const stats = document.getElementById('status');

document.addEventListener("DOMContentLoaded", function () {
    loadUsers();
});

//-----------NEW/SAVE---------------
newButton.addEventListener('click', async function () {
    ClearData();
    new bootstrap.Modal(document.getElementById('userModal')).show();
});


saveButton.addEventListener('click', async function () {

    if (saveButton.textContent === "Save") {
        const formData = new FormData();

        if (userID.value == '') {
            showCustomAlert('Please input User ID', "danger");
            return;
        }
        if (memberType.value == '') {
            showCustomAlert('Please select Member Type', "danger");
            return;
        }
        if (firstName.value == '') {
            showCustomAlert('Please input First Name', "danger");
            return;
        }
        if (lastName.value == '') {
            showCustomAlert('Please input Last Name', "danger");
            return;
        }
        if (middleName.value == '') {
            showCustomAlert('Please input Middle Name', "danger");
            return;
        }
        if (address.value == '') {
            showCustomAlert('Please input Address', "danger");
            return;
        }
        if (mobileNumber.value == '') {
            showCustomAlert('Please input Mobile Number', "danger");
            return;
        }
        if (stats.value == '') {
            showCustomAlert('Please input Status', "danger");
            return;
        }

        // Gather form inputs
        formData.append('UserId', document.getElementById('userId').value);
        formData.append('MemberType', document.getElementById('memberType').value);
        formData.append('FirstName', document.getElementById('firstName').value);
        formData.append('MiddleName', document.getElementById('middleName').value);
        formData.append('LastName', document.getElementById('lastName').value);
        formData.append('Suffix', document.getElementById('suffix').value);
        formData.append('Address', document.getElementById('address').value);
        formData.append('EmailAddress', document.getElementById('emailAddress').value);
        formData.append('MobileNumber', document.getElementById('mobileNumber').value);
        formData.append('Status', document.getElementById('status').value);

        try {
            const response = await fetch('/Account/SaveUserInformation', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();
            if (result.success) {
                ClearData();
                const modalEl = document.getElementById('userModal');
                const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modalInstance.hide();
                loadUsers();
                showCustomAlert('User saved successfully!', "success");
            } else {
                alert('Failed to save user: ' + result.message);
            }
        } catch (error) {
            alert('An error occurred while saving the user.');
        }
    }
    if (saveButton.textContent == 'Update') {
        const formData = new FormData();

        // Gather form inputs
        formData.append('UserId', document.getElementById('userId').value);
        formData.append('MemberType', document.getElementById('memberType').value);
        formData.append('FirstName', document.getElementById('firstName').value);
        formData.append('MiddleName', document.getElementById('middleName').value);
        formData.append('LastName', document.getElementById('lastName').value);
        formData.append('Suffix', document.getElementById('suffix').value);
        formData.append('Address', document.getElementById('address').value);
        formData.append('EmailAddress', document.getElementById('emailAddress').value);
        formData.append('MobileNumber', document.getElementById('mobileNumber').value);
        formData.append('Status', document.getElementById('status').value);


        try {
            const response = await fetch('/Account/UpdateUserInformation', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();
            if (result.success) {
                ClearData();
                const modalEl = document.getElementById('userModal');
                const modalInstance = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modalInstance.hide();
                loadUsers();
                showCustomAlert('User updated successfully!', "success");
            } else {
                alert('Failed to update user: ' + result.message);
            }
        } catch (error) {
            alert('An error occurred while saving the user.');
        }
    }

});

const searchInput = document.getElementById('searchBar');

if (searchInput) {
    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase();

        // Get fresh table rows every time input changes
        const tableRows = document.querySelectorAll('#userTable tbody tr');

        tableRows.forEach(row => {
            const rowText = row.textContent.toLowerCase();
            row.style.display = rowText.includes(searchValue) ? '' : 'none';
        });
    });
}


async function loadUsers() {
    try {
        const response = await fetch('/Account/GetAllTeachers');
        const data = await response.json();

        const tableBody = document.querySelector('#userTable tbody');
        tableBody.innerHTML = ''; // Clear current rows

        if (data.length > 0) {
            data.forEach(user => {
                // Mask email
                let maskedEmail = user.email || '';
                if (maskedEmail.includes('@')) {
                    const atIndex = maskedEmail.indexOf('@');
                    if (atIndex > 1) {
                        const visible = maskedEmail.substring(0, 1);
                        const domain = maskedEmail.substring(atIndex);
                        maskedEmail = visible + '*'.repeat(atIndex - 1) + domain;
                    }
                }

                // Mask mobile number
                let maskedMobile = user.mobileNo || '';
                if (maskedMobile.length >= 7) {
                    const prefix = maskedMobile.substring(0, 4);
                    const suffix = maskedMobile.substring(maskedMobile.length - 3);
                    maskedMobile = prefix + "****" + suffix;
                }

                // Profile picture fallback
                const profilePicture = user.profilePicture
                    ? `data:image/png;base64,${user.profilePicture}`
                    : '/images/default-user.png';

                // Build the row
                const row = `
                    <tr
                        data-userid="${user.userID}"
                        data-membertype="${user.memberType}"
                        data-firstname="${user.firstName}"
                        data-middlename="${user.middleName || ''}"
                        data-lastname="${user.lastName}"
                        data-suffix="${user.suffix || ''}"
                        data-address="${user.address}"
                        data-email="${user.email || ''}"
                        data-mobile="${user.mobileNo || ''}"
                        data-status="${user.status}"
                        data-profilepicture="${profilePicture}">
                        
                        <td>${user.userID}</td>
                        <td>${user.memberType}</td>
                        <td>${[user.firstName, user.middleName, user.lastName, user.suffix].filter(x => x).join(" ")}</td>
                        <td>${user.address}</td>
                        <td>${maskedEmail}</td>
                        <td>${maskedMobile}</td>
                        <td>
                            <!-- Status badge -->
                            ${user.status
                                                ? '<span class="badge bg-success">Active</span>'
                                                : '<span class="badge bg-danger">Inactive</span>'}

                            <!-- Small dropdown with no value selected by default -->
                            <select class="form-select status-dropdown" data-userid="@user.UserID">
                                <option value="" selected disabled hidden> </option> 
                                <option value="true" @(user.Status ? "selected" : "")>Active</option>
                                <option value="false" @(!user.Status ? "selected" : "")>Inactive</option>
                            </select>
                        </td>


                        <td>
                            <button type="button" class="btn btn-primary btn-sm edit-user" id="btnEdit" data-isbn="${user.userID}">
                                <img src="/images/edit.png" alt="Edit" style="width: 16px; height: 16px;" />
                            </button>
                            <button type="button" class="btn btn-danger btn-sm delete-user" data-isbn="${user.userID}">
                                <img src="/images/delete.png" alt="Delete" style="width: 16px; height: 16px;" />
                            </button>
                        </td>
                    </tr>`;

                tableBody.innerHTML += row;
            });

            //attachEditEvent(); // Reattach event after dynamic load
        } else {
            tableBody.innerHTML = `<tr><td colspan="10" class="text-center">No records found.</td></tr>`;
        }
    } catch (error) {
        console.error('Error loading users:', error);
    }
}


document.querySelector("#userTable tbody").addEventListener("click", function (e) {
    if (e.target.closest(".edit-user")) {
        const row = e.target.closest("tr");

        // Retrieve data from row attributes or from cells
        const userId = row.getAttribute("data-userid");
        const memberType = row.getAttribute("data-membertype");
        const firstName = row.getAttribute("data-firstname");
        const middleName = row.getAttribute("data-middlename");
        const lastName = row.getAttribute("data-lastname");
        const suffix = row.getAttribute("data-suffix");
        const address = row.getAttribute("data-address");
        const email = row.getAttribute("data-email");
        const mobile = row.getAttribute("data-mobile");
        const status = row.getAttribute("data-status");

        // Populate the modal fields
        document.getElementById("userId").value = userId || "";
        document.getElementById("memberType").value = memberType || "";
        document.getElementById("firstName").value = firstName || "";
        document.getElementById("middleName").value = middleName || "";
        document.getElementById("lastName").value = lastName || "";
        document.getElementById("suffix").value = suffix || "";
        document.getElementById("address").value = address || "";
        document.getElementById("emailAddress").value = email || "";
        document.getElementById("mobileNumber").value = mobile || "";
        document.getElementById("status").value = status === "true" ? "Active" : "Inactive";

        // Show the modal
        const userModal = new bootstrap.Modal(document.getElementById("userModal"));
        document.getElementById("btnSaveUser").textContent = "Update";
        userModal.show();
    }
});

document.querySelector("#userTable tbody").addEventListener("click", async function (e) {
    if (e.target.closest(".delete-user")) {
        const row = e.target.closest("tr");
        const userId = row.getAttribute("data-userid");

        showConfirmModal("Are you sure you want to delete this user?", async function () {
            try {
                const response = await fetch(`/Account/DeleteUser/${userId}`, {
                    method: "DELETE"
                });

                if (response.ok) {
                    showCustomAlert('User deleted successfully', "success");
                    loadUsers(); // Refresh the table
                } else {
                    const error = await response.text();
                    showCustomAlert('Failed to delete user. ' + error, "danger");
                }
            } catch (error) {
                showCustomAlert('Error deleting user: ' + error.message, "danger");
            }
        });
    }
});


document.querySelector("#userTable tbody").addEventListener("change", async function (e) {
    if (e.target.classList.contains("status-dropdown")) {
        const row = e.target.closest("tr");
        const userId = row.getAttribute("data-userid");
        const newStatus = e.target.value;

        try {
            const response = await fetch("/Account/UpdateUserStatus", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ userId: userId, status: newStatus === "true" })
            });

            if (response.ok) {
                showCustomAlert('Status updated successfully!', "danger");
                loadUsers(); // Optional: refresh user table
            } else {
                const error = await response.text();
                showCustomAlert('Failed to update status. ', "danger");
            }
        } catch (error) {
            showCustomAlert('Error updating status: ' + error.message, "danger");
        }
    }
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