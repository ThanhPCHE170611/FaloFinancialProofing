function changeCampaign(button, Id, name) {
    localStorage.setItem('campaignId', Id);
    localStorage.setItem('campaignName', name);
    window.location.href = '/Campaign/CampaignDetail';
}
function changeCampaignUpdateProcess(button, Id) {
    localStorage.setItem('campaignId', Id);
    window.location.href = '/Campaign/CampaignUpdateProcessRequest';
}
function changeCampaignPm(button, Id, name) {
    localStorage.setItem('campaignId', Id);
    localStorage.setItem('campaignName', name);
    window.location.href = '/Campaign/CampaignDetail_PMB';
}
$(document).ready(function () {
    let currentPage = 1;
    const userId = localStorage.getItem('userId');
    const roleId = localStorage.getItem('loggingRoleId');
    const checkrole = localStorage.getItem('loggingRole');
    const pageSize = 7;
    if (checkrole === "Project Management Board") {
        document.getElementById('statusFilterStatus').style.display = 'block';
    }
    function fetchCampaignData(searchInput = '', Stage = null, isActive=null, page = 1) {
        const jwtToken = localStorage.getItem('jwtToken');

        let apiUrl;
        if (checkrole === "Project Management Board") {
            apiUrl = `https://localhost:7294/api/Campaign/GetAllCampaignInSystem`
        } else {
            apiUrl = `https://localhost:7294/api/CampaignMember/GetAllCampaignMembersByUserIdAndRoleId`
        }

        $.ajax({
            url: apiUrl,
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            data: {
                searchInput: searchInput,
                userId: userId,
                roleId: roleId,
                status: Stage,
                isActive: isActive,
                currentPage: page
            },
            success: function (response) {
                if (response.success) {
                    console.log(userId);
                    console.log(roleId);
                    renderTable(response.data.data);
                    setupPagination(response.data.dataCount);
                } else {
                    $('#campaignTable tbody').html('<tr><td colspan="5">No data found.</td></tr>');
                }
            },
            error: function (err) {
                $('#campaignTable tbody').html('<tr><td colspan="5">No data found.</td></tr>');
            }
        });
    }

    function renderTable(data) {
        const tableBody = $('#campaignTable tbody');
        tableBody.empty();
        if (data.length === 0) {
            tableBody.append('<tr><td colspan="5">No results found</td></tr>');
            return;
        }

        data.forEach((item, index) => {
            console.log(item.campaignId);
            let actionButtons = `<button onclick="changeCampaign(this, ${item.campaignId}, '${item.campaignTitle}')" class="btn btn-info btn-sm detail-btn">Detail</button>`;
            let campaignName = `${item.campaignTitle}`;
            if (checkrole === "Project Manager") {
                actionButtons = `<button onclick="changeCampaign(this, ${item.campaignId}, '${item.campaignTitle}')" class="btn btn-info btn-sm detail-btn">Detail</button>
                                        <button class="btn btn-primary btn-sm" onclick="changeCampaignUpdateProcess(this, ${item.campaignId})">Update Process</a>
                    `;
            }
            if (checkrole === "Project Manager" && item.status === "Close") {
                actionButtons = `<button onclick="changeCampaign(this, ${item.campaignId}, '${item.campaignTitle}')" class="btn btn-info btn-sm detail-btn">Detail</button>`;
            }
            if (checkrole === "Project Management Board") {
                campaignName = `${item.title}`;
                actionButtons = `<button onclick="changeCampaignPm(this, ${item.campaignId}, '${item.title}')" class="btn btn-info btn-sm detail-btn">Detail</button>
                    `;
            }
            const row = `
                    <tr>
                        <td class="align-middle text-center text-sm">${index + 1}</td>
                        <td class="align-middle text-center text-sm">${campaignName}</td>
                        <td class="align-middle text-center text-sm">${item.projectName}</td>
                        <td class="align-middle text-center text-sm">${item.fundTarget.toLocaleString()}</td>
                        <td class="align-middle text-center text-sm">${item.status}</td>
                        <td class="align-middle text-center">
                            ${actionButtons}
                        </td>
                    </tr>
                `;
            tableBody.append(row);
        });
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
        fetchCampaignData($('#searchInput').val(), $('#statusFilter').val(), $('#statusFilterStatus').val(), currentPage);
    };


    $('#searchInput').on('keyup', function () {
        currentPage = 1;
        fetchCampaignData($(this).val(), $('#statusFilter').val(), $('#statusFilterStatus').val(),currentPage);
    });

    $('#statusFilter').on('change', function () {
        currentPage = 1;
        fetchCampaignData($('#searchInput').val(), $(this).val(), $('#statusFilterStatus').val(),currentPage);
    });
    $('#statusFilterStatus').on('change', function () {
        currentPage = 1;
        fetchCampaignData($('#searchInput').val(), $('#statusFilter').val(), $(this).val(), currentPage);
    });


    fetchCampaignData();
});