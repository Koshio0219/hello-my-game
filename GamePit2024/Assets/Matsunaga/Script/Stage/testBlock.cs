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
        List<GameObject> collisionObject = new List<GameObject>();
        [SerializeField] private Game.Base.BlockBaseType blockBaseType = Base.BlockBaseType.Null;

        private Vector3 _prevPosition;
        private Vector3 _nowPosition;
        private Vector3 _velocity;
        [Header("基本設定")]
        [SerializeField]
        Vector3 speed = Vector3.zero;
        private void Start()
        {
            _nowPosition = transform.position;
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

        /*private void OnCollisionEnter(Collision collision)
        {
            if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
            {
                NoApproachField = collision.gameObject.transform.position;
            }
        }

       

        private void OnCollisionExit(Collision collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
            {
                NoApproachField = Vector3.zero;
            }
        }*/

        private void Update()
        {
            //_nowPosition = transform.position;
            //AddVelocity();
            //MoveWith();
            //_prevPosition = transform.position;
            /*Vector3 position = transform.position;
            _velocity = (position - _prevPosition) / Time.deltaTime;

            // 前フレーム位置を更新
            _prevPosition = position;*/
        }

        void FixedUpdate()
        {
            //_nowPosition = transform.position;
            //AddVelocity();
            //MovePlatform();
            //MoveWith();
            //_prevPosition = transform.position;
        }

        void MovePlatform()
        {
            rigidBody.MovePosition(transform.position + Time.fixedDeltaTime * speed);
        }

        void OnTriggerEnter(Collider other)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
            {
                NoApproachField = other.gameObject.transform.position;
                return;
            }
            Rigidbody rd = other.gameObject.GetComponent<Rigidbody>();
            if (rd == null) return;
            rigidBodies.Add(rd);
            collisionObject.Add(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
            {
                NoApproachField = Vector3.zero;
                return;
            }
            rigidBodies.Remove(other.gameObject.GetComponent<Rigidbody>());
            collisionObject.Remove(other.gameObject);
        }

        void AddVelocity()
        {

            // 現在速度計算
            //Vector3 velocity = (_nowPosition - _prevPosition) / Time.fixedDeltaTime;
            //Vector3 velocity = rigidBody.GetPointVelocity(Vector3.zero);
            //Vector3 velocity = rigidBody.velocity;
            //Debug.Log("Velocity: " + velocity);
            if (rigidBody.velocity.sqrMagnitude <= 0.01f)
            {
                return;
            }

            for (int i = 0; i < rigidBodies.Count; i++)
            {
                //rigidBodies[i].velocity = Vector3.zero;
                //rigidBodies[i].velocity += velocity * Time.deltaTime;
                rigidBodies[i].AddForce(rigidBody.velocity, ForceMode.VelocityChange);
                //rigidBodies[i].position = rigidBodies[i].position + velocity * Time.deltaTime;
                //rigidBodies[i].MovePosition(rigidBodies[i].position + rigidBody.velocity * Time.fixedDeltaTime);
                //rigidBodies[i].MovePosition(rigidBody.position + new Vector3(0f, transform.localScale.y * 0.5f, 0f));
                
            }
            
        }

        void MoveWith()
        {
            //Move
            Vector3 deltaMove = (_nowPosition - _prevPosition);
            for (int i = 0; i < collisionObject.Count; i++)
            {
                Debug.Log(collisionObject[i].name);
                Vector3 player_pastPos = collisionObject[i].transform.position;
                collisionObject[i].transform.position += deltaMove;
                //Y軸上方向へは床に押される力で上がるので、ここでは移動させない
                if (collisionObject[i].transform.position.y - player_pastPos.y > 0)
                {
                    collisionObject[i].transform.position = new Vector3(collisionObject[i].transform.position.x, player_pastPos.y, collisionObject[i].transform.position.z);
                }
            }
        }
    }
}
