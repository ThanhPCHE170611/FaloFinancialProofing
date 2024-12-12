$(document).ready(function () {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    $('form').on('submit', function (event) {
        event.preventDefault();

        const email = $('#email').val().trim();
        const jwtToken = localStorage.getItem('jwtToken');
        ưif(!jwtToken) {
            window.location.href = '/Authentication/Login';
            return;
        }
        if (!email) {
            alert("Email field cannot be empty.");
            return;
        }

        if (!emailRegex.test(email)) {
            alert("Please enter a valid email address.");
            return;
        }

        $.ajax({
            url: `${apiBaseUrl}/api/Users/ForgotPassword?email=${email}`,
            method: 'POST',
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    window.location.href = '/UserProfile/ChangePassword';
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error:', error);
                alert('An error occurred while processing your request. Please try again later.');
            }
        });
    });
});