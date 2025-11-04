using UnityEngine;

namespace _States
{
    public class IdleAnimState : HeroAnimState
    {
        public IdleAnimState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            Animator.CrossFade("idle", 0.1f); 
        }

        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class ForwardWalkAnimState : HeroAnimState
    {
        public ForwardWalkAnimState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_ileri_yürüme", 0.1f);
        }

        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class BackwardWalkAnimState : HeroAnimState
    {
        public BackwardWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_geri_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class LeftWalkAnimState : HeroAnimState
    {
        public LeftWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_sola_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class RightWalkAnimState : HeroAnimState
    {
        public RightWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_saga_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class ForwardRunAnimState : HeroAnimState
    {
        public ForwardRunAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_ileri_kosma", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class BackwardRunAnimState : HeroAnimState
    {
        public BackwardRunAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_geri_kosma", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class LeftRunAnimState : HeroAnimState
    {
        public LeftRunAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_sola_kosma", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class RightRunAnimState : HeroAnimState
    {
        public RightRunAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_saga_kosma", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class NormalJumpAnimState : HeroAnimState
    {
        public NormalJumpAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.Play("ziplama"); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class RunningJumpAnimState : HeroAnimState
    {
        public RunningJumpAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.Play("kosarken_ziplama");
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    public class StartAimingAnimState : HeroAnimState
    {
        public StartAimingAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.Play("ayakta_nisan_alma", 0, 0f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class AimIdleAnimState : HeroAnimState
    {
        public AimIdleAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.CrossFade("ayakta_nisanda_bekleme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    public class ShootAnimState : HeroAnimState
    {
        public ShootAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            Animator.Play("ok_at", 0, 0f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    public class AimForwardWalkAnimState : HeroAnimState
    {
        public AimForwardWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.CrossFade("ayakta_nisanli_ileri_yurume", 0.1f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    public class AimBackwardWalkAnimState : HeroAnimState
    {
        public AimBackwardWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.CrossFade("ayakta_nisanli_geri_yurume", 0.1f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    public class AimLeftWalkAnimState : HeroAnimState
    {
        public AimLeftWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.CrossFade("ayakta_nisanli_sola_yurume", 0.1f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    
    public class AimRightWalkAnimState : HeroAnimState
    {
        public AimRightWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            Animator.CrossFade("ayakta_nisanli_saga_yurume", 0.1f); 
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
}