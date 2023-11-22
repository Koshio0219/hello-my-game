﻿using Cysharp.Threading.Tasks;
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
        public Vector3 CenterPoint { get; set; }

        // The size of the build bounds
        public Vector3 size = new Vector3(80.0f, 20.0f, 80.0f);

        private NavMeshData mNavMesh;
        private NavMeshDataInstance mInstance;
        private List<NavMeshBuildSource> mSources = new List<NavMeshBuildSource>();

        public void Build()
        {
            mInstance.Remove();
            mNavMesh = new NavMeshData();
            mInstance = NavMesh.AddNavMeshData(mNavMesh);
            if (tracked == null)
                CenterPoint = transform.position;

            BuildMesh();
        }

        protected virtual void BuildMesh()
        {
            if (!isActiveAndEnabled) return;
            UpdateNavMesh().Forget();
        }

        protected virtual void OnEnable()
        {
            Build();
        }

        protected virtual void OnDestroy()
        {
            // Unload navmesh and clear handle
            mInstance.Remove();
        }

        protected virtual async UniTaskVoid UpdateNavMesh()
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
            var center = tracked ? tracked.position : CenterPoint;
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
            var center = tracked ? tracked.position : CenterPoint;
            Gizmos.DrawWireCube(center, size);
        }
    }
}