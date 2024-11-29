document.addEventListener("DOMContentLoaded", function () {
        var QRDetails = @Html.Raw(donationDetailsJson); 

        console.log("QRDetails:", QRDetails); 

        var donateButton = document.getElementById('donateButton');
        if (!donateButton) {
            console.error("Donate button not found!");
        }

        donateButton.addEventListener('click', function (e) {
            e.preventDefault();  // Prevent the default link behavior
            console.log("Donate button clicked!"); // Log message when button clicked

            if (!QRDetails.userId) {
                alert('User ID is missing.');
                return; // Exit early if UserId is not present
            }

            const amountInput = document.querySelector('.addAmount-value');
            const amount = parseFloat(amountInput.value);

            // Validate the amount input
            if (isNaN(amount) || amount < 2000) {
                alert('Xin hãy nhập số tiền từ 2000đ trở lên');
                return;
            } else if (amount > 10000000) {
                alert('Xin hãy nhập số tiền dưới 10000000đ trở xuống');
                return; 
            }

            const data = {
                userId: QRDetails.userId,
                amount: amount,
                bankId: QRDetails.bankId,
                campaignId: QRDetails.campaignId
            };

            console.log("Data being sent:", data); // Log data object

            // Make the API call to generate the QR code
            fetch('https://localhost:7294/api/CreateQrCodes/CreateQrCode', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => {
                    console.log('API Response:', response);  // Log API response
                    return response.blob(); // Expecting image (blob)
                })
                .then(blob => {
                    console.log('Received blob:', blob);  // Log received blob
                    const reader = new FileReader();
                    reader.onloadend = function () {
                        const base64Image = reader.result;
                        console.log('Base64 Image:', base64Image);  // Log base64 image string

                        // Show the image in a popup
                        showQRCodePopUp(base64Image);
                    };
                    reader.readAsDataURL(blob);  // Convert blob to base64
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while generating the QR code.');
                });
        });
    });

    // Function to show the QR code popup
    function showQRCodePopUp(imageData) {
        const modal = document.getElementById("myModal");
        const qrCodeImage = document.getElementById("qrCodeImage");

        if (imageData && imageData.startsWith('data:image')) {
            qrCodeImage.src = imageData;  // Set the image source to the received base64 image data
        } else {
            console.error('Invalid image data:', imageData);  // Log error if the data is invalid
            return;
        }

        // Display the modal
        modal.style.display = "block";

        // Close the modal when the user clicks on <span> (x)
        const closeButton = document.getElementsByClassName("close")[0];
        closeButton.onclick = function () {
            modal.style.display = "none"; // Close the modal
        }

        // Close the modal when the user clicks anywhere outside the modal
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    }

    //Sync Transaction
    document.addEventListener("DOMContentLoaded", function () {
        var donateButton = document.getElementById('syncButton');
        var QRDetails = @Html.Raw(donationDetailsJson);

        console.log("QRDetails:", QRDetails);

        if (!donateButton) {
            console.error("Sync button not found!");
        }

        donateButton.addEventListener('click', function (e) {
            e.preventDefault(); // Prevent the default link behavior

            // Assuming you have QRDetails available from your previous code
            var bankId = QRDetails.bankId;  // Assuming QRDetails is available from your previous code

            if (!bankId) {
                alert('Bank ID is missing.');
                return;
            }

            // Step 1: Fetch bank details using the first API
            fetch(`https://localhost:7294/api/Banks/GetBankById/${bankId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Failed to fetch bank details');
                    }
                    return response.json();  // Parse the response as JSON
                })
                .then(bankDetails => {
                    console.log("Bank details:", bankDetails);

                    // Extract account number from bank details
                    var accountNumber = bankDetails.accountNumber;

                    if (!accountNumber) {
                        alert('Account number not found.');
                        return;
                    }

                    // Step 2: Call the second API using the account number

                    fetch(`https://localhost:7294/api/WebHooks/SyncTransaction/${accountNumber}`, {
                        method: 'POST',  // POST request
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(response => {
                            if (response.status === 204) {
                                // No content in response, but the request was successful
                                console.log("Transaction successfully synced with no content in response.");
                                alert('Transaction successfully synced!');
                            } else {
                                // Handle other non-204 responses (if any)
                                throw new Error(`Unexpected response status: ${response.status}`);
                            }
                        })
                        .catch(error => {
                            console.error('Sync Transaction Error:', error);
                            alert('An error occurred while syncing the transaction.');
                        });
                })
                .catch(error => {
                    console.error('GetBankById Error:', error);
                    alert('An error occurred while fetching bank details.');
                });
        });
    });