﻿using Cysharp.Threading.Tasks;
using Game.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Navigation
{
    [DefaultExecutionOrder(-102)]
    public class RuntimeNavMeshBuilder : MonoBehaviour
    {
        // The center of the build
        public Transform tracked;
        public Vector3 CenterPoint
        {
            get 
            {
                if (tracked == null)
                    return transform.position;
                return tracked.position;
            }
        }

        // The size of the build bounds
        public Vector3 size = new(80.0f, 20.0f, 80.0f);

        private NavMeshData mNavMesh;
        private NavMeshDataInstance mInstance;
        private List<NavMeshBuildSource> mSources = new List<NavMeshBuildSource>();

        public async UniTask Build()
        {
            mInstance.Remove();
            mNavMesh = new NavMeshData();
            mInstance = NavMesh.AddNavMeshData(mNavMesh);
            //if (tracked == null)
            //    CenterPoint = transform.position;

            await BuildMesh();
        }

        protected virtual async UniTask BuildMesh()
        {
            if (!isActiveAndEnabled) return;
            await UpdateNavMesh();
        }

#if DEBUG_MODE
        protected virtual void OnEnable()
        {
            Build();
        }
#endif
        private void Awake()
        {
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
            EventQueueSystem.AddListener<UpdateNavMeshEvent>(UpdateNavMeshHandler);
        }

        private void UpdateNavMeshHandler(UpdateNavMeshEvent e)
        {
            Build().Forget();
        }

        private async void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to != Manager.StageStates.NavMeshBuildStart) return;
            await Build();
            EventQueueSystem.QueueEvent(new StageStatesEvent(Manager.StageStates.NavMeshBuildEnd));
        }

        protected virtual void OnDestroy()
        {
            // Unload navmesh and clear handle
            mInstance.Remove();
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
            EventQueueSystem.RemoveListener<UpdateNavMeshEvent>(UpdateNavMeshHandler);
        }

        protected virtual async UniTask UpdateNavMesh()
        {
            NavMeshObject.Collect(ref mSources);
            var settings = NavMesh.GetSettingsByID(0);
            var bounds = QuantizedBounds();
            await NavMeshBuilder.UpdateNavMeshDataAsync(mNavMesh, settings, mSources, bounds).ToUniTask();
        }

        protected static Vector3 Quantize(Vector3 v, Vector3 quant)
        {
            float x = quant.x * Mathf.Floor(v.x / quant.x);
            float y = quant.y * Mathf.Floor(v.y / quant.y);
            float z = quant.z * Mathf.Floor(v.z / quant.z);
            return new Vector3(x, y, z);
        }

        protected Bounds QuantizedBounds()
        {
            // Quantize the bounds to update only when theres a 0.1% change in size
            var center =  CenterPoint;
            return new Bounds(Quantize(center, .001f * size), size);
        }

        void OnDrawGizmosSelected()
        {
            if (mNavMesh)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(mNavMesh.sourceBounds.center, mNavMesh.sourceBounds.size);
            }

            Gizmos.color = Color.yellow;
            var bounds = QuantizedBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.color = Color.green;
            var center =  CenterPoint;
            Gizmos.DrawWireCube(center, size);
        }
    }
}