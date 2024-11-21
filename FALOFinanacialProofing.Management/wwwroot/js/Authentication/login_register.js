$(document).ready(function () {
    // Xử lý khi ấn nút Login
    document.querySelector('.img__btn').addEventListener('click', function () {
        document.querySelector('.cont').classList.toggle('s--signup');
    });
    $('#loginBtn').click(function () {
        console.log('Login button clicked');
        $('#loginMessage').text('');
        const username = $('#username').val().trim();
        const password = $('#password').val();
        if (!username || !password) {
            $('#loginMessage').html('<span class="text-danger">Username and Password cannot be empty.</span>');
            return;
        }

        if (/\s/.test(username)) {
            $('#loginMessage').html('<span class="text-danger">Username cannot contain spaces. Please remove any spaces.</span>');
            return;
        }

        const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
        if (!passwordRegex.test(password)) {
            $('#loginMessage').html('<span class="text-danger">Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.</span>');
            return;
        }


        const loginData = {
            Username: username,
            Password: password
        };

        $('#loginMessage').text('');
        $.ajax({
            url: 'https://localhost:7294/api/Users/Login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(loginData),
            success: function (response) {
                if (response.success) {
                    console.log(response.data.accessToken);
                    localStorage.setItem('jwtToken', response.data.accessToken);

                    const jwtToken = localStorage.getItem('jwtToken');
                    const decodedToken = jwt_decode(jwtToken);

                    const userId = decodedToken.nameid;
                    const roleId = decodedToken.RoleId;
                    localStorage.setItem('userId', userId);
                    localStorage.setItem('loggingRoleId', roleId);
                    const sub = decodedToken.sub;
                    localStorage.setItem('fullname', sub);

                    let roles = decodedToken.role || [];
                    if (!Array.isArray(roles)) {
                        localStorage.setItem('loggingRole', roles);
                        console.log(roles);
                        $('#loginMessage').html('<span class="text-success">Login Success</span>');
                        setTimeout(() => {
                            window.location.href = '/Home/Index';
                        }, 1000);
                    } else {
                        $('#loginMessage').html('<span class="text-success">Login Success</span>');
                        setTimeout(() => {
                            window.location.href = '/Authentication/ChooseRole';
                        }, 1000);
                    }
                } else {
                    $('#loginMessage').html('<span class="text-danger">' + response.message + '</span>');
                }
            },
            error: function () {
                $('#loginMessage').html('<span class="text-danger">An error occurred. Please try again.</span>');
            }
        });
    });

    $('#registerBtn').click(function () {
        console.log('Register button clicked');

        $('#registerMessage').text('');

        const firstname = $('#firstname').val().trim();
        const lastname = $('#lastname').val().trim();
        const username = $('#usernameRegister').val().trim();
        const email = $('#email').val().trim();
        const password = $('#passwordRegister').val();
        const confirmPassword = $('#confirmPassword').val();

        if (!firstname || !lastname || !username || !email || !password || !confirmPassword) {
            $('#registerMessage').html('<span class="text-danger">All fields are required. Please fill them out.</span>');
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            $('#registerMessage').html('<span class="text-danger">Invalid email format. Please enter a valid email address.</span>');
            return;
        }

        if (/\s/.test(username)) {
            $('#registerMessage').html('<span class="text-danger">Username cannot contain spaces. Please remove any spaces.</span>');
            return;
        }

        const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
        if (!passwordRegex.test(password)) {
            $('#registerMessage').html('<span class="text-danger">Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.</span>');
            return;
        }

        if (password !== confirmPassword) {
            $('#registerMessage').html('<span class="text-danger">Passwords do not match. Please try again.</span>');
            return;
        }

        const registerData = {
            FirstName: firstname,
            LastName: lastname,
            UserName: username,
            Email: email,
            Password: password
        };

        $.ajax({
            url: 'https://localhost:7294/api/Users/Register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(registerData),
            success: function (response) {
                if (response.success) {
                    $('#registerMessage').html('<span class="text-success">Register Success</span>');
                    setTimeout(() => {
                        window.location.href = '/Authentication/Login';
                    }, 1000);
                } else {
                    $('#registerMessage').html('<span class="text-danger">' + response.message + '</span>');
                }
            },
            error: function (errors) {
                $('#registerMessage').html('<span class="text-danger">An error occurred. Please try again.</span>');
                console.error(errors);
            }
        });
    });

});
