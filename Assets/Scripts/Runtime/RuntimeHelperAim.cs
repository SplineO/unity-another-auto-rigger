﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeHelperAim : DebugSetter
    {
        // settings
        public string target;
        public Transform targetTransform;
        public int _blockMultiplier = 1;

        // debug
        public override float DebugSize { get { return 0.005f; } }
        public override Color DebugColor { get { return Color.yellow; } }

        // private
        private bool isValid;
        private Vector3 xDirection = new Vector3(1, 0, 0);

        void Awake()
        {
            // populate transforms
            if (targetTransform == null)
                targetTransform = this.GetComponentInGameObjectFromString<Transform>(target);

            // validate
            isValid = (targetTransform == null) ? false : true;
        }

        void LateUpdate()
        {
            // only continue when helper is valid
            if (!isValid)
                return;

            // get parent x vector
            Transform parentTransform = transform.parent;
            Matrix4x4 parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.localScale);
            Vector3 xVectorParent = parentMatrix.MultiplyVector(xDirection);

            // get y vector
            Vector3 yVector = (targetTransform.position - transform.position).normalized * _blockMultiplier;

            // get z vector
            Vector3 zVector = Vector3.Cross(xVectorParent, yVector);

            // set rotation
            transform.rotation = Quaternion.LookRotation(zVector, yVector);
        }
    }
}
