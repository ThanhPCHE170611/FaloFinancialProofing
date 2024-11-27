$(document).ready(function () {
    $('#save-btn button').on('click', function (e) {
        e.preventDefault();

        const jwtToken = localStorage.getItem('jwtToken');

        const firstName = $('#fullName').val().trim();
        const lastName = $('#lastName').val().trim();
        const email = $('#email').val().trim();
        const userName = $('#use').val().trim();
        const password = $('#password').val().trim();
        const confirmPassword = $('#confirmPassword').val().trim();
        const birthDate = $('#dob').val();
        const gender = $('input[name="gender"]:checked').val();
        const address = $('#address').val().trim();
        const phoneNumber = $('#phone').val().trim();;
        const roles = [];
        $('.role-checkbox:checked').each(function () {
            roles.push($(this).val());
        });
        if (!firstName || !lastName || !email || !userName || !password || !confirmPassword || !birthDate || !gender || !address || !phoneNumber || roles.length === 0) {
            alert('Please fill in all required fields.');
            return;
        }

        console.log(birthDate);

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            alert('Please enter a valid email address.');
            return;
        }

        const nameRegex = /^[a-zA-ZÀ-ỹ\s]+$/u;
        if (!nameRegex.test(firstName.trim())) {
            alert('First name should only contain letters and spaces, with no special characters.');
            return;
        }

        if (!nameRegex.test(lastName.trim())) {
            alert('Last name should only contain letters and spaces, with no special characters.');
            return;
        }
        const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
        if (!passwordRegex.test(password)) {
            alert('Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.');
            return;
        }
        if (password !== confirmPassword) {
            alert('Password and Confirm Password do not match.');
            return;
        }

        const userNameRegex = /^[a-zA-Z0-9_]+$/;
        if (!userNameRegex.test(userName)) {
            alert('Username should be alphanumeric without spaces or special characters.');
            return;
        }

        if (address.length < 8) {
            alert('Address should be at least 8 characters long.');
            return;
        }

        const phoneRegex = /^[0-9]{10,15}$/;
        if (!phoneRegex.test(phoneNumber)) {
            alert('Phone number should only contain 10 to 15 digits.');
            return;
        }

        const birthDateRegex = /^\d{4}-\d{2}-\d{2}$/;
        if (!birthDateRegex.test(birthDate)) {
            alert('Date of Birth must be in the format YYYY-MM-DD.');
            return;
        }
        const enteredDate = new Date(birthDate);
        const currentDate = new Date();

        if (isNaN(enteredDate.getTime())) {
            alert('Invalid date. Please enter a valid date in the format YYYY-MM-DD.');
            return;
        }

        if (enteredDate <= currentDate) {
            alert('Date of Birth must be after today.');
            return;
        }
        const registerRequest = {
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password,
            userName: userName,
            birthDate: birthDate,
            gender: gender,
            address: address,
            phoneNumber: phoneNumber,
            roles: roles
        };
        console.log(registerRequest);
        $.ajax({
            url: 'https://localhost:7294/api/Users/Admin-Register',
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${jwtToken}`,
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(registerRequest),
            success: function (response) {
                if (response.success) {
                    alert('User registered successfully!');
                    window.location.href = `/User/UserManagement`;
                } else {
                    alert('Registration failed: ' + response.message);
                }
            },
            error: function (errors) {
                console.log(errors);
                alert('Error registering the user');
            }
        });
    });
});