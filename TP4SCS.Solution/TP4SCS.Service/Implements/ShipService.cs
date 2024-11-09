using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TP4SCS.Library.Models.Request.ShipFee;
using TP4SCS.Library.Models.Response.Location;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ShipService : IShipService
    {
        private readonly IConfiguration _configuration;

        public ShipService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get Available Services
        public async Task<List<AvailableService>?> GetAvailableServicesAsync(HttpClient httpClient, int fromDistrict, int toDistrict)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                var requestBody = new
                {
                    shop_id = _configuration["GHN_API:ShopId"],
                    from_district = fromDistrict,
                    to_district = toDistrict
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(_configuration["GHN_API:AvailableServicesUrl"], content);
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
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string?> GetDistrictNamByIdAsync(HttpClient httpClient, int id)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                // Gửi request đến API
                var url = _configuration["GHN_API:DistrictUrl"];
                var response = await httpClient.GetStringAsync(url);

                using var document = JsonDocument.Parse(response);
                var districtName = document.RootElement
                    .GetProperty("data")
                    .EnumerateArray()
                    .Where(district => district.GetProperty("DistrictID").GetInt32() == id)
                    .Select(district => district.GetProperty("DistrictName").GetString() ?? string.Empty)
                    .SingleOrDefault();

                return districtName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Get District By Province Id
        public async Task<List<District>?> GetDistrictsByProvinceIdAsync(HttpClient httpClient, int provinceId)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                // Gửi request đến API
                var url = _configuration["GHN_API:DistrictUrl"] + "?province_id=" + provinceId;
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
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string?> GetProvinceNameByIdAsync(HttpClient httpClient, int id)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                // Gửi request đến API
                var response = await httpClient.GetStringAsync(_configuration["GHN_API:ProvinceUrl"]);

                // Parse dữ liệu JSON và lấy danh sách Province
                using var document = JsonDocument.Parse(response);
                var provinceName = document.RootElement
                    .GetProperty("data")
                    .EnumerateArray()
                    .Where(province => province.GetProperty("ProvinceID").GetInt32() == id)
                    .Select(province => province.GetProperty("ProvinceName").GetString() ?? string.Empty)
                    .SingleOrDefault();

                return provinceName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Get Provinces
        public async Task<List<Province>?> GetProvincesAsync(HttpClient httpClient)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                // Gửi request đến API
                var response = await httpClient.GetStringAsync(_configuration["GHN_API:ProvinceUrl"]);

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
            catch (Exception)
            {
                return null;
            }
        }

        //Get Shipping Fee
        public async Task<decimal> GetShippingFeeAsync(HttpClient httpClient, GetShipFeeRequest getShipFeeRequest, int quantity)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Token"))
            {
                httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
            }

            int heightPerBox = 15;
            int lengthPerBox = 35;
            int widthPerBox = 25;
            int weightPerBox = 400;

            int widthCount = Math.Min(quantity, 5);
            int lengthCount = Math.Min((quantity + 4) / 5, 5);
            int heightCount = (quantity + 24) / (5 * 5);

            int totalWidth = widthPerBox * widthCount;
            int totalLength = lengthPerBox * lengthCount;
            int totalHeight = heightPerBox * heightCount;
            int totalWeight = weightPerBox * quantity;

            var requestBody = new
            {
                service_type_id = 2,
                from_district_id = getShipFeeRequest.FromDistricId,
                from_ward_code = getShipFeeRequest.FromWardCode,
                to_district_id = getShipFeeRequest.ToDistricId,
                to_ward_code = getShipFeeRequest.ToWardCode,
                height = totalHeight,
                length = totalLength,
                weight = totalWeight,
                width = totalWidth,
                insurance_value = 0,
                coupon = (string?)null
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(_configuration["GHN_API:ShipFeeUrl"], content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseBody);

            var totalFee = document.RootElement
                .GetProperty("data")
                .GetProperty("total");

            return totalFee.GetDecimal();
        }

        public async Task<string?> GetWardNameByWardCodeAsync(HttpClient httpClient, int districtId, string code)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                var requestData = new { district_id = districtId };
                var response = await httpClient.PostAsJsonAsync(_configuration["GHN_API:WardUrl"], requestData);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch wards: {response.ReasonPhrase}");
                }

                var responseData = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseData);
                var wardName = document.RootElement
                    .GetProperty("data")
                    .EnumerateArray()
                    .Where(ward => (ward.GetProperty("WardCode").GetString() ?? string.Empty).Equals(code, StringComparison.OrdinalIgnoreCase))
                    .Select(ward => ward.GetProperty("WardName").GetString() ?? string.Empty)
                    .SingleOrDefault();

                return wardName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Get Ward By District Id
        public async Task<List<Ward>?> GetWardsByDistrictIdAsync(HttpClient httpClient, int districtId)
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Token"))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN_API:ApiToken"]);
                }

                var requestData = new { district_id = districtId };
                var response = await httpClient.PostAsJsonAsync(_configuration["GHN_API:WardUrl"], requestData);

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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
