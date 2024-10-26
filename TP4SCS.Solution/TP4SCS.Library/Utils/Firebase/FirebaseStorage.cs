using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Net.Http.Headers;

namespace TP4SCS.Library.Utils.Firebase
{
    public class FirebaseStorage
    {
        private readonly HttpClient _httpClient;

        public FirebaseStorage(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FirebaseApp> InitializeFirebaseApp()
        {
            // Firebase Storage URL for the JSON file (replace with your project details)
            var storageUrl = "https://firebasestorage.googleapis.com/v0/b/<your-project-id>.appspot.com/o/keys%2FserviceAccountKey.json?alt=media";

            // Add Authorization if needed
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, storageUrl);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "<YOUR_FIREBASE_AUTH_TOKEN>");

            var response = await _httpClient.SendAsync(requestMessage);
            var jsonKey = await response.Content.ReadAsStringAsync();

            // Initialize Firebase with downloaded JSON key
            var firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(jsonKey)
            });

            return firebaseApp;
        }
    }
}
