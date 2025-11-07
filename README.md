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
  Her düşman tipi, FSM (Finite State Machine) mantığı ve materyalleriyle birlikte prefab olarak oluşturulmuş, yeniden kullanılabilir hale getirilmiştir.

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

### 🤖 3. Yapay Zeka (AI) ve Yol Bulma (Pathfinding)

- **FSM (Finite State Machine):**  
  Düşman davranışları dört temel durumdan oluşur:
  - `Idle` (Boşta)
  - `Patrol` (Devriye)
  - `Chase` (Kovalama)
  - `Attack` (Saldırı)

- **Özel Yol Bulma Sistemi:**  
  Unity’nin statik **NavMesh** sistemi, dinamik NPC doğurma sırasında sorun yarattığından kaldırılmış;  
  bunun yerine FSM tabanlı, hedefe yönelmeyi ve engellerden kaçınmayı sağlayan **özel bir script tabanlı pathfinding** geliştirilmiştir.

---

### 🧩 4. Ekip Çalışması ve Versiyon Kontrolü (Git)

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

## 🧩 Sistem Şeması ve Oyun Mekaniği

### ⚙️ Sistem Mimarisi

**Character (GameObject)**  
├── `CharacterHealth` (Script → `IDamageable` arayüzünü uygular)  
├── `CharacterMovement` (Script)  
└── `CharacterData` (ScriptableObject: Sağlık, hız, güç verilerini tutar)

---

### 🧠 FSM (Finite State Machine) Yapısı

| Durum | Açıklama |
|-------|-----------|
| **Idle** | Düşman etrafı dinler ve oyuncuyu arar. |
| **Patrol** | Belirlenmiş noktalarda devriye gezer. |
| **Chase** | Oyuncuyu gördüğünde özel pathfinding algoritmasıyla takip eder. |
| **Attack** | Oyuncu menzile girdiğinde saldırı gerçekleştirir. |

---

## 🎮 Oyun Akışı (Game Flow)

Aşağıdaki akış, oyuncunun oyun deneyimini adım adım açıklar:  
ana menüden oyunu başlatma, düşmanlarla çatışma ve portalı bulma süreci.

### 🧭 Oyun Akışı Diyagramı

```mermaid
flowchart TD
  A[Ana Menü] --> B{Seçim}
  B --> |Play| C[Oyun Başlatılır]
  B --> |Quit| D[Oyundan Çıkılır]

  C --> E[Müzik Başlar 🎵]
  E --> F[Düşmanlar Doğar (Spawn)]
  F --> G[Düşmanlar Oyuncuya Doğru Hareket Eder]
  G --> H[Oyuncu Silahlarıyla Otomatik Olarak Saldırır 🔫]
  H --> I[Portal Aranır]
  I --> J{Portal Bulundu mu?}
  J --> |Evet| K[Portala Gir -> Bölüm Tamamlandı 🌀]
  J --> |Hayır| I

  G --> L{Oyuncu Sağlığı Sıfır mı?}
  L --> |Evet| M[Oyuncu Ölür 💀 -> Oyun Biter (Retry/Exit)]
  L --> |Hayır| H
