YEMEKHANE
proje oluştur web api (xyz.API)
sln oluştur (xyz_Deg.sln)
gerekli class libraries ekle (add new project)

Yemekhane.sln
 ├── Yemekhane.API           → Presentation Layer (Web API projesi)
 ├── Yemekhane.Business      → Class Library (.NET Class Library)
 ├── Yemekhane.Data          → Class Library (.NET Class Library)
 └── Yemekhane.Entities      → Class Library (.NET Class Library)

Görünüm bu şekilde ,


Yemekhane.Entities projesini hem Data hem de Business (ve muhtemelen API) projelerine referans olarak ekleyin. Bu sayede diğer katmanlar entity sınıflarını kullanabilir.
Yemekhane.Data projesini Business katmanına referans ekleyin (Business, Data’daki repository’leri kullanacak).
Yemekhane.Business projesini API katmanına referans ekleyin (Web API, Business servislerini çağıracak).
Ayrıca Yemekhane.Data projesini API projesine de referans ekleyebilirsiniz çünkü Startup/Program tarafında DbContext ve repository bağımlılıklarını eklerken Data katmanındaki sınıflara ihtiyaç duyacağız.


ENTITIES-----> -----   model classlar
DATA---------> ENTITIES veri işlemleri
BUSINESS-----> DATA + ENTITIES iş kuralları
API----------> BUSINESS + ENTITES ana proje presentation
---
DATA VE ENTITIES KURULUMU
ENTITIES
⦁	 Entites classları oluşturuldu. not-nullable uyarılarına required eklendi class atamalarına
⦁	Rating.cs üzerinden ICollection ile one2Many ilişkiler bağlandı.
DB
⦁	PACKAGES:  Install-Package Microsoft.EntityFrameworkCore -Project Yemekhane.Data -Version 9.*
Uninstall-Package Microsoft.EntityFrameworkCore.Relational
Uninstall-Package Microsoft.EntityFrameworkCore.Tools
⦁	SÜRÜMLERİ DÜZENLE KONTROL ET AYIKLA
⦁	NuGet manager ile halledildi :+
⦁	Her şey 9.0.8
appsettings.json --- sqlexpress

AppDbContext tamam

MIGRATION UPDATE DB
PM> Add-Migration InitialCreate -Project Yemekhane.Data -StartupProject Yemekhane.API
Build started...
Build succeeded.

PM> Update-Database -Project Yemekhane.Data -StartupProject Yemekhane.API
Build started...
Build succeeded.
TABLO'S
Done.

DATA REPOSİTORİES
////Servislerin Repository’lerle İletişimi: Dikkat ederseniz service sınıfları, satır aralarında _repo.Add çağırıp duruyor ama _context.SaveChanges() çağrısı görünmüyor. Bu, tasarımı basit tutmak için: SaveChanges’i doğrudan burada çağırmak yerine, kontrolcülerde birden fazla işlem yapıldığında hepsini toplayıp en sonda tek bir SaveChanges yapabiliriz.
⦁	Dosya implementations /  interfaces olarak ayrıldı .IRepositories
 	class         /  interface olarak eklendi .


⦁	 BUSINESS SERVİCES IMPL VE INTERFACE RATING OUT!

⦁	repo interface uyumsuzlukları düzeltildi..

⦁	 (usin namespace proc.data.repos.interf/impl ) kaldırıldı repository den alıyor direkt

⦁	"Jwt": { "Key": "ralphcamoraisalive", appsettings.json key
⦁	DI CONTAINER EKLENDİ program.cs e
⦁	Sorun yok
BUSINESS DONE
WEB API SON KISIM                                  (ENTITIES için bir değişiklik 						olursa -migration, update db)


 														otherwise clean-rebuild / ctrl+shift+b  **** ctrl f5
swagger ,swashbuckle eklendi
controller oluşturuldu
entegrasyon ve namespace hataları düzeltildi

Nullable hataları düzeltildi. 25 err, 10 warn


Clean Build
ReBuild ========== Rebuild All: 4 succeeded, 0 failed, 0 skipped ==========
========== Rebuild completed at 14:37 and took 13,179 seconds ==========
host düzeltildi

14 54 done
TOKEN DÜZENLENDİ
DTo's ekle EKLE program.cs  AppDbContextFile.cs eksikliklerini düzeelt
MEALSCONTROLLER SUGGEST CONTROLLER İŞLERİNİ HALLET .
CRUD OPT için ; link
                code
_____________________________________________________________________________
 								ÇARŞAMBA 13.08.25
.CRUD DÜZENLEMELERİ:
MealsController ,Repo , impl , interface düzenlemeleri


Meals için getbyid, create update delete eklendi

⦁	EF Core veya API, veritabanından eski veriyi çekip boş gelen alanları otomatik olarak doldurmaz
⦁	DefaultIfEmpty ile verilen parametreli default değer EF Core’da her zaman SQL’e çevrilemez.


REPOSİTORY HER ZAMAN ENTITY İLE
SERVICE İÇİNE DTO ELEMANLARI ALIR TRANSFERLER DTO ÜZERİNDEN OLUR
14.08.25

ratingrepository.cs 20ln --->defaultifempty()
 içinde parametre ile EFCore SELECT AVG(CASE WHEN COUNT(*) = 0 THEN 5 ELSE Score END) dönüşümünü her zaman yapamaz translate hatası verir.
  

public IActionResult Rate(int id, RateMealRequestDto body(dto))  veya [FromBody ]   RateMealDto body(dto) çok parametre için iyi

parametre adının “body” olması şart değil. İstersen “dto” yap, çalışması değişmez. ASP.NET Core (özellikle [ApiController] varken) basit tipleri (int, string) route/query’den; kompleks tipleri (sınıflar) request body’den bağlar. Yani ad önemli değil, kaynak önemli.




Hangi Aracı Seçmelisiniz?
Postman/Insomnia → Manuel ve otomatik test için.

Swagger → Dökümantasyon + Test.

GitHub Actions → CI/CD ile entegre test.

RunKit → Hızlı Node.js API test


YEMEK SEÇİMİ İLE İLGİLİ İSTATİSTİK İÇİN DTO EKLENDİ 

bir DTO dosyasıdır → SuggestionStatDto.cs (yeni .cs).
İstatistiği üretmek için mevcut servisi genişletiyoruz (ISuggestionService + SuggestionService).
Repository’leri güncellemen gerekmiyor. (JOIN’i servis içinde AppDbContext ile yapacağız.)
İstersen ayrı bir “query service” de açabilirsin; ama en az değişiklik için mevcut SuggestionService’e eklemek yeterli.

Kısacık “anti-pattern → çözüm” özeti
Meal içine Rating’i gömmek → SRP ihlali → ayrı entity & servis.
Controller’ın EF’e dayanması → DIP ihlali → sadece servis arayüzlerine dayan.
Kocaman IMealService → ISP ihlali → Meal/Rating/Suggestion servislerini böl.
Ortalama hesaplamayı MealService’e sabitlemek → OCP ihlali → Strategy ile çöz.
Farklı repo implementasyonlarında farklı davranış → LSP ihlali → sözleşmeyi netleştir.


 
lisans işlemleri vs halloldu düzeltmeler yapıldı. PROJE AYAĞA KALKTI YÜRÜYOR :d 

TO DO LIST
⦁	GLOBAL HATA YAKALAMA VE CONVERT İŞLEMLERİ,
⦁	SOLID ENTEGRASYONU 
⦁	suggestion for meals----> daha önce eklenen yemekleri, yeni suggest yapacak kullanıcılar görebilsin
⦁	yemek puanlamasındda  1 -5 arası puanlama kontrolünü yakalasın .....
⦁	


upgrade delete için  id ----dto 






























