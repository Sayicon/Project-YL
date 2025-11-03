// Dosya Yolu: Assets/Scripts/_Controllers/NPC_AnimationsControl.cs
using System;
using _States; // Paylaşılan HeroAnimState'i kullanmak için
using UnityEngine;

namespace _Controllers
{
    // Bu script NPC GameObject'ine takılacak
    public class NPC_AnimationsControl : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private HeroAnimStateController _heroAnimStateController; 

        // NPC'nin tüm state'leri
        private HeroAnimState _idleState;
        private HeroAnimState _walkState;
        private HeroAnimState _runState;
        private HeroAnimState _attackState;
        private HeroAnimState _dieState;
        private HeroAnimState _turnLeftState;
        private HeroAnimState _turnRightState;
        
        // NPC'nin beyninin ("NPC_Controller") anlayabileceği komutlar
        private enum NPC_AnimationsEnum 
        { 
            Idle, 
            Walk, 
            Run, 
            Attack, 
            Die, 
            TurnLeft, 
            TurnRight 
        }

        private void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            
            // Player'ın kullandığı HeroAnimStateController'ı kullanıyoruz
            _heroAnimStateController = new HeroAnimStateController(animator); 
            
            // NPC'nin animasyon state'lerini oluşturuyoruz
            _idleState = new NPC_IdleState(animator);
            _walkState = new NPC_WalkState(animator);
            _runState = new NPC_RunState(animator);
            _attackState = new NPC_AttackState(animator);
            _dieState = new NPC_DieState(animator);
            _turnLeftState = new NPC_TurnLeftState(animator);
            _turnRightState = new NPC_TurnRightState(animator);
        }
        
        // Bu metot, HeroAnimStateController'a hangi state'e geçeceğini söyler
        private void ChangeAnimation(NPC_AnimationsEnum expression)
        {
            switch (expression)
            {
                case NPC_AnimationsEnum.Idle: _heroAnimStateController.ChangeState(_idleState); break;
                case NPC_AnimationsEnum.Walk: _heroAnimStateController.ChangeState(_walkState); break;
                case NPC_AnimationsEnum.Run: _heroAnimStateController.ChangeState(_runState); break;
                case NPC_AnimationsEnum.Attack: _heroAnimStateController.ChangeState(_attackState); break;
                case NPC_AnimationsEnum.Die: _heroAnimStateController.ChangeState(_dieState); break;
                case NPC_AnimationsEnum.TurnLeft: _heroAnimStateController.ChangeState(_turnLeftState); break;
                case NPC_AnimationsEnum.TurnRight: _heroAnimStateController.ChangeState(_turnRightState); break;
            }
        }

        // === PUBLIC KOMUTLAR (NPC'nin beyni bu komutları çağıracak) ===
        public void IdleAnim() { ChangeAnimation(NPC_AnimationsEnum.Idle); }
        public void WalkAnim() { ChangeAnimation(NPC_AnimationsEnum.Walk); }
        public void RunAnim() { ChangeAnimation(NPC_AnimationsEnum.Run); }
        public void AttackAnim() { ChangeAnimation(NPC_AnimationsEnum.Attack); }
        public void DieAnim() { ChangeAnimation(NPC_AnimationsEnum.Die); }
        public void TurnLeftAnim() { ChangeAnimation(NPC_AnimationsEnum.TurnLeft); }
        public void TurnRightAnim() { ChangeAnimation(NPC_AnimationsEnum.TurnRight); }
    }
}