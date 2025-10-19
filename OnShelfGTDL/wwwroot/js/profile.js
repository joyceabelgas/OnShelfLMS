function previewSelectedImage(event) {
    const reader = new FileReader();
    reader.onload = function () {
        const output = document.getElementById('previewImage');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);
}

$(document).ready(function () {
    $.ajax({
        url: '/Account/GetProfileData',
        type: 'GET',
        success: function (data) {
            console.log(data); // Debug

            // Populate inputs
            $('#userId').val(data.userID);
            $('#firstName').val(data.firstName);
            $('#middleName').val(data.middleName);
            $('#lastName').val(data.lastName);
            $('#suffix').val(data.suffix);
            $('#address').val(data.address);
            $('#emailAddress').val(data.email);
            $('#mobileNumber').val(data.mobileNo);

            // Show image if profile picture exists
            if (data.profilePicture) {
                const imgSrc = 'data:image/png;base64,' + data.profilePicture;
                $('#previewImage').attr('src', imgSrc);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error loading profile:", error);

            showCustomAlert('Failed to load profile data.', "danger");
        }
    });
});


$('#profileForm').on('submit', function (e) {
    e.preventDefault();

    var formData = new FormData(this);

    $.ajax({
        url: '/Account/Profile',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            showCustomAlert('Successfully updated!', "danger");
        },
        error: function (xhr) {
            alert("" + xhr.responseText);
            showCustomAlert('Error: ' + xhr.responseText, "danger");
        }
    });
});



document.getElementById("NewPassword").addEventListener("input", function () {
    const password = this.value;

    // Requirements
    const isLongEnough = password.length >= 8;
const hasUpperCase = /[A-Z]/.test(password);
const hasNumber = /[0-9]/.test(password);
const hasSpecialChar = /[^A-Za-z0-9]/.test(password); // non-alphanumeric

// Update UI
document.getElementById("charLength").className = isLongEnough ? "text-success" : "text-danger";
document.getElementById("upperCase").className = hasUpperCase ? "text-success" : "text-danger";
document.getElementById("numberCheck").className = hasNumber ? "text-success" : "text-danger";
document.getElementById("specialChar").className = hasSpecialChar ? "text-success" : "text-danger";
});

const newPassInput = document.getElementById("NewPassword");
const confirmPassInput = document.getElementById("ConfirmPassword");
const newPassIcon = document.getElementById("newPassIcon");
const confirmPassIcon = document.getElementById("confirmPassIcon");

function updateMatchStatus() {
    const newVal = newPassInput.value;
    const confirmVal = confirmPassInput.value;

    if (!newVal && !confirmVal) {
        newPassIcon.innerHTML = "";
        confirmPassIcon.innerHTML = "";
        return;
    }

    if (newVal === confirmVal) {
        newPassIcon.innerHTML = "✅";
        newPassIcon.style.color = "green";
        confirmPassIcon.innerHTML = "✅";
        confirmPassIcon.style.color = "green";
    } else {
        newPassIcon.innerHTML = "❌";
        newPassIcon.style.color = "red";
        confirmPassIcon.innerHTML = "❌";
        confirmPassIcon.style.color = "red";
    }
}

newPassInput.addEventListener("input", updateMatchStatus);
confirmPassInput.addEventListener("input", updateMatchStatus);

$('#changePasswordForm').submit(function (e) {
    e.preventDefault();

    const data = {
        CurrentPassword: $('#CurrentPassword').val(),
        NewPassword: $('#NewPassword').val(),
        ConfirmPassword: $('#ConfirmPassword').val()
    };

    $.ajax({
        url: '/Account/ChangePassword', // Update based on your controller
        type: 'POST',
        data: data,
        success: function (res) {
            if (res.success) {
                showCustomAlert(res.message, 'success');
                $('#changePasswordForm')[0].reset();
            } else {
                showCustomAlert(res.message,'danger');
            }
        },
        error: function () {
            alert("Something went wrong. Please try again.");
        }
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
