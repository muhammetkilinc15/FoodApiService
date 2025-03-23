# FoodApiService

Bu proje, yemek rezervasyon sistemi için oluşturulmuş bir API servisidir. Mikroservis mimarisi kullanılarak geliştirilmiştir.

## Özellikler

- Restful API
- PostgreSQL veritabanı entegrasyonu
- Docker ile container oluşturma
- CI/CD entegrasyonu (GitHub Actions kullanılarak)

## Gereksinimler

- .NET 6 SDK veya üzeri
- Docker
- PostgreSQL

## Kurulum

### Adım 1: Depoyu Klonlayın

```bash
git clone https://github.com/muhammetkilinc15/FoodApiService.git
cd FoodApiService
```

### Adım 2: Docker Containerlarını Başlatın

Proje, Docker kullanılarak kolayca başlatılabilir.

```bash
docker-compose up -d
```

### Adım 3: Veritabanı Migrasyonlarını Çalıştırın

Veritabanı migrasyonlarını çalıştırarak gerekli tabloları oluşturun.

```bash
dotnet ef database update
```

### Adım 4: API Servisini Başlatın

Projenin kök dizininde aşağıdaki komutu çalıştırarak API servisini başlatın.

```bash
dotnet run
```

## Kullanım

API'yi çalıştırdıktan sonra, aşağıdaki URL üzerinden erişim sağlayabilirsiniz:

```
http://localhost:5000
```

### Örnek API Çağrıları

- **GET** `/api/foods` - Tüm yemekleri listele
- **POST** `/api/foods` - Yeni bir yemek ekle
- **GET** `/api/foods/{id}` - Belirli bir yemeği getir
- **PUT** `/api/foods/{id}` - Belirli bir yemeği güncelle
- **DELETE** `/api/foods/{id}` - Belirli bir yemeği sil

## GitHub Actions

Bu proje, CI/CD işlemleri için GitHub Actions kullanmaktadır. Workflow dosyaları `.github/workflows` dizininde bulunmaktadır.

### Örnek GitHub Actions Workflow

```yaml
name: CI/CD Pipeline

on:
  push:
    branches:
      - main

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v2

      - name: Set up .NET Core
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '6.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore

      - name: Test
        run: dotnet test --no-build --verbosity normal

      - name: Publish
        run: dotnet publish --no-build -c Release -o out

      - name: Build Docker image
        run: docker build -t foodapiservice .

      - name: Push Docker image to Docker Hub
        run: docker push foodapiservice
```

## Katkıda Bulunma

Katkıda bulunmak için lütfen bir pull request oluşturun. Her türlü katkı ve geri bildirim değerlidir.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakın.
