function register(username, password, email) {
    return $.ajax({
        url: "https://localhost:7294/api/Users/Register", // Adjust the URL as needed
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ Username: username, Password: password, Email: email })
    });
}