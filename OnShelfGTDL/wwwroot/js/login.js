$(document).ready(function () {
    $("#loginForm").submit(function (event) {
        event.preventDefault(); // Prevent the default form submission

        var username = $("#Username").val();
        var password = $("#Password").val();

        $.ajax({
            url: "/Account/Login",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Username: username, Password: password }),
            success: function (response) {
                if (response.success) {
                    if (response.role === "Administrator") {
                        window.location.href = "/Dashboard/Index"; // Redirect to Dashboard
                    } else {
                        window.location.href = "/Home/Index"; // Redirect to Home for others
                    }
                } else {
                    $("#errorMessage").text(response.message); // Show error message
                }
            },
            error: function () {
                $("#errorMessage").text("Login failed. Please try again.");
            }
        });
    });
});
