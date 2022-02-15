using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Hand = UnityEngine.XR.Hand;

namespace Valve.VR.InteractionSystem.Sample
{
    

    [RequireComponent( typeof( Interactable))]
    public class line_interactable_script : MonoBehaviour
    {
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        private Interactable interactable;
        public GameObject spawnPosition;
        public GameObject repairLinePrefab;
        private GameObject repairLine;
        private GameObject repairLineParent;
        private bool repairSuccessfull=false;
        void Awake()
        {
            repairLineParent = new GameObject("fixingLineParent");
            repairLineParent.transform.position = spawnPosition.transform.position;
            repairLineParent.transform.parent = this.gameObject.transform;
            interactable = this.GetComponent<Interactable>();
            repairLine = Instantiate(repairLinePrefab, spawnPosition.transform.position, Quaternion.Euler(-20,0,-110));
            repairLine.transform.parent = repairLineParent.transform;
            repairLine.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
        }

        public void OnTriggerEnter(Collider collision)
        {
            
            if (collision.gameObject.CompareTag("needToDraw"))
            {
                print("Collision with broken black line");
                var lineRenderer = collision.gameObject.GetComponentInChildren<Renderer>();
                lineRenderer.material.SetColor("_Color", Color.white);
                collision.gameObject.tag = "fixed";
                collision.gameObject.transform.parent.transform.parent =
                    collision.gameObject.transform.parent.transform.parent.transform.parent.GetChild(2).transform;

            }
        }
        
        private void HandHoverUpdate(Hand hand)
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

          
                repairSuccessfull = false;

            }



        }
        void Start()
        {
          
        }

        void Update()
        {
            
        }
    }
}