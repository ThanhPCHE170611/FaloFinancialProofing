using Newtonsoft.Json;
namespace Client.Manager
{
    public class EntityManager<T>
    {
        private readonly string _baseUrl;

        public EntityManager(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<List<T>?> GetListAsync(string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl + endpoint))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        List<T> items = JsonConvert.DeserializeObject<List<T>>(data);
                        return items;
                    }
                }
            }
        }

        public async Task<T?> GetByIdAsync(string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl + endpoint))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        T item = JsonConvert.DeserializeObject<T>(data);
                        return item;
                    }
                }
            }
        }

        public async Task<T?> UpdateAsync(T? item, string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PutAsJsonAsync(_baseUrl + endpoint, item))
                {
                    string data = await res.Content.ReadAsStringAsync();
                    item = JsonConvert.DeserializeObject<T>(data);
                    return item;
                }
            }
        }

        public async Task<bool> UpdateCheckBoolAsync(T? item, string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PutAsJsonAsync(_baseUrl + endpoint, item))
                {
                    return res.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> InsertAsync(T? item, string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(_baseUrl + endpoint, item))
                {
                    return res.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> DeleteAsync(string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.DeleteAsync(_baseUrl + endpoint))
                {
                    return res.IsSuccessStatusCode;
                }
                    
            }
        }

        public async Task<int> InsertReturnItemAsync(T? item, string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(_baseUrl + endpoint, item))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        int id = JsonConvert.DeserializeObject<int>(data);
                        return id;
                    }                       
                }
            }
        }

        public async Task<T?> GetUserWithGplx(string? endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl + endpoint))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        T item = JsonConvert.DeserializeObject<T>(data);
                        return item;
                    }
                }
            }
        }
    }
}
