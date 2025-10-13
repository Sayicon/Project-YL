# 🎯 Yapay Zeka Destekli TPS (Third Person Shooter) Oyunu

Bu proje, **Kocaeli Üniversitesi Teknoloji Fakültesi Bilişim Sistemleri Mühendisliği** bölümü  
**2025-2026 Güz Dönemi Yazılım Geliştirme Laboratuvarı – I** dersi kapsamında geliştirilmiştir.  

Amaç: TPS (Third Person Shooter) oyun türünün temel mekaniklerini içeren, yapay zekâ destekli bir NPC (Non-Player Character) sistemine sahip bir oyun geliştirmektir.  

---

## 👥 Proje Ekibi
| Ad Soyad | Öğrenci No | Görevi |
|-----------|------------|--------|
| Kerem Çekici | ... | Yapay zekâ & karakter kontrol sistemleri |
| ... | ... | Oyun mekaniği & level tasarımı |
| ... | ... | UI/UX & optimizasyon |

---

## 🧩 Proje Tanımı

Oyuncunun üçüncü şahıs bakış açısından yönettiği bir karakterle, düşman NPC’lerle savaştığı aksiyon tabanlı bir oyundur.  
Oyuncu; koşma, zıplama, nişan alma, ateş etme ve siper alma gibi temel hareketleri gerçekleştirebilir.  
Düşman NPC’ler **FSM (Finite State Machine)** tabanlı yapay zekâ ile hareket eder.

### 🎮 Temel Özellikler
- Üçüncü şahıs kamera sistemi (TPS)
- Karakter hareket mekanikleri (koşma, zıplama, siper alma)
- Silah ve mermi sistemi
- NPC davranışları:
  - **Idle:** NPC hareketsiz bekler  
  - **Patrol:** Devriye gezme  
  - **Chase:** Oyuncuyu fark edip kovalamaya başlama  
  - **Attack:** Oyuncuya saldırma
- **Pathfinding (Yol Bulma):** Unity `NavMesh Agent` kullanılarak NPC’lerin oyuncuya en kısa yoldan ulaşması
- Basit sağlık (health) ve hasar (damage) sistemi
- Oyun sahnesi (tek level veya çoklu level desteği)
- Düşük poligonlu (low-poly) model optimizasyonu

---

## 🧠 Kullanılan Teknolojiler

| Teknoloji | Kullanım Amacı |
|------------|----------------|
| **Unity** | Oyun motoru |
| **C#** | Oyun ve AI kodlaması |
| **NavMesh / NavMesh Agent** | Yol bulma (pathfinding) sistemi |
| **FSM (Finite State Machine)** | NPC durum yönetimi |
| **GitHub** | Sürüm ve ekip yönetimi |
| **Blender / Asset Store** | 3D model ve sahne tasarımı |

---

## 🧩 Oyun Mekanikleri ve Yapay Zekâ Diyagramı

```mermaid
stateDiagram-v2
    [*] --> Idle
    Idle --> Patrol: Süre doldu
    Patrol --> Chase: Oyuncu algılandı
    Chase --> Attack: Oyuncu menzilde
    Attack --> Chase: Oyuncu uzaklaştı
    Chase --> Patrol: Oyuncu kayboldu
    Patrol --> Idle: Devriye tamamlandı
