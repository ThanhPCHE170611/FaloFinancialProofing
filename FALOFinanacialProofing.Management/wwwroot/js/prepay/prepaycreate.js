
$(document).ready(function () {
    const userId = localStorage.getItem('userId');
    const jwtToken = localStorage.getItem('jwtToken');
    const campaignId = localStorage.getItem('campaignId');
    const checkrole = localStorage.getItem('loggingRole');

    document.getElementById('attachments').addEventListener('change', function (e) {
        const fileList = document.getElementById('fileList');
        fileList.innerHTML = '';
        Array.from(e.target.files).forEach(file => {
            const li = document.createElement('li');
            const a = document.createElement('a');
            a.href = URL.createObjectURL(file);
            a.textContent = file.name;
            a.download = file.name;
            li.appendChild(a);
            fileList.appendChild(li);
        });
    });

    if (checkrole && checkrole !== 'Volunteer') {
        const newLink = document.createElement('a');
        newLink.setAttribute('asp-controller', 'Prepay');
        newLink.setAttribute('asp-action', 'PrepayManagement_PM');
        newLink.textContent = 'Prepay Created Request';

        const newLink2 = document.createElement('a');
        newLink2.setAttribute('asp-controller', 'Payment');
        newLink2.setAttribute('asp-action', 'PaymentManagement_PM');
        newLink2.textContent = 'Payment Created Request';

        const url = new URL(`/Prepay/PrepayManagement_PM`, window.location.origin);


        const url2 = new URL(`/Payment/PaymentManagement_PM`, window.location.origin);

        newLink.href = url.toString();
        newLink2.href = url2.toString();
        navTabs.appendChild(newLink);
        navTabs.appendChild(newLink2);
    }

    $.ajax({
        url: `https://localhost:7294/api/Users/getuserdebincampaign?userId=${userId}&campaignId=${campaignId}`,
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            console.log('User Debt Response:', response);
            if (response.success && response.data !== 0) {
                const debtSection = `
                    <div class="mb-3">
                        <label class="form-label">Current Debt</label>
                        <p class="form-control" id="userDebt" readonly>${response.data.toLocaleString()} VND</p>
                    </div>
                `;
                $('.card-body').prepend(debtSection);
            } else if (!response.success) {
                alert('Error fetching user debt: ' + response.message);
            }
        },
        error: function () {
            alert('Failed to fetch user debt. Please try again.');
        }
    });

    var today = new Date();
    var year = today.getFullYear();
    var month = String(today.getMonth() + 1).padStart(2, '0');
    var day = String(today.getDate()).padStart(2, '0');
    var hours = String(today.getHours()).padStart(2, '0');
    var minutes = String(today.getMinutes()).padStart(2, '0');
    var seconds = String(today.getSeconds()).padStart(2, '0');

    var date = `${year}-${month}-${day}`;
    var time = `${hours}:${minutes}:${seconds}`;
    var dateTime = `${date}T${time}`;
    console.log(dateTime);

    let apiUrl;
    if (checkrole === "Volunteer") {
        apiUrl = `https://localhost:7294/api/RequestForm/getapproverlistforvolunteer/${campaignId}`;
    } else if (checkrole === "Volunteer Leader") {
        apiUrl = `https://localhost:7294/api/RequestForm/getapproverforvolunteerleader/${campaignId}`;
    } else if (checkrole === "Accounting") {
        apiUrl = `https://localhost:7294/api/RequestForm/getapproverforaccounting/${campaignId}`;
    } else if (checkrole === "Project Manager") {
        apiUrl = `https://localhost:7294/api/RequestForm/getapproverforprojectmanagement/${campaignId}`;
    } else {
        alert('Invalid role. Please check your role and try again.');
        return;
    }

    $.ajax({
        url: apiUrl,
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success && response.data.length > 0) {
                console.log(response.data.length);
                response.data.forEach(function (approver) {
                    console.log(approver.fullName);
                    $('#assignFrom').append(new Option(approver.fullName, approver.userId));
                });
            } else if (response.success && response.data.length === undefined) {
                $('#assignFrom').append(new Option(response.data.fullName, response.data.userId));
            } else {
                alert('No approver found for this campaign.');
            }
        },
        error: function () {
            alert('Failed to load approvers. Please try again.');
        }
    });

    $('#create-btn').on('click', function () {
        const expectedMoney = $('#expectedMoney').val();
        const assignFrom = $('#assignFrom').val();
        const description = $('#description').val();
        const files = $('#attachments')[0].files;

        if (!expectedMoney || !assignFrom || !description) {
            alert('Please fill in all required fields.');
            return;
        }

        const formData = new FormData();
        formData.append('CreatedBy', userId);
        formData.append('CampaignId', campaignId);
        formData.append('ExpectedMoney', expectedMoney);
        formData.append('Description', description);
        formData.append('ApproverId', assignFrom);
        formData.append('CreateAt', dateTime);

        Array.from(files).forEach(file => {
            formData.append('UploadFiles', file);
        });

        console.log(assignFrom);
        console.log(userId);
        console.log(expectedMoney);
        console.log(campaignId);
        console.log(description);
        console.log(dateTime);
        console.log(jwtToken);

        $.ajax({
            url: 'https://localhost:7294/api/RequestForm/creatnewprepayrequest',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (response.success) {
                    alert('Create new PrePay RequestForm successfully.');
                    window.location.href = `/Prepay/PrepayManagement?campaignid=${campaignId}`;
                } else {
                    alert('Error: ' + response.Message);
                }
            },
            error: function () {
                alert('Failed to create new PrePay request. Please try again.');
            }
        });
    });
});

