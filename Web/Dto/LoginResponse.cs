namespace Web.Dto {
    public class LoginResponse {
        
        public string LongLivedToken { get; set; }
        public string ShortLivedToken { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}