using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Hand = UnityEngine.XR.Hand;

namespace Valve.VR.InteractionSystem.Sample
{
    

    [RequireComponent( typeof( Interactable))]
    public class interactable_script : MonoBehaviour
    {
        // Start is called before the first frame update

        // dont need next to if the position should be new (unless the person fails and we want to set it back?)
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        public Transform fixedParent;
        private Interactable interactable;
        private Interactable new_interactable;
        private Vector3 spawnPosition = new Vector3(12, 0, 5);
        public bool brokenPosition = false;
        public GameObject circle;
        private GameObject circle1;
        public bool canBeMoved = true;
        public Transform fixingCircleParent;
        private bool repairSuccessfull=false;
        void Awake()
        {
            interactable = this.GetComponent<Interactable>();
            circle1 = Instantiate(circle, spawnPosition, Quaternion.identity);
            circle1.transform.parent = fixingCircleParent.transform;
            circle1.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
        }

        public void OnTriggerEnter(Collider collision)
        {
            print("Hit something");
            if (collision.gameObject.tag == "needToFix")
            {
                print("Collision with to fix element");
                var lineRenderer = collision.gameObject.GetComponentInChildren<Renderer>();
                lineRenderer.material.SetColor("_Color", Color.white);
                repairSuccessfull = true;
                Destroy(circle1);
                circle1 = Instantiate(circle, spawnPosition, Quaternion.identity);
                circle1.transform.parent = fixingCircleParent.transform;
                circle1.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
                collision.gameObject.tag = "fixed";
                collision.gameObject.transform.parent.gameObject.transform.parent = fixedParent.transform;

                repairSuccessfull = true;
            }
        }
        
        private void HandHoverUpdate(Hand hand)
        {
            if (canBeMoved)
            { 
                GrabTypes startingGrabType = hand.GetGrabStarting();
                bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

                if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
                {
                    oldPosition = transform.position;
                    oldRotation = transform.rotation;

                    hand.HoverLock(interactable);
                    hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
                }
                else if (isGrabEnding || repairSuccessfull)
                {

                    hand.DetachObject(gameObject);
                    hand.HoverUnlock(interactable);

                    // TODO 
                    // If the object does not land on needed position, replace it with start position
                    // else lock it onto wanted position - not interactable anymore
                    // spawn new object at start position

                    repairSuccessfull = false;

                }
            }



        }
        void Start()
        {
          
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}