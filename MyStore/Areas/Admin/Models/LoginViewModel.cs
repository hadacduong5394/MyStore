using System;

namespace MyStore.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string Src_Img { get; set; }
        private LoginViewModel() { }
        public LoginViewModel(byte[] imgArr, string userName, string password, string captcha)
        {
            this.UserName = userName;
            this.Password = password;
            this.Captcha = captcha;
            this.ImageByteArray = imgArr;
            var base64 = Convert.ToBase64String(this.ImageByteArray);
            this.Src_Img = string.Format("data:image/png;base64,{0}", base64);
        }
    }
}