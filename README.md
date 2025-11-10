# 🎮 Yapay Zeka Destekli TPS Oyunu    

**Kocaeli Üniversitesi - Bilişim Sistemleri Mühendisliği**      
**2025-2026 Güz Dönemi | Yazılım Geliştirme Laboratuvarı I Projesi**    

---    

## 👨‍💻 Ekip Üyeleri    

- **Mustafa Mehmet Aslandağ** — 231307067@kocaeli.edu.tr - Github: mezoxy-dev    
- **Mustafa Kerem Çekici** — 231307121@kocaeli.edu.tr - Github: Sayicon    
- **Oğuzhan Erbil** — 231307021@kocaeli.edu.tr - Github: oguzhanerbil    

---    

## 🧠 Proje Tanımı    

Bu proje, **Yapay Zeka Destekli TPS (Third-Person Shooter)** türünde bir oyunun teknik altyapısını ve geliştirme sürecini kapsamaktadır.      
Unity oyun motoru kullanılarak **C# dili** ile geliştirilmiştir.      

Amaç, hem **Nesne Yönelimli Programlama (OOP)** hem de **Veri Odaklı Mimari (Data-Oriented Design)** yaklaşımlarını birleştirerek,      
**esnek, genişletilebilir ve öğrenmeye yönelik** bir oyun sistemi kurmaktır.    

---    

## ⚙️ Kullanılan Teknolojiler, Mimariler ve Yöntemler    

### 🔹 1. Mimari Yaklaşım — OOP + Veri Odaklı Tasarım    

- **Temel OOP İlkeleri:**      
  Kalıtım (Inheritance) ve kapsülleme (Encapsulation) ilkeleri `MonoBehaviour` tabanlı sınıflarda uygulanmıştır.    

- **Prefab Sistemi:**      
  Her düşman tipi, davranış mantığı ve materyalleriyle birlikte prefab olarak oluşturulmuş, yeniden kullanılabilir hale getirilmiştir.    

- **Interface Kullanımı:**      
  `IDamageable` arayüzü ile oyuncu, düşman ve kırılabilir nesneler hasar alma davranışını ortak şekilde uygular.    

- **ScriptableObject:**      
  `CharacterData` yapısı ile karakter/düşman özellikleri (sağlık, hız, güç vb.) veri olarak soyutlanmış, kod yazmadan yeni varyantlar üretmek mümkün hale getirilmiştir.    

- **Decoupling (Ayrıştırma):**      
  Arayüzlerle davranış, ScriptableObject’lerle veri birbirinden ayrılmış, bu sayede yönetilebilir ve genişletilebilir bir mimari elde edilmiştir.    

---    

### 🧱 2. Seviye Tasarımı Araçları    

- **ProBuilder:**      
  Seviye geometrisi (zemin, duvar, siper alanları vb.) hızlıca oluşturmak için kullanılmıştır.      

- **Polybrush:**      
  Yüzey detaylandırma, şekillendirme (sculpting) ve doku boyama (texture painting) işlemleri için uygulanmıştır.      

---    

### 🤖 3. Yapay Zeka (AI) ve Hareket Sistemi    

- **Rigidbody Tabanlı Hareket:**      
  Düşman karakterlerin hareketi, Unity’nin `Rigidbody` bileşeniyle kontrol edilmektedir. Bu sayede fizik tabanlı çarpışmalar daha doğal bir şekilde gerçekleşir.    

- **rb.MovePosition ile Takip:**      
  Düşman, oyuncunun pozisyonunu sürekli izleyerek `rb.MovePosition` yöntemiyle hedefe doğru ilerler. Bu yaklaşım, hem stabil hareket hem de çarpışma farkındalığı sağlar.    

- **Engel Algılama ve Tırmanma Sistemi:**      
  Eğer düşman bir duvara çarparsa, bu durum `Raycast` ile algılanır. Düşman, duvar yüksekliği belirli bir eşiğin altındaysa,   
  `AddForce(Vector3.up * forceAmount, ForceMode.Impulse)` komutuyla kısa bir sıçrama yaparak tırmanır. Böylece düşman,  
  dar alanlarda veya engebeli arazilerde takılıp kalmadan oyuncuya doğru ilerleyebilir.    

- **Yapay Zeka Davranış Akışı:**      
  Düşman, hedefini sürekli analiz eder. Oyuncu belirli bir menzildeyse saldırıya geçer, değilse hareketine devam eder.  
  Böylece basit ama etkili bir düşman zekası oluşturulmuştur.    

---    

### ♻️ 4. Object Pooling Sistemi    

- **Neden Object Pooling?**      
  Oyun içinde sürekli olarak düşman yaratmak (`Instantiate`) ve yok etmek (`Destroy`) performans sorunlarına yol açar. Bu yüzden **Object Pooling** sistemi kullanılmıştır.    

- **Nasıl Çalışıyor:**      
  Oyun başında belirli sayıda düşman objesi oluşturulup, devre dışı bırakılarak bellekte tutulur. Yeni düşman gerektiğinde hazır olan bir obje etkinleştirilir. Düşman öldüğünde, `SetActive(false)` ile havuza geri gönderilir.    

- **Avantajları:**      
  - Garbage Collector yükünü azaltır.    
  - FPS düşüşlerini önler.    
  - Spawn/Despawn işlemleri çok daha hızlı gerçekleşir.    

---    

### 🧩 5. Ekip Çalışması ve Versiyon Kontrolü (Git)    

- **Kullanılan Araçlar:**      
  - Git Bash (komut satırı)      
  - GitHub Desktop (görsel arayüz)    

- **Yapılandırmalar:**    
  - `Unity.gitignore`: Gereksiz klasörlerin (Library, Temp, obj) repoya eklenmesi engellendi.      
  - **Git LFS (Large File Storage):** Büyük boyutlu `.fbx`, `.png`, `.wav` dosyaları verimli şekilde yönetildi.    

- **Çalışma Akışı (Workflow):**    
  - Her ekip üyesi kendi `feature-branch` dalında geliştirme yaptı.      
  - Kodlar test edilip onaylandıktan sonra `main` dalına **Pull Request** ile birleştirildi.    

---    

## 🎮 Oyun Akışı (Game Flow)    

Aşağıdaki akış, oyuncunun oyun deneyimini adım adım açıklar:      
ana menüden oyunu başlatma, düşmanlarla çatışma ve portalı bulma süreci.    

### 🧭 Oyun Akışı Diyagramı    

```mermaid    
flowchart TD    
  %% --- MENU ---    
  A[Main Menu] --> B{Select Option}    
  B --> |Play| C[Game Starts]    
  B --> |Quit| D[Exit Game]    

  %% --- GAME START ---    
  C --> E[Music Starts Playing]    
  E --> F[Enemies Spawn via Object Pooling]    
  F --> G[Enemies Move Toward Player (rb.MovePosition)]    
  G --> H[If Wall → Jump with Force]    
  H --> I[Player Auto-Shoots & Kills Enemies]    
  I --> J[Search for Portal]    

  %% --- PORTAL SEARCH LOOP ---    
  J --> K{Portal Found?}    
  K --> |Yes| L[Enter Portal → Level Complete]    
  K --> |No| J    

  %% --- OPTIONAL: PLAYER DEATH ---    
  G --> M{Player Health = 0?}    
  M --> |Yes| N[Player Dies → Game Over]    
  M --> |No| H    
```
