using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TemplateController : MonoBehaviour
{
    // PUBLICS
    public bool mNoSpaceForTemplate = false;

    // SHOWN IN INSPECTOR
    [SerializeField] private Transform mNewBuildingBlock;
    [SerializeField] private TMP_Text mCoinCostText;

    // PRIVATE OBJECTS	
    private Dropdown mBuildingBlockSelector;

    // PRIVATE COMPONENTS
    private BuildingBlockManager mBuildingBlockManager;
    private CoinManager mCoinManager;
    private List<SpriteRenderer> mPieceSpriteRenderers = new List<SpriteRenderer>();
    private List<MonoBehaviour> mPieceScriptComponents = new List<MonoBehaviour>();
    

    // PRIVATE 
    private Vector2 mMousePosition;
    private float mPingTimeTracker;
    private int mTotalCost;
    private bool mFire1InUse;



    /*
    -> Initialize references
    -> Initialize template
 */
    
    private void Start()
    {
        // UI dropdown used for selecting building blocks
        if (GameObject.Find("Building Block Selector") != null)
        {
            mBuildingBlockSelector = GameObject.Find("Building Block Selector").GetComponent<Dropdown>();
        }

        if (GameObject.Find("Persistent Data") != null)
        {
            mBuildingBlockManager = GameObject.Find("Persistent Data").GetComponent<BuildingBlockManager>();
            mCoinManager = GameObject.Find("Persistent Data").GetComponent<CoinManager>();
        }

        updateBuildingBlockSelector();
        changeTemplate();
    }

    private void Update()
    {
        if (mPieceSpriteRenderers.Count != 0)
        {
            moveTemplate();
            standAloneRotate();
            standAloneConstructFromTemplate();
            pingAlpha();
        }
    }

    /*
		-> Get building block by looking at the value chosen
			from the dropdown menu. 
		-> Populate the template with a new building block. 
		-> For each piece in the building block, create an 
			equivalent in template with the correct position, 
			rotation, and tag.
	 */


    public void changeTemplate()
    {
        int lNewBuildingBlockIndex = mBuildingBlockSelector.value;
        bool lBuildingBlockExists = mBuildingBlockManager.Positions[lNewBuildingBlockIndex] != null;
        changeTemplate(lNewBuildingBlockIndex, lBuildingBlockExists, mBuildingBlockManager, this.transform.position);
    }

    private void changeTemplate(int pNewBuildingBlockIndex, bool pBuildingBlockExists, BuildingBlockManager pBuildingBlockManager, Vector3 pTemplatePosition)
    {
        clearTemplate();

        if (pBuildingBlockExists)
        {
            for (int i = 0; i < pBuildingBlockManager.Positions[pNewBuildingBlockIndex].Count; i++)
            {
                Vector3 lPiecePosition = pBuildingBlockManager.Positions[pNewBuildingBlockIndex][i] + transform.position;
                Quaternion lPieceRotation = pBuildingBlockManager.Rotations[pNewBuildingBlockIndex][i];
                string lPieceTag = pBuildingBlockManager.Tags[pNewBuildingBlockIndex][i];

                // create each piece using one of a preset list of prefabs
                placePieceInTemplate(lPieceTag, lPiecePosition, lPieceRotation);             
            }            
        }
        

        initialize();
    }

    /*
        -> Shut down all scripts on children of template 
			except those specified
        -> Set sorting order and color of all sprite renderers
            of all children of the template. 
     */

    private void initialize()
    {
        initialize(mPieceScriptComponents, mPieceSpriteRenderers);
    }

    private void initialize(List<MonoBehaviour> pPieceScripts, List<SpriteRenderer> pPieceSpriteRenderers)
    {
        disablePieceScripts(pPieceScripts);
        initializePieceSpriteRenderers(pPieceSpriteRenderers);       
    }

    private void disablePieceScripts(List<MonoBehaviour> pPieceScripts)
    {
        pPieceScripts.AddRange(this.gameObject.GetComponentsInChildren<MonoBehaviour>());
        if (pPieceScripts != null)
        {
            foreach (MonoBehaviour lPieceScript in pPieceScripts)
            {
                if (!(lPieceScript is CostManager || lPieceScript is TemplateController))
                {
                    lPieceScript.enabled = false;
                }
            }
        }
    }

    private void initializePieceSpriteRenderers(List<SpriteRenderer> pPieceSpriteRenderers)
    {
        //pPieceSpriteRenderers.AddRange(this.gameObject.GetComponentsInChildren<SpriteRenderer>());
        //pPieceSpriteRenderers.RemoveAt(0);

        if (pPieceSpriteRenderers != null)
        {
            Color TEMPLATE_PIECE_COLOR = new Color(.1f, .1f, .1f, .5f);
            foreach (SpriteRenderer lPieceSpriteRenderer in pPieceSpriteRenderers)
            {
                lPieceSpriteRenderer.sortingOrder = 1;
                lPieceSpriteRenderer.color = TEMPLATE_PIECE_COLOR;
            }
        }
    }

    /*
        -> Empty lists that depend on children of template
        -> Remove all children from the template. 
     */

    private void clearTemplate()
    {
        clearTemplate(mPieceSpriteRenderers, mPieceScriptComponents, this.transform);
    }

    private void clearTemplate(List<SpriteRenderer> pPieceSpriteRenderers, List<MonoBehaviour> pPieceScriptComponents, Transform pTemplate)
    {
        mPieceScriptComponents.Clear();
        mPieceSpriteRenderers.Clear();

        foreach (Transform piece in pTemplate)
        {
            Destroy(piece.gameObject);
        }
    }



    /*
		-> Make template follow mouse in discrete jumps.
			4 units in the x and 4 units in the y. 
		-> TODO: Get rid of magic number (4). Should be 
			a const.
	*/


    private void moveTemplate()
    {
#if UNITY_STANDALONE
        Vector2 lMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveTemplate(lMousePosition);
#else 
        this.transform.position = new Vector3(Mathf.Round(Camera.main.transform.position.x) + 2f, Mathf.Round(Camera.main.transform.position.y) + 1f, 0f);
#endif
    }

    private void moveTemplate(Vector2 pMousePosition)
    {
        this.transform.position = new Vector2(Mathf.Round(pMousePosition.x), Mathf.Round(pMousePosition.y));
    }

    /**
		-> Change alpha of all children sprites of the template
			according to how much time has elapsed since the start
			of the game. 
		-> NOTE: The pinging effect is achieved using a sine wave. 
	*/

    private void pingAlpha()
    {
        pingAlpha(mPieceSpriteRenderers);
    }

    private void pingAlpha(List<SpriteRenderer> pPieceSpriteRenderers)
    {
        float lPingTimeTracker = getAlpha();
        
        if (pPieceSpriteRenderers.Count > 0)
        {
            foreach (SpriteRenderer lPieceSpriteRenderer in pPieceSpriteRenderers)
            {
                Color currentColor = new Color(lPieceSpriteRenderer.color.r, lPieceSpriteRenderer.color.g, lPieceSpriteRenderer.color.b, lPingTimeTracker);
                lPieceSpriteRenderer.color = currentColor;
            }
        }
    }

    private float getAlpha()
    {
        const float FREQUENCY_MODIFIER = 8f;
        const float AMPLITUDE = .4f;
        const float VERTICAL_SHIFT = .6f;

        float lPingTimeTracker = VERTICAL_SHIFT + Mathf.Sin(Time.time * FREQUENCY_MODIFIER) * AMPLITUDE;
        return lPingTimeTracker;
    }

    private string logSpriteRenderers()
    {
        string log = "";

        foreach (SpriteRenderer s in mPieceSpriteRenderers)
        {
            log += s.gameObject.name + ", ";
        }

        return log;
    }

    // Rotate the template when right click
    public void rotate()
    {
        const float XY_ROTATION_AMOUNT = 0f;
        const float Z_ROTATION_AMOUNT = 90f;

        rotate(true, new Vector3(XY_ROTATION_AMOUNT, XY_ROTATION_AMOUNT, Z_ROTATION_AMOUNT));
    }

    private void standAloneRotate()
    {
        const float XY_ROTATION_AMOUNT = 0f;
        const float Z_ROTATION_AMOUNT = 90f;

#if UNITY_STANDALONE
        rotate(Input.GetKeyDown(KeyCode.Mouse1), new Vector3(XY_ROTATION_AMOUNT, XY_ROTATION_AMOUNT, Z_ROTATION_AMOUNT));
#endif
    }

    private void rotate(bool pRotationTriggered, Vector3 pRotationVector)
    {
        const int HORIZONTAL_CHECK = 180;
        const double EPSILON = 1E-10;
        const string LONG_WALL_TAG = "LongWall";
        const string SHORT_WALL_TAG = "ShortWall";

        if (pRotationTriggered)
        {
            transform.Rotate(pRotationVector);
            foreach (Transform child in this.transform)
            {

                if (Mathf.Abs(child.transform.eulerAngles.z % HORIZONTAL_CHECK) <= EPSILON)
                {
                    child.transform.localEulerAngles = -this.transform.localEulerAngles;
                }
                else
                {
                    child.transform.localEulerAngles = -this.transform.localEulerAngles + pRotationVector;
                }

                if (child.tag == LONG_WALL_TAG || child.tag == SHORT_WALL_TAG)
                {
                    child.GetComponent<WallManager>().setImage();
                }
            }
        }
    }


    public void constructFromTemplate() {
        Debug.Log("Should construct");
        constructFromTemplate(1, mFire1InUse, new Collider2D[900], new ContactFilter2D(), mNewBuildingBlock);
    }
    // If the mouse is not over a ui element, and if the template is not colliding with objects already in the scene
    // When left click, paste the template into the scene.
    // If the template is colliding with objects, when left clics, erase those objects
    private void standAloneConstructFromTemplate()
    {
        // Windows
#if UNITY_STANDALONE
            constructFromTemplate(Input.GetAxisRaw("Fire1"), mFire1InUse, new Collider2D[900], new ContactFilter2D(), mNewBuildingBlock);
#endif

    }

    private void constructFromTemplate(float pFire1, bool pFire1InUse, Collider2D[] pPieceColliders, ContactFilter2D pContactFilter, Transform pNewBuildingBlock)
    {
        // Get the number of colliders that the template overlaps
        // I've set the maximum number of pieces in a building block 
        // to 150. The largest piece is 3x1. 
        int lNumColliders = Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), pContactFilter, pPieceColliders);
        bool lNoSpaceForTemplate;

        // If the template overlaps with at least 1 collider, then 
        // there is no room to construct from template
        if (lNumColliders > 0)
        {
            lNoSpaceForTemplate = true;
        }
        else
        {
            lNoSpaceForTemplate = false;
        }

        bool pFiring = pFire1 != 0;
        bool lNoStandAloneProblems = true;

#if UNITY_STANDALONE
        lNoStandAloneProblems = !mFire1InUse && !checkPointerOverUI();
#endif
        


        // If you left click, and you are not hovering over a UI element, 
        if (pFiring && lNoStandAloneProblems)
        {
            if (!lNoSpaceForTemplate)
            {
                
                Color white = new Color(1f, 1f, 1f, 1f);
                const int PIECE_SORTING_ORDER = 0;

                // Create piece and turn on all monobehaviours. 
                foreach (Transform child in this.transform)
                {
                    GameObject clone = (GameObject)Instantiate(child.gameObject, child.position, child.rotation);
                    clone.transform.parent = pNewBuildingBlock;
                    clone.GetComponent<SpriteRenderer>().color = white;
                    clone.GetComponent<SpriteRenderer>().sortingOrder = PIECE_SORTING_ORDER;

                    foreach(MonoBehaviour m in clone.GetComponents<MonoBehaviour>())
                    {
                        m.enabled = true;
                    }                    
                }

            }
            else
            {
                foreach (Collider2D c in pPieceColliders)
                {
                    const string CONTAINER_TAG = "Container";
                    if (c != null && c.gameObject.tag != CONTAINER_TAG)
                    {
                        DestroyImmediate(c.gameObject);
                    }
                }
            }
            
            CalcTotalConstructCost();
            //mFire1InUse = true;
        }

        //amFire1InUse = false;

#if UNITY_STANDALONE
        mFire1InUse = pFire1 == 0 ? false : true;
#endif
    }

    // Check if mouse is over a ui element
    private bool checkPointerOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void CalcTotalConstructCost()
    {
        Color32 NOT_ENOUGH_COINS = new Color32(0xFF, 0x6D, 0x6C, 0xFF);
        Color32 ENOUGH_COINS = new Color32(0x1E,0xA5,0xFF, 0xFF);

        mTotalCost = 0;

        foreach (Transform t in mNewBuildingBlock)
        {
            mTotalCost += t.GetComponent<CostManager>().Cost;
        }

        mCoinCostText.text = string.Format("{0:n0}", mTotalCost); 

        if (mTotalCost > mCoinManager.Coins)
        {
            mCoinCostText.color = NOT_ENOUGH_COINS;
        }
        else
        {
            mCoinCostText.color = ENOUGH_COINS;
        }
    }
    
    /*		
        Use the tag to select and instantiate a built-in 
        prefab to create and child to the template. 
    */
    private void placePieceInTemplate(string tag, Vector3 position, Quaternion rotation)
    {
        foreach (Transform builtInPrefab in mBuildingBlockManager.BuiltInPrefabs)
        {
            // find the right prefab to use
            if (tag == builtInPrefab.tag)
            {
                // create the piece, set the right position, rotation, and name, and make it 
                // a child of the template.
                
                Transform piece = (Transform)Instantiate(builtInPrefab, position, rotation);
                piece.parent = this.transform;
                piece.name = builtInPrefab.name;
                mPieceSpriteRenderers.Add(piece.GetComponent<SpriteRenderer>());
            }
        }
    }

    public void updateBuildingBlockSelector()
    {
        mBuildingBlockSelector.ClearOptions();
        mBuildingBlockSelector.AddOptions(mBuildingBlockManager.Names);
    }

}
