namespace JWT_Token_Example.Helpers;

public static class UserNotificationBody
{
    public static string UserNotificationMail(string orderNumber, string address)
    {
        return $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Order Confirmation</title>
                <!-- Add your custom styles here -->
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Order Confirmation</h1>
                    </div>
                    <div class='content'>
                        <p class='thank-you'>Thank you for your order!</p>
                        <p class='order-details'>Your order has been successfully placed. Below are the order details:</p>
                        <ul>
                            <li><strong>Order Number:</strong>{ orderNumber }</li>
                            <li><strong>Order Date:</strong> {DateTime.Today}</li>
                            <li><strong>Shipping Address:</strong>{ address }</li>
                        </ul>
                        <p>If you have any questions or need further assistance, please don't hesitate to <a href='mailto:aditya.gonnade@swimlane.com'>contact us</a>.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2023 JAVS. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>
            ";
    }
}