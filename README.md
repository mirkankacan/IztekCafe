# IztekCafe - .NET Core 9

Modern ve ölçeklenebilir bir cafe yönetim sistemi projesi.

## Kullanılan Teknolojiler ve Patternler

### Teknolojiler
+ **.NET Core 9**: En son sürüm .NET Core. Platformlar arası uyumluluk ve performans sağlayan projenin temeli.
+ **MSSQL**: İlişkisel veri tabanı yönetim sistemi.
+ **Entity Framework Core**: Veri tabanı etkileşimleri için Object-Relational Mapping (ORM) aracı.
+ **LINQ**: Veri koleksiyonlarını sorgulamak, verileri filtrelemek veya dönüştürmek gibi işlemleri kolaylaştıran teknoloji.
+ **Mapster**: Farklı tipteki complex objeleri birbilerine otomatik dönüştüren kütüphane.
+ **FluentValidation**: Model doğrulama işlemleri için güçlü ve esnek validasyon kütüphanesi.
+ **NewId**: Benzersiz ID üretimi için performanslı kütüphane.
+ **Swagger**: API'lerin açıklayıcı bir şekilde belgelenmesini sağlayan açık kaynaklı araç.
+ **Carter**: Minimal API'lar için endpoint routing ve configuration sağlayan framework.

### Mimari Patternler
+ **Clean Architecture**: Katmanlı mimari yapısı ile bağımlılıkları tersine çeviren, test edilebilir ve sürdürülebilir kod yapısı.
+ **Dependency Injection**: Bağımlılıkları verimli bir şekilde yönetme ve çözme pattern'i.
+ **Repository Pattern**: Veri erişim katmanını soyutlayan ve veri kaynağından bağımsız hale getiren tasarım deseni.
+ **Unit of Work Pattern**: İş birimlerini yöneten ve transaction tutarlılığını sağlayan tasarım deseni.

## Gereksinimler

+ Makinenizde yüklü bir [.NET Core 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
+ [Microsoft SQL Server 2022 Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) kurulmuş ve yapılandırılmış olmalıdır
+ [Visual Studio 2022](https://visualstudio.microsoft.com/) veya [Visual Studio Code](https://code.visualstudio.com/) 

## Veri Tabanı Tasarımı

+ Proje dizini altındaki `diagram.drawio` dosyası ile [draw.io](https://app.diagrams.net) sitesi üzerinde görüntülenebilir
+ Microsoft SQL Server Management Studio ile ilgili veri tabanı içinde `Database Diagrams` altında oluşturulabilir

## Kurulum 

### 1. Projeyi Klonlama
Bu GitHub deposunu yerel makinenize klonlamak için aşağıdaki komutu kullanın:
```bash
git clone https://github.com/mirkankacan/IztekCafe.git
```

### 2. Proje Dizinine Gitme
```bash
cd IztekCafe
```

### 3. Bağımlılıkları Geri Yükleme
```bash
dotnet restore
```

### 4. Veri Tabanı Konfigürasyonu
`appsettings.Development.json` dosyasındaki connection string'i kendi SQL Server bilgilerinize göre düzenleyin:

```json
{
   "ConnectionStrings": {
      "SqlServer": "Data Source=MIRKANVICTUS;Initial Catalog=IztekCafeDb;User ID=sa;Password=123456aA*;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;"
   }
}
```

### 5. Veri Tabanı Migration'larını Uygulama
Veri tabanını oluşturmak ve tabloları kurmak için:
```bash
dotnet ef database update
```

> **Not**: Eğer `dotnet ef` komutu bulunamıyorsa, önce Entity Framework tools'u yükleyin:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 6. Projeyi Derleme
```bash
dotnet build
```

### 7. Projeyi Çalıştırma
```bash
dotnet run
```
Proje `http://localhost:5066` adresinde çalışacaktır.

## API Dokümantasyonu

Proje çalıştırıldıktan sonra Swagger UI'ya şu adresten erişebilirsiniz:
```
http://localhost:5066/swagger
```

## Proje Yapısı

Bu proje **Clean Architecture** prensiplerine uygun olarak katmanlı mimari ile tasarlanmıştır:

```
IztekCafe/
├── src/
│   ├── Api/
│   │   └── IztekCafe.WebApi/           # Presentation Layer
│   │       ├── Endpoints/              # Carter Minimal API Endpoints
│   │       ├── Extensions/             # Extension metotları
│   │       ├── Filters/                # Validation Filters
│   │       ├── appsettings.json        # Konfigürasyon dosyası
│   │       └── Program.cs              # Uygulama giriş noktası
│   ├── Core/
│   │   ├── IztekCafe.Application/      # Application Layer
│   │   │   ├── Contracts/              # Interface'ler
│   │   │   │   ├── Repositories/       # Repository Pattern Interface'leri
│   │   │   │   ├── Services/           # Service Interface'leri
│   │   │   │   └── UnitOfWork/         # Unit of Work Pattern
│   │   │   ├── Dtos/                   # Data Transfer Objects
│   │   │   ├── MappingConfigs/         # Mapster Mapping Configurations
│   │   │   └── Validators/             # FluentValidation Rules
│   │   └── IztekCafe.Domain/           # Domain Layer
│   │       ├── Entities/               # Domain Entities
│   │       ├── Enums/                  # Domain Enums
│   │       └── Extensions/             # Domain Extensions
│   └── Infrastructure/
│       └── IztekCafe.Persistance/      # Infrastructure Layer
│           ├── Data/
│           │   ├── Context/            # Entity Framework DbContext
│           │   ├── Configurations/     # EF Entity Configurations
│           │   └── Migrations/         # EF Core Migrations
│           ├── Repositories/           # Repository Pattern Implementations
│           ├── Services/               # Business Logic Services
│           └── UnitOfWork/             # Unit of Work Implementation
└── README.md
```

## Kullanım

1. Projeyi başlattıktan sonra `http://localhost:5066/swagger` adresine giderek API endpointlerini görüntüleyebilirsiniz
2. Swagger UI üzerinden API'ları test edebilirsiniz
3. Postman veya benzeri araçlarla API'lara istek gönderebilirsiniz

## İletişim

Proje sahibi: [mirkankacan](https://github.com/mirkankacan)

Herhangi bir sorunuz varsa issue açabilir veya [email](mailto:kacanmirkan@gmail.com) yoluyla iletişime geçebilirsiniz.
