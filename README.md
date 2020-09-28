# aspnet-core-registration-login-api

### Clone repositori
```bash 
git clone https://github.com/msyamf/dotnet-core2-registration-login-api.git
```

### masuk direktori project
```bash 
cd dotnet-core2-registration-login-api
```

### install dependency 
```bash 
dotnet prepare
```

### update database  
```bash 
dotnet ef database update
```


### run 
```bash 
dotnet run
```


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
 ### lalu coba jalankan ulang `dotnet run`
 
 
 
 
