using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    // 通用的组件事件
    public class UpdateAction : MonoBehaviour
    {
        public System.Action<UpdateAction> FixedUpdateAction;
        private void FixedUpdate()
        {
            if (FixedUpdateAction != null)
                FixedUpdateAction(this);
        }
        public System.Action<UpdateAction> LateUpdateAction;
        private void LateUpdate()
        {
            if (LateUpdateAction != null)
                LateUpdateAction(this);
        }

        public System.Action<UpdateAction> _UpdateAction;
        private void Update()
        {
            if (_UpdateAction != null)
                _UpdateAction(this);
        }
    }
}