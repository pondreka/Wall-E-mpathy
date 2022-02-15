using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Hand = UnityEngine.XR.Hand;

namespace Valve.VR.InteractionSystem.Sample
{
    

    [RequireComponent( typeof( Interactable))]
    public class circle_interactable_script : MonoBehaviour
    {
        // Start is called before the first frame update
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        private Interactable interactable;
        public GameObject spawnPosition;
        public GameObject circlePrefab;
        private GameObject repairCircle;
        private GameObject repairCircleParent;
        private bool repairSuccessfull=false;
        void Awake()
        {
            repairCircleParent = new GameObject("repairCircleParent");
            repairCircleParent.transform.position = spawnPosition.transform.position;
            repairCircleParent.transform.parent = this.gameObject.transform;
            interactable = this.GetComponent<Interactable>();
            repairCircle = Instantiate(circlePrefab, spawnPosition.transform.position, Quaternion.identity);
            repairCircle.transform.parent = repairCircleParent.transform;
            repairCircle.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
            repairCircle.GetComponentInChildren<Transform>().name = "Repair";
            repairCircle.transform.GetChild(0).name = "white";
            repairCircle.transform.GetChild(1).name = "green";
        }

        public void OnTriggerEnter(Collider collision)
        {
            
            if (collision.gameObject.CompareTag("needToFix"))
            {
                print("Collision with broken black circle");
                var lineRenderer = collision.gameObject.GetComponentInChildren<Renderer>();
                lineRenderer.material.SetColor("_Color", Color.white);
                
                
                repairCircle.transform.position = spawnPosition.transform.position;
                collision.gameObject.tag = "fixed";
                collision.gameObject.transform.parent.transform.parent = 
                    collision.gameObject.transform.parent.transform.parent.transform.parent.GetChild(1).transform;

                repairSuccessfull = true;
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

                // TODO 
                // If the object does not land on needed position, replace it with start position
                // else lock it onto wanted position - not interactable anymore
                // spawn new object at start position

                repairSuccessfull = false;

            }
            



        }
       

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}