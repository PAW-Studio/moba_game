using Fusion;

namespace Animations.AnimatorNetworkMecanimAnimator
{
    public class Player_NetworkMecanimAnimator : NetworkBehaviour
    {
        // PRIVATE MEMBERS


        private NetworkMecanimAnimator _networkAnimator;

        // NetworkBehaviour INTERFACE

        public override void FixedUpdateNetwork()
        {
            if (IsProxy == true)
                return;

            if (Runner.IsForward == false)
                return;

            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                var dir = data.direction.normalized;

                if (dir.sqrMagnitude != 0)
                {

                    _networkAnimator.Animator.SetBool("run", true);

                }
                else
                {

                    _networkAnimator.Animator.SetBool("run", false);
                }
            }
        }

        // MONOBEHAVIOUR

        protected void Awake()
        {

            _networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();
        }
    }
}
