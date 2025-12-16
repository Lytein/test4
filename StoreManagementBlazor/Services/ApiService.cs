using StoreManagementBlazorApp.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace StoreManagementBlazorApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly TokenState _token;

        public ApiService(HttpClient http, TokenState token)
        {
            _http = http;
            _token = token;
        }
        /* Categories
         */


        /* Product*/
        public async Task<List<Product>> GetProducts()
        {

            return await _http.GetFromJsonAsync<List<Product>>("api/product")
                ?? new List<Product>();
        }

        public async Task<Product?> AddProductAsync(Product product)
        {
            var response = await _http.PostAsJsonAsync("api/customer", product);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Product>()
                : null;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var response = await _http.PutAsJsonAsync($"api/product/{product.Id}", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/product/{id}");
            return response.IsSuccessStatusCode;
        }

        /* =====================
           AUTH / LOGIN
        ===================== */
        public async Task<AuthResult?> LoginAsync(string username, string password)
        {
            // ---- ADMIN / STAFF ----
            var userRes = await _http.PostAsJsonAsync("api/user/login", new
            {
                Username = username,
                Password = password
            });

            if (userRes.IsSuccessStatusCode)
            {
                var data = await userRes.Content.ReadFromJsonAsync<UserLoginResponse>();

                SaveToken(data!.token, data.role);
                return new AuthResult
                {
                    Token = data.token,
                    Role = data.role,
                    User = new UserDto
                    {
                        User_Id = data.user_id,
                        Username = data.username,
                        Full_Name = data.full_name,
                        Role = data.role,
                        Created_At = data.created_at
                    }
                };
            }

            // ---- CUSTOMER ----
            var cusRes = await _http.PostAsJsonAsync("api/customer-auth/login", new
            {
                username,
                password
            });

            if (!cusRes.IsSuccessStatusCode)
                return null;

            var cusData = await cusRes.Content.ReadFromJsonAsync<CustomerResponse>();

            SaveToken(cusData!.token, "customer");

            return new AuthResult
            {
                Token = cusData.token,
                Role = "customer",
                Customer = cusData.customer
            };
        }

        private void SaveToken(string token, string role)
        {
            _token.Token = token;
            _token.Role = role;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        /* =====================
           CUSTOMER REGISTER
        ===================== */
        public async Task<bool> RegisterAsync(
            string fullName,
            string username,
            string password,
            string email,
            string phone,
            string address)
            {
                var res = await _http.PostAsJsonAsync(
                    "api/customer-auth/register",
                    new
                    {
                        full_name = fullName,
                        username,
                        password,
                        email,
                        phone,
                        address
                    });

                return res.IsSuccessStatusCode;
            }


        /* =====================
           USERS
        ===================== */
        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<UserDto>>("api/user")
                    ?? new List<UserDto>();
            }
            catch
            {
                return new List<UserDto>();
            }
        }

        /* =====================
           CUSTOMERS
        ===================== */
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _http.GetFromJsonAsync<List<Customer>>("api/customer")
                ?? new List<Customer>();
        }

        public async Task<Customer?> AddCustomerAsync(Customer customer)
        {
            var response = await _http.PostAsJsonAsync("api/customer", customer);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Customer>()
                : null;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var response = await _http.PutAsJsonAsync($"api/customer/{customer.Id}", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/customer/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                return await _http.GetFromJsonAsync<Customer>($"api/customer/{customerId}");
            }
            catch
            {
                return null;
            }
        }
        /* =====================
           PROMOTION
        ===================== */
        public async Task<List<Promotion>> GetPromotions()
        {

            return await _http.GetFromJsonAsync<List<Promotion>>("api/promotion")
                ?? new List<Promotion>();
        }

        public async Task<Promotion?> AddPromotionAsync(Promotion promotion)
        {
            var response = await _http.PostAsJsonAsync("api/customer", promotion);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Promotion>()
                : null;
        }

        public async Task<bool> UpdatePromotionAsync(Promotion promotion)
        {
            var response = await _http.PutAsJsonAsync($"api/promotion/{promotion.PromoId}", promotion);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/promotion/{id}");
            return response.IsSuccessStatusCode;
        }


        /* =====================
           GENERIC
        ===================== */
        public async Task<List<T>> GetListAsync<T>(string route)
        {
            return await _http.GetFromJsonAsync<List<T>>(route) ?? new List<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string route, TRequest obj)
        {
            var response = await _http.PostAsJsonAsync(route, obj);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TResponse>()
                : default;
        }

        public async Task<bool> PutAsync<T>(string route, T obj)
        {
            var response = await _http.PutAsJsonAsync(route, obj);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string route)
        {
            var response = await _http.DeleteAsync(route);
            return response.IsSuccessStatusCode;
        }

        /* =====================
           CATEGORIES
        ===================== */
        public async Task<List<Category>> GetCategoriesAsync()
        {
            // Backend: GET api/category
            return await _http.GetFromJsonAsync<List<Category>>("api/category")
                ?? new List<Category>();
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            // Backend: POST api/category
            var response = await _http.PostAsJsonAsync("api/category", category);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            // Backend: PUT api/category/{id}
            var response = await _http.PutAsJsonAsync($"api/category/{category.category_id}", category);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            // Backend: DELETE api/category/{id}
            var response = await _http.DeleteAsync($"api/category/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    /* =====================
       LOGIN RESPONSE DTOs
    ===================== */
    public class UserLoginResponse
    {
        public string token { get; set; } = "";
        public int user_id { get; set; }
        public string username { get; set; } = "";
        public string full_name { get; set; } = "";
        public string role { get; set; }
        public DateTime created_at { get; set; }
    }

    public class CustomerResponse
    {
        public string token { get; set; } = "";
        public Customer customer { get; set; } = null!;
    }
    
    }
