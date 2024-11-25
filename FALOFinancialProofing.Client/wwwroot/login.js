function login(username, password) {
    return $.ajax({
        url: "https://localhost:7294/api/Users/Login", // Adjust the URL as needed
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ Username: username, Password: password })
    });
}