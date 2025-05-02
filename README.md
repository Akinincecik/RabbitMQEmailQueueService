# RabbitMQ Email Queue Service

## 🇹🇷 Proje Açıklaması (Turkish)

Bu servis, veritabanındaki e-posta gönderim işlemlerini kontrol ederek, uygun verileri **RabbitMQ** kuyruğuna göndermek amacıyla geliştirilmiştir. Böylece e-posta gönderimi ile ilgili görevler, başka bir servis (örneğin: `EmailSenderService`) tarafından asenkron bir şekilde işlenebilir hale gelir. Amaç, sistemi kilitlemeden veri akışını yönlendirmek ve bu işlemleri **responsive** bir yapıda gerçekleştirmektir.

### 🎯 Proje Amacı

- Veritabanındaki gönderilmemiş e-postaları tespit etmek
- Uygun olanları RabbitMQ kuyruğuna yazmak
- Queue yapısıyla sistemi ölçeklenebilir hale getirmek
- Email gönderim servisinden iş yükünü ayırmak

### 🛠️ Özellikler

- Veritabanı kontrolü ile uygun kayıtları seçme
- RabbitMQ kuyruğuna veri gönderme
- Servis tabanlı mimari
- Loglama ile işlem takibi
- Zamanlayıcı ile periyodik kontrol (örneğin Timer/Hosted Service)

### 🚀 Kullanılan Teknolojiler

- **.NET (C#)**
- **RabbitMQ.Client**
- **Entity Framework Core**
- **MSSQL** (Veritabanı)
- **HostedService / Timer** yapısı
- **Serilog** veya benzeri loglama aracı (isteğe bağlı)

### 📦 Kurulum ve Çalıştırma

1. Bu repoyu klonlayın:
   ```bash
   git clone https://github.com/Akinincecik/RabbitMQEmailQueueService.git
------------------------------------------------------------------------------------------

# RabbitMQ Email Queue Service

## 🇬🇧 Project Description

This service is designed to monitor the database for email tasks and publish eligible ones to a **RabbitMQ** queue. This enables email sending to be handled asynchronously by a separate service (e.g., `EmailSenderService`). The goal is to maintain a **responsive** and **non-blocking** system by decoupling the workflow and delegating the workload efficiently.

### 🎯 Project Objectives

- Detect unsent emails in the database
- Publish eligible records to RabbitMQ queue
- Make the system scalable using queue architecture
- Separate the email delivery process from data monitoring

### 🛠️ Features

- Periodic database checks for pending email tasks
- Publishing messages to RabbitMQ queue
- Service-oriented architecture
- Logging support for operation tracking
- Timer or hosted service-based background execution

### 🚀 Technologies Used

- **.NET (C#)**
- **RabbitMQ.Client**
- **Entity Framework Core**
- **MSSQL**
- **HostedService / Timer**
- **Serilog** or similar logging library (optional)

### 📦 Installation & Running the Service

1. Clone this repository:
   ```bash
   git clone https://github.com/Akinincecik/RabbitMQEmailQueueService.git
