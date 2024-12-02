const jwtToken = localStorage.getItem('jwtToken');
const checkrole = localStorage.getItem('loggingRole');
const userId = localStorage.getItem('userId');
let currentPage = 1;
const pageSize = 7;

$(document).ready(function () {
    loadProjects();
});

function loadProjects() {
    const searchInput = $('#searchInput').val().trim();
    const statusFilter = $('#statusFilter').val();
    let apiUrl;

    if (checkrole === "Project Manager") {
        apiUrl = `https://localhost:7294/api/Projects/GetAllProjectByUserId/${userId}?searchInput=${searchInput}&status=${statusFilter}&currentPage=${currentPage}`;
    } else if (checkrole === "Project Management Board") {
        apiUrl = `https://localhost:7294/api/Projects/GetAllProjectInSystem?searchInput=${searchInput}&status=${statusFilter}&currentPage=${currentPage}&PageSizeCustom=7`;
    } else {
        alert('Unauthorized role. Unable to fetch projects.');
        return;
    }

    $.ajax({
        url: apiUrl,
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (response) {
            if (response.success && response.data.data.length > 0) {
                renderTable(response.data.data);
                setupPagination(response.data.dataCount);
            } else {
                $('#projectTable tbody').html('<tr><td colspan="7">No projects found.</td></tr>');
            }
        },
        error: function () {
            alert('Error fetching projects');
        }
    });
}

function renderTable(projects) {
    const projectTableBody = $('#projectTable tbody');
    projectTableBody.empty();

    projects.forEach((project, index) => {
        const statusBadge = project.status === 'Running'
            ? `<span class="badge badge-approved">Running</span>`
            : `<span class="badge badge-declined">Close</span>`;

        let actionButtons = `
                    <button onclick="changeProject(this, ${project.id})" class="btn btn-info btn-sm">Detail</button>

                `;
        if (checkrole !== "Project Management Board") {
            if (checkrole === "Project Manager") {
                actionButtons = `
                                    <button onclick="changeProject(this, ${project.id})" class="btn btn-info btn-sm">Detail</button>
                                    <a class="btn btn-primary btn-sm" href="/Campaign/CampaignCreate?projectId=${project.id}">Campaign</a>`;
            }
            if (project.status === 'Close') {
                actionButtons = `
                                    <button onclick="changeProject(this, ${project.id})" class="btn btn-info btn-sm">Detail</button>`;
            }
        }

        const row = `
                    <tr>
                        <td class="align-middle text-center text-sm">${index + 1 + (currentPage - 1) * pageSize}</td>
                        <td class="align-middle text-center text-sm">${project.projectName}</td>
                        <td class="align-middle text-center text-sm">${project.lastName} ${project.firstName}</td>
                        <td class="align-middle text-center text-sm">${new Date(project.dateOfCreation).toLocaleDateString()}</td>
                        <td class="align-middle text-center text-sm">${statusBadge}</td>
                        <td class="align-middle text-center text-sm">
                            ${actionButtons}
                        </td>
                    </tr>
                `;
        projectTableBody.append(row);
    });
}

// function setupPagination(totalCount) {
//     const totalPages = Math.ceil(totalCount / pageSize);
//     const paginationContainer = $('#pagination');
//     paginationContainer.empty();

//     for (let i = 1; i <= totalPages; i++) {
//         const pageButton = `<button class="btn btn-sm btn-page ${i === currentPage ? 'btn-primary' : 'btn-light'}" onclick="goToPage(${i})">${i}</button>`;
//         paginationContainer.append(pageButton);
//     }
// }
function setupPagination(totalRecords) {
    const totalPages = Math.ceil(totalRecords / pageSize);
    const paginationContainer = $('#pagination');
    paginationContainer.empty();

    const maxVisibleButtons = 5; 
    const ellipsis = `<span class="btn btn-sm btn-light disabled">...</span>`;

    if (currentPage > 1) {
        const prevButton = `<button class="btn btn-sm btn-page btn-light" onclick="goToPage(${currentPage - 1}, ${totalRecords})">Previous</button>`;
        paginationContainer.append(prevButton);
    } else {
        const prevButton = `<button class="btn btn-sm btn-page btn-light disabled">Previous</button>`;
        paginationContainer.append(prevButton);
    }

    let startPage = Math.max(1, currentPage - Math.floor(maxVisibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxVisibleButtons - 1);

    if (endPage - startPage + 1 < maxVisibleButtons) {
        startPage = Math.max(1, endPage - maxVisibleButtons + 1);
    }

    if (startPage > 1) {
        paginationContainer.append(`<button class="btn btn-sm btn-page btn-light" onclick="goToPage(1, ${totalRecords})">1</button>`);
        if (startPage > 2) {
            paginationContainer.append(ellipsis);
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        const pageButton = `<button class="btn btn-sm btn-page ${i === currentPage ? 'btn-primary' : 'btn-light'}" onclick="goToPage(${i}, ${totalRecords})">${i}</button>`;
        paginationContainer.append(pageButton);
    }

    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            paginationContainer.append(ellipsis);
        }
        paginationContainer.append(`<button class="btn btn-sm btn-page btn-light" onclick="goToPage(${totalPages}, ${totalRecords})">${totalPages}</button>`);
    }

    if (currentPage < totalPages) {
        const nextButton = `<button class="btn btn-sm btn-page btn-light" onclick="goToPage(${currentPage + 1}, ${totalRecords})">Next</button>`;
        paginationContainer.append(nextButton);
    } else {
        const nextButton = `<button class="btn btn-sm btn-page btn-light disabled">Next</button>`;
        paginationContainer.append(nextButton);
    }
}

function goToPage(page, totalRecords) {
    const totalPages = Math.ceil(totalRecords / pageSize);
    if (page < 1 || page > totalPages) return;
    currentPage = page;
    loadProjects(); 
}




$('#searchInput').on('keyup', function () {
    currentPage = 1;
    loadProjects();
});

$('#statusFilter').on('change', function () {
    currentPage = 1;
    loadProjects();
});
// function goToPage(page) {
//     currentPage = page;
//     loadProjects();
// }

function changeProject(button, Id) {
    localStorage.setItem('projectId', Id);
    window.location.href = '/Project/ProjectDetail';
}