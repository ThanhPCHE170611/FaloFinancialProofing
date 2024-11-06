
let currentDeclineButton = null;
function showDeclinePopup(button) {
    currentDeclineButton = button;
    document.getElementById('declineModal').style.display = 'block';
}

function submitDecline() {
    if (!currentDeclineButton) return;

    const reason = document.getElementById('declineReason').value;
    if (!reason.trim()) {
        alert('Please enter a reason for decline.');
        return;
    }

    const row = currentDeclineButton.closest('tr');
    const actionCell = row.querySelector('td:nth-child(9)');
    currentDeclineButton.style.display = 'none';
    row.querySelector('.btn-success').style.display = 'none';

    const detailButton = actionCell.querySelector('.btn-info');
    const declinedText = document.createElement('span');
    declinedText.classList.add('text-declined', 'me-2');
    declinedText.textContent = 'Declined';
    actionCell.insertBefore(declinedText, detailButton);

    row.querySelector('td:nth-child(7)').innerHTML = '<span class="text-declined text-xs font-weight-bold">Declined</span>';
    row.dataset.declineReason = reason;

    document.getElementById('declineReason').value = '';
    closeModal();
}

function showDetail(button) {
    const row = button.closest('tr');
    const stt = row.cells[0].innerText;
    const sender = row.cells[1].innerText;
    const money = row.cells[2].innerText;
    const assignFrom = row.cells[3].innerText;
    const file = row.cells[4].innerText;
    const voucher = row.cells[5].innerText;
    const status = row.cells[6].innerText;
    const date = row.cells[7].innerText;

    let detailContent = `
                <p><strong>STT:</strong> ${stt}</p>
                <p><strong>Sender:</strong> ${sender}</p>
                <p><strong>Expected Money:</strong> ${money}</p>
                <p><strong>Assign From:</strong> ${assignFrom}</p>
                <p><strong>File:</strong> <a href="#" download="${file}">${file}</a></p>
                <p><strong>Voucher:</strong> <a href="#" download="${voucher}">${voucher}</a></p>
                <p><strong>Status:</strong> ${status}</p>
                <p><strong>Cteated Date:</strong> ${date}</p>
            `;

    const declineReason = row.dataset.declineReason;
    if (declineReason) {
        detailContent += `<p><strong>Reason for Decline:</strong> ${declineReason}</p>`;
    }

    document.getElementById('voucherDetailContent').innerHTML = detailContent;
    document.getElementById('detailModal').style.display = 'block';
}

function closeModal() {
    document.getElementById('declineModal').style.display = 'none';
    document.getElementById('detailModal').style.display = 'none';
}

window.onclick = function (event) {
    const declineModal = document.getElementById('declineModal');
    const detailModal = document.getElementById('detailModal');
    if (event.target === declineModal) {
        declineModal.style.display = 'none';
    }
    if (event.target === detailModal) {
        detailModal.style.display = 'none';
    }
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

$(document).ready(function () {
    const userId = localStorage.getItem('userId');
    const campaignId = localStorage.getItem('campaignId');
    const jwtToken = localStorage.getItem('jwtToken');
    let currentPage = 1;
    const pageSize = 5;

    console.log(campaignId);

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

    function loadPrepayRequests(page) {
        $.ajax({
            url: `https://localhost:7294/api/RequestForm/getallprepayrequestincampaign/${campaignId}?userId=${userId}&page=${page}`,
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
                                "bg-gradient-warning";
                        const statusLabel = `<span class="badge badge-sm ${statusClass}">${request.status}</span>`;

                        let actionButtons = `<a class="btn btn-info btn-sm" href="/Prepay/PrepayDetail_PM?requestId=${request.id}">Detail</a>`;

                        let attachmentLinks = '';
                        if (request.attachmentFiles && request.attachmentFiles.length > 0) {
                            attachmentLinks = request.attachmentFiles.map(file => `
                                <a href="javascript:void(0);" onclick="downloadAttachment('${file.filePath}')" class="btn btn-link text-info">${file.filePath}</a>
                            `).join('<br>');
                        } else {
                            attachmentLinks = '<span class="text-muted">No Attachments</span>';
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
                                        <td class="align-middle text-center text-sm"><span class="text-secondary text-xs font-weight-bold">${request.expectedMoney}</span></td>
                                        <td class="align-middle text-center text-sm"><span class="text-secondary text-xs font-weight-bold">${request.description}</span></td>
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

    function setupPagination(totalRecords, currentPage) {
        const totalPages = Math.ceil(totalRecords / pageSize);
        const paginationContainer = $('#pagination');
        paginationContainer.empty();
        for (let i = 1; i <= totalPages; i++) {
            const pageButton = $(`<button class="btn btn-sm btn-page ${i === currentPage ? 'btn-primary' : 'btn-light'}">${i}</button>`);
            pageButton.on('click', () => changePage(i));
            paginationContainer.append(pageButton);
        }
    }

    window.changePage = function (page) {
        currentPage = page;
        loadPrepayRequests(page);
    };

    loadPrepayRequests(currentPage);
});
