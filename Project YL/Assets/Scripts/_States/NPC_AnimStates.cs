// Dosya Yolu: Assets/Scripts/_States/NPC_AnimStates.cs
using UnityEngine;

namespace _States
{
    // NPC'nin "Durma" durumu
    public class NPC_IdleState : HeroAnimState
    {
        public NPC_IdleState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.CrossFade("idle", 0.1f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // NPC'nin "Yürüme" durumu
    public class NPC_WalkState : HeroAnimState
    {
        public NPC_WalkState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.CrossFade("walking", 0.1f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // NPC'nin "Koşma" durumu
    public class NPC_RunState : HeroAnimState
    {
        public NPC_RunState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.CrossFade("run", 0.1f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // NPC'nin "Saldırı" durumu
    public class NPC_AttackState : HeroAnimState
    {
        public NPC_AttackState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.Play("attack", 0, 0f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // NPC'nin "Ölme" durumu
    public class NPC_DieState : HeroAnimState
    {
        public NPC_DieState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.Play("die", 0, 0f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    // NPC'nin "Sola Dönme" durumu
    public class NPC_TurnLeftState : HeroAnimState
    {
        public NPC_TurnLeftState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.Play("degree_left", 0, 0f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    // NPC'nin "Sağa Dönme" durumu
    public class NPC_TurnRightState : HeroAnimState
    {
        public NPC_TurnRightState(Animator animator) : base(animator) { }
        public override void OnEnter() { Animator.Play("degree_right", 0, 0f); }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
}