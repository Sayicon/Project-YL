// Dosya Yolu: Assets/Scripts/_States/HeroAnimStateController.cs
using UnityEngine;

namespace _States // <-- Hatanın çözümü için bu namespace çok önemli!
{
    // Animasyon durumlarını (State) yöneten ana kontrolcü
    public class HeroAnimStateController
    {
        // ==> Components
        private HeroAnimState _currentState; // Mevcut aktif state

        // Kontrolcü başladığında, başlangıç state'i olarak Idle'ı ata
        public HeroAnimStateController(Animator animator)
        {
            // Başlangıçta IdleAnimState'i oluştur ve onu mevcut state yap
            _currentState = new IdleAnimState(animator);
            _currentState.OnEnter();
        }

        // ==> Main Functions
        // Durumu (State) değiştirmek için bu metot kullanılır
        public void ChangeState(HeroAnimState newState)
        {
            // Eğer yeni state zaten mevcut state ise, hiçbir şey yapma
            if (_currentState == newState)
                return;

            // 1. Mevcut state'ten çıkış yap
            _currentState.OnExit();
            // 2. Yeni state'i mevcut state olarak ata
            _currentState = newState;
            // 3. Yeni state'e giriş yap
            _currentState.OnEnter();
        }

        // Mevcut state'in Update'ini çalıştır (gerekirse)
        public void Update()
        {
            _currentState.OnUpdate();
        }
    }
}