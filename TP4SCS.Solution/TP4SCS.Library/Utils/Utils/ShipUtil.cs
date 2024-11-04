using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TP4SCS.Library.Models.Response.Location;

namespace TP4SCS.Library.Utils.Utils
{
    public static class ShipUtil
    {
        private const string ProvinceUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province";
        private const string DistrictUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district";
        private const string WardUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward";
        private const string AvailableServicesUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services";
        private const string ApiToken = "4eda8e6d-9a9f-11ef-8e53-0a00184fe694";
        private const int ShopId = 195216;


        public static async Task<List<Province>> GetProvincesAsync(HttpClient httpClient)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", ApiToken);
            }

            // Gửi request đến API
            var response = await httpClient.GetStringAsync(ProvinceUrl);

            // Parse dữ liệu JSON và lấy danh sách Province
            using var document = JsonDocument.Parse(response);
            var provinces = document.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(province => new Province
                {
                    ProvinceID = province.GetProperty("ProvinceID").GetInt32(),
                    ProvinceName = province.GetProperty("ProvinceName").GetString() ?? string.Empty,
                    NameExtension = province.GetProperty("NameExtension").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToList()
                })
                .ToList();

            return provinces;
        }

        public static async Task<List<District>> GetDistrictsAsync(HttpClient httpClient, int provinceId)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", ApiToken);
            }

            // Gửi request đến API
            var url = DistrictUrl + "?province_id="+provinceId;
            var response = await httpClient.GetStringAsync(url);

            using var document = JsonDocument.Parse(response);
            var provinces = document.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(district => new District
                {
                    ProvinceID = district.GetProperty("ProvinceID").GetInt32(),
                    DistrictID = district.GetProperty("DistrictID").GetInt32(),
                    DistrictName = district.GetProperty("DistrictName").GetString() ?? string.Empty,
                    NameExtension = district.GetProperty("NameExtension").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToList()
                })
                .ToList();

            return provinces;
        }

        public static async Task<List<Ward>> GetWardsAsync(HttpClient httpClient, int districtId)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", ApiToken);
            }

            var requestData = new { district_id = districtId };
            var response = await httpClient.PostAsJsonAsync(WardUrl, requestData);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch wards: {response.ReasonPhrase}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseData);
            var wards = document.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(ward => new Ward
                {
                    WardCode = ward.GetProperty("WardCode").GetString() ?? string.Empty,
                    DistrictID = ward.GetProperty("DistrictID").GetInt32(),
                    WardName = ward.GetProperty("WardName").GetString() ?? string.Empty,
                    NameExtension = ward.GetProperty("NameExtension").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToList()
                })
                .ToList();

            return wards;
        }
        public static async Task<List<AvailableService>> GetAvailableServicesAsync(HttpClient httpClient, int fromDistrict, int toDistrict)
        {

            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", ApiToken);
            }

            var requestBody = new
            {
                shop_id = ShopId,
                from_district = fromDistrict,
                to_district = toDistrict
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(AvailableServicesUrl, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseBody);
            var services = document.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(service => new AvailableService
                {
                    ServiceID = service.GetProperty("service_id").GetInt32(),
                    ShortName = service.GetProperty("short_name").GetString() ?? string.Empty,
                    ServiceTypeID = service.GetProperty("service_type_id").GetInt32()
                })
                .ToList();

            return services;
        }
        public static async Task<decimal> GetShippingFeeAsync(HttpClient httpClient,
            int serviceTypeId,
            int fromDistrictId,
            string fromWardCode,
            int toDistrictId,
            string toWardCode,
            int height,
            int length,
            int weight,
            int width,
            decimal insuranceValue,
            List<Item> items)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", ApiToken);
            }

            var requestBody = new
            {
                service_type_id = serviceTypeId,
                from_district_id = fromDistrictId,
                from_ward_code = fromWardCode,
                to_district_id = toDistrictId,
                to_ward_code = toWardCode,
                height = height,
                length = length,
                weight = weight,
                width = width,
                insurance_value = insuranceValue,
                coupon = (string?)null,
                items = items
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseBody);

            // Giả sử bạn sẽ nhận được total trong response
            var totalFee = document.RootElement
                .GetProperty("data")
                .GetProperty("total");

            return totalFee.GetDecimal();
        }
    }
}
