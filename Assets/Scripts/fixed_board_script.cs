using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Valve.VR.InteractionSystem;
using Hand = UnityEngine.XR.Hand;

namespace  Valve.VR.InteractionSystem.Sample
{
    
    [RequireComponent( typeof( Interactable))]

    public class fixed_board_script : MonoBehaviour
    {
        
        private bool boardFixed = false;
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        private Interactable interactable;
        public bool deleteCollider=true;
        
        

        private void Awake()
        {
            interactable = this.GetComponent<Interactable>();
            //this.gameObject.transform.parent = this.gameObject.transform.parent;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (GameObject.Find("needToFixParent") != null)
            {
                //print("This gameobject: " + this.gameObject.name );
                //print("Noumber of childs: " + this.gameObject.transform.GetChild(4).childCount);
                if (this.gameObject.transform.GetChild(3).childCount== 0 && boardFixed==false)
                {
                    print("Board is fixed!!!");
                    boardFixed = true;
                    if (deleteCollider)
                    {
                        DeleteCollider();

                    }
                }
            }
        }


        private void DeleteCollider()
        {

            print("Deleting colliders: of circles");

            GameObject circlesParent = this.gameObject.transform.GetChild(1).gameObject;
            foreach (Transform circle in circlesParent.transform)
            {
                GameObject whiteCircle = circle.GetChild(0).gameObject;
                GameObject greencircle = circle.GetChild(1).gameObject;
                Destroy(whiteCircle.GetComponent<CapsuleCollider>());
                Destroy(greencircle.GetComponent<CapsuleCollider>());

            }
            
            print("Deleting colliders of lines(cubes)");
            GameObject linesParent = this.gameObject.transform.GetChild(2).gameObject;
            foreach (Transform line in linesParent.transform)
            {
                GameObject cube = line.GetChild(0).gameObject;
                Destroy(cube.GetComponent<BoxCollider>());
            }
          
        
        }
        public void OnTriggerEnter(Collider collision)
        {
            
            
        }
        
        
        private void HandHoverUpdate(Hand hand)
        {

            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None && boardFixed==true)
            {
                oldPosition = transform.position;
                oldRotation = transform.rotation;

                hand.HoverLock(interactable);
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
            else if (isGrabEnding)
            {

                hand.DetachObject(gameObject);
                hand.HoverUnlock(interactable);




            }
        }
    }
}