var quill = new Quill('#editor-container', {
    theme: 'snow'
});

document.getElementById('relatedDocs').addEventListener('change', function (event) {
    const docList = document.getElementById('docList');
    docList.innerHTML = '';
    const files = event.target.files;
    Array.from(files).forEach(file => {
        const listItem = document.createElement('li');
        listItem.classList.add('list-group-item');
        listItem.textContent = file.name;
        docList.appendChild(listItem);
    });
});

var win = navigator.platform.indexOf('Win') > -1;
if (win && document.querySelector('#sidenav-scrollbar')) {
    var options = {
        damping: '0.5'
    }
    Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
}
let uploadedImage = null;
const projectImageInput = document.getElementById('imagePreview');
const userId = localStorage.getItem('userId');
const projectId = localStorage.getItem('projectId');
const jwtToken = localStorage.getItem('jwtToken');
const checkrole = localStorage.getItem('loggingRole');
const roleid = localStorage.getItem('loggingRoleId');

document.addEventListener('DOMContentLoaded', function () {
    const jwtToken = localStorage.getItem('jwtToken');
    const checkrole = localStorage.getItem('loggingRole');
    function loadProjectDetails() {
        $.ajax({
            url: `https://localhost:7294/api/Projects/GetProjectDetailsById/${projectId}`,
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (response.success && response.data) {
                    populateProjectDetails(response.data);
                } else {
                    alert('Failed to load project details: ' + response.message);
                }
            },
            error: function () {
                alert('Error loading project details');
            }
        });
    }

    if (checkrole === "Project Management Board") {
        const saveBtnContainer = document.getElementById('save-btn');
        const updateBtn = document.createElement('button');
        updateBtn.textContent = 'Update';
        updateBtn.className = 'btn btn-primary btn-lg';
        updateBtn.disabled = true;
        updateBtn.id = 'update-btn';
        saveBtnContainer.appendChild(updateBtn);

        const fieldsToWatch = ['projectName', 'projectProcess', 'projectStatus', 'editor-container','choseImage'];
        fieldsToWatch.forEach(id => {
            document.getElementById(id).addEventListener('input', () => {
                updateBtn.disabled = false;
            });
        });

        updateBtn.addEventListener('click', function () {
            const updateData = prepareUpdateData();

            $.ajax({
                url: 'https://localhost:7294/api/Projects/UpdateProject',
                type: 'PUT',
                headers: {
                    'Authorization': `Bearer ${jwtToken}`
                },
                data: updateData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data.success) {
                        alert('Project updated successfully!');
                        location.reload();
                    } else {
                        alert('Failed to update project: ' + data.message);
                    }
                },
                error: function (xhr, status, error) {
                    alert('Error updating project: ' + xhr.responseText || status);
                }
            });
        });
    }
    function prepareUpdateData() {
        const formData = new FormData();
        formData.append('ProjectId', projectId);
        formData.append('UserId', userId);
        formData.append('RoleId', roleid);
        formData.append('projectName', document.getElementById('projectName').value);
        formData.append('Description', quill.getText());
        if (uploadedImage) {
            formData.append('LogoFile', uploadedImage);
        }
        formData.append('Status', document.getElementById('projectProcess').value);
        formData.append('isActive', document.getElementById('projectStatus').value);


        return formData;
    }

    function populateProjectDetails(project) {
        document.getElementById('projectName').value = project.projectName;
        document.getElementById('constructionInfo').value = `${project.lastName} ${project.firstName}`;
        document.getElementById('unit').value = new Date(project.dateOfCreation).toISOString().split('T')[0];
        document.getElementById('projectProcess').value = project.status;
        document.getElementById('projectStatus').value = project.isActive;

        quill.setText(project.description);

        if (project.image) {
            const imagePreview = document.getElementById('imagePreview');
            imagePreview.innerHTML = `
                        <div class="col-md-4 mb-3">
                            <img src="${project.image}" alt="Project Image" class="img-fluid" id="existing-image">
                        </div>`;
        }

        //displayFiles(files);

    }
    loadProjectDetails();
});

projectImageInput.addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        uploadedImage = file;
        const imagePreview = document.getElementById('imagePreview');
        imagePreview.innerHTML = `
                        <div class="col-md-4 mb-3">
                            <img src="${URL.createObjectURL(file)}" alt="New Project Image" class="img-fluid">
                        </div>`;
    }
});

function displayFiles(files) {
    const docList = document.getElementById('docList');
    docList.innerHTML = '';

    files.forEach(file => {
        const listItem = document.createElement('li');
        listItem.classList.add('list-group-item');
        listItem.textContent = file;
        listItem.style.cursor = 'pointer';

        listItem.addEventListener('click', function () {
            downloadFile(file);
        });

        docList.appendChild(listItem);
    });
}

function downloadFile(fileName) {
    window.location.href = `https://localhost:7294/api/Projects/downloadattachmentfilewithnotypebyfilename/${fileName}`;
}