$(document).ready(function () {
    $('input[name="userType"]').change(function () {
        if ($('#teacher').is(':checked')) {
            $('#lrnLabel').text("Employee Number");
            $('#lrnInput').attr("placeholder", "Employee Number");
            $('#GradeLabel').text("Advisory Class");
            $('#GradeInput').attr("placeholder", "Advisory Class");
        } else {
            $('#lrnLabel').text("Learner’s Registered Number");
            $('#lrnInput').attr("placeholder", "LRN");
            $('#GradeLabel').text("Grade");
            $('#GradeInput').attr("placeholder", "Grade");
        }
    });
});

document.getElementById("password").addEventListener("input", function () {
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

const passInput = document.getElementById("password");
const confirmPassInput = document.getElementById("confirmPassword");
const passIcon = document.getElementById("passIcon");
const confirmPassIcon = document.getElementById("confirmPassIcon");

function updateMatchStatus() {
    const newVal = passInput.value;
    const confirmVal = confirmPassInput.value;

    if (!newVal && !confirmVal) {
        passIcon.innerHTML = "";
        confirmPassIcon.innerHTML = "";
        return;
    }

    if (newVal === confirmVal) {
        passIcon.innerHTML = "✅";
        passIcon.style.color = "green";
        confirmPassIcon.innerHTML = "✅";
        confirmPassIcon.style.color = "green";
    } else {
        passIcon.innerHTML = "❌";
        passIcon.style.color = "red";
        confirmPassIcon.innerHTML = "❌";
        confirmPassIcon.style.color = "red";
    }
}

passInput.addEventListener("input", updateMatchStatus);
confirmPassInput.addEventListener("input", updateMatchStatus);

$(document).ready(function () {
    $('#registrationForm').on('submit', function (e) {
        e.preventDefault(); // Prevent default form submission

        const privacyChecked = $('#privacyPolicy').is(':checked');
        const termsChecked = $('#termsConditions').is(':checked');
        const _lrn = $('#lrnInput').val();
        const _emailAdd = $('#emailAddress').val();

        if (!privacyChecked || !termsChecked) {

            let msg = 'Please accept ';
            if (!privacyChecked && !termsChecked) {
                msg += 'the Privacy Policy and Terms & Condition.';
            } else if (!privacyChecked) {
                msg += 'the Privacy Policy.';
            } else {
                msg += 'the Terms & Condition.';
            }

            alert(msg); 

            return false;
        }

        const data = {
            FirstName: $('#firstName').val(),
            MiddleName: $('#middleName').val(),
            LastName: $('#lastName').val(),
            Suffix: $('#suffix').val(),
            LRN: $('#lrnInput').val(),
            Grade: $('#GradeInput').val(),
            Section: $('#section').val(),
            Adviser: $('#adviserName').val(),
            Address: $('#address').val(),
            Email: $('#emailAddress').val(),
            MobileNumber: $('#mobileNumber').val(),
            Password: $('#password').val(),
            ConfirmPassword: $('#confirmPassword').val(),
            memberType: $('input[name="userType"]:checked').attr('id') // "student" or "teacher"
        };

        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    ProcessOTPSending(_lrn);
                    window.location.href = '/Account/Otp?email=' + encodeURIComponent(_emailAdd);
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
                alert('An error occurred. Please try again.');
            }
        });
    });
});

function ProcessOTPSending(lrn) {
    $.ajax({
        url: "/Account/OTPSend",
        type: "POST",
        data: { userID: lrn },
        success: function (response) {
        },
        error: function (xhr, status, error) {
            alert("Error: " + xhr.responseText);
        }
    });
};
