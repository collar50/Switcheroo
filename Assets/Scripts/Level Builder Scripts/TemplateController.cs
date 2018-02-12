using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TemplateController : MonoBehaviour
{
    // PUBLICS
    public bool noSpaceForTemplate = false;

    // SHOWN IN INSPECTOR
    [SerializeField] private Transform currentConstruction;
    [SerializeField] private Text costIndicator;

    // PRIVATE OBJECTS	
    private Dropdown bbSelector;

    // PRIVATE COMPONENTS
    private BuildingBlock_Storage bbStorage;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<MonoBehaviour> scriptComponents = new List<MonoBehaviour>();

    // PRIVATE 
    private Vector2 mousePosition;
    private float pingTimeTracker;
    private int totalCost;

    /*
		-> Get building block by looking at the value chosen
			from the dropdown menu. 
		-> Populate the template with a new building block. 
		-> For each piece in the building block, create an 
			equivalent in template with the correct position, 
			rotation, and tag.
	 */
    public void ChangeTemplate()
    {
        Clear();

        int bbIndex = bbSelector.value;
        bool bbExists = bbStorage.Positions[bbIndex] != null;
        if (bbExists)
        {
            for (int i = 0; i < bbStorage.Positions[bbIndex].Count; i++)
            {
                Vector3 position = bbStorage.Positions[bbIndex][i]
                                   + transform.position;
                Quaternion rotation = bbStorage.Rotations[bbIndex][i];
                string tag = bbStorage.Tags[bbIndex][i];

                // create each piece using one of a preset list of prefabs
                PlacePrefabInTemplate(tag, position, rotation);
            }
        }

        //   - bbStorage.Positions[bbIndex][0]

        Initialize();
    }

    /*
        -> Shut down all scripts on children of template 
			except those specified
        -> Set sorting order and color of all sprite renderers
            of all children of the template. 
     */
    private void Initialize()
    {
        scriptComponents.AddRange(this.gameObject.GetComponentsInChildren<MonoBehaviour>());
        if (scriptComponents != null)
        {
            foreach (MonoBehaviour m in scriptComponents)
            {
                Debug.Log(m);
                if (!(m is CostManager || m is TemplateController))
                    m.enabled = false;
            }
        }

        spriteRenderers.AddRange(this.gameObject.GetComponentsInChildren<SpriteRenderer>());
        if (spriteRenderers != null)
        {
            foreach (SpriteRenderer s in spriteRenderers)
            {
                s.sortingOrder = 1;
                s.color = new Color(0, 0, 255, .5f);
            }
        }
    }

    /*
        -> Empty lists that depend on children of template
        -> Remove all children from the template. 
     */
    private void Clear()
    {
        scriptComponents.Clear();
        spriteRenderers.Clear();
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /*
		-> Initialize references
		-> Initialize template
	 */
    private void Start()
    {
        // UI dropdown used for selecting building blocks
        if (GameObject.Find("Building Block Selector") != null)
        {
            bbSelector = GameObject.Find("Building Block Selector").GetComponent<Dropdown>();
        }

        bbStorage = GameObject.Find("Persistent Data").GetComponent<BuildingBlock_Storage>();
        Initialize();
    }

    private void Update()
    {
        if (spriteRenderers.Count > 0)
        {
            Move();
            Rotate();
            //PingAlpha();
            ConstructFromTemplate();
        }
    }

    /*
		-> Make template follow mouse in discrete jumps.
			4 units in the x and 4 units in the y. 
		-> TODO: Get rid of magic number (4). Should be 
			a const.
	*/
    private void Move()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePosition.x / 4) * 4, Mathf.Round(mousePosition.y / 4) * 4);
    }

    /*
		-> Change alpha of all children sprites of the template
			according to how much time has elapsed since the start
			of the game. 
		-> NOTE: The pinging effect is achieved using a sine wave. 
	*/
    private void PingAlpha()
    {
        pingTimeTracker = .6f + Mathf.Sin(Time.time * 8f) * .4f;

        if (spriteRenderers[0] != null)
        {
            foreach (SpriteRenderer sp in spriteRenderers)
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, pingTimeTracker);
            }
        }
    }

    // Rotate the template when right click
    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            transform.Rotate(new Vector3(0f, 0f, 90f % 360));
        }
    }

    // If the mouse is not over a ui element, and if the template is not colliding with objects already in the scene
    // When left click, paste the template into the scene.
    // If the template is colliding with objects, when left click, erase those objects

    private void ConstructFromTemplate()
    {
        // Get the number of colliders that the template overlaps
        // I've set the maximum number of pieces in a building block 
        // to 150. The largest piece is 3x1. 
        Collider2D[] colliders = new Collider2D[900];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int numColliders = Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), contactFilter, colliders);

        // If the template overlaps with at least 1 collider, then 
        // there is no room to construct from template
        if (numColliders > 0)
        {
            noSpaceForTemplate = true;
        }
        else
        {
            noSpaceForTemplate = false;
        }

        // If you left click, and you are not hovering over a UI element, 
        if (Input.GetKeyDown(KeyCode.Mouse0) && !CheckPointerOverUI())
        {
            if (!noSpaceForTemplate)
            {
                foreach (Transform child in this.transform)
                {
                    GameObject clone = (GameObject)Instantiate(child.gameObject, child.position, child.rotation);
                    clone.transform.parent = GameObject.Find("Current Construction").transform;
                    clone.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    clone.GetComponent<SpriteRenderer>().sortingOrder = 0;

                    foreach(MonoBehaviour m in clone.GetComponents<MonoBehaviour>())
                    {
                        m.enabled = true;
                    }                    
                }
            }
            else
            {
                foreach (Collider2D c in colliders)
                {
                    if (c != null && c.gameObject.tag != "Container")
                    {
                        DestroyImmediate(c.gameObject);
                    }
                }
            }
        }
    }

    // Check if mouse is over a ui element
    private bool CheckPointerOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void CalcTotalConstructCost()
    {
        totalCost = 0;

        foreach (Transform t in currentConstruction)
        {
            totalCost += t.GetComponent<CostManager>().Cost;
        }

        costIndicator.text = totalCost.ToString();
    }
    /*		
    ->  Use the tag to select and instantiate a built-in 
        prefab to create and child to the template. 
 */
    private void PlacePrefabInTemplate(string tag, Vector3 position, Quaternion rotation)
    {
        foreach (Transform builtInPrefab in bbStorage.BuiltInPrefabs)
        {
            // find the right prefab to use
            if (tag == builtInPrefab.tag)
            {
                // create the piece, set the right position, rotation, and name, and make it 
                // a child of the template.
                Transform piece = (Transform)Instantiate(builtInPrefab, position, rotation);
                piece.parent = this.transform;
                piece.name = builtInPrefab.name;
            }
        }
    }

}
