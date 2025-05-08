using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.Scened
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField]
        private float speed = 3f;
        
        [SerializeField]
        private float jumpForce = 9f;
        
        [SerializeField]
        private bool _clientAuth = true;
        [SerializeField]
        private CharacterController _controller;
        
        private GameObject _camera;
        private float _gravity = -13f;
        private float _yVelocity=0f;
        private void Awake()
        {
            Debug.Log(transform.position);
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!base.IsOwner)
            {
                // If not owner then disable this script.
                this.enabled = false;
                return;
            }
            
        }

        private void Update()
        {
            if (base.IsOwner)
            {
                _camera = Camera.main.gameObject;
            }

            if (_camera == null)
            {
                return;
            }
            
            _camera.transform.position = transform.position + new Vector3(0f, 3f, -6f);
            
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");

            /* If ground cannot be found for 20 units then bump up 3 units. 
             * This is just to keep player on ground if they fall through
             * when changing scenes.             */
            if (_clientAuth || (!_clientAuth && base.IsServerStarted))
            {
                if (!Physics.Linecast(transform.position + new Vector3(0f, 0.3f, 0f), transform.position - (Vector3.one * 20f)))
                    transform.position += new Vector3(0f, 3f, 0f);
            }

            if (_clientAuth){
                Move(hor, ver);
            }
            else{
                ServerMove(hor, ver);
            }
        }

        [ServerRpc]
        private void ServerMove(float hor, float ver)
        {
            Move(hor, ver);
        }

        private void Move(float hor, float ver)
        {
            Debug.Log($"IsGrounded: {_controller.isGrounded}");
            //If ray hits floor then cancel gravity.
            if (_controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _yVelocity = jumpForce;
                }
                else
                {
                    _yVelocity = 0f;
                }
                
            }
            else
            {
                _yVelocity += _gravity * Time.deltaTime;
            }

            
            Vector3 moveVector = new Vector3(hor * speed, _yVelocity, ver * speed) * Time.deltaTime;
            _controller.Move(moveVector);

        }
    }
}