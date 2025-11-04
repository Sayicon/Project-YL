using System;
using _States; 
using UnityEngine;

namespace _Controllers
{

    public enum AnimType 
    {
        Forward,
        Backward,
        Left,
        Right
    }
    public class AnimationsControl : MonoBehaviour
    {
        [SerializeField] private Animator animator; 

        private HeroAnimStateController _heroAnimStateController; 
        
        private HeroAnimState _idleAnimState;
        private HeroAnimState _forwardWalkAnimState;
        private HeroAnimState _backwardWalkAnimState; 
        private HeroAnimState _leftWalkAnimState;     
        private HeroAnimState _rightWalkAnimState;    

        private HeroAnimState _forwardRunAnimState;
        private HeroAnimState _backwardRunAnimState;
        private HeroAnimState _leftRunAnimState;
        private HeroAnimState _rightRunAnimState;

        private HeroAnimState _normalJumpAnimState;
        private HeroAnimState _runningJumpAnimState;

        private HeroAnimState _startAimingAnimState;
        private HeroAnimState _aimIdleAnimState;
        private HeroAnimState _shootAnimState;

        private HeroAnimState _aimForwardWalkAnimState;
        private HeroAnimState _aimBackwardWalkAnimState;
        private HeroAnimState _aimLeftWalkAnimState;
        private HeroAnimState _aimRightWalkAnimState;

        private enum AnimationsEnum : byte
        {
            Idle,
            ForwardWalk,
            BackwardWalk,
            LeftWalk,
            RightWalk,
            ForwardRun,
            BackwardRun,
            LeftRun,
            RightRun,
            NormalJump,
            RunningJump,
            StartAiming,
            AimIdle,
            Shoot,
            AimForwardWalk,
            AimBackwardWalk,
            AimLeftWalk,
            AimRightWalk
        }

        private void Awake()
        {
            if (animator == null)
            {
                Debug.LogError("AnimationsControl içerisinde Animator atanmamış");
                return;
            }
            
            _heroAnimStateController = new HeroAnimStateController(animator);
            
            _idleAnimState = new IdleAnimState(animator);
            _forwardWalkAnimState = new ForwardWalkAnimState(animator);
            _backwardWalkAnimState = new BackwardWalkAnimState(animator); 
            _leftWalkAnimState = new LeftWalkAnimState(animator);         
            _rightWalkAnimState = new RightWalkAnimState(animator);       
            
            _forwardRunAnimState = new ForwardRunAnimState(animator);
            _backwardRunAnimState = new BackwardRunAnimState(animator);
            _leftRunAnimState = new LeftRunAnimState(animator);
            _rightRunAnimState = new RightRunAnimState(animator);

            _normalJumpAnimState = new NormalJumpAnimState(animator);
            _runningJumpAnimState = new RunningJumpAnimState(animator);

            _startAimingAnimState = new StartAimingAnimState(animator);
            _aimIdleAnimState = new AimIdleAnimState(animator);
            _shootAnimState = new ShootAnimState(animator);
            _aimForwardWalkAnimState = new AimForwardWalkAnimState(animator);
            _aimBackwardWalkAnimState = new AimBackwardWalkAnimState(animator);
            _aimLeftWalkAnimState = new AimLeftWalkAnimState(animator);
            _aimRightWalkAnimState = new AimRightWalkAnimState(animator);
            
        }
        
        private void AnimatoinsController(AnimationsEnum expression)
        {
            switch (expression)
            {
                case AnimationsEnum.Idle:
                    _heroAnimStateController.ChangeState(_idleAnimState);
                    break;
                case AnimationsEnum.ForwardWalk:
                    _heroAnimStateController.ChangeState(_forwardWalkAnimState);
                    break;
                case AnimationsEnum.BackwardWalk: 
                    _heroAnimStateController.ChangeState(_backwardWalkAnimState);
                    break;
                case AnimationsEnum.LeftWalk: 
                    _heroAnimStateController.ChangeState(_leftWalkAnimState);
                    break;
                case AnimationsEnum.RightWalk: 
                    _heroAnimStateController.ChangeState(_rightWalkAnimState);
                    break;
                
                // YENİ Case'ler
                case AnimationsEnum.ForwardRun:
                    _heroAnimStateController.ChangeState(_forwardRunAnimState);
                    break;
                case AnimationsEnum.BackwardRun:
                    _heroAnimStateController.ChangeState(_backwardRunAnimState);
                    break;
                case AnimationsEnum.LeftRun:
                    _heroAnimStateController.ChangeState(_leftRunAnimState);
                    break;
                case AnimationsEnum.RightRun:
                    _heroAnimStateController.ChangeState(_rightRunAnimState);
                    break;
                case AnimationsEnum.NormalJump:
                    _heroAnimStateController.ChangeState(_normalJumpAnimState);
                    break;
                case AnimationsEnum.RunningJump:
                    _heroAnimStateController.ChangeState(_runningJumpAnimState);
                    break;
                case AnimationsEnum.StartAiming:
                    _heroAnimStateController.ChangeState(_startAimingAnimState);
                    break;
                case AnimationsEnum.AimIdle:
                    _heroAnimStateController.ChangeState(_aimIdleAnimState);
                    break;
                case AnimationsEnum.Shoot:
                    _heroAnimStateController.ChangeState(_shootAnimState);
                    break;
                case AnimationsEnum.AimForwardWalk:
                    _heroAnimStateController.ChangeState(_aimForwardWalkAnimState);
                    break;
                case AnimationsEnum.AimBackwardWalk:
                    _heroAnimStateController.ChangeState(_aimBackwardWalkAnimState);
                    break;
                case AnimationsEnum.AimLeftWalk:
                    _heroAnimStateController.ChangeState(_aimLeftWalkAnimState);
                    break;
                case AnimationsEnum.AimRightWalk:
                    _heroAnimStateController.ChangeState(_aimRightWalkAnimState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression), expression, null);
            }
        }
        
        public void IdleAnim()
        {
            AnimatoinsController(AnimationsEnum.Idle);
        }
        
        public void ChangeAnim(bool isRunning, AnimType type)
        {
            if (isRunning)
            {
                switch (type)
                {
                    case AnimType.Forward: AnimatoinsController(AnimationsEnum.ForwardRun); break;
                    case AnimType.Backward: AnimatoinsController(AnimationsEnum.BackwardRun); break;
                    case AnimType.Left: AnimatoinsController(AnimationsEnum.LeftRun); break;
                    case AnimType.Right: AnimatoinsController(AnimationsEnum.RightRun); break;
                }
            }
            else 
            {
                switch (type)
                {
                    case AnimType.Forward: AnimatoinsController(AnimationsEnum.ForwardWalk); break;
                    case AnimType.Backward: AnimatoinsController(AnimationsEnum.BackwardWalk); break;
                    case AnimType.Left: AnimatoinsController(AnimationsEnum.LeftWalk); break;
                    case AnimType.Right: AnimatoinsController(AnimationsEnum.RightWalk); break;
                }
            }
        }
        
        public void NormalJumpAnim()
        {
            AnimatoinsController(AnimationsEnum.NormalJump);
        }

        public void RunningJumpAnim()
        {
            AnimatoinsController(AnimationsEnum.RunningJump);
        }
        public void StartAimingAnim()
        {
            AnimatoinsController(AnimationsEnum.StartAiming);
        }

        public void AimIdleAnim()
        {
            AnimatoinsController(AnimationsEnum.AimIdle);
        }
        public void ShootAnim()
        {
            AnimatoinsController(AnimationsEnum.Shoot);
        }
        public void AimForwardWalkAnim()
        {
            AnimatoinsController(AnimationsEnum.AimForwardWalk);
        }
        public void AimBackwardWalkAnim()
        {
            AnimatoinsController(AnimationsEnum.AimBackwardWalk);
        }
        public void AimLeftWalkAnim()
        {
            AnimatoinsController(AnimationsEnum.AimLeftWalk);
        }
        public void AimRightWalkAnim()
        {
            AnimatoinsController(AnimationsEnum.AimRightWalk);
        }
        
    }
}