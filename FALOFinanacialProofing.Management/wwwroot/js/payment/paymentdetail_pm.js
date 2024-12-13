
$(document).ready(function () {
    const requestId = new URLSearchParams(window.location.search).get('requestId');
    const jwtToken = localStorage.getItem('jwtToken');
    const userId = localStorage.getItem('userId');

    $.ajax({
        url: `${apiBaseUrl}/api/RequestForm/getrequestdetailbyrequestid/${requestId}`,
        type: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success) {
                const data = response.data;

                $(".left-section").html(`
    <h5 class="mb-3">Payment Request Information</h5>
    <p><strong>Sender:</strong> ${data.createByName}</p>
    <p><strong>Create Date:</strong> ${new Date(data.createAt).toLocaleDateString()}</p>
    <p><strong>Expected Money:</strong> ${data.expectedMoney.toLocaleString()} VND</p>
    <p><strong>Description:</strong> ${data.description}</p>
    <p><strong>Feedback:</strong> ${safeValue(data.feedBack)}</p>
    `);

                //if (data.attachmentFiles && data.attachmentFiles.length > 0) {
                //    const attachmentLinks = data.attachmentFiles.map(file =>
                //        `<a href="${file.filePath}" download="${file.filePath.split('_').pop()}">${file.filePath.split('_').pop()}</a>`
                //    ).join(', ');
                //    $(".left-section").append(`<p><strong>Attachment Files:</strong> ${attachmentLinks}</p>`);
                //} else {
                //    $(".left-section").append(`<p><strong>Attachment Files:</strong> No Attachment File</p>`);
                //}

                //if (data.voucherFiles && data.voucherFiles.length > 0 && data.voucherFiles.some(file => file.filePath)) {
                //    const voucherLinks = data.voucherFiles.filter(file => file.filePath).map(file =>
                //        `<a href="${file.filePath}" download="${file.filePath.split('_').pop()}">${file.filePath.split('_').pop()}</a>`
                //    ).join(', ');
                //    $(".left-section").append(`<p><strong>Voucher Files:</strong> ${voucherLinks}</p>`);
                //} else {
                //    $(".left-section").append(`<p><strong>Voucher Files:</strong> No Attachment File</p>`);
                //}

                if (data.attachmentFiles && data.attachmentFiles.length > 0) {
                    const attachmentLinks = data.attachmentFiles.map(file =>
                        `<a href="#" class="attachment-link" data-file="${file.filePath}">${file.filePath.split('_').pop()}</a>`
                    ).join(', ');
                    $(".left-section").append(`<p><strong>Attachment Files:</strong> ${attachmentLinks}</p>`);
                } else {
                    $(".left-section").append(`<p><strong>Attachment Files:</strong> No Attachment File</p>`);
                }

                if (data.voucherFiles && data.voucherFiles.length > 0 && data.voucherFiles.some(file => file.filePath)) {
                    const voucherLinks = data.voucherFiles.filter(file => file.filePath).map(file =>
                        `<a href="#" class="voucher-link" data-file="${file.filePath}">${file.filePath.split('_').pop()}</a>`
                    ).join(', ');
                    $(".left-section").append(`<p><strong>Voucher Files:</strong> ${voucherLinks}</p>`);
                } else {
                    $(".left-section").append(`<p><strong>Voucher Files:</strong> No Attachment File</p>`);
                }

                $(".attachment-link").on('click', function (e) {
                    e.preventDefault();
                    const fileName = $(this).data('file');
                    downloadFile(fileName, 'attachment');
                });

                $(".voucher-link").on('click', function (e) {
                    e.preventDefault();
                    const fileName = $(this).data('file');
                    downloadFile(fileName, 'voucher');
                });

                const timelineContainer = $(".approval-timeline ul");
                timelineContainer.empty();

                const createdStep = `
    <li class="step-created">
        <div class="step-title">Sender</div>
        <div class="step-name">${data.createByName}</div>
        <div class="timestamp">${new Date(data.createAt).toLocaleDateString()}</div>
        <div class="step-status" style="color: #007bff;">Status: Created</div>
    </li>`;
                timelineContainer.append(createdStep);

                data.approveProcesses.forEach((process, index) => {
                    const isSingleItem = data.approveProcesses.length === 1;
                    const isLastItem = index === data.approveProcesses.length - 1;
                    const statusClass = process.approveStatus.toLowerCase();
                    const borderColor = isSingleItem ? "3px solid #f8f9fa" : isLastItem ? "3px solid #f8f9fa" : `2px solid ${statusClass === 'approved' ? 'green' : (statusClass === 'rejected' ? 'red' : 'gray')}`;

                    const listItem = `
    <li class="step-${statusClass} ${isLastItem ? 'current-step' : ''}" style="border-left: ${borderColor};">
        <div class="step-title">${process.userWithRole.roleName}</div>
        <div class="step-name">${process.userWithRole.fullName}</div>
        <div class="timestamp">${process.userWithRole.email}</div>
        <div class="step-status" style="color: ${statusClass === 'approved' ? 'green' : (statusClass === 'rejected' ? 'red' : 'gray')};">
            Status: ${process.approveStatus}
        </div>
    </li>`;
                    timelineContainer.append(listItem);
                });

                if (data.approveProcesses[0] && data.approveProcesses[0].approveStatus.toLowerCase() === 'process' && data.createdBy === userId) {
                    const cancelBtn = `<button id="cancelRequestBtn" class="btn btn-danger">Cancel</button>`;
                    $(".back-button-container").append(cancelBtn);

                    $("#cancelRequestBtn").on('click', function () {
                        if (confirm("Are you sure you want to cancel this request?")) {
                            $.ajax({
                                url: `${apiBaseUrl}/api/RequestForm/cancelrequest/${requestId}`,
                                type: 'GET',
                                headers: {
                                    'Authorization': `Bearer ${jwtToken}`
                                },
                                success: function (response) {
                                    if (response.success) {
                                        alert(response.message);
                                        //location.reload();
                                        window.location.href = '/Payment/PaymentManagement';
                                    } else {
                                        alert(`Failed to cancel: ${response.message}`);
                                    }
                                },
                                error: function () {
                                    alert("Error occurred while trying to cancel the request.");
                                }
                            });
                        }
                    });
                }
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Error retrieving request details.");
        }
    });
});

function safeValue(value, defaultValue = 'N/A') {
    return value ? value : defaultValue;
}
function downloadFile(fileName, fileType) {
    const jwtToken = localStorage.getItem('jwtToken');
    const endpoint = fileType === 'attachment'
        ? `${apiBaseUrl}/api/AttachmentFile/downloadpaymentattachmentfile/${fileName}`
        : `${apiBaseUrl}/api/Voucher/downloadpaymentvoucherfile/${fileName}`;

    $.ajax({
        url: endpoint,
        type: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data, status, xhr) {
            const contentType = xhr.getResponseHeader('Content-Type');
            const blob = new Blob([data], { type: contentType });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName.split('_').pop();
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        },
        error: function () {
            alert(`Error downloading the ${fileType} file.`);
        }
    });
}