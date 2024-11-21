$(document).ready(function () {
        loadTransactions(1, "");

        $('#searchButton').click(function () {
            var searchInput = $('#searchInput').val();
            loadTransactions(1, searchInput);  
        });

        $('#searchInput').keypress(function (e) {
            if (e.which === 13) {  // Enter key
                var searchInput = $('#searchInput').val();
                loadTransactions(1, searchInput);  
            }
        });
    });

    // Function to load transactions
    function loadTransactions(currentPage, searchInput) {
        var userId = '@userId';
        var apiUrl = `https://localhost:7294/api/TransactionLogs/GetUserTransactionLogs/${userId}?searchInput=${searchInput}&currentPage=${currentPage}`;

        $.ajax({
            url: apiUrl,
            type: 'GET',
            success: function (response) {
                if (response && response.data && response.data.data) {
                    renderTransactionLogs(response.data.data);
                    renderPagination(response.data.dataCount, currentPage, searchInput);
                } else {
                    showError("No transaction logs available.");
                }
            },
            error: function () {
                showError("Failed to fetch transaction logs.");
            }
        });
    }

    function renderTransactionLogs(transactions) {
        $('#transactions-table').show();  
        $('#transactionLogs').empty();  

        transactions.forEach(function (transaction) {
            var rowHtml = `<tr>
                        <td>${transaction.tid}</td>
                        <td>${new Date(transaction.transactionDate).toLocaleString()}</td>
                        <td>${transaction.amount}</td>
                        <td>${transaction.description}</td>
                        <td>${transaction.campaignName}</td>
                    </tr>`;
            $('#transactionLogs').append(rowHtml);
        });

        $('#errorMessage').hide(); 

    function renderPagination(totalRecords, currentPage, searchInput) {
        var pageSize = 5;  
        var totalPages = Math.ceil(totalRecords / pageSize);
        var paginationHtml = '';

        if (currentPage > 1) {
            paginationHtml += `<button onclick="loadTransactions(${currentPage - 1}, '${searchInput}')">Previous</button>`;
        }

        for (var i = 1; i <= totalPages; i++) {
            if (i === currentPage) {
                paginationHtml += `<button class="active" disabled>${i}</button>`;
            } else {
                paginationHtml += `<button onclick="loadTransactions(${i}, '${searchInput}')">${i}</button>`;
            }
        }

        if (currentPage < totalPages) {
            paginationHtml += `<button onclick="loadTransactions(${currentPage + 1}, '${searchInput}')">Next</button>`;
        }

        $('#paginationControls').html(paginationHtml); 
    }

    function showError(message) {
        $('#errorMessage').text(message).show();
        $('#transactions-table').hide();  
    }
    }