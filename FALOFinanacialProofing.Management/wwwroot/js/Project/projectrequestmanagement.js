let currentButton = null;
let currentRequestId = null;

function openDeclineModal(button, requestId) {
    currentButton = button;
    currentRequestId = requestId;
    $('#declineModal').fadeIn();
}

function closeModalDeclineProjectRequest() {
    $('#declineModal').fadeOut();
}

function submitDecline() {
    const feedback = $('#declineReason').val().trim();

    if (!feedback) {
        alert('Please enter a reason for the decline.');
        return;
    }

    closeModalDeclineProjectRequest();
    handleProjectReject(currentButton, currentRequestId, false, feedback);
}

function handleProjectReject(button, createProjectRequestId, isAllowed, feedback) {
    const userId = localStorage.getItem('userId');
    const jwtToken = localStorage.getItem('jwtToken');

    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const hours = String(today.getHours()).padStart(2, '0');
    const minutes = String(today.getMinutes()).padStart(2, '0');
    const seconds = String(today.getSeconds()).padStart(2, '0');
    const dateTime = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;

    const payload = {
        createProjectRequestId: createProjectRequestId,
        approverId: userId,
        dateOfApproval: dateTime,
        isAllowed: isAllowed,
        feedBack: feedback
    };

    console.log('Payload:', payload);


    $.ajax({
        url: `${apiBaseUrl}/api/CreateProjectRequestApproveHistories/CreateCreateProjectRequestApproveHistory`,
        type: 'POST',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (response) {
            if (response.success) {
                console.log(createProjectRequestId);
                updateRowToRejected(button, createProjectRequestId);
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error in project approval:', error);
            alert('An error occurred. Please try again.');
        }
    });
}

function updateRowToApprovedForPM(button, id) {
    const row = button.closest('tr');
    const actionCell = row.querySelector('td:nth-child(6)');
    const statusCell = row.querySelector('td:nth-child(5)');

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
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';
    detailLink.href = `/Project/ProjectRequestDetail?requestId=${id}`;

    actionCell.appendChild(approvedText);
    actionCell.appendChild(detailLink);
    statusCell.innerHTML = '';
    statusCell.appendChild(declinedText);
}

function updateRowToRejected(button, id) {
    const row = button.closest('tr');
    const actionCell = row.querySelector('td:nth-child(6)');
    const statusCell = row.querySelector('td:nth-child(5)');

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
    detailLink.classList.add('text-blue');
    detailLink.textContent = 'Detail';
    detailLink.href = `/Project/ProjectRequestDetail?requestId=${id}`;

    actionCell.appendChild(rejectedText);
    actionCell.appendChild(detailLink);
    statusCell.innerHTML = '';
    statusCell.appendChild(declinedText);
}

function filterTable() {
    const searchInput = document.getElementById('searchBox').value.toLowerCase();
    const statusFilter = document.getElementById('statusFilter').value.toLowerCase();
    const table = document.getElementById('projectTable');
    const rows = table.getElementsByTagName('tr');

    for (let i = 1; i < rows.length; i++) {
        const project = rows[i].cells[1].innerText.toLowerCase();
        const address = rows[i].cells[2].innerText.toLowerCase();
        const status = rows[i].cells[4].innerText.toLowerCase();

        let matchesSearch = project.includes(searchInput) || address.includes(searchInput);
        let matchesStatus = statusFilter === "" || status.includes(statusFilter);

        if (matchesSearch && matchesStatus) {
            rows[i].style.display = '';
        } else {
            rows[i].style.display = 'none';
        }
    }
}
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

function handleProjectApproval(button, createProjectRequestId, isAllowed) {
    const userId = localStorage.getItem('userId');

    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const hours = String(today.getHours()).padStart(2, '0');
    const minutes = String(today.getMinutes()).padStart(2, '0');
    const seconds = String(today.getSeconds()).padStart(2, '0');
    const dateTime = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;

    const payload = {
        createProjectRequestId: createProjectRequestId,
        approverId: userId,
        dateOfApproval: dateTime,
        isAllowed: isAllowed
    };

    const jwtToken = localStorage.getItem('jwtToken');
    $.ajax({
        url: `${apiBaseUrl}/api/CreateProjectRequestApproveHistories/CreateCreateProjectRequestApproveHistory`,
        type: 'POST',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (response) {
            if (response.success) {
                console.log(createProjectRequestId);
                updateRowToApprovedForPM(button, createProjectRequestId);
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error in project approval:', error);
            alert('An error occurred. Please try again.');
        }
    });
}

$(document).ready(function () {
    const jwtToken = localStorage.getItem('jwtToken');
    const checkrole = localStorage.getItem('loggingRole');
    const userId = localStorage.getItem('userId');
    let currentPage = 1;
    const pageSize = 7;
    let apiUrl;
    if (checkrole === "Project Management Board") {
        $('.action-buttons').show();
        apiUrl = `${apiBaseUrl}/api/CreateProjectRequests/GetCreateProjectRequestByPMB`;
    } else {
        apiUrl = `${apiBaseUrl}/api/CreateProjectRequests/GetAllCreateProjectRequestsByUserId/${userId}`;
    }

    if (checkrole === 'Project Management Board') {
        document.getElementById('createRequestBtn').style.display = 'none';
    }

    function fetchProjectRequests(searchInput = '', status = null, page = 1) {
        $.ajax({
            url: apiUrl,
            type: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            data: {
                searchInput: searchInput,
                userId: userId,
                status: status,
                currentPage: page
            },
            success: function (response) {
                if (response.success) {
                    console.log("API response data:", response.data.data);
                    populateTable(response.data.data);
                    setupPagination(response.data.dataCount);
                } else {
                    $('#projectTable tbody').append('<tr><td colspan="6" class="text-center">No campaign requests found</td></tr>');
                }
            },
            error: function (xhr, status, error) {
                const tableBody = $('#projectTable tbody');
                tableBody.empty();
                tableBody.append('<tr><td colspan="6" class="text-center">No campaign requests found</td></tr>');
            }
        });
    }
    function checkOrganizations() {
        $.ajax({
            url: `${apiBaseUrl}/api/Organizations/GetOrganizationsByUserId/${userId}`,
            type: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (!response.success || !response.data || response.data.length === 0) {
                    // No organization found
                    alert('You have not created any organizations yet. Redirecting to the organization creation page...');
                    window.location.href = '/Organization/OrganizationCreate';
                } else {
                    window.location.href = '/Project/ProjectCreate_Organization';
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching organizations:', error);
                alert('An error occurred while checking organizations. Please try again later.');
            }
        });
    }
    $('#organizationCreated').on('click', function (e) {
        e.preventDefault();
        checkOrganizations();
    });
    function populateTable(data) {
        const tableBody = $('#projectTable tbody');
        tableBody.empty();

        if (data.length === 0) {
            tableBody.append('<tr><td colspan="6" class="text-center">No campaign requests found</td></tr>');
            return;
        }

        data.forEach((request, index) => {
            const statusClass = getStatusClass(request.status);

            let actionButtons = `
                    <button class="btn btn-success btn-sm" onclick="handleProjectApproval(this,${request.id}, true)">Approve</button>
                    <button class="btn btn-danger btn-sm" onclick="openDeclineModal(this,${request.id})">Decline</button>
                    <a class="btn btn-info btn-sm" href="/Project/ProjectRequestDetail?requestId=${request.id}">Detail</a>
                `;
            if (request.status === "Accepted") {
                actionButtons = `<span class="text-approved me-2" style="margin-left:5px">Approved</span>
                                    <a href="/Project/ProjectRequestDetail?requestId=${request.id}" class="text-blue me-2" >Detail</a>`;
            } else if (request.status === "Rejected") {
                actionButtons = `<span class="text-declined me-2" style="margin-left:5px">Rejected</span>
                                <a href="/Project/ProjectRequestDetail?requestId=${request.id}" class="text-blue me-2">Detail</a>`;
            } else if (request.status === "Cancel") {
                actionButtons = `
                        <a class="btn btn-info btn-sm" href="/Project/ProjectRequestDetail?requestId=${request.id}">Detail</a>
                    `;
            }

            if (checkrole !== "Project Management Board") {
                if (checkrole === "Project Manager") {
                    if (request.status === "Rejected" || request.status === "Pending" || request.status === "Cancel") {
                        actionButtons = `<a class="btn btn-info btn-sm" href="/Project/ProjectRequestDetail?requestId=${request.id}">Detail</a>`;
                    } else {
                        actionButtons = `<a class="btn btn-info btn-sm" href="/Project/ProjectRequestDetail?requestId=${request.id}">Detail</a>`;
                    }
                } else {
                    actionButtons = `<a class="btn btn-info btn-sm" href="/Project/ProjectRequestDetail?requestId=${request.id}">Detail</a>`;
                }

            }
            const row = `
                    <tr>
                        <td class="align-middle text-center text-sm">${index + 1}</td>
                        <td class="align-middle text-center text-sm">${request.title}</td>
                        <td class="align-middle text-center text-sm">${request.senderName}</td>
                        <td class="align-middle text-center text-sm">${formatDateTime(request.createdAt)}</span></td>
                        <td class="align-middle text-center text-sm">
                            <span class="badge badge-sm ${statusClass}">${request.status}</span>
                        </td>
                        <td class="align-middle text-center">${actionButtons}</td>
                    </tr>
                `;

            tableBody.append(row);
        });
    }
    function getStatusClass(status) {
        if (status === "Accepted") return "badge-approved";
        if (status === "Rejected") return "badge-declined";
        if (status === "Cancel") return "bg-gradient-secondary";
        return "badge-pending";
    }
    function setupPagination(totalItemsCount) {
        const totalPages = Math.ceil(totalItemsCount / pageSize);
        const paginationContainer = $('#pagination');
        paginationContainer.empty();

        const maxVisibleButtons = 5;
        const ellipsis = `<span class="btn btn-sm btn-light disabled">...</span>`;

        function createPageButton(page) {
            return $(`<button class="btn btn-sm btn-page ${page === currentPage ? 'btn-primary' : 'btn-light'}">${page}</button>`)
                .on('click', () => changePage(page, totalItemsCount));
        }

        const prevButton = $(`<button class="btn btn-sm btn-page ${currentPage === 1 ? 'btn-light disabled' : 'btn-light'}">Previous</button>`);
        prevButton.on('click', () => {
            if (currentPage > 1) changePage(currentPage - 1, totalItemsCount);
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
            if (currentPage < totalPages) changePage(currentPage + 1, totalItemsCount);
        });
        paginationContainer.append(nextButton);
    }

    window.changePage = function (page, totalItemsCount) {
        const totalPages = Math.ceil(totalItemsCount / pageSize);
        if (page < 1 || page > totalPages) return;
        currentPage = page;
        fetchProjectRequests($('#searchInput').val(), $('#statusFilter').val(), currentPage);
    };

    $('#searchInput').on('keyup', function () {
        currentPage = 1;
        fetchProjectRequests($(this).val(), $('#statusFilter').val(), currentPage);
    });

    $('#statusFilter').on('change', function () {
        currentPage = 1;
        fetchProjectRequests($('#searchInput').val(), $(this).val(), currentPage);
    });

    fetchProjectRequests();
});