# 🎮 Yapay Zeka Destekli Third Person Shooter (TPS) Oyunu

## 📋 Proje Hakkında

Bu proje, Kocaeli Üniversitesi Teknoloji Fakültesi Bilişim Sistemleri Mühendisliği bölümü 2025-2026 Güz Dönemi Yazılım Geliştirme Laboratuvarı - I dersi kapsamında geliştirilmiştir.

**Proje Amacı:** TPS oyun mekaniklerini karşılayan, yapay zeka destekli NPC'ler içeren, Unity oyun motoru ile geliştirilmiş bir third person shooter oyunu oluşturmak.

## 👥 Ekip Üyeleri

- **Üye 1:** [İsim Soyisim] - [GitHub Kullanıcı Adı]
- **Üye 2:** [İsim Soyisim] - [GitHub Kullanıcı Adı]
- **Üye 3:** [İsim Soyisim] - [GitHub Kullanıcı Adı]

## 🎯 Oyun Senaryosu

[Buraya oyununuzun temasını, hikayesini ve senaryosunu detaylı şekilde yazınız. Örnek: "Post-apokaliptik bir dünyada, oyuncu son kalan insanlardan biri olarak düşman robotlara karşı savaşmaktadır..."]

## 🎮 Oyun Mekanikleri

### Temel TPS Mekanikleri
- ✅ Üçüncü şahıs kamera sistemi
- ✅ Karakter hareketi (koşma, yürüme, zıplama)
- ✅ Nişan alma ve ateş etme sistemi
- ✅ Kapak alma (Cover System) mekaniği
- ✅ Silah sistemi ve envanter yönetimi
- ✅ Sağlık ve hasar sistemi

### Yapay Zeka (AI) Sistemi

#### 1. Finite State Machine (FSM)
NPC'ler için implementasyon:

- **Idle (Boşta):** Düşman beklemede, çevresini gözlemler
- **Patrol (Devriye):** Belirlenen waypoint'ler arasında devriye gezme
- **Chase (Kovalama):** Oyuncuyu tespit edince takip etme
- **Attack (Saldırı):** Menzile girince saldırı başlatma

#### 2. Pathfinding (Yol Bulma)
- Unity NavMesh sistemi kullanılarak AI Navigation
- NavMesh Agent ile akıllı düşman hareketi
- Engelleri aşabilme ve en kısa yolu bulma

## 🛠️ Kullanılan Teknolojiler

- **Oyun Motoru:** Unity [Version]
- **Programlama Dili:** C#
- **AI Sistemi:** Unity NavMesh, Finite State Machine
- **Versiyon Kontrol:** Git & GitHub
- **3D Modelleme:** [Kullanılan araçlar]
- **Ses Efektleri:** [Kullanılan araçlar/kaynaklar]

## 📦 Kullanılan Asset'ler ve Paketler

- [Asset/Paket İsmi 1] - [Kullanım amacı]
- [Asset/Paket İsmi 2] - [Kullanım amacı]
- [Asset/Paket İsmi 3] - [Kullanım amacı]

*Not: Tüm asset'ler sadece model, texture ve materyal amaçlı kullanılmıştır.*

## 🏗️ Proje Mimarisi

### Sistem Şeması
```
[Buraya sistemin genel yapısını gösteren bir metin tabanlı şema ekleyiniz]

Player Controller
    ├── Movement System
    ├── Shooting System
    └── Health System

AI System
    ├── FSM Controller
    ├── NavMesh Agent
    └── Detection System

Game Manager
    ├── Level Manager
    ├── UI Manager
    └── Score System
```

### Sınıf Diyagramı
[UML sınıf diyagramınızı buraya ekleyiniz]

### Oyun Akış Diyagramı
[Oyun akışını gösteren flowchart'ı buraya ekleyiniz]

## 🎨 Grafik ve Optimizasyon

- **Polygon Sayısı:** Low-poly modeller kullanılmıştır
- **Texture Optimizasyonu:** [Detaylar]
- **Lighting:** [Kullanılan aydınlatma teknikleri]
- **Post-Processing:** [Kullanılan efektler]

## 📚 Literatür Taraması

### İncelenen Örnek Çalışmalar

1. **[Oyun/Çalışma İsmi 1]**
   - Özellikler: [...]
   - Benzerlikler: [...]
   - Farklılıklar: [...]

2. **[Oyun/Çalışma İsmi 2]**
   - Özellikler: [...]
   - Benzerlikler: [...]
   - Farklılıklar: [...]

### Karşılaştırma
[Projenizin literatürdeki çalışmalardan farkını ve avantajlarını yazınız]

## 🔧 Geliştirme Süreci

### Kullanılan Yazılımsal Mimariler
- **Design Pattern'ler:** Singleton, State Pattern, Observer Pattern
- **SOLID Prensipleri:** [Nasıl uygulandığı]
- **OOP Prensipleri:** Encapsulation, Inheritance, Polymorphism

### Geliştirme Aşamaları

#### Sprint 1: Temel Mekanikler (Tarih - Tarih)
- Karakter kontrol sistemi
- Kamera sistemi
- Temel hareket mekanikleri

#### Sprint 2: AI Sistemi (Tarih - Tarih)
- FSM implementasyonu
- NavMesh kurulumu
- Düşman davranışları

#### Sprint 3: Oynanış ve Optimizasyon (Tarih - Tarih)
- Silah sistemi
- UI/UX tasarımı
- Performans optimizasyonu

## 🚧 Karşılaşılan Zorluklar ve Çözümler

### Zorluk 1: [Zorluk Başlığı]
**Problem:** [Detaylı açıklama]
**Çözüm:** [Nasıl çözüldü]

### Zorluk 2: [Zorluk Başlığı]
**Problem:** [Detaylı açıklama]
**Çözüm:** [Nasıl çözüldü]

### Zorluk 3: [Zorluk Başlığı]
**Problem:** [Detaylı açıklama]
**Çözüm:** [Nasıl çözüldü]

## 💡 Projenin Kazanımları

- Unity oyun motoru deneyimi
- AI programlama ve FSM implementasyonu
- NavMesh ve pathfinding algoritmaları
- C# ve OOP prensipleri
- Git/GitHub ile versiyon kontrolü ve takım çalışması
- Oyun tasarımı ve level design
- Problem çözme ve debugging yetenekleri

## 📂 Proje Yapısı

```
Project-YL/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Player/
│   │   ├── AI/
│   │   ├── Weapons/
│   │   ├── Managers/
│   │   └── UI/
│   ├── Models/
│   ├── Materials/
│   ├── Textures/
│   ├── Scenes/
│   └── Prefabs/
│
├── Packages/
├── ProjectSettings/
└── README.md
```

## 🎮 Nasıl Oynanır?

### Kontroller
- **WASD:** Karakter hareketi
- **Mouse:** Kamera kontrolü
- **Sol Tık:** Ateş etme
- **Sağ Tık:** Nişan alma
- **Space:** Zıplama
- **Shift:** Koşma
- **C:** Eğilme/Kapanma

### Oyun Hedefi
[Oyunun amacını ve kazanma koşullarını yazınız]

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- Unity [Version] veya üzeri
- [Diğer gereksinimler]

### Adımlar
1. Repository'yi klonlayın:
   ```bash
   git clone https://github.com/Sayicon/Project-YL.git
   ```
2. Unity Hub'dan projeyi açın
3. [Gerekli paketlerin kurulumu]
4. Ana sahneyi açın: `Assets/Scenes/MainScene.unity`
5. Play butonuna basarak oyunu başlatın

## 📹 Ekran Görüntüleri ve Videolar

### Oynanış Görselleri
![Gameplay 1](screenshots/gameplay1.png)
![Gameplay 2](screenshots/gameplay2.png)

### Demo Video
[YouTube/Video linki]

## 📊 Görev Dağılımı

| Görev | Sorumlu | Durum |
|-------|---------|-------|
| Karakter kontrolcüsü | [İsim] | ✅ Tamamlandı |
| AI FSM sistemi | [İsim] | ✅ Tamamlandı |
| Silah sistemi | [İsim] | ✅ Tamamlandı |
| NavMesh kurulumu | [İsim] | ✅ Tamamlandı |
| UI tasarımı | [İsim] | ✅ Tamamlandı |
| Level design | [İsim] | ✅ Tamamlandı |
| Ses efektleri | [İsim] | ✅ Tamamlandı |
| Optimizasyon | [İsim] | ✅ Tamamlandı |

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 📧 İletişim

Proje ile ilgili sorularınız için ekip üyeleriyle iletişime geçebilirsiniz.

---

**Kocaeli Üniversitesi - Teknoloji Fakültesi**  
**Bilişim Sistemleri Mühendisliği**  
**2025-2026 Güz Dönemi**
