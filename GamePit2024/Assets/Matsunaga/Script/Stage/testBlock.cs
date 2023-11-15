using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class testBlock : MonoBehaviour
    {//https://kurokumasoft.com/2022/06/07/unity-moving-platform/
        [Header("必要なコンポーネントを登録")]
        [SerializeField]
        Rigidbody rigidBody = null;
        private Vector3 NoApproachField;
        List<Rigidbody> rigidBodies = new List<Rigidbody>();
        [SerializeField] private Game.Base.BlockBaseType blockBaseType = Base.BlockBaseType.Null;

        private Vector3 _prevPosition;
        private Vector3 _velocity;

        private void Start()
        {
            _prevPosition = transform.position;
            _velocity = Vector3.zero;
            NoApproachField = Vector3.zero;
        }

        public Vector3 getNoApproachField()
        {
            return NoApproachField;
        }

        public Game.Base.BlockBaseType GetBlockBaseType()
        {
            return blockBaseType;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
            {
                NoApproachField = collision.gameObject.transform.position;
            }
        }

       

        private void OnCollisionExit(Collision collision)
        {
            NoApproachField = Vector3.zero;
        }

        private void Update()
        {
            Vector3 position = transform.position;
            _velocity = (position - _prevPosition) / Time.deltaTime;

            // 前フレーム位置を更新
            _prevPosition = position;
        }

        void FixedUpdate()
        {
            AddVelocity();
        }

        void OnTriggerEnter(Collider other)
        {
            Rigidbody rd = other.gameObject.GetComponent<Rigidbody>();
            if (rd == null) return;
            rigidBodies.Add(rd);
            if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
            {
                NoApproachField = other.gameObject.transform.position;
            }
        }

        void OnTriggerExit(Collider other)
        {
            rigidBodies.Remove(other.gameObject.GetComponent<Rigidbody>());
            if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
            {
                NoApproachField = Vector3.zero;
            }
        }

        void AddVelocity()
        {

            // 現在速度計算
            //Vector3 velocity = (position - _prevPosition) / Time.deltaTime;
            //Vector3 velocity = rigidBody.GetPointVelocity(Vector3.zero);
            Vector3 velocity = rigidBody.velocity;
            Debug.Log("Velocity: " + velocity);
            if (velocity.sqrMagnitude <= 0.01f)
            {
                return;
            }

            for (int i = 0; i < rigidBodies.Count; i++)
            {
                rigidBodies[i].velocity += velocity * Time.deltaTime;
                //rigidBodies[i].AddForce(velocity);
                //rigidBodies[i].position = rigidBodies[i].position + velocity * Time.deltaTime;
                //rigidBodies[i].MovePosition(rigidBodies[i].position + _velocity * Time.deltaTime);
            }
            
        }
    }
}
