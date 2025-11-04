using UnityEngine;

namespace _States 
{
    public class HeroAnimStateController
    {
        private HeroAnimState _currentState; 

        public HeroAnimStateController(Animator animator)
        {
            // Başlangıçta için idle kullanılıyor
            _currentState = new IdleAnimState(animator);
            _currentState.OnEnter();
        }

        // Durumu değiştirmek için bu metot kullanılıyor
        public void ChangeState(HeroAnimState newState)
        {
            // Eğer yeni state zaten mevcut state ise değişiklik olmuyor
            if (_currentState == newState)
                return;

            // state den çıkış yapar
            _currentState.OnExit();
            // yeni state i alır
            _currentState = newState;
            // yeni state e girer
            _currentState.OnEnter();
        }
        public void Update()
        {
            _currentState.OnUpdate();
        }
    }
}