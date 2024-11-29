const API_KEY =
    "AK_CS.5e3f98c0987011ef98eebd0598ac83dd.zKntSv5qWnmjiDZoy2xJJCRVs2g7Sqdk3XM4XvbfhPAla72SPbShdfMiSoEaIKYJQQBBvpMz";
const API_GET = "https://oauth.casso.vn/v2/transactions";

    async function checkPaid() {
        const response = await fetch(API_GET, {
            headers: {
                Authorization: 'Apikey ' +  API_KEY,
                "Content-Type": "application/json"
            },
        });
        const data = await response.json();
        console.log(data);
}

checkPaid();
