// Dosya yolu: Assets/Scripts/_States/AnimStates.cs
using UnityEngine;

namespace _States
{
    // IDLE (Durma) STATE
    public class IdleAnimState : HeroAnimState
    {
        public IdleAnimState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            // "idle" durumuna 0.1 saniyelik yumuşak geçiş yapar
            Animator.CrossFade("idle", 0.1f); 
        }

        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // "WalkAnimState" adını "ForwardWalkAnimState" olarak değiştirdik
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

    // YENİ: GERİ YÜRÜME STATE'İ
    public class BackwardWalkAnimState : HeroAnimState
    {
        public BackwardWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            // Animator'de "backward walking" adında bir state olmalı
            Animator.CrossFade("ayakta_geri_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // YENİ: SOL YÜRÜME (STRAFE) STATE'İ
    public class LeftWalkAnimState : HeroAnimState
    {
        public LeftWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            // Animator'de "left strafe" adında bir state olmalı
            Animator.CrossFade("ayakta_sola_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // YENİ: SAĞ YÜRÜME (STRAFE) STATE'İ
    public class RightWalkAnimState : HeroAnimState
    {
        public RightWalkAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            // Animator'de "right strafe" adında bir state olmalı
            Animator.CrossFade("ayakta_saga_yürüme", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // İLERİ KOŞMA STATE'İ
    public class ForwardRunAnimState : HeroAnimState
    {
        public ForwardRunAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            // ÖNEMLİ: Animator'deki state adını "ayakta_ileri_kosma" ile değiştirin
            Animator.CrossFade("ayakta_ileri_kosma", 0.1f);
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // GERİ KOŞMA STATE'İ
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

    // SOL KOŞMA (STRAFE) STATE'İ
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

    // SAĞ KOŞMA (STRAFE) STATE'İ
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
            // Zıplama animasyonları genellikle başa sarılmalı ve kesilmemelidir.
            // Bu yüzden "CrossFade" yerine "Play" kullanmak daha iyi olabilir.
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
    
    // Bu, "nişan alma" (geçiş) animasyonudur
    public class StartAimingAnimState : HeroAnimState
    {
        public StartAimingAnimState(Animator animator) : base(animator) { }
        public override void OnEnter() 
        { 
            // Bu animasyon kesilmemeli ve baştan başlamalı
            Animator.Play("ayakta_nisan_alma", 0, 0f); // 0. katman, baştan başla
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }

    // Bu, "nişanda bekleme" (idle) animasyonudur
    public class AimIdleAnimState : HeroAnimState
    {
        public AimIdleAnimState(Animator animator) : base(animator) { }
        public override void OnEnter()
        {
            // Bu animasyona yumuşak geçiş yapılabilir
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
            // Atış animasyonu kesilmemeli ve baştan başlamalı
            Animator.Play("ok_at", 0, 0f); // 0. katman, baştan başla
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