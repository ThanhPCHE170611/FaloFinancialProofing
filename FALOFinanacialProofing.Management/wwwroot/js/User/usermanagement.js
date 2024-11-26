var win = navigator.platform.indexOf('Win') > -1;
if (win && document.querySelector('#sidenav-scrollbar')) {
    var options = {
        damping: '0.5'
    }
    Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
}
const jwtToken = localStorage.getItem('jwtToken');
let currentPage = 1;
const pageSize = 5
$(document).ready(function () {
    loadAccounts();
});
function changeUserId(button, Id) {
    localStorage.setItem('accountId', Id);
    window.location.href = '/User/UserDetail';
}

function goToPage(page) {
    currentPage = page;
    loadAccounts();
}
function loadAccounts() {
    const searchInput = $('#searchInput').val().trim();
    $.ajax({
        url: `https://localhost:7294/api/Users/GetAccountList?searchInput=${searchInput}&currentPage=${currentPage}`,
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success && response.data.data.length > 0) {
                renderTable(response.data.data);
                setupPagination(response.data.dataCount);
            } else {
                $('#userTable').html('<tr><td colspan="5">No accounts found.</td></tr>');
            }
        },
        error: function () {
            alert('Error fetching account list');
        }
    });
}
function renderTable(accounts) {
    const userTableBody = $('#userTable');
    userTableBody.empty();

    accounts.forEach((account, index) => {
        const roles = account.roles.map(role => role.roleName).join(', ');

        const row = `
                        <tr>
                            <td class="align-middle text-center text-sm">${index + 1 + (currentPage - 1) * pageSize}</td>
                            <td class="align-middle text-center text-sm">${account.lastName} ${account.firstName}</td>
                            <td class="align-middle text-center text-sm">${roles}</td>
                            <td class="align-middle text-center text-sm">${account.email}</td>
                            <td class="align-middle text-center text-sm">
                                <button onclick="changeUserId(this, '${String(account.id)}')" class="btn btn-info btn-sm detail-btn">Detail</button>
                            </td>
                        </tr>
                    `;
        userTableBody.append(row);
    });
}

function setupPagination(totalCount) {
    const totalPages = Math.ceil(totalCount / pageSize);
    const paginationContainer = $('#pagination');
    paginationContainer.empty();

    for (let i = 1; i <= totalPages; i++) {
        const pageButton = `<button class="btn btn-sm btn-page ${i === currentPage ? 'btn-primary' : 'btn-light'}" onclick="goToPage(${i})">${i}</button>`;
        paginationContainer.append(pageButton);
    }
}

$('#searchInput').on('keyup', function () {
    currentPage = 1;
    loadAccounts();
});