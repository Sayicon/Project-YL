// Dosya Yolu: Assets/Scripts/_States/HeroAnimState.cs
using UnityEngine;

namespace _States
{
    // Tüm animasyon state'lerinin uyması gereken temel şablon
    public abstract class HeroAnimState
    {
        // ==> Components
        protected readonly Animator Animator; // Animator'a erişim

        protected HeroAnimState(Animator animator)
        {
            this.Animator = animator;
        }

        // ==> Abstract Functions
        // Bir State'e girildiğinde çalışacak metot
        public abstract void OnEnter(); 
        
        // Bir State aktifken her frame çalışacak metot (Şimdilik kullanmayacağız)
        public abstract void OnUpdate(); 
        
        // Bir State'ten çıkıldığında çalışacak metot
        public abstract void OnExit(); 
    }
}