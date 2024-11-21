function validateAndSubmit() {
            const currentPassword = document.getElementById('currentPassword').value;
            const newPassword = document.getElementById('newPassword').value;
            const confirmPassword = document.getElementById('confirmPassword').value;

            let valid = true;

            document.getElementById('currentPasswordError').textContent = '';
            document.getElementById('newPasswordError').textContent = '';
            document.getElementById('confirmPasswordError').textContent = '';

            if (currentPassword === '') {
                document.getElementById('currentPasswordError').textContent = 'Vui lòng nhập mật khẩu hiện tại.';
                valid = false;
            }

            if (newPassword === '') {
                document.getElementById('newPasswordError').textContent = 'Vui lòng nhập mật khẩu mới.';
                valid = false;
            }
            if (confirmPassword !== newPassword) {
                document.getElementById('confirmPasswordError').textContent = 'Mật khẩu xác nhận không khớp.';
                valid = false;
            }
            if (valid) {
                alert('Mật khẩu của bạn đã được cập nhật thành công!');
            }
        }