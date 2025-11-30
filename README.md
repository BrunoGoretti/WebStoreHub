# WebStoreHub 
WebStoreHub is a full-stack e-commerce web application built with C# (.NET) backend and Angular frontend. It offers a complete shopping experience including user management, product catalog, cart and orders, discounts, and image uploads.
![Eshop](https://github.com/user-attachments/assets/30c23285-1017-4533-aaee-3a8939845533)

# Features
# User Account Management
* User registration with validation
* Secure login with JWT authentication
* Password reset via email link
* Profile view and update

# Product Browsing & Searching
* Browse products by category and brand
* Search products by name or description
* Detailed product pages with images, pricing, and discounts

# Wishlist
* Add or remove products from wishlist
* View wishlist with live product info

# Shopping Cart
* Add/update/remove products with quantity control
* View cart total including discounts
* Checkout and place orders

# Orders
* View past orders and status
* Download order confirmation as PDF

# Admin Features

* Manage products, brands, and categories (product types)
* Upload images for categories and products
* Import and manage discounts via Excel files
* Update order statuses

## Installation

Clone repository in Visual Studio.

Put your server name in DefaultConnection

![asdsdaasd](https://github.com/user-attachments/assets/7f002da3-2ded-47cf-999b-382bfdae65a5)

Create/Edit appsetings.json in C# backend

{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string here"
  },
  "Jwt": {
    "Key": "Your_JWT_Secret_Key",
    "Issuer": "Your_Issuer",
    "Audience": "Your_Audience"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "EmailAddress": "your-email@gmail.com",
    "EmailPassword": "your-app-specific-password"
  },
  "ImgbbSettings": {
    "ApiKey": "your-imgbb-api-key",
    "UploadUrl": "https://api.imgbb.com/1/upload"
  }
}

ConnectionStrings → DefaultConnection:
Put your database server and database name.

Jwt:
Add your secret key, issuer, and audience for authentication.

EmailSettings:
Add your Gmail address and app-specific password here (get the app password from your Google Account security settings).

ImgbbSettings:
Add your imgbb API key and upload URL if you use imgbb for image hosting.

<img width="1098" height="437" alt="image" src="https://github.com/user-attachments/assets/777c45de-281c-4ed6-84e0-60d7a7263a7a" />

Create migrations for SQL.

![sadaewrds](https://github.com/user-attachments/assets/91c6723b-2ba0-4811-8667-ac19ef895160)

Update migrations

![sadae32wrds](https://github.com/user-attachments/assets/ccc8d4e0-4e64-40db-b637-12a278052ebf)

## Frontend Setup

1. Clone the repository
2. Navigate to the `frontend` directory
3. Run `npm install` to install all dependencies
4. Run `ng serve` to start the development server
5. Open `http://localhost:4200` in your browser

Now you can launch and use!:)
