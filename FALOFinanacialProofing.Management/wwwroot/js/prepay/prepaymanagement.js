
let currentDeclineButton = null;
let currentRequestId = null;
function showDeclinePopup(button, requestId) {
    currentDeclineButton = button;
    currentRequestId = requestId;
    document.getElementById('declineModal').style.display = 'block';
}
function submitDecline() {
    const userId = localStorage.getItem('userId');
    const checkrole = localStorage.getItem('loggingRole');
    const jwtToken = localStorage.getItem('jwtToken');
    if (!currentDeclineButton || !currentRequestId) return;

    const reason = document.getElementById('declineReason').value.trim();
    if (!reason) {
        alert('Please enter a reason for decline.');
        return;
    }

    

    let apiUrl = '';
    if (checkrole === "Volunteer Leader") {
        apiUrl = `https://localhost:7294/api/ApproveProcess/rejectprepayrequestforvolunteerleader/${currentRequestId}?userid=${userId}&currentLoggingRole=${checkrole}&feedback=${reason}`;
    } else if (checkrole === "Accounting") {
        apiUrl = `https://localhost:7294/api/ApproveProcess/rejectprepayrequestforaccounting/${currentRequestId}?userid=${userId}&currentLoggingRole=${checkrole}&feedback=${reason}`;
    } else if (checkrole === "Project Manager") {
        apiUrl = `https://localhost:7294/api/ApproveProcess/rejectprepayrequestforprojectmanager/${currentRequestId}?userid=${userId}&currentLoggingRole=${checkrole}&feedback=${reason}`;
    } else {
        alert('Invalid role. Cannot decline request.');
        closeModal();
        return;
    }
    $.ajax({
        url: apiUrl,
        method: 'POST',
        data:{
            userid: userId,
            currentLoggingRole: checkrole,
            feedback: reason
        },
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success) {
                updateRowToRejected(currentDeclineButton, currentRequestId);
                alert('Request declined successfully.');
            } else {
                alert(response.message || 'Failed to decline the request.');
            }
            closeModalDeclinePrepay();
        },
        error: function (xhr) {
            alert(`Error ${xhr.status}: ${xhr.responseText || 'An error occurred while declining the request.'}`);
            closeModalDeclinePrepay();
        }
    });
}
function closeModalDeclinePrepay() {
    document.getElementById('declineModal').style.display = 'none';
    document.getElementById('declineReason').value = '';
}

window.onclick = function (event) {
    const declineModal = document.getElementById('declineModal');
    if (event.target === declineModal) {
        closeModal();
    }
}
function showFileUploadPopup(requestId) {

    const fileInputHtml = `
                <div id="fileUploadPopup" class="modalFile">
                    <div class="modalFile-content">
                        <span class="close" onclick="closeFileUploadPopup()">&times;</span>
                        <h3>Submit Prepay Voucher</h3>
                        <div class="mb-3">
                            <label for="voucherFiles" class="form-label">Attachments</label>
                            <input type="file" class="form-control" id="voucherFiles" multiple>
                            <ul id="fileList" style="margin-top: 10px; display: flex; flex-wrap: wrap; gap: 5px;"></ul>
                        </div>
                        <button class="btn btn-primary mt-3" onclick="submitVoucherFiles(${requestId})">Submit</button>
                        <button class="btn btn-secondary mt-3" onclick="closeFileUploadPopup()">Cancel</button>
                    </div>
                </div>
            `;

    $('body').append(fileInputHtml);
    document.getElementById('voucherFiles').addEventListener('change', function (e) {
        const fileList = document.getElementById('fileList');
        fileList.innerHTML = '';
        Array.from(e.target.files).forEach(file => {
            const li = document.createElement('li');
            li.style.cssText = "padding: 5px 10px; background-color: #e0e0e0; border-radius: 5px; list-style: none;";
            const a = document.createElement('a');
            a.href = URL.createObjectURL(file);
            a.textContent = file.name;
            a.download = file.name;
            li.appendChild(a);
            fileList.appendChild(li);
        });
    });
}

function closeFileUploadPopup() {
    $('#fileUploadPopup').remove();
}


function submitVoucherFiles(requestId) {
    const userId = localStorage.getItem('userId');
    const checkrole = localStorage.getItem('loggingRole');
    const jwtToken = localStorage.getItem('jwtToken');
    const files = $('#voucherFiles')[0].files;

    if (files.length === 0) {
        alert("Please select at least one file.");
        return;
    }

    console.log("User ID:", userId);
    console.log("Current Logging Role:", checkrole);
    console.log("Request ID:", requestId);

    const formData = new FormData();
    // formData.append('userid', userId);
    // formData.append('currentLoggingRole', checkrole);
    // formData.append('requestid', requestId);

    Array.from(files).forEach(file => {
        console.log(file);
        formData.append('voucherFiles', file);
    });

    $.ajax({
        url: `https://localhost:7294/api/ApproveProcess/approverequestforaccounting?userid=${userId}&currentLoggingRole=${checkrole}&requestid=${requestId}`,
        method: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success) {
                updateRowToApprovedForAcc(requestId);
                closeFileUploadPopup();
                alert(response.message || "Voucher files submitted and request approved successfully.");
            } else {
                alert(response.message || "Failed to submit voucher files or approve request.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error details:", xhr.responseText);
            alert('Error occurred while submitting voucher files. Please try again.');
        }
    });
}


function closeFileUploadPopup() {
    $('#fileUploadPopup').remove();
}

function filterTable() {
    const searchInput = document.getElementById('searchBox').value.toLowerCase();
    const statusFilter = document.getElementById('statusFilter').value.toLowerCase();
    const table = document.getElementById('prepayTable');
    const rows = table.getElementsByTagName('tr');
    for (let i = 1; i < rows.length; i++) {
        const sender = rows[i].cells[1].innerText.toLowerCase();
        const status = rows[i].cells[6].innerText.toLowerCase();

        const matchesSearch = sender.includes(searchInput);
        const matchesStatus = !statusFilter || status === statusFilter;
        rows[i].style.display = (matchesSearch && matchesStatus) ? '' : 'none';
    }
}

function approveRequest(button, requestId) {

    const userId = localStorage.getItem('userId');
    const checkrole = localStorage.getItem('loggingRole');
    const jwtToken = localStorage.getItem('jwtToken');
    if (checkrole === "Volunteer Leader") {
        $.ajax({
            url: `https://localhost:7294/api/ApproveProcess/approverequestforvolunteerleader/${requestId}`,
            method: 'GET',
            data: {
                userid: userId,
                currentLoggingRole: checkrole
            },
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                console.log(checkrole);
                console.log(userId);
                if (response.success) {
                    updateRowToApproved(button, requestId);
                    alert("Request approved successfully for Volunteer Leader.");
                } else {
                    alert(response.message || "Failed to approve the request.");
                }
            },
            error: function () {
                alert('Error occurred while approving the request for Volunteer Leader. Please try again.');
            }
        });
    }
    else if (checkrole === "Accounting") {
        $.ajax({
            url: `https://localhost:7294/api/ApproveProcess/approverequestforaccounting/${requestId}`,
            method: 'GET',
            data: {
                userid: userId,
                currentLoggingRole: checkrole
            },
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (response.success) {
                    console.log(checkrole);
                    console.log(userId);
                    console.log(requestId);
                    //showFileUploadPopup(requestId);
                    updateRowToApprovedForPM(button, requestId);
                    alert("Request approved successfully for Accouting");
                    location.reload();
                } else {
                    alert(response.message || "Failed to approve the request.");
                }
            },
            error: function () {
                alert('Error occurred while approving the request for Accounting. Please try again.');
            }
        });
    }
    if (checkrole === "Project Manager") {
        $.ajax({
            url: `https://localhost:7294/api/ApproveProcess/approverequestforprojectmanager/${requestId}`,
            method: 'GET',
            data: {
                userid: userId,
                currentLoggingRole: checkrole
            },
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                console.log(checkrole);
                console.log(userId);
                if (response.success) {
                    updateRowToApprovedForPM(button, requestId);
                    alert("Request approved successfully for Volunteer Leader.");
                } else {
                    alert(response.message || "Failed to approve the request.");
                }
            },
            error: function () {
                alert('Error occurred while approving the request for Volunteer Leader. Please try again.');
            }
        });
    }
}


function updateRowToRejected(button, id) {
    const row = button.closest('tr');
    const actionCell = row.querySelector('td:nth-child(9)');
    const statusCell = row.querySelector('td:nth-child(7)');

    const declinedText = document.createElement('span');
    declinedText.classList.add('badge', 'badge-sm', 'bg-gradient-danger');
    declinedText.textContent = 'Rejected';

    button.style.display = 'none';
    actionCell.querySelector('.btn-info').style.display = 'none';
    actionCell.querySelector('.btn-success').style.display = 'none';

    const rejectedText = document.createElement('span');
    rejectedText.classList.add('text-declined', 'me-2');
    rejectedText.textContent = 'Rejected';

    const detailLink = document.createElement('a');
    detailLink.href = `/Prepay/PrepayDetail?requestId=${id}`;
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';

    actionCell.appendChild(rejectedText);
    actionCell.appendChild(detailLink);
    statusCell.innerHTML = '';
    statusCell.appendChild(declinedText);
}

function updateRowToApproved(button, id) {
    const row = button.closest('tr');
    const actionCell = row.querySelector('td:nth-child(9)');
    const statusCell = row.querySelector('td:nth-child(7)');


    button.style.display = 'none';
    actionCell.querySelector('.btn-info').style.display = 'none';
    actionCell.querySelector('.btn-danger').style.display = 'none';

    const approvedText = document.createElement('span');
    approvedText.classList.add('text-approved', 'me-2');
    approvedText.textContent = 'Approved';

    const detailLink = document.createElement('a');
    detailLink.href = `/Prepay/PrepayDetail?requestId=${id}`;
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';

    actionCell.appendChild(approvedText);
    actionCell.appendChild(detailLink);
}
function updateRowToApprovedForAcc(requestId) {
    const row = document.querySelector(`tr[data-request-id="${requestId}"]`);
    if (!row) return;
    const actionCell = row.querySelector('td:nth-child(9)');

    const successButton = actionCell.querySelector('.btn-success');
    const infoButton = actionCell.querySelector('.btn-info');
    const dangerButton = actionCell.querySelector('.btn-danger');

    if (successButton) successButton.style.display = 'none';
    if (infoButton) infoButton.style.display = 'none';
    if (dangerButton) dangerButton.style.display = 'none';

    const approvedText = document.createElement('span');
    approvedText.classList.add('text-approved', 'me-2');
    approvedText.textContent = 'Approved';

    const detailLink = document.createElement('a');
    detailLink.href = `/Prepay/PrepayDetail?requestId=${requestId}`;
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';

    actionCell.innerHTML = '';
    actionCell.appendChild(approvedText);
    actionCell.appendChild(detailLink);
}

function updateRowToApprovedForPM(button, id) {
    const row = button.closest('tr');
    const actionCell = row.querySelector('td:nth-child(9)');
    const statusCell = row.querySelector('td:nth-child(7)');

    const declinedText = document.createElement('span');
    declinedText.classList.add('badge', 'badge-sm', 'bg-gradient-success');
    declinedText.textContent = 'Approved';

    button.style.display = 'none';
    actionCell.querySelector('.btn-info').style.display = 'none';
    actionCell.querySelector('.btn-danger').style.display = 'none';

    const approvedText = document.createElement('span');
    approvedText.classList.add('text-approved', 'me-2');
    approvedText.textContent = 'Approved';

    const detailLink = document.createElement('a');
    detailLink.href = `/Prepay/PrepayDetail?requestId=${id}`;
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';

    actionCell.appendChild(approvedText);
    actionCell.appendChild(detailLink);
    statusCell.innerHTML = '';
    statusCell.appendChild(declinedText);
}

function downloadAttachment(fileName) {
    $.ajax({
        url: `https://localhost:7294/api/AttachmentFile/downloadprepayattachmentfile/${fileName}`,
        method: 'GET',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (response, status, xhr) {
            const contentType = xhr.getResponseHeader('Content-Type');
            const blob = new Blob([response], { type: contentType });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
        },
        error: function () {
            alert('Failed to download attachment file. Please try again.');
        }
    });
}

function downloadVoucher(fileName) {
    $.ajax({
        url: `https://localhost:7294/api/Voucher/downloadprepayvoucherfile/${fileName}`,
        method: 'GET',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (response, status, xhr) {
            const contentType = xhr.getResponseHeader('Content-Type');
            const blob = new Blob([response], { type: contentType });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
        },
        error: function () {
            alert('Failed to download voucher file. Please try again.');
        }
    });
}

function showAddMissingFilePopup(requestId) {
    const popupHtml = `
        <div id="addFileModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal('addFileModal')">&times;</span>
                <h3>Add Missing File</h3>
                <input type="file" id="missingFileInput" class="form-control" />
                <button class="btn btn-success mt-3" onclick="submitMissingFile(${requestId})">Submit</button>
            </div>
        </div>
    `;
    $('body').append(popupHtml);
    $('#addFileModal').show();
}

function closeModal(modalId) {
    $(`#${modalId}`).remove();
}

function submitMissingFile(requestId) {
    const jwtToken = localStorage.getItem('jwtToken');
    const fileInput = document.getElementById('missingFileInput');
    if (fileInput.files.length === 0) {
        alert('Please upload a file before submitting.');
        return;
    }

    const formData = new FormData();
    formData.append('attachment', fileInput.files[0]);

    $.ajax({
        url: `https://localhost:7294/api/RequestForm/addmissingattachmentforrequest/${requestId}`,
        method: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success) {
                alert(response.message);
                location.reload();
            } else {
                alert('Failed to add missing file: ' + response.message);
            }
        },
        error: function () {
            alert('Error occurred while adding the missing file.');
        }
    });
}
$(document).ready(function () {
    const userId = localStorage.getItem('userId');
    // const campaignId = new URLSearchParams(window.location.search).get('campaignid');
    const campaignId = localStorage.getItem('campaignId');
    const checkrole = localStorage.getItem('loggingRole');
    const jwtToken = localStorage.getItem('jwtToken');
    let currentPage = 1;
    const pageSize = 30;

    console.log(userId);
    console.log(campaignId);
    console.log(checkrole);
    console.log(jwtToken);

    //if (checkrole && checkrole !== 'Volunteer') {
    //    const nametitle = document.getElementById('nametitle');
    //    nametitle.textContent = 'Approve Prepay Request';

    //    const createRequestLink = document.getElementById('create_request_page');
    //    createRequestLink.style.display = 'none';

    //    const newLink = document.createElement('a');
    //    newLink.setAttribute('asp-controller', 'Prepay');
    //    newLink.setAttribute('asp-action', 'PrepayManagement_PM');
    //    newLink.textContent = 'Prepay Created Request';

    //    const newLink2 = document.createElement('a');
    //    newLink2.setAttribute('asp-controller', 'Payment');
    //    newLink2.setAttribute('asp-action', 'PaymentManagement_PM');
    //    newLink2.textContent = 'Payment Created Request';

    //    const url = new URL(`/Prepay/PrepayManagement_PM`, window.location.origin);


    //    const url2 = new URL(`/Payment/PaymentManagement_PM`, window.location.origin);

    //    newLink.href = url.toString();
    //    newLink2.href = url2.toString();
    //    navTabs.appendChild(newLink);
    //    navTabs.appendChild(newLink2);
    //}

    function formatDateTime(dateString) {
        if (!dateString) return 'N/A';

        const date = new Date(dateString);
        if (isNaN(date.getTime())) {
            return 'Invalid Date';
        }

        const formattedDate = date.getDate().toString().padStart(2, '0') + '-' +
            (date.getMonth() + 1).toString().padStart(2, '0') + '-' +
            date.getFullYear();

        const formattedTime = date.getHours().toString().padStart(2, '0') + ':' +
            date.getMinutes().toString().padStart(2, '0') + ':' +
            date.getSeconds().toString().padStart(2, '0');

        return formattedDate + ' ' + formattedTime;
    }


    let searchEmail = '';
    let filterStatus = '';


    function loadPrepayRequests(page) {

        let apiUrl;
        switch (checkrole) {
            case "Accounting":
                apiUrl = `https://localhost:7294/api/ApproveProcess/getallprepayrequestforaccountingincampaign/${userId}?currentLoggingRole=${checkrole}&campaignId=${campaignId}&page=${page}`;
                break;
            case "Project Manager":
                apiUrl = `https://localhost:7294/api/ApproveProcess/getallprepayrequestforprojectmanagerincampaign/${userId}?currentLoggingRole=${checkrole}&campaignId=${campaignId}&page=${page}`;
                break;
            case "Volunteer Leader":
                apiUrl = `https://localhost:7294/api/ApproveProcess/getallprepayrequestforvolunteerleaderincampaign/${userId}?currentLoggingRole=${checkrole}&campaignId=${campaignId}&page=${page}`;
                break;
            default:
                break;
        }
        apiUrl += `&status=${filterStatus}&createdByEmail=${searchEmail}`;
        $.ajax({
            url: apiUrl,
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (response.success && response.data.data.length > 0) {
                    const prepayRequests = response.data.data;
                    const tbody = $('#prepayTable tbody');
                    tbody.empty();

                    prepayRequests.forEach((request, index) => {
                        const statusClass = request.status === "Approved" ? "bg-gradient-success" :
                            request.status === "Rejected" ? "bg-gradient-danger" :
                                request.status === "Cancel" ? "bg-gradient-secondary" :
                                "bg-gradient-warning";
                        const statusLabel = `<span class="badge badge-sm ${statusClass}">${request.status}</span>`;

                        let actionButtons = `<a class="btn btn-info btn-sm" href="/Prepay/PrepayDetail?requestId=${request.id}">Detail</a>`;
                        const isApproved = request.approveProcessStatus === "Approved";
                        const isRejected = request.approveProcessStatus === "Rejected";
                        const isCancel = request.approveProcessStatus === "Cancel";
                        if (checkrole !== "Volunteer" && request.createdBy !== userId) {
                            if (isApproved) {
                                actionButtons = `<span class="text-approved me-2" style="margin-left:25px">Approved</span>
                                                    <a href="/Prepay/PrepayDetail?requestId=${request.id}" class="text-blue me-2" >Detail</a>`;
                            } else if (isRejected) {
                                actionButtons = `<span class="text-declined me-2" style="margin-left:25px">Rejected</span>
                                                    <a href="/Prepay/PrepayDetail?requestId=${request.id}" class="text-blue me-2">Detail</a>`;
                            } else if (isCancel) {
                                console.log("xx");
                                actionButtons = `<a class="btn btn-info btn-sm" href="/Prepay/PrepayDetail?requestId=${request.id}">Detail</a>`;
                            } else {
                                actionButtons = `
                                        <button class="btn btn-success btn-sm" onclick="approveRequest(this,'${request.id}' )">Approve</button>
                                        <button class="btn btn-danger btn-sm" onclick="showDeclinePopup(this, '${request.id}')">Rejected</button>
                                        ${actionButtons}
                                    `;
                            }
                        }
                        let attachmentLinks = '';
                        if (request.attachmentFiles && request.attachmentFiles.length > 0) {
                            attachmentLinks = request.attachmentFiles.map(file => `
                                <a href="javascript:void(0);" onclick="downloadAttachment('${file.filePath}')" class="btn btn-link text-info">${file.filePath}</a>
                            `).join('<br>');
                        } else {
                            attachmentLinks = '<span class="text-muted">No Attachments</span>';
                            if (request.status === "Approved" && request.createdBy === userId) {
                                actionButtons += `
                                <button class="btn btn-warning btn-sm" onclick="showAddMissingFilePopup(${request.id})">Add Missing File</button>
                            `;
                            }
                        }

                        let voucherLinks = '';
                        if (request.voucherFiles && request.voucherFiles.length > 0) {
                            const validFiles = request.voucherFiles.filter(file => file.filePath);
                            if (validFiles.length > 0) {
                                voucherLinks = validFiles.map(file => `
                                <a href="javascript:void(0);" onclick="downloadVoucher('${file.filePath}')" class="btn btn-link text-info">${file.filePath}</a>
                                `).join('<br>');
                            } else {
                                voucherLinks = '<span class="text-muted">No Attachments</span>';
                            }
                        } else {
                            voucherLinks = '<span class="text-muted">No Attachments</span>';
                        }


                        tbody.append(`
                                    <tr data-request-id="${request.id}">
                                        <td class="align-middle text-center text-sm">${index + 1}</td>
                                        <td><span class="text-secondary text-xs font-weight-bold">${request.createByName}</span></td>
                                        <td class="align-middle text-center text-sm"><span class="text-secondary text-xs font-weight-bold">${request.expectedMoney.toLocaleString()}</span></td>
                                        <td class="align-middle text-center text-sm"><span class="text-secondary text-xs font-weight-bold">${request.createByEmail}</span></td>
                                        <td class="align-middle text-center">${attachmentLinks}</td>
                                        <td class="align-middle text-center">${voucherLinks}</td>
                                        <td class="align-middle text-center text-sm">${statusLabel}</td>
                                        <td class="align-middle text-center text-sm"><span class="text-secondary text-xs font-weight-bold">${formatDateTime(request.createAt)}</span></td>
                                        <td class="align-middle text-center">${actionButtons}</td>
                                    </tr>
                                `);
                    });

                    setupPagination(response.data.totalRecords, page);
                } else {
                    $('#prepayTable tbody').html('<tr><td colspan="9" class="text-center">No RequestForms found.</td></tr>');
                }
            },
            error: function () {
                alert('Failed to load prepay requests. Please try again.');
            }
        });
    }

    //function setupPagination(totalRecords, currentPage) {
    //    const totalPages = Math.ceil(totalRecords / pageSize);
    //    const paginationContainer = $('#pagination');
    //    paginationContainer.empty();
    //    for (let i = 1; i <= totalPages; i++) {
    //        const pageButton = $(`<button class="btn btn-sm btn-page ${i === currentPage ? 'btn-primary' : 'btn-light'}">${i}</button>`);
    //        pageButton.on('click', () => changePage(i));
    //        paginationContainer.append(pageButton);
    //    }
    //}
    function setupPagination(totalRecords, currentPage) {
        const totalPages = Math.ceil(totalRecords / pageSize);
        const paginationContainer = $('#pagination');
        paginationContainer.empty();

        const maxVisibleButtons = 5;
        const ellipsis = `<span class="btn btn-sm btn-light disabled">...</span>`;

        function createPageButton(page) {
            return $(`<button class="btn btn-sm btn-page ${page === currentPage ? 'btn-primary' : 'btn-light'}">${page}</button>`)
                .on('click', () => changePage(page));
        }

        const prevButton = $(`<button class="btn btn-sm btn-page ${currentPage === 1 ? 'btn-light disabled' : 'btn-light'}">Previous</button>`);
        prevButton.on('click', () => {
            if (currentPage > 1) changePage(currentPage - 1);
        });
        paginationContainer.append(prevButton);

        let startPage = Math.max(1, currentPage - Math.floor(maxVisibleButtons / 2));
        let endPage = Math.min(totalPages, startPage + maxVisibleButtons - 1);

        if (endPage - startPage + 1 < maxVisibleButtons) {
            startPage = Math.max(1, endPage - maxVisibleButtons + 1);
        }

        if (startPage > 1) {
            paginationContainer.append(createPageButton(1));
            if (startPage > 2) {
                paginationContainer.append($(ellipsis));
            }
        }

        for (let page = startPage; page <= endPage; page++) {
            paginationContainer.append(createPageButton(page));
        }

        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                paginationContainer.append($(ellipsis));
            }
            paginationContainer.append(createPageButton(totalPages));
        }

        const nextButton = $(`<button class="btn btn-sm btn-page ${currentPage === totalPages ? 'btn-light disabled' : 'btn-light'}">Next</button>`);
        nextButton.on('click', () => {
            if (currentPage < totalPages) changePage(currentPage + 1);
        });
        paginationContainer.append(nextButton);
    }

    window.changePage = function (page) {
        if (page < 1 || page > Math.ceil(totalRecords / pageSize)) return; 
        currentPage = page;
        loadPrepayRequests(page); 
    };

    $('#searchBox').on('keyup', function () {
        searchEmail = $(this).val().trim();
        currentPage = 1;
        loadPrepayRequests(currentPage);
    });

    $('#statusFilter').on('change', function () {
        filterStatus = $(this).val();
        currentPage = 1;
        loadPrepayRequests(currentPage);
    });

    window.changePage = function (page) {
        currentPage = page;
        loadPrepayRequests(page);
    };

    loadPrepayRequests(currentPage);
});