# aspnet-core-registration-login-api
pertama siapkan koneksi internet, Lektop dan secangkir kopi KOPI :)


Download dotnet 2.2
* [windows](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-2.2.207-windows-x64-installer)

* linux
```bash 
sudo apt install dotnet-sdk-2.2
```

### Clone repositori
```bash 
git clone https://github.com/msyamf/dotnet-core2-registration-login-api.git
```

### masuk direktori project
```bash 
cd dotnet-core2-registration-login-api
```

### seting DB
Buka file `Helpers/AppSettings.cs` edit sesuai koneksi yg ada

### install dependency 
```bash 
dotnet restore
```

### update database  
```bash 
dotnet ef database update
```


### run 
```bash 
dotnet run
```
 lalu buka broser `http://localhost:4000/` jika ingin coba test api lewat `Swagger UI`

### buat model 
contoh untuk buat tabel baru, buka file `Entities/Model.cs` kode dibawah sebagai contoh

```c#
...
    public class ProfileUser
    {
        public int Id { get; set; }
        public virtual string Kodepos { get; set; }
        public string Alamat { get; set; }
        public virtual string NoTelfon { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
...
```
 buka file `Entities/DataContext.cs` untuk Konfigurasi relasi tabel
 
 ### migrasi database
 ```bash
 dotnet ef migrations add InitialCreate
 ``` 
### update database
 ```bash
  dotnet ef database update
 ```
 
 

 ### Build a project
 lihat link berikut [https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build) untuk tutorial lengkap nya

```bash
  dotnet build
```

jika sudah akan ada folder seperti berikut `dotnet-core2-registration-login-api/bin/Debug/netcoreapp2.2`

masuk dalam folder dan jalankan jika ingin mencobanya
```bash
dotnet WebApi.dll
```

untuk uplod pada `VPS` kunjungi link brikut
[https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.2](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.2)

### NOTE
Jika ingin di build jadi `EXE` atau binary pada linux
```bash
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r ubuntu.16.10-x64
```
 ### tree
```bash
├── appsettings.Development.json
├── appsettings.json
├── bin 
├── Class
│   ├── FormBody.cs
│   └── UserDto.cs
├── Controllers
│   ├── NewController.cs
│   └── UsersController.cs
├── Entities
│   ├── DataContext.cs
│   └── Model.cs
├── Helpers
│   ├── AppException.cs
│   ├── AppSettings.cs
│   ├── AutoMapperProfile.cs
│   └── CustomValidation.cs
├── LICENSE
├── Migrations
│   ├── 20200928121021_InitialCreate.cs
│   ├── 20200928121021_InitialCreate.Designer.cs
│   └── DataContextModelSnapshot.cs
├── obj 
├── Program.cs
├── README.md
├── Services
│   └── UserService.cs
├── Startup.cs
└── WebApi.csproj
```
 
 
 
 
