# Proje Analizi: Project YL

Bu dosya, "Project YL" adlı Unity projesinin yapısını ve temel bileşenlerini özetlemektedir.

## Genel Bakış

Proje, C# dilinde geliştirilmiş bir 3D Aksiyon RPG oyunudur. ScriptableObject tabanlı bir veri mimarisi ve Unity'nin yeni Input System'ini kullanmaktadır.

## Dizin Yapısı ve Önemli Dosyalar

- **`Assets/Scripts`**: Oyunun ana mantığını içeren C# scriptlerinin bulunduğu ana dizindir.
    - **`Scripts/Classes`**: Oyun içindeki temel varlıkların (yaratık, oyuncu, düşman, silah vb.) ana sınıflarını barındırır.
        - `Creature.cs`: Oyuncu ve düşmanlar için ortak özellikler (can, seviye, stat'ler) içeren soyut bir temel sınıftır.
        - `Player.cs`: Oyuncu karakterinin hareket, envanter ve saldırı gibi özel mantığını içerir. `CharacterConfigSO` ile yapılandırılır.
        - `Enemy.cs`: Düşmanların yapay zeka (takip etme vb.) ve davranışlarını yönetir. `EnemyConfigSO` ile yapılandırılır.
        - `Weapon.cs`: Silahlar için soyut bir temel sınıftır.
        - `RangedWeapon.cs`: Menzilli silahların (mermi ateşleme vb.) mantığını içerir.
        - `Projectile.cs`: Ateşlenen mermilerin hareketini ve hedefe isabet etme mantığını yönetir.
    - **`Scripts/ScriptableObjects`**: Oyun verilerini (karakter, düşman, silah istatistikleri) tutan ScriptableObject tanımlarını içerir. Bu yapı, koddan bağımsız olarak editör üzerinden veri yönetimini kolaylaştırır.
        - `CharacterConfigSO.cs`: Oyuncu karakterlerinin yapılandırma verilerini tutar.
        - `EnemyConfigSO.cs`: Düşman türlerinin yapılandırma verilerini tutar.
    - **`PlayerController.cs`**: Oyuncu hareketini yöneten bir diğer script. `Player.cs` içinde de hareket mantığı bulunduğundan, bu scriptin eski bir versiyon veya test amaçlı olabileceği düşünülmektedir.
    - **`CameraController.cs`**: Üçüncü şahıs (third-person) kamera kontrolünü sağlar.
    - **`WaveManager.cs`**: Zamana ve dalgalara göre düşmanların doğmasını yöneten sistemdir.

- **`Assets/Prefabs`**: Oyun içinde kullanılan önceden yapılandırılmış GameObjects (Prefab) burada bulunur.
    - `Player .prefab`: Oyuncu karakterinin ana prefab'ı. `PlayerController`, `PlayerInput` gibi bileşenleri içerir.
    - `Dummy.prefab`: Test amaçlı kullanılan basit bir düşman prefab'ı.
    - `Health Bar`: Karakterlerin üzerinde görünen can barlarının prefab'ını ve ilgili scriptleri içerir.
    - `projectile.prefab`: Menzilli silahlar tarafından kullanılan mermi prefab'ı.

- **`Assets/InputSystem_Actions.inputactions`**: Unity'nin yeni Input System'i için tanımlanmış olan aksiyonları (hareket, zıplama, saldırı vb.) içerir.

## Mimari ve Tasarım Notları

- **Veri Odaklı Tasarım**: Proje, oyun verilerini (karakter, düşman, silah özellikleri) ScriptableObject'ler aracılığıyla yöneterek esnek ve modüler bir yapı sunar. Bu, yeni içerik eklemeyi veya mevcut verileri dengelemeyi kolaylaştırır.
- **Kalıtım (Inheritance)**: `Creature` gibi temel sınıfların kullanılması, kod tekrarını azaltır ve hiyerarşik bir yapı sağlar.
- **Input System**: Girdi yönetimi için Unity'nin modern ve esnek yeni Input System'i tercih edilmiştir.
- **Zıplama Mekanizması**: Karakterin zıplama mekanizması, yere temasını kontrol etmek için `Physics.CheckSphere` kullanan bir yapıya dönüştürülmüştür. Bu, daha güvenilir ve esnek bir zemin kontrolü sağlar. Ayrıca, oyuncuya zıplaması için küçük bir ek süre tanıyan "coyote time" mekanizması eklenmiştir.
- **Potansiyel Kod Tekrarı**: `PlayerController.cs` ve `Player.cs` scriptlerinin her ikisinin de oyuncu hareketini kontrol etmesi, gelecekte birleştirme veya yeniden düzenleme (refactoring) gerektirebilecek bir duruma işaret etmektedir.
- **Geçici Görsel Efektler (Placeholder VFX)**: Alan hasarı (AOE) saldırıları için `RangedWeapon.cs` içinde geçici bir görsel efekt (`AnimateAreaOfEffect` metodu) oluşturulmuştur. Bu efekt, bir küre oluşturup onu anlık olarak büyüterek ve saydamlaştırarak çalışır. Gelecekte bu yapı, daha gelişmiş bir VFX prefabı ile kolayca değiştirilebilir.

## WaveManager Sistemi

Oyunun ana döngüsünü ve zorluğunu yönetecek olan `WaveManager` sistemi eklendi.

**Tamamlanan Adımlar:**

1.  **`WaveManager` Script'ini Oluşturma:**
    -   `[x]` `WaveManager.cs` adında yeni bir C# scripti oluşturuldu.
    -   `[x]` Script, `MonoBehaviour`'dan kalıtım alıyor ve **Singleton** tasarım desenini uyguluyor.
    -   `[x]` Temel dalga (`Wave`) verilerini (düşman türleri, süre, vb.) tutacak `struct` veya `class` yapıları oluşturuldu.
    -   `[x]` Inspector üzerinden kolayca ayarlanabilen bir dalga listesi (`List<Wave>`) eklendi.

2.  **Temel Dalga ve Spawn Mantığını Geliştirme:**
    -   `[x]` Zamanla bir sonraki dalgaya geçişi sağlayan temel mantık kuruldu.
    -   `[x]` Sahnedeki toplam düşman sayısını sınırlayan (`maxConcurrentEnemies`) bir kontrol mekanizması eklendi.

3.  **Gelişmiş Spawn Pozisyonu Belirleme:**
    -   `[x]` Düşmanların oyuncu etrafında dairesel bir alanda (`minSpawnRadius`, `maxSpawnRadius`) doğmasını sağlayan mantık geliştirildi.
    -   `[x]` **NavMesh.SamplePosition** ve **Raycast** kullanarak spawn pozisyonunun yürünebilir bir zeminde olup olmadığı kontrol edildi.
    -   `[x]` Oyuncunun kenarda, çukurda veya duvara yakın olma gibi istisnai durumları ele alan bir algoritma geliştirildi.

4.  **Düşman Türü Seçimi ve Optimizasyon:**
    -   `[x]` Her dalga için hangi düşmanın hangi olasılıkla doğacağını belirleyen ağırlıklı bir seçim sistemi kuruldu.
    -   `[x]` Düşmanları `Instantiate` etmek yerine, daha performanslı bir yöntem olan **Object Pooling** (Nesne Havuzlama) sistemi entegre edildi.

5.  **Elit Düşman Sistemini Ekleme:**
    -   `[x]` `Enemy.cs` scriptine `isElite` bayrağı ve elit statlarını yönetecek değişkenler eklendi.
    -   `[x]` `WaveManager`, düşmanları spawn ederken belirli bir şansla (`eliteChance`) onları elit olarak işaretliyor.
    -   `[x]` `Enemy.cs`'in `InitEnemyConfig` ve `Die` metodları, elit düşmanların daha fazla cana, hasara, daha büyük boyuta sahip olmasını ve öldüklerinde daha fazla XP/Gold ve şansla sandık düşürmesini sağlayacak şekilde güncellendi.

### WaveManager Geliştirme Fikirleri ve Potansiyel Adımlar

`WaveManager`'ın harici scriptler tarafından (örneğin bir oyun yöneticisi, bir bölüm tetikleyicisi) kontrol edilebilmesi için aşağıdaki adımlar izlenebilir:

1.  **Kontrol Değişkeni Eklemek:**
    *   `WaveManager.cs` içine `private bool isSpawningActive = false;` gibi bir kontrol değişkeni eklenebilir. `Update` metodunun başı, bu değişken `false` ise çalışmayı durduracak şekilde güncellenir (`if (!isSpawningActive) return;`).

2.  **Public Kontrol Metotları Oluşturmak:**
    *   **`public void StartSpawning()`:** Bu metod, `isSpawningActive` değişkenini `true` yapar ve zamanlayıcıları (`waveTimer`, `spawnTimer`) sıfırlayarak dalgaları başlatır.
    *   **`public void StopSpawning()`:** Bu metod, `isSpawningActive` değişkenini `false` yaparak `Update` döngüsünü ve dolayısıyla düşman üretimini durdurur.
    *   **`public void GoToNextWave()`:** Mevcut dalgayı anında bitirip bir sonrakine geçmek için kullanılabilir. Bu metod, `currentWaveIndex`'i artırır ve `waveTimer`'ı sıfırlar.
    *   **`public void ResetWaves()`:** Tüm dalgaları başa sarmak için `currentWaveIndex` ve zamanlayıcıları sıfırlar.

3.  **Harici Script'ten Çağırmak:**
    *   `WaveManager` bir **Singleton** olduğu için, herhangi bir script içerisinden ona kolayca erişilebilir.
    *   Örneğin, bir `GameManager` scripti, oyun başladığında veya oyuncu bir alana girdiğinde `WaveManager.Instance.StartSpawning();` komutunu çağırabilir.
    *   Benzer şekilde, bölüm bittiğinde veya oyun duraklatıldığında `WaveManager.Instance.StopSpawning();` çağrılabilir.

Bu yapı, `WaveManager`'ı mevcut otomatik çalışma mantığından çıkarıp, oyunun genel akışına daha entegre ve esnek bir hale getirecektir.

## Oyun Planı: Temel Oynanış

- **Başlangıç**: Karakter, bir başlangıç noktasında (spawn point) üçüncü şahıs bakış açısıyla (TPS) oyuna başlar. Karakterin sınıfına göre bir başlangıç silahı bulunur.
- **Savaş ve Gelişim**: Oyuncu, çevrede doğan düşmanları silahını kullanarak öldürür. Ölen düşmanlardan tecrübe puanı (XP) ve altın düşer. Yeterli tecrübe puanı toplayan oyuncu seviye atlar.
- **Ekonomi ve Eşyalar**: Altın, haritada bulunan veya elit düşmanlardan düşen sandıkları açmak için kullanılır. Bu sandıklardan, oyuncunun 'şans' istatistiğine bağlı olarak çeşitli nadirlikte özel eşyalar çıkar.
- **Zaman ve Zorluk**: Oyunda bir dakika sistemi bulunur. Süre azaldıkça düşmanlar güçlenir ve sayıları artar. Belirli aralıklarla düşman sürülerinin ve Boss'ların ortaya çıktığı olaylar yaşanır.
- **Boss Savaşları**: Boss'lar, kesildiklerinde kesin olarak sandık düşürürler. Oyuncu, haritadaki "Boss Portalı"nı bularak istediği zaman Boss'u çağırabilir. Portal, ilgili Boss kesilmeden aktif hale gelmez.
- **Oyun Sonu**: Oyunu bitirmek için Boss kesildikten sonra aktifleşen portaldan geçmek gerekir. Oyuncu, süre bitmeden de portaldan geçebilir.
- **Sonsuz Mod (Endgame)**: Süre bittiğinde, çok daha güçlü ve hızlı "Özel Düşmanlar" doğmaya başlar. Bu düşmanlar zamanla daha da güçlenir. Oyunun bu aşamasındaki temel amaç, mümkün olduğunca uzun süre hayatta kalmak ve en yüksek skoru (en çok düşmanı öldürmek) yapmaktır.
