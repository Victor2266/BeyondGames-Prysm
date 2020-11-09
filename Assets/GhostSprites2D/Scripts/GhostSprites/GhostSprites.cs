using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Author: Marco Samaritoni
/// Release Version: 2.1.1.8
/// This script creates ghost images from the previous locations
/// of a srpite.  Depending on the trail size setting, it will create n
/// "ghosts" corresponding to n Positions(Vector3)
/// 
/// In the Unity Editor, use the Color picker to specify the transparancy and color of your 
/// ghosts; while high settings of transparancy will yield what will
/// look like duplicates of the SpriteRenderer, lower transparancy 
/// settings will produce the a more Ghost like effect.
/// 
/// Reccommended settings are:
/// 
/// Color: 255,255,255, 50
/// Trail Size: 10 to 50
/// 
/// Watch the instructional videos to get a full understanding of what
/// you can do with this component:
/// 
/// Demo video: https://www.youtube.com/watch?v=exwExUHX4YA
/// Instructional video #1: https://www.youtube.com/watch?v=NWLVsCeSYBw
/// Instructional video #2: https://www.youtube.com/watch?v=xx5VYiIlunU
/// </summary>

[RequireComponent(typeof(SpriteRenderer))]
public class GhostSprites : MonoBehaviour
{
	#region public members
	
	[Range(2, 200)]
	[Tooltip("Number of Ghost frames in the Sprite's trail")]
	public int trailSize = 20;
	
	[Tooltip("Spacing increments on a frame basis - n spacing setting results in waiting n frames before the next ghost will be set.")]
	[Range(0, 20)]
	public int spacing = 0;
	
	[Tooltip("Pick a color for the trail.  The Aplha of this color will be used to determine the transparancy fluctuation.")]
	public Color color = new Color (255,255,255,100);
	
	[Tooltip("If the Ghost Material is not set, then the material from the GameObject's SpriteRenderer will be used for the ghosts.")]
	[ContextMenuItem("Clear List", "DeleteMaterials")]
	public List<Material> ghostMaterial;
	
	[Tooltip("Integers below the corresponding Sprite will allow different color ghosting - while integers that are above will turn the corresponding Sprite to the configured color value.")]
	public int ghostSpriteSortingOrder;
	
	[Tooltip("If checked, and a RigidBody2D is attached to this gameobject, then ghosts will only render when this gameobject is in motion.")]
	public bool renderOnMotion;
	
	[Tooltip("Set this to true to use the Aplha Fluctuation Override value.  See tooltip for Alpha Fluctuation Override.")]
	public bool colorAlphaOverride;
	
	[Tooltip("If the Color Alpha Override bool is set to true, this value (up to 255) will be used instead of the automagically set alpha fluctuation for the ghosts.")]
	[Range(0,1)]
	public float alphaFluctuationOverride;
	
	[Range(0,250)]
	public int alphaFluctuationDivisor;
	
	#endregion
	
	#region private members
	
	private int spacingCounter = 0;
	private SpriteRenderer character;								
	private List<Sprite> spriteList = new List<Sprite>();
	private List<Vector3> spritePositions = new List<Vector3>();	
	private List<GameObject> ghostList = new List<GameObject>();
	private bool hasRigidBody2D;
	private int zAnchor;
	private float alpha;
    private bool killSwitch;
	
	#endregion
	
	void Start ()
	{
		// Reference the GameObject's SpriteRenderer for the trail list
		if(this.gameObject != null)
			character = gameObject.GetComponent<SpriteRenderer>();
		
		ghostMaterial = TruncateMaterialList(ghostMaterial);
		
		// If not materials are assigned, use the SpriteRenderer's current material
		if(ghostMaterial.Count == 0)
			ghostMaterial.Add (character.GetComponent<SpriteRenderer>().material);
		
		Vector3 position = gameObject.transform.position;
		
		for (int i = 0; i < trailSize; i++) 
			Populate(position, true);
		
		alpha = color.a;
		
		if(spacing < 0)
			spacing = 0;
		
		
		zAnchor = this.gameObject.transform.position.z > Camera.main.transform.position.z ? 1 : -1;
		
		hasRigidBody2D = this.gameObject.GetComponent<Rigidbody2D>() != null ? true : false;
		
		ghostMaterial.Reverse();
		
	}
	
	#region public methods

	public void ClearTrail(){
		trailSize = 2;
        foreach (Sprite s in spriteList)
            GameObject.Destroy(s);
        spriteList.Clear();
		spritePositions.Clear();

	}

    private void KillSwitchEngage()
    {
        killSwitch = true;
        foreach (GameObject g in ghostList)
            GameObject.Destroy(g);
    }

    void OnDestroy()
    {
        killSwitch = true;
        KillSwitchEngage();
    }

    public void AddToMaterialList(Material material){
		
		ghostMaterial.Add (material);
	}

#if UNITY_EDITOR
	public void RestoreDefaults(){
		Undo.RecordObject(gameObject.GetComponent<GhostSprites>(),"Restore Defaults");
		ghostMaterial.Clear();
		trailSize = 20;
		color = new Color (255,255,255,50);
		spacing = 0;
		ghostSpriteSortingOrder = 0;
		renderOnMotion = false;
		colorAlphaOverride = false;
		alphaFluctuationOverride = 0;
		alphaFluctuationDivisor = 1;
		
	}
#endif
	#endregion
	
	void Update ()
	{
        if (killSwitch)
        {
            return;
        }

		if(ghostMaterial.Count == 0)
			ghostMaterial.Add (gameObject.GetComponent<SpriteRenderer>().material);
		
		if(trailSize < ghostList.Count){
			for(int i = trailSize; i < ghostList.Count -1; i++){
				GameObject gone = ghostList[i];
				GameObject.Destroy(gone);
				ghostList.RemoveAt(i);
			}
			
		}
		zAnchor = this.gameObject.transform.position.z > Camera.main.transform.position.z ? 1 : -1;
		if(spacingCounter < spacing){
			spacingCounter++;
			return;
		}
		Vector3 position = gameObject.transform.position;
		
		if(ghostList.Count < trailSize){
			
			Populate(position, false);		
		}
		else{
			GameObject gone = ghostList[0];
			ghostList.RemoveAt(0);
			GameObject.Destroy(gone);
			Populate(position, false);
		}
		float temp;
		if(colorAlphaOverride)
			temp = alphaFluctuationOverride;
		else
		{
			temp = alpha;
		}
		int materialDivisor = ( ghostList.Count -1 ) / ghostMaterial.Count + 1;
		for(int i = ghostList.Count - 1 ; i >= 0; i--){
			
			temp -= colorAlphaOverride && alphaFluctuationDivisor != 0 ? 
				alphaFluctuationOverride / alphaFluctuationDivisor : alpha / ghostList.Count;
			color.a = temp;
			SpriteRenderer sprite = ghostList[i].GetComponent<SpriteRenderer>();
			sprite.color = color;
			int subMat = (int)Mathf.Floor(i / materialDivisor);
			sprite.material = subMat <= 0 ? ghostMaterial[0] : ghostMaterial[subMat];
			ghostList[i].transform.position = new Vector3(ghostList[i].transform.position.x,
			                                              ghostList[i].transform.position.y,
			                                              i * 0.0001f);
			
		}
		spacingCounter = 0;
	}
	
	#region private methods
	
	private void Populate(Vector3 position, bool allowPositionOverride)
	{
		if(RenderOnMotion 
		   && hasRigidBody2D 
		   && gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero 
		   && !allowPositionOverride)
		{
			if(ghostList.Count > 0){
				GameObject gone = ghostList[0];
				ghostList.RemoveAt(0);
				GameObject.Destroy(gone);
			}
			return;
		}
		GameObject g = new GameObject();
		g.name = gameObject.name + " - GhostSprite";
		g.AddComponent<SpriteRenderer>();
		g.transform.position = position;
		g.transform.localScale = gameObject.transform.localScale;
		g.transform.rotation = gameObject.transform.rotation;
		g.GetComponent<SpriteRenderer>().sprite = character.sprite;
		g.GetComponent<SpriteRenderer> ().sortingOrder = GhostSpriteSortingOrder;
		ghostList.Add (g);
	}
	
	private List<Material> TruncateMaterialList(List<Material> materialList)
	{
		List<Material> tempList = new List<Material>();
		foreach(Material material in materialList)
		{
			if(material)
				tempList.Add (material);
		}
		return tempList;			
	}
	
	private void DeleteMaterials()
	{
		ghostMaterial.Clear();
	}
	
	#endregion
	
	#region public getters/mutators 
	
	public int ZAnchor {
		get {
			return zAnchor;
		}
		set {
			if(value == 1 || value == -1)
				zAnchor = value;
		}
	}
	
	public int GhostSpriteSortingOrder {
		get {
			return ghostSpriteSortingOrder;
		}
		set{
			ghostSpriteSortingOrder = value;
		}
	}
	
	public bool RenderOnMotion {
		get {
			return renderOnMotion;
		}
		set{
			renderOnMotion = value;
		}
	}
	
	public int TrailSize {
		get {
			return trailSize;
		}
		set{
			if(value >= 2)
				trailSize = value;
		}
	}
	
	public int Spacing {
		get {
			return spacing;
		}
		set{
			if(value >= 0 && value <= 20)
				spacing = value;
		}
	}
	
	#endregion
}

