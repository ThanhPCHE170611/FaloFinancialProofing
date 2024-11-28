function previewImage() {
        var fileInput = document.getElementById('profilePicture');
        var imagePreview = document.getElementById('imagePreview');
        var file = fileInput.files[0];
        var reader = new FileReader();

        reader.onload = function(e) {
            var img = document.createElement('img');
            img.src = e.target.result;
            imagePreview.innerHTML = '';
            imagePreview.appendChild(img);
        }

        if (file) {
            reader.readAsDataURL(file);
        }
    }
    document.querySelectorAll('.sdg-tags span').forEach(function (tag) {
        tag.addEventListener('click', function () {
            this.classList.toggle('active');
        });
    });
    
    $(document).ready(function () {
        const userId = '@userId';
        const jwtToken = '@jwt';
        loadUserProfile(userId);


        $('#socialMediaContainer').on('click', '.add-social-btn', function () {
            const newInput = `<div class="input-group mb-2">
                <input type="url" class="form-control social-media-input" data-id="0" placeholder="Enter social media link">
                <button type="button" class="btn btn-outline-danger remove-social-btn">-</button>
            </div>`;
            $('#socialMediaContainer').append(newInput);
        });

        $('#socialMediaContainer').on('click', '.remove-social-btn', function () {
            $(this).closest('.input-group').remove();
        });

        function loadUserProfile(userId) {
            $.ajax({
                url: `https://localhost:7294/api/Users/GetUserProfile/${userId}`,
                type: 'GET',
                headers: {
                    'Authorization': `Bearer ${jwtToken}`
                },
                success: function (response) {
                    if (response.success) {
                        const data = response.data;
                        $('#fullName').val(`${data.firstName} ${data.lastName}`);
                        $('#dob').val(data.birthDate);
                        if (data.gender === true) {
                            $('#male').prop('checked', true);
                        } else if (data.gender === false) {
                            $('#female').prop('checked', true);
                        }
                        $('#address').val(data.address);
                        $('#workplace').val(data.workPlace);
                        $('#bio').val(data.bio);
                        $('#education').val(data.education);
                        $('#skills').val(data.skill);
                        $('#hobbies').val(data.hobby);
                        $('#strengths').val(data.strength);
                        $('#email').val(data.email);
                        $('#phone').val(data.phoneNumber);
                        const socialMediaContainer = $('#socialMediaContainer');
                        socialMediaContainer.empty();
                        if (data.socialNetworkRequests && data.socialNetworkRequests.length > 0) {
                            data.socialNetworkRequests.forEach(social => {
                                const socialInput = `<div class="input-group mb-2">
                                    <input type="url" class="form-control social-media-input" data-id="${social.id}" value="${social.socialNetworksLink}" placeholder="Enter social media link">
                                </div>`;
                                socialMediaContainer.append(socialInput);
                            });
                        } else {
                            const blankInput = `<div class="input-group mb-2">
                            <input type="url" class="form-control social-media-input" data-id="0" placeholder="Enter social media link">
                            </div>`;
                            socialMediaContainer.append(blankInput);
                        }
                        $('#volunteerExperience').val(data.volunteerExperience);
                        $('#volunteerGoals').val(data.volunteerGoal);
                        const activeSDGIds = data.userSDGInformations.map(item => item.sDGInformation.id);
                        $('#sdgList span').each(function () {
                            const sdgId = $(this).data('id');
                            if (activeSDGIds.includes(sdgId)) {
                            $(this).addClass('active');
                            }
                        });
                        if (data.image) {
                            $('#imagePreview').html(`<img src="${data.image}" alt="Profile Picture">`);
                        }
                    } else {
                        alert(response.message);
                    }
                },
                error: function (err) {
                    console.error('Error loading profile:', err);
                }
            });
        }


        $('#profilePicture').on('change', function () {
            var file = this.files[0];
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.createElement('img');
                img.src = e.target.result;
                $('#imagePreview').html(img);
            }
            if (file) {
                reader.readAsDataURL(file);
            }
        });

        $(document).on('input change', 'input, textarea, select', function () {
            $('#updateProfileBtn').prop('disabled', false);
        });

        $('#updateProfileBtn').on('click', function () {
            const socialNetworkRequests = [];
            $('.social-media-input').each(function () {
                const socialLink = $(this).val();
                const socialId = $(this).data('id') || 0;
                 if (socialLink) {
                    socialNetworkRequests.push({
                        id: socialId,
                        socialNetworksLink: socialLink,
                        userId: userId
                    });
                }
            });
            console.log('SocialNetworkRequestJsons:', JSON.stringify(socialNetworkRequests));

            const sdgUserRequests = [];
            $('#sdgList span').each(function () {
                sdgUserRequests.push({
                    userId: userId,
                    sdgId: $(this).data('id'),
                    isActive: $(this).hasClass('active')
                });
            });
            console.log('SDGUserRequestJsons:', JSON.stringify(sdgUserRequests));


            const gender = $('input[name="gender"]:checked').val();
            const dob = $('#dob').val();
            const phoneNumber = $('#phone').val();
            let validationErrors = [];

            if (!gender) {
                validationErrors.push('Gender field cannot be empty.');
            }
            if (!dob) {
                validationErrors.push('Date of Birth cannot be empty.');
            } else {
                const dobDate = new Date(dob);
                const currentDate = new Date();
                if (dobDate > currentDate) {
                    validationErrors.push('Date of Birth must be in the past.');
                }
            }
            const phonePattern = /^[0-9]{10}$/;
            if (!phoneNumber) {
                validationErrors.push('Phone Number cannot be empty.');
            } else if (!phonePattern.test(phoneNumber)) {
                validationErrors.push('Phone Number must be 10 digits.');
            }

            if (validationErrors.length > 0) {
                alert('Validation errors:\n' + validationErrors.join('\n'));
                return;
            }

            var formData = new FormData();
            formData.append('Id', userId);
            formData.append('FirstName', $('#fullName').val().split(' ')[0]);
            formData.append('LastName', $('#fullName').val().split(' ').slice(1).join(' '));
            formData.append('BirthDate', dob);
            formData.append('Gender', gender);
            formData.append('Address', $('#address').val());
            formData.append('WorkPlace', $('#workplace').val());
            formData.append('Bio', $('#bio').val());
            formData.append('Education', $('#education').val());
            formData.append('Skill', $('#skills').val());
            formData.append('Hobby', $('#hobbies').val());
            formData.append('Strength', $('#strengths').val());
            formData.append('VolunteerExperience', $('#volunteerExperience').val());
            formData.append('VolunteerGoal', $('#volunteerGoals').val());
            formData.append('PhoneNumber', phoneNumber);
            formData.append('SocialNetworkRequestJsons', JSON.stringify(socialNetworkRequests));
            formData.append('SDGUserRequestJsons', JSON.stringify(sdgUserRequests));
            console.log('Final formData:', socialNetworkRequests, sdgUserRequests);
            var logoFile = $('#profilePicture')[0].files[0];
            if (logoFile) {
                formData.append('LogoFile', logoFile);
            }

            $.ajax({
                url: 'https://localhost:7294/api/Users/UpdateUserProfile',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                headers: {
                    'Authorization': `Bearer ${jwtToken}`
                },
                success: function (response) {
                    if (response.success) {
                        alert('Profile updated successfully!');
                        console.log($('#fullName').val());
                        localStorage.setItem('fullname', $('#fullName').val());
                        location.reload();
                    } else {
                    alert('Failed to update profile: ' + response.message);
                    }
                },
                error: function (errors) {
                    if (errors.responseJSON && errors.responseJSON.errors) {
                        console.error('Detailed Validation Errors:');
                        for (const [field, messages] of Object.entries(errors.responseJSON.errors)) {
                            console.group(`Field: ${field}`);
                            messages.forEach(message => console.error(`Error: ${message}`));
                            console.groupEnd();
                        }
                        alert('Validation errors occurred. Check console for details.');
                    } else {
                        console.error('Unexpected error:', errors);
                        alert('An unexpected error occurred. Check console for details.');
                    }
                }
            });
        });
    });