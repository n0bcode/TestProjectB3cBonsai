using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace TestB3cBonsai
{
    public class TestLogins
    {
        public class LoginTest : IDisposable
        {
            private readonly IWebDriver _driver;

            public LoginTest()
            {
                // Khởi tạo ChromeDriver
                _driver = new EdgeDriver();
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

            [Fact]
            public void TestLogin_ValidCredentials_ShouldRedirectToHomePage()
            {
                // Điều hướng đến trang đăng nhập
                _driver.Navigate().GoToUrl("https://localhost:7020/Identity/Account/Login?viewAccess=CustomerAccess");

                // Tìm các phần tử form đăng nhập
                var emailField = _driver.FindElement(By.Name("Input.Email"));
                var passwordField = _driver.FindElement(By.Name("Input.Password"));
                var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

                // Nhập thông tin hợp lệ
                emailField.SendKeys("admin@dotnetmastery.com");
                passwordField.SendKeys("Admin123*@");
                loginButton.Click();

                // Kiểm tra điều hướng sau khi đăng nhập
                Assert.True(new string[]{"https://localhost:7020", "https://localhost:7020/Employee/Dashboard"}.Contains(_driver.Url), $"Unexpected URL: {_driver.Url}");
            }

            [Fact]
            public void TestLogin_InvalidCredentials_ShouldShowErrorMessage()
            {
                // Điều hướng đến trang đăng nhập
                _driver.Navigate().GoToUrl("https://localhost:7020/Identity/Account/Login?viewAccess=CustomerAccess");

                // Tìm các phần tử form đăng nhập
                var emailField = _driver.FindElement(By.Name("Input.Email"));
                var passwordField = _driver.FindElement(By.Name("Input.Password"));
                var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

                // Nhập thông tin không hợp lệ
                emailField.SendKeys("invaliduser@example.com");
                passwordField.SendKeys("WrongPassword123");
                loginButton.Click();

                // Kiểm tra thông báo lỗi hiển thị
                var errorMessage = _driver.FindElement(By.CssSelector(".alert.alert-danger"));
                Assert.True(errorMessage.Displayed, "Error message not displayed for invalid login.");
            }

            public void Dispose()
            {
                // Đóng trình duyệt
                _driver.Quit();
            }
        }
    }
}