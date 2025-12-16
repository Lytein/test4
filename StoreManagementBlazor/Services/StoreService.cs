using StoreManagementBlazorApp.Entities;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace StoreManagementBlazorApp.Services
{
    public class StoreService
    {
        private readonly IJSRuntime? _js;

        public event Action? OnChange;
        private readonly ApiService _api;
       
        public StoreService(ApiService api, IJSRuntime? js = null)
        {
            _api = api;
            _js = js;
        }

        public List<Product> Products { get; private set; } = new();
        public async Task LoadProductsAsync()
        {
            Products = await _api.GetProducts();
        }

        public List<Promotion> Promotions { get; private set; } = new();
        public async Task LoadPromotionsAsync()
        {
            Promotions = await _api.GetPromotions();
        }


        /* =========================
           CART
        ========================= */
        public List<CartItem> Cart { get; private set; } = new();
        public void AddToCart(Product product, int quantity)
        {
            if (quantity <= 0) return;
            var item = Cart.FirstOrDefault(c => c.Product.Id == product.Id);
            if (item == null) Cart.Add(new CartItem { Product = product, Quantity = quantity });
            else item.Quantity += quantity;
            OnChange?.Invoke();
        }

        public void UpdateCartQuantity(int productId, int quantity)
        {
            var item = Cart.FirstOrDefault(c => c.Product.Id == productId);
            if (item == null) return;
            if (quantity <= 0) Cart.Remove(item);
            else item.Quantity = quantity;
            OnChange?.Invoke();
        }

        public void RemoveFromCart(int productId)
        {
            var item = Cart.FirstOrDefault(c => c.Product.Id == productId);
            if (item != null) Cart.Remove(item);
            OnChange?.Invoke();
        }

        public decimal CartTotal => Cart.Sum(c => c.Product.Price * c.Quantity);
        public void ClearCart() => Cart.Clear();

        /* =========================
           CUSTOMER / AUTH STATE
        ========================= */
        public Customer? CurrentCustomer { get; set; }
        public UserDto? CurrentUser { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }

        public bool IsCustomer => Role == "customer";
        public bool IsAdmin => Role == "admin";
        public bool IsStaff => Role == "staff";
        public bool IsLoggedIn => !string.IsNullOrEmpty(Role);

        // Chỉ lưu state nội bộ (không gọi JS)
        public void SetCustomer(Customer customer, string token)
        {
            CurrentCustomer = customer;
            Role = "customer";
            Token = token;
            OnChange?.Invoke();
        }
        public void SetStaff(UserDto userDto, string token)
        {
            CurrentUser = userDto;
            Role = "staff"; 
            Token = token;
            OnChange?.Invoke();
        }

        public void SetAdmin(UserDto userDto, string token)
        {
            CurrentUser = userDto;
            Role = "admin";
            Token = token;
            OnChange?.Invoke();
        }

        public void SetRole(string role, string token)
        {
            Role = role;
            Token = token;
            OnChange?.Invoke();
        }

        public void Logout()
        {
            CurrentCustomer = null;
            Role = null;
            Token = null;
            ClearCart();
            OnChange?.Invoke();
        }


        public async Task LoadAuthFromStorageAsync()
        {
            if (_js == null) return;

            Role = await _js.InvokeAsync<string>("localStorageHelper.getItem", "role");
            Token = await _js.InvokeAsync<string>("localStorageHelper.getItem", "token");

            if (string.IsNullOrEmpty(Role))
                return;

            if (Role == "customer")
            {
                var customerJson = await _js.InvokeAsync<string>(
                    "localStorageHelper.getItem", "customer");

                if (!string.IsNullOrEmpty(customerJson))
                    CurrentCustomer = JsonSerializer.Deserialize<Customer>(customerJson);
            }
            else if (Role == "admin" || Role == "staff")
            {
                var userJson = await _js.InvokeAsync<string>(
                    "localStorageHelper.getItem", Role);

                if (!string.IsNullOrEmpty(userJson))
                    CurrentUser = JsonSerializer.Deserialize<UserDto>(userJson);
            }

            OnChange?.Invoke();
        }


        public async Task SaveToStorageAsync()
        {
            if (_js == null) return;

            if (IsCustomer && CurrentCustomer != null)
            {
                await _js.InvokeVoidAsync(
                    "localStorageHelper.setItem",
                    "customer",
                    JsonSerializer.Serialize(CurrentCustomer)
                );
            }
            else if ((IsAdmin || IsStaff) && CurrentUser != null)
            {
                await _js.InvokeVoidAsync(
                    "localStorageHelper.setItem",
                    Role!, // "admin" hoặc "staff"
                    JsonSerializer.Serialize(CurrentUser)
                );
            }

            await _js.InvokeVoidAsync("localStorageHelper.setItem", "role", Role ?? "");
            await _js.InvokeVoidAsync("localStorageHelper.setItem", "token", Token ?? "");
        }


        public async Task ClearStorageAsync()
        {
            if (_js == null) return;
            await _js.InvokeVoidAsync("localStorageHelper.removeItem", "customer");
            await _js.InvokeVoidAsync("localStorageHelper.removeItem", "admin");
            await _js.InvokeVoidAsync("localStorageHelper.removeItem", "staff");
            await _js.InvokeVoidAsync("localStorageHelper.removeItem", "role");
            await _js.InvokeVoidAsync("localStorageHelper.removeItem", "token");
        }
    }
}
