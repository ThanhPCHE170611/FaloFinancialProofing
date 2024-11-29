$(document).ready(function () {
    loadCampaigns();
});

// Function to load campaigns with pagination
function loadCampaigns() {
    $.ajax({
        url: `https://localhost:7294/api/Campaign/GetFourCampaignByFilter?IsActive=true&OrderByAscending=true&numOfElements=4`,  // Adjust URL if necessary, e.g., for a specific controller
        type: 'GET',
        data: {
            status: null,
        },
        success: function (response) {
            if (response.success) {
                renderCampaigns(response.data);
            } else {
                alert("Cannot get the campaigns!");
            }
        },
        error: function (error) {
            console.error("Error loading campaigns:", error);
        }
    });
}

// Function to render the campaigns data as cards
function renderCampaigns(data) {
    var campaignsContainer = $('#campaignsContainer');
    campaignsContainer.empty();  // Clear existing data

    // Loop through campaigns only if data.data is a valid array
    data.forEach(function (campaign) {
        var date = new Date(campaign.dateOfCreation);

        // Format the date as "Jan 1, 2024"
        var formattedDate = date.toLocaleDateString('en-US', {
            month: 'short',  // "Jan"
            day: 'numeric',  // "1"
            year: 'numeric'  // "2024"
        });
        const formattedAmountGoal = campaign.fundTarget.toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND'
        });
        const formattedAmountRaise = campaign.totalMoneyEarned.toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND'
        });
        var campaignImage = campaign.image || '/images/event/Project_image.jpg';

        var percentage = Math.round((campaign.totalMoneyEarned / campaign.fundTarget) * 100);
        var percentageDisplay = percentage > 100 ? 100 : percentage;

        var url = '/Campaignpage/Index/' + campaign.campaignId;

        var cardHtml = `<div class="swiper-slide">
                                <div class="cause__item">
                                    <div class="cause__image image">
                                        <img src="${campaignImage}" alt="image">
                                            <span class="cause-tag">${formattedDate}</span>
                                    </div>
                                    <div class="cause__content">
                                        <h4 class="mb-4 mt-20">
                                            <a href="${url}" class="btn-inner">
                                                ${campaign.title}
                                            </a>
                                        </h4>
                                        <div class="progress-area">
                                            <div class="progress__item">
                                                <div class="progress__content" style="width: ${percentageDisplay}%;"><span>${percentage}%</span></div>
                                            </div>
                                            <div class="progress__goal mt-15">
                                                    <h6>Goal : <span>${formattedAmountGoal} VND</span></h6>
                                                    <h6>Raised : <span>${formattedAmountRaise} VND</span></h6>
                                            </div>
                                            <div class="btn-three mt-30">
                                                <span class="btn-circle">
                                                </span>
                                                <a href="${url}" class="btn-inner">
                                                    <span class="btn-text">
                                                        ỦNG HỘ
                                                    </span>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            `;
        campaignsContainer.append(cardHtml);
    });
}

$(document).ready(function () {
    loadProjects();
});



// Function to load projects with pagination
function loadProjects() {
    $.ajax({
        url: `https://localhost:7294/api/Projects/GetFourProjectByFilter?IsActive=true&OrderByAscending=true&numOfElements=4`,  // Adjust URL if necessary, e.g., for a specific controller
        type: 'GET',
        data: {
            status: null,  // You can set the status filter if needed
        },
        success: function (response) {
            if (response.success) {
                renderProjects(response.data);
            } else {
                alert("Cannot get the projects!");
            }
        },
        error: function (error) {
            console.error("Error loading projects:", error);
        }
    });
}

// Function to render the projects data as cards
function renderProjects(data) {
    var projectsContainer = $('#projectsContainer');
    projectsContainer.empty();  // Clear existing data

    data.forEach(function (project) {
        var date = new Date(project.dateOfCreation);

        // Format the date as "Jan 1, 2024"
        var formattedDate = date.toLocaleDateString('en-US', {
            month: 'short',  // "Jan"
            day: 'numeric',  // "1"
            year: 'numeric'  // "2024"
        });

        var url = '/Projectpage/Index/' + project.id;

        var creator = project.firstName + ' ' + project.lastName;

        var shortenedDescription = project.description.length > 150 ? project.description.substring(0, 150) + '...' : project.description;

        var projectImage = project.image || '/images/event/Project_image.jpg';

        var cardHtml = `<div class="col-xl-6 wow fadeInUp" data-wow-delay="200ms" data-wow-duration="1500ms">
                        <div class="event__item h-100">
                            <div class="image h-100">
                                            <img class="h-100" src="${projectImage}" style="height:100%; object-fit:cover; max-width: 283px" alt="image" >
                            </div>
                            <div class="event__content">
                                <ul class="mb-4">
                                    <li>
                                        <svg width="20" height="20" viewBox="0 0 20 20" fill="none"
                                             xmlns="http://www.w3.org/2000/svg">
                                            <path d="M14.5435 5.19275C14.5435 7.69093 12.4989 9.7355 10.0008 9.7355C7.50262 9.7355 5.45804 7.69093 5.45804 5.19275C5.45804 2.69457 7.50258 0.65 10.0008 0.65C12.4989 0.65 14.5435 2.69458 14.5435 5.19275Z"
                                                  stroke="#F74F22" stroke-width="1.3" />
                                            <path d="M18.2644 14.6706C18.1052 14.9458 17.9241 15.2073 17.7169 15.4766L17.7168 15.4765L17.7089 15.4873C17.4203 15.8788 17.0845 16.2373 16.7294 16.5924C16.4326 16.8892 16.0932 17.186 15.7567 17.4385C14.0794 18.6911 12.0621 19.3499 9.97814 19.3499C7.89836 19.3499 5.88506 18.6938 4.20976 17.4461C3.84588 17.1504 3.51367 16.8792 3.22686 16.5924L3.2199 16.5854L3.21272 16.5787C2.85663 16.2436 2.54238 15.8877 2.24745 15.4874L2.24747 15.4873L2.24414 15.4829C2.06192 15.24 1.8732 14.9756 1.71919 14.7169C1.83618 14.4559 1.98455 14.1847 2.14521 13.9526L2.14533 13.9527L2.15284 13.9413C3.06984 12.5556 4.53705 11.6388 6.16642 11.4148L6.186 11.4121L6.20538 11.4082C6.23087 11.4031 6.29494 11.4117 6.34548 11.4496L6.34546 11.4496L6.34947 11.4525C7.41651 12.2401 8.68629 12.6453 10.0008 12.6453C11.3153 12.6453 12.5851 12.2401 13.6521 11.4525L13.6521 11.4525L13.6561 11.4496C13.6715 11.438 13.7403 11.408 13.8492 11.4167C15.4688 11.6435 16.9121 12.5568 17.8524 13.9468L17.8524 13.9469L17.8564 13.9526C18.0165 14.1839 18.1557 14.4231 18.2644 14.6706Z"
                                                  stroke="#F74F22" stroke-width="1.3" />
                                        </svg>
                                        <span>${creator}</span>
                                    </li>
                                    <li>
                                        <svg width="22" height="22" viewBox="0 0 22 22" fill="none"
                                             xmlns="http://www.w3.org/2000/svg">
                                            <path d="M7.33333 5.271C6.9575 5.271 6.64583 4.95933 6.64583 4.5835V1.8335C6.64583 1.45766 6.9575 1.146 7.33333 1.146C7.70917 1.146 8.02083 1.45766 8.02083 1.8335V4.5835C8.02083 4.95933 7.70917 5.271 7.33333 5.271ZM14.6667 5.271C14.2908 5.271 13.9792 4.95933 13.9792 4.5835V1.8335C13.9792 1.45766 14.2908 1.146 14.6667 1.146C15.0425 1.146 15.3542 1.45766 15.3542 1.8335V4.5835C15.3542 4.95933 15.0425 5.271 14.6667 5.271ZM7.79167 13.2918C7.6725 13.2918 7.55333 13.2643 7.44333 13.2185C7.32417 13.1727 7.2325 13.1085 7.14083 13.026C6.97583 12.8518 6.875 12.6227 6.875 12.3752C6.875 12.256 6.9025 12.1368 6.94833 12.0268C6.99417 11.9168 7.05833 11.816 7.14083 11.7243C7.2325 11.6418 7.32417 11.5777 7.44333 11.5318C7.77333 11.3943 8.18583 11.4677 8.4425 11.7243C8.6075 11.8985 8.70833 12.1368 8.70833 12.3752C8.70833 12.4302 8.69917 12.4943 8.69 12.5585C8.68083 12.6135 8.6625 12.6685 8.635 12.7235C8.61667 12.7785 8.58917 12.8335 8.5525 12.8885C8.525 12.9343 8.47917 12.9802 8.4425 13.026C8.26833 13.191 8.03 13.2918 7.79167 13.2918ZM11 13.2918C10.8808 13.2918 10.7617 13.2643 10.6517 13.2185C10.5325 13.1727 10.4408 13.1085 10.3492 13.026C10.1842 12.8518 10.0833 12.6227 10.0833 12.3752C10.0833 12.256 10.1108 12.1368 10.1567 12.0268C10.2025 11.9168 10.2667 11.816 10.3492 11.7243C10.4408 11.6418 10.5325 11.5777 10.6517 11.5318C10.9817 11.3852 11.3942 11.4677 11.6508 11.7243C11.8158 11.8985 11.9167 12.1368 11.9167 12.3752C11.9167 12.4302 11.9075 12.4943 11.8983 12.5585C11.8892 12.6135 11.8708 12.6685 11.8433 12.7235C11.825 12.7785 11.7975 12.8335 11.7608 12.8885C11.7333 12.9343 11.6875 12.9802 11.6508 13.026C11.4767 13.191 11.2383 13.2918 11 13.2918ZM14.2083 13.2918C14.0892 13.2918 13.97 13.2643 13.86 13.2185C13.7408 13.1727 13.6492 13.1085 13.5575 13.026L13.4475 12.8885C13.4128 12.8374 13.385 12.7819 13.365 12.7235C13.3385 12.6716 13.32 12.6159 13.31 12.5585C13.3008 12.4943 13.2917 12.4302 13.2917 12.3752C13.2917 12.1368 13.3925 11.8985 13.5575 11.7243C13.6492 11.6418 13.7408 11.5777 13.86 11.5318C14.1992 11.3852 14.6025 11.4677 14.8592 11.7243C15.0242 11.8985 15.125 12.1368 15.125 12.3752C15.125 12.4302 15.1158 12.4943 15.1067 12.5585C15.0975 12.6135 15.0792 12.6685 15.0517 12.7235C15.0333 12.7785 15.0058 12.8335 14.9692 12.8885C14.9417 12.9343 14.8958 12.9802 14.8592 13.026C14.685 13.191 14.4467 13.2918 14.2083 13.2918ZM7.79167 16.5002C7.6725 16.5002 7.55333 16.4727 7.44333 16.4268C7.33333 16.381 7.2325 16.3168 7.14083 16.2343C6.97583 16.0602 6.875 15.8218 6.875 15.5835C6.875 15.4643 6.9025 15.3452 6.94833 15.2352C6.99417 15.116 7.05833 15.0152 7.14083 14.9327C7.48 14.5935 8.10333 14.5935 8.4425 14.9327C8.6075 15.1068 8.70833 15.3452 8.70833 15.5835C8.70833 15.8218 8.6075 16.0602 8.4425 16.2343C8.26833 16.3993 8.03 16.5002 7.79167 16.5002ZM11 16.5002C10.7617 16.5002 10.5233 16.3993 10.3492 16.2343C10.1842 16.0602 10.0833 15.8218 10.0833 15.5835C10.0833 15.4643 10.1108 15.3452 10.1567 15.2352C10.2025 15.116 10.2667 15.0152 10.3492 14.9327C10.6883 14.5935 11.3117 14.5935 11.6508 14.9327C11.7333 15.0152 11.7975 15.116 11.8433 15.2352C11.8892 15.3452 11.9167 15.4643 11.9167 15.5835C11.9167 15.8218 11.8158 16.0602 11.6508 16.2343C11.4767 16.3993 11.2383 16.5002 11 16.5002ZM14.2083 16.5002C13.97 16.5002 13.7317 16.3993 13.5575 16.2343C13.4727 16.1482 13.4071 16.0451 13.365 15.9318C13.3192 15.8218 13.2917 15.7027 13.2917 15.5835C13.2917 15.4643 13.3192 15.3452 13.365 15.2352C13.4108 15.116 13.475 15.0152 13.5575 14.9327C13.7683 14.7218 14.0892 14.621 14.3825 14.6852C14.4467 14.6943 14.5017 14.7127 14.5567 14.7402C14.6117 14.7585 14.6667 14.786 14.7217 14.8227C14.7675 14.8502 14.8133 14.896 14.8592 14.9327C15.0242 15.1068 15.125 15.3452 15.125 15.5835C15.125 15.8218 15.0242 16.0602 14.8592 16.2343C14.685 16.3993 14.4467 16.5002 14.2083 16.5002ZM18.7917 9.02016H3.20833C2.8325 9.02016 2.52083 8.7085 2.52083 8.33266C2.52083 7.95683 2.8325 7.64516 3.20833 7.64516H18.7917C19.1675 7.64516 19.4792 7.95683 19.4792 8.33266C19.4792 8.7085 19.1675 9.02016 18.7917 9.02016Z"
                                                  fill="#F74F22" />
                                            <path d="M14.6667 20.8543H7.33333C3.9875 20.8543 2.0625 18.9293 2.0625 15.5835V7.79183C2.0625 4.446 3.9875 2.521 7.33333 2.521H14.6667C18.0125 2.521 19.9375 4.446 19.9375 7.79183V15.5835C19.9375 18.9293 18.0125 20.8543 14.6667 20.8543ZM7.33333 3.896C4.71167 3.896 3.4375 5.17016 3.4375 7.79183V15.5835C3.4375 18.2052 4.71167 19.4793 7.33333 19.4793H14.6667C17.2883 19.4793 18.5625 18.2052 18.5625 15.5835V7.79183C18.5625 5.17016 17.2883 3.896 14.6667 3.896H7.33333Z"
                                                  fill="#F74F22" />
                                        </svg>
                                        <span>${formattedDate}</span>
                                    </li>
                                </ul>
                                <h3 class="mb-4">
                                    <a href="${url}" class="primary-hover">
                                            ${project.projectName}
                                    </a>
                                </h3>
                                <p>
                                    ${shortenedDescription}
                                </p>
                                <a href="${url}" class="primary-hover fw-bold mt-4">
                                    KHÁM PHÁ <i class="fa-regular fa-arrow-right ms-2"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                    `;
        projectsContainer.append(cardHtml);
    });
}