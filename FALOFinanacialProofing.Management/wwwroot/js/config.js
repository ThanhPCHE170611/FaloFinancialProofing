//const apiConfig = {
//    baseUrl: '${apiBaseUrl}'
//};

let apiBaseUrl = '';

$.getJSON('/appsettings.json')
    .then(config => {
        apiBaseUrl = config.ApiBaseUrl;
        console.log("API Base URL:", apiBaseUrl);
    })
    .catch(err => {
        console.error('Lỗi khi tải appsettings.json:', err);
    });