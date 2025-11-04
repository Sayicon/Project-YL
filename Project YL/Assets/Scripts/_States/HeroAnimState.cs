using UnityEngine;

namespace _States
{
    public abstract class HeroAnimState
    {
        protected readonly Animator Animator; // Animator erişimi

        protected HeroAnimState(Animator animator)
        {
            this.Animator = animator;
        }

        public abstract void OnEnter(); 
        
        public abstract void OnUpdate(); 
        
        public abstract void OnExit(); 
    }
}